using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading;
using TechTalk.SpecFlow;

namespace selenium_zap.steps
{
    [Binding]
    public class ajaxspider
    {
        [Given(@"Url should be opened")]
        public void GivenUrlShouldBeOpened()
        {
            var zapApiKey = "37h604kvh2cset0t8c1b7td3m6";
            Console.WriteLine("zapapikeyis:" + zapApiKey);
            var zapUrl = "http://localhost:8081";
            Console.WriteLine("zapUrl:" + zapUrl);
            var scanUrl = "https://www.nathcorp.com";
            Console.WriteLine("scanUrl:" + scanUrl);

            var httpClient = new HttpClient();
            var startAjaxSpiderUrl = $"{zapUrl}/JSON/ajaxSpider/action/scan/?apikey={zapApiKey}";
            var response = httpClient.GetAsync(startAjaxSpiderUrl).Result;
            var responseBody = response.Content.ReadAsStringAsync().Result;
            var scanId = JsonConvert.DeserializeObject<dynamic>(responseBody).scan;



            // Existing code...

            // Start AJAX spider
           
            Console.WriteLine("ajaxScanId: " + scanId);

            // Wait for AJAX spider to complete
            while (true)
            {
                var ajaxStatusUrl = $"{zapUrl}/JSON/ajaxSpider/view/status/?scanId={scanId}&apikey={zapApiKey}";
                Console.WriteLine("ajaxStatusUrl: " + ajaxStatusUrl);
                var ajaxStatusResponse = httpClient.GetAsync(ajaxStatusUrl).Result;
                var ajaxStatusResponseBody = ajaxStatusResponse.Content.ReadAsStringAsync().Result;
                var ajaxStatus = JsonConvert.DeserializeObject<dynamic>(ajaxStatusResponseBody).status;

                if (ajaxStatus == "stopped")
                    break;
                else
                    Thread.Sleep(1000);
            }

            // Retrieve the AJAX spider results
            var ajaxResultsUrl = $"{zapUrl}/JSON/ajaxSpider/view/results/?scanId={scanId}&apikey={zapApiKey}";
            Console.WriteLine("ajaxResultsUrl: " + ajaxResultsUrl);
            var ajaxResultsResponse = httpClient.GetAsync(ajaxResultsUrl).Result;
            var ajaxResults = ajaxResultsResponse.Content.ReadAsStringAsync().Result;
            Console.WriteLine("ajaxResults: " + ajaxResults);

            // Process the AJAX spider results
            // ...

            // Existing code...
        }
    }
}
