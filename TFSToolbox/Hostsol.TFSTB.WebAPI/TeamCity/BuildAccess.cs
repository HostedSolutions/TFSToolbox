using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Hostsol.TFSTB.WebAPI.Models;
using Hostsol.TFSTB.WebAPI.Settings.TeamCity;
using Newtonsoft.Json;

namespace Hostsol.TFSTB.WebAPI.TeamCity
{
    public class BuildAccess
    {
        private string _userName;
        private string _password;
        private string _url;
        public BuildAccess(TeamCityUserName userName,TeamCityPassword password,TeamCityURL url)
        {
            _userName = userName;
            _password = password;
            _url = url;
        }

        public DisplayBuild GetLastestBuild(string buildType)
        {
            DisplayBuild rtn = new DisplayBuild();
            string cmd =
                string.Format("/httpAuth/app/rest/builds?locator=buildType:{0},running:any,lookupLimit:1",
                    buildType);
            string res = DoRequest(cmd, "");

            // response will look like this
            //@{id=1624; buildTypeId=SalesForce_BuildTestAndPackage; number=1.1.8.0; status=SUCCESS; state=finished; 
            //href=/httpAuth/app/rest/builds/id:1624; 
            //webUrl=/viewLog.html?buildId=1624&buildTypeId=SalesForce_BuildTestAndPackage}
            
            dynamic LastBuild = JsonConvert.DeserializeObject<dynamic>(res);
            int buildId = LastBuild.build[0].id;
            cmd = string.Format("/httpAuth/app/rest/buildQueue?locator=buildType:{0}", buildType);

            string BuildQueueString = DoRequest(cmd, "");

            dynamic BuildQueue = JsonConvert.DeserializeObject<dynamic>(BuildQueueString);
            if (BuildQueue.count > 0)
            {
                rtn.BuildStatusText = "NotStarted";
            }
            else
            {
                rtn.BuildStatusText = "";
            }
            cmd = string.Format("/httpAuth/app/rest/builds/id:{0}", buildId);

            string resBuild = DoRequest(cmd, "");
            dynamic BuildDetails = JsonConvert.DeserializeObject<dynamic>(resBuild);
            //BuildDetails  looks like this
            //@{id=1624; buildTypeId=SalesForce_BuildTestAndPackage; number=1.1.8.0; status=SUCCESS; state=finished; href=/httpAuth/app/rest/builds/id:1624; webUrl=http://teamcity
            //.net/viewLog.html?buildId=1624&buildTypeId=SalesForce_BuildTestAndPackage; statusText=Success; buildType=; queuedDate=20150622T154426+1000; startDate=20150622T154430+1000;
            //finishDate=20150622T154503+1000; triggered=; lastChanges=; changes=; revisions=; agent=; artifacts=; relatedIssues=; properties=; statistics=; artifact-dependencies=}
            string FriendlyBuildName = BuildDetails.BuildType.name;
            string Username = BuildDetails.lastChanges.change[0].username;
            rtn.BuildName = buildType;
            /*
                All	All status applies.
                Failed	Build failed.
                InProgress	Build is in progress.
                None	No status available.
                NotStarted	Build is not started.
                PartiallySucceeded	Build is partially succeeded.
                Stopped	Build is stopped.
                Succeeded
             */
            if (rtn.BuildStatusText == "")
            {
                switch ((string)BuildDetails.status)
                {
                    case "SUCCESS":
                        if (BuildDetails.state == "running")
                        {
                            rtn.BuildStatusText = "InProgress";
                        }
                        else
                        {
                            rtn.BuildStatusText = "Succeeded";
                        }
                        break;
                    case "FAILURE":
                        rtn.BuildStatusText = "Failed";
                        break;
                    case "UNKNOWN":
                        if (BuildDetails.state == "finished")
                        {
                            rtn.BuildStatusText = "Stopped";
                        }
                        break;
                    default:
                        rtn.BuildStatusText = "None";
                        break;
                }
            }
            rtn.RequestByUser = Username;

            var d = DateTime.ParseExact(BuildDetails.startDate, "yyyyMMdd'T'HHmmsszzz", CultureInfo.InvariantCulture);
            rtn.DateBuildStarted = d;
            rtn.TestFailed = 0;
            rtn.TestRun = 0;
            rtn.TestsPast = 0;

            return rtn;

        }
        public string GetVersionNumber()
        {
            return DoRequest("/httpAuth/app/rest/version", "");
        }

        private string DoRequest(string methodLocation, string data)
        {
            string rtn = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_url + methodLocation);
            string authInfo = _userName + ":" + _password;
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            request.Headers["Authorization"] = "Basic " + authInfo;
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";

            if (request.Method == "POST")
            {
                request.ContentLength = data.Length;
                using (Stream webStream = request.GetRequestStream())
                using (StreamWriter requestWriter = new StreamWriter(webStream, System.Text.Encoding.ASCII))
                {
                    requestWriter.Write(data);
                }
            }

            try
            {
                WebResponse webResponse = request.GetResponse();
                using (Stream webStream = webResponse.GetResponseStream())
                {
                    if (webStream != null)
                    {
                        using (StreamReader responseReader = new StreamReader(webStream))
                        {
                            rtn = responseReader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                rtn = e.Message;
            }
            return rtn;
        }
    }
}