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

    public static class RunCommand
    {
        public static string RunCmdAndGetOutput(string cmd)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardInput = true;
            startInfo.UseShellExecute = false;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C " + cmd;
            process.StartInfo = startInfo;
            process.Start();

            var res = process.StandardOutput.ReadToEnd();

            return res;
        }

        public static List<string> RunChainCmdsAndGetOutput(List<string> commands)
        {
            string cmd = "/C ";
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();

            //Chain commands together
            for (int i = 0; i < commands.Count - 1; i++)
            {
                cmd += commands[i] + " && ";
            }
            cmd += commands[commands.Count - 1];
            commands.Clear();

            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardInput = true;
            startInfo.UseShellExecute = false;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = cmd;
            process.StartInfo = startInfo;
            process.Start();

            while (!process.StandardOutput.EndOfStream)
            {
                commands.Add(process.StandardOutput.ReadLine());
            }

            return commands;
        }
    }
}
