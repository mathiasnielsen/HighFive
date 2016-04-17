using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using HighFive.Server.Api.Filters.Data.Cache;
using System.Web;
using HighFive.Server.Api.Filters;

namespace HighFive.Server.Api.Controllers
{
    [RoutePrefix("api")]
    public class HighFiveController : ApiController
    {
        private const int BufferSequenceCount = 4;
        private const int BufferSequenceTime = 1000;
        private const string Name = nameof(Name);
        private const string TimeStamp = nameof(TimeStamp);

        [Route("highfive/{name}")]
        [HttpGet]
        public async Task<string> HighFive(string name)
        {
            // Try to get pending slap.
            var lastName = (string)HttpContext.Current.Application[Name];
            var lastTimeStamp = HttpContext.Current.Application[TimeStamp];
            lastTimeStamp = lastTimeStamp ?? DateTime.MinValue;

            var hasLastName = string.IsNullOrWhiteSpace(lastName) == false;
            var timeStamp = DateTime.Now;

            string result;
            if (hasLastName)
            {
                result = $"SMACK! You highfived {lastName}!\nThanks {name}\n{DateTime.Now.ToString("yyyy MMM dd HH:mm")}";

                HttpContext.Current.Application[Name] = name;
                HttpContext.Current.Application[TimeStamp] = timeStamp;
            }
            else
            {
                // Default.
                result = $"No highfives available.\n{DateTime.Now.ToString("yyyy MMM dd HH:mm")}";

                HttpContext.Current.Application[Name] = name;
                HttpContext.Current.Application[TimeStamp] = timeStamp;

                for (int i = 0; i < BufferSequenceCount; i++)
                {
                    await Task.Delay(BufferSequenceTime);

                    // Check for new highfive
                    var potentialNewHighFive = (string)HttpContext.Current.Application[Name];
                    var potentialNewHighFiveTimeStamp = (DateTime)HttpContext.Current.Application[TimeStamp];

                    if (potentialNewHighFive != name && potentialNewHighFiveTimeStamp != timeStamp)
                    {
                        // new highfive
                        result = $"SMACK! You highfived {potentialNewHighFive}!\nThanks {name}\n{DateTime.Now.ToString("yyyy MMM dd HH:mm")}";
                        break;
                    }
                }

                // Clean up.
                HttpContext.Current.Application[Name] = string.Empty;
                HttpContext.Current.Application[TimeStamp] = DateTime.MinValue;
            }

            return result;
        }
    }
}
