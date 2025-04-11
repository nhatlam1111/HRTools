#ACCESS
[ACCESS_SELECT_ATTENDANCE]=select USERID as USER_ID, '' AS CARD_ID, Format(CHECKTIME, 'yyyymmdd') as WORK_DT, Format(CHECKTIME, 'HH:mm:ss') as WORK_TIME, Format(CHECKTIME, 'yyyymmddHHmmss') as WORK_DATE_FULL, CHECKTYPE as EVT, sn as MACHINE_ID from CHECKINOUT where Format(CHECKTIME, 'yyyymmdd') between '$[from_dt]' and '$[to_dt]'




#ORACLE
[ORACLE_SELECT_ATTENDANCE]=select id as USER_ID, CARD_NO AS CARD_ID, WORK_DT, time as WORK_TIME, WORK_DT_FULL AS WORK_DATE_FULL, EVENT AS EVT, LOCATION AS MACHINE_ID FROM THR_TIME_TEMP WHERE WORK_DT between '$[from_dt]' and '$[to_dt]'

[ORACLE_INSERT_ATTENDANCE]=insert into thr_time_temp( pk, id, work_dt, time, location, crt_by, crt_dt, WORK_DT_FULL) VALUES (thr_time_temp_seq.nextval, '$[USER_ID]', '$[WORK_DT]', '$[WORK_TIME]', '$[MACHINE_ID]', 'auto-sync', sysdate, '$[WORK_DATE_FULL]'))