using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace HighFive.Server.Api.Controllers
{
    [RoutePrefix("api")]
    public class HighFiveController : ApiController
    {
        [Route("highfive/{name}")]
        [HttpPost]
        public string HighFive(string name)
        {
            return $"You slapped! Thanks {name} - {DateTime.Now.ToString("yyyy MMM dd HH:mm")}";
        }
    }
}
