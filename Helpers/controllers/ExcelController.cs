using Helpers.classes;
using NPOI.XSSF.UserModel;
using System.Data;

namespace Helpers.controllers
{
    internal static class ExcelController
    {
        public static List<string> ColumnInExcel = new List<string> {
            "index", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
            "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ",
            "BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH", "BI", "BJ", "BK", "BL", "BM", "BN", "BO", "BP", "BQ", "BR", "BS", "BT", "BU", "BV", "BW", "BX", "BY", "BZ",
            "CA", "CB", "CC", "CD", "CE", "CF", "CG", "CH", "CI", "CJ", "CK", "CL", "CM", "CN", "CO", "CP", "CQ", "CR", "CS", "CT", "CU", "CV", "CW", "CX", "CY", "CZ",
            "DA", "DB", "DC", "DD", "DE", "DF", "DG", "DH", "DI", "DJ", "DK", "DL", "DM", "DN", "DO", "DP", "DQ", "DR", "DS", "DT", "DU", "DV", "DW", "DX", "DY", "DZ",
            "EA", "EB", "EC", "ED", "EE", "EF", "EG", "EH", "EI", "EJ", "EK", "EL", "EM", "EN", "EO", "EP", "EQ", "ER", "ES", "ET", "EU", "EV", "EW", "EX", "EY", "EZ",
            "FA", "FB", "FC", "FD", "FE", "FF", "FG", "FH", "FI", "FJ", "FK", "FL", "FM", "FN", "FO", "FP", "FQ", "FR", "FS", "FT", "FU", "FV", "FW", "FX", "FY", "FZ",
            "GA", "GB", "GC", "GD", "GE", "GF", "GG", "GH", "GI", "GJ", "GK", "GL", "GM", "GN", "GO", "GP", "GQ", "GR", "GS", "GT", "GU", "GV", "GW", "GX", "GY", "GZ",
            "HA", "HB", "HC", "HD", "HE", "HF", "HG", "HH", "HI", "HJ", "HK", "HL", "HM", "HN", "HO", "HP", "HQ", "HR", "HS", "HT", "HU", "HV", "HW", "HX", "HY", "HZ",
            "IA", "IB", "IC", "ID", "IE", "IF", "IG", "IH", "II", "IJ", "IK", "IL", "IM", "IN", "IO", "IP", "IQ", "IR", "IS", "IT", "IU", "IV", "IW", "IX", "IY", "IZ",
            "JA", "JB", "JC", "JD", "JE", "JF", "JG", "JH", "JI", "JJ", "JK", "JL", "JM", "JN", "JO", "JP", "JQ", "JR", "JS", "JT", "JU", "JV", "JW", "JX", "JY", "JZ",
            "KA", "KB", "KC", "KD", "KE", "KF", "KG", "KH", "KI", "KJ", "KK", "KL", "KM", "KN", "KO", "KP", "KQ", "KR", "KS", "KT", "KU", "KV", "KW", "KX", "KY", "KZ",
            "LA", "LB", "LC", "LD", "LE", "LF", "LG", "LH", "LI", "LJ", "LK", "LL", "LM", "LN", "LO", "LP", "LQ", "LR", "LS", "LT", "LU", "LV", "LW", "LX", "LY", "LZ",
            "MA", "MB", "MC", "MD", "ME", "MF", "MG", "MH", "MI", "MJ", "MK", "ML", "MM", "MN", "MO", "MP", "MQ", "MR", "MS", "MT", "MU", "MV", "MW", "MX", "MY", "MZ",
            "NA", "NB", "NC", "ND", "NE", "NF", "NG", "NH", "NI", "NJ", "NK", "NL", "NM", "NN", "NO", "NP", "NQ", "NR", "NS", "NT", "NU", "NV", "NW", "NX", "NY", "NZ",
            "OA", "OB", "OC", "OD", "OE", "OF", "OG", "OH", "OI", "OJ", "OK", "OL", "OM", "ON", "OO", "OP", "OQ", "OR", "OS", "OT", "OU", "OV", "OW", "OX", "OY", "OZ",
            "PA", "PB", "PC", "PD", "PE", "PF", "PG", "PH", "PI", "PJ", "PK", "PL", "PM", "PN", "PO", "PP", "PQ", "PR", "PS", "PT", "PU", "PV", "PW", "PX", "PY", "PZ",
            "QA", "QB", "QC", "QD", "QE", "QF", "QG", "QH", "QI", "QJ", "QK", "QL", "QM", "QN", "QO", "QP", "QQ", "QR", "QS", "QT", "QU", "QV", "QW", "QX", "QY", "QZ",
            "RA", "RB", "RC", "RD", "RE", "RF", "RG", "RH", "RI", "RJ", "RK", "RL", "RM", "RN", "RO", "RP", "RQ", "RR", "RS", "RT", "RU", "RV", "RW", "RX", "RY", "RZ",
            "SA", "SB", "SC", "SD", "SE", "SF", "SG", "SH", "SI", "SJ", "SK", "SL", "SM", "SN", "SO", "SP", "SQ", "SR", "SS", "ST", "SU", "SV", "SW", "SX", "SY", "SZ",
            "TA", "TB", "TC", "TD", "TE", "TF", "TG", "TH", "TI", "TJ", "TK", "TL", "TM", "TN", "TO", "TP", "TQ", "TR", "TS", "TT", "TU", "TV", "TW", "TX", "TY", "TZ",
            "UA", "UB", "UC", "UD", "UE", "UF", "UG", "UH", "UI", "UJ", "UK", "UL", "UM", "UN", "UO", "UP", "UQ", "UR", "US", "UT", "UU", "UV", "UW", "UX", "UY", "UZ",
            "VA", "VB", "VC", "VD", "VE", "VF", "VG", "VH", "VI", "VJ", "VK", "VL", "VM", "VN", "VO", "VP", "VQ", "VR", "VS", "VT", "VU", "VV", "VW", "VX", "VY", "VZ",
            "WA", "WB", "WC", "WD", "WE", "WF", "WG", "WH", "WI", "WJ", "WK", "WL", "WM", "WN", "WO", "WP", "WQ", "WR", "WS", "WT", "WU", "WV", "WW", "WX", "WY", "WZ",
            "XA", "XB", "XC", "XD", "XE", "XF", "XG", "XH", "XI", "XJ", "XK", "XL", "XM", "XN", "XO", "XP", "XQ", "XR", "XS", "XT", "XU", "XV", "XW", "XX", "XY", "XZ",
            "YA", "YB", "YC", "YD", "YE", "YF", "YG", "YH", "YI", "YJ", "YK", "YL", "YM", "YN", "YO", "YP", "YQ", "YR", "YS", "YT", "YU", "YV", "YW", "YX", "YY", "YZ",
            "ZA", "ZB", "ZC", "ZD", "ZE", "ZF", "ZG", "ZH", "ZI", "ZJ", "ZK", "ZL", "ZM", "ZN", "ZO", "ZP", "ZQ", "ZR", "ZS", "ZT", "ZU", "ZV", "ZW", "ZX", "ZY", "ZZ",
        };

        public static XSSFWorkbook DataTableToWorkbook(DataTable dataTable)
        { 
            var wb = new XSSFWorkbook();
            var sheet = wb.CreateSheet();

            var columnCount = dataTable.Columns.Count;
            var rowCount = dataTable.Rows.Count;

            // add column headers
            var row = sheet.CreateRow(0);
            for (var columnIndex = 0; columnIndex < columnCount; columnIndex++)
            {
                var col = dataTable.Columns[columnIndex];
                row.CreateCell(columnIndex).SetCellValue(col.ColumnName);
            }

            // add data rows
            for (var rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                var dataRow = dataTable.Rows[rowIndex];
                var sheetRow = sheet.CreateRow(rowIndex + 1);
                for (var columnIndex = 0; columnIndex < columnCount; columnIndex++)
                {
                    sheetRow.CreateCell(columnIndex).SetCellValue(dataRow[columnIndex]+"");
                }
            }

            return wb;
        }

        public static ExcelWorkbook ReadWorkbook(string filePath)
        {
            ExcelWorkbook excel = new ExcelWorkbook();
            excel.workBookPath = filePath;

            try
            {
                using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    excel.workBook = new XSSFWorkbook(file);

                    if (excel.workBook != null)
                    {
                        excel.workSheets = new List<ExcelWorkSheet>();
                        for (int i = 0; i < excel.workBook.NumberOfSheets; i++)
                        {
                            ExcelWorkSheet sheet = ReadWorkSheet(excel, i);
                            excel.workSheets.Add(sheet);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //log
                throw;
            }

            return excel;
        }

        public static ExcelWorkSheet ReadWorkSheet(ExcelWorkbook excel, int sheetIndex)
        {
            ExcelWorkSheet sheet = new ExcelWorkSheet 
            { 
                index = sheetIndex, 
                name = excel.workBook.GetSheetName(sheetIndex), 
                sheet = excel.workBook.GetSheetAt(sheetIndex) 
            };


            int firstCol = 0;
            int firstRow = 0;
            int lastRow = sheet.sheet.LastRowNum;
            int lastCol = 0;

            //find last col
            for (int rowIdx = 0; rowIdx <= lastRow; rowIdx++)
            {
                var row = sheet.sheet.GetRow(rowIdx);
                if (row != null) lastCol = lastCol < row.LastCellNum ? row.LastCellNum : lastCol;
            }

            DataTable dt = new DataTable();

            for (int i = 0; i <= lastCol; i++)
            {
                DataColumn idColumn = new DataColumn();
                idColumn.DataType = typeof(object);
                idColumn.ColumnName = ColumnInExcel[i];
                idColumn.DefaultValue = null;
                dt.Columns.Add(idColumn);
            }

            //read data
            for (int rowIdx = 0; rowIdx <= lastRow; rowIdx++)
            {
                var row = sheet.sheet.GetRow(rowIdx);
                var dtRow = dt.NewRow();
                if (row != null)
                {
                    int excelCol = 0;
                    dtRow[excelCol++] = rowIdx + 1;

                    for (int colIdx = 0; colIdx <= lastCol; colIdx++)
                    {
                        var cell = row.GetCell(colIdx);

                        if (cell != null)
                        {
                            var cellType = cell.CellType;

                            switch (cell.CellType)
                            {
                                case NPOI.SS.UserModel.CellType.Numeric:
                                    dtRow[excelCol] = cell.NumericCellValue; break;
                                case NPOI.SS.UserModel.CellType.Boolean:
                                    dtRow[excelCol] = cell.BooleanCellValue; break;
                                case NPOI.SS.UserModel.CellType.Formula:
                                    switch (cell.CachedFormulaResultType)
                                    {
                                        case NPOI.SS.UserModel.CellType.Numeric:
                                            dtRow[excelCol] = cell.NumericCellValue; break;
                                        case NPOI.SS.UserModel.CellType.Boolean:
                                            dtRow[excelCol] = cell.BooleanCellValue; break;
                                        default:
                                            try { dtRow[excelCol] = cell.StringCellValue;  } catch { }
                                            break;
                                    }
                                    break;
                                default:
                                    try { dtRow[excelCol] = cell.StringCellValue; } catch { }
                                    break;
                            }
                        }
                        excelCol++;
                    }
                }
                dt.Rows.Add(dtRow);
            }

            sheet.datas = dt;

            return sheet;
        }
    }
}
