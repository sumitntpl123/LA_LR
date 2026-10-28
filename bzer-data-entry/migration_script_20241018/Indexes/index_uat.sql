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
      AND index_name IN ('idx_metadata_value_file', 'idx_metadata_value_field_lang', 'idx_metadata_value_master');

    -- DROP index if exists
    IF index_count > 0 THEN
        DROP INDEX idx_metadata_value_file ON metadata_value_cs;
	DROP INDEX idx_metadata_value_field_lang ON metadata_value_cs;
	DROP INDEX idx_metadata_value_master ON metadata_value_cs;
    END IF;
    CREATE INDEX idx_metadata_value_file ON metadata_value_cs (file_id);
    CREATE INDEX idx_metadata_value_field_lang ON metadata_value_cs (field_name, lang);
    CREATE INDEX idx_metadata_value_master ON metadata_value_cs (master_id);

    SELECT "Index created successfully" AS Status;
END$$

DELIMITER ;

-- Call the Procedure
CALL CheckAndCreateIndex();
