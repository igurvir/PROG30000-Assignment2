using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;

namespace API.Models.Helpers
{
    public static class ImageHelper
    {
        public static IEnumerable<string> GetTags(string imageUrl)
        {
           
            string apiKey = "acc_42a01e35340cd22";
            string apiSecret = "0b743b54d27333f3df78af4176dc1e74";

            
            string basicAuthValue = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{apiKey}:{apiSecret}"));

            // Set up the REST client for the Imagga API
            var client = new RestClient("https://api.imagga.com/v2/tags");

            // Configure the request
            var request = new RestRequest();
            request.Method = Method.Get;
            request.AddParameter("image_url", imageUrl);
            request.AddHeader("Authorization", $"Basic {basicAuthValue}");

            // Execute the request and get the response
            var response = client.Execute(request);
            if (!response.IsSuccessful || response.Content == null)
            {
                throw new Exception("Error retrieving tags from Imagga: " + response.ErrorMessage);
            }

            // Parse the response to extract tags
            var lines = response.Content.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            var results = lines.Where(l => l.Contains("\"tag\""));
            foreach (var line in results)
            {
                yield return line.Split(':')[2]
                                 .Replace("}", string.Empty)
                                 .Replace("\"", string.Empty)
                                 .Replace("]", string.Empty)
                                 .Trim();
            }
        }
    }
}
