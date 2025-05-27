-- Cleanup
DROP TRIGGER IF EXISTS trg_log_new_part ON parts;
DROP FUNCTION IF EXISTS log_new_part();
DROP FUNCTION IF EXISTS build_device();
DROP TABLE IF EXISTS part_logs, device_logs, boms, parts, devices CASCADE;

-- Tables

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
    part_id INTEGER NOT NULL REFERENCES parts(id) ON DELETE SET NULL,
    part_name TEXT NOT NULL,
    quantity_changed INTEGER NOT NULL,   -- positive = added, negative = removed
    changed_by TEXT,
    note TEXT,
    timestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE device_logs (
    id SERIAL PRIMARY KEY,
    device_id INTEGER NOT NULL REFERENCES devices(id) ON DELETE SET NULL,
    device_name TEXT NOT NULL,
    quantity_built INTEGER NOT NULL,
    built_by TEXT,
    timestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Trigger: log initial stock when a part is inserted
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

-- build_device() with detailed part usage logging
CREATE OR REPLACE FUNCTION build_device(p_device_id INTEGER, p_quantity INTEGER)
RETURNS VOID AS $$
DECLARE
    device_name TEXT;
    part_record RECORD;
BEGIN
    -- Get device name
    SELECT name INTO device_name FROM devices WHERE id = p_device_id;

    -- For each BOM entry, deduct quantity and log it
    FOR part_record IN
        SELECT p.id AS part_id, p.name AS part_name, b.quantity_required
        FROM boms b
        JOIN parts p ON p.id = b.part_id
        WHERE b.device_id = p_device_id
    LOOP
        -- Deduct quantity
        UPDATE parts
        SET quantity = quantity - (part_record.quantity_required * p_quantity)
        WHERE id = part_record.part_id;

        -- Log part usage
        INSERT INTO part_logs (
            part_id, part_name, quantity_changed, changed_by, note
        )
        VALUES (
            part_record.part_id,
            part_record.part_name,
            -(part_record.quantity_required * p_quantity),
            SESSION_USER,
            FORMAT('Used to build %s (x%s)', device_name, p_quantity)
        );
    END LOOP;

    -- Log the device build
    INSERT INTO device_logs (device_id, device_name, quantity_built, built_by)
    VALUES (p_device_id, device_name, p_quantity, SESSION_USER);
END;
$$ LANGUAGE plpgsql;
