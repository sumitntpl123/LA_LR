update bundle_master bm
	join case_file_master cfm on bm.proj_code=cfm.proj_code and bm.bundle_key=cfm.bundle_key
	join metadata_entry me on bm.proj_code=me.proj_code and bm.bundle_key=me.bundle_key
	join metadata_record_cs mrc on bm.proj_code=mrc.project_code and bm.bundle_key=mrc.batch_code
	join metadata_value_cs mvc on mvc.file_id=mrc.file_id
	join master_code_cs mcc on mcc.master_id=mvc.master_id
	left join metadata_value_cs mvc_file_name on mvc_file_name.file_id=mrc.file_id and mvc_file_name.field_name='file-name' and 	mvc_file_name.lang='En'
set
bundle_code=trim(bundle_code), bundle_name=trim(bundle_name),
cfm.filename=trim(cfm.filename),
me.filename=trim(me.filename),
mcc.code=trim(mcc.code),
bundle_path=@new_bundle_path,
mvc_file_name.val=trim(mvc_file_name.val)
where bm.proj_code=@proj_code and bm.bundle_key=@bundle_key and mvc.field_name='district';






