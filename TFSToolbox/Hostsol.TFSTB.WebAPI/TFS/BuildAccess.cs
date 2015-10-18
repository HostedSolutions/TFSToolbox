using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hostsol.TFSTB.WebAPI.Settings.TFS;
using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace Hostsol.TFSTB.WebAPI.TFS
{
    public class BuildAccess
    {
        private readonly ProjectNameSetting _projectName;
        private TfsTeamProjectCollection tpc;
        public BuildAccess(ProjectNameSetting projectName, ServerAccess tfsServer)
        {
            _projectName = projectName;
            tpc = tfsServer.tpc;
        }

        public List<KeyValuePair<string, string>> GetListOfRecentBuilds(string BuildName, string ScopeName)
        {
            var rtn = new List<KeyValuePair<string, string>>();
            var vcs = tpc.GetService<VersionControlServer>();
            var proj = vcs.GetTeamProject(_projectName);
            IBuildServer buildServer = (IBuildServer)tpc.GetService(typeof(IBuildServer));

            var builds = buildServer.QueryBuilds(proj.Name, BuildName);
            List<IBuildDetail> bds = new List<IBuildDetail>();
            foreach (IBuildDetail b in builds)
            {
                VersionControlLabel[] ls;
                ls = vcs.QueryLabels(b.BuildNumber, ScopeName, null, false);
                if (ls.Length != 0)
                {
                    var l = ls[0];
                    rtn.Add(new KeyValuePair<string, string>(b.BuildNumber + " (" + b.Quality + ") " + l.Comment, b.BuildNumber));
                }
            }
            return rtn;
        }

        public string GetLastBuildMarkedReleased(string BuildName)
        {
            var vcs = tpc.GetService<VersionControlServer>();
            var proj = vcs.GetTeamProject(_projectName);
            IBuildServer buildServer = (IBuildServer)tpc.GetService(typeof(IBuildServer));
            var builds = buildServer.QueryBuilds(proj.Name, BuildName);
            List<IBuildDetail> bds = new List<IBuildDetail>();
            foreach (IBuildDetail b in builds)
            {
                if (b.Quality == "Released")
                {
                    bds.Add(b);
                }
            }
            if (bds.Count == 0)
            {
                throw new Exception("No Previous Builds Marked as released");
            }
            List<IBuildDetail> bds2 = bds.OrderByDescending(o => o.FinishTime).ToList();
            string rtn = "";
            if (bds2[0].LabelName != null)
            {
                rtn = bds2[0].LabelName.Split('@')[0];
            }
            else
            {// change for new label name created by team city
                rtn = bds2[0].BuildNumber;
            }
            return rtn;
        }
        
        public void ChangeBuildQuality(string buildDefinitionName, string quality, string BuildNumber)
        {
            IBuildServer _buildServer;
            _buildServer = (IBuildServer)tpc.GetService(typeof(IBuildServer));
            IBuildDetailSpec buildSpec = _buildServer.CreateBuildDetailSpec(_projectName, buildDefinitionName);
            buildSpec.BuildNumber = BuildNumber;
            IBuildDetail build = _buildServer.QueryBuilds(buildSpec).Builds.First();
            build.Quality = quality;
            if (quality == "Released")
            {
                build.KeepForever = true;
            }
            build.Save();
        }
    }
}