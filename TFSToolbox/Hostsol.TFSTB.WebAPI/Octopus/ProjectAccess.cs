using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hostsol.TFSTB.WebAPI.Settings.Octopus;
using Octopus.Client;

namespace Hostsol.TFSTB.WebAPI.Octopus
{
    public class ProjectAccess
    {
        private readonly OctopusUri _octopusUri;
        private readonly ApiKey _apiKey;
        private readonly OctopusServerEndpoint _octopusServerEndpoint;
        private readonly OctopusRepository _octopusRepo;

        public ProjectAccess(OctopusUri octopusUri,ApiKey apiKey)
        {
            _octopusUri = octopusUri;
            _apiKey = apiKey;
            _octopusServerEndpoint = new OctopusServerEndpoint(_octopusUri.ToString(),_apiKey);
            _octopusRepo = new OctopusRepository(_octopusServerEndpoint);
        }

        public string TestOctopus()
        {
            return _octopusRepo.ServerStatus.GetServerStatus().Id;
        }
    }
}