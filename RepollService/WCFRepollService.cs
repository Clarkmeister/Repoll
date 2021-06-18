using RepollInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using RepollService;
using System.IO;


namespace RepollService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "WCFRepollService" in both code and config file together.
    public class WCFRepollService : IWCFRepollService
    {
        private readonly string filePath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Repoll\repos.json";
        private readonly string directoryPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Repoll";
        public List<Tuple<string, string>> GetTrackedRepos()
        {
            return RepollService.repos;
        }

        public string ManualUpdate(string cmd)
        {
            try
            {
                return RunCommand.RunCmdAndGetOutput(cmd);
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public Tuple<bool, string> RemoveTrackedRepo(Tuple<string, string> tuple)
        {
            try
            {
                RepollService.repos.Remove(tuple);
                SaveFile();
                return new Tuple<bool, string>(true, "Success!");
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, ex.Message);
            }
        }

        public Tuple<bool, string> SubmitTrackedRepo(string nickname, string directory)
        {
            try
            {
                var tuple = new Tuple<string, string>(nickname, directory);
                RepollService.repos.Add(tuple);
                return new Tuple<bool, string>(SaveFile(), "Sucess!");
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, ex.Message);
            }
        }

        private bool SaveFile()
        {
            if (File.Exists(filePath))
            {
                try
                {
                    File.WriteAllText(filePath, RepollService.repos.ToJsonString());
                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public string TestCmd()
        {
            return RunCommand.RunCmdAndGetOutput("echo Repoll: && cd C:\\Users\\clark\\Documents\\Projects\\Repoll && git pull");
        }
    }
}
