using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;

using FinalGetTest;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;
using Amazon;
using Amazon.Runtime;

namespace FinalGetTest.Tests
{
    public class FunctionTest
    {
        private static AmazonDynamoDBClient dbclient = new AmazonDynamoDBClient();
        private static string tableName = "Final2";
        [Fact]
        public async Task TestGetTitleFromDB()
        {

            string id = "2021-04-30";

            GetItemResponse res = await dbclient.GetItemAsync(tableName, new Dictionary<string, AttributeValue>
            {
                { 
                    "date", new AttributeValue { S = id } 
                }
            });
            
            
            Document myDoc = Document.FromAttributeMap(res.Item);
            NasaPOD pod = JsonConvert.DeserializeObject<NasaPOD>(myDoc.ToJson());


            string title = "Pink and the Perigee Moon";

            Assert.Equal(title, pod.title);
        }
    }
}
