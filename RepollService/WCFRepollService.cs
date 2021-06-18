using RepollInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using RepollService;

namespace RepollService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "WCFRepollService" in both code and config file together.
    public class WCFRepollService : IWCFRepollService
    {
        public List<Tuple<string,string>> GetTrackedRepos()
        {
            return RepollService.repos;
        }

        public List<string> GitPull()
        {
            return null;
        }

        public bool SubmitTrackedRepo(string nickname, string directory)
        {
            try
            {
                var tuple = new Tuple<string, string>(nickname, directory);
                RepollService.repos.Add(tuple);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}
