using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using HighFive.Server.Api.Filters.Data.Cache;

namespace HighFive.Server.Api.Controllers
{
    [RoutePrefix("api")]
    public class HighFiveController : ApiController
    {
        private string lastName;

        [Route("highfive/{name}")]
        [HttpGet]
        public string HighFive(string name)
        {
            string result;
            if (string.IsNullOrWhiteSpace(lastName))
            {
                result = $"No highfives available.\n{DateTime.Now.ToString("yyyy MMM dd HH:mm")}";
            }
            else
            {
                result = $"You slapped {lastName}!\nThanks {name}\n{DateTime.Now.ToString("yyyy MMM dd HH:mm")}";
            }

            lastName = name;
            return result;
        }
    }
}
