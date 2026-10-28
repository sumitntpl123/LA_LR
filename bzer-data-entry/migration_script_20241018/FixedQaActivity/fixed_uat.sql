-- STEP 1: Create a temporary table to hold bundle names directly from DB
CREATE TEMPORARY TABLE temp_bundle_names (
    bundle_name VARCHAR(50)
);

-- STEP 2: Insert bundle names into temp table based on a condition
--  Update the WHERE clause as per your need (e.g., UAT-related bundles)
INSERT INTO temp_bundle_names (bundle_name)
SELECT DISTINCT bundle_name
FROM bundle_master
WHERE status= '7'; 

-- STEP 3: Preview - Create temp table to hold missing policy_numbers
 CREATE TEMPORARY TABLE temp_missing_khatian
 SELECT
     cfm.filename AS policy_number
 FROM
    temp_bundle_names tbn
 JOIN
    bundle_master bm ON bm.bundle_name = tbn.bundle_name
 JOIN
    case_file_master cfm ON cfm.proj_code = bm.proj_code AND cfm.bundle_key = bm.bundle_key
 LEFT JOIN
    lic_qa_log lq ON lq.proj_key = cfm.proj_code AND lq.policy_number = cfm.filename
 WHERE
    lq.policy_number IS NULL;

-- Optional: Check count and preview
 SELECT COUNT(*) AS total_count_of_khatian FROM temp_missing_khatian;
 SELECT * FROM temp_missing_khatian;

-- STEP 4: Insert missing policy_numbers into lic_qa_log
INSERT INTO lic_qa_log (
    proj_key,
    box_number,
    policy_number,
    batch_key,
    created_by,
    created_dttm
)
SELECT
    bm.proj_code,
    '1' AS box_number,  -- Replace with actual box_number if available
    cfm.filename AS policy_number,
    bm.bundle_key AS batch_key,
    'u7' AS created_by,  -- Replace with actual user
    NOW() AS created_dttm
FROM
    temp_bundle_names tbn
JOIN
    bundle_master bm ON bm.bundle_name = tbn.bundle_name
JOIN
    case_file_master cfm ON cfm.proj_code = bm.proj_code AND cfm.bundle_key = bm.bundle_key
LEFT JOIN
    lic_qa_log lq ON lq.proj_key = cfm.proj_code AND lq.policy_number = cfm.filename
WHERE
    lq.policy_number IS NULL;

-- STEP 5: Clean up temporary tables (optional)
DROP TEMPORARY TABLE IF EXISTS temp_bundle_names;
DROP TEMPORARY TABLE IF EXISTS temp_missing_khatian;
