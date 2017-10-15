using AnswerPoortal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AnswerPoortal.Controllers
{

    [App_Start.AccessDeniedAuthorize(Roles = "Professor, Student")]
    public class TokenInfoController : Controller
    {
        // GET: TokenInfo
        public ActionResult Index()
        {
            AnswerPortalEntities db = new AnswerPortalEntities();
            TokenInfoModel tim = new TokenInfoModel();
            var L2EQuery = db.Configs;
            var conf = L2EQuery.FirstOrDefault<Models.Config>();
            tim.gold = conf.goldPriceG;
            tim.silver = conf.silverPriceS;
            tim.platinum = conf.platinumPriceP;
            var L2EQuery1 = db.Accounts.Where(s => s.email == User.Identity.Name);
            var acc = L2EQuery1.FirstOrDefault<Models.Account>();
            tim.tokenNum = (int)acc.tokenNum;

            return View(tim);
        }

        // POST: TokenInfo/Gold
        [HttpPost]
        public ActionResult Gold(String phoneNum) {
            AnswerPortalEntities db = new AnswerPortalEntities();
            var L2EQuery1 = db.Accounts.Where(s => s.email == User.Identity.Name);
            var acc = L2EQuery1.FirstOrDefault<Models.Account>();
            Order ord = new Order();
            ord.idAccount = acc.id;
            ord.tokenPrice = 50;
            var L2EQuery = db.Configs;
            var conf = L2EQuery.FirstOrDefault<Models.Config>();
            ord.tokenNum = conf.goldPriceG;
            ord.status = "unconfirmed";
            db.Orders.Add(ord);
            db.SaveChanges();
            int ordId = ord.id;
            String link = "http://stage.centili.com/payment/widget?apikey=7e7f74bcffc30dba2774d5e65b903d3f&returnurl=http://localhost:63250/TokenInfo/CentiliNotif&userid=" + ordId + "&msisdn=" + phoneNum;
            return Redirect(link);
        }

        // POST: TokenInfo/Silver
        [HttpPost]
        public ActionResult Silver(String phoneNum) {
            AnswerPortalEntities db = new AnswerPortalEntities();
            var L2EQuery1 = db.Accounts.Where(s => s.email == User.Identity.Name);
            var acc = L2EQuery1.FirstOrDefault<Models.Account>();
            Order ord = new Order();
            ord.idAccount = acc.id;
            ord.tokenPrice = 50;
            var L2EQuery = db.Configs;
            var conf = L2EQuery.FirstOrDefault<Models.Config>();
            ord.tokenNum = conf.silverPriceS;
            ord.status = "unconfirmed";
            db.Orders.Add(ord);
            db.SaveChanges();
            int ordId = ord.id;
            String link = "http://stage.centili.com/payment/widget?apikey=7e7f74bcffc30dba2774d5e65b903d3f&returnurl=http://localhost:63250/TokenInfo/CentiliNotif&userid=" + ordId + "&msisdn=" + phoneNum;
            return Redirect(link);
            //idOrder, phoneNum, returnURL za link ka centiliju
        }

        // POST: TokenInfo/Platinum
        [HttpPost]
        public ActionResult Platinum(String phoneNum)
        {
            AnswerPortalEntities db = new AnswerPortalEntities();
            var L2EQuery1 = db.Accounts.Where(s => s.email == User.Identity.Name);
            var acc = L2EQuery1.FirstOrDefault<Models.Account>();
            Order ord = new Order();
            ord.idAccount = acc.id;
            ord.tokenPrice = 50;
            var L2EQuery = db.Configs;
            var conf = L2EQuery.FirstOrDefault<Models.Config>();
            ord.tokenNum = conf.platinumPriceP;
            ord.status = "unconfirmed";
            db.Orders.Add(ord);
            db.SaveChanges();
            int ordId = ord.id;
            String link = "http://stage.centili.com/payment/widget?apikey=7e7f74bcffc30dba2774d5e65b903d3f&returnurl=http://localhost:63250/TokenInfo/CentiliNotif&userid=" + ordId + "&msisdn=" + phoneNum;
            return Redirect(link);

        }


        [AllowAnonymous]
        //[EnableCors("AllowSpecificOrigin")]
        public ActionResult CentiliNotif(String status, int userid) {
            AnswerPortalEntities db = new AnswerPortalEntities();
            var L2EQuery1 = db.Orders.Where(s => s.id == userid);
            var ord = L2EQuery1.FirstOrDefault<Models.Order>();
            ord.status = status;
            db.Orders.Add(ord);
            db.SaveChanges();
            var L2EQuery = db.Accounts.Where(s => s.id == ord.idAccount);
            var account = L2EQuery.FirstOrDefault<Models.Account>();
            if (status == "success") {

                account.tokenNum += ord.tokenNum;
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();

            }
            OrderStatus ordStat = new OrderStatus();
            ordStat.email = account.email;
            ordStat.status = status;
            ordStat.ordId = userid;
            if (status == "success" || status == "failed") sendMail(ordStat);
            return View("OrderStatus", ordStat);
            
        }


        public void sendMail(OrderStatus ord)
        {

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress("wagon035@gmail.com", "AnswerPortal");
            mail.To.Add(ord.email);
            mail.Subject = "Token Order";
            if (ord.status == "success")
            {
                mail.Body = "Your order was successful.";
            }
            else if (ord.status == "failed") {
                mail.Body = "Your order failed.";
            }
            mail.Body = mail.Body + " Id of your order: " + ord.ordId;
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("wagon035@gmail.com", "vagonce1995");
            SmtpServer.EnableSsl = true;
            SmtpServer.Send(mail);

        }


        // GET: TokenInfo/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TokenInfo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TokenInfo/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: TokenInfo/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TokenInfo/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: TokenInfo/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TokenInfo/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
