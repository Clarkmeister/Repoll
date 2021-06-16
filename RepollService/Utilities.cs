using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Diagnostics;

namespace RepollService
{
    public static class Extensions
    {
        public static T ToObject<T>(this string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public static T ToObject<T>(this Stream stream)
        {
            var reader = new StreamReader(stream);
            var body = reader.ReadToEnd();
            return ToObject<T>(body);
        }

        public static string ToJsonString(this object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                });
        }

        public static Stream ToStream(this object obj)
        {
            return obj.ToJsonString().ToStream();
        }

        public static Stream ToStream(this string str)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(str));
        }
    }

    public static class RepollEventLogger
    {
        public static void Initialize(ref EventLog logger)
        {
            if (!EventLog.SourceExists("MySource"))
            {
                EventLog.CreateEventSource(
                    "MySource", "MyNewLog");
            }
            logger.Source = "RepollSource";
            logger.Log = "RepollLog";
        }
    }
}
