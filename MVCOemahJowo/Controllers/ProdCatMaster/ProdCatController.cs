using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using MVCOemahJowo.Models;
using DocumentFormat.OpenXml.Office2010.Excel;
namespace MVCOemahJowo.Controllers.ProdCatMaster
{
    public class ProdCatController : Controller
    {
        private oemahjowodbEntities db = new oemahjowodbEntities();

        // GET: ProdCat
        public ActionResult Index()
        {
            var id = Session["id"];
            if (id == null)
            {
                return RedirectToAction("Index", "Login");

            }
            else
            {
                return View();
            }
        }
        public ActionResult GetData()
        {
            var id = Session["id"];
            if (id == null)
            {
                return RedirectToAction("Index", "Login");

            }
            else
            {
                List<mt_prod_cat> catDt = db.mt_prod_cat.ToList().Select(dt => new mt_prod_cat
                {
                    id = dt.id,
                    category_name = dt.category_name,
                    amount = dt.amount,
                    entry_user = dt.entry_user,
                    entry_date = dt.entry_date,
                    update_date = dt.update_date,
                    update_user = dt.update_user
                }).ToList<mt_prod_cat>();
                return Json(new { data = catDt }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult AddorEdit(string prodid)
        {
            var id = Session["id"];
            if (id == null)
            {
                return RedirectToAction("Index", "Login");

            }
            else
            {
                return View();
            }
        }
        public ActionResult Edit(string catid)
        {
            var userobject = Session["id"];
            if (userobject == null)
            {
                return RedirectToAction("Index", "Login");

            }
            else
            {
                catid = Request.QueryString["catid"];
                int ids = Convert.ToInt32(catid);
                using (db)
                {

                    var formDt = db.mt_prod_cat.Where(x => x.id == ids).FirstOrDefault<mt_prod_cat>();
                    return View(formDt);
                }
            }
        }
        [HttpPost]
        //POST: Insert to database logic
        public ActionResult Insert(mt_prod_cat catDt)
        {
            var userobject = Session["id"];
            if (userobject == null)
            {
                return RedirectToAction("Index", "Login");

            }
            else
            {

                var editmode = Request.QueryString["editmode"];
                var username = Session["id"].ToString();
                catDt.entry_date = DateTime.Now;
                catDt.entry_user = userobject.ToString();
                catDt.update_date = DateTime.Now;
                catDt.update_user = userobject.ToString();
                var dtname = username;

                using (db)
                {
                    //dt = db.PEKAONLINE_ACTIONLIST.Where(x => x.LSR_NUMBER == actionListDt.LSR_NUMBER).FirstOrDefault<PEKAONLINE_ACTIONLIST>();
                    //if (dt == null)
                    //{

                    //    db.Dispose();
                    //}
                    //else
                    //{
                    //    db.Dispose();
                    //    return Json(new { success = false, message = "Duplicate Entry" }, JsonRequestBehavior.AllowGet);
                    //}
                    try
                    {
                        db.mt_prod_cat.Add(catDt);
                        db.SaveChanges();
                        return Json(new { success = true, message = "Saved successfully" }, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception e)
                    {
                        return Json(new { success = false, message = e.InnerException.Message }, JsonRequestBehavior.AllowGet);
                    }

                }
            }
        }
        [HttpPost]
        //POST: Update to database logic
        public ActionResult Update(mt_prod_cat catDt)
        {
            var userobject = Session["id"];
            if (userobject == null)
            {
                return RedirectToAction("Index", "Login");

            }
            else
            {
                var editmode = Request.QueryString["editmode"];

                catDt.update_date = DateTime.Now;
                catDt.update_user = userobject.ToString();
                using (db)
                {
                    var id = catDt.id;
                    var dtfromdb = db.mt_prod_cat.Find(id);
                    dtfromdb.category_name = catDt.category_name;
                    dtfromdb.amount = catDt.amount;
                    dtfromdb.update_date = catDt.update_date;
                    dtfromdb.update_user = catDt.update_user;
                    try
                    {
                        db.Entry(dtfromdb).State = EntityState.Modified;
                        db.SaveChanges();
                        return Json(new { success = true, message = "Updated successfully" }, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception e)
                    {
                        return Json(new { success = false, message = e.InnerException.Message }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
        }
        [HttpPost]
        //POST: delete from database logic
        public ActionResult Delete(string custid)
        {
            var userobject = Session["id"];
            if (userobject == null)
            {
                return RedirectToAction("Index", "Login");

            }
            else
            {
                custid = Request.QueryString["catid"];
                int ids = Convert.ToInt32(custid);
                using (db)
                {
                    mt_prod_cat catDt = db.mt_prod_cat.Where(x => x.id == ids).FirstOrDefault<mt_prod_cat>();
                    try
                    {
                        db.mt_prod_cat.Remove(catDt);
                        db.SaveChanges();
                        return Json(new { success = true, message = "Deleted successfully" }, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception e)
                    {
                        return Json(new { success = false, message = e.InnerException.Message }, JsonRequestBehavior.AllowGet);
                    }

                }
            }

        }
    }
}