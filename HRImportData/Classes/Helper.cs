using System.Text.RegularExpressions;

namespace HRImportData.Classes
{
    public static class Helper
    {
        public static Dictionary<string, string> clientList = new Dictionary<string, string>();
        

        public static string loginInfoPath = $"{AppDomain.CurrentDomain.BaseDirectory}\\login.dat";
        public static List<LoginInfo> loginInfos = new List<LoginInfo>();
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

        public static void SaveLoginInfo(LoginInfo loginInfo)
        {
            bool isChanged = false;
            int idx = loginInfos.FindIndex(q => q.Site == loginInfo.Site
                && q.DbUserName == loginInfo.DbUserName
                && q.SiteVersion == loginInfo.SiteVersion
            );

            if (idx >= 0) //update info
            {
                if (!loginInfos[idx].Equals(loginInfo))
                {
                    loginInfos[idx] = loginInfo;
                    isChanged = true;
                }
            }
            else
            {
                loginInfos.Add(loginInfo);
                isChanged = true;
            }

            if (isChanged)
            {
                string[] updateLogin = loginInfos.Select(q => q.ToString()).ToArray();
                File.WriteAllLines(loginInfoPath, updateLogin);
            }
        }

        public static void LoadLoginInfo()
        {
            if(!File.Exists(loginInfoPath)) return;
            var lines = File.ReadAllLines(loginInfoPath);
            foreach (var line in lines)
            {
                loginInfos.Add(LoginInfo.Parse(line));
            }
        }


    }
}
