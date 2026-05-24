CREATE OR REPLACE procedure HR_PRO_TRANSFER_FROM_ALPETA
is
    l_from_dt date := sysdate - 15;
    l_to_dt date:= sysdate;
begin


    insert into thr_time_temp 
    (
        pk
        , id
        , full_name
        , card_no
        , work_dt
        , time
        , location
        , work_dt_full
        , crt_dt
        , crt_by
    )
    select thr_time_temp_seq.nextval
        , qq.USER_ID
        , qq.USER_NAME
        , qq.CARD_ID
        , qq.work_dt
        , qq.time
        , qq.TERMINAL_ID
        , qq.work_dt_full
        , qq.CREATED_DATE
        , qq.crt_by
    from (
        select distinct
             q.USER_ID
            , q.USER_NAME
            , q.CARD_ID
            , to_char(q.EVENT_TIME, 'yyyymmdd') as work_dt
            , to_char(q.EVENT_TIME, 'hh24:mi') as time
            , q.TERMINAL_ID
            , to_char(q.EVENT_TIME, 'yyyymmddhh24miss') as work_dt_full
            , q.CREATED_DATE
            , 'auto-get-data-from-alpeta' as crt_by
        from thr_terminal_logs q, thr_unis_user w
        where q.EVENT_TIME between l_from_dt and l_to_dt
        and w.DEL_IF = 0 and w.USER_ID = q.USER_ID
        and not exists (
            select 1
            from thr_time_temp w
            where w.ID = q.USER_ID
            and w.WORK_DT = to_char(q.EVENT_TIME, 'yyyymmdd')
            and w.TIME = to_char(q.EVENT_TIME, 'hh24:mi')
        )
    ) qq
    
    ;

end;
/