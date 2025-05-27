DROP TRIGGER IF EXISTS trg_log_new_part ON parts;
DROP TRIGGER IF EXISTS trg_log_part_update ON parts;
DROP FUNCTION IF EXISTS log_new_part();
DROP FUNCTION IF EXISTS log_part_quantity_update();
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
    part_name TEXT,
    quantity_changed INTEGER NOT NULL,   -- positive = added, negative = removed
    changed_by TEXT,
    timestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE device_logs (
    id SERIAL PRIMARY KEY,
    device_id INTEGER REFERENCES devices(id) ON DELETE SET NULL,
    device_name TEXT,
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
    diff INTEGER;
BEGIN
    diff := NEW.quantity - OLD.quantity;

    IF diff <> 0 THEN
        INSERT INTO part_logs (part_id, part_name, quantity_changed, changed_by)
        VALUES (NEW.id, NEW.name, diff, SESSION_USER);
    END IF;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_log_part_update
AFTER UPDATE ON parts
FOR EACH ROW
WHEN (OLD.quantity IS DISTINCT FROM NEW.quantity)
EXECUTE FUNCTION log_part_quantity_update();

CREATE OR REPLACE FUNCTION build_device(p_device_id INTEGER, p_quantity INTEGER)
RETURNS VOID AS $$
DECLARE
    device_name TEXT;
BEGIN
    UPDATE parts
    SET quantity = quantity - (b.quantity_required * p_quantity)
    FROM boms b
    WHERE parts.id = b.part_id AND b.device_id = p_device_id;

    SELECT name INTO device_name FROM devices WHERE id = p_device_id;

    INSERT INTO device_logs (device_id, device_name, quantity_built, built_by)
    VALUES (p_device_id, device_name, p_quantity, SESSION_USER);
END;
$$ LANGUAGE plpgsql;
