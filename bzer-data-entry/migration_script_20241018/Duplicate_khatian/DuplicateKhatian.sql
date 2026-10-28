-- Drop the temporary table if it already exists
DROP TEMPORARY TABLE IF EXISTS tempBundles;
DROP TEMPORARY TABLE IF EXISTS temp;
DROP TEMPORARY TABLE IF EXISTS tempFilename;
DROP TEMPORARY TABLE IF EXISTS tempFileIdTable;


-- Create a temporary table to store bundle names from the CSV file
CREATE TEMPORARY TABLE tempBundles (
    bundle_name VARCHAR(255) -- Stores the bundle names
);

-- Load data from the CSV file into tempBundles table
LOAD DATA LOCAL INFILE 'C:/duplicate_khatian.csv'
INTO TABLE tempBundles
FIELDS TERMINATED BY ','  -- Fields are separated by commas
ENCLOSED BY '"'           -- Fields may be enclosed in double quotes
LINES TERMINATED BY '\r\n'; -- Each row ends with a carriage return and newline

-- Create a temporary table to store filenames from metadata_value_cs
CREATE TEMPORARY TABLE temp
SELECT mv.val AS filename 
FROM metadata_value_cs mv 
INNER JOIN metadata_record_cs mr
ON mv.file_id = mr.file_id
INNER JOIN bundle_master bm
ON bm.bundle_key = mr.batch_code
AND bm.bundle_name IN (SELECT * FROM tempBundles) -- Filters by bundle names from CSV
WHERE mv.field_name = 'file-name' -- Ensures only file names are selected
AND mv.lang = 'En' -- Filters only English language entries
ORDER BY mv.val ASC; -- Sorts the filenames in ascending order

-- Create a temporary table to store filenames that are not present in case_file_master
CREATE TEMPORARY TABLE tempFilename
SELECT * FROM temp 
WHERE filename NOT IN (
    SELECT filename 
    FROM case_file_master cfm 
    INNER JOIN bundle_master bm
    ON bm.bundle_key = cfm.bundle_key
    AND bm.bundle_name IN (SELECT * FROM tempBundles) -- Filters by bundle names from CSV
);

-- Create a temporary table to store matching file IDs to be deleted
CREATE TEMPORARY TABLE tempFileIdTable (
    file_id BINARY(16) -- Stores GUIDs in binary format
);

-- Create a stored procedure to process the temporary table
DELIMITER //

DROP PROCEDURE IF EXISTS process_temp_table;
CREATE PROCEDURE process_temp_table()
BEGIN
    DECLARE match_count INT; -- Variable to store count of matching file IDs

    -- Insert matching file IDs into tempFileIdTable
    INSERT INTO tempFileIdTable (file_id)
    SELECT file_id
    FROM metadata_value_cs
    WHERE field_name = 'file-name' 
    AND lang = 'En' 
    AND val IN (SELECT filename FROM tempFilename); -- Matches filenames from tempFilename

    -- Count the number of matching file IDs
    SELECT COUNT(*) INTO match_count FROM tempFileIdTable;

    -- If there are matching file IDs, delete related records
    IF match_count > 0 THEN
        -- Delete records from metadata_value_cs where file_id matches
        DELETE FROM metadata_value_cs 
        WHERE file_id IN (SELECT file_id FROM tempFileIdTable);

        -- Delete records from metadata_record_cs where file_id matches
        DELETE FROM metadata_record_cs 
        WHERE file_id IN (SELECT file_id FROM tempFileIdTable);

        -- Return a message indicating the number of deleted records
        SELECT CONCAT('Deleted ', match_count, ' records') AS StatusMessage;
    ELSE
        -- Return a message if no matching file IDs were found
        SELECT 'No matching file IDs found. Nothing to delete.' AS StatusMessage;
    END IF;    

    -- Display remaining filenames that were not deleted
    SELECT * FROM tempFilename;
END //

DELIMITER ;

-- Execute the stored procedure
CALL process_temp_table();
