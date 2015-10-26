using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;

using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNet.Mvc.Routing;

namespace Hostsol.TFSTB.WebFrontend.Services
{
    public class RESTRequest
    {
        private readonly Uri _requestHost;
        private readonly string _requestPath;
        private readonly UrlAction _urlAction;
        private readonly RequestDataFormat _requestDataFormat;

        public enum UrlAction { GET, POST , PUT, DELETE}
        public enum RequestDataFormat { Json,Soap}
        public RESTRequest(Uri requestHost,string requestPath, UrlAction urlAction, 
            RequestDataFormat requestDataFormat)
        {
            _requestHost = requestHost;
            _requestPath = requestPath;
            _urlAction = urlAction;
            _requestDataFormat = requestDataFormat;
        }
        
        public async Task<T> SendRequest<T>()
        {
            T rtn;

            using (var client = new HttpClient())
            {
                client.BaseAddress = _requestHost;
                client.DefaultRequestHeaders.Accept.Clear();
                
                switch (_requestDataFormat)
                {
                    case RequestDataFormat.Soap:
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("soap/xml"));
                        break;
                    case RequestDataFormat.Json:
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        break;
                }
                
                // HTTP GET
                HttpResponseMessage response = await client.GetAsync(_requestPath);
                if (response.IsSuccessStatusCode)
                {
                    rtn = await response.Content.ReadAsAsync<T>();
                    return rtn;
                }
                else
                {
                    throw new Exception(String.Format("Bad response, return code is {0}",response.StatusCode ));
                }
            }

        }
    }
}
