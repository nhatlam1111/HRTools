#MODEL TREN C#, tất cả các trường trên model đều là dạng string, nên sql cũng phải tự parse về string, hoặc từ string parse sang các dạng trên db (do mỗi db 1 kiểu nên ko nên làm động kiểu dữ liệu)
#UserModel: USER_ID, USER_NAME, FULL_NAME, LAST_NAME, CARD_ID, POSITION, PASSWORD, SEX, BIRTH_DATE, BIRTH_PLACE, JOIN_DATE, ADDRESS, PHONE, EMAIL, NATION, CITIZEN_ID, CITIZEN_DATE, CITIZEN_PLACE, REMARK, ACTIVE_YN
#AttendanceModel: USER_ID, USER_NAME, CARD_ID, WORK_DATE_FULL, WORK_DATE, WORK_TIME, EVT, MACHINE_ID, MACHINE_NAME, MACHINE_IP, MACHINE_TYPE, REMARK
#$[from_dt]: format YYYYMMDD, tính từ ngày hiện tại - sync days
#$[to_dt]: format YYYYMMDD là ngày hiện tại

#ACCESS
[ACCESS_SELECT_ATTENDANCE]=select CStr(USERID) as USER_ID, '' AS CARD_ID, Format(CHECKTIME, 'yyyymmdd') as WORK_DATE, Format(CHECKTIME, 'HH:mm') as WORK_TIME, Format(CHECKTIME, 'yyyymmddHHmmss') as WORK_DATE_FULL, CHECKTYPE as EVT, sn as MACHINE_ID from CHECKINOUT where Format(CHECKTIME, 'yyyymmdd') between '$[from_dt]' and '$[to_dt]'
[ACCESS_SELECT_USER]
[ACCESS_INSERT_USER]


#ORACLE
[ORACLE_SELECT_ATTENDANCE]=SELECT Q.ID AS USER_ID, Q.CARD_NO AS CARD_ID, Q.WORK_DT AS WORK_DATE, Q.TIME AS WORK_TIME FROM thr_time_temp Q WHERE Q.WORK_DT BETWEEN '$[from_dt]' and '$[to_dt]'
[ORACLE_INSERT_ATTENDANCE]=insert into thr_time_temp( pk, id, work_dt, time, location, crt_by, crt_dt) select thr_time_temp_seq.nextval, '$[USER_ID]', '$[WORK_DATE]', '$[WORK_TIME]', '$[MACHINE_ID]', 'auto-sync', sysdate from dual where not exists (select 1 from thr_time_temp q where q.id = '$[USER_ID]' and q.work_dt = '$[WORK_DATE]' and q.time = '$[WORK_TIME]')
#[ORACLE_SELECT_USER]=select q.USER_ID, q.USER_NAME, q.CARD_ID, q.PASSWORD from thr_unis_user q where q.del_if = 0 and nvl(q.UPLOAD_YN, 'N') = 'N'
#[ORACLE_INSERT_USER]: hệ thống của mình hiện tại ko insert ngược từ phần mềm khác vào
#[ORACLE_UPDATE_USER]=update thr_unis_user q set q.remark = nvl('$[REMARK]', q.remark), q.upload_yn = 'Y', mod_by = 'auto-sync', mod_dt = sysdate where q.del_if = 0 and q.user_id = '$[USER_ID]'


select id as USER_ID, 