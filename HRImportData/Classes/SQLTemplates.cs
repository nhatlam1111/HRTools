namespace HRImportData.Classes
{
    internal static class SQLTemplates
    {
        public const string ORACLE_USER_LOGIN_NODEJS = "select user_pw from tes_user q where q.del_if = 0 and lower(user_id) = lower('{0}') and nvl(use_yn, 'N') = 'Y' and nvl(SYSADMIN_YN, 'N') = 'Y' ";
        public const string ORACLE_USER_LOGIN_GASP = "select user_pw from GASP.tes_user q where q.del_if = 0 and lower(user_id) = lower('{0}') and nvl(use_yn, 'N') = 'Y' and nvl(SYSADMIN_YN, 'N') = 'Y' ";
        public const string ORACLE_USER_LOGIN_ESYS = "select user_pw from COMM.TCO_BSUSER q where q.del_if = 0 and lower(user_id) = lower('{0}') and nvl(use_yn, 'N') = 'Y' and nvl(SYSADMIN_YN, 'N') = 'Y' ";

        public const string ORACLE_SALARY_IMPORT_DATA = "update {0} set {1} where {2}";

        public const string ORACLE_TIME_TEMP_IMPORT = "insert into thr_time_temp" +
            "(" +
            "pk, id, work_dt, time, location, crt_by, remark" +
            ") " +
            "values" +
            "(" +
            "thr_time_temp_seq.nextval, '{0}', '{1}', '{2}', '{3}', '{4}', '{5}'" +
            ")";

        public const string ORACLE_TABLE_DBMAPPING = "TMP_DBMAPPING";
        public const string ORACLE_TABLE_DBMAPPING_CREATE = "CREATE TABLE TMP_DBMAPPING({0})"; //{0}: COLUMN LIST FROM DBMapping COLUMN BY IMPORT
        public const string ORACLE_TABLE_DBMAPPING_DROP = "DROP TABLE TMP_DBMAPPING";
        public const string ORACLE_TABLE_DBMAPPING_INSERT = "insert  into TMP_DBMAPPING({0}) values ({1})";
        public const string ORACLE_SELECT_BACKUP_DB_DATA = "SELECT {0} FROM {1} WHERE {2}";
        public const string ORACLE_SELECT_BACKUP_DB_TABLE = "SELECT * FROM {0}";

        public const string ORACLE_UPDATE_IMPORT_DATA = "update {0} set {1} where {2}";
        public const string ORACLE_INSERT_IMPORT_DATA = "insert into {0}({1}) values ({2})";
    }
}
