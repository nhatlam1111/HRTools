using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Helpers
{
    public static class Helper
    {
        public static Dictionary<string, string> clientList = new Dictionary<string, string>();

        public static void LoadTnsFile()
        {
            string tnsAdmin = Environment.GetEnvironmentVariable("TNS_ADMIN");
            if (string.IsNullOrEmpty(tnsAdmin))
            {
                MessageBox.Show("TNS_ADMIN is not set. Please set TNS_ADMIN environment variable.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string tnsFilePath = Path.Combine(tnsAdmin, "tnsnames.ora");
            if (!File.Exists(tnsFilePath))
            {
                MessageBox.Show($"TNS file not found at: {tnsFilePath}", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            clientList = ParseTnsFile(tnsFilePath);
        }

        private static Dictionary<string, string> ParseTnsFile(string filePath)
        {
            var connections = new Dictionary<string, string>();
            var content = File.ReadAllText(filePath);

            // Improved regex using nested parentheses matching
            var regex = new Regex(@"(?<key>\w+)\s*=\s*(?<value>\((?>\((?<depth>)|\)(?<-depth>)|[^()])*(?(depth)(?!))\))", RegexOptions.Singleline);

            var matches = regex.Matches(content);
            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    string key = match.Groups["key"].Value.Trim();
                    string value = match.Groups["value"].Value.Trim();

                    if (!connections.ContainsKey(key))
                    {
                        connections[key] = value;
                    }
                }
            }
            return connections;
        }

        public static void ReleaseMemory()
        {
            try
            {
                //long before = GC.GetTotalMemory(false);

                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, true);
                GC.WaitForPendingFinalizers();
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true, true);


                //long after = GC.GetTotalMemory(false);
                //MessageBox.Show($"Memory before GC: {before / 1024} KB\nMemory after GC: {after / 1024} KB", "Memory Usage", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error releasing memory: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static string ReplaceText(string text, DataRow model)
        {
            string _text = text;

            try
            {
                foreach (DataColumn c in model.Table.Columns)  //loop through the columns. 
                {
                    string str = string.Format("$[{0}]", c.ColumnName.ToLower());
                    _text = _text.Replace(str, ObjectToString(model[c.ColumnName]));
                }
            }
            catch
            { }

            return _text;
        }

        public static string ReplaceText<T>(string text, T model)
        {
            string _text = text;
            try
            {
                // Lấy tất cả các thuộc tính của đối tượng T
                var properties = typeof(T).GetProperties();

                foreach (var property in properties)
                {
                    string str = string.Format("$[{0}]", property.Name);
                    _text = _text.Replace(str, ObjectToString(property.GetValue(model)));
                }
            }
            catch (Exception ex)
            {
                // Có thể log lỗi ở đây thay vì bỏ qua lỗi
                Console.WriteLine($"Lỗi khi thay thế text: {ex.Message}");
            }
            return _text;
        }


        public static string ObjectToString(object value)
        {
            if (value == null) return "";

            string _value = "";
            string _type = value.GetType().Name;

            switch (_type)
            {
                case "DateTime": _value = ((DateTime)value).ToString("yyyyMMddHH:mm:ss.ff"); break;
                case "DBNull": _value = null; break;
                default: _value = value?.ToString(); break;
            }

            return _value;
        }

        public static List<T> DatatableToList<T>(DataTable dt) where T : new()
        {
            List<T> list = new List<T>();

            foreach (DataRow row in dt.Rows)
            {
                T obj = new T();

                var properties = typeof(T).GetProperties();

                foreach (var prop in properties)
                {
                    if (dt.Columns.Contains(prop.Name) && row[prop.Name] != DBNull.Value)
                    {
                        try
                        {
                            prop.SetValue(obj, row[prop.Name]);
                        }
                        catch (Exception e)
                        {
                            Log.Error(e.Message);
                        }

                    }
                }

                list.Add(obj);
            }

            return list;
        }


        /// <summary>
        /// Ghi đối tượng xuống file với mỗi thuộc tính trên một dòng và mã hóa nội dung
        /// </summary>
        /// <typeparam name="T">Kiểu của đối tượng cần ghi</typeparam>
        /// <param name="obj">Đối tượng cần ghi</param>
        /// <param name="filePath">Đường dẫn đến file</param>
        public static void WriteObjectToFile<T>(T obj, string filePath)
        {
            try
            {
                // Tạo nội dung từ các thuộc tính của đối tượng
                StringBuilder contentBuilder = new StringBuilder();

                // Lấy tất cả các thuộc tính public của đối tượng
                PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (PropertyInfo property in properties)
                {
                    // Lấy giá trị của thuộc tính
                    object value = property.GetValue(obj);
                    string valueString = value?.ToString() ?? string.Empty;

                    // Thêm vào định dạng [tên_thuộc_tính]=[giá_trị]
                    contentBuilder.AppendLine($"[{property.Name}]={valueString}");
                }

                // Mã hóa nội dung
                string encryptedContent = EncryptionHelper.Encrypt(contentBuilder.ToString(), true);

                // Ghi file
                File.WriteAllText(filePath, encryptedContent);

                Console.WriteLine($"Đã ghi file thành công: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi ghi file: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Đọc file đã mã hóa và chuyển đổi nội dung thành đối tượng
        /// </summary>
        /// <typeparam name="T">Kiểu của đối tượng cần đọc</typeparam>
        /// <param name="filePath">Đường dẫn đến file</param>
        /// <returns>Đối tượng được khôi phục từ file</returns>
        public static T ReadObjectFromFile<T>(string filePath, bool isEncrypted) where T : new()
        {
            try
            {
                // Đọc nội dung file đã mã hóa
                string encryptedContent = File.ReadAllText(filePath);
                string decryptedContent = encryptedContent;
                if (isEncrypted)
                {
                    // Giải mã nội dung
                    decryptedContent = EncryptionHelper.Decrypt(encryptedContent, true);
                }
                    
                // Khởi tạo đối tượng mới
                T obj = new T();

                // Đọc từng dòng
                string[] lines = decryptedContent.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string line in lines)
                {
                    // Tìm vị trí của dấu "]=" để tách tên thuộc tính và giá trị
                    int separatorIndex = line.IndexOf("]=");
                    if (separatorIndex <= 1) continue; // Bỏ qua dòng không hợp lệ

                    // Tách tên thuộc tính (bỏ dấu "[" ở đầu)
                    string propertyName = line.Substring(1, separatorIndex - 1);

                    // Tách giá trị (sau "]=")
                    string valueString = line.Substring(separatorIndex + 2);

                    // Lấy thông tin thuộc tính
                    PropertyInfo property = typeof(T).GetProperty(propertyName);
                    if (property == null) continue; // Bỏ qua nếu không tìm thấy thuộc tính

                    // Chuyển đổi giá trị về đúng kiểu và gán cho thuộc tính
                    object typedValue = ConvertToPropertyType(valueString, property.PropertyType);
                    property.SetValue(obj, typedValue);
                }

                Console.WriteLine($"Đã đọc file thành công: {filePath}");
                return obj;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi đọc file: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Chuyển đổi chuỗi thành kiểu dữ liệu tương ứng của thuộc tính
        /// </summary>
        private static object ConvertToPropertyType(string value, Type targetType)
        {
            // Xử lý trường hợp giá trị null
            if (string.IsNullOrEmpty(value))
            {
                return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
            }

            // Xử lý các kiểu dữ liệu phổ biến
            if (targetType == typeof(string))
                return value;
            else if (targetType == typeof(int) || targetType == typeof(int?))
                return int.TryParse(value, out int intValue) ? intValue : 0;
            else if (targetType == typeof(double) || targetType == typeof(double?))
                return double.TryParse(value, out double doubleValue) ? doubleValue : 0.0;
            else if (targetType == typeof(decimal) || targetType == typeof(decimal?))
                return decimal.TryParse(value, out decimal decimalValue) ? decimalValue : 0m;
            else if (targetType == typeof(bool) || targetType == typeof(bool?))
                return bool.TryParse(value, out bool boolValue) ? boolValue : false;
            else if (targetType == typeof(DateTime) || targetType == typeof(DateTime?))
                return DateTime.TryParse(value, out DateTime dateValue) ? dateValue : DateTime.MinValue;
            else if (targetType.IsEnum)
                return Enum.TryParse(targetType, value, true, out object enumValue) ? enumValue : Activator.CreateInstance(targetType);

            // Thêm các kiểu dữ liệu khác nếu cần

            // Trường hợp mặc định
            return Convert.ChangeType(value, targetType);
        }
    }
}
