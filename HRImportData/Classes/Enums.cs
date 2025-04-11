namespace HRImportData.Classes
{
    internal class Enums
    {
    }

    public enum SITE_VERSION
    { 
        NODEJS,
        GASP,
        ESYS
    }

    public enum IMPORT_TYPE
    {
        SELECT_IMPORT_TYPE,
        UPDATE_EMPLOYEE,
        INSERT_EMPLOYEE,
        UPDATE_SALARY,
        INSERT_TIME_TEMP,
        INSERT_TABLE,
        UPDATE_TABLE
    }

    public static class APP_COLOR
    {
        public static readonly Color GRID_BACK_COLOR_IMPORT = Color.Khaki;
        public static readonly Color GRID_FORE_COLOR_IMPORT = Color.Black;
    }
}
