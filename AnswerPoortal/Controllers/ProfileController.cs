using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnswerPoortal.Controllers
{
    [App_Start.AccessDeniedAuthorize(Roles = "Admin , Professor, Student")]
    public class ProfileController : Controller
    {

        private Models.AnswerPortalEntities db = new Models.AnswerPortalEntities();

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

       
        public ActionResult Account() {
            var L2EQuery = db.Accounts.Where(s => s.email == User.Identity.Name);
            var account = L2EQuery.FirstOrDefault<Models.Account>();
            return View(account);
        }

      
        public ActionResult Users()
        {
            return View();
        }

     


    }
}