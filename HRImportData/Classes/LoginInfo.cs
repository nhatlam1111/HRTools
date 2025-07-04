using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System;
using Helpers;

namespace HRImportData.Classes
{
    public class LoginInfo
    {
        public string Site { get; set; }
        public string DbUserName { get; set; }
        public string DbUserPass { get; set; }

        public string SiteUserName { get; set; }
        public string SiteUserPass { get; set; }

        public SITE_VERSION SiteVersion { get; set; }

        public LoginInfo()
        {
            Site = "";
            DbUserName = "";
            DbUserPass = "";
            SiteUserName = "";
            SiteUserPass = "";
            SiteVersion = SITE_VERSION.NODEJS;
        }

        public override bool Equals(object obj)
        {
            if (obj is LoginInfo other)
            {
                return Site == other.Site
                    && DbUserName == other.DbUserName
                    && DbUserPass == other.DbUserPass
                    //&& SiteUserName == other.SiteUserName
                    //&& SiteUserPass == other.SiteUserPass
                    && SiteVersion == other.SiteVersion;
            }
            return false;
        }

        public override string ToString()
        {
            /*string format = string.Format("Site:{0}|DbUserName:{1}|DbUserPass:{2}|SiteUserName:{3}|SiteUserPass:{4}|SiteVersion:{5}",
                Site
                , DbUserName
                , DbUserPass
                , SiteUserName
                , SiteUserPass
                , SiteVersion
            );*/

            string format = string.Format("Site:{0}|DbUserName:{1}|DbUserPass:{2}|SiteVersion:{3}",
                Site
                , DbUserName
                , DbUserPass
                , SiteVersion
            );


            return EncryptionHelper.Encrypt(format, true);
        }

        public static LoginInfo Parse(string str)
        {
            string format = EncryptionHelper.Decrypt(str, true);
            string[] arr = format.Split('|');
            LoginInfo loginInfo = new LoginInfo()
            {
                Site = arr[0].Split(':')[1],
                DbUserName = arr[1].Split(':')[1],
                DbUserPass = arr[2].Split(':')[1],
                //SiteUserName = arr[3].Split(':')[1],
                //SiteUserPass = arr[4].Split(':')[1],
                SiteVersion = (SITE_VERSION)Enum.Parse(typeof(SITE_VERSION), arr[3].Split(':')[1])
            };
            return loginInfo;
        }

    }
}
