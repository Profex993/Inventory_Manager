-- creating logins
CREATE ROLE inventory_user WITH LOGIN PASSWORD '1234';
GRANT USAGE ON SCHEMA public TO inventory_user;
GRANT SELECT, INSERT, UPDATE, DELETE ON parts, devices, boms TO inventory_user;
GRANT SELECT, INSERT ON part_logs, device_logs TO inventory_user;
GRANT USAGE, SELECT, UPDATE ON ALL SEQUENCES IN SCHEMA public TO inventory_user;
GRANT EXECUTE ON FUNCTION build_device(integer, integer) TO inventory_user;
GRANT EXECUTE ON FUNCTION remove_broken_part_quantity(integer, integer) TO inventory_user;

DROP TRIGGER IF EXISTS trg_log_new_part ON parts;
DROP FUNCTION IF EXISTS log_new_part();
DROP FUNCTION IF EXISTS build_device();
DROP TABLE IF EXISTS part_logs, device_logs, boms, parts, devices CASCADE;

CREATE TABLE parts (
    id SERIAL PRIMARY KEY,
    name TEXT NOT NULL UNIQUE,
    quantity INTEGER NOT NULL DEFAULT 0
);

CREATE TABLE devices (
    id SERIAL PRIMARY KEY,
    name TEXT NOT NULL UNIQUE
);

CREATE TABLE boms (
    id SERIAL PRIMARY KEY,
    device_id INTEGER NOT NULL REFERENCES devices(id) ON DELETE CASCADE,
    part_id INTEGER NOT NULL REFERENCES parts(id) ON DELETE CASCADE,
    quantity_required INTEGER NOT NULL
);

CREATE TABLE part_logs (
    id SERIAL PRIMARY KEY,
    part_id INTEGER REFERENCES parts(id) ON DELETE SET NULL,
    part_name TEXT NOT NULL,
    quantity_changed INTEGER NOT NULL,   -- positive = added, negative = removed
    changed_by TEXT,
    note TEXT,
    timestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE device_logs (
    id SERIAL PRIMARY KEY,
    device_id INTEGER REFERENCES devices(id) ON DELETE SET NULL,
    device_name TEXT NOT NULL,
    quantity_built INTEGER NOT NULL,
    built_by TEXT,
    timestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);



CREATE OR REPLACE FUNCTION log_new_part()
RETURNS TRIGGER AS $$
BEGIN
    INSERT INTO part_logs (part_id, part_name, quantity_changed, changed_by, note)
    VALUES (NEW.id, NEW.name, NEW.quantity, SESSION_USER, 'Initial stock');
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_log_new_part
AFTER INSERT ON parts
FOR EACH ROW
EXECUTE FUNCTION log_new_part();



CREATE OR REPLACE FUNCTION log_part_quantity_update()
RETURNS TRIGGER AS $$
DECLARE
    suppress_log TEXT := 'false';
BEGIN
    BEGIN
        suppress_log := COALESCE(current_setting('myapp.suppress_part_log', true), 'false');
    EXCEPTION
        WHEN OTHERS THEN
            NULL;
    END;

    IF suppress_log <> 'true' AND NEW.quantity IS DISTINCT FROM OLD.quantity THEN
        INSERT INTO part_logs (
            part_id,
            part_name,
            quantity_changed,
            changed_by,
            note
        )
        VALUES (
            NEW.id,
            NEW.name,
            NEW.quantity - OLD.quantity,
            SESSION_USER,
            'Manual quantity adjustment'
        );
    END IF;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;



CREATE TRIGGER trg_log_part_quantity_update
AFTER UPDATE ON parts
FOR EACH ROW
EXECUTE FUNCTION log_part_quantity_update();


CREATE OR REPLACE FUNCTION build_device(p_device_id INTEGER, p_quantity INTEGER)
RETURNS VOID AS $$
DECLARE
    device_name TEXT;
    part_record RECORD;
    required_quantity INTEGER;
    available_quantity INTEGER;
    quantity_to_deduct INTEGER;
BEGIN
	IF p_quantity IS NULL OR p_quantity <= 0 THEN
        RAISE EXCEPTION 'Quantity to build must be a positive integer, got %', p_quantity;
    END IF;
	
    SELECT name INTO device_name FROM devices WHERE id = p_device_id;
    IF device_name IS NULL THEN
        RAISE EXCEPTION 'Device with id % does not exist', p_device_id;
    END IF;

    -- Suppress trigger-based logging
    PERFORM set_config('myapp.suppress_part_log', 'true', true);

    FOR part_record IN
        SELECT p.id AS part_id, p.name AS part_name, p.quantity AS available_quantity, b.quantity_required
        FROM boms b
        JOIN parts p ON p.id = b.part_id
        WHERE b.device_id = p_device_id
    LOOP
        required_quantity := part_record.quantity_required * p_quantity;
        available_quantity := part_record.available_quantity;

        quantity_to_deduct := LEAST(available_quantity, required_quantity);

        UPDATE parts
        SET quantity = quantity - quantity_to_deduct
        WHERE id = part_record.part_id;

        INSERT INTO part_logs (
            part_id, part_name, quantity_changed, changed_by, note
        )
        VALUES (
            part_record.part_id,
            part_record.part_name,
            -quantity_to_deduct,
            SESSION_USER,
            FORMAT(
                'Used to build %s (x%s) — required %s, available %s, deducted %s',
                device_name, p_quantity, required_quantity, available_quantity, quantity_to_deduct
            )
        );
    END LOOP;

    INSERT INTO device_logs (device_id, device_name, quantity_built, built_by)
    VALUES (p_device_id, device_name, p_quantity, SESSION_USER);
END;
$$ LANGUAGE plpgsql;


CREATE OR REPLACE FUNCTION remove_broken_part_quantity(p_part_id INTEGER, p_requested_quantity INTEGER)
RETURNS VOID AS $$
DECLARE
    part_name TEXT;
    current_quantity INTEGER;
    quantity_to_remove INTEGER;
BEGIN
IF p_requested_quantity IS NULL OR p_requested_quantity <= 0 THEN
        RAISE EXCEPTION 'Requested quantity must be a positive integer, got %', p_requested_quantity;
    END IF;
	
    SELECT name, quantity INTO part_name, current_quantity
    FROM parts
    WHERE id = p_part_id;
	IF part_name IS NULL THEN
        RAISE EXCEPTION 'Part with id % does not exist', p_part_id;
    END IF;

    quantity_to_remove := LEAST(current_quantity, p_requested_quantity);

    PERFORM set_config('myapp.suppress_part_log', 'true', true);

    UPDATE parts
    SET quantity = quantity - quantity_to_remove
    WHERE id = p_part_id;

    INSERT INTO part_logs (
        part_id, part_name, quantity_changed, changed_by, note
    )
    VALUES (
        p_part_id, part_name, -quantity_to_remove, SESSION_USER,
        FORMAT('Broken amount: %s, actually removed: %s', p_requested_quantity, quantity_to_remove)
    );
END;
$$ LANGUAGE plpgsql;
