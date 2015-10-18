using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hostsol.TFSTB.WebAPI.Models
{
    public class BuildQuality
    {
        public string buildDefinitionName { get; set; }
        public string quality { get; set; }
        public string BuildNumber { get; set; }
    }
}