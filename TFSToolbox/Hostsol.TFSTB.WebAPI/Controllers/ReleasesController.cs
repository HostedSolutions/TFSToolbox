using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Hostsol.TFSTB.WebAPI.Models;
using Hostsol.TFSTB.WebAPI.TFS;

namespace Hostsol.TFSTB.WebAPI.Controllers
{
    public class ReleasesController : ApiController
    {
        private readonly BuildAccess _buildAccess;
        private readonly VersionControlAccess _versionControlAccess;

        public ReleasesController(BuildAccess buildAccess, VersionControlAccess versionControlAccess)
        {
            _buildAccess = buildAccess;
            _versionControlAccess = versionControlAccess;
        }

        // POST: api/Releases
        public void Post([FromBody] BuildQuality value)
        {
            _buildAccess.ChangeBuildQuality(value.buildDefinitionName, value.quality, value.BuildNumber);
        }

        [Route("LastReleased")]
        public string Get(string buildName)
        {
            return _buildAccess.GetLastBuildMarkedReleased(buildName);
        }

        [Route("RecentBuildLabels")]
        public IQueryable<KeyValuePair<string, string>> RecentBuilds(BuildScope buildScope)
        {
            return _buildAccess.GetListOfRecentBuilds(buildScope.BuildName, buildScope.ScopeName).AsQueryable();
        }

        [Route("ChangeSetBetweenLabels")]
        public IEnumerable<int> ChangeSetBetweenLabels(VersionControlQuery value)
        {
            return _versionControlAccess.GetChangeSetListBetweenTwoLabels(value.LabelScope, value.BaseLabel,
                value.TargetLabel);
        }

        [Route("ReleaseNotes")]
        public string ReleaseNotes(VersionControlQuery value)
        {
            return _versionControlAccess.GetReleaseNotesBetweenTwoLabels(value.LabelScope, value.BaseLabel,
                value.TargetLabel);
        }
        
    }
}