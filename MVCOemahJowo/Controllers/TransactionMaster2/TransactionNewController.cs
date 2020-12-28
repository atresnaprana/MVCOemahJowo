using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCOemahJowo.Models;
using System.Data.Entity;
using System.Data;
namespace MVCOemahJowo.Controllers.TransactionMaster2
{
    public class TransactionNewController : Controller
    {
        private oemahjowodbEntities db = new oemahjowodbEntities();

        // GET: TransactionNew
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
            List<OemahJowoClass> costDt = db.mt_trans_hdr.ToList().Select(dt => new OemahJowoClass
            {
                TRANS_ID = dt.TRANS_ID,
                DESCRIPTION = dt.DESCRIPTION,
                CUST_NAME = dt.mt_customer?.CUST_NAME,
                TRANS_DATE = dt.TRANS_DATE,
                ENTRY_USER = dt.ENTRY_USER,
                ENTRY_DATE = dt.ENTRY_DATE,
                UPDATE_DATE = dt.UPDATE_DATE,
                UPDATE_USER = dt.UPDATE_USER
            }).ToList<OemahJowoClass>();
            return Json(new { data = costDt }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddForm()
        {
            var id = Session["id"];

            if (id == null)
            {
                return RedirectToAction("Index", "Login");

            }
            else
            {
                OemahJowoClass formfield = new OemahJowoClass();
                Session["FormDtls"] = new List<mt_trans_dtl>();
                formfield.TRANS_DATE = DateTime.Now;
                var dtCust = db.mt_customer.OrderBy(y => y.CUST_NAME).ToList();
                formfield.custDD = dtCust;
                return View(formfield);

            }
        }
        public ActionResult TransactionFormPartial(string command)
        {
            OemahJowoClass Hdr = new OemahJowoClass();
            List<mt_trans_dtl> p = Session["FormDtls"] as List<mt_trans_dtl>;
            string[] results = command.Split(';');
            string action = results[0];
            int index = Convert.ToInt32(results[1]);

            if (action == "add")
            {
                p.Add(new mt_trans_dtl());
            }
            else
            {
                p.RemoveAt(index);
                //var idDelete = Request.QueryString["deleteid"];
                //if (!string.IsNullOrEmpty(idDelete))
                //{
                //    var id = Convert.ToDecimal(idDelete);
                //    if (id != 0)
                //    {
                //        List<mt_trans_dtl> dtForm = Session["FormDtls"] as List<mt_trans_dtl>;
                //        var removeField = dtForm.Where(y => y.DTL_ID == id).FirstOrDefault();
                       
                //        dtForm.Remove(removeField);

                //    }
                //}
            }
            Hdr.transDtl = p;
            var dtProd = db.mt_prod.OrderBy(y => y.PROD_NAME).ToList();
            Hdr.prodDD = dtProd;
            return PartialView(Hdr);
        }
    }
}