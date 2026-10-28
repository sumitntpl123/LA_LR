-- First, Created temporary table
DROP TEMPORARY TABLE IF EXISTS temp_table;
CREATE TEMPORARY TABLE temp_table (
    file_name VARCHAR(255),
    proja_name VARCHAR(255)
);

-- Load CSV data
LOAD DATA LOCAL INFILE 'C:/missing_proja.csv'
INTO TABLE temp_table
FIELDS TERMINATED BY ',' 
ENCLOSED BY '"'
LINES TERMINATED BY '\r\n'
;

-- Then create a stored procedure to work with the data
DELIMITER //

DROP PROCEDURE IF EXISTS process_temp_table;
CREATE PROCEDURE process_temp_table()
BEGIN
    -- Declare variables to hold fetched values from the cursor
    DECLARE done INT DEFAULT 0; -- Flag to indicate when the cursor reaches the end of the result set
    DECLARE p_file_name VARCHAR(255);
    DECLARE p_proja_name VARCHAR(255);

    -- Declare a cursor to iterate over the rows in the temp_table
    DECLARE cur CURSOR FOR SELECT file_name, proja_name FROM temp_table;

    -- Declare a handler to handle the NOT FOUND condition
    -- When no rows are found or the cursor reaches the end of the result set, set done = 1
    DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = 1;

    OPEN cur;

    -- Begin the loop to process each row fetched by the cursor
    read_loop: LOOP

        -- Fetch the next row from the cursor into the declared variables
        FETCH cur INTO p_file_name, p_proja_name;

        IF done THEN
            -- Exit the loop if there are no more rows
            select count(*) as Total_count from temp_table;
            LEAVE read_loop;
        END IF;

        -- Fetch master_id from metadata_master_cs table for the given proja_name
        set @master_id_mmc = (select master_id from metadata_master_cs 
        where field_name ='proja-sotter-type'
        and lang='BnPhonetic'
        and val=p_proja_name
        );
        -- Fetch master_id from metadata_value_cs table for the given file_name
        set @master_id_mvc = (select master_id from metadata_value_cs where field_name ='proja-sotter-type' 
        and file_id = (select file_id from metadata_value_cs where field_name ='file-name'
         and lang='En'
         and val=p_file_name
         ));
        -- Update the metadata_value_cs table with the new master_id value
        update metadata_value_cs set master_id = @master_id_mmc where
        field_name ='proja-sotter-type'
        and lang='En' and master_id = @master_id_mvc;

    END LOOP;

    CLOSE cur;
END //

DELIMITER ;

-- Call the stored procedure
CALL process_temp_table();
