using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCOemahJowo.Models;
using System.Data.Entity;
using System.Data;
using System.Web.Script.Serialization;
namespace MVCOemahJowo.Controllers
{
    public class HomeController : Controller
    {
        private oemahjowodbEntities db = new oemahjowodbEntities();

        public ActionResult Index()
        {
            ViewBag.Loggeduser = Session["id"];
            if(Session["id"] != null)
            {
                OemahJowoClass dropdownDt = new OemahJowoClass();
                List<homeCls> dt = new List<homeCls>();
                homeCls val1 = new homeCls();
                val1.total = 5;
                val1.totalstr = "5";
                dt.Add(val1);
                homeCls val2 = new homeCls();
                val2.total = 10;
                val2.totalstr = "10";
                dt.Add(val2);
                homeCls val3 = new homeCls();
                val3.total = 15;
                val3.totalstr = "15";
                dt.Add(val3);
                homeCls val4 = new homeCls();
                val4.total = 20;
                val4.totalstr = "20";
                dt.Add(val4);
                dropdownDt.ddValDt = dt;
                return View(dropdownDt);

            }
            else
            {
                return RedirectToAction("Index", "Login");

            }
        }
        public JsonResult GetProdDashboard()
        {
            List<dashboardprod> dashboardprod = new List<dashboardprod>();
            var prodcatdt = db.mt_prod_cat.ToList();
            var yearstr = Request.QueryString["year"];
            var monthstr = Request.QueryString["month"];
            var totaldata = Request.QueryString["total"];
            int totals = 5;
            if (!string.IsNullOrEmpty(totaldata))
            {
                totals = Convert.ToInt32(totaldata);
            }
            foreach (var dt in prodcatdt)
            {
                if(dt.id != 9)
                {
                    dashboardprod fld = new dashboardprod();
                    var catname = dt.category_name;
                    fld.prodcat_id = dt.id;
                    fld.prodcat_name = catname;
                    var prodcatlist = dt.mt_prod.ToList();
                    int countprod = 0;
                    foreach (var prodlist in prodcatlist)
                    {
                        var field = prodlist.mt_trans_dtl.ToList();
                        if (!string.IsNullOrEmpty(yearstr) && !string.IsNullOrEmpty(monthstr))
                        {
                            int year = Convert.ToInt32(yearstr);
                            int month = Convert.ToInt32(monthstr);
                            field = field.Where(y => y.mt_trans_hdr.TRANS_DATE.Year == year && y.mt_trans_hdr.TRANS_DATE.Month == month).ToList();
                        }
                        foreach (var transdt in field)
                        {
                            countprod = countprod + transdt.ITEM_AMT;
                        }
                    }
                    fld.total_sold = countprod;
                    dashboardprod.Add(fld);
                }
              
            }
            return Json(dashboardprod.OrderByDescending(y => y.total_sold).Take(totals).ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCustDashboard()
        {
            List<dashboardcust> dashboardcust = new List<dashboardcust>();
            var custcatdt = db.mt_customer.ToList();
            var yearstr = Request.QueryString["year"];
            var monthstr = Request.QueryString["month"];
            var totaldata = Request.QueryString["total"];
            int totals = 5;
            if (!string.IsNullOrEmpty(totaldata))
            {
                totals = Convert.ToInt32(totaldata);
            }
            foreach (var dt in custcatdt)
            {
                dashboardcust fld = new dashboardcust();
                var custname = dt.CUST_NAME;
                fld.cust_id = dt.CUST_ID;
                fld.cust_name = custname;
                var translist = dt.mt_trans_hdr.ToList();
                double countpurchase = 0;
                foreach (var trans in translist)
                {
                    var field = trans.mt_trans_dtl.ToList();
                    if (!string.IsNullOrEmpty(yearstr) && !string.IsNullOrEmpty(monthstr))
                    {
                        int year = Convert.ToInt32(yearstr);
                        int month = Convert.ToInt32(monthstr);
                        field = field.Where(y => y.mt_trans_hdr.TRANS_DATE.Year == year && y.mt_trans_hdr.TRANS_DATE.Month == month).ToList();
                    }
                    foreach (var transdt in field)
                    {
                        countpurchase = countpurchase + transdt.PRICE;
                    }
                }
                fld.total_purchase = countpurchase;
                dashboardcust.Add(fld);

            }
            return Json(dashboardcust.OrderByDescending(y => y.total_purchase).Take(totals).ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCostDashboard()
        {
            List<dashboardcost> dashboardcosts = new List<dashboardcost>();
            var shopcatdt = db.mg_shoplist.ToList();
            var yearstr = Request.QueryString["year"];
            var monthstr = Request.QueryString["month"];
            var totaldata = Request.QueryString["total"];
            int totals = 5;
            if (!string.IsNullOrEmpty(totaldata))
            {
                totals = Convert.ToInt32(totaldata);
            }
            foreach (var dt in shopcatdt)
            {
                var datetimenow = DateTime.Now;
                if (!string.IsNullOrEmpty(yearstr) && !string.IsNullOrEmpty(monthstr))
                {
                    int year = Convert.ToInt32(yearstr);
                    int month = Convert.ToInt32(monthstr);
                    datetimenow = new DateTime(year, month, 1);
                   
                }
                //else
                //{
                //    if (dt.TRANS_DATE > datetimenow.AddMonths(-4))
                //    {
                //        dashboardcost fld = new dashboardcost();
                //        var month = dt.TRANS_DATE.ToString("MM-yyyy");
                //        fld.Monthyear = month;
                //        double countpurchase = 0;
                //        countpurchase = countpurchase + dt.AMOUNT;

                //        fld.totalcost = countpurchase;
                //        dashboardcosts.Add(fld);
                //    }
                //}
                if (dt.TRANS_DATE.Month <= datetimenow.Month)
                {
                    dashboardcost fld = new dashboardcost();
                    var months = dt.TRANS_DATE.ToString("MM-yyyy");
                    fld.Monthyear = months;
                    double countpurchase = 0;
                    countpurchase = countpurchase + dt.AMOUNT;

                    fld.totalcost = countpurchase;
                    dashboardcosts.Add(fld);
                }
            }
            List<dashboardcost> result = dashboardcosts.GroupBy(l => l.Monthyear)
            .Select(cl => new dashboardcost
            {
                Monthyear = cl.First().Monthyear,
                totalcost = cl.Sum(c => c.totalcost),
            }).OrderByDescending(y => y.Monthyear).Take(totals).ToList();
            //dashboardcosts = dashboardcosts.OrderBy(y => y.Monthyear).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProfitLoss()
        {
            List<dashboardcost> dashboardcosts = new List<dashboardcost>();
            var shopcatdt = db.mg_shoplist.ToList();
            var yearstr = Request.QueryString["year"];
            var monthstr = Request.QueryString["month"];
            var totaldata = Request.QueryString["total"];
            int totals = 5;
            if (!string.IsNullOrEmpty(totaldata))
            {
                totals = Convert.ToInt32(totaldata);
            }
            foreach (var dt in shopcatdt)
            {
                var datetimenow = DateTime.Now;
                if (!string.IsNullOrEmpty(yearstr) && !string.IsNullOrEmpty(monthstr))
                {
                    int year = Convert.ToInt32(yearstr);
                    int month = Convert.ToInt32(monthstr);
                    datetimenow = new DateTime(year, month, 1);
                }
                if (dt.TRANS_DATE.Month <= datetimenow.Month)
                {
                    dashboardcost fld = new dashboardcost();
                    var month = dt.TRANS_DATE.ToString("MM-yyyy");
                    fld.Monthyear = month;
                    double countpurchase = 0;
                    countpurchase = countpurchase + dt.AMOUNT;
                    fld.totalcost = countpurchase;
                    dashboardcosts.Add(fld);
                }
            }
            List<dashboardcost> result = dashboardcosts.GroupBy(l => l.Monthyear)
            .Select(cl => new dashboardcost
            {
                Monthyear = cl.First().Monthyear,
                totalcost = cl.Sum(c => c.totalcost),
            }).OrderBy(y => y.Monthyear).ToList();
            List<dashboardincome> dashboardincomes = new List<dashboardincome>();
            var incomedt = db.mt_trans_dtl.ToList();
            foreach (var dt in incomedt)
            {
                var datetimenow = DateTime.Now;
                if (!string.IsNullOrEmpty(yearstr) && !string.IsNullOrEmpty(monthstr))
                {
                    int year = Convert.ToInt32(yearstr);
                    int month = Convert.ToInt32(monthstr);
                    datetimenow = new DateTime(year, month, 1);
                }
                if (dt.mt_trans_hdr.TRANS_DATE.Month <= datetimenow.Month)
                {
                    dashboardincome fld = new dashboardincome();
                    var month = dt.mt_trans_hdr.TRANS_DATE.ToString("MM-yyyy");
                    fld.Monthyear = month;
                    double countpurchase = 0;
                    countpurchase = countpurchase + dt.PRICE;
                    fld.totalincome = countpurchase;
                    dashboardincomes.Add(fld);
                }
            }
            List<dashboardincome> resultincome = dashboardincomes.GroupBy(l => l.Monthyear)
            .Select(cl => new dashboardincome
            {
                Monthyear = cl.First().Monthyear,
                totalincome = cl.Sum(c => c.totalincome),
            }).OrderByDescending(y => y.Monthyear).ToList();
            foreach(var dt in resultincome)
            {
                var dtcost = result.Where(y => y.Monthyear == dt.Monthyear).FirstOrDefault();
                if(dtcost != null)
                {
                    dt.totalincome = dt.totalincome - dtcost.totalcost;
                }
            }
            //dashboardcosts = dashboardcosts.OrderBy(y => y.Monthyear).ToList();
            return Json(resultincome.Take(totals).ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}