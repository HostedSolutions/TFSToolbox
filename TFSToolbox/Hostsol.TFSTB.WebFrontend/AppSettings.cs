using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hostsol.TFSTB.WebFrontend
{
    public class AppSettings
    {
        
        public string AdminEmail { get; set; }
        public string AuthPath { get; set; }
        public Uri AuthHost { get; set; }
        public string AllowedUserIDs { get; set; }
        
    }
}
