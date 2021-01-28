using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCOemahJowo.Models;
using System.Data.Entity;
using System.Data;

namespace MVCOemahJowo.Controllers.CostMaster
{
    public class CostController : Controller
    {
        private oemahjowodbEntities db = new oemahjowodbEntities();

        // GET: Cost
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
                List<mg_shoplist> costDt = db.mg_shoplist.ToList().Select(dt => new mg_shoplist
                {
                    ID = dt.ID,
                    NOTES = dt.NOTES,
                    AMOUNT = dt.AMOUNT,
                    TRANS_DATE = dt.TRANS_DATE,
                    ENTRY_USER = dt.ENTRY_USER,
                    ENTRY_DATE = dt.ENTRY_DATE,
                    UPDATE_DATE = dt.UPDATE_DATE,
                    UPDATE_USER = dt.UPDATE_USER
                }).ToList<mg_shoplist>();
                return Json(new { data = costDt }, JsonRequestBehavior.AllowGet);
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
        public ActionResult Edit(string costid)
        {
            var userobject = Session["id"];
            if (userobject == null)
            {
                return RedirectToAction("Index", "Login");

            }
            else
            {
                costid = Request.QueryString["costid"];
                int ids = Convert.ToInt32(costid);
                using (db)
                {

                    var formDt = db.mg_shoplist.Where(x => x.ID == ids).FirstOrDefault<mg_shoplist>();
                    return View(formDt);
                }
            }
        }
        [HttpPost]
        //POST: Insert to database logic
        public ActionResult Insert(mg_shoplist costDt)
        {
            var userobject = Session["id"];
            if (userobject == null)
            {
                return RedirectToAction("Index", "Login");

            }
            else
            {
                costDt.TRANS_DATE = Convert.ToDateTime(costDt.TRANS_DATE.ToString("MM/dd/yyyy"));
                var editmode = Request.QueryString["editmode"];
                var username = User.Identity.Name;
                costDt.ENTRY_DATE = DateTime.Now;
                costDt.ENTRY_USER = userobject.ToString();
                costDt.UPDATE_DATE = DateTime.Now;
                costDt.UPDATE_USER = userobject.ToString();
                var dtname = username;

                using (db)
                {
                    try
                    {
                        db.mg_shoplist.Add(costDt);
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
        public ActionResult Update(mg_shoplist costDt)
        {
            var userobject = Session["id"];
            if (userobject == null)
            {
                return RedirectToAction("Index", "Login");

            }
            else
            {
                var editmode = Request.QueryString["editmode"];
                var username = User.Identity.Name;
                costDt.TRANS_DATE = Convert.ToDateTime(costDt.TRANS_DATE.ToString("MM/dd/yyyy"));
                costDt.UPDATE_DATE = DateTime.Now;
                costDt.UPDATE_USER = userobject.ToString();
                using (db)
                {
                    var id = costDt.ID;
                    var dtfromdb = db.mg_shoplist.Find(id);
                    dtfromdb.NOTES = costDt.NOTES;
                    dtfromdb.AMOUNT = costDt.AMOUNT;
                    dtfromdb.TRANS_DATE = costDt.TRANS_DATE;
                    dtfromdb.UPDATE_DATE = costDt.UPDATE_DATE;
                    dtfromdb.UPDATE_USER = costDt.UPDATE_USER;
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
        public ActionResult Delete(string costid)
        {
            var userobject = Session["id"];
            if (userobject == null)
            {
                return RedirectToAction("Index", "Login");

            }
            else
            {
                costid = Request.QueryString["costid"];
                int ids = Convert.ToInt32(costid);
                using (db)
                {
                    mg_shoplist costdt = db.mg_shoplist.Where(x => x.ID == ids).FirstOrDefault<mg_shoplist>();
                    try
                    {
                        db.mg_shoplist.Remove(costdt);
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