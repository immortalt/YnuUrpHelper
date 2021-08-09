using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 不朽URP助手.Entities;

namespace 不朽URP助手.Helpers
{
    public class FileHelper
    {
        public static void WriteSettings(UrpSettings settings)
        {
            try
            {
                string json = JsonHelper.Serialize<UrpSettings>(settings);
                var urpSettings = EncryptHelper.EncryptDES(json, "hahaseti");
                System.IO.File.WriteAllText("Settings.ini", urpSettings);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        public static UrpSettings ReadSettings()
        {
            try
            {
                var txt = System.IO.File.ReadAllText("Settings.ini");
                var json = EncryptHelper.DecryptDES(txt, "hahaseti");
                Debug.WriteLine(json);
                var urpSettings = JsonHelper.Deserialize<UrpSettings>(json);
                return urpSettings;
            }
            catch
            {
                return new UrpSettings
                {
                    savepwd = true
                };
            }
        }
    }
}
