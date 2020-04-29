using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using MVCOemahJowo.Models;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace MVCOemahJowo.Controllers.CustomerMaster
{
    public class CustomerController : Controller
    {
        private oemahjowodbEntities db = new oemahjowodbEntities();
        // GET: Customer
        public ActionResult Index()
        {
            return View();
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
                List<mt_customer> custDt = db.mt_customer.ToList().Select(dt => new mt_customer
                {
                    CUST_ID = dt.CUST_ID,
                    CUST_NAME = dt.CUST_NAME,
                    PHONE = dt.PHONE,
                    ADDRESS = dt.ADDRESS,
                    EMAIL = dt.EMAIL,
                    ENTRY_USER = dt.ENTRY_USER,
                    ENTRY_DATE = dt.ENTRY_DATE,
                    UPDATE_DATE = dt.UPDATE_DATE,
                    UPDATE_USER = dt.UPDATE_USER
                }).ToList<mt_customer>();
                return Json(new { data = custDt }, JsonRequestBehavior.AllowGet);
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
        public ActionResult Edit(string custid)
        {
            var userobject = Session["id"];
            if (userobject == null)
            {
                return RedirectToAction("Index", "Login");

            }
            else
            {
                custid = Request.QueryString["custid"];
                int ids = Convert.ToInt32(custid);
                using (db)
                {

                    var formDt = db.mt_customer.Where(x => x.CUST_ID == ids).FirstOrDefault<mt_customer>();
                    return View(formDt);
                }
            }
        }
        [HttpPost]
        //POST: Insert to database logic
        public ActionResult Insert(mt_customer custDt)
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
                custDt.ENTRY_DATE = DateTime.Now;
                custDt.ENTRY_USER = userobject.ToString();
                custDt.UPDATE_DATE = DateTime.Now;
                custDt.UPDATE_USER = userobject.ToString();
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
                        db.mt_customer.Add(custDt);
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
        public ActionResult Update(mt_customer custDt)
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

                custDt.UPDATE_DATE = DateTime.Now;
                custDt.UPDATE_USER = userobject.ToString();
                using (db)
                {
                    var id = custDt.CUST_ID;
                    var dtfromdb = db.mt_customer.Find(id);
                    dtfromdb.CUST_NAME = custDt.CUST_NAME;
                    dtfromdb.PHONE = custDt.PHONE;
                    dtfromdb.ADDRESS = custDt.ADDRESS;
                    dtfromdb.EMAIL = custDt.EMAIL;
                    dtfromdb.UPDATE_DATE = custDt.UPDATE_DATE;
                    dtfromdb.UPDATE_USER = custDt.UPDATE_USER;
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
                custid = Request.QueryString["custid"];
                int ids = Convert.ToInt32(custid);
                using (db)
                {
                    mt_customer custDt = db.mt_customer.Where(x => x.CUST_ID == ids).FirstOrDefault<mt_customer>();
                    try
                    {
                        db.mt_customer.Remove(custDt);
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