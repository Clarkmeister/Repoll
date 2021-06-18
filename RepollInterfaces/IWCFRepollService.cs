using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace RepollInterfaces
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IWCFRepollService" in both code and config file together.
    [ServiceContract]
    public interface IWCFRepollService
    {
        [OperationContract]
        List<Tuple<string,string>> GetTrackedRepos();
        [OperationContract]
        Tuple<bool, string> SubmitTrackedRepo(string nickname, string directory);
        [OperationContract]
        Tuple<bool, string> RemoveTrackedRepo(Tuple<string, string> tuple);
        [OperationContract]
        string ManualUpdate(string cmd);
    }
}
