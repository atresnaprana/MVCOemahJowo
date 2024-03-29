﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCOemahJowo.Models;
using System.Data.Entity;
using System.Data;
using System.Windows.Forms;

namespace MVCOemahJowo.Controllers.CostMaster
{
    public class CostController : Controller
    {
        private oemahjowodbEntities db = new oemahjowodbEntities();

        // GET: Cost
        public ActionResult Index()
        {
            if (Session["id"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("index");
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
                    mg_shoplist2 forms = new mg_shoplist2();
                    forms.ID = formDt.ID;
                    forms.TRANS_DATE_STR = formDt.TRANS_DATE.ToString("dd/MM/yyyy");
                    forms.AMOUNT = formDt.AMOUNT;
                    forms.NOTES = formDt.NOTES;
                    return View(forms);
                }
            }
        }
        [HttpPost]
        //POST: Insert to database logic
        public ActionResult Insert(mg_shoplist2 costDt)
        {
            var userobject = Session["id"];
            if (userobject == null)
            {
                return RedirectToAction("Index", "Login");

            }
            else
            {
                //var dates = Convert.ToDateTime(costDt.TRANS_DATE_STR).ToString("MM/dd/yyyy");
                costDt.TRANS_DATE = Convert.ToDateTime(costDt.TRANS_DATE_STR);
                var editmode = Request.QueryString["editmode"];
                var username = User.Identity.Name;
                var dtname = username;
                mg_shoplist form = new mg_shoplist();
                form.AMOUNT = costDt.AMOUNT;
                form.TRANS_DATE = costDt.TRANS_DATE;
                form.NOTES = costDt.NOTES;
                form.ENTRY_DATE = DateTime.Now;
                form.ENTRY_USER = userobject.ToString();
                form.UPDATE_DATE = DateTime.Now;
                form.UPDATE_USER = userobject.ToString();
                using (db)
                {
                    try
                    {
                        db.mg_shoplist.Add(form);
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
        public ActionResult Update(mg_shoplist2 costDt)
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
                var datestr = costDt.TRANS_DATE_STR;
                costDt.TRANS_DATE = Convert.ToDateTime(datestr);
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