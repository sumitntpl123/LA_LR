DELIMITER $$

-- Drop the function if it already exists to avoid duplicate function errors
DROP FUNCTION IF EXISTS InsertProjaSotterTypeEntries $$

-- Create a new function that returns a VARCHAR(20) status message
CREATE FUNCTION InsertProjaSotterTypeEntries() RETURNS VARCHAR(28)
BEGIN

    -- SECTION 1: First, delete all proja-sotter-type codes (if exists) from master_code_cs table 
    delete from master_code_cs where master_id in ( select master_id from metadata_master_cs where field_name='proja-sotter-type' and lang='BnPhonetic');


    -- This ID will be used as parent_master_id for all new entries
    set @parent_master_id = (select master_id from metadata_master_cs where field_name='district' and parent_master_id is null and lang='BnPhonetic');

    -- SECTION 2: Insert Proja-Sotter-Type Entries:
    -- code 101
    set @master_id_101 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_101, 'proja-sotter-type', 'En', 1, '', @parent_master_id
    from dual
     where not exists ( 
    select 1 from metadata_master_cs where val = 'malik' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_101, 'proja-sotter-type', 'Bn', 1, 'মালিক', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'malik' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_101, 'proja-sotter-type', 'BnPhonetic', 1, 'malik', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'malik' and lang = 'BnPhonetic'
    );

   -- code 102
    set @master_id_102 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_102, 'proja-sotter-type', 'En', 2, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI khajana brriddhir zOgy' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_102, 'proja-sotter-type', 'Bn', 2, 'মধ্যস্বত্বাধিকারী চিরস্থায়ী খাজানা বৃদ্ধির যোগ্য', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI khajana brriddhir zOgy' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_102, 'proja-sotter-type', 'BnPhonetic', 2, 'mdhyswtwadhikarI cirsthayI khajana brriddhir zOgy', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI khajana brriddhir zOgy' and lang = 'BnPhonetic'
    );

   -- code 103
    set @master_id_103 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_103, 'proja-sotter-type', 'En', 3, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt dokhliswtw shUny' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_103, 'proja-sotter-type', 'Bn', 3, 'রায়ত দখলিস্বত্ব শূন্য', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt dokhliswtw shUny' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_103, 'proja-sotter-type', 'BnPhonetic', 3, 'rayt dokhliswtw shUny', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt dokhliswtw shUny' and lang = 'BnPhonetic'
    );

    -- code 104
    set @master_id_104 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_104, 'proja-sotter-type', 'En', 4, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'pottnI modhy swtwadhikarI cirsthayI mOkborI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_104, 'proja-sotter-type', 'Bn', 4, 'পত্তনী মধ্য স্বত্বাধিকারী চিরস্থায়ী মোকবরী', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'pottnI modhy swtwadhikarI cirsthayI mOkborI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_104, 'proja-sotter-type', 'BnPhonetic', 4, 'pottnI modhy swtwadhikarI cirsthayI mOkborI', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'pottnI modhy swtwadhikarI cirsthayI mOkborI' and lang = 'BnPhonetic'
    );

    -- code 105
    set @master_id_105 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_105, 'proja-sotter-type', 'En', 5, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'modhyswtwadhiKarI cirsthayI mOk`rrI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_105, 'proja-sotter-type', 'Bn', 5, 'মধ্যস্বত্বাধিকারী চিরস্থায়ী মোকররী', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'modhyswtwadhiKarI cirsthayI mOk`rrI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_105, 'proja-sotter-type', 'BnPhonetic', 5, 'modhyswtwadhiKarI cirsthayI mOk`rrI', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'modhyswtwadhiKarI cirsthayI mOk`rrI' and lang = 'BnPhonetic'
    );

    -- code 106
    set @master_id_106 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_106, 'proja-sotter-type', 'En', 6, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt mOk`rrI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_106, 'proja-sotter-type', 'Bn', 6, 'রায়ত মোকররী', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt mOk`rrI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_106, 'proja-sotter-type', 'BnPhonetic', 6, 'rayt mOk`rrI', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt mOk`rrI' and lang = 'BnPhonetic'
    );

    -- code 107
    set @master_id_107 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_107, 'proja-sotter-type', 'En', 7, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt sthitiban' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_107, 'proja-sotter-type', 'Bn', 7, 'রায়ত স্থিতিবান', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt sthitiban' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_107, 'proja-sotter-type', 'BnPhonetic', 7, 'rayt sthitiban', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt sthitiban' and lang = 'BnPhonetic'
    );

    -- code 108
    set @master_id_108 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_108, 'proja-sotter-type', 'En', 8, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'kOrrfa dkhliswtwbiSiSHT' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_108, 'proja-sotter-type', 'Bn', 8, 'কোর্ফা দখলিস্বত্ববিশিষ্ট', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'kOrrfa dkhliswtwbiSiSHT' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_108, 'proja-sotter-type', 'BnPhonetic', 8, 'kOrrfa dkhliswtwbiSiSHT', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'kOrrfa dkhliswtwbiSiSHT' and lang = 'BnPhonetic'
    );

    -- code 109
    set @master_id_109 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_109, 'proja-sotter-type', 'En', 9, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'kOrrfa' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_109, 'proja-sotter-type', 'Bn', 9, 'কোর্ফা', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'kOrrfa' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_109, 'proja-sotter-type', 'BnPhonetic', 9, 'kOrrfa', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'kOrrfa' and lang = 'BnPhonetic'
    );

    -- code 110
    set @master_id_110 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_110, 'proja-sotter-type', 'En', 10, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt dkhliswtwbiSiSHT' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_110, 'proja-sotter-type', 'Bn', 10, 'রায়ত দখলিস্বত্ববিশিষ্ট', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt dkhliswtwbiSiSHT' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_110, 'proja-sotter-type', 'BnPhonetic', 10, 'rayt dkhliswtwbiSiSHT', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt dkhliswtwbiSiSHT' and lang = 'BnPhonetic'
    );

    -- code 111
    set @master_id_111 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_111, 'proja-sotter-type', 'En', 11, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_111, 'proja-sotter-type', 'Bn', 11, 'দখলকার', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_111, 'proja-sotter-type', 'BnPhonetic', 11, 'dkhl`kar', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar' and lang = 'BnPhonetic'
    );

    -- code 112
    set @master_id_112 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_112, 'proja-sotter-type', 'En', 12, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI osthayI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_112, 'proja-sotter-type', 'Bn', 12, 'মধ্যস্বত্বাধিকারী অস্থায়ী', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI osthayI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_112, 'proja-sotter-type', 'BnPhonetic', 12, 'mdhyswtwadhikarI osthayI', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI osthayI' and lang = 'BnPhonetic'
    );

    -- code 113
    set @master_id_113 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_113, 'proja-sotter-type', 'En', 13, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'paTTamule mOkarrI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_113, 'proja-sotter-type', 'Bn', 13, 'পাট্টামুলে মোকাররী', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'paTTamule mOkarrI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_113, 'proja-sotter-type', 'BnPhonetic', 13, 'paTTamule mOkarrI', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'paTTamule mOkarrI' and lang = 'BnPhonetic'
    );

    -- code 114
    set @master_id_114 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_114, 'proja-sotter-type', 'En', 14, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'jmidar' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_114, 'proja-sotter-type', 'Bn', 14, 'জমিদার', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'jmidar' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_114, 'proja-sotter-type', 'BnPhonetic', 14, 'jmidar', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'jmidar' and lang = 'BnPhonetic'
    );

    -- code 115
    set @master_id_115 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_115, 'proja-sotter-type', 'En', 15, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhlakar cak`ran' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_115, 'proja-sotter-type', 'Bn', 15, 'দখলাকার চাকরান', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhlakar cak`ran' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_115, 'proja-sotter-type', 'BnPhonetic', 15, 'dkhlakar cak`ran', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhlakar cak`ran' and lang = 'BnPhonetic'
    );

    -- code 116
    set @master_id_116 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_116, 'proja-sotter-type', 'En', 16, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI nisk`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_116, 'proja-sotter-type', 'Bn', 16, 'মধ্যস্বত্বাধিকারী চিরস্থায়ী নিস্কর', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI nisk`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_116, 'proja-sotter-type', 'BnPhonetic', 16, 'mdhyswtwadhikarI cirsthayI nisk`r', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI nisk`r' and lang = 'BnPhonetic'
    );

    -- code 117
    set @master_id_117 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_117, 'proja-sotter-type', 'En', 17, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt nisk`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_117, 'proja-sotter-type', 'Bn', 17, 'রায়ত নিস্কর', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt nisk`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_117, 'proja-sotter-type', 'BnPhonetic', 17, 'rayt nisk`r', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt nisk`r' and lang = 'BnPhonetic'
    );

    -- code 118
    set @master_id_118 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_118, 'proja-sotter-type', 'En', 18, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_118, 'proja-sotter-type', 'Bn', 18, 'মধ্যস্বত্বাধিকারী চিরস্থায়ী', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_118, 'proja-sotter-type', 'BnPhonetic', 18, 'mdhyswtwadhikarI cirsthayI', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI' and lang = 'BnPhonetic'
    );

    -- code 119
    set @master_id_119 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_119, 'proja-sotter-type', 'En', 19, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_119, 'proja-sotter-type', 'Bn', 19, 'রায়ত', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_119, 'proja-sotter-type', 'BnPhonetic', 19, 'rayt', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt' and lang = 'BnPhonetic'
    );

    -- code 120
    set @master_id_120 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_120, 'proja-sotter-type', 'En', 20, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_120, 'proja-sotter-type', 'Bn', 20, 'দখল', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_120, 'proja-sotter-type', 'BnPhonetic', 20, 'dkhl', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl' and lang = 'BnPhonetic'
    );

    -- code 121
    set @master_id_121 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_121, 'proja-sotter-type', 'En', 21, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'ullekh nai' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_121, 'proja-sotter-type', 'Bn', 21, 'উল্লেখ নাই', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'ullekh nai' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_121, 'proja-sotter-type', 'BnPhonetic', 21, 'ullekh nai', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'ullekh nai' and lang = 'BnPhonetic'
    );

    -- code 122
    set @master_id_122 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_122, 'proja-sotter-type', 'En', 22, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar nisk`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_122, 'proja-sotter-type', 'Bn', 22, 'দখলকার নিস্কর', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar nisk`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_122, 'proja-sotter-type', 'BnPhonetic', 22, 'dkhl`kar nisk`r', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar nisk`r' and lang = 'BnPhonetic'
    );

    -- code 123
    set @master_id_123 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_123, 'proja-sotter-type', 'En', 23, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'kOrrfa b`hukalab`dhi' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_123, 'proja-sotter-type', 'Bn', 23, 'কোর্ফা বহুকালাবধি', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'kOrrfa b`hukalab`dhi' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_123, 'proja-sotter-type', 'BnPhonetic', 23, 'kOrrfa b`hukalab`dhi', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'kOrrfa b`hukalab`dhi' and lang = 'BnPhonetic'
    );

    -- code 124
    set @master_id_124 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_124, 'proja-sotter-type', 'En', 24, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI niSHk`rdar pIrtr' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_124, 'proja-sotter-type', 'Bn', 24, 'মধ্য স্বত্বাধিকারী নিষ্করদার পীরত্র', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI niSHk`rdar pIrtr' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_124, 'proja-sotter-type', 'BnPhonetic', 24, 'mdhy swtwadhikarI niSHk`rdar pIrtr', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI niSHk`rdar pIrtr' and lang = 'BnPhonetic'
    );

    -- code 125
    set @master_id_125 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_125, 'proja-sotter-type', 'En', 25, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'lakherajdar' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_125, 'proja-sotter-type', 'Bn', 25, 'লাখেরাজদার', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'lakherajdar' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_125, 'proja-sotter-type', 'BnPhonetic', 25, 'lakherajdar', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'lakherajdar' and lang = 'BnPhonetic'
    );

    -- code 126
    set @master_id_126 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_126, 'proja-sotter-type', 'En', 26, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'd`r p`ttni mdhyswtwadhikarI cirsthayI mOk`rrI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_126, 'proja-sotter-type', 'Bn', 26, 'দর পত্তনি মধ্যস্বত্বাধিকারী চিরস্থায়ী মোকররী', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'd`r p`ttni mdhyswtwadhikarI cirsthayI mOk`rrI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_126, 'proja-sotter-type', 'BnPhonetic', 26, 'd`r p`ttni mdhyswtwadhikarI cirsthayI mOk`rrI', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'd`r p`ttni mdhyswtwadhikarI cirsthayI mOk`rrI' and lang = 'BnPhonetic'
    );

    -- code 127
    set @master_id_127 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_127, 'proja-sotter-type', 'En', 27, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'raYt nisk`r brhmtr' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_127, 'proja-sotter-type', 'Bn', 27, 'রায়ত নিস্কর ব্রহ্মত্র', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'raYt nisk`r brhmtr' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_127, 'proja-sotter-type', 'BnPhonetic', 27, 'raYt nisk`r brhmtr', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'raYt nisk`r brhmtr' and lang = 'BnPhonetic'
    );

    -- code 128
    set @master_id_128 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_128, 'proja-sotter-type', 'En', 28, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI nisk`rdar brhmtr' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_128, 'proja-sotter-type', 'Bn', 28, 'মধ্য স্বত্বাধিকারী নিস্করদার ব্রহ্মত্র', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI nisk`rdar brhmtr' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_128, 'proja-sotter-type', 'BnPhonetic', 28, 'mdhy swtwadhikarI nisk`rdar brhmtr', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI nisk`rdar brhmtr' and lang = 'BnPhonetic'
    );

    -- code 129
    set @master_id_129 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_129, 'proja-sotter-type', 'En', 29, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI nisk`rdar' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_129, 'proja-sotter-type', 'Bn', 29, 'মধ্য স্বত্বাধিকারী নিস্করদার', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI nisk`rdar' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_129, 'proja-sotter-type', 'BnPhonetic', 29, 'mdhy swtwadhikarI nisk`rdar', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI nisk`rdar' and lang = 'BnPhonetic'
    );

    -- code 130
    set @master_id_130 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_130, 'proja-sotter-type', 'En', 30, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'modhy swtwadhikarI niSHk`r cir`sthayI n`he' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_130, 'proja-sotter-type', 'Bn', 30, 'মধ্য স্বত্বাধিকারী নিষ্কর চিরস্থায়ী নহে', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'modhy swtwadhikarI niSHk`r cir`sthayI n`he' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_130, 'proja-sotter-type', 'BnPhonetic', 30, 'modhy swtwadhikarI niSHk`r cir`sthayI n`he', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'modhy swtwadhikarI niSHk`r cir`sthayI n`he' and lang = 'BnPhonetic'
    );

    -- code 131
    set @master_id_131 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_131, 'proja-sotter-type', 'En', 31, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'raYt niSHk`r debtr' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_131, 'proja-sotter-type', 'Bn', 31, 'রায়ত নিষ্কর দেবত্র', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'raYt niSHk`r debtr' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_131, 'proja-sotter-type', 'BnPhonetic', 31, 'raYt niSHk`r debtr', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'raYt niSHk`r debtr' and lang = 'BnPhonetic'
    );

    -- code 132
    set @master_id_132 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_132, 'proja-sotter-type', 'En', 32, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dokhol`kar cak`raN mNDlan' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_132, 'proja-sotter-type', 'Bn', 32, 'দখলকার চাকরাণ মণ্ডলান', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dokhol`kar cak`raN mNDlan' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_132, 'proja-sotter-type', 'BnPhonetic', 32, 'dokhol`kar cak`raN mNDlan', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dokhol`kar cak`raN mNDlan' and lang = 'BnPhonetic'
    );

    -- code 133
    set @master_id_133 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_133, 'proja-sotter-type', 'En', 33, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dokhol`kar bos`t prja' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_133, 'proja-sotter-type', 'Bn', 33, 'দখলকার বসত প্রজা', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dokhol`kar bos`t prja' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_133, 'proja-sotter-type', 'BnPhonetic', 33, 'dokhol`kar bos`t prja', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dokhol`kar bos`t prja' and lang = 'BnPhonetic'
    );

    -- code 134
    set @master_id_134 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_134, 'proja-sotter-type', 'En', 34, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'kOrrfa dkhli swtwbiSiSHT deSacar`onusare' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_134, 'proja-sotter-type', 'Bn', 34, 'কোর্ফা দখলি স্বত্ববিশিষ্ট দেশাচারঅনুসারে', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'kOrrfa dkhli swtwbiSiSHT deSacar`onusare' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_134, 'proja-sotter-type', 'BnPhonetic', 34, 'kOrrfa dkhli swtwbiSiSHT deSacar`onusare', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'kOrrfa dkhli swtwbiSiSHT deSacar`onusare' and lang = 'BnPhonetic'
    );

    -- code 135
    set @master_id_135 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_135, 'proja-sotter-type', 'En', 35, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dokhol`kar cak`raN tohoSIldar' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_135, 'proja-sotter-type', 'Bn', 35, 'দখলকার চাকরাণ তহশীলদার', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dokhol`kar cak`raN tohoSIldar' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_135, 'proja-sotter-type', 'BnPhonetic', 35, 'dokhol`kar cak`raN tohoSIldar', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dokhol`kar cak`raN tohoSIldar' and lang = 'BnPhonetic'
    );

    -- code 136
    set @master_id_136 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_136, 'proja-sotter-type', 'En', 36, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dokhol`kar cak`raN napit' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_136, 'proja-sotter-type', 'Bn', 36, 'দখলকার চাকরাণ নাপিত', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dokhol`kar cak`raN napit' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_136, 'proja-sotter-type', 'BnPhonetic', 36, 'dokhol`kar cak`raN napit', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dokhol`kar cak`raN napit' and lang = 'BnPhonetic'
    );

    -- code 137
    set @master_id_137 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_137, 'proja-sotter-type', 'En', 37, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dokhol`kar cak`raN DHUlI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_137, 'proja-sotter-type', 'Bn', 37, 'দখলকার চাকরাণ ঢূলী', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dokhol`kar cak`raN DHUlI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_137, 'proja-sotter-type', 'BnPhonetic', 37, 'dokhol`kar cak`raN DHUlI', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dokhol`kar cak`raN DHUlI' and lang = 'BnPhonetic'
    );

    -- code 138
    set @master_id_138 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_138, 'proja-sotter-type', 'En', 38, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'modhy swtwadhikarI cirosthayI mOk`rorI nohe' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_138, 'proja-sotter-type', 'Bn', 38, 'মধ্য স্বত্বাধিকারী চিরস্থায়ী মোকররী নহে', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'modhy swtwadhikarI cirosthayI mOk`rorI nohe' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_138, 'proja-sotter-type', 'BnPhonetic', 38, 'modhy swtwadhikarI cirosthayI mOk`rorI nohe', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'modhy swtwadhikarI cirosthayI mOk`rorI nohe' and lang = 'BnPhonetic'
    );

    -- code 139
    set @master_id_139 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_139, 'proja-sotter-type', 'En', 39, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dokhol`kar cak`raN halsahana' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_139, 'proja-sotter-type', 'Bn', 39, 'দখলকার চাকরাণ হালসাহানা', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dokhol`kar cak`raN halsahana' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_139, 'proja-sotter-type', 'BnPhonetic', 39, 'dokhol`kar cak`raN halsahana', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dokhol`kar cak`raN halsahana' and lang = 'BnPhonetic'
    );

    -- code 140
    set @master_id_140 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_140, 'proja-sotter-type', 'En', 40, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'pottni modhy swtwadhikarI cirosthayI mOk`rorI nohe' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_140, 'proja-sotter-type', 'Bn', 40, 'পত্তনি মধ্য স্বত্বাধিকারী চিরস্থায়ী মোকররী নহে', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'pottni modhy swtwadhikarI cirosthayI mOk`rorI nohe' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_140, 'proja-sotter-type', 'BnPhonetic', 40, 'pottni modhy swtwadhikarI cirosthayI mOk`rorI nohe', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'pottni modhy swtwadhikarI cirosthayI mOk`rorI nohe' and lang = 'BnPhonetic'
    );

    -- code 141
    set @master_id_141 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_141, 'proja-sotter-type', 'En', 41, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'modhy swtwadhikarI niSHKordar debOtt`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_141, 'proja-sotter-type', 'Bn', 41, 'মধ্য স্বত্বাধিকারী নিষ্করদার দেবোত্তর', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'modhy swtwadhikarI niSHKordar debOtt`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_141, 'proja-sotter-type', 'BnPhonetic', 41, 'modhy swtwadhikarI niSHKordar debOtt`r', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'modhy swtwadhikarI niSHKordar debOtt`r' and lang = 'BnPhonetic'
    );

    -- code 142
    set @master_id_142 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_142, 'proja-sotter-type', 'En', 42, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'kOrrfa sthitibanbishiSHT' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_142, 'proja-sotter-type', 'Bn', 42, 'কোর্ফা স্থিতিবানবিশিষ্ট', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'kOrrfa sthitibanbishiSHT' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_142, 'proja-sotter-type', 'BnPhonetic', 42, 'kOrrfa sthitibanbishiSHT', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'kOrrfa sthitibanbishiSHT' and lang = 'BnPhonetic'
    );

    -- code 143
    set @master_id_143 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_143, 'proja-sotter-type', 'En', 43, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_143, 'proja-sotter-type', 'Bn', 43, 'মধ্য স্বত্বাধিকারী', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_143, 'proja-sotter-type', 'BnPhonetic', 43, 'mdhy swtwadhikarI', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI' and lang = 'BnPhonetic'
    );

    -- code 144
    set @master_id_144 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_144, 'proja-sotter-type', 'En', 44, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI myadI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_144, 'proja-sotter-type', 'Bn', 44, 'মধ্য স্বত্বাধিকারী ম্যাদী', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI myadI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_144, 'proja-sotter-type', 'BnPhonetic', 44, 'mdhy swtwadhikarI myadI', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI myadI' and lang = 'BnPhonetic'
    );

    -- code 145
    set @master_id_145 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_145, 'proja-sotter-type', 'En', 45, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt mOk`rrI sthitiban' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_145, 'proja-sotter-type', 'Bn', 45, 'রায়ত মোকররী স্থিতিবান', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt mOk`rrI sthitiban' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_145, 'proja-sotter-type', 'BnPhonetic', 45, 'rayt mOk`rrI sthitiban', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt mOk`rrI sthitiban' and lang = 'BnPhonetic'
    );

    -- code 146
    set @master_id_146 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_146, 'proja-sotter-type', 'En', 46, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dokhl`kar cak`raN DhakI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_146, 'proja-sotter-type', 'Bn', 46, 'দখলকার চাকরাণ ঢাকী', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dokhl`kar cak`raN DhakI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_146, 'proja-sotter-type', 'BnPhonetic', 46, 'dokhl`kar cak`raN DhakI', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dokhl`kar cak`raN DhakI' and lang = 'BnPhonetic'
    );

    -- code 147
    set @master_id_147 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_147, 'proja-sotter-type', 'En', 47, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI osthayI myad kal`mdhye khajanabrriddhir`ozOgy' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_147, 'proja-sotter-type', 'Bn', 47, 'মধ্যস্বত্বাধিকারী অস্থায়ী ম্যাদ কালমধ্যে খাজানাবৃদ্ধিরঅযোগ্য', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI osthayI myad kal`mdhye khajanabrriddhir`ozOgy' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_147, 'proja-sotter-type', 'BnPhonetic', 47, 'mdhyswtwadhikarI osthayI myad kal`mdhye khajanabrriddhir`ozOgy', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI osthayI myad kal`mdhye khajanabrriddhir`ozOgy' and lang = 'BnPhonetic'
    );

    -- code 148
    set @master_id_148 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_148, 'proja-sotter-type', 'En', 48, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'vOgdkhlsUtre' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_148, 'proja-sotter-type', 'Bn', 48, 'ভোগদখলসূত্রে', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'vOgdkhlsUtre' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_148, 'proja-sotter-type', 'BnPhonetic', 48, 'vOgdkhlsUtre', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'vOgdkhlsUtre' and lang = 'BnPhonetic'
    );

    -- code 149
    set @master_id_149 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_149, 'proja-sotter-type', 'En', 49, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI nhe' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_149, 'proja-sotter-type', 'Bn', 49, 'মধ্যস্বত্বাধিকারী চিরস্থায়ী নহে', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI nhe' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_149, 'proja-sotter-type', 'BnPhonetic', 49, 'mdhyswtwadhikarI cirsthayI nhe', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI nhe' and lang = 'BnPhonetic'
    );

    -- code 150
    set @master_id_150 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_150, 'proja-sotter-type', 'En', 50, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'kOrrfa myadI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_150, 'proja-sotter-type', 'Bn', 50, 'কোর্ফা ম্যাদী', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'kOrrfa myadI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_150, 'proja-sotter-type', 'BnPhonetic', 50, 'kOrrfa myadI', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'kOrrfa myadI' and lang = 'BnPhonetic'
    );

    -- code 151
    set @master_id_151 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_151, 'proja-sotter-type', 'En', 51, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI nhe khajana brriddhir zOgy' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_151, 'proja-sotter-type', 'Bn', 51, 'মধ্যস্বত্বাধিকারী চিরস্থায়ী নহে খাজানা বৃদ্ধির যোগ্য', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI nhe khajana brriddhir zOgy' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_151, 'proja-sotter-type', 'BnPhonetic', 51, 'mdhyswtwadhikarI cirsthayI nhe khajana brriddhir zOgy', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI nhe khajana brriddhir zOgy' and lang = 'BnPhonetic'
    );

    -- code 152
    set @master_id_152 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_152, 'proja-sotter-type', 'En', 52, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = '22.2 dhara' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_152, 'proja-sotter-type', 'Bn', 52, '২২।২ ধারা', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = '22.2 dhara' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_152, 'proja-sotter-type', 'BnPhonetic', 52, '22.2 dhara', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = '22.2 dhara' and lang = 'BnPhonetic'
    );

    -- code 153
    set @master_id_153 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_153, 'proja-sotter-type', 'En', 53, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI niSHk`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_153, 'proja-sotter-type', 'Bn', 53, 'মধ্য স্বত্বাধিকারী নিষ্কর', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI niSHk`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_153, 'proja-sotter-type', 'BnPhonetic', 53, 'mdhy swtwadhikarI niSHk`r', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI niSHk`r' and lang = 'BnPhonetic'
    );

    -- code 154
    set @master_id_154 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_154, 'proja-sotter-type', 'En', 54, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayti sthitiban' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_154, 'proja-sotter-type', 'Bn', 54, 'রায়তি স্থিতিবান', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayti sthitiban' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_154, 'proja-sotter-type', 'BnPhonetic', 54, 'rayti sthitiban', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayti sthitiban' and lang = 'BnPhonetic'
    );

    -- code 155
    set @master_id_155 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_155, 'proja-sotter-type', 'En', 55, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt niSHk`r Sibtr' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_155, 'proja-sotter-type', 'Bn', 55, 'রায়ত নিষ্কর শিবত্র', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt niSHk`r Sibtr' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_155, 'proja-sotter-type', 'BnPhonetic', 55, 'rayt niSHk`r Sibtr', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt niSHk`r Sibtr' and lang = 'BnPhonetic'
    );

    -- code 156
    set @master_id_156 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_156, 'proja-sotter-type', 'En', 56, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r mdhy swtwadhikarI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_156, 'proja-sotter-type', 'Bn', 56, 'নিষ্কর মধ্য স্বত্বাধিকারী', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r mdhy swtwadhikarI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_156, 'proja-sotter-type', 'BnPhonetic', 56, 'niSHk`r mdhy swtwadhikarI', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r mdhy swtwadhikarI' and lang = 'BnPhonetic'
    );

    -- code 157
    set @master_id_157 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_157, 'proja-sotter-type', 'En', 57, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar niSHk`r brhmtr' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_157, 'proja-sotter-type', 'Bn', 57, 'দখলকার নিষ্কর ব্রহ্মত্র', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar niSHk`r brhmtr' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_157, 'proja-sotter-type', 'BnPhonetic', 57, 'dkhl`kar niSHk`r brhmtr', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar niSHk`r brhmtr' and lang = 'BnPhonetic'
    );

    -- code 158
    set @master_id_158 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_158, 'proja-sotter-type', 'En', 58, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`ran SIbpUjarjny' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_158, 'proja-sotter-type', 'Bn', 58, 'দখলকার চাকরান শীবপূজারজন্য', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`ran SIbpUjarjny' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_158, 'proja-sotter-type', 'BnPhonetic', 58, 'dkhl`kar cak`ran SIbpUjarjny', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`ran SIbpUjarjny' and lang = 'BnPhonetic'
    );

    -- code 159
    set @master_id_159 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_159, 'proja-sotter-type', 'En', 59, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN nity pUjar jny' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_159, 'proja-sotter-type', 'Bn', 59, 'দখলকার চাকরাণ নিত্য পূজার জন্য', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN nity pUjar jny' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_159, 'proja-sotter-type', 'BnPhonetic', 59, 'dkhl`kar cak`raN nity pUjar jny', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN nity pUjar jny' and lang = 'BnPhonetic'
    );

    -- code 160
    set @master_id_160 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_160, 'proja-sotter-type', 'En', 60, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt niShk`r bOISHNbOtt`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_160, 'proja-sotter-type', 'Bn', 60, 'রায়ত নিষ্কর বৈষ্ণবোত্তর', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt niShk`r bOISHNbOtt`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_160, 'proja-sotter-type', 'BnPhonetic', 60, 'rayt niShk`r bOISHNbOtt`r', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt niShk`r bOISHNbOtt`r' and lang = 'BnPhonetic'
    );

    -- code 161
    set @master_id_161 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_161, 'proja-sotter-type', 'En', 61, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI niSHk`rdar debtr' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_161, 'proja-sotter-type', 'Bn', 61, 'মধ্য স্বত্বাধিকারী নিষ্করদার দেবত্র', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI niSHk`rdar debtr' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_161, 'proja-sotter-type', 'BnPhonetic', 61, 'mdhy swtwadhikarI niSHk`rdar debtr', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI niSHk`rdar debtr' and lang = 'BnPhonetic'
    );

    -- code 162
    set @master_id_162 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_162, 'proja-sotter-type', 'En', 62, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN ceragI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_162, 'proja-sotter-type', 'Bn', 62, 'দখলকার চাকরাণ চেরাগী', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN ceragI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_162, 'proja-sotter-type', 'BnPhonetic', 62, 'dkhl`kar cak`raN ceragI', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN ceragI' and lang = 'BnPhonetic'
    );

    -- code 163
    set @master_id_163 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_163, 'proja-sotter-type', 'En', 63, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN pUjarI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_163, 'proja-sotter-type', 'Bn', 63, 'দখলকার চাকরাণ পূজারী', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN pUjarI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_163, 'proja-sotter-type', 'BnPhonetic', 63, 'dkhl`kar cak`raN pUjarI', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN pUjarI' and lang = 'BnPhonetic'
    );

    -- code 164
    set @master_id_164 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_164, 'proja-sotter-type', 'En', 64, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN dh`rrmm`raj' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_164, 'proja-sotter-type', 'Bn', 64, 'দখলকার চাকরাণ ধর্ম্মরাজ', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN dh`rrmm`raj' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_164, 'proja-sotter-type', 'BnPhonetic', 64, 'dkhl`kar cak`raN dh`rrmm`raj', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN dh`rrmm`raj' and lang = 'BnPhonetic'
    );

    -- code 165
    set @master_id_165 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_165, 'proja-sotter-type', 'En', 65, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN kOTal' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_165, 'proja-sotter-type', 'Bn', 65, 'দখলকার চাকরাণ কোটাল', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN kOTal' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_165, 'proja-sotter-type', 'BnPhonetic', 65, 'dkhl`kar cak`raN kOTal', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN kOTal' and lang = 'BnPhonetic'
    );

    -- code 166
    set @master_id_166 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_166, 'proja-sotter-type', 'En', 66, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'kOrrfa cak`raN' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_166, 'proja-sotter-type', 'Bn', 66, 'কোর্ফা চাকরাণ', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'kOrrfa cak`raN' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_166, 'proja-sotter-type', 'BnPhonetic', 66, 'kOrrfa cak`raN', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'kOrrfa cak`raN' and lang = 'BnPhonetic'
    );

    -- code 167
    set @master_id_167 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_167, 'proja-sotter-type', 'En', 67, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN paik' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_167, 'proja-sotter-type', 'Bn', 67, 'দখলকার চাকরাণ পাইক', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN paik' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_167, 'proja-sotter-type', 'BnPhonetic', 67, 'dkhl`kar cak`raN paik', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN paik' and lang = 'BnPhonetic'
    );

    -- code 168
    set @master_id_168 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_168, 'proja-sotter-type', 'En', 68, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar bs`t' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_168, 'proja-sotter-type', 'Bn', 68, 'দখলকার বসত', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar bs`t' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_168, 'proja-sotter-type', 'BnPhonetic', 68, 'dkhl`kar bs`t', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar bs`t' and lang = 'BnPhonetic'
    );

    -- code 169
    set @master_id_169 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_169, 'proja-sotter-type', 'En', 69, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN kham`roi' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_169, 'proja-sotter-type', 'Bn', 69, 'দখলকার চাকরাণ খামরই', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN kham`roi' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_169, 'proja-sotter-type', 'BnPhonetic', 69, 'dkhl`kar cak`raN kham`roi', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN kham`roi' and lang = 'BnPhonetic'
    );

    -- code 170
    set @master_id_170 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_170, 'proja-sotter-type', 'En', 70, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN gOrait' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_170, 'proja-sotter-type', 'Bn', 70, 'দখলকার চাকরাণ গোরাইত', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN gOrait' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_170, 'proja-sotter-type', 'BnPhonetic', 70, 'dkhl`kar cak`raN gOrait', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN gOrait' and lang = 'BnPhonetic'
    );

    -- code 171
    set @master_id_171 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_171, 'proja-sotter-type', 'En', 71, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN cak`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_171, 'proja-sotter-type', 'Bn', 71, 'দখলকার চাকরাণ চাকর', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN cak`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_171, 'proja-sotter-type', 'BnPhonetic', 71, 'dkhl`kar cak`raN cak`r', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN cak`r' and lang = 'BnPhonetic'
    );

    -- code 172
    set @master_id_172 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_172, 'proja-sotter-type', 'En', 72, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r mdhy swtwadhikarI cirsthayI debOtt`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_172, 'proja-sotter-type', 'Bn', 72, 'নিষ্কর মধ্য স্বত্বাধিকারী চিরস্থায়ী দেবোত্তর', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r mdhy swtwadhikarI cirsthayI debOtt`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_172, 'proja-sotter-type', 'BnPhonetic', 72, 'niSHk`r mdhy swtwadhikarI cirsthayI debOtt`r', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r mdhy swtwadhikarI cirsthayI debOtt`r' and lang = 'BnPhonetic'
    );

    -- code 173
    set @master_id_173 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_173, 'proja-sotter-type', 'En', 73, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r mdhy swtwadhikarI cirsthayI brhmOtt`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_173, 'proja-sotter-type', 'Bn', 73, 'নিষ্কর মধ্য স্বত্বাধিকারী চিরস্থায়ী ব্রহ্মোত্তর', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r mdhy swtwadhikarI cirsthayI brhmOtt`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_173, 'proja-sotter-type', 'BnPhonetic', 73, 'niSHk`r mdhy swtwadhikarI cirsthayI brhmOtt`r', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r mdhy swtwadhikarI cirsthayI brhmOtt`r' and lang = 'BnPhonetic'
    );

    -- code 174
    set @master_id_174 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_174, 'proja-sotter-type', 'En', 74, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN behara' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_174, 'proja-sotter-type', 'Bn', 74, 'দখলকার চাকরাণ বেহারা', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN behara' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_174, 'proja-sotter-type', 'BnPhonetic', 74, 'dkhl`kar cak`raN behara', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN behara' and lang = 'BnPhonetic'
    );

    -- code 175
    set @master_id_175 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_175, 'proja-sotter-type', 'En', 75, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt sthitiban niSHk`r pIrOtt`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_175, 'proja-sotter-type', 'Bn', 75, 'রায়ত স্থিতিবান নিষ্কর পীরোত্তর', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt sthitiban niSHk`r pIrOtt`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_175, 'proja-sotter-type', 'BnPhonetic', 75, 'rayt sthitiban niSHk`r pIrOtt`r', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt sthitiban niSHk`r pIrOtt`r' and lang = 'BnPhonetic'
    );

    -- code 176
    set @master_id_176 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_176, 'proja-sotter-type', 'En', 76, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'kOrrfa cak`raN napit' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_176, 'proja-sotter-type', 'Bn', 76, 'কোর্ফা চাকরাণ নাপিত', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'kOrrfa cak`raN napit' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_176, 'proja-sotter-type', 'BnPhonetic', 76, 'kOrrfa cak`raN napit', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'kOrrfa cak`raN napit' and lang = 'BnPhonetic'
    );

    -- code 177
    set @master_id_177 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_177, 'proja-sotter-type', 'En', 77, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt sthitiban niSHk`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_177, 'proja-sotter-type', 'Bn', 77, 'রায়ত স্থিতিবান নিষ্কর', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt sthitiban niSHk`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_177, 'proja-sotter-type', 'BnPhonetic', 77, 'rayt sthitiban niSHk`r', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt sthitiban niSHk`r' and lang = 'BnPhonetic'
    );

    -- code 178
    set @master_id_178 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_178, 'proja-sotter-type', 'En', 78, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar candina' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_178, 'proja-sotter-type', 'Bn', 78, 'দখলকার চান্দিনা', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar candina' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_178, 'proja-sotter-type', 'BnPhonetic', 78, 'dkhl`kar candina', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar candina' and lang = 'BnPhonetic'
    );

    -- code 179
    set @master_id_179 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_179, 'proja-sotter-type', 'En', 79, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'p`ttnI mdhy swtwadhikarI cirsthayI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_179, 'proja-sotter-type', 'Bn', 79, 'পত্তনী মধ্য স্বত্বাধিকারী চিরস্থায়ী', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'p`ttnI mdhy swtwadhikarI cirsthayI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_179, 'proja-sotter-type', 'BnPhonetic', 79, 'p`ttnI mdhy swtwadhikarI cirsthayI', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'p`ttnI mdhy swtwadhikarI cirsthayI' and lang = 'BnPhonetic'
    );

    -- code 180
    set @master_id_180 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_180, 'proja-sotter-type', 'En', 80, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'cak`ran' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_180, 'proja-sotter-type', 'Bn', 80, 'চাকরান', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'cak`ran' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_180, 'proja-sotter-type', 'BnPhonetic', 80, 'cak`ran', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'cak`ran' and lang = 'BnPhonetic'
    );

    -- code 181
    set @master_id_181 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_181, 'proja-sotter-type', 'En', 81, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r rayt sthitiban vOgtt`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_181, 'proja-sotter-type', 'Bn', 81, 'নিষ্কর রায়ত স্থিতিবান ভোগত্তর', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r rayt sthitiban vOgtt`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_181, 'proja-sotter-type', 'BnPhonetic', 81, 'niSHk`r rayt sthitiban vOgtt`r', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r rayt sthitiban vOgtt`r' and lang = 'BnPhonetic'
    );

    -- code 182
    set @master_id_182 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_182, 'proja-sotter-type', 'En', 82, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt sthitiban paTTamule nirikh mOk`rrI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_182, 'proja-sotter-type', 'Bn', 82, 'রায়ত স্থিতিবান পাট্টামুলে নিরিখ মোকররী', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt sthitiban paTTamule nirikh mOk`rrI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_182, 'proja-sotter-type', 'BnPhonetic', 82, 'rayt sthitiban paTTamule nirikh mOk`rrI', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt sthitiban paTTamule nirikh mOk`rrI' and lang = 'BnPhonetic'
    );

    -- code 183
    set @master_id_183 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_183, 'proja-sotter-type', 'En', 83, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN begar' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_183, 'proja-sotter-type', 'Bn', 83, 'দখলকার চাকরাণ বেগার', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN begar' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_183, 'proja-sotter-type', 'BnPhonetic', 83, 'dkhl`kar cak`raN begar', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN begar' and lang = 'BnPhonetic'
    );

    -- code 184
    set @master_id_184 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_184, 'proja-sotter-type', 'En', 84, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN kumvkar' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_184, 'proja-sotter-type', 'Bn', 84, 'দখলকার চাকরাণ কুম্ভকার', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN kumvkar' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_184, 'proja-sotter-type', 'BnPhonetic', 84, 'dkhl`kar cak`raN kumvkar', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN kumvkar' and lang = 'BnPhonetic'
    );

    -- code 185
    set @master_id_185 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_185, 'proja-sotter-type', 'En', 85, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r rayt sthitiban pIrOtt`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_185, 'proja-sotter-type', 'Bn', 85, 'নিষ্কর রায়ত স্থিতিবান পীরোত্তর', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r rayt sthitiban pIrOtt`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_185, 'proja-sotter-type', 'BnPhonetic', 85, 'niSHk`r rayt sthitiban pIrOtt`r', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r rayt sthitiban pIrOtt`r' and lang = 'BnPhonetic'
    );

    -- code 186
    set @master_id_186 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_186, 'proja-sotter-type', 'En', 86, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r mdhy swtwadhikarI cirsthayI brhmtr' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_186, 'proja-sotter-type', 'Bn', 86, 'নিষ্কর মধ্য স্বত্বাধিকারী চিরস্থায়ী ব্রহ্মত্র', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r mdhy swtwadhikarI cirsthayI brhmtr' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_186, 'proja-sotter-type', 'BnPhonetic', 86, 'niSHk`r mdhy swtwadhikarI cirsthayI brhmtr', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r mdhy swtwadhikarI cirsthayI brhmtr' and lang = 'BnPhonetic'
    );

    -- code 187
    set @master_id_187 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_187, 'proja-sotter-type', 'En', 87, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN jhi girir jny' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_187, 'proja-sotter-type', 'Bn', 87, 'দখলকার চাকরাণ ঝি গিরির জন্য', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN jhi girir jny' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_187, 'proja-sotter-type', 'BnPhonetic', 87, 'dkhl`kar cak`raN jhi girir jny', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN jhi girir jny' and lang = 'BnPhonetic'
    );

    -- code 188
    set @master_id_188 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_188, 'proja-sotter-type', 'En', 88, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`ran jhaRudar O alO dan`karI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_188, 'proja-sotter-type', 'Bn', 88, 'দখলকার চাকরান ঝাড়ুদার ও আলো দানকারী', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`ran jhaRudar O alO dan`karI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_188, 'proja-sotter-type', 'BnPhonetic', 88, 'dkhl`kar cak`ran jhaRudar O alO dan`karI', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`ran jhaRudar O alO dan`karI' and lang = 'BnPhonetic'
    );

    -- code 189
    set @master_id_189 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_189, 'proja-sotter-type', 'En', 89, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`ran purOhit' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_189, 'proja-sotter-type', 'Bn', 89, 'দখলকার চাকরান পুরোহিত', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`ran purOhit' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_189, 'proja-sotter-type', 'BnPhonetic', 89, 'dkhl`kar cak`ran purOhit', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`ran purOhit' and lang = 'BnPhonetic'
    );

    -- code 190
    set @master_id_190 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_190, 'proja-sotter-type', 'En', 90, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'sep`ttnI mdhy swtwadhikarI cirsthayI mOk`rrI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_190, 'proja-sotter-type', 'Bn', 90, 'সেপত্তনী মধ্য স্বত্বাধিকারী চিরস্থায়ী মোকররী', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'sep`ttnI mdhy swtwadhikarI cirsthayI mOk`rrI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_190, 'proja-sotter-type', 'BnPhonetic', 90, 'sep`ttnI mdhy swtwadhikarI cirsthayI mOk`rrI', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'sep`ttnI mdhy swtwadhikarI cirsthayI mOk`rrI' and lang = 'BnPhonetic'
    );

    -- code 191
    set @master_id_191 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_191, 'proja-sotter-type', 'En', 91, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN kkhOUr karrzyer jny' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_191, 'proja-sotter-type', 'Bn', 91, 'দখলকার চাকরাণ ক্ষৌর কার্য্যের জন্য', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN kkhOUr karrzyer jny' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_191, 'proja-sotter-type', 'BnPhonetic', 91, 'dkhl`kar cak`raN kkhOUr karrzyer jny', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN kkhOUr karrzyer jny' and lang = 'BnPhonetic'
    );

    -- code 192
    set @master_id_192 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_192, 'proja-sotter-type', 'En', 92, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI niShk`rdar bOISHNbOtt`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_192, 'proja-sotter-type', 'Bn', 92, 'মধ্য স্বত্বাধিকারী নিষ্করদার বৈষ্ণবোত্তর', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI niShk`rdar bOISHNbOtt`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_192, 'proja-sotter-type', 'BnPhonetic', 92, 'mdhy swtwadhikarI niShk`rdar bOISHNbOtt`r', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI niShk`rdar bOISHNbOtt`r' and lang = 'BnPhonetic'
    );

    -- code 193
    set @master_id_193 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_193, 'proja-sotter-type', 'En', 93, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN gh`ramI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_193, 'proja-sotter-type', 'Bn', 93, 'দখলকার চাকরাণ ঘরামী', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN gh`ramI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_193, 'proja-sotter-type', 'BnPhonetic', 93, 'dkhl`kar cak`raN gh`ramI', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN gh`ramI' and lang = 'BnPhonetic'
    );

    -- code 194
    set @master_id_194 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_194, 'proja-sotter-type', 'En', 94, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI osthayI khajanabrriddhir zOgy' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_194, 'proja-sotter-type', 'Bn', 94, 'মধ্যস্বত্বাধিকারী অস্থায়ী খাজানাবৃদ্ধির যোগ্য', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI osthayI khajanabrriddhir zOgy' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_194, 'proja-sotter-type', 'BnPhonetic', 94, 'mdhyswtwadhikarI osthayI khajanabrriddhir zOgy', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI osthayI khajanabrriddhir zOgy' and lang = 'BnPhonetic'
    );

    -- code 195
    set @master_id_195 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_195, 'proja-sotter-type', 'En', 95, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'cak`raN mdhyswtwadhikarI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_195, 'proja-sotter-type', 'Bn', 95, 'চাকরাণ মধ্যস্বত্বাধিকারী', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'cak`raN mdhyswtwadhikarI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_195, 'proja-sotter-type', 'BnPhonetic', 95, 'cak`raN mdhyswtwadhikarI', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'cak`raN mdhyswtwadhikarI' and lang = 'BnPhonetic'
    );

    -- code 196
    set @master_id_196 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_196, 'proja-sotter-type', 'En', 96, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt prthanusareSUny' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_196, 'proja-sotter-type', 'Bn', 96, 'রায়ত প্রথানুসারেশূন্য', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt prthanusareSUny' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_196, 'proja-sotter-type', 'BnPhonetic', 96, 'rayt prthanusareSUny', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt prthanusareSUny' and lang = 'BnPhonetic'
    );

    -- code 197
    set @master_id_197 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_197, 'proja-sotter-type', 'En', 97, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dokhol`kar cak`ran khlifa' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_197, 'proja-sotter-type', 'Bn', 97, 'দখলকার চাকরান খলিফা', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dokhol`kar cak`ran khlifa' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_197, 'proja-sotter-type', 'BnPhonetic', 97, 'dokhol`kar cak`ran khlifa', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dokhol`kar cak`ran khlifa' and lang = 'BnPhonetic'
    );

    -- code 198
    set @master_id_198 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_198, 'proja-sotter-type', 'En', 98, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dokhol`kar cak`ran sorrddar' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_198, 'proja-sotter-type', 'Bn', 98, 'দখলকার চাকরান সর্দ্দার', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dokhol`kar cak`ran sorrddar' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_198, 'proja-sotter-type', 'BnPhonetic', 98, 'dokhol`kar cak`ran sorrddar', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dokhol`kar cak`ran sorrddar' and lang = 'BnPhonetic'
    );

    -- code 199
    set @master_id_199 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_199, 'proja-sotter-type', 'En', 99, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r rayt sthitiban brhmtr' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_199, 'proja-sotter-type', 'Bn', 99, 'নিষ্কর রায়ত স্থিতিবান ব্রহ্মত্র', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r rayt sthitiban brhmtr' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_199, 'proja-sotter-type', 'BnPhonetic', 99, 'niSHk`r rayt sthitiban brhmtr', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r rayt sthitiban brhmtr' and lang = 'BnPhonetic'
    );

    -- code 200
    set @master_id_200 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_200, 'proja-sotter-type', 'En', 100, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI osthayI myadI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_200, 'proja-sotter-type', 'Bn', 100, 'মধ্যস্বত্বাধিকারী অস্থায়ী ম্যাদী', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI osthayI myadI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_200, 'proja-sotter-type', 'BnPhonetic', 100, 'mdhyswtwadhikarI osthayI myadI', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI osthayI myadI' and lang = 'BnPhonetic'
    );

    -- code 201
    set @master_id_201 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_201, 'proja-sotter-type', 'En', 101, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r mdhy swtwadhikarI cirsthayI bOISHNbOttor' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_201, 'proja-sotter-type', 'Bn', 101, 'নিষ্কর মধ্য স্বত্বাধিকারী চিরস্থায়ী বৈষ্ণবোত্তর', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r mdhy swtwadhikarI cirsthayI bOISHNbOttor' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_201, 'proja-sotter-type', 'BnPhonetic', 101, 'niSHk`r mdhy swtwadhikarI cirsthayI bOISHNbOttor', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r mdhy swtwadhikarI cirsthayI bOISHNbOttor' and lang = 'BnPhonetic'
    );

    -- code 202
    set @master_id_202 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_202, 'proja-sotter-type', 'En', 102, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'modhy swtwadhikarI cir`sthayI niSHk`r pIrOttor' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_202, 'proja-sotter-type', 'Bn', 102, 'মধ্য স্বত্বাধিকারী চিরস্থায়ী নিষ্কর পীরোত্তর', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'modhy swtwadhikarI cir`sthayI niSHk`r pIrOttor' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_202, 'proja-sotter-type', 'BnPhonetic', 102, 'modhy swtwadhikarI cir`sthayI niSHk`r pIrOttor', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'modhy swtwadhikarI cir`sthayI niSHk`r pIrOttor' and lang = 'BnPhonetic'
    );

    -- code 203
    set @master_id_203 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_203, 'proja-sotter-type', 'En', 103, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'cak`raN rayt' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_203, 'proja-sotter-type', 'Bn', 103, 'চাকরাণ রায়ত', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'cak`raN rayt' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_203, 'proja-sotter-type', 'BnPhonetic', 103, 'cak`raN rayt', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'cak`raN rayt' and lang = 'BnPhonetic'
    );

    -- code 204
    set @master_id_204 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_204, 'proja-sotter-type', 'En', 104, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r dokhl`kar' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_204, 'proja-sotter-type', 'Bn', 104, 'নিষ্কর দখলকার', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r dokhl`kar' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_204, 'proja-sotter-type', 'BnPhonetic', 104, 'niSHk`r dokhl`kar', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r dokhl`kar' and lang = 'BnPhonetic'
    );

    -- code 205
    set @master_id_205 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_205, 'proja-sotter-type', 'En', 105, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN dhOpa' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_205, 'proja-sotter-type', 'Bn', 105, 'দখলকার চাকরাণ ধোপা', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN dhOpa' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_205, 'proja-sotter-type', 'BnPhonetic', 105, 'dkhl`kar cak`raN dhOpa', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN dhOpa' and lang = 'BnPhonetic'
    );

    -- code 206
    set @master_id_206 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_206, 'proja-sotter-type', 'En', 106, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dokhol`kar cak`raN methor' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_206, 'proja-sotter-type', 'Bn', 106, 'দখলকার চাকরাণ মেথর', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dokhol`kar cak`raN methor' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_206, 'proja-sotter-type', 'BnPhonetic', 106, 'dokhol`kar cak`raN methor', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dokhol`kar cak`raN methor' and lang = 'BnPhonetic'
    );

    -- code 207
    set @master_id_207 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_207, 'proja-sotter-type', 'En', 107, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI cirsthayI nisk`r brHMttOr' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_207, 'proja-sotter-type', 'Bn', 107, 'মধ্য স্বত্বাধিকারী চিরস্থায়ী নিস্কর ব্রহ্মত্তোর', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI cirsthayI nisk`r brHMttOr' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_207, 'proja-sotter-type', 'BnPhonetic', 107, 'mdhy swtwadhikarI cirsthayI nisk`r brHMttOr', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI cirsthayI nisk`r brHMttOr' and lang = 'BnPhonetic'
    );

    -- code 208
    set @master_id_208 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_208, 'proja-sotter-type', 'En', 108, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt niSHk`r pirOtt`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_208, 'proja-sotter-type', 'Bn', 108, 'রায়ত নিষ্কর পিরোত্তর', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt niSHk`r pirOtt`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_208, 'proja-sotter-type', 'BnPhonetic', 108, 'rayt niSHk`r pirOtt`r', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt niSHk`r pirOtt`r' and lang = 'BnPhonetic'
    );

    -- code 209
    set @master_id_209 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_209, 'proja-sotter-type', 'En', 109, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI niSHk`rdar pirOttor' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_209, 'proja-sotter-type', 'Bn', 109, 'মধ্য স্বত্বাধিকারী নিষ্করদার পিরোত্তর', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI niSHk`rdar pirOttor' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_209, 'proja-sotter-type', 'BnPhonetic', 109, 'mdhy swtwadhikarI niSHk`rdar pirOttor', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI niSHk`rdar pirOttor' and lang = 'BnPhonetic'
    );

    -- code 210
    set @master_id_210 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_210, 'proja-sotter-type', 'En', 110, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI niSHk`rdar SibOttor' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_210, 'proja-sotter-type', 'Bn', 110, 'মধ্য স্বত্বাধিকারী নিষ্করদার শিবোত্তর', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI niSHk`rdar SibOttor' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_210, 'proja-sotter-type', 'BnPhonetic', 110, 'mdhy swtwadhikarI niSHk`rdar SibOttor', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI niSHk`rdar SibOttor' and lang = 'BnPhonetic'
    );

    -- code 211
    set @master_id_211 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_211, 'proja-sotter-type', 'En', 111, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt niSHk`r SibOtwr' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_211, 'proja-sotter-type', 'Bn', 111, 'রায়ত নিষ্কর শিবোত্বর', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt niSHk`r SibOtwr' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_211, 'proja-sotter-type', 'BnPhonetic', 111, 'rayt niSHk`r SibOtwr', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt niSHk`r SibOtwr' and lang = 'BnPhonetic'
    );

    -- code 212
    set @master_id_212 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_212, 'proja-sotter-type', 'En', 112, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI nisk`rdar pirpal' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_212, 'proja-sotter-type', 'Bn', 112, 'মধ্য স্বত্বাধিকারী নিস্করদার পিরপাল', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI nisk`rdar pirpal' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_212, 'proja-sotter-type', 'BnPhonetic', 112, 'mdhy swtwadhikarI nisk`rdar pirpal', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI nisk`rdar pirpal' and lang = 'BnPhonetic'
    );

    -- code 213
    set @master_id_213 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_213, 'proja-sotter-type', 'En', 113, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar pukur' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_213, 'proja-sotter-type', 'Bn', 113, 'দখলকার পুকুর', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar pukur' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_213, 'proja-sotter-type', 'BnPhonetic', 113, 'dkhl`kar pukur', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar pukur' and lang = 'BnPhonetic'
    );

    -- code 214
    set @master_id_214 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_214, 'proja-sotter-type', 'En', 114, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dokhol`kar cak`ran puSHPcoyon' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_214, 'proja-sotter-type', 'Bn', 114, 'দখলকার চাকরান পুষ্পচয়ন', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dokhol`kar cak`ran puSHPcoyon' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_214, 'proja-sotter-type', 'BnPhonetic', 114, 'dokhol`kar cak`ran puSHPcoyon', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dokhol`kar cak`ran puSHPcoyon' and lang = 'BnPhonetic'
    );

    -- code 215
    set @master_id_215 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_215, 'proja-sotter-type', 'En', 115, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN cak`raNI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_215, 'proja-sotter-type', 'Bn', 115, 'দখলকার চাকরাণ চাকরাণী', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN cak`raNI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_215, 'proja-sotter-type', 'BnPhonetic', 115, 'dkhl`kar cak`raN cak`raNI', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN cak`raNI' and lang = 'BnPhonetic'
    );

    -- code 216
    set @master_id_216 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_216, 'proja-sotter-type', 'En', 116, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN badykor' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_216, 'proja-sotter-type', 'Bn', 116, 'দখলকার চাকরাণ বাদ্যকর', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN badykor' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_216, 'proja-sotter-type', 'BnPhonetic', 116, 'dkhl`kar cak`raN badykor', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN badykor' and lang = 'BnPhonetic'
    );

    -- code 217
    set @master_id_217 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_217, 'proja-sotter-type', 'En', 117, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dokhol`kar cak`raN sutrdhor' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_217, 'proja-sotter-type', 'Bn', 117, 'দখলকার চাকরাণ সুত্রধর', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dokhol`kar cak`raN sutrdhor' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_217, 'proja-sotter-type', 'BnPhonetic', 117, 'dokhol`kar cak`raN sutrdhor', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dokhol`kar cak`raN sutrdhor' and lang = 'BnPhonetic'
    );

    -- code 218
    set @master_id_218 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_218, 'proja-sotter-type', 'En', 118, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dokhol`kar cak`ran poricarok' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_218, 'proja-sotter-type', 'Bn', 118, 'দখলকার চাকরান পরিচারক', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dokhol`kar cak`ran poricarok' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_218, 'proja-sotter-type', 'BnPhonetic', 118, 'dokhol`kar cak`ran poricarok', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dokhol`kar cak`ran poricarok' and lang = 'BnPhonetic'
    );

    -- code 219
    set @master_id_219 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_219, 'proja-sotter-type', 'En', 119, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'sodor p`ttni mdhy swtwadhikarI cirsthayI mOk`rrI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_219, 'proja-sotter-type', 'Bn', 119, 'সদর পত্তনি মধ্য স্বত্বাধিকারী চিরস্থায়ী মোকররী', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'sodor p`ttni mdhy swtwadhikarI cirsthayI mOk`rrI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_219, 'proja-sotter-type', 'BnPhonetic', 119, 'sodor p`ttni mdhy swtwadhikarI cirsthayI mOk`rrI', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'sodor p`ttni mdhy swtwadhikarI cirsthayI mOk`rrI' and lang = 'BnPhonetic'
    );

    -- code 220
    set @master_id_220 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_220, 'proja-sotter-type', 'En', 120, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI cirsthayI myadI myad mdhye khajanabrriddhirzOgy' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_220, 'proja-sotter-type', 'Bn', 120, 'মধ্য স্বত্বাধিকারী চিরস্থায়ী ম্যাদী ম্যাদ মধ্যে খাজানাবৃদ্ধিরযোগ্য', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI cirsthayI myadI myad mdhye khajanabrriddhirzOgy' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_220, 'proja-sotter-type', 'BnPhonetic', 120, 'mdhy swtwadhikarI cirsthayI myadI myad mdhye khajanabrriddhirzOgy', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI cirsthayI myadI myad mdhye khajanabrriddhirzOgy' and lang = 'BnPhonetic'
    );

    -- code 221
    set @master_id_221 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_221, 'proja-sotter-type', 'En', 121, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`ran' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_221, 'proja-sotter-type', 'Bn', 121, 'দখলকার চাকরান', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`ran' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_221, 'proja-sotter-type', 'BnPhonetic', 121, 'dkhl`kar cak`ran', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`ran' and lang = 'BnPhonetic'
    );

    -- code 222
    set @master_id_222 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_222, 'proja-sotter-type', 'En', 122, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'pottnI mdhy swtwadhikarI cirsthayI mOk`rrI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_222, 'proja-sotter-type', 'Bn', 122, 'পত্তনি মধ্য স্বত্বাধিকারী চিরস্থায়ী মোকররী', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'pottnI mdhy swtwadhikarI cirsthayI mOk`rrI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_222, 'proja-sotter-type', 'BnPhonetic', 122, 'pottnI mdhy swtwadhikarI cirsthayI mOk`rrI', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'pottnI mdhy swtwadhikarI cirsthayI mOk`rrI' and lang = 'BnPhonetic'
    );

    -- code 223
    set @master_id_223 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_223, 'proja-sotter-type', 'En', 123, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI myadI myad mdhye khajana brriddhir ozOgy' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_223, 'proja-sotter-type', 'Bn', 123, 'মধ্য স্বত্বাধিকারী ম্যাদী ম্যাদ মধ্যে খাজানা বৃদ্ধির অযোগ্য', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI myadI myad mdhye khajana brriddhir ozOgy' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_223, 'proja-sotter-type', 'BnPhonetic', 123, 'mdhy swtwadhikarI myadI myad mdhye khajana brriddhir ozOgy', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI myadI myad mdhye khajana brriddhir ozOgy' and lang = 'BnPhonetic'
    );

    -- code 224
    set @master_id_224 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_224, 'proja-sotter-type', 'En', 124, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'kOrrfa nisk`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_224, 'proja-sotter-type', 'Bn', 124, 'কোর্ফা নিস্কর', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'kOrrfa nisk`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_224, 'proja-sotter-type', 'BnPhonetic', 124, 'kOrrfa nisk`r', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'kOrrfa nisk`r' and lang = 'BnPhonetic'
    );

    -- code 225
    set @master_id_225 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_225, 'proja-sotter-type', 'En', 125, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt myadI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_225, 'proja-sotter-type', 'Bn', 125, 'রায়ত ম্যাদী', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt myadI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_225, 'proja-sotter-type', 'BnPhonetic', 125, 'rayt myadI', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt myadI' and lang = 'BnPhonetic'
    );

    -- code 226
    set @master_id_226 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_226, 'proja-sotter-type', 'En', 126, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'kOrrfa dkhliswtwbiSiSHT niSHk`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_226, 'proja-sotter-type', 'Bn', 126, 'কোর্ফা দখলিস্বত্ববিশিষ্ট নিষ্কর', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'kOrrfa dkhliswtwbiSiSHT niSHk`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_226, 'proja-sotter-type', 'BnPhonetic', 126, 'kOrrfa dkhliswtwbiSiSHT niSHk`r', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'kOrrfa dkhliswtwbiSiSHT niSHk`r' and lang = 'BnPhonetic'
    );

    -- code 227
    set @master_id_227 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_227, 'proja-sotter-type', 'En', 127, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'kOrrfa dkhliswtw' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_227, 'proja-sotter-type', 'Bn', 127, 'কোর্ফা দখলিস্বত্ব', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'kOrrfa dkhliswtw' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_227, 'proja-sotter-type', 'BnPhonetic', 127, 'kOrrfa dkhliswtw', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'kOrrfa dkhliswtw' and lang = 'BnPhonetic'
    );

    -- code 228
    set @master_id_228 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_228, 'proja-sotter-type', 'En', 128, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI cak`raN' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_228, 'proja-sotter-type', 'Bn', 128, 'মধ্য স্বত্বাধিকারী চাকরাণ', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI cak`raN' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_228, 'proja-sotter-type', 'BnPhonetic', 128, 'mdhy swtwadhikarI cak`raN', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI cak`raN' and lang = 'BnPhonetic'
    );

    -- code 229
    set @master_id_229 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_229, 'proja-sotter-type', 'En', 129, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r mdhy swtwadhikarI cirsthayI nhe' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_229, 'proja-sotter-type', 'Bn', 129, 'নিষ্কর মধ্য স্বত্বাধিকারী চিরস্থায়ী নহে', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r mdhy swtwadhikarI cirsthayI nhe' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_229, 'proja-sotter-type', 'BnPhonetic', 129, 'niSHk`r mdhy swtwadhikarI cirsthayI nhe', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r mdhy swtwadhikarI cirsthayI nhe' and lang = 'BnPhonetic'
    );

    -- code 230
    set @master_id_230 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_230, 'proja-sotter-type', 'En', 130, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r mdhy swtwadhikarI cirsthayI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_230, 'proja-sotter-type', 'Bn', 130, 'নিষ্কর মধ্য স্বত্বাধিকারী চিরস্থায়ী', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r mdhy swtwadhikarI cirsthayI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_230, 'proja-sotter-type', 'BnPhonetic', 130, 'niSHk`r mdhy swtwadhikarI cirsthayI', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r mdhy swtwadhikarI cirsthayI' and lang = 'BnPhonetic'
    );

    -- code 231
    set @master_id_231 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_231, 'proja-sotter-type', 'En', 131, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI niSHk`r debtr' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_231, 'proja-sotter-type', 'Bn', 131, 'মধ্যস্বত্বাধিকারী চিরস্থায়ী নিষ্কর দেবত্র', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI niSHk`r debtr' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_231, 'proja-sotter-type', 'BnPhonetic', 131, 'mdhyswtwadhikarI cirsthayI niSHk`r debtr', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI niSHk`r debtr' and lang = 'BnPhonetic'
    );

    -- code 232
    set @master_id_232 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_232, 'proja-sotter-type', 'En', 132, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI niSHk`r brhmtr' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_232, 'proja-sotter-type', 'Bn', 132, 'মধ্যস্বত্বাধিকারী চিরস্থায়ী নিষ্কর ব্রহ্মত্র', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI niSHk`r brhmtr' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_232, 'proja-sotter-type', 'BnPhonetic', 132, 'mdhyswtwadhikarI cirsthayI niSHk`r brhmtr', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI niSHk`r brhmtr' and lang = 'BnPhonetic'
    );

    -- code 233
    set @master_id_233 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_233, 'proja-sotter-type', 'En', 133, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`ran mNDl' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_233, 'proja-sotter-type', 'Bn', 133, 'দখলকার চাকরান মণ্ডল', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`ran mNDl' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_233, 'proja-sotter-type', 'BnPhonetic', 133, 'dkhl`kar cak`ran mNDl', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`ran mNDl' and lang = 'BnPhonetic'
    );

    -- code 234
    set @master_id_234 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_234, 'proja-sotter-type', 'En', 134, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r mdhy swtwadhikarI cirsthayI debtr' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_234, 'proja-sotter-type', 'Bn', 134, 'নিষ্কর মধ্য স্বত্বাধিকারী চিরস্থায়ী দেবত্র', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r mdhy swtwadhikarI cirsthayI debtr' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_234, 'proja-sotter-type', 'BnPhonetic', 134, 'niSHk`r mdhy swtwadhikarI cirsthayI debtr', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r mdhy swtwadhikarI cirsthayI debtr' and lang = 'BnPhonetic'
    );

    -- code 235
    set @master_id_235 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_235, 'proja-sotter-type', 'En', 135, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r mdhy swtwadhikarI cirsthayI pirOttor' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_235, 'proja-sotter-type', 'Bn', 135, 'নিষ্কর মধ্য স্বত্বাধিকারী চিরস্থায়ী পিরোত্তর', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r mdhy swtwadhikarI cirsthayI pirOttor' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_235, 'proja-sotter-type', 'BnPhonetic', 135, 'niSHk`r mdhy swtwadhikarI cirsthayI pirOttor', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r mdhy swtwadhikarI cirsthayI pirOttor' and lang = 'BnPhonetic'
    );

    -- code 236
    set @master_id_236 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_236, 'proja-sotter-type', 'En', 136, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt sthitiban debtr' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_236, 'proja-sotter-type', 'Bn', 136, 'রায়ত স্থিতিবান দেবত্র', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt sthitiban debtr' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_236, 'proja-sotter-type', 'BnPhonetic', 136, 'rayt sthitiban debtr', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt sthitiban debtr' and lang = 'BnPhonetic'
    ); 

    -- code 237
    set @master_id_237 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_237, 'proja-sotter-type', 'En', 137, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r rayt sthitiban' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_237, 'proja-sotter-type', 'Bn', 137, 'নিষ্কর রায়ত স্থিতিবান', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r rayt sthitiban' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_237, 'proja-sotter-type', 'BnPhonetic', 137, 'niSHk`r rayt sthitiban', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r rayt sthitiban' and lang = 'BnPhonetic'
    );

    -- code 238
    set @master_id_238 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_238, 'proja-sotter-type', 'En', 138, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r mdhy swtwadhikarI cirsthayI mhatraN' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_238, 'proja-sotter-type', 'Bn', 138, 'নিষ্কর মধ্য স্বত্বাধিকারী চিরস্থায়ী মহাত্রাণ', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r mdhy swtwadhikarI cirsthayI mhatraN' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_238, 'proja-sotter-type', 'BnPhonetic', 138, 'niSHk`r mdhy swtwadhikarI cirsthayI mhatraN', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r mdhy swtwadhikarI cirsthayI mhatraN' and lang = 'BnPhonetic'
    );

    -- code 239
    set @master_id_239 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_239, 'proja-sotter-type', 'En', 139, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`ran begarI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_239, 'proja-sotter-type', 'Bn', 139, 'দখলকার চাকরান বেগারী', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`ran begarI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_239, 'proja-sotter-type', 'BnPhonetic', 139, 'dkhl`kar cak`ran begarI', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cak`ran begarI' and lang = 'BnPhonetic'
    );

    -- code 240
    set @master_id_240 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_240, 'proja-sotter-type', 'En', 140, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r rayt sthitiban debOtt`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_240, 'proja-sotter-type', 'Bn', 140, 'নিষ্কর রায়ত স্থিতিবান দেবোত্তর', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r rayt sthitiban debOtt`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_240, 'proja-sotter-type', 'BnPhonetic', 140, 'niSHk`r rayt sthitiban debOtt`r', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r rayt sthitiban debOtt`r' and lang = 'BnPhonetic'
    );

    -- code 241
    set @master_id_241 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_241, 'proja-sotter-type', 'En', 141, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt sthitiban kbuliyt mule mOk`rrI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_241, 'proja-sotter-type', 'Bn', 141, 'রায়ত স্থিতিবান কবুলিয়ত মুলে মোকররী', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt sthitiban kbuliyt mule mOk`rrI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_241, 'proja-sotter-type', 'BnPhonetic', 141, 'rayt sthitiban kbuliyt mule mOk`rrI', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt sthitiban kbuliyt mule mOk`rrI' and lang = 'BnPhonetic'
    );

    -- code 242
    set @master_id_242 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_242, 'proja-sotter-type', 'En', 142, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r mdhy swtwadhikarI cirsthayI bOIdyOtt`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_242, 'proja-sotter-type', 'Bn', 142, 'নিষ্কর মধ্য স্বত্বাধিকারী চিরস্থায়ী বৈদ্যোত্তর', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r mdhy swtwadhikarI cirsthayI bOIdyOtt`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_242, 'proja-sotter-type', 'BnPhonetic', 142, 'niSHk`r mdhy swtwadhikarI cirsthayI bOIdyOtt`r', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r mdhy swtwadhikarI cirsthayI bOIdyOtt`r' and lang = 'BnPhonetic'
    );

    -- code 243
    set @master_id_243 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_243, 'proja-sotter-type', 'En', 143, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt sthitiban paTTamUle mOk`rrI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_243, 'proja-sotter-type', 'Bn', 143, 'রায়ত স্থিতিবান পাট্টামূলে মোকররী', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt sthitiban paTTamUle mOk`rrI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_243, 'proja-sotter-type', 'BnPhonetic', 143, 'rayt sthitiban paTTamUle mOk`rrI', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt sthitiban paTTamUle mOk`rrI' and lang = 'BnPhonetic'
    );

    -- code 244
    set @master_id_244 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_244, 'proja-sotter-type', 'En', 144, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r rayt sthitiban bOISHNbOttor' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_244, 'proja-sotter-type', 'Bn', 144, 'নিষ্কর রায়ত স্থিতিবান বৈষ্ণবোত্তর', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r rayt sthitiban bOISHNbOttor' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_244, 'proja-sotter-type', 'BnPhonetic', 144, 'niSHk`r rayt sthitiban bOISHNbOttor', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r rayt sthitiban bOISHNbOttor' and lang = 'BnPhonetic'
    );

    -- code 245
    set @master_id_245 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_245, 'proja-sotter-type', 'En', 145, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r rayt sthitiban vTTOtt`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_245, 'proja-sotter-type', 'Bn', 145, 'নিষ্কর রায়ত স্থিতিবান ভট্টোত্তর', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r rayt sthitiban vTTOtt`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_245, 'proja-sotter-type', 'BnPhonetic', 145, 'niSHk`r rayt sthitiban vTTOtt`r', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r rayt sthitiban vTTOtt`r' and lang = 'BnPhonetic'
    );

    -- code 246
    set @master_id_246 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_246, 'proja-sotter-type', 'En', 146, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt sthitiban brHMOtt`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_246, 'proja-sotter-type', 'Bn', 146, 'রায়ত স্থিতিবান ব্রহ্মোত্তর', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt sthitiban brHMOtt`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_246, 'proja-sotter-type', 'BnPhonetic', 146, 'rayt sthitiban brHMOtt`r', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt sthitiban brHMOtt`r' and lang = 'BnPhonetic'
    );

    -- code 247
    set @master_id_247 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_247, 'proja-sotter-type', 'En', 147, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI cirsthayI niSHk`r brhmtr' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_247, 'proja-sotter-type', 'Bn', 147, 'মধ্য স্বত্বাধিকারী চিরস্থায়ী নিষ্কর ব্রহ্মত্র', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI cirsthayI niSHk`r brhmtr' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_247, 'proja-sotter-type', 'BnPhonetic', 147, 'mdhy swtwadhikarI cirsthayI niSHk`r brhmtr', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI cirsthayI niSHk`r brhmtr' and lang = 'BnPhonetic'
    );

    -- code 248
    set @master_id_248 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_248, 'proja-sotter-type', 'En', 148, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r rayt sthitiban brHMOtt`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_248, 'proja-sotter-type', 'Bn', 148, 'নিষ্কর রায়ত স্থিতিবান ব্রহ্মোত্তর', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r rayt sthitiban brHMOtt`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_248, 'proja-sotter-type', 'BnPhonetic', 148, 'niSHk`r rayt sthitiban brHMOtt`r', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r rayt sthitiban brHMOtt`r' and lang = 'BnPhonetic'
    );

    -- code 249
    set @master_id_249 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_249, 'proja-sotter-type', 'En', 149, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'pottnI mdhyswtwadhikarI cirsthayI khajana brriddhir zOgy' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_249, 'proja-sotter-type', 'Bn', 149, 'পত্তনি মধ্যস্বত্বাধিকারী চিরস্থায়ী খাজানা বৃদ্ধির যোগ্য', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'pottnI mdhyswtwadhikarI cirsthayI khajana brriddhir zOgy' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_249, 'proja-sotter-type', 'BnPhonetic', 149, 'pottnI mdhyswtwadhikarI cirsthayI khajana brriddhir zOgy', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'pottnI mdhyswtwadhikarI cirsthayI khajana brriddhir zOgy' and lang = 'BnPhonetic'
    );

    -- code 250
    set @master_id_250 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_250, 'proja-sotter-type', 'En', 150, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI cirsthayI n`he myad mdhye khajana brriddhir ozOgy' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_250, 'proja-sotter-type', 'Bn', 150, 'মধ্য স্বত্বাধিকারী চিরস্থায়ী নহে ম্যাদ মধ্যে খাজানা বৃদ্ধির অযোগ্য', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI cirsthayI n`he myad mdhye khajana brriddhir ozOgy' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_250, 'proja-sotter-type', 'BnPhonetic', 150, 'mdhy swtwadhikarI cirsthayI n`he myad mdhye khajana brriddhir ozOgy', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhy swtwadhikarI cirsthayI n`he myad mdhye khajana brriddhir ozOgy' and lang = 'BnPhonetic'
    );

    -- code 251
    set @master_id_251 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_251, 'proja-sotter-type', 'En', 151, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI n`he meyad`mdhyekhajanabrriddhir ozOgy' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_251, 'proja-sotter-type', 'Bn', 151, 'মধ্যস্বত্বাধিকারী চিরস্থায়ী নহে মেয়াদমধ্যেখাজানাবৃদ্ধির অযোগ্য', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI n`he meyad`mdhyekhajanabrriddhir ozOgy' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_251, 'proja-sotter-type', 'BnPhonetic', 151, 'mdhyswtwadhikarI cirsthayI n`he meyad`mdhyekhajanabrriddhir ozOgy', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI n`he meyad`mdhyekhajanabrriddhir ozOgy' and lang = 'BnPhonetic'
    );

    -- code 252
    set @master_id_252 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_252, 'proja-sotter-type', 'En', 152, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtw cirsthayI mOk`rrI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_252, 'proja-sotter-type', 'Bn', 152, 'মধ্যস্বত্ব চিরস্থায়ী মোকররী', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtw cirsthayI mOk`rrI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_252, 'proja-sotter-type', 'BnPhonetic', 152, 'mdhyswtw cirsthayI mOk`rrI', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtw cirsthayI mOk`rrI' and lang = 'BnPhonetic'
    );

    -- code 253
    set @master_id_253 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_253, 'proja-sotter-type', 'En', 153, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtw cirsthayI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_253, 'proja-sotter-type', 'Bn', 153, 'মধ্যস্বত্ব চিরস্থায়ী', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtw cirsthayI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_253, 'proja-sotter-type', 'BnPhonetic', 153, 'mdhyswtw cirsthayI', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtw cirsthayI' and lang = 'BnPhonetic'
    );

    -- code 254
    set @master_id_254 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_254, 'proja-sotter-type', 'En', 154, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtw cirsthayI khajana brriddhir zOgy' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_254, 'proja-sotter-type', 'Bn', 154, 'মধ্যস্বত্ব চিরস্থায়ী খাজানা বৃদ্ধির যোগ্য', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtw cirsthayI khajana brriddhir zOgy' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_254, 'proja-sotter-type', 'BnPhonetic', 154, 'mdhyswtw cirsthayI khajana brriddhir zOgy', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtw cirsthayI khajana brriddhir zOgy' and lang = 'BnPhonetic'
    );

    -- code 255
    set @master_id_255 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_255, 'proja-sotter-type', 'En', 155, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'malikI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_255, 'proja-sotter-type', 'Bn', 155, 'মালিকী', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'malikI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_255, 'proja-sotter-type', 'BnPhonetic', 155, 'malikI', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'malikI' and lang = 'BnPhonetic'
    );

    -- code 256
    set @master_id_256 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_256, 'proja-sotter-type', 'En', 156, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r mdhy swtwadhikarI cirsthayI SibOtt`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_256, 'proja-sotter-type', 'Bn', 156, 'নিষ্কর মধ্য স্বত্বাধিকারী চিরস্থায়ী শিবোত্তর', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r mdhy swtwadhikarI cirsthayI SibOtt`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_256, 'proja-sotter-type', 'BnPhonetic', 156, 'niSHk`r mdhy swtwadhikarI cirsthayI SibOtt`r', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'niSHk`r mdhy swtwadhikarI cirsthayI SibOtt`r' and lang = 'BnPhonetic'
    );

    -- code 257
    set @master_id_257 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_257, 'proja-sotter-type', 'En', 157, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt sthitiban dakhilay swIkrrit mule mOk`rrI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_257, 'proja-sotter-type', 'Bn', 157, 'রায়ত স্থিতিবান দাখিলায় স্বীকৃত মুলে মোকররী', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt sthitiban dakhilay swIkrrit mule mOk`rrI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_257, 'proja-sotter-type', 'BnPhonetic', 157, 'rayt sthitiban dakhilay swIkrrit mule mOk`rrI', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt sthitiban dakhilay swIkrrit mule mOk`rrI' and lang = 'BnPhonetic'
    );

     -- code 258
     set @master_id_258 = UNHEX(REPLACE(uuid(), '-', ''));
     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_258, 'proja-sotter-type', 'En', 158, '', @parent_master_id
     from dual
      where not exists ( 
     select 1 from metadata_master_cs where val = 'rayti dkhlIswtwbiSiSHT' and lang = 'BnPhonetic'
     );

     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_258, 'proja-sotter-type', 'Bn', 158, 'রায়তি দখলীস্বত্ববিশিষ্ট', @parent_master_id
     from dual
      where not exists (
     select 1 from metadata_master_cs where val = 'rayti dkhlIswtwbiSiSHT' and lang = 'BnPhonetic'
     );

     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_258, 'proja-sotter-type', 'BnPhonetic', 158, 'rayti dkhlIswtwbiSiSHT', @parent_master_id
     from dual
      where not exists (
     select 1 from metadata_master_cs where val = 'rayti dkhlIswtwbiSiSHT' and lang = 'BnPhonetic'
     );

     -- code 259
     set @master_id_259 = UNHEX(REPLACE(uuid(), '-', ''));
     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_259, 'proja-sotter-type', 'En', 159, '', @parent_master_id
     from dual
      where not exists ( 
     select 1 from metadata_master_cs where val = 'kOrrfa dkhliswtwSuny' and lang = 'BnPhonetic'
     );

     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_259, 'proja-sotter-type', 'Bn', 159, 'কোর্ফা দখলিস্বত্বশুন্য', @parent_master_id
     from dual
      where not exists (
     select 1 from metadata_master_cs where val = 'kOrrfa dkhliswtwSuny' and lang = 'BnPhonetic'
     );

     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_259, 'proja-sotter-type', 'BnPhonetic', 159, 'kOrrfa dkhliswtwSuny', @parent_master_id
     from dual
      where not exists (
     select 1 from metadata_master_cs where val = 'kOrrfa dkhliswtwSuny' and lang = 'BnPhonetic'
     );

     -- code 260
     set @master_id_260 = UNHEX(REPLACE(uuid(), '-', ''));
     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_260, 'proja-sotter-type', 'En', 160, '', @parent_master_id
     from dual
      where not exists ( 
     select 1 from metadata_master_cs where val = 'jol`kor' and lang = 'BnPhonetic'
     );

     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_260, 'proja-sotter-type', 'Bn', 160, 'জলকর', @parent_master_id
     from dual
      where not exists (
     select 1 from metadata_master_cs where val = 'jol`kor' and lang = 'BnPhonetic'
     );

     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_260, 'proja-sotter-type', 'BnPhonetic', 160, 'jol`kor', @parent_master_id
     from dual
      where not exists (
     select 1 from metadata_master_cs where val = 'jol`kor' and lang = 'BnPhonetic'
     );

     -- code 261
     set @master_id_261 = UNHEX(REPLACE(uuid(), '-', ''));
     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_261, 'proja-sotter-type', 'En', 161, '', @parent_master_id
     from dual
      where not exists ( 
     select 1 from metadata_master_cs where val = 'rayti mOk`rrI' and lang = 'BnPhonetic'
     );

     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_261, 'proja-sotter-type', 'Bn', 161, 'রায়তি মোকররী', @parent_master_id
     from dual
      where not exists (
     select 1 from metadata_master_cs where val = 'rayti mOk`rrI' and lang = 'BnPhonetic'
     );

     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_261, 'proja-sotter-type', 'BnPhonetic', 161, 'rayti mOk`rrI', @parent_master_id
     from dual
      where not exists (
     select 1 from metadata_master_cs where val = 'rayti mOk`rrI' and lang = 'BnPhonetic'
     );

     -- code 262
     set @master_id_262 = UNHEX(REPLACE(uuid(), '-', ''));
     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_262, 'proja-sotter-type', 'En', 162, '', @parent_master_id
     from dual
      where not exists ( 
     select 1 from metadata_master_cs where val = 'rayti' and lang = 'BnPhonetic'
     );

     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_262, 'proja-sotter-type', 'Bn', 162, 'রায়তি', @parent_master_id
     from dual
      where not exists (
     select 1 from metadata_master_cs where val = 'rayti' and lang = 'BnPhonetic'
     );

     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_262, 'proja-sotter-type', 'BnPhonetic', 162, 'rayti', @parent_master_id
     from dual
      where not exists (
     select 1 from metadata_master_cs where val = 'rayti' and lang = 'BnPhonetic'
     );

     -- code 263
     set @master_id_263 = UNHEX(REPLACE(uuid(), '-', ''));
     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_263, 'proja-sotter-type', 'En', 163, '', @parent_master_id
     from dual
      where not exists ( 
     select 1 from metadata_master_cs where val = 'rayti dkhliswtwSuny' and lang = 'BnPhonetic'
     );

     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_263, 'proja-sotter-type', 'Bn', 163, 'রায়তি দখলিস্বত্বশুন্য', @parent_master_id
     from dual
      where not exists (
     select 1 from metadata_master_cs where val = 'rayti dkhliswtwSuny' and lang = 'BnPhonetic'
     );

     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_263, 'proja-sotter-type', 'BnPhonetic', 163, 'rayti dkhliswtwSuny', @parent_master_id
     from dual
      where not exists (
     select 1 from metadata_master_cs where val = 'rayti dkhliswtwSuny' and lang = 'BnPhonetic'
     );

     -- code 264
     set @master_id_264 = UNHEX(REPLACE(uuid(), '-', ''));
     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_264, 'proja-sotter-type', 'En', 164, '', @parent_master_id
     from dual
      where not exists ( 
     select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN khid`mt``dar' and lang = 'BnPhonetic'
     );

     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_264, 'proja-sotter-type', 'Bn', 164, 'দখলকার চাকরাণ খিদমৎদার', @parent_master_id
     from dual
      where not exists (
     select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN khid`mt``dar' and lang = 'BnPhonetic'
     );

     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_264, 'proja-sotter-type', 'BnPhonetic', 164, 'dkhl`kar cak`raN khid`mt``dar', @parent_master_id
     from dual
      where not exists (
     select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN khid`mt``dar' and lang = 'BnPhonetic'
     );

     -- code 265
     set @master_id_265 = UNHEX(REPLACE(uuid(), '-', ''));
     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_265, 'proja-sotter-type', 'En', 165, '', @parent_master_id
     from dual
      where not exists ( 
     select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN muci' and lang = 'BnPhonetic'
     );

     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_265, 'proja-sotter-type', 'Bn', 165, 'দখলকার চাকরাণ মুচি', @parent_master_id
     from dual
      where not exists (
     select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN muci' and lang = 'BnPhonetic'
     );

     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_265, 'proja-sotter-type', 'BnPhonetic', 165, 'dkhl`kar cak`raN muci', @parent_master_id
     from dual
      where not exists (
     select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN muci' and lang = 'BnPhonetic'
     );

     -- code 266
     set @master_id_266 = UNHEX(REPLACE(uuid(), '-', ''));
     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_266, 'proja-sotter-type', 'En', 166, '', @parent_master_id
     from dual
      where not exists ( 
     select 1 from metadata_master_cs where val = 'rayt sthitiban chOlesutre mOk`rrI' and lang = 'BnPhonetic'
     );

     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_266, 'proja-sotter-type', 'Bn', 166, 'রায়ত স্থিতিবান ছোলেসুত্রে মোকররী', @parent_master_id
     from dual
      where not exists (
     select 1 from metadata_master_cs where val = 'rayt sthitiban chOlesutre mOk`rrI' and lang = 'BnPhonetic'
     );

     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_266, 'proja-sotter-type', 'BnPhonetic', 166, 'rayt sthitiban chOlesutre mOk`rrI', @parent_master_id
     from dual
      where not exists (
     select 1 from metadata_master_cs where val = 'rayt sthitiban chOlesutre mOk`rrI' and lang = 'BnPhonetic'
     );

     -- code 267
     set @master_id_267 = UNHEX(REPLACE(uuid(), '-', ''));
     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_267, 'proja-sotter-type', 'En', 167, '', @parent_master_id
     from dual
      where not exists ( 
     select 1 from metadata_master_cs where val = 'dkhl`kar candina cirsthayI' and lang = 'BnPhonetic'
     );

     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_267, 'proja-sotter-type', 'Bn', 167, 'দখলকার চান্দিনা চিরস্থায়ী', @parent_master_id
     from dual
      where not exists (
     select 1 from metadata_master_cs where val = 'dkhl`kar candina cirsthayI' and lang = 'BnPhonetic'
     );

     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_267, 'proja-sotter-type', 'BnPhonetic', 167, 'dkhl`kar candina cirsthayI', @parent_master_id
     from dual
      where not exists (
     select 1 from metadata_master_cs where val = 'dkhl`kar candina cirsthayI' and lang = 'BnPhonetic'
     );

     -- code 268
     set @master_id_268 = UNHEX(REPLACE(uuid(), '-', ''));
     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_268, 'proja-sotter-type', 'En', 168, '', @parent_master_id
     from dual
      where not exists ( 
     select 1 from metadata_master_cs where val = 'rayt sthitiban nirikh mOk`rrI' and lang = 'BnPhonetic'
     );

     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_268, 'proja-sotter-type', 'Bn', 168, 'রায়ত স্থিতিবান নিরিখ মোকররী', @parent_master_id
     from dual
      where not exists (
     select 1 from metadata_master_cs where val = 'rayt sthitiban nirikh mOk`rrI' and lang = 'BnPhonetic'
     );

     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_268, 'proja-sotter-type', 'BnPhonetic', 168, 'rayt sthitiban nirikh mOk`rrI', @parent_master_id
     from dual
      where not exists (
     select 1 from metadata_master_cs where val = 'rayt sthitiban nirikh mOk`rrI' and lang = 'BnPhonetic'
     );

     -- code 269
     set @master_id_269 = UNHEX(REPLACE(uuid(), '-', ''));
     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_269, 'proja-sotter-type', 'En', 169, '', @parent_master_id
     from dual
      where not exists ( 
     select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN jagir m`nDlan' and lang = 'BnPhonetic'
     );

     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_269, 'proja-sotter-type', 'Bn', 169, 'দখলকার চাকরাণ জাগির মন্ডলান', @parent_master_id
     from dual
      where not exists (
     select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN jagir m`nDlan' and lang = 'BnPhonetic'
     );

     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_269, 'proja-sotter-type', 'BnPhonetic', 169, 'dkhl`kar cak`raN jagir m`nDlan', @parent_master_id
     from dual
      where not exists (
     select 1 from metadata_master_cs where val = 'dkhl`kar cak`raN jagir m`nDlan' and lang = 'BnPhonetic'
     );

     -- code 270
     set @master_id_270 = UNHEX(REPLACE(uuid(), '-', ''));
     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_270, 'proja-sotter-type', 'En', 170, '', @parent_master_id
     from dual
      where not exists ( 
     select 1 from metadata_master_cs where val = 'niSHk`r rayt' and lang = 'BnPhonetic'
     );

     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_270, 'proja-sotter-type', 'Bn', 170, 'নিষ্কর রায়ত', @parent_master_id
     from dual
      where not exists (
     select 1 from metadata_master_cs where val = 'niSHk`r rayt' and lang = 'BnPhonetic'
     );

     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_270, 'proja-sotter-type', 'BnPhonetic', 170, 'niSHk`r rayt', @parent_master_id
     from dual
      where not exists (
     select 1 from metadata_master_cs where val = 'niSHk`r rayt' and lang = 'BnPhonetic'
     );

     -- code 271
     set @master_id_271 = UNHEX(REPLACE(uuid(), '-', ''));
     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_271, 'proja-sotter-type', 'En', 171, '', @parent_master_id
     from dual
      where not exists ( 
     select 1 from metadata_master_cs where val = 'cak`raN dkhl`kar' and lang = 'BnPhonetic'
     );

     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_271, 'proja-sotter-type', 'Bn', 171, 'চাকরাণ দখলকার', @parent_master_id
     from dual
      where not exists (
     select 1 from metadata_master_cs where val = 'cak`raN dkhl`kar' and lang = 'BnPhonetic'
     );

     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_271, 'proja-sotter-type', 'BnPhonetic', 171, 'cak`raN dkhl`kar', @parent_master_id
     from dual
      where not exists (
     select 1 from metadata_master_cs where val = 'cak`raN dkhl`kar' and lang = 'BnPhonetic'
     );

     -- code 272
     set @master_id_272 = UNHEX(REPLACE(uuid(), '-', ''));
     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_272, 'proja-sotter-type', 'En', 172, '', @parent_master_id
     from dual
      where not exists ( 
     select 1 from metadata_master_cs where val = 'dkhl`kar bs`t cirsthayI' and lang = 'BnPhonetic'
     );

     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_272, 'proja-sotter-type', 'Bn', 172, 'দখলকার বসত চিরস্থায়ী', @parent_master_id
     from dual
      where not exists (
     select 1 from metadata_master_cs where val = 'dkhl`kar bs`t cirsthayI' and lang = 'BnPhonetic'
     );

     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_272, 'proja-sotter-type', 'BnPhonetic', 172, 'dkhl`kar bs`t cirsthayI', @parent_master_id
     from dual
      where not exists (
     select 1 from metadata_master_cs where val = 'dkhl`kar bs`t cirsthayI' and lang = 'BnPhonetic'
     );

     -- code 273
     set @master_id_273 = UNHEX(REPLACE(uuid(), '-', ''));
     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_273, 'proja-sotter-type', 'En', 173, '', @parent_master_id
     from dual
      where not exists ( 
     select 1 from metadata_master_cs where val = 'niSHk`r dkhl`kar bs`t' and lang = 'BnPhonetic'
     );

     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_273, 'proja-sotter-type', 'Bn', 173, 'নিষ্কর দখলকার বসত', @parent_master_id
     from dual
      where not exists (
     select 1 from metadata_master_cs where val = 'niSHk`r dkhl`kar bs`t' and lang = 'BnPhonetic'
     );

     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_273, 'proja-sotter-type', 'BnPhonetic', 173, 'niSHk`r dkhl`kar bs`t', @parent_master_id
     from dual
      where not exists (
     select 1 from metadata_master_cs where val = 'niSHk`r dkhl`kar bs`t' and lang = 'BnPhonetic'
     );

     -- code 274
     set @master_id_274 = UNHEX(REPLACE(uuid(), '-', ''));
     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_274, 'proja-sotter-type', 'En', 174, '', @parent_master_id
     from dual
      where not exists (
     select 1 from metadata_master_cs where val = 'mdhyswtw myadI' and lang = 'BnPhonetic'
     );

     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_274, 'proja-sotter-type', 'Bn', 174, 'মধ্যস্বত্ব ম্যাদী', @parent_master_id
     from dual
      where not exists (
     select 1 from metadata_master_cs where val = 'mdhyswtw myadI' and lang = 'BnPhonetic'
     );

     insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
     select @master_id_274, 'proja-sotter-type', 'BnPhonetic', 174, 'mdhyswtw myadI', @parent_master_id
     from dual
      where not exists (
     select 1 from metadata_master_cs where val = 'mdhyswtw myadI' and lang = 'BnPhonetic'
     );

     -- code 275
    set @master_id_275 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_275, 'proja-sotter-type', 'En', 175, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtw cirsthayI n`he khajana brriddhir zOgy' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_275, 'proja-sotter-type', 'Bn', 175, 'মধ্যস্বত্ব চিরস্থায়ী নহে খাজানা বৃদ্ধির যোগ্য', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtw cirsthayI n`he khajana brriddhir zOgy' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_275, 'proja-sotter-type', 'BnPhonetic', 175, 'mdhyswtw cirsthayI n`he khajana brriddhir zOgy', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtw cirsthayI n`he khajana brriddhir zOgy' and lang = 'BnPhonetic'
    );

    -- code 276
    set @master_id_276 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_276, 'proja-sotter-type', 'En', 176, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'kOrrfa puruSHanukrme' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_276, 'proja-sotter-type', 'Bn', 176, 'কোর্ফা পুরুষানুক্রমে', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'kOrrfa puruSHanukrme' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_276, 'proja-sotter-type', 'BnPhonetic', 176, 'kOrrfa puruSHanukrme', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'kOrrfa puruSHanukrme' and lang = 'BnPhonetic'
    );

    -- code 277
    set @master_id_277 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_277, 'proja-sotter-type', 'En', 177, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt niSHk`r vOgOtt`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_277, 'proja-sotter-type', 'Bn', 177, 'রায়ত নিষ্কর ভোগোত্তর', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt niSHk`r vOgOtt`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_277, 'proja-sotter-type', 'BnPhonetic', 177, 'rayt niSHk`r vOgOtt`r', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'rayt niSHk`r vOgOtt`r' and lang = 'BnPhonetic'
    );

    -- code 278
    set @master_id_278 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_278, 'proja-sotter-type', 'En', 178, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'kOrrfa mOk`rrI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_278, 'proja-sotter-type', 'Bn', 178, 'কোর্ফা মোকররী', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'kOrrfa mOk`rrI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_278, 'proja-sotter-type', 'BnPhonetic', 178, 'kOrrfa mOk`rrI', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'kOrrfa mOk`rrI' and lang = 'BnPhonetic'
    );

    -- code 279
    set @master_id_279 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_279, 'proja-sotter-type', 'En', 179, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar debtr niSHk`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_279, 'proja-sotter-type', 'Bn', 179, 'দখলকার দেবত্র নিষ্কর', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar debtr niSHk`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_279, 'proja-sotter-type', 'BnPhonetic', 179, 'dkhl`kar debtr niSHk`r', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar debtr niSHk`r' and lang = 'BnPhonetic'
    );

    -- code 280
    set @master_id_280 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_280, 'proja-sotter-type', 'En', 180, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar jl`k`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_280, 'proja-sotter-type', 'Bn', 180, 'দখলকার জলকর', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar jl`k`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_280, 'proja-sotter-type', 'BnPhonetic', 180, 'dkhl`kar jl`k`r', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar jl`k`r' and lang = 'BnPhonetic'
    );

    -- code 281
    set @master_id_281 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_281, 'proja-sotter-type', 'En', 181, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cirsthayI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_281, 'proja-sotter-type', 'Bn', 181, 'দখলকার চিরস্থায়ী', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cirsthayI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_281, 'proja-sotter-type', 'BnPhonetic', 181, 'dkhl`kar cirsthayI', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cirsthayI' and lang = 'BnPhonetic'
    );

    -- code 282
    set @master_id_282 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_282, 'proja-sotter-type', 'En', 182, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar bs`tprja cirsthayI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_282, 'proja-sotter-type', 'Bn', 182, 'দখলকার বসতপ্রজা চিরস্থায়ী', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar bs`tprja cirsthayI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_282, 'proja-sotter-type', 'BnPhonetic', 182, 'dkhl`kar bs`tprja cirsthayI', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar bs`tprja cirsthayI' and lang = 'BnPhonetic'
    );

    -- code 283
    set @master_id_283 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_283, 'proja-sotter-type', 'En', 183, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar paTTamUle mOk`rrI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_283, 'proja-sotter-type', 'Bn', 183, 'দখলকার পাট্টামূলে মোকররী', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar paTTamUle mOk`rrI' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_283, 'proja-sotter-type', 'BnPhonetic', 183, 'dkhl`kar paTTamUle mOk`rrI', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar paTTamUle mOk`rrI' and lang = 'BnPhonetic'
    );

    -- code 284
    set @master_id_284 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_284, 'proja-sotter-type', 'En', 184, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cirsthayI debOtt`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_284, 'proja-sotter-type', 'Bn', 184, 'দখলকার চিরস্থায়ী দেবোত্তর', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cirsthayI debOtt`r' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_284, 'proja-sotter-type', 'BnPhonetic', 184, 'dkhl`kar cirsthayI debOtt`r', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar cirsthayI debOtt`r' and lang = 'BnPhonetic'
    );

    -- code 285
    set @master_id_285 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_285, 'proja-sotter-type', 'En', 185, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI khajana brriddhir zOgy' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_285, 'proja-sotter-type', 'Bn', 185, 'মধ্যস্বত্বাধিকারী খাজানা বৃদ্ধির যোগ্য', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI khajana brriddhir zOgy' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_285, 'proja-sotter-type', 'BnPhonetic', 185, 'mdhyswtwadhikarI khajana brriddhir zOgy', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI khajana brriddhir zOgy' and lang = 'BnPhonetic'
    );

    -- code 286
    set @master_id_286 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_286, 'proja-sotter-type', 'En', 186, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI b`rrttman myad onte khajana brriddhir zOgy' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_286, 'proja-sotter-type', 'Bn', 186, 'মধ্যস্বত্বাধিকারী চিরস্থায়ী বর্ত্তমান ম্যাদ অন্তে খাজানা বৃদ্ধির যোগ্য', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI b`rrttman myad onte khajana brriddhir zOgy' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_286, 'proja-sotter-type', 'BnPhonetic', 186, 'mdhyswtwadhikarI cirsthayI b`rrttman myad onte khajana brriddhir zOgy', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI b`rrttman myad onte khajana brriddhir zOgy' and lang = 'BnPhonetic'
    );

    -- code 287
    set @master_id_287 = UNHEX(REPLACE(uuid(), '-', ''));
    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_287, 'proja-sotter-type', 'En', 187, '', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar napit' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_287, 'proja-sotter-type', 'Bn', 187, 'দখলকার নাপিত', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar napit' and lang = 'BnPhonetic'
    );

    insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
    select @master_id_287, 'proja-sotter-type', 'BnPhonetic', 187, 'dkhl`kar napit', @parent_master_id
    from dual
     where not exists (
    select 1 from metadata_master_cs where val = 'dkhl`kar napit' and lang = 'BnPhonetic'
    );

        -- code 288
        set @master_id_288 = UNHEX(REPLACE(uuid(), '-', ''));
        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_288, 'proja-sotter-type', 'En', 188, '', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI osthayI myad onte khajana brriddhir zOgy' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_288, 'proja-sotter-type', 'Bn', 188, 'মধ্যস্বত্বাধিকারী অস্থায়ী ম্যাদ অন্তে খাজানা বৃদ্ধির যোগ্য', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI osthayI myad onte khajana brriddhir zOgy' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_288, 'proja-sotter-type', 'BnPhonetic', 188, 'mdhyswtwadhikarI osthayI myad onte khajana brriddhir zOgy', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI osthayI myad onte khajana brriddhir zOgy' and lang = 'BnPhonetic'
        );
    
        -- code 289
        set @master_id_289 = UNHEX(REPLACE(uuid(), '-', ''));
        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_289, 'proja-sotter-type', 'En', 189, '', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'niSHk`r kOrrfa' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_289, 'proja-sotter-type', 'Bn', 189, 'নিষ্কর কোর্ফা', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'niSHk`r kOrrfa' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_289, 'proja-sotter-type', 'BnPhonetic', 189, 'niSHk`r kOrrfa', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'niSHk`r kOrrfa' and lang = 'BnPhonetic'
        );

    -- code 290
        set @master_id_290 = UNHEX(REPLACE(uuid(), '-', ''));
        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_290, 'proja-sotter-type', 'En', 190, '', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtw cirsthayI mOk`rrI brriddhir zOgy' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_290, 'proja-sotter-type', 'Bn', 190, 'মধ্যস্বত্ব চিরস্থায়ী মোকররী বৃদ্ধির যোগ্য', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtw cirsthayI mOk`rrI brriddhir zOgy' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_290, 'proja-sotter-type', 'BnPhonetic', 190, 'mdhyswtw cirsthayI mOk`rrI brriddhir zOgy', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtw cirsthayI mOk`rrI brriddhir zOgy' and lang = 'BnPhonetic'
        );

            -- code 291
        set @master_id_291 = UNHEX(REPLACE(uuid(), '-', ''));
        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_291, 'proja-sotter-type', 'En', 191, '', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtw' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_291, 'proja-sotter-type', 'Bn', 191, 'মধ্যস্বত্ব', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtw' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_291, 'proja-sotter-type', 'BnPhonetic', 191, 'mdhyswtw', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtw' and lang = 'BnPhonetic'
        );

            -- code 292
        set @master_id_292 = UNHEX(REPLACE(uuid(), '-', ''));
        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_292, 'proja-sotter-type', 'En', 192, '', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhy swtw osthayI' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_292, 'proja-sotter-type', 'Bn', 192, 'মধ্য স্বত্ব অস্থায়ী', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhy swtw osthayI' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_292, 'proja-sotter-type', 'BnPhonetic', 192, 'mdhy swtw osthayI', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhy swtw osthayI' and lang = 'BnPhonetic'
        );

            -- code 293
        set @master_id_293 = UNHEX(REPLACE(uuid(), '-', ''));
        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_293, 'proja-sotter-type', 'En', 193, '', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtw myadi myad mdhye mOk`rrI' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_293, 'proja-sotter-type', 'Bn', 193, 'মধ্যস্বত্ব ম্যাদি ম্যাদ মধ্যে মোকররী', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtw myadi myad mdhye mOk`rrI' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_293, 'proja-sotter-type', 'BnPhonetic', 193, 'mdhyswtw myadi myad mdhye mOk`rrI', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtw myadi myad mdhye mOk`rrI' and lang = 'BnPhonetic'
        );

            -- code 294
        set @master_id_294 = UNHEX(REPLACE(uuid(), '-', ''));
        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_294, 'proja-sotter-type', 'En', 194, '', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtw cirsthayI niSHk`r' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_294, 'proja-sotter-type', 'Bn', 194, 'মধ্যস্বত্ব চিরস্থায়ী নিষ্কর', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtw cirsthayI niSHk`r' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_294, 'proja-sotter-type', 'BnPhonetic', 194, 'mdhyswtw cirsthayI niSHk`r', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtw cirsthayI niSHk`r' and lang = 'BnPhonetic'
        );

            -- code 295
        set @master_id_295 = UNHEX(REPLACE(uuid(), '-', ''));
        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_295, 'proja-sotter-type', 'En', 195, '', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'rayti dkhliswtwbihin' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_295, 'proja-sotter-type', 'Bn', 195, 'রায়তি দখলিস্বত্ববিহিন', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'rayti dkhliswtwbihin' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_295, 'proja-sotter-type', 'BnPhonetic', 195, 'rayti dkhliswtwbihin', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'rayti dkhliswtwbihin' and lang = 'BnPhonetic'
        );

            -- code 296
        set @master_id_296 = UNHEX(REPLACE(uuid(), '-', ''));
        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_296, 'proja-sotter-type', 'En', 196, '', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'rayt debtt`rdar' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_296, 'proja-sotter-type', 'Bn', 196, 'রায়ত দেবত্তরদার', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'rayt debtt`rdar' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_296, 'proja-sotter-type', 'BnPhonetic', 196, 'rayt debtt`rdar', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'rayt debtt`rdar' and lang = 'BnPhonetic'
        );

            -- code 297
        set @master_id_297 = UNHEX(REPLACE(uuid(), '-', ''));
        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_297, 'proja-sotter-type', 'En', 197, '', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'rayt brHMOtt`rdar' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_297, 'proja-sotter-type', 'Bn', 197, 'রায়ত ব্রহ্মোত্তরদার', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'rayt brHMOtt`rdar' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_297, 'proja-sotter-type', 'BnPhonetic', 197, 'rayt brHMOtt`rdar', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'rayt brHMOtt`rdar' and lang = 'BnPhonetic'
        );

            -- code 298
        set @master_id_298 = UNHEX(REPLACE(uuid(), '-', ''));
        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_298, 'proja-sotter-type', 'En', 198, '', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'rayt brHMtt`r' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_298, 'proja-sotter-type', 'Bn', 198, 'রায়ত ব্রহ্মত্তর', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'rayt brHMtt`r' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_298, 'proja-sotter-type', 'BnPhonetic', 198, 'rayt brHMtt`r', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'rayt brHMtt`r' and lang = 'BnPhonetic'
        );

            -- code 299
        set @master_id_299 = UNHEX(REPLACE(uuid(), '-', ''));
        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_299, 'proja-sotter-type', 'En', 199, '', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'rayt niSHk`rdar' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_299, 'proja-sotter-type', 'Bn', 199, 'রায়ত নিষ্করদার', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'rayt niSHk`rdar' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_299, 'proja-sotter-type', 'BnPhonetic', 199, 'rayt niSHk`rdar', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'rayt niSHk`rdar' and lang = 'BnPhonetic'
        );

            -- code 300
        set @master_id_300 = UNHEX(REPLACE(uuid(), '-', ''));
        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_300, 'proja-sotter-type', 'En', 200, '', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI osthayI myad mdhye khajana brriddhir ozOgy' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_300, 'proja-sotter-type', 'Bn', 200, 'মধ্যস্বত্বাধিকারী অস্থায়ী ম্যাদ মধ্যে খাজানা বৃদ্ধির অযোগ্য', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI osthayI myad mdhye khajana brriddhir ozOgy' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_300, 'proja-sotter-type', 'BnPhonetic', 200, 'mdhyswtwadhikarI osthayI myad mdhye khajana brriddhir ozOgy', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI osthayI myad mdhye khajana brriddhir ozOgy' and lang = 'BnPhonetic'
        );

            -- code 301
        set @master_id_301 = UNHEX(REPLACE(uuid(), '-', ''));
        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_301, 'proja-sotter-type', 'En', 201, '', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI myad mdhye mOk`rrI' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_301, 'proja-sotter-type', 'Bn', 201, 'মধ্যস্বত্বাধিকারী চিরস্থায়ী ম্যাদ মধ্যে মোকররী', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI myad mdhye mOk`rrI' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_301, 'proja-sotter-type', 'BnPhonetic', 201, 'mdhyswtwadhikarI cirsthayI myad mdhye mOk`rrI', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI myad mdhye mOk`rrI' and lang = 'BnPhonetic'
        );

            -- code 302
        set @master_id_302 = UNHEX(REPLACE(uuid(), '-', ''));
        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_302, 'proja-sotter-type', 'En', 202, '', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI nirikh mOk`rrI' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_302, 'proja-sotter-type', 'Bn', 202, 'মধ্যস্বত্বাধিকারী চিরস্থায়ী নিরিখ মোকররী', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI nirikh mOk`rrI' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_302, 'proja-sotter-type', 'BnPhonetic', 202, 'mdhyswtwadhikarI cirsthayI nirikh mOk`rrI', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI nirikh mOk`rrI' and lang = 'BnPhonetic'
        );

            -- code 303
        set @master_id_303 = UNHEX(REPLACE(uuid(), '-', ''));
        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_303, 'proja-sotter-type', 'En', 203, '', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'rayt nirikh mOk`rrI' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_303, 'proja-sotter-type', 'Bn', 203, 'রায়ত নিরিখ মোকররী', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'rayt nirikh mOk`rrI' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_303, 'proja-sotter-type', 'BnPhonetic', 203, 'rayt nirikh mOk`rrI', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'rayt nirikh mOk`rrI' and lang = 'BnPhonetic'
        );

        -- code 304
        set @master_id_304 = UNHEX(REPLACE(uuid(), '-', ''));
        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_304, 'proja-sotter-type', 'En', 204, '', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI osthayI myad mdhye mOk`rrI' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_304, 'proja-sotter-type', 'Bn', 204, 'মধ্যস্বত্বাধিকারী অস্থায়ী ম্যাদ মধ্যে মোকররী', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI osthayI myad mdhye mOk`rrI' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_304, 'proja-sotter-type', 'BnPhonetic', 204, 'mdhyswtwadhikarI osthayI myad mdhye mOk`rrI', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI osthayI myad mdhye mOk`rrI' and lang = 'BnPhonetic'
        );

        -- code 305
        set @master_id_305 = UNHEX(REPLACE(uuid(), '-', ''));
        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_305, 'proja-sotter-type', 'En', 205, '', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtw cirsthayI brriddhir zOgy' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_305, 'proja-sotter-type', 'Bn', 205, 'মধ্যস্বত্ব চিরস্থায়ী বৃদ্ধির যোগ্য', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtw cirsthayI brriddhir zOgy' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_305, 'proja-sotter-type', 'BnPhonetic', 205, 'mdhyswtw cirsthayI brriddhir zOgy', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtw cirsthayI brriddhir zOgy' and lang = 'BnPhonetic'
        );

        -- code 306
        set @master_id_306 = UNHEX(REPLACE(uuid(), '-', ''));
        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_306, 'proja-sotter-type', 'En', 206, '', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI debOtt`r' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_306, 'proja-sotter-type', 'Bn', 206, 'মধ্যস্বত্বাধিকারী চিরস্থায়ী দেবোত্তর', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI debOtt`r' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_306, 'proja-sotter-type', 'BnPhonetic', 206, 'mdhyswtwadhikarI cirsthayI debOtt`r', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI debOtt`r' and lang = 'BnPhonetic'
        );

        -- code 307
        set @master_id_307 = UNHEX(REPLACE(uuid(), '-', ''));
        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_307, 'proja-sotter-type', 'En', 207, '', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI n`he myad mdhye mOk`rrI' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_307, 'proja-sotter-type', 'Bn', 207, 'মধ্যস্বত্বাধিকারী চিরস্থায়ী নহে ম্যাদ মধ্যে মোকররী', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI n`he myad mdhye mOk`rrI' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_307, 'proja-sotter-type', 'BnPhonetic', 207, 'mdhyswtwadhikarI cirsthayI n`he myad mdhye mOk`rrI', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI n`he myad mdhye mOk`rrI' and lang = 'BnPhonetic'
        );

         -- code 308
        set @master_id_308 = UNHEX(REPLACE(uuid(), '-', ''));
        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_308, 'proja-sotter-type', 'En', 208, '', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI brHMOtt`r' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_308, 'proja-sotter-type', 'Bn', 208, 'মধ্যস্বত্বাধিকারী চিরস্থায়ী ব্রহ্মোত্তর', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI brHMOtt`r' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_308, 'proja-sotter-type', 'BnPhonetic', 208, 'mdhyswtwadhikarI cirsthayI brHMOtt`r', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI brHMOtt`r' and lang = 'BnPhonetic'
        );

        -- code 309
        set @master_id_309 = UNHEX(REPLACE(uuid(), '-', ''));
        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_309, 'proja-sotter-type', 'En', 209, '', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI niSHk`rdar' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_309, 'proja-sotter-type', 'Bn', 209, 'মধ্যস্বত্বাধিকারী চিরস্থায়ী নিষ্করদার', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI niSHk`rdar' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_309, 'proja-sotter-type', 'BnPhonetic', 209, 'mdhyswtwadhikarI cirsthayI niSHk`rdar', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI niSHk`rdar' and lang = 'BnPhonetic'
        );

        -- code 310
        set @master_id_310 = UNHEX(REPLACE(uuid(), '-', ''));
        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_310, 'proja-sotter-type', 'En', 210, '', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI SibOtt`r' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_310, 'proja-sotter-type', 'Bn', 210, 'মধ্যস্বত্বাধিকারী চিরস্থায়ী শিবোত্তর', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI SibOtt`r' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_310, 'proja-sotter-type', 'BnPhonetic', 210, 'mdhyswtwadhikarI cirsthayI SibOtt`r', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI SibOtt`r' and lang = 'BnPhonetic'
        );

        -- code 311
        set @master_id_311 = UNHEX(REPLACE(uuid(), '-', ''));
        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_311, 'proja-sotter-type', 'En', 211, '', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'dor kOrrfa dkhliswtwbiSiSHT' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_311, 'proja-sotter-type', 'Bn', 211, 'দর কোর্ফা দখলিস্বত্ববিশিষ্ট', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'dor kOrrfa dkhliswtwbiSiSHT' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_311, 'proja-sotter-type', 'BnPhonetic', 211, 'dor kOrrfa dkhliswtwbiSiSHT', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'dor kOrrfa dkhliswtwbiSiSHT' and lang = 'BnPhonetic'
        );

        -- code 312
        set @master_id_312 = UNHEX(REPLACE(uuid(), '-', ''));
        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_312, 'proja-sotter-type', 'En', 212, '', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI bOISHNbOttor' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_312, 'proja-sotter-type', 'Bn', 212, 'মধ্যস্বত্বাধিকারী চিরস্থায়ী বৈষ্ণবোত্তর', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI bOISHNbOttor' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_312, 'proja-sotter-type', 'BnPhonetic', 212, 'mdhyswtwadhikarI cirsthayI bOISHNbOttor', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI bOISHNbOttor' and lang = 'BnPhonetic'
        );

                -- code 313
        set @master_id_313 = UNHEX(REPLACE(uuid(), '-', ''));
        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_313, 'proja-sotter-type', 'En', 213, '', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'ghas`kor' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_313, 'proja-sotter-type', 'Bn', 213, 'ঘাসকর', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'ghas`kor' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_313, 'proja-sotter-type', 'BnPhonetic', 213, 'ghas`kor', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'ghas`kor' and lang = 'BnPhonetic'
        );

                -- code 314
        set @master_id_314 = UNHEX(REPLACE(uuid(), '-', ''));
        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_314, 'proja-sotter-type', 'En', 214, '', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'niSHk`r cirsthayI nhe' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_314, 'proja-sotter-type', 'Bn', 214, 'নিষ্কর চিরস্থায়ী নহে', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'niSHk`r cirsthayI nhe' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_314, 'proja-sotter-type', 'BnPhonetic', 214, 'niSHk`r cirsthayI nhe', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'niSHk`r cirsthayI nhe' and lang = 'BnPhonetic'
        );

                -- code 315
        set @master_id_315 = UNHEX(REPLACE(uuid(), '-', ''));
        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_315, 'proja-sotter-type', 'En', 215, '', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'cak`raN mdhyswtw' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_315, 'proja-sotter-type', 'Bn', 215, 'চাকরাণ মধ্যস্বত্ব', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'cak`raN mdhyswtw' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_315, 'proja-sotter-type', 'BnPhonetic', 215, 'cak`raN mdhyswtw', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'cak`raN mdhyswtw' and lang = 'BnPhonetic'
        );

                -- code 316
        set @master_id_316 = UNHEX(REPLACE(uuid(), '-', ''));
        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_316, 'proja-sotter-type', 'En', 216, '', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'ijaradar' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_316, 'proja-sotter-type', 'Bn', 216, 'ইজারাদার', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'ijaradar' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_316, 'proja-sotter-type', 'BnPhonetic', 216, 'ijaradar', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'ijaradar' and lang = 'BnPhonetic'
        );

           -- code 317
        set @master_id_317 = UNHEX(REPLACE(uuid(), '-', ''));
        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_317, 'proja-sotter-type', 'En', 217, '', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'cak`raN kOrrfa' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_317, 'proja-sotter-type', 'Bn', 217, 'চাকরাণ কোর্ফা', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'cak`raN kOrrfa' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_317, 'proja-sotter-type', 'BnPhonetic', 217, 'cak`raN kOrrfa', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'cak`raN kOrrfa' and lang = 'BnPhonetic'
        );

           -- code 318
        set @master_id_318 = UNHEX(REPLACE(uuid(), '-', ''));
        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_318, 'proja-sotter-type', 'En', 218, '', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtw myadI myad mdhye khajana brriddhir ozOgy' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_318, 'proja-sotter-type', 'Bn', 218, 'মধ্যস্বত্ব ম্যাদী ম্যাদ মধ্যে খাজানা বৃদ্ধির অযোগ্য', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtw myadI myad mdhye khajana brriddhir ozOgy' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_318, 'proja-sotter-type', 'BnPhonetic', 218, 'mdhyswtw myadI myad mdhye khajana brriddhir ozOgy', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtw myadI myad mdhye khajana brriddhir ozOgy' and lang = 'BnPhonetic'
        );

         -- code 319
        set @master_id_319 = UNHEX(REPLACE(uuid(), '-', ''));
        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_319, 'proja-sotter-type', 'En', 219, '', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'myadI ijara myad mdhye mOk`rrI' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_319, 'proja-sotter-type', 'Bn', 219, 'ম্যাদী ইজারা ম্যাদ মধ্যে মোকররী', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'myadI ijara myad mdhye mOk`rrI' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_319, 'proja-sotter-type', 'BnPhonetic', 219, 'myadI ijara myad mdhye mOk`rrI', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'myadI ijara myad mdhye mOk`rrI' and lang = 'BnPhonetic'
        );

        -- code 320
        set @master_id_320 = UNHEX(REPLACE(uuid(), '-', ''));
        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_320, 'proja-sotter-type', 'En', 220, '', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI mhatraN' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_320, 'proja-sotter-type', 'Bn', 220, 'মধ্যস্বত্বাধিকারী চিরস্থায়ী মহাত্রাণ', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI mhatraN' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_320, 'proja-sotter-type', 'BnPhonetic', 220, 'mdhyswtwadhikarI cirsthayI mhatraN', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI mhatraN' and lang = 'BnPhonetic'
        );

        -- code 321
        set @master_id_321 = UNHEX(REPLACE(uuid(), '-', ''));
        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_321, 'proja-sotter-type', 'En', 221, '', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI pirOtt`r' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_321, 'proja-sotter-type', 'Bn', 221, 'মধ্যস্বত্বাধিকারী চিরস্থায়ী পিরোত্তর', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI pirOtt`r' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_321, 'proja-sotter-type', 'BnPhonetic', 221, 'mdhyswtwadhikarI cirsthayI pirOtt`r', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI pirOtt`r' and lang = 'BnPhonetic'
        );

        -- code 322
        set @master_id_322 = UNHEX(REPLACE(uuid(), '-', ''));
        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_322, 'proja-sotter-type', 'En', 222, '', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI khajana brriddhir ozOgy' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_322, 'proja-sotter-type', 'Bn', 222, 'মধ্যস্বত্বাধিকারী চিরস্থায়ী খাজানা বৃদ্ধির অযোগ্য', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI khajana brriddhir ozOgy' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_322, 'proja-sotter-type', 'BnPhonetic', 222, 'mdhyswtwadhikarI cirsthayI khajana brriddhir ozOgy', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'mdhyswtwadhikarI cirsthayI khajana brriddhir ozOgy' and lang = 'BnPhonetic'
        );

        -- code 323
        set @master_id_323 = UNHEX(REPLACE(uuid(), '-', ''));
        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_323, 'proja-sotter-type', 'En', 223, '', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'myadI' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_323, 'proja-sotter-type', 'Bn', 223, 'ম্যাদী', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'myadI' and lang = 'BnPhonetic'
        );

        insert into metadata_master_cs (master_id, field_name, lang, seq, val, parent_master_id)
        select @master_id_323, 'proja-sotter-type', 'BnPhonetic', 223, 'myadI', @parent_master_id
        from dual
         where not exists (
        select 1 from metadata_master_cs where val = 'myadI' and lang = 'BnPhonetic'
        );


    -- SECTION 3: Update proja-sotter-type codes into master_code_cs table
    -- Create a temporary table to store the key-value pairs for proja-sotter-type type_name and code
    create temporary table temp_new_proja_sotter_type(
        code varchar(50),
        type_name varchar(100)
    );
    -- Insert hard-coded key-value pairs into the temporary table 
    insert into temp_new_proja_sotter_type (code,type_name) values
    ('101','malik'),
    ('102','mdhyswtwadhikarI cirsthayI khajana brriddhir zOgy'),
    ('103','rayt dokhliswtw shUny'),
    ('104','pottnI modhy swtwadhikarI cirsthayI mOkborI'),
    ('105','modhyswtwadhiKarI cirsthayI mOk`rrI'),
    ('106','rayt mOk`rrI'),
    ('107','rayt sthitiban'),
    ('108','kOrrfa dkhliswtwbiSiSHT'),
    ('109','kOrrfa'),
    ('110','rayt dkhliswtwbiSiSHT'),
    ('111','dkhl`kar'),
    ('112','mdhyswtwadhikarI osthayI'),
    ('113','paTTamule mOkarrI'),
    ('114','jmidar'),
    ('115','dkhlakar cak`ran'),
    ('116','mdhyswtwadhikarI cirsthayI nisk`r'),
    ('117','rayt nisk`r'),
    ('118','mdhyswtwadhikarI cirsthayI'),
    ('119','rayt'),
    ('120','dkhl'),
    ('121','ullekh nai'),
    ('122','dkhl`kar nisk`r'),
    ('123','kOrrfa b`hukalab`dhi'),
    ('124','mdhy swtwadhikarI niSHk`rdar pIrtr'),
    ('125','lakherajdar'),
    ('126','d`r p`ttni mdhyswtwadhikarI cirsthayI mOk`rrI'),
    ('127','raYt nisk`r brhmtr'),
    ('128','mdhy swtwadhikarI nisk`rdar brhmtr'),
    ('129','mdhy swtwadhikarI nisk`rdar'),
    ('130','modhy swtwadhikarI niSHk`r cir`sthayI n`he'),
    ('131','raYt niSHk`r debtr'),
    ('132','dokhol`kar cak`raN mNDlan'),
    ('133','dokhol`kar bos`t prja'),
    ('134','kOrrfa dkhli swtwbiSiSHT deSacar`onusare'),
    ('135','dokhol`kar cak`raN tohoSIldar'),
    ('136','dokhol`kar cak`raN napit'),
    ('137','dokhol`kar cak`raN DHUlI'),
    ('138','modhy swtwadhikarI cirosthayI mOk`rorI nohe'),
    ('139','dokhol`kar cak`raN halsahana'),
    ('140','pottni modhy swtwadhikarI cirosthayI mOk`rorI nohe'),
    ('141','modhy swtwadhikarI niSHKordar debOtt`r'),
    ('142','kOrrfa sthitibanbishiSHT'),
    ('143','mdhy swtwadhikarI'),
    ('144','mdhy swtwadhikarI myadI'),
    ('145','rayt mOk`rrI sthitiban'),
    ('146','dokhl`kar cak`raN DhakI'),
    ('147','mdhyswtwadhikarI osthayI myad kal`mdhye khajanabrriddhir`ozOgy'),
    ('148','vOgdkhlsUtre'),
    ('149','mdhyswtwadhikarI cirsthayI nhe'),
    ('150','kOrrfa myadI'),
    ('151','mdhyswtwadhikarI cirsthayI nhe khajana brriddhir zOgy'),
    ('152','22.2 dhara'),
    ('153','mdhy swtwadhikarI niSHk`r'),
    ('154','rayti sthitiban'),
    ('155','rayt niSHk`r Sibtr'),
    ('156','niSHk`r mdhy swtwadhikarI'),
    ('157','dkhl`kar niSHk`r brhmtr'),
    ('158','dkhl`kar cak`ran SIbpUjarjny'),
    ('159','dkhl`kar cak`raN nity pUjar jny'),
    ('160','rayt niShk`r bOISHNbOtt`r'),
    ('161','mdhy swtwadhikarI niSHk`rdar debtr'),
    ('162','dkhl`kar cak`raN ceragI'),
    ('163','dkhl`kar cak`raN pUjarI'),
    ('164','dkhl`kar cak`raN dh`rrmm`raj'),
    ('165','dkhl`kar cak`raN kOTal'),
    ('166','kOrrfa cak`raN'),
    ('167','dkhl`kar cak`raN paik'),
    ('168','dkhl`kar bs`t'),
    ('169','dkhl`kar cak`raN kham`roi'),
    ('170','dkhl`kar cak`raN gOrait'),
    ('171','dkhl`kar cak`raN cak`r'),
    ('172','niSHk`r mdhy swtwadhikarI cirsthayI debOtt`r'),
    ('173','niSHk`r mdhy swtwadhikarI cirsthayI brhmOtt`r'),
    ('174','dkhl`kar cak`raN behara'),
    ('175','rayt sthitiban niSHk`r pIrOtt`r'),
    ('176','kOrrfa cak`raN napit'),
    ('177','rayt sthitiban niSHk`r'),
    ('178','dkhl`kar candina'),
    ('179','p`ttnI mdhy swtwadhikarI cirsthayI'),
    ('180','cak`ran'),
    ('181','niSHk`r rayt sthitiban vOgtt`r'),
    ('182','rayt sthitiban paTTamule nirikh mOk`rrI'),
    ('183','dkhl`kar cak`raN begar'),
    ('184','dkhl`kar cak`raN kumvkar'),
    ('185','niSHk`r rayt sthitiban pIrOtt`r'),
    ('186','niSHk`r mdhy swtwadhikarI cirsthayI brhmtr'),
    ('187','dkhl`kar cak`raN jhi girir jny'),
    ('188','dkhl`kar cak`ran jhaRudar O alO dan`karI'),
    ('189','dkhl`kar cak`ran purOhit'),
    ('190','sep`ttnI mdhy swtwadhikarI cirsthayI mOk`rrI'),
    ('191','dkhl`kar cak`raN kkhOUr karrzyer jny'),
    ('192','mdhy swtwadhikarI niShk`rdar bOISHNbOtt`r'),
    ('193','dkhl`kar cak`raN gh`ramI'),
    ('194','mdhyswtwadhikarI osthayI khajanabrriddhir zOgy'),
    ('195','cak`raN mdhyswtwadhikarI'),
    ('196','rayt prthanusareSUny'),
    ('197','dokhol`kar cak`ran khlifa'),
    ('198','dokhol`kar cak`ran sorrddar'),
    ('199','niSHk`r rayt sthitiban brhmtr'),
    ('200','mdhyswtwadhikarI osthayI myadI'),
    ('201','niSHk`r mdhy swtwadhikarI cirsthayI bOISHNbOttor'),
    ('202','modhy swtwadhikarI cir`sthayI niSHk`r pIrOttor'),
    ('203','cak`raN rayt'),
    ('204','niSHk`r dokhl`kar'),
    ('205','dkhl`kar cak`raN dhOpa'),
    ('206','dokhol`kar cak`raN methor'),
    ('207','mdhy swtwadhikarI cirsthayI nisk`r brHMttOr'),
    ('208','rayt niSHk`r pirOtt`r'),
    ('209','mdhy swtwadhikarI niSHk`rdar pirOttor'),
    ('210','mdhy swtwadhikarI niSHk`rdar SibOttor'),
    ('211','rayt niSHk`r SibOtwr'),
    ('212','mdhy swtwadhikarI nisk`rdar pirpal'),
    ('213','dkhl`kar pukur'),
    ('214','dokhol`kar cak`ran puSHPcoyon'),
    ('215','dkhl`kar cak`raN cak`raNI'),
    ('216','dkhl`kar cak`raN badykor'),
    ('217','dokhol`kar cak`raN sutrdhor'),
    ('218','dokhol`kar cak`ran poricarok'),
    ('219','sodor p`ttni mdhy swtwadhikarI cirsthayI mOk`rrI'),
    ('220','mdhy swtwadhikarI cirsthayI myadI myad mdhye khajanabrriddhirzOgy'),
    ('221','dkhl`kar cak`ran'),
    ('222','pottnI mdhy swtwadhikarI cirsthayI mOk`rrI'),
    ('223','mdhy swtwadhikarI myadI myad mdhye khajana brriddhir ozOgy'),
    ('224','kOrrfa nisk`r'),
    ('225','rayt myadI'),
    ('226','kOrrfa dkhliswtwbiSiSHT niSHk`r'),
    ('227','kOrrfa dkhliswtw'),
    ('228','mdhy swtwadhikarI cak`raN'),
    ('229','niSHk`r mdhy swtwadhikarI cirsthayI nhe'),
    ('230','niSHk`r mdhy swtwadhikarI cirsthayI'),
    ('231','mdhyswtwadhikarI cirsthayI niSHk`r debtr'),
    ('232','mdhyswtwadhikarI cirsthayI niSHk`r brhmtr'),
    ('233','dkhl`kar cak`ran mNDl'),
    ('234','niSHk`r mdhy swtwadhikarI cirsthayI debtr'),
    ('235','niSHk`r mdhy swtwadhikarI cirsthayI pirOttor'),
    ('236','rayt sthitiban debtr'),
    ('237','niSHk`r rayt sthitiban'),
    ('238','niSHk`r mdhy swtwadhikarI cirsthayI mhatraN'),
    ('239','dkhl`kar cak`ran begarI'),
    ('240','niSHk`r rayt sthitiban debOtt`r'),
    ('241','rayt sthitiban kbuliyt mule mOk`rrI'),
    ('242','niSHk`r mdhy swtwadhikarI cirsthayI bOIdyOtt`r'),
    ('243','rayt sthitiban paTTamUle mOk`rrI'),
    ('244','niSHk`r rayt sthitiban bOISHNbOttor'),
    ('245','niSHk`r rayt sthitiban vTTOtt`r'),
    ('246','rayt sthitiban brHMOtt`r'),
    ('247','mdhy swtwadhikarI cirsthayI niSHk`r brhmtr'),
    ('248','niSHk`r rayt sthitiban brHMOtt`r'),
    ('249','pottnI mdhyswtwadhikarI cirsthayI khajana brriddhir zOgy'),
    ('250','mdhy swtwadhikarI cirsthayI n`he myad mdhye khajana brriddhir ozOgy'),
    ('251','mdhyswtwadhikarI cirsthayI n`he meyad`mdhyekhajanabrriddhir ozOgy'),
    ('252','mdhyswtw cirsthayI mOk`rrI'),
    ('253','mdhyswtw cirsthayI'),
    ('254','mdhyswtw cirsthayI khajana brriddhir zOgy'),
    ('255','malikI'),
    ('256','niSHk`r mdhy swtwadhikarI cirsthayI SibOtt`r'),
    ('257','rayt sthitiban dakhilay swIkrrit mule mOk`rrI'),
    ('258','rayti dkhlIswtwbiSiSHT'),
    ('259','kOrrfa dkhliswtwSuny'),
    ('260','jol`kor'),
    ('261','rayti mOk`rrI'),
    ('262','rayti'),
    ('263','rayti dkhliswtwSuny'),
    ('264','dkhl`kar cak`raN khid`mt``dar'),
    ('265','dkhl`kar cak`raN muci'),
    ('266','rayt sthitiban chOlesutre mOk`rrI'),
    ('267','dkhl`kar candina cirsthayI'),
    ('268','rayt sthitiban nirikh mOk`rrI'),
    ('269','dkhl`kar cak`raN jagir m`nDlan'),
    ('270','niSHk`r rayt'),
    ('271','cak`raN dkhl`kar'),
    ('272','dkhl`kar bs`t cirsthayI'),
    ('273','niSHk`r dkhl`kar bs`t'),
    ('274','mdhyswtw myadI'),
    ('275','mdhyswtw cirsthayI n`he khajana brriddhir zOgy'),
    ('276','kOrrfa puruSHanukrme'),
    ('277','rayt niSHk`r vOgOtt`r'),
    ('278','kOrrfa mOk`rrI'),
    ('279','dkhl`kar debtr niSHk`r'),
    ('280','dkhl`kar jl`k`r'),
    ('281','dkhl`kar cirsthayI'),
    ('282','dkhl`kar bs`tprja cirsthayI'),
    ('283','dkhl`kar paTTamUle mOk`rrI'),
    ('284','dkhl`kar cirsthayI debOtt`r'),
    ('285','mdhyswtwadhikarI khajana brriddhir zOgy'),
    ('286','mdhyswtwadhikarI cirsthayI b`rrttman myad onte khajana brriddhir zOgy'),
    ('287','dkhl`kar napit'),
    ('288','mdhyswtwadhikarI osthayI myad onte khajana brriddhir zOgy'),
    ('289','niSHk`r kOrrfa'),
    ('290','mdhyswtw cirsthayI mOk`rrI brriddhir zOgy'),
    ('291','mdhyswtw'),
    ('292','mdhy swtw osthayI'),
    ('293','mdhyswtw myadi myad mdhye mOk`rrI'),
    ('294','mdhyswtw cirsthayI niSHk`r'),
    ('295','rayti dkhliswtwbihin'),
    ('296','rayt debtt`rdar'),
    ('297','rayt brHMOtt`rdar'),
    ('298','rayt brHMtt`r'),
    ('299','rayt niSHk`rdar'),
    ('300','mdhyswtwadhikarI osthayI myad mdhye khajana brriddhir ozOgy'),
    ('301','mdhyswtwadhikarI cirsthayI myad mdhye mOk`rrI'),
    ('302','mdhyswtwadhikarI cirsthayI nirikh mOk`rrI'),
    ('303','rayt nirikh mOk`rrI'),
    ('304','mdhyswtwadhikarI osthayI myad mdhye mOk`rrI'),
    ('305','mdhyswtw cirsthayI brriddhir zOgy'),
    ('306','mdhyswtwadhikarI cirsthayI debOtt`r'),
    ('307','mdhyswtwadhikarI cirsthayI n`he myad mdhye mOk`rrI'),
    ('308','mdhyswtwadhikarI cirsthayI brHMOtt`r'),
    ('309','mdhyswtwadhikarI cirsthayI niSHk`rdar'),
    ('310','mdhyswtwadhikarI cirsthayI SibOtt`r'),
    ('311','dor kOrrfa dkhliswtwbiSiSHT'),
    ('312','mdhyswtwadhikarI cirsthayI bOISHNbOttor'),
    ('313','ghas`kor'),
    ('314','niSHk`r cirsthayI nhe'),
    ('315','cak`raN mdhyswtw'),
    ('316','ijaradar'),
    ('317','cak`raN kOrrfa'),
    ('318','mdhyswtw myadI myad mdhye khajana brriddhir ozOgy'),
    ('319','myadI ijara myad mdhye mOk`rrI'),
    ('320','mdhyswtwadhikarI cirsthayI mhatraN'),
    ('321','mdhyswtwadhikarI cirsthayI pirOtt`r'),
    ('322','mdhyswtwadhikarI cirsthayI khajana brriddhir ozOgy'),
    ('323','myadI')
    ;

    -- insert the corresponding proja-sotter-type code into master_code_cs table
    insert into master_code_cs
    select mm.master_id , pt.code from metadata_master_cs mm 
    inner join temp_new_proja_sotter_type pt
    on mm.val=pt.type_name
    and mm.field_name='proja-sotter-type'
    and master_id not in (select master_id from master_code_cs);

    -- SECTION 4: Identifying and Removing Unmatched proja-sotter-type Entries Across metadata_master_cs and master_code_cs Tables
    -- Step 1: Create a temporary table to hold unmatched master_ids
    CREATE TEMPORARY TABLE temp_unmatched_master_ids (
        master_id VARBINARY(16) NOT NULL
    );

    -- Step 2: Populate the temporary table with unmatched master_ids from metadata_master_cs
    INSERT INTO temp_unmatched_master_ids (master_id)
    SELECT mm.master_id
    FROM metadata_master_cs mm
    LEFT JOIN temp_new_proja_sotter_type tn ON mm.val = tn.type_name
    WHERE mm.field_name = 'proja-sotter-type'
      AND mm.lang = 'BnPhonetic'
      AND tn.type_name IS NULL;

    -- Step 3: Delete from master_code_cs using the master_ids from the temporary table
    DELETE mc
    FROM master_code_cs mc
    WHERE mc.master_id IN (
        SELECT master_id FROM temp_unmatched_master_ids
    );

    -- Step 4: Delete from metadata_master_cs using the master_ids from the temporary table
    DELETE mm
    FROM metadata_master_cs mm
    WHERE mm.master_id IN (
         SELECT master_id FROM temp_unmatched_master_ids
    );

    -- Step 5: Drop the temporary table
    DROP TEMPORARY TABLE temp_unmatched_master_ids;

    drop temporary table if exists temp_new_proja_sotter_type;
 
    RETURN 'Execution done successfully.';

END$$

DELIMITER ;

-- Check if there's exactly one parent master_id
select 
    case 
        when count(*) = 1 then 0
        when count(*) = 0 then 1
	else 2
    end into @error_check
from metadata_master_cs 
where field_name='district' and parent_master_id is null and lang='BnPhonetic';

-- Execute the function only if parent master_id check passes
select 
    case 
        when @error_check = 1 then 'No parent master ID found. Script execution should be aborted.'
        when @error_check = 2 then 'Multiple parent master IDs found. Script execution should be aborted.'
        else InsertProjaSotterTypeEntries()
    end as Status;