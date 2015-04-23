using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using EmptyWebApiProject.Models;

namespace EmptyWebApiProject.Controllers
{
    public class Dial800_CallController : ApiController
    {
        static readonly IDial800Repository databasePlaceholder = new Dial800Repository();

        public IEnumerable<Dial800_Call> GetAllDial800_Calls() { return databasePlaceholder.GetAll(); }
        public Dial800_Call GetDial800_CallByID(int id)
        {
            Dial800_Call dial800_call = databasePlaceholder.Get(id);
            if (dial800_call == null) { throw new HttpResponseException(HttpStatusCode.NotFound); }
            return dial800_call;
        }
        public HttpResponseMessage PostDial800_Call(Dial800_Call dial800_call)
        {
            dial800_call = databasePlaceholder.Add(dial800_call);
            string apiName = App_Start.WebApiConfig.DEFAULT_ROUTE_NAME;
            var response = this.Request.CreateResponse<Dial800_Call>(HttpStatusCode.Created, dial800_call);
            string uri = Url.Link(apiName, new { id = dial800_call.id });
            response.Headers.Location = new Uri(uri);
            return response;
        }
        public bool PutDial800_Call(Dial800_Call dial800_call)
        {
            if (!databasePlaceholder.Update(dial800_call)) { throw new HttpResponseException(HttpStatusCode.NotFound); }
            return true;
        }
        public void DeleteDial800_Call(int id)
        {

            Dial800_Call dial800_call = databasePlaceholder.Get(id);
            if (dial800_call == null) { throw new HttpResponseException(HttpStatusCode.NotFound); }
            databasePlaceholder.Remove(id);
        }
    }
}
