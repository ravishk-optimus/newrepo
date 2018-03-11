using CompanyLocator.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CompanyLocator.Controllers
{
    /// <summary>
    /// This Class contains controllers for MVC architecture
    /// </summary>
    public class HomePageController : Controller
    {

        /// <summary>
        /// This Action handler handles all the GET Requests
        /// </summary>
        [HttpGet]
        public ActionResult Home()
        {
            return View();
        }

        /// <summary>
        /// This method deals with HttpPost Methods
        /// </summary>
        /// <param name="locationBox">It conatains location query entered by user</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Home(string locationBox)
        {
            Parsing parser = new Parsing();
            var resultlist = parser.GetLocation(locationBox);

            //MAPPING
            if (resultlist.Count != 0)
            {
                var resultCompany = new List<Company>();
                foreach (var item in resultlist)
                {
                    var companylist = new Company
                    {
                        Name = item.Name,
                        Address = item.Address
                    };
                    resultCompany.Add(companylist);
                }
                return View(resultCompany);
            }

            else
            {

                return View("NoCompanyFound");
            }
        }
    }
}