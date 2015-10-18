using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hostsol.TFSTB.WebAPI.Models
{
    public class DisplayBuild
    {
        public string BuildName { get; set; }
        public string RequestByUser { get; set; }
        public DateTime DateBuildStarted { get; set; }
        public int TestsPast { get; set; }
        public int TestRun { get; set; }
        public int TestFailed { get; set; }
        public string BuildStatusText { get; set; }// Success, Fail, etc
        public int BuildStatusId { get; set; }
    }
}