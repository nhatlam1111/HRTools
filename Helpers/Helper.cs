using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
        /// Ghi đối tượng xuống file với mỗi thuộc tính trên nhiều dòng kết thúc thuộc tính là dòng chứa dấu chấm phẩy cuối cùng và mã hóa nội dung
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

                    // Ghi theo format: [tên_thuộc_tính]=
                    contentBuilder.AppendLine($"[{property.Name}]=");

                    // Ghi giá trị (có thể nhiều dòng) và kết thúc bằng dấu chấm phẩy
                    contentBuilder.AppendLine($"{valueString};");
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
        /// <param name="isEncrypted">File có được mã hóa hay không</param>
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
                string[] lines = decryptedContent.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

                string currentPropertyName = null;
                StringBuilder currentValueBuilder = new StringBuilder();

                foreach (string line in lines)
                {
                    // Kiểm tra xem dòng có phải là dòng bắt đầu thuộc tính mới không
                    if (line.StartsWith("[") && line.Contains("]="))
                    {
                        // Nếu đang xử lý thuộc tính trước đó, gán giá trị cho nó
                        if (!string.IsNullOrEmpty(currentPropertyName))
                        {
                            SetPropertyValue(obj, currentPropertyName, currentValueBuilder.ToString());
                        }

                        // Bắt đầu thuộc tính mới
                        int separatorIndex = line.IndexOf("]=");
                        currentPropertyName = line.Substring(1, separatorIndex - 1);
                        currentValueBuilder.Clear();

                        // Nếu có nội dung sau dấu "]=" trên cùng dòng
                        string remainingContent = line.Substring(separatorIndex + 2);
                        if (!string.IsNullOrEmpty(remainingContent))
                        {
                            currentValueBuilder.AppendLine(remainingContent);
                        }
                    }
                    else if (!string.IsNullOrEmpty(currentPropertyName))
                    {
                        // Kiểm tra xem dòng có kết thúc bằng dấu chấm phẩy không
                        if (line.EndsWith(";"))
                        {
                            // Thêm nội dung (bỏ dấu chấm phẩy cuối) và kết thúc thuộc tính
                            string lineContent = line.Substring(0, line.Length - 1);
                            if (!string.IsNullOrEmpty(lineContent))
                            {
                                currentValueBuilder.AppendLine(lineContent);
                            }

                            // Gán giá trị cho thuộc tính
                            SetPropertyValue(obj, currentPropertyName, currentValueBuilder.ToString());

                            // Reset cho thuộc tính tiếp theo
                            currentPropertyName = null;
                            currentValueBuilder.Clear();
                        }
                        else
                        {
                            // Thêm dòng vào giá trị hiện tại
                            currentValueBuilder.AppendLine(line);
                        }
                    }
                }

                // Xử lý thuộc tính cuối cùng nếu còn dang dở
                if (!string.IsNullOrEmpty(currentPropertyName))
                {
                    SetPropertyValue(obj, currentPropertyName, currentValueBuilder.ToString());
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
        /// Gán giá trị cho thuộc tính của đối tượng
        /// </summary>
        private static void SetPropertyValue<T>(T obj, string propertyName, string valueString)
        {
            // Lấy thông tin thuộc tính
            PropertyInfo property = typeof(T).GetProperty(propertyName);
            if (property == null) return; // Bỏ qua nếu không tìm thấy thuộc tính

            // Loại bỏ các dòng trống ở cuối và đầu
            valueString = valueString.Trim();

            // Chuyển đổi giá trị về đúng kiểu và gán cho thuộc tính
            object typedValue = ConvertToPropertyType(valueString, property.PropertyType);
            property.SetValue(obj, typedValue);
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
            {
                try
                {
                    // Cách 1: Parse qua tên enum
                    return Enum.Parse(targetType, value, true);
                }
                catch
                {
                    try
                    {
                        // Cách 2: Parse qua giá trị số nguyên nếu cách 1 thất bại
                        int enumValue;
                        if (int.TryParse(value, out enumValue))
                        {
                            return Enum.ToObject(targetType, enumValue);
                        }
                    }
                    catch { }

                    // Trả về giá trị mặc định nếu không parse được
                    return Activator.CreateInstance(targetType);
                }
            }

            // Thêm các kiểu dữ liệu khác nếu cần

            // Trường hợp mặc định
            return Convert.ChangeType(value, targetType);
        }

        public static bool IsProcessOpen(string name)
        {
            //int count = 0;
            Process currentProcess = Process.GetCurrentProcess();
            foreach (Process clsProcess in Process.GetProcessesByName(name))
            {
                if (currentProcess.Id != clsProcess.Id)
                {

                    return true;
                }
            }

            return false;
        }

        public static void BindListToGrid<T>(BindingList<T> items, DataGridView grid, bool isreadonly)
        {
            bool isHaveImage = false;
            // Clear existing columns
            grid.Columns.Clear();

            // Use reflection to get properties of T
            var properties = typeof(T).GetProperties();

            // Dynamically create columns based on property types
            foreach (var property in properties)
            {
                DataGridViewColumn column;

                // Determine column type based on property type
                if (property.PropertyType == typeof(int) || property.PropertyType == typeof(long))
                {
                    column = new DataGridViewTextBoxColumn
                    {
                        ValueType = typeof(int),
                        HeaderText = property.Name,
                        DataPropertyName = property.Name,
                        ReadOnly = isreadonly
                    };
                }
                else if (property.PropertyType == typeof(string))
                {
                    column = new DataGridViewTextBoxColumn
                    {
                        ValueType = typeof(string),
                        HeaderText = property.Name,
                        DataPropertyName = property.Name,
                        ReadOnly = isreadonly
                    };
                }
                else if (property.PropertyType == typeof(bool))
                {
                    column = new DataGridViewCheckBoxColumn
                    {
                        ValueType = typeof(bool),
                        HeaderText = property.Name,
                        DataPropertyName = property.Name,
                        ReadOnly = isreadonly
                    };
                }
                else if (property.PropertyType == typeof(DateTime))
                {
                    column = new DataGridViewTextBoxColumn
                    {
                        ValueType = typeof(DateTime),
                        HeaderText = property.Name,
                        DataPropertyName = property.Name,
                        ReadOnly = isreadonly
                    };
                }
                else if (property.PropertyType == typeof(System.Drawing.Image))
                {
                    column = new DataGridViewImageColumn
                    {
                        ValueType = typeof(System.Drawing.Image),
                        HeaderText = property.Name,
                        DataPropertyName = property.Name,
                        ReadOnly = isreadonly,
                        ImageLayout = DataGridViewImageCellLayout.Zoom // Adjust image layout as needed
                    };
                    isHaveImage = true;
                }
                else
                {
                    // Default to text column for unsupported types
                    column = new DataGridViewTextBoxColumn
                    {
                        ValueType = property.PropertyType,
                        HeaderText = property.Name,
                        DataPropertyName = property.Name,
                        ReadOnly = isreadonly
                    };
                }

                column.Width = 100;
                grid.Columns.Add(column);
            }

            if (isHaveImage)
            {
                grid.RowTemplate.Height = 50; // Set row height for image display
            }

            // Set the data source of the DataGridView to the list of items
            grid.DataSource = items;
        }

        public static string RandomString(int length)
        {
            if (length <= 0)
                throw new ArgumentException("Length must be greater than 0.");

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder result = new StringBuilder(length);
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                result.Append(chars[random.Next(chars.Length)]);
            }

            return result.ToString();
        }

        public static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
        }

        /// <summary>
        /// Ghi danh sách đối tượng xuống file với mỗi đối tượng cách nhau bởi dấu phân cách và mã hóa nội dung
        /// </summary>
        /// <typeparam name="T">Kiểu của đối tượng cần ghi</typeparam>
        /// <param name="list">Danh sách đối tượng cần ghi</param>
        /// <param name="filePath">Đường dẫn đến file</param>
        /// <param name="isEncrypted">Có mã hóa file hay không</param>
        public static void WriteListObjectToFile<T>(List<T> list, string filePath, bool isEncrypted = true)
        {
            try
            {
                // Tạo nội dung từ danh sách đối tượng
                StringBuilder contentBuilder = new StringBuilder();

                // Lấy tất cả các thuộc tính public của đối tượng
                PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (T obj in list)
                {
                    // Ghi từng thuộc tính của đối tượng
                    foreach (PropertyInfo property in properties)
                    {
                        // Lấy giá trị của thuộc tính
                        object value = property.GetValue(obj);
                        string valueString = value?.ToString() ?? string.Empty;

                        // Ghi theo format: [tên_thuộc_tính]=
                        contentBuilder.AppendLine($"[{property.Name}]=");

                        // Ghi giá trị (có thể nhiều dòng) và kết thúc bằng dấu chấm phẩy
                        contentBuilder.AppendLine($"{valueString};");
                    }

                    // Thêm dấu phân cách giữa các đối tượng
                    contentBuilder.AppendLine("---OBJECT_SEPARATOR---");
                }

                string finalContent = contentBuilder.ToString();

                // Mã hóa nội dung nếu cần
                if (isEncrypted)
                {
                    finalContent = EncryptionHelper.Encrypt(finalContent, true);
                }

                // Ghi file
                File.WriteAllText(filePath, finalContent);

                Console.WriteLine($"Đã ghi {list.Count} đối tượng vào file thành công: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi ghi file: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Đọc file và chuyển đổi nội dung thành danh sách đối tượng
        /// </summary>
        /// <typeparam name="T">Kiểu của đối tượng cần đọc</typeparam>
        /// <param name="filePath">Đường dẫn đến file</param>
        /// <param name="isEncrypted">File có được mã hóa hay không</param>
        /// <returns>Danh sách đối tượng được khôi phục từ file</returns>
        public static List<T> ReadListObjectFromFile<T>(string filePath, bool isEncrypted = true) where T : new()
        {
            try
            {
                // Đọc nội dung file
                string fileContent = File.ReadAllText(filePath);
                string decryptedContent = fileContent;

                if (isEncrypted)
                {
                    // Giải mã nội dung
                    decryptedContent = EncryptionHelper.Decrypt(fileContent, true);
                }

                List<T> resultList = new List<T>();

                // Tách nội dung theo dấu phân cách đối tượng
                string[] objectSections = decryptedContent.Split(new[] { "---OBJECT_SEPARATOR---" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string objectSection in objectSections)
                {
                    if (string.IsNullOrWhiteSpace(objectSection)) continue;

                    // Tạo đối tượng mới từ section
                    T obj = ParseObjectFromSection<T>(objectSection.Trim());
                    if (obj != null)
                    {
                        resultList.Add(obj);
                    }
                }

                Console.WriteLine($"Đã đọc {resultList.Count} đối tượng từ file thành công: {filePath}");
                return resultList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi đọc file: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Phân tích một section và tạo đối tượng từ nó
        /// </summary>
        /// <typeparam name="T">Kiểu của đối tượng</typeparam>
        /// <param name="section">Nội dung section chứa thông tin đối tượng</param>
        /// <returns>Đối tượng được tạo từ section</returns>
        private static T ParseObjectFromSection<T>(string section) where T : new()
        {
            try
            {
                // Khởi tạo đối tượng mới
                T obj = new T();

                // Đọc từng dòng trong section
                string[] lines = section.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

                string currentPropertyName = null;
                StringBuilder currentValueBuilder = new StringBuilder();

                foreach (string line in lines)
                {
                    // Kiểm tra xem dòng có phải là dòng bắt đầu thuộc tính mới không
                    if (line.StartsWith("[") && line.Contains("]="))
                    {
                        // Nếu đang xử lý thuộc tính trước đó, gán giá trị cho nó
                        if (!string.IsNullOrEmpty(currentPropertyName))
                        {
                            SetPropertyValue(obj, currentPropertyName, currentValueBuilder.ToString());
                        }

                        // Bắt đầu thuộc tính mới
                        int separatorIndex = line.IndexOf("]=");
                        currentPropertyName = line.Substring(1, separatorIndex - 1);
                        currentValueBuilder.Clear();

                        // Nếu có nội dung sau dấu "]=" trên cùng dòng
                        string remainingContent = line.Substring(separatorIndex + 2);
                        if (!string.IsNullOrEmpty(remainingContent))
                        {
                            currentValueBuilder.AppendLine(remainingContent);
                        }
                    }
                    else if (!string.IsNullOrEmpty(currentPropertyName))
                    {
                        // Kiểm tra xem dòng có kết thúc bằng dấu chấm phẩy không
                        if (line.EndsWith(";"))
                        {
                            // Thêm nội dung (bỏ dấu chấm phẩy cuối) và kết thúc thuộc tính
                            string lineContent = line.Substring(0, line.Length - 1);
                            if (!string.IsNullOrEmpty(lineContent))
                            {
                                currentValueBuilder.AppendLine(lineContent);
                            }

                            // Gán giá trị cho thuộc tính
                            SetPropertyValue(obj, currentPropertyName, currentValueBuilder.ToString());

                            // Reset cho thuộc tính tiếp theo
                            currentPropertyName = null;
                            currentValueBuilder.Clear();
                        }
                        else
                        {
                            // Thêm dòng vào giá trị hiện tại
                            currentValueBuilder.AppendLine(line);
                        }
                    }
                }

                // Xử lý thuộc tính cuối cùng nếu còn dang dở
                if (!string.IsNullOrEmpty(currentPropertyName))
                {
                    SetPropertyValue(obj, currentPropertyName, currentValueBuilder.ToString());
                }

                return obj;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi phân tích section: {ex.Message}");
                return default(T);
            }
        }
    }
}
