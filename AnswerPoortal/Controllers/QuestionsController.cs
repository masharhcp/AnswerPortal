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
    public class QuestionsController : Controller
    {
        private AnswerPortalEntities db = new AnswerPortalEntities();

        // GET: Questions
        public ActionResult Index()
        {

            var query =
                      from o in db.Questions
                      join a in db.Accounts on o.idAccount equals a.id
                      where a.email == User.Identity.Name
                      select new { o.Account, o.Answers, o.creationDate, o.id, o.idAccount, o.lockDate, o.pic_path, o.QuestionCopies, o.status, o.text, o.title};
            var questions = query.ToList();
            List<Question> li = new List<Question>();
            foreach (var q in questions) {
                Question qu = new Question();
                qu.Account = q.Account;
                qu.Answers = q.Answers;
                qu.creationDate = q.creationDate;
                qu.id = q.id;
                qu.idAccount = q.idAccount;
                qu.lockDate = q.lockDate;
                qu.pic_path = q.pic_path;
                qu.QuestionCopies = q.QuestionCopies;
                qu.status = q.status;
                qu.text = q.text;
                qu.title = q.title;
                li.Add(qu);
            }
            //var questions = db.Questions.Include(q => q.Account);
            return View(li);
        }

        // GET: Questions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // GET: Questions/Create
        public ActionResult Create()
        {
            AnswerPortalEntities db = new AnswerPortalEntities();
            var cong = db.Configs.First();
            ViewBag.NumberOfAnswers = cong.answerPriceK;
            //ViewBag.idAccount = new SelectList(db.Accounts, "id", "name");

            return View();
        }

        // POST: Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "title,text,ImageToUpload,status, Answers")] Question question)
        {
            question.pic_path = new byte[question.ImageToUpload.ContentLength];
            question.ImageToUpload.InputStream.Read(question.pic_path, 0, question.pic_path.Length);
            if (ModelState.IsValid)
            {
                for(int i = 0; i<question.Answers.Count; i++) {
                    if (i == 0) question.Answers[i].isCorrect = true;
                    else question.Answers[i].isCorrect = false;
                    }

               
                DateTime dt = System.DateTime.Now;
                var query = db.Accounts.Where(s => s.email == User.Identity.Name);
                Account acc = query.FirstOrDefault();
                question.idAccount = acc.id;
                question.creationDate = dt;
                db.Questions.Add(question);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var cong = db.Configs.First();
            ViewBag.NumberOfAnswers = cong.answerPriceK;
            ViewBag.idAccount = new SelectList(db.Accounts, "id", "name", question.idAccount);
            return View();
        }

        // GET: Questions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            ViewBag.idAccount = new SelectList(db.Accounts, "id", "name", question.idAccount);
            return View(question);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,title,text,ImageToUpload,status,idAccount,creationDate, lockDate, Answers")] Question question)
        {
            var query = db.Accounts.Where(s => s.email == User.Identity.Name);
            
            //Account acc = query.FirstOrDefault();
            //question.idAccount = acc.id;
            var config = db.Configs.Find(1);
           // Question questionorig = db.Questions.Find(question.id);
           if (question.ImageToUpload != null) { 
            question.pic_path = new byte[question.ImageToUpload.ContentLength];
            question.ImageToUpload.InputStream.Read(question.pic_path, 0, question.pic_path.Length);
            
            }
            if (ModelState.IsValid)
            {

                //if (questionorig.status == true && question.status == false && (acc.tokenNum < config.unlockPriceM)) return View("UnlockFailed");
                //else if (questionorig.status == true && question.status == false && (acc.tokenNum >= config.unlockPriceM)) {

                //  acc.tokenNum = acc.tokenNum - config.unlockPriceM;
                //}

                // if (questionorig.status == false && question.status == true) { question.lockDate = DateTime.Now; }
                //db.Entry(acc).State = EntityState.Modified;
                
                db.Entry(question).State = EntityState.Modified;
                if (question.ImageToUpload == null) db.Entry(question).Property(x => x.pic_path).IsModified = false;
                foreach (var answ in question.Answers) {
                    db.Entry(answ).State = EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idAccount = new SelectList(db.Accounts, "id", "name", question.idAccount);
            return View(question);
        }

        // GET: Questions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Question question = db.Questions.Find(id);
            question.Account = null;
            db.Questions.Remove(question);
          
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        // GET: Questions/Delete/5
        public ActionResult Lock(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Lock")]
        [ValidateAntiForgeryToken]
        public ActionResult LockConfirmed(int id)
        {

            Question question = db.Questions.Find(id);
            question.lockDate = System.DateTime.Now;
            question.status = true;
            db.Entry(question).Property(x => x.status).IsModified = true;
            db.Entry(question).Property(x => x.lockDate).IsModified = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Questions/Delete/5
        public ActionResult Unlock(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Unlock")]
        [ValidateAntiForgeryToken]
        public ActionResult UnlockConfirmed(int id)
        {
            Question question = db.Questions.Find(id);
            Account acc = question.Account;
            Config conf = db.Configs.Find(1);
            if (acc.tokenNum >= conf.unlockPriceM)
            {
                acc.tokenNum = acc.tokenNum - conf.unlockPriceM;
                question.status = false;
                db.Entry(question).Property(x => x.status).IsModified = true;
                db.Entry(acc).Property(x => x.tokenNum).IsModified = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else return View("UnlockFailed");

   
            
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
