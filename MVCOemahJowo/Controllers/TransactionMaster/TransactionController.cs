using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCOemahJowo.Models;
using System.Data.Entity;
using System.Data;

namespace MVCOemahJowo.Controllers.TransactionMaster
{
    public class TransactionController : Controller
    {
        private oemahjowodbEntities db = new oemahjowodbEntities();

        // GET: Transaction
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
                List<OemahJowoClass> costDt = db.mt_transaction.ToList().Select(dt => new OemahJowoClass
                {
                    TRANS_ID = dt.TRANS_ID,
                    DESCRIPTION = dt.DESCRIPTION,
                    PROD_NAME = dt.mt_prod?.PROD_NAME,
                    PRICE = dt.PRICE,
                    CUST_NAME = dt.mt_customer?.CUST_NAME,
                    TRANS_DATE = dt.TRANS_DATE,
                    ITEM_AMT = dt.ITEM_AMT,
                    IS_GOJEK = dt.IS_GOJEK,
                    ENTRY_USER = dt.ENTRY_USER,
                    ENTRY_DATE = dt.ENTRY_DATE,
                    UPDATE_DATE = dt.UPDATE_DATE,
                    UPDATE_USER = dt.UPDATE_USER
                }).ToList<OemahJowoClass>();
                return Json(new { data = costDt }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult AddorEdit(string transid)
        {
            var id = Session["id"];
            if (id == null)
            {
                return RedirectToAction("Index", "Login");

            }
            else
            {
                OemahJowoClass formfield = new OemahJowoClass();
                formfield.TRANS_DATE = DateTime.Now;
                var dtProd = db.mt_prod.OrderBy(y => y.PROD_NAME).ToList();
                var dtCust = db.mt_customer.OrderBy(y => y.CUST_NAME).ToList();
                formfield.custDD = dtCust;
                formfield.prodDD = dtProd;
                return View(formfield);
            }
        }
        public ActionResult GetProdPrice()
        {
            var id = Session["id"];
            if (id == null)
            {
                return RedirectToAction("Index", "Login");

            }
            else
            {
                double price = 0;
                var idz = Request.QueryString["prodid"];
                if (!string.IsNullOrEmpty(idz))
                {
                    var ids = Convert.ToInt32(idz);
                    var prod = db.mt_prod.Find(ids);
                    if(prod != null)
                    {
                        price = prod.PRICE;
                    }
                }               
                return Json(new { data = price }, JsonRequestBehavior.AllowGet);
            }
        }
        private bool isgojekreturn(string isgojek)
        {
            bool stats = false;
            if(isgojek == "Y")
            {
                stats = true;
            }
            return stats;
        }
        public ActionResult Edit(string transid)
        {
            var userobject = Session["id"];
            if (userobject == null)
            {
                return RedirectToAction("Index", "Login");

            }
            else
            {
                transid = Request.QueryString["transid"];
                int ids = Convert.ToInt32(transid);
                using (db)
                {
                    var dtProd = db.mt_prod.OrderBy(y => y.PROD_NAME).ToList();
                    var dtCust = db.mt_customer.OrderBy(y => y.CUST_NAME).ToList();
                    var formfield = db.mt_transaction.Where(x => x.TRANS_ID == ids).ToList().Select(dt => new OemahJowoClass { 
                        TRANS_ID = dt.TRANS_ID, 
                        DESCRIPTION = dt.DESCRIPTION,
                        TRANS_DATE = dt.TRANS_DATE,
                        PROD_ID = dt.PROD_ID,
                        CUST_ID = dt.CUST_ID,
                        PRICE = dt.PRICE,
                        ITEM_AMT = dt.ITEM_AMT,
                        IS_GOJEK = dt.IS_GOJEK,
                        custDD = dtCust,
                        prodDD = dtProd,
                        IS_ONLINE = isgojekreturn(dt.IS_GOJEK)
                    }).FirstOrDefault();
                    return View(formfield);
                }
            }
        }

        [HttpPost]
        public ActionResult Insert(OemahJowoClass transDt)
        {
            var userobject = Session["id"];
            if (userobject == null)
            {
                return RedirectToAction("Index", "Login");

            }
            else
            {
                transDt.TRANS_DATE = Convert.ToDateTime(transDt.TRANS_DATE.ToString("MM/dd/yyyy"));
                var editmode = Request.QueryString["editmode"];
                var username = User.Identity.Name;
                transDt.ENTRY_DATE = DateTime.Now;
                transDt.ENTRY_USER = userobject.ToString();
                transDt.UPDATE_DATE = DateTime.Now;
                transDt.UPDATE_USER = userobject.ToString();
                var dtname = username;

                using (db)
                {
                    try
                    {
                        mt_transaction transfield = new mt_transaction();
                        transfield.TRANS_DATE = transDt.TRANS_DATE;
                        transfield.PROD_ID = transDt.PROD_ID;
                        transfield.DESCRIPTION = transDt.DESCRIPTION;
                        transfield.CUST_ID = transDt.CUST_ID;
                        transfield.ITEM_AMT = transDt.ITEM_AMT;
                        transfield.PRICE = transDt.PRICE;
                        if (transDt.IS_ONLINE)
                        {
                            transfield.IS_GOJEK = "Y";
                        }
                        else
                        {
                            transfield.IS_GOJEK = "N";
                        }
                        transfield.UPDATE_DATE = transDt.UPDATE_DATE;
                        transfield.UPDATE_USER = transDt.UPDATE_USER;
                        transfield.ENTRY_DATE = transDt.ENTRY_DATE;
                        transfield.ENTRY_USER = transDt.ENTRY_USER;
                        db.mt_transaction.Add(transfield);
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
        public ActionResult Update(OemahJowoClass transDt)
        {
            var userobject = Session["id"];
            if (userobject == null)
            {
                return RedirectToAction("Index", "Login");

            }
            else
            {
                var editmode = Request.QueryString["editmode"];
                transDt.TRANS_DATE = Convert.ToDateTime(transDt.TRANS_DATE.ToString("MM/dd/yyyy"));
                var username = User.Identity.Name;
                transDt.UPDATE_DATE = DateTime.Now;
                transDt.UPDATE_USER = userobject.ToString();
                var dtname = username;
                using (db)
                {
                    var id = transDt.TRANS_ID;
                    var transfield = db.mt_transaction.Find(id);
                    transfield.TRANS_DATE = transDt.TRANS_DATE;
                    transfield.PROD_ID = transDt.PROD_ID;
                    transfield.DESCRIPTION = transDt.DESCRIPTION;
                    transfield.CUST_ID = transDt.CUST_ID;
                    transfield.ITEM_AMT = transDt.ITEM_AMT;
                    transfield.PRICE = transDt.PRICE;
                    if (transDt.IS_ONLINE)
                    {
                        transfield.IS_GOJEK = "Y";
                    }
                    else
                    {
                        transfield.IS_GOJEK = "N";
                    }
                    transfield.UPDATE_DATE = transDt.UPDATE_DATE;
                    transfield.UPDATE_USER = transDt.UPDATE_USER;
                
                    try
                    {
                        db.Entry(transfield).State = EntityState.Modified;
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
        public ActionResult Delete(string transid)
        {
            var userobject = Session["id"];
            if (userobject == null)
            {
                return RedirectToAction("Index", "Login");

            }
            else
            {
                transid = Request.QueryString["transid"];
                int ids = Convert.ToInt32(transid);
                using (db)
                {
                    mt_transaction transdt = db.mt_transaction.Where(x => x.TRANS_ID == ids).FirstOrDefault<mt_transaction>();
                    try
                    {
                        db.mt_transaction.Remove(transdt);
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