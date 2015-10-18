using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ConfigInjector;

namespace Hostsol.TFSTB.WebAPI.Settings.TFS
{
    public class ProjectNameSetting : ConfigurationSetting<string>
    {
        protected override IEnumerable<string> ValidationErrors(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) yield return "No TFS Project name specifie in AppSettings";
        }
    }
}