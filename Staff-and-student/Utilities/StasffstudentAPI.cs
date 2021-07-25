using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Staff_and_student.Utilities
{
    public class StasffstudentAPI:HttpClient
    {
        public StasffstudentAPI()
        {
            if (!string.IsNullOrWhiteSpace(Utility.GetAppSettings("StudentAPIUrl")))
            {
                string apiURL = Utility.GetAppSettings("StudentAPIUrl");
                Uri apiURI = new Uri(apiURL);
                this.BaseAddress = apiURI;
                this.Timeout = new TimeSpan(0, 20, 0);
            }
            this.DefaultRequestHeaders.Accept.Clear();
            this.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
