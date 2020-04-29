using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCOemahJowo.Models;
using System.Data;
using System.Data.Entity;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace MVCOemahJowo.Controllers.ProductMaster
{
    public class ProductController : Controller
    {
        private oemahjowodbEntities db = new oemahjowodbEntities();

        // GET: Product
        public ActionResult Index()
        {
            var id = Session["id"];
            if(id == null)
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
                List<mt_prod> prodDt = db.mt_prod.ToList().Select(dt => new mt_prod { PROD_ID = dt.PROD_ID, 
                    PROD_NAME = dt.PROD_NAME, DESCRIPTION = dt.DESCRIPTION, PRICE = dt.PRICE, ENTRY_USER = dt.ENTRY_USER, ENTRY_DATE = dt.ENTRY_DATE,
                    UPDATE_DATE = dt.UPDATE_DATE, UPDATE_USER = dt.UPDATE_USER}).ToList<mt_prod>();
                return Json(new { data = prodDt }, JsonRequestBehavior.AllowGet);
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
        //GET: /BBSActionList/Edit
        //NB: Edit popup is being separated for the time being
        public ActionResult Edit(string prodid)
        {
            var userobject = Session["id"];
            if (userobject == null)
            {
                return RedirectToAction("Index", "Login");

            }
            else
            {
                prodid = Request.QueryString["prodid"];
                int ids = Convert.ToInt32(prodid);
                using (db)
                {
                    
                    var formDt = db.mt_prod.Where(x => x.PROD_ID == ids).FirstOrDefault<mt_prod>();
                    return View(formDt);
                }
            }
        }

        [HttpPost]
        //POST: Insert to database logic
        public ActionResult Insert(mt_prod prodDt)
        {
            var userobject = Session["id"];
            if (userobject == null)
            {
                return RedirectToAction("Index", "Login");

            }
            else
            {
                mt_prod dt;

                var editmode = Request.QueryString["editmode"];
                var username = User.Identity.Name;
                prodDt.ENTRY_DATE = DateTime.Now;
                prodDt.ENTRY_USER = userobject.ToString();
                prodDt.UPDATE_DATE = DateTime.Now;
                prodDt.UPDATE_USER = userobject.ToString();
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
                    db.mt_prod.Add(prodDt);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved successfully" }, JsonRequestBehavior.AllowGet);


                }



            }

        }

        //POST: Update to database logic
        public ActionResult Update(mt_prod prodDt)
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

                prodDt.UPDATE_DATE = DateTime.Now;
                prodDt.UPDATE_USER = userobject.ToString();
                using (db)
                {
                    var id = prodDt.PROD_ID;
                    var dtfromdb = db.mt_prod.Find(id);
                    dtfromdb.PROD_NAME = prodDt.PROD_NAME;
                    dtfromdb.DESCRIPTION = prodDt.DESCRIPTION;
                    dtfromdb.PRICE = prodDt.PRICE;
                    dtfromdb.UPDATE_DATE = prodDt.UPDATE_DATE;
                    dtfromdb.UPDATE_USER = prodDt.UPDATE_USER;
                    db.Entry(dtfromdb).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Updated successfully" }, JsonRequestBehavior.AllowGet);

                }
            }

        }

        [HttpPost]
        //POST: delete from database logic
        public ActionResult Delete(string prodid)
        {
            var userobject = Session["id"];
            if (userobject == null)
            {
                return RedirectToAction("Index", "Login");

            }
            else
            {
                prodid = Request.QueryString["prodid"];
                int ids = Convert.ToInt32(prodid);
                using (db)
                {
                    mt_prod prodDt = db.mt_prod.Where(x => x.PROD_ID == ids).FirstOrDefault<mt_prod>();
                    db.mt_prod.Remove(prodDt);
                    db.SaveChanges();
                }
                return Json(new { success = true, message = "Deleted successfully" }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}