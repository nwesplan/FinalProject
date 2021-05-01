using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using System.Dynamic;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace FinalPostTest
{
    public class Function
    {
        [Serializable]
        class NasaPOD
        {
            public string date;
            public string explanation;
            public string hdurl;
            public string media_type;
            public string service_version;
            public string title;
            public string url;
        }

        private static AmazonDynamoDBClient dbclient = new AmazonDynamoDBClient();
        private static string tableName = "Final2";

        public async Task<ExpandoObject> FunctionHandler(APIGatewayProxyRequest input, ILambdaContext db)
        {
            HttpClient client = new HttpClient();

            string url = $"https://api.nasa.gov/planetary/apod?api_key=DcyC0NbDLkfedANtzI58asjhgNQWWHYozbs0yHeP";

            Table pod = Table.LoadTable(dbclient, tableName);
            NasaPOD podItem = JsonConvert.DeserializeObject<NasaPOD>(await client.GetStringAsync(url));


            PutItemOperationConfig config = new PutItemOperationConfig();
            config.ReturnValues = ReturnValues.AllOldAttributes;

            await pod.PutItemAsync(Document.FromJson(JsonConvert.SerializeObject(podItem)), config);

            return JsonConvert.DeserializeObject<ExpandoObject>(await client.GetStringAsync(url));

        }
    }
}
