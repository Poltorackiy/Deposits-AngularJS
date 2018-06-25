using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microservices.Deposits.Response.Models;
using System.Net;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Microservices.Deposits.Response.Controllers
{
    public class HomeController : Controller
    {
        static readonly string apiHost;
        static readonly string apiDepositsPath;
        static readonly string apiPaymentsPath;

        static HomeController() // Вызывается 1 раз
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

            var configuration = builder.Build();
            var apiSection = configuration.GetSection("API");
            apiHost = apiSection.GetValue<string>("ApiHost");
            apiDepositsPath = apiSection.GetValue<string>("GetDepositsPath");
            apiPaymentsPath = apiSection.GetValue<string>("PaymentsPath");
        }

        public IActionResult Index()
        {
            try
            {
                var depositsResponse = DownloadDeposits();
                var selectList = new SelectList(depositsResponse.Body.Deposits, "Guid", "Name");
                ViewBag.Deposits = selectList;
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }

            return View();
        }

        public IActionResult Payments(string guid, int months, decimal initialAmount)
        {
            DepositPaymentsCollection payments = null;
            try
            {
                Guid depGuid = Guid.Parse(guid);
                var depositsResponse = DownloadDeposits();
                if (depositsResponse.Body.Deposits.Count(d => d.Guid == depGuid) <= 0)
                    throw new NullReferenceException("Депозит не найден");

                payments = DownloadPayments(guid, initialAmount, months).Body;
            }
            catch (NullReferenceException ex)
            {
                NotFound();
                ViewData["ErrorMessage"] = ex.Message;
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            };

            return View(payments);
        }

        private DepositsResponse DownloadDeposits()
        {
            Uri path = new Uri(apiHost);
            path = new Uri(path, apiDepositsPath);
            var webRequest = (HttpWebRequest)WebRequest.Create(path);

            WebResponse response = null;
            try
            {
                response = webRequest.GetResponse();

                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var json = reader.ReadToEnd();
                    return JsonConvert.DeserializeObject<DepositsResponse>(json);
                }
            }
            
            finally
            {
                if (response != null)
                    response.Dispose();
            }
        }

        private DepositPaymentsResponse DownloadPayments(string guid, decimal amount, int months)
        {
            Uri path = new Uri(apiHost);
            path = new Uri(path, apiPaymentsPath+"/");
            path = new Uri(path, guid+"/");
            path = new Uri(path, amount.ToString()+"/");
            path = new Uri(path, months.ToString());
            var webRequest = (HttpWebRequest)WebRequest.Create(path);

            WebResponse response = null;
            try
            {
                response = webRequest.GetResponse();

                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var json = reader.ReadToEnd();
                    return JsonConvert.DeserializeObject<DepositPaymentsResponse>(json);
                }
            }

            finally
            {
                if (response != null)
                    response.Dispose();
            }
        }



        [Produces("application/json")]
        [Route("api")]
        public IResponse GetDeposits()
        {
            var response = new Models.DepositsResponse();
            response.StatusCode = "200";
            response.Message = "OK";

            Deposit dep = new Deposit();
            dep.Capitalization = true;
            dep.Guid = Guid.NewGuid();
            dep.Name = "Пробник";
            dep.InterestRate = 6.2M;

            var deps = new DepositsCollection();
            deps.Deposits.Add(dep);

            response.Body = deps;
            return response;
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
    }
}
