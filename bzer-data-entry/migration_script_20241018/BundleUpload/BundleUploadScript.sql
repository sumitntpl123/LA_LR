-- Update master_id for 'proja-sotter-type' entries in metadata_value_cs by joining with metadata_master_cs
UPDATE metadata_value_cs mv
	JOIN metadata_master_cs mm ON mm.field_name = mv.field_name 
	AND mm.val = mv.val 
	and mm.lang IN ( 'BnPhonetic') 
	and mm.field_name = 'proja-sotter-type'
SET mv.master_id = mm.master_id
WHERE mv.field_name = 'proja-sotter-type'
  AND mv.lang IN ( 'BnPhonetic')
  AND mv.master_id IS NULL;

-- Propagate master_id for 'proja-sotter-type' across other language entries
UPDATE metadata_value_cs mv_target
	JOIN metadata_value_cs mv_source
	ON mv_source.file_id = mv_target.file_id
  AND mv_source.field_name = mv_target.field_name
  AND mv_source.lang = 'BnPhonetic'
  AND mv_source.master_id IS NOT NULL
set mv_target.master_id = mv_source.master_id
where mv_target.lang in ('En','Bn')
and mv_target.field_name='proja-sotter-type';

-- Removed all the proja-sotter-type record for lang 'BnPhonetic'
delete from metadata_value_cs
where
    field_name = 'proja-sotter-type'
    and lang in ('BnPhonetic','Bn')
    and master_id is not null;

drop temporary table if exists temp_proja_types ;
-- Select specific fields and export them into a text file
CREATE TEMPORARY TABLE temp_proja_types AS
SELECT
    fn.val AS `file-name`,
    pst_phonetic.val AS `proja-sotter-type-phonetic`,
    pst_bangla.val AS `proja-sotter-type-bangla`
FROM
    metadata_value_cs fn
    JOIN metadata_value_cs pst_phonetic ON fn.file_id = pst_phonetic.file_id
    LEFT JOIN metadata_value_cs pst_bangla ON pst_phonetic.file_id = pst_bangla.file_id
    AND pst_bangla.field_name = 'proja-sotter-type'
    AND pst_bangla.lang = 'Bn'
    AND pst_bangla.master_id IS NULL
WHERE
    fn.field_name = 'file-name'
    AND fn.lang = 'En'
    AND pst_phonetic.field_name = 'proja-sotter-type'
    AND pst_phonetic.lang = 'BnPhonetic'
    AND pst_phonetic.master_id IS NULL;

 select * from temp_proja_types;