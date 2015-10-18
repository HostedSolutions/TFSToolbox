using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Hostsol.TFSTB.WebAPI.Models;
using Hostsol.TFSTB.WebAPI.TeamCity;

namespace Hostsol.TFSTB.WebAPI.Controllers
{
    public class DisplayBuildsController : ApiController
    {
        private readonly BuildAccess _teamCityBuild;

        public DisplayBuildsController(TeamCity.BuildAccess teamCityBuild)
        {
            _teamCityBuild = teamCityBuild;
        }

        // GET: api/DisplayBuilds
        public IEnumerable<DisplayBuild> Get()
        {
            throw new NotImplementedException();
        }

        // GET: api/DisplayBuilds/5
        public IQueryable<DisplayBuild> Get(string id)
        {
            if (id.Contains(","))
            {
                var rtn = new List<DisplayBuild>();
                foreach (string def in id.Split(','))
                {
                    var r = _teamCityBuild.GetLastestBuild(def);
                    rtn.Add(r);
                }
                return rtn.AsQueryable();
            }
            else
            {
                return (new List<DisplayBuild> {_teamCityBuild.GetLastestBuild(id)}).AsQueryable();
            }
        }

        // POST: api/DisplayBuilds
        public void Post([FromBody]string value)
        {
            throw new NotImplementedException();
        }

        // PUT: api/DisplayBuilds/5
        public void Put(int id, [FromBody]string value)
        {
            throw new NotImplementedException();
        }

        // DELETE: api/DisplayBuilds/5
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
