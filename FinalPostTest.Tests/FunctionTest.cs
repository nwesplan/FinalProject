using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;

using FinalPostTest;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using System.Dynamic;
using System.Net.Http;

namespace FinalPostTest.Tests
{
    public class FunctionTest
    {
        [Fact]
        public async Task TestGetDate()
        {
            HttpClient client = new HttpClient();

            string url = $"https://api.nasa.gov/planetary/apod?api_key=DcyC0NbDLkfedANtzI58asjhgNQWWHYozbs0yHeP";

            dynamic httpString = JsonConvert.DeserializeObject<ExpandoObject>(await client.GetStringAsync(url));
            
            string test = httpString.date;
            string date = DateTime.Today.Date.ToString("yyyy-MM-dd");
  
            Assert.Equal(date, test);
        }
    }
}
