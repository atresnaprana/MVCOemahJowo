using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCOemahJowo.Models;
using System.Data.Entity;
using System.Data;
using System.Web.Script.Serialization;
using DocumentFormat.OpenXml.ExtendedProperties;
using System.Runtime.CompilerServices;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using OfficeOpenXml.FormulaParsing.ExpressionGraph.FunctionCompilers;
using MVCOemahJowo.Views.Report;
using Microsoft.Win32.SafeHandles;
namespace MVCOemahJowo.Controllers.Reporting
{
    public class ReportController : Controller
    {
        private oemahjowodbEntities db = new oemahjowodbEntities();

        // GET: Report
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PrintPdfTrans(OemahJowoClass obj)
        {
            var from = obj.datefromstr;
            var to = obj.datetostr;
            return new Rotativa.MVC.ActionAsPdf("TransRptPdf", new { froms = from, tos = to }) { FileName = "TransactionRpt.pdf", RotativaOptions = new Rotativa.Core.DriverOptions { PageOrientation = Rotativa.Core.Options.Orientation.Landscape } };
        }
        public ActionResult TransRptPdf(string froms, string tos)
        {
            OemahJowoClass form = new OemahJowoClass();
            List<TransRptField> transDt = new List<TransRptField>();
            var datefrom = froms;
            var dateto = tos;
            var date = DateTime.Now;
            DateTime from = new DateTime(date.Year, date.Month, 1);
            DateTime to = from.AddMonths(1).AddDays(-1);
            if (!string.IsNullOrEmpty(datefrom) && !string.IsNullOrEmpty(dateto))
            {
                from = Convert.ToDateTime(datefrom);
                to = Convert.ToDateTime(dateto);
            }
            var datediff = to - from;
            var shopcatdt = db.mg_shoplist.Where(y => y.TRANS_DATE >= from && y.TRANS_DATE <= to).ToList();

            var incomedt = db.mt_trans_hdr.Where(y => y.TRANS_DATE >= from && y.TRANS_DATE <= to).ToList();
            for (int i = 0; i <= datediff.Days; i++)
            {
                var datenows = from.AddDays(i);
                var shopcatfield = shopcatdt.Where(y => y.TRANS_DATE == datenows).ToList();
                var incomefield = incomedt.Where(y => y.TRANS_DATE == datenows).ToList();
                foreach (var dt in incomefield)
                {
                    var price = dt.mt_trans_dtl.Sum(y => y.PRICE);
                    TransRptField field = new TransRptField();
                    field.TRANS_DATE = datenows;
                    field.DESCRIPTION = dt.DESCRIPTION;
                    field.debit = price;
                    field.CUST_NAME = dt.mt_customer?.CUST_NAME;
                    field.phone = dt.mt_customer?.PHONE;
                    transDt.Add(field);
                }

                foreach (var dt in shopcatfield)
                {
                    TransRptField field = new TransRptField();
                    field.TRANS_DATE = datenows;
                    field.DESCRIPTION = dt.NOTES;
                    field.credit = dt.AMOUNT;
                    field.CUST_NAME = null;
                    field.phone = null;
                    transDt.Add(field);
                }

            }
            transDt = transDt.OrderBy(y => y.TRANS_DATE).ToList();

            form.transrptdt = transDt;
            //dashboardcosts = dashboardcosts.OrderBy(y => y.Monthyear).ToList();
            return View(form);
        }

        public ActionResult TransRpt()
        {
            OemahJowoClass form = new OemahJowoClass();
            List<TransRptField> transDt = new List<TransRptField>();
            var datefrom = Request.QueryString["dateFrom"];
            var dateto = Request.QueryString["dateTo"];
            var date = DateTime.Now;
            DateTime from = new DateTime(date.Year, date.Month, 1);
            DateTime to = from.AddMonths(1).AddDays(-1);
            if(!string.IsNullOrEmpty(datefrom) && !string.IsNullOrEmpty(dateto))
            {
                from = Convert.ToDateTime(datefrom);
                to = Convert.ToDateTime(dateto);
            }
            var datediff = to - from;
            var shopcatdt = db.mg_shoplist.Where(y => y.TRANS_DATE >= from && y.TRANS_DATE <= to).ToList();
         
            var incomedt = db.mt_trans_hdr.Where(y => y.TRANS_DATE >= from && y.TRANS_DATE <= to).ToList();
            for (int i = 0; i <= datediff.Days; i++)
            {
                var datenows = from.AddDays(i);
                var shopcatfield = shopcatdt.Where(y => y.TRANS_DATE == datenows).ToList();
                var incomefield = incomedt.Where(y => y.TRANS_DATE == datenows).ToList();
                foreach(var dt in incomefield)
                {
                    var price = dt.mt_trans_dtl.Sum(y => y.PRICE);
                    TransRptField field = new TransRptField();
                    field.TRANS_DATE = datenows;
                    field.DESCRIPTION = dt.DESCRIPTION;
                    field.debit = price;
                    field.CUST_NAME = dt.mt_customer?.CUST_NAME;
                    field.phone = dt.mt_customer?.PHONE;
                    transDt.Add(field);
                }

                foreach (var dt in shopcatfield)
                {
                    TransRptField field = new TransRptField();
                    field.TRANS_DATE = datenows;
                    field.DESCRIPTION = dt.NOTES;
                    field.credit = dt.AMOUNT;
                    field.CUST_NAME = null;
                    field.phone = null;
                    transDt.Add(field);
                }

            }
            transDt = transDt.OrderBy(y => y.TRANS_DATE).ToList();

            form.transrptdt = transDt;
            //dashboardcosts = dashboardcosts.OrderBy(y => y.Monthyear).ToList();
            return PartialView(form);
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
    }
}