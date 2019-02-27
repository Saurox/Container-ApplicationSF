using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BackendService.Controllers
{
    public class StatusController : ApiController
    {
        public IEnumerable<string> Get()
        {
            return new string[] { "Backend", " is OK!" };
        }
    }
}
