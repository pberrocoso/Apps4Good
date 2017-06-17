using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;


namespace SiderApi.ErrorControllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        [System.Web.Mvc.Route("Error/NotFound")]
        public ActionResult NotFoundPage()
        {
            Response.StatusCode = 404;  
            return View("Error404");
        }
    }
}