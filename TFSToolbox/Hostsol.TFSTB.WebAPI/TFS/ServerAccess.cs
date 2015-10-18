using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hostsol.TFSTB.WebAPI.Settings.TFS;
using Microsoft.TeamFoundation.Client;

namespace Hostsol.TFSTB.WebAPI.TFS
{
    public class ServerAccess
    {
        private readonly CollectionIdSetting _collectionId;
        private readonly ServerUrlSetting _serverUrl;
        public TfsTeamProjectCollection tpc;
        public ServerAccess(CollectionIdSetting collectionId, ServerUrlSetting serverUrl)
        {
            _collectionId = collectionId;
            _serverUrl = serverUrl;
            var TFSServer = TfsConfigurationServerFactory.GetConfigurationServer(_serverUrl);
            tpc = TFSServer.GetTeamProjectCollection(_collectionId);
        }

    }
}