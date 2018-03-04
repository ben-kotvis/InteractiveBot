using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using InteractiveBot.Model;
using RestSharp;

namespace InteractiveBot.Handlers
{
    [Serializable]
    public class QuestionAndAnswerHandler : IHandler
    {
        public QuestionAndAnswerHandler()
        {
        }

        public async Task<QnAMakerResult> Handle(string input)
        {
            var request = new RestRequest();

            string responseString = string.Empty;
            
            var knowledgebaseId = "f7ec6d84-a854-4777-bf1e-98ef6c0e1a3e"; // Use knowledge base id created.
            var qnamakerSubscriptionKey = "dcd70e96076f4d0ea10cdf19e6d87afa"; //Use subscription key assigned to you.

            //Build the URI
            Uri qnamakerUriBase = new Uri("https://westus.api.cognitive.microsoft.com/qnamaker/v1.0");
            var builder = new UriBuilder($"{qnamakerUriBase}/knowledgebases/{knowledgebaseId}/generateAnswer");

            //Add the question as part of the body
            var postBody = new
            {
                question = input
            };

            var postBotyString = JsonConvert.SerializeObject(postBody);

            //Send the POST request
            using (WebClient client = new WebClient())
            {
                //Set the encoding to UTF8
                client.Encoding = System.Text.Encoding.UTF8;

                //Add the subscription key header
                client.Headers.Add("Ocp-Apim-Subscription-Key", qnamakerSubscriptionKey);
                client.Headers.Add("Content-Type", "application/json");
                responseString = await client.UploadStringTaskAsync(builder.Uri, postBotyString);
            }
            
            return JsonConvert.DeserializeObject<QnAMakerResult>(responseString);
            
        }
    }
}
