using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OktaAuthLab.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult PortalHome()
        {
            string ApiToken = ConfigurationManager.AppSettings["okta:ApiToken"];
            string ApiUrl = ConfigurationManager.AppSettings["okta:ApiUrl"];
            string userId = Session["userid"].ToString();
            string appUrl = ApiUrl + "/api/v1/users/" + userId + "/appLinks";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(appUrl);
            request.Method = "GET";
            request.Headers.Add("Authorization", "SSWS " + ApiToken);
            request.ContentType = "application/json";
            request.Accept = "application/json";

            try
            {
                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                ViewBag.apps = (JArray)JToken.ReadFrom(new JsonTextReader(reader));
            }
            catch (WebException e)
            {
                Console.WriteLine("error: " + e.Message);
                ModelState.AddModelError("", e.Message);
            }

            return View();
        }
    }
}
