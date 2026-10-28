@echo off
setlocal enabledelayedexpansion
set /p hostname=Please enter your hostname:
set /p username=Please enter your username:
set /p password=Please enter your password:
set /p dbname=Please enter your database name:
for /F "tokens=1,2,3 delims=," %%A in ('mysql -N -h !hostname! -u !username! -p!password! -D !dbname! -e "select concat(bundle_path,',',proj_code,',',bundle_key) from bundle_master;"') do (
	set folderName=%%~nXA
	set folderPathWithEscape=%%A
	set oldFolderPath=!folderPathWithEscape:\\=\!

	set projectCode=%%B
	set bundleKey=%%C
	if exist !oldFolderPath! (
		set removedSpacesFolderName=!folderName: =!
		RENAME "!oldFolderPath!" "!removedSpacesFolderName!"
		echo Folder renamed !folderName! to !removedSpacesFolderName!
		for %%D in ("!oldFolderPath!") do set parentDirectory=%%~dpD
	       	set newFolderPathConcat=!parentDirectory!!removedSpacesFolderName!
	        set newFolderPath=!newFolderPathConcat:\=\\!

		mysql -N -h !hostname! -u !username! -p!password! -D !dbname! -e "select count(*) from bundle_master join metadata_record_cs mcs on mcs.project_code=proj_code and mcs.batch_code=bundle_key where proj_code=!projectCode! and bundle_key=!bundleKey!;" > temp_result.txt
		set /p numberOfFiles=<temp_result.txt

		if !numberOfFiles! equ 0 (
			mysql -h !hostname! -u !username! -p!password! -D !dbname! -e "update bundle_master set bundle_path='!newFolderPath!', bundle_code=trim(bundle_code),bundle_name=trim(bundle_name) where proj_code=!projectCode! and bundle_key=!bundleKey!;"
		) else (
			mysql -N -h !hostname! -u !username! -p!password! -D !dbname! --init-command "set @new_bundle_path='!newFolderPath!'; set @proj_code='!projectCode!'; set @bundle_key='!bundleKey!';" < UpdateToTrim.sql
		)
		del temp_result.txt	
		echo Updated !oldFolderPath! to !newFolderPath! in the database
	) else (
		echo Filename !oldFolderPath! does not exist!
	)	
)
endlocal
pause

