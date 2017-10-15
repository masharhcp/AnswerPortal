using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AnswerPoortal.Models;

namespace AnswerPoortal.Controllers
{
    public class OrdersController : Controller
    {
        private AnswerPortalEntities db = new AnswerPortalEntities();

        // GET: Orders
        public ActionResult Index()
        {
            var query =
                      from o in db.Orders
                      join a in db.Accounts on o.idAccount equals a.id
                      where a.email == User.Identity.Name
                      select new {  o.id, o.tokenNum, o.tokenPrice, o.status, o.idAccount, a.email };
            var orders = query.ToList();
            List<Order> li = new List<Order>();
            foreach (var i in orders)
            {
              Order o = new Order();
                o.id = i.id;
                o.idAccount = i.idAccount;
                o.status = i.status;
                o.tokenNum = i.tokenNum;
                o.tokenPrice = i.tokenPrice;
                li.Add(o);
            }

            return View(li);
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }


            var L2EQuery = db.Accounts.Where(s => s.id == order.idAccount);
            var account = L2EQuery.FirstOrDefault<Models.Account>();
            OrderVO ov = new OrderVO();
            ov.id = order.id;
            ov.idAccount = order.idAccount;
            ov.status = order.status;
            ov.tokenNum = order.tokenNum;
            ov.tokenPrice = order.tokenPrice;
            ov.email = account.email.Trim();
            return View(ov);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.idAccount = new SelectList(db.Accounts, "id", "name");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,tokenNum,tokenPrice,status,idAccount")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idAccount = new SelectList(db.Accounts, "id", "name", order.idAccount);
            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.idAccount = new SelectList(db.Accounts, "id", "name", order.idAccount);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,tokenNum,tokenPrice,status,idAccount")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idAccount = new SelectList(db.Accounts, "id", "name", order.idAccount);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
