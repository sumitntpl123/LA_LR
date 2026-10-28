DELIMITER $$

DROP PROCEDURE IF EXISTS CheckAndCreateIndex;
-- Stored Procedure to Check and Create Index
CREATE PROCEDURE CheckAndCreateIndex()
BEGIN
    DECLARE index_count INT;

    -- Check if the index exists
    SELECT COUNT(*) INTO index_count
    FROM information_schema.statistics
    WHERE table_schema = DATABASE()
      AND table_name = 'metadata_value_cs'
      AND index_name = 'idx_val';

    -- DROP index if exists
    IF index_count > 0 THEN
        DROP INDEX idx_val ON metadata_value_cs;
    END IF;
    CREATE INDEX idx_val ON metadata_value_cs (val);
    SELECT "Index created successfully" AS Status;
END$$

DELIMITER ;

-- Call the Procedure
CALL CheckAndCreateIndex();
