using MVCOemahJowo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc;
using MVCOemahJowo.Models;
using System.Data.Entity;
using System.Data;
namespace MVCOemahJowo.Controllers.Reporting
{
    public class CustomerRptController : Controller
    {
        private oemahjowodbEntities db = new oemahjowodbEntities();
        // GET: CustomerRpt
        public ActionResult Index()
        {
            OemahJowoClass fld = new OemahJowoClass();
            List<custcat> catfld = new List<custcat>();
            custcat fld1 = new custcat();
            fld1.catid = "ALL";
            fld1.catname = "ALL";
            catfld.Add(fld1);
            custcat fld2 = new custcat();
            fld2.catid = "GOJEK";
            fld2.catname = "GOJEK";
            catfld.Add(fld2);

            custcat fld3 = new custcat();
            fld3.catid = "GRAB";
            fld3.catname = "GRAB";
            catfld.Add(fld3);
            fld.custcatdt = catfld;
            return View(fld);
        }
        public ActionResult CustRpt()
        {
            string category = "";
            var catselect = Request.QueryString["category"];
            if (!string.IsNullOrEmpty(catselect))
            {
                if (catselect == "GOJEK")
                {
                    category = "GF";
                }
                else
                if (catselect == "GRAB")
                {
                    category = "GRB";
                }
            }
            else
            {
                category = "";
            }
            OemahJowoClass field = new OemahJowoClass();
            List<mt_customer> custDt = new List<mt_customer>();
            var dt = db.mt_customer.OrderBy(y => y.CUST_NAME).ToList();
            if (!string.IsNullOrEmpty(category))
            {
                foreach (var dts in dt)
                {
                    var transdt = dts.mt_trans_hdr.ToList();
                    var forms = transdt.Where(str => category.All(str.DESCRIPTION.Contains)).ToList();
                    if(forms.Count()> 0)
                    {
                        custDt.Add(dts);
                    }
                }

            }
            else
            {
                custDt = dt;
            }
            field.custDD = custDt;
            return PartialView(field);
        }
        public ActionResult PrintPdfCust(OemahJowoClass obj)
        {
            var filter = obj.catfilter;
            return new Rotativa.MVC.ActionAsPdf("CustRptPdf", new { filters = filter }) { FileName = "CustomerList.pdf", RotativaOptions = new Rotativa.Core.DriverOptions { PageOrientation = Rotativa.Core.Options.Orientation.Portrait } };
        }
        public ActionResult CustRptPdf(string filters)
        {
            string category = "";
            var catselect = Request.QueryString["category"];
            if (!string.IsNullOrEmpty(catselect))
            {
                if (catselect == "GOJEK")
                {
                    category = "GF";
                }
                else
                if (catselect == "GRAB")
                {
                    category = "GRB";
                }
            }
            else
            {
                category = "";
            }
            OemahJowoClass field = new OemahJowoClass();
            List<mt_customer> custDt = new List<mt_customer>();
            var dt = db.mt_customer.OrderBy(y => y.CUST_NAME).ToList();
            if (!string.IsNullOrEmpty(category))
            {
                foreach (var dts in dt)
                {
                    var transdt = dts.mt_trans_hdr.ToList();
                    var forms = transdt.Where(str => category.All(str.DESCRIPTION.Contains)).ToList();
                    if (forms.Count() > 0)
                    {
                        custDt.Add(dts);
                    }
                }

            }
            else
            {
                custDt = dt;
            }
            field.custDD = custDt;
            return View(field);
        }
    }
}