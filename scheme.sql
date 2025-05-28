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
    SELECT name INTO device_name FROM devices WHERE id = p_device_id;

    -- Suppress trigger-based logging
    PERFORM set_config('myapp.suppress_part_log', 'true', true);

    -- For each BOM part, deduct what we can
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
