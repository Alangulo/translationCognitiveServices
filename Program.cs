using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace translate_sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
         
            //Cognitive services API
            string host = "https://api.cognitive.microsofttranslator.com";

            //This changes depending on the language we want to translate to, specifically this portion: to=de&to=it&to=ja&to=th
            string route = "/translate?api-version=3.0&to=de&to=it&to=ja&to=th";

            //Key
            string subscriptionKey = "";

            // Console.Write("Hello");
            //string textToTranslate = Console.ReadLine();
            String testText = "Hello world";

            //await TranslateTextRequest(subscriptionKey, host, route, testText);
           // GetAllLanguages();

           // Console.ReadLine();
        }

        // Async call to the Translator Text API
        static public async Task TranslateTextRequest(string subscriptionKey, string host, string route, string inputText)
        {
         
         
            System.Object[] body = new System.Object[] { new { Text = inputText } };
            var requestBody = JsonConvert.SerializeObject(body);

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
               
                request.Method = HttpMethod.Post;

                // Construct the URI and add headers.
                request.RequestUri = new Uri(host + route);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

                // Send the request and get response.
                HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);

                // Read response as a string.
                string result = await response.Content.ReadAsStringAsync();

                //Parsing the response
                TranslationResult[] deserializedOutput = JsonConvert.DeserializeObject<TranslationResult[]>(result);

                // Iterate over the deserialized result (testing purposes)
                foreach (TranslationResult o in deserializedOutput)
                {
                    // Print the detected input language and confidence score.
                    Console.WriteLine("Detected input language: {0}\nConfidence score: {1}\n", o.DetectedLanguage.Language, o.DetectedLanguage.Score);
                    //Showing results
                    foreach (Translation t in o.Translations)
                    {
                        Console.WriteLine("Translated to {0}: {1}", t.To, t.Text);
                    }
                }
            }
        }


        static void GetAllLanguages()
        {
            string host = "https://api.cognitive.microsofttranslator.com";
            string route = "/languages?api-version=3.0";

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                // Set the method to GET
                request.Method = HttpMethod.Get;
                // Construct the full URI
                request.RequestUri = new Uri(host + route);
                // Send request, get response
                var response = client.SendAsync(request).Result;
                var jsonResponse = response.Content.ReadAsStringAsync().Result;
                
                //Console.WriteLine(PrintJSON(jsonResponse));
                //Console.WriteLine("Press any key to continue.");

            }
        }

        static string PrintJSON(string s)
        {
            return JsonConvert.SerializeObject(JsonConvert.DeserializeObject(s), Formatting.Indented);
        }

    }
}
