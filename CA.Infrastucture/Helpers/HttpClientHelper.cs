using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CA.Infrastucture.Helpers
{
    public class HttpClientHelper
    {
        public static async Task<T> GetRequest<T>(string url) where T: class
        {
            using (var client = new HttpClient())
            {
                var result = await client.GetStringAsync(url);
                return JsonConvert.DeserializeObject<T>(result);
            }
        }
    }
}
