using System.Text.RegularExpressions;

namespace HRImportData.Classes
{
    public static class AppHelper
    {
        public static string loginInfoPath = $"{AppDomain.CurrentDomain.BaseDirectory}\\login.dat";
        public static List<LoginInfo> loginInfos = new List<LoginInfo>();
        
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
