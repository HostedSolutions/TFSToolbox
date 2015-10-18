using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Hostsol.TFSTB.WebAPI.Settings.TFS;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace Hostsol.TFSTB.WebAPI.TFS
{
    public class VersionControlAccess
    {
        private readonly ProjectNameSetting _projectName;
        public TfsTeamProjectCollection tpc;
        public VersionControlAccess(ProjectNameSetting projectName, ServerAccess serverAccess)
        {
            _projectName = projectName;
            tpc = serverAccess.tpc;
        }

        public int GetStartChangesetForBranch(string path)
        {
            // Get TFS version control service.  
            var vcServer = tpc.GetService<VersionControlServer>();

            // Get the first changeset within a branch.  
            int firstChangeset = vcServer.QueryHistory(path,
              LatestVersionSpec.Latest, 0, RecursionType.None, null,
              null, null, 1, false, false, false, true).Cast<Changeset>().First().ChangesetId;

            // Calculate and return a parent changeset for the branch.  
            return vcServer.QueryMerges(null, null,
              new ItemSpec(path, RecursionType.Full),
              LatestVersionSpec.Latest,
              null,
              new ChangesetVersionSpec(firstChangeset)).Max(x => x.SourceVersion);
        }
        public List<int> GetChangeSetListBetweenTwoLabels(string LabelScope, string BaseLabel, string TargetLabel)
        {
            //tf.exe history /collection:http://server/tfs/defaultcollection "$/project/solution" 
            //        /version:LBuild.Solution_20130308.1~LBuild.Soolution_20130328.2 /recursive 

            var vcs = tpc.GetService<VersionControlServer>();
            
            var sourceSpec = new LabelVersionSpec(BaseLabel, LabelScope);
            var targetSpec = new LabelVersionSpec(TargetLabel, LabelScope);

            var deltaChangesets = new List<Changeset>();
            var rtn = new List<int>();
            foreach (Changeset changeSet in
                vcs.QueryHistory(
                            path: LabelScope,
                            version: targetSpec,
                            deletionId: 0,
                            recursion: RecursionType.Full,
                            user: string.Empty,
                            versionFrom: sourceSpec,
                            versionTo: targetSpec,
                            maxCount: int.MaxValue,
                            includeChanges: true,
                            slotMode: false,
                            includeDownloadInfo: false,
                            sortAscending: true
                        )
                )
                rtn.Add(changeSet.ChangesetId);

            return rtn;
        }

        public string GetReleaseNotesBetweenTwoLabels(string LabelScope, string BaseLabel, string TargetLabel)
        {

            var vcs = tpc.GetService<VersionControlServer>();
            var proj = vcs.GetTeamProject(_projectName);

            var sourceSpec = new LabelVersionSpec(BaseLabel, LabelScope);
            var targetSpec = new LabelVersionSpec(TargetLabel, LabelScope);

            var deltaChangesets = new List<Changeset>();

            foreach (Changeset changeSet in
                vcs.QueryHistory(
                            path: LabelScope,
                            version: targetSpec,
                            deletionId: 0,
                            recursion: RecursionType.Full,
                            user: string.Empty,
                            versionFrom: sourceSpec,
                            versionTo: targetSpec,
                            maxCount: int.MaxValue,
                            includeChanges: true,
                            slotMode: false,
                            includeDownloadInfo: false,
                            sortAscending: true
                        )
                )
                deltaChangesets.Add(changeSet);

            var html = "";// ReleaseNotesRender.GetHtml(deltaChangesets);
            //Create output 
            byte[] b;
            using (var memoryStream = new System.IO.MemoryStream())
            {
                using (var r = new System.IO.StreamWriter(memoryStream, Encoding.UTF8))
                {
                    // Various for loops etc as necessary that will ultimately do this:
                    //r.Write(html.style);
                    //r.Write(html.workitems);
                    //r.Write(html.changesets);
                }
                b = memoryStream.ToArray();
            }
            return html;
        }


    }
}