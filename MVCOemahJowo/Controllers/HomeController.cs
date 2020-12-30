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
                return View();

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
            foreach(var dt in prodcatdt)
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
                        foreach(var transdt in field)
                        {
                            countprod = countprod + transdt.ITEM_AMT;
                        }
                    }
                    fld.total_sold = countprod;
                    dashboardprod.Add(fld);
                }
              
            }
            return Json(dashboardprod.OrderByDescending(y => y.total_sold).Take(5).ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCustDashboard()
        {
            List<dashboardcust> dashboardcust = new List<dashboardcust>();
            var custcatdt = db.mt_customer.ToList();
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
                    foreach (var transdt in field)
                    {
                        countpurchase = countpurchase + transdt.PRICE;
                    }
                }
                fld.total_purchase = countpurchase;
                dashboardcust.Add(fld);

            }
            return Json(dashboardcust.OrderByDescending(y => y.total_purchase).Take(5).ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCostDashboard()
        {
            List<dashboardcost> dashboardcosts = new List<dashboardcost>();
            var shopcatdt = db.mg_shoplist.ToList();
            foreach (var dt in shopcatdt)
            {
                if(dt.TRANS_DATE > DateTime.Now.AddMonths(-4))
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
            //dashboardcosts = dashboardcosts.OrderBy(y => y.Monthyear).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProfitLoss()
        {
            List<dashboardcost> dashboardcosts = new List<dashboardcost>();
            var shopcatdt = db.mg_shoplist.ToList();
            foreach (var dt in shopcatdt)
            {
                if (dt.TRANS_DATE > DateTime.Now.AddMonths(-4))
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
            //dashboardcosts = dashboardcosts.OrderBy(y => y.Monthyear).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
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