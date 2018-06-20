using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CrossPlataform.Utilities
{
    public class Functions
    {
        public string ReplaceCharacteres(string textToReplace)
        {
            var stringsToReplace = new string[]
            {
                "<li>",
                "</li>",
                "<ul>",
                "</ul>",
                "<p>",
                "</p>",
                "<br>"
            };

            foreach (var replacement in stringsToReplace)
            {
                textToReplace = textToReplace.Replace(replacement, string.Empty);
            }

            return textToReplace;
        }

        public async Task<bool> IsConnect()
        {
            HttpClient client = new HttpClient();
            bool statusCode = false;

            try
            {
                var httpResponse = await client.GetAsync("http://ecommercee.azurewebsites.net/api/products");

                if (httpResponse.IsSuccessStatusCode)
                {
                    statusCode = true;
                }
            }
            catch(Exception)
            {
                //
            }

            return statusCode;
        }

        public async Task<string> LoadApiData()
        {
            HttpClient client = new HttpClient();
            string response = string.Empty;

            try
            {
                response = await client.GetStringAsync("http://ecommercee.azurewebsites.net/api/products");
            }
            catch (Exception)
            {
                response = null;
            }

            return response;
        }
    }
}