using RepollInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace RepollService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "WCFRepollService" in both code and config file together.
    public class WCFRepollService : IWCFRepollService
    {
        public List<string> ListProducts()
        {
            return new List<string>(new string[] { "Hello", "World" });
        }
    }
}
