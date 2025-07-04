using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnionApp.classes
{
    public class Monitoring
    {
        public Image Image { get; set; }
        public int UserId { get; set; }
        public string Time { get; set; }
        public int DeviceId { get; set; }
        public string AuthMode { get; set; }
        public string AuthType { get; set; }
        public string AuthResult { get; set; }
        public string ThermalBurn { get; set; }

        public byte[] ImageBytes
        {
            get
            {
                if (Image == null) return null;
                using (var ms = new MemoryStream())
                {
                    Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    return ms.ToArray();
                }
            }
        }
    }
}
