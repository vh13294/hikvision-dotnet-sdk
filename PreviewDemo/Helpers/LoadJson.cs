using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;


namespace PreviewDemo.Helpers
{
    public class LoadJson
    {
        public static Config GetDVRs()
        {
            using (StreamReader r = new StreamReader("DeviceConfig.json"))
            {
                string json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<Config>(json);
            }
        }

        public static Dictionary<string, string> GetErrorMessageDict()
        {
            using (StreamReader r = new StreamReader("ErrorMessage.json"))
            {
                string json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<Dictionary<string, string>> (json);
            }
        }

        public class Device
        {
            public string ip { get; set; }
            public string username { get; set; }
            public string password { get; set; }
            public Int16 port { get; set; }
            public List<Int16> channels { get; set; }
        }

        public class Config
        {
            public List<Device> Devices { get; set; }
            public int displayScreen { get; set; }
            public int numberOfScreen { get; set; }
        }
    }
}
