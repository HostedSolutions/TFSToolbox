using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ConfigInjector;

namespace Hostsol.TFSTB.WebAPI.Settings.TeamCity
{
    public class TeamCityUserName : ConfigurationSetting<string>
    {
        protected override IEnumerable<string> ValidationErrors(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) yield return "No TeamCity UserName specifie in AppSettings";
        }
    }
}