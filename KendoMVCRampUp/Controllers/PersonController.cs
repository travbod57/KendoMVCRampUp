using KendoMVCRampUp.CustomKendo;
using KendoMVCRampUp.CustomResultTypes;
using KendoMVCRampUp.Dtos;
using KendoMVCRampUp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;

namespace KendoMVCRampUp.Controllers
{
    public class PersonController : Controller
    {
        public new IQueryable<Person> People { get; set; }

        public PersonController()
        {
            People = new List<Person>()
            {
                new Person() { FirstName = "Alex", SecondName = "Williams", Age = "29"  },
                new Person() { FirstName = "Jerry", SecondName = "Smith", Age = "46"  },
                new Person() { FirstName = "Kathy", SecondName = "Wilson", Age = "32"  },
                new Person() { FirstName = "Bernard", SecondName = "Tallin", Age = "16"  },
                new Person() { FirstName = "Harry", SecondName = "Small", Age = "55"  },
                new Person() { FirstName = "Beatty", SecondName = "Boop", Age = "67"  },
                new Person() { FirstName = "Patrick", SecondName = "KOrangtang-Addo", Age = "31"  },
                new Person() { FirstName = "Amit", SecondName = "Dam", Age = "32"  },
                new Person() { FirstName = "Andy", SecondName = "Fooks", Age = "40"  },
                new Person() { FirstName = "Geoff", SecondName = "Goulson", Age = "68"  },
                new Person() { FirstName = "Harry", SecondName = "Croydon", Age = "55"  },
                new Person() { FirstName = "Phil", SecondName = "Harris", Age = "55"  }
            }.AsQueryable<Person>();
        }

        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult SimplePagination()
        {
            return View();
        }

        public JsonResult GetPeopleWithPagination([Kendo.Mvc.UI.DataSourceRequest] Kendo.Mvc.UI.DataSourceRequest request)
        {
            var result = People.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);
            var totalNumberOfItems = People.Count();

            return Json(new { data = result, total = totalNumberOfItems }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ServersideSorting()
        {
            return View();
        }

        public JsonResult GetPeopleWithServersideSorting([Kendo.Mvc.UI.DataSourceRequest] Kendo.Mvc.UI.DataSourceRequest request)
        {
            // the order string to pass to Dynamic LINQ
            string orderByClause = new KendoSortBuilder().CreateString(request.Sorts, "FirstName asc").CleanString();

            // the implementation of server side pagination
            var result = People.OrderBy(orderByClause).Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);
            var totalNumberOfItems = People.Count();

            return Json(new { data = result, total = totalNumberOfItems }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ServersideFiltering()
        {
            return View();
        }

        public JsonResult GetPeopleWithServersideFiltering([Kendo.Mvc.UI.DataSourceRequest] Kendo.Mvc.UI.DataSourceRequest request)
        {
            int totalNumberOfItems;
            IEnumerable<Person> result;

            string orderByClause = new KendoSortBuilder().CreateString(request.Sorts, "FirstName asc").CleanString();

            KendoFilterBuilder b = new KendoFilterBuilder();
            FilterDto kendoFilter = b.AssessFilters(request.Filters).BuildDynamicLinqQuery();


            if (kendoFilter.IsFilterApplying)
            {
                totalNumberOfItems = People.Where(kendoFilter.DynamicLinqString, kendoFilter.Parameters).Count();
                result = People.OrderBy(orderByClause).Where(kendoFilter.DynamicLinqString, kendoFilter.Parameters).Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);
            }
            else
            {
                totalNumberOfItems = People.Count();
                result = People.OrderBy(orderByClause).Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);
            }

            return Json(new { data = result, total = totalNumberOfItems }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExcelExport()
        {
            return View();
        }

        /// <summary>
        /// IE Proxy Excel download hack
        /// </summary>
        /// <param name="contentType"></param>
        /// <param name="base64"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DownloadList(string contentType, string base64, string fileName)
        {
            return new ExcelResult("attachment", base64, fileName);
        }

    }
}