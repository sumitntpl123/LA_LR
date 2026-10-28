-- Drop the temporary table if it exists
DROP TEMPORARY TABLE IF EXISTS affected_bundles;

-- Create a temporary table to store the affected bundles
CREATE TEMPORARY TABLE affected_bundles AS
SELECT bm.bundle_name
FROM bundle_master bm
WHERE bm.status = 5
    AND NOT EXISTS (
          SELECT 1
          FROM case_file_master cfm
          WHERE cfm.proj_code = bm.proj_code
            AND cfm.bundle_key = bm.bundle_key
            AND cfm.status != 3
    )
    AND NOT EXISTS (
          SELECT 1
          FROM case_file_master cfm
          JOIN image_master im
            ON cfm.proj_code = im.proj_key
           AND cfm.bundle_key = im.batch_key
           AND cfm.filename = im.policy_number
          WHERE cfm.proj_code = bm.proj_code
            AND cfm.bundle_key = bm.bundle_key
            AND im.status NOT IN (25, 26, 29)
    );

-- Generate the dynamic file name with the current date and time
SET @file_path = CONCAT('C:/affected_bundles_', DATE_FORMAT(NOW(), '%Y%m%d_%H%i%S'), '.txt');

-- Generate the dynamic query for the INTO OUTFILE
SET @query = CONCAT(
    "SELECT bundle_name ",
    "FROM affected_bundles ",
    "INTO OUTFILE '", @file_path, "' ",
    "FIELDS TERMINATED BY '\\t' ",
    "LINES TERMINATED BY '\\n';"
);

-- Execute the dynamically generated query
PREPARE stmt FROM @query;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- Update the statuses of the affected bundles
UPDATE bundle_master bm
SET bm.status = 3
WHERE bm.status = 5
    AND NOT EXISTS (
          SELECT 1
          FROM case_file_master cfm
          WHERE cfm.proj_code = bm.proj_code
            AND cfm.bundle_key = bm.bundle_key
            AND cfm.status != 3
    )
    AND NOT EXISTS (
          SELECT 1
          FROM case_file_master cfm
          JOIN image_master im
            ON cfm.proj_code = im.proj_key
           AND cfm.bundle_key = im.batch_key
           AND cfm.filename = im.policy_number
          WHERE cfm.proj_code = bm.proj_code
            AND cfm.bundle_key = bm.bundle_key
            AND im.status NOT IN (25, 26, 29)
    );
    
-- Drop the temporary table after use
DROP TEMPORARY TABLE IF EXISTS affected_bundles;