using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using TechTalk.SpecFlow;
//using Xunit.Sdk;

namespace Project1.Steps
{
    [Binding]
    public class SpecFlowFeature1Steps
    {
        public static string Path = AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin\\Debug", "\\Result");

        public IWebDriver driver;
        string scanid;
        string alertsFilePath = @"C:\Users\Kulde\source\repos\selenium-zap\selenium-zap\steps\alerts.txt";
        [Given(@"Url should be open")]
        public void GivenUrlShouldBeOpen()
        {
            var proxy = new Proxy { HttpProxy = "localhost:8081", SslProxy = "localhost:8081" };
            var options = new ChromeOptions { Proxy = proxy };
            driver = new ChromeDriver(options);
            options.AddArgument("--proxy-server=http://localhost:8081");
            //   driver.Navigate().GoToUrl("https://www.nathcorp.com");
            // driver.Manage().Window.Maximize();

            var zapApiKey = "37h604kvh2cset0t8c1b7td3m6";
            Console.WriteLine("zapapikeyis:" + zapApiKey);
            var zapUrl = "http://localhost:8081";
            Console.WriteLine("zapUrl:" + zapUrl);
            var scanUrl = "http://www.nathcorp.com";
            Console.WriteLine("scanUrl:" + scanUrl);
            var httpClient = new HttpClient();
            Console.WriteLine("httpClient:" + httpClient);
            var scanApiUrl = $"{zapUrl}/JSON/spider/action/scan/?apikey={zapApiKey}&url={scanUrl}";
            Console.WriteLine("scanApiUrl:" + scanApiUrl);
            var response = httpClient.GetAsync(scanApiUrl).Result;
            string responseBody = response.Content.ReadAsStringAsync().Result;
            //Console.WriteLine("response:" + responseBody);
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = response.Content.ReadAsStringAsync().Result;
                dynamic data = JsonConvert.DeserializeObject(jsonResponse);

                scanid = data.scan;
                while (true)
                {
                    //http://localhost:8081/JSON/spider/view/status/?scanId=2&apikey=37h604kvh2cset0t8c1b7td3m6
                    scanApiUrl = $"{zapUrl}/JSON/spider/view/status/?scanId={scanid}&apikey=37h604kvh2cset0t8c1b7td3m6";
                    //Console.WriteLine("scanApiUrl:" + scanApiUrl);
                    response = httpClient.GetAsync(scanApiUrl).Result;
                    responseBody = response.Content.ReadAsStringAsync().Result;
                    //Console.WriteLine("response:" + responseBody);

                    jsonResponse = response.Content.ReadAsStringAsync().Result;
                    data = JsonConvert.DeserializeObject(jsonResponse);

                    var Status = data.status;

                    if (Status > "30")
                        break;
                    else
                        Thread.Sleep(1000);
                }
            }
            else
            {
                Console.WriteLine("Error: " + response.StatusCode);
            }


            //stop scan
            //http://zap-api-host:port/JSON/ascan/action/stopAllScans/?zapapiformat=JSON&apikey=your-api-key

            scanApiUrl = $"{zapUrl}/JSON/spider/action/stopAllScans/?zapapiformat=JSON&apikey={zapApiKey}";
            Console.WriteLine("scanApiUrl:" + scanApiUrl);
            response = httpClient.GetAsync(scanApiUrl).Result;
            responseBody = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine("response:" + responseBody);



            ////ajax spider
            //var startAjaxSpiderUrl = $"{zapUrl}/JSON/ajaxSpider/action/scan/?apikey={zapApiKey}&url={scanUrl}";
            // response = httpClient.GetAsync(startAjaxSpiderUrl).Result;
            // responseBody = response.Content.ReadAsStringAsync().Result;
            //var scanId = JsonConvert.DeserializeObject<dynamic>(responseBody).scan;

            //// Existing code...

            //// Start AJAX spider


            //// Wait for AJAX spider to complete
            //while (true)
            //{

            //    var ajaxStatusUrl = $"{zapUrl}/JSON/ajaxSpider/view/status/?apikey={zapApiKey}";
            //    //Console.WriteLine("ajaxStatusUrl: " + ajaxStatusUrl);

            //    var ajaxStatusResponse = httpClient.GetAsync(ajaxStatusUrl).Result;
            //    var ajaxStatusResponseBody = ajaxStatusResponse.Content.ReadAsStringAsync().Result;
            //    var ajaxStatus = JsonConvert.DeserializeObject<dynamic>(ajaxStatusResponseBody).status;
            //    Console.WriteLine(ajaxStatus);
            //    if (ajaxStatus == "stopped")
            //        break;
            //    else
            //        Thread.Sleep(8000);
            //}

            //start active scanning
            //http://localhost:8080/JSON/ascan/action/scan/?zapapiformat=JSON&apikey=<API_KEY>&url=<TARGET_URL>
            //scanApiUrl = $"{zapUrl}/JSON/ascan/action/scan/?zapapiformat=JSON&apikey={zapApiKey}&url={scanUrl}";
            //Console.WriteLine("scanApiUrl:" + scanApiUrl);
            //response = httpClient.GetAsync(scanApiUrl).Result;
            //responseBody = response.Content.ReadAsStringAsync().Result;

            //if (response.IsSuccessStatusCode)
            //{
            //    string jsonResponse = response.Content.ReadAsStringAsync().Result;
            //    dynamic data = JsonConvert.DeserializeObject(jsonResponse);

            //    scanid = data.scan;
            //    while (true)
            //    {

            //        scanApiUrl = $"{zapUrl}/JSON/ascan/view/status/?zapapiformat=JSON&scanId={scanid}";
            //        //Console.WriteLine("scanApiUrl:" + scanApiUrl);
            //        response = httpClient.GetAsync(scanApiUrl).Result;
            //        responseBody = response.Content.ReadAsStringAsync().Result;
            //        //Console.WriteLine("response:" + responseBody);

            //        jsonResponse = response.Content.ReadAsStringAsync().Result;
            //        data = JsonConvert.DeserializeObject(jsonResponse);

            //        var Status = data.status;

            //        if (Status == "100")
            //            break;
            //        else
            //            Thread.Sleep(1000);
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("Error: " + response.StatusCode);
            //}



            ///Find all the vulnearabilities
            //var start = 0;
            //var pagesize = 100;
            //while(start<100)
            //{
            ////Retrieve the scan results
            //var reportApiUrl = $"{zapUrl}/JSON/core/view/alerts/?apikey={zapApiKey}&start={start}&count={pagesize}";
            ////Console.WriteLine("reportApiUrl:" + reportApiUrl);
            //var reportResponse = httpClient.GetAsync(reportApiUrl).Result;
            // //Console.WriteLine("reportResponse:" + reportResponse);
            // var scanResults = reportResponse.Content.ReadAsStringAsync().Result;
            // //Console.WriteLine("scanResults" + scanResults);

            //    start++;
            //   // Console.WriteLine("start"+start);
            //File.WriteAllText(alertsFilePath, scanResults);
            //}

            //generate report

            
            driver.Navigate().GoToUrl("http://localhost:8081/OTHER/core/other/htmlreport/?zapapiformat=JSON&apikey=37h604kvh2cset0t8c1b7td3m6&out=report.html");
            // Get the page source
            string pageSource = driver.PageSource;

            // Save the page source to a file
            string filePath = Path+"test.html";
            System.IO.File.WriteAllText(filePath, pageSource);

            Thread.Sleep(10000);
            driver.Close();
        }


    }
    
}

