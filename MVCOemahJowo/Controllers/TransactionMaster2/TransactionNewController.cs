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
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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
        public ActionResult GetLengthTrans()
        {
            var model = Session["FormDtls"] as List<mt_trans_dtl>;
            decimal? number = 0;
            number = model.Count();
            return Json(new { success = true, length = number }, JsonRequestBehavior.DenyGet);
        }
        [HttpPost]
        public ActionResult AddData(string formHdr, string formTrans)
        {
            string err = "ok";
            string username = Session["id"].ToString();
            var js = new JavaScriptSerializer();
            string errmsg = "ok";
            string issubmit = Request.QueryString["isSubmit"];
            mt_trans_hdr[] formHdrDt = js.Deserialize<mt_trans_hdr[]>(formHdr);
            mt_trans_dtl[] formDtlDt = js.Deserialize<mt_trans_dtl[]>(formTrans);
            
            mt_trans_hdr formDt = new mt_trans_hdr();

            //if (string.IsNullOrEmpty(formHdrDt[0].VENDOR_ID))
            //{
            //    errmsg += "*Please fill vendor information \n" + Environment.NewLine;
            //}
            //else
            //{
            //    formDt.VENDOR_ID = formHdrDt[0].VENDOR_ID;
            //}

            //formDt.KINERJA_ID = nextidkontrak();
            formDt.CUST_ID = formHdrDt[0].CUST_ID;
            formDt.DESCRIPTION = formHdrDt[0].DESCRIPTION;
            //var date = formHdrDt[0].TRANS_DATE.ToString("MM/dd/yyyy");
            //var dateconv = Convert.ToDateTime(date);
            formDt.TRANS_DATE = formHdrDt[0].TRANS_DATE;

            formDt.ENTRY_USER = username;
            formDt.ENTRY_DATE = DateTime.Now;

            formDt.UPDATE_DATE = DateTime.Now;
            formDt.UPDATE_USER = username;
            foreach (var dtl in formDtlDt)
            {
                dtl.IS_DISCOUNT = "N";

                var prodDt = db.mt_prod.Find(dtl.PROD_ID);
                if (prodDt != null)
                {
                    dtl.PRICE_ORI = prodDt.PRICE;

                }
               
            }
            formDt.mt_trans_dtl = formDtlDt;
          
            //formDt.datatype = formHdrDt[0].datatype;

            //if (errmsg != "")
            //{
            //    err = errmsg;
            //}
           
            if (errmsg == "ok")
            {
                try
                {
                    db.mt_trans_hdr.Add(formDt);
                    db.SaveChanges();

                }
                catch (Exception ex)
                {
                    errmsg = ex.ToString();
                }
                return Json(errmsg);

            }
            else
            {
                return Json(errmsg);

            }
        }
        public ActionResult EditForm()
        {
            var id = Session["id"];

            if (id == null)
            {
                return RedirectToAction("Index", "Login");

            }
            else
            {
                var formid = Request.QueryString["formid"];
                int ids = 0;
                OemahJowoClass formfield = new OemahJowoClass();

                if (!string.IsNullOrEmpty(formid))
                {
                    ids = Convert.ToInt32(formid);
                    var formhdr = db.mt_trans_hdr.Find(ids);
                    formfield.transhdr = formhdr;
                    if (formhdr != null)
                    {
                        formfield.TRANS_DATE = formhdr.TRANS_DATE;
                        formfield.TRANS_DATE_STR = formhdr.TRANS_DATE.ToString("M/d/yyyy");
                        formfield.DESCRIPTION = formhdr.DESCRIPTION;
                        formfield.CUST_ID = formhdr.CUST_ID;
                        Session["FormDtls"] = formhdr.mt_trans_dtl.ToList();
                    }
                }
               
                var dtCust = db.mt_customer.OrderBy(y => y.CUST_NAME).ToList();
                formfield.custDD = dtCust;
                return View(formfield);

            }
        }
        [HttpPost]
        public ActionResult EditTrans(string formHdr, string formTrans)
        {
            string err = "ok";
            string username = User.Identity.Name;
            var js = new JavaScriptSerializer();
            string errmsg = "ok";
            string issubmit = Request.QueryString["isSubmit"];
            mt_trans_hdr[] formHdrDt = js.Deserialize<mt_trans_hdr[]>(formHdr);
            mt_trans_dtl[] formDtlDt = js.Deserialize<mt_trans_dtl[]>(formTrans);

            var formids = formHdrDt[0].TRANS_ID;

            mt_trans_hdr formDt = db.mt_trans_hdr.Find(formids);

            //var dtlDt = SMSFDb.SMSF_KINERJA_DETAIL.Where(y => y.FORM_ID == formids).ToList();
            //formDt.SMSF_KINERJA_DETAIL = dtlDt;

            //var userDt = SMSFDb.SMSF_KINERJA_EMP.Where(y => y.KINERJA_ID == formids).ToList();
            //formDt.SMSF_KINERJA_EMP = userDt;

            //var pelaksanaDt = SMSFDb.SMSF_KINERJA_PELAKSANA.Where(y => y.KINERJA_ID == formids).ToList();
            //formDt.SMSF_KINERJA_PELAKSANA = pelaksanaDt;

            //var anggotaDt = SMSFDb.SMSF_KINERJA_ANGGOTA.Where(y => y.KINERJA_ID == formids).ToList();
            //formDt.SMSF_KINERJA_ANGGOTA = anggotaDt;


            formDt.CUST_ID = formHdrDt[0].CUST_ID;
            formDt.DESCRIPTION = formHdrDt[0].DESCRIPTION;
         
            formDt.TRANS_DATE = formHdrDt[0].TRANS_DATE;

            formDt.ENTRY_USER = username;
            formDt.ENTRY_DATE = DateTime.Now;

            formDt.UPDATE_DATE = DateTime.Now;
            formDt.UPDATE_USER = username;
          
            //formDt.SMSF_KINERJA_DETAIL = formDtlDt;

            //Processing UserData
            List<int> listtrans = new List<int>();
            List<int> listExistingtrans = new List<int>();

            var idToRemovetrans = new List<int>();
            var idToAddtrans = new List<int>();

            var ExistingTransDt = formDt.mt_trans_dtl.ToList();
            for (int i = 0; i < formDtlDt.Count(); i++)
            {
                var idDtl = formDtlDt[i].DTL_ID;
                listtrans.Add(idDtl);
                idToAddtrans.Add(idDtl);
            }

            foreach (var exist in ExistingTransDt)
            {
                var transExist = exist.DTL_ID;
                listExistingtrans.Add(transExist);
                idToRemovetrans.Add(transExist);

            }

            //removing logic 
            for (int i = 0; i < listExistingtrans.Count(); i++)
            {
                var nopekExist = listExistingtrans[i];
                for (int y = 0; y < listtrans.Count(); y++)
                {
                    var nopekNew = listtrans[y];
                    if (nopekExist == nopekNew)
                    {
                        idToRemovetrans.Remove(nopekExist);
                    }
                }
            }
            var empDt = formDt.mt_trans_dtl.Where(y => idToRemovetrans.Contains(y.DTL_ID)).ToList<mt_trans_dtl>();
            foreach (var dtlemp in empDt)
            {
                var formEmp = dtlemp;
                formDt.mt_trans_dtl.Remove(formEmp);
            }
            //adding logic
            foreach (var dts in formDtlDt)
            {
                var iddts = dts.DTL_ID;
                if (iddts != 0) {
                    var formtrans = db.mt_trans_dtl.Find(iddts);
                    formtrans.PROD_ID = dts.PROD_ID;
                    formtrans.ITEM_AMT = dts.ITEM_AMT;
                    formtrans.IS_ONLINE = dts.IS_ONLINE;
                    formtrans.PRICE = dts.PRICE;
                    formtrans.IS_DISCOUNT = "N";
                    var prodDt = db.mt_prod.Find(dts.PROD_ID);
                    if (prodDt != null)
                    {
                        formtrans.PRICE_ORI = prodDt.PRICE;

                    }
                }
                else
                {
                    mt_trans_dtl formnew = new mt_trans_dtl();
                    formnew.PROD_ID = dts.PROD_ID;
                    formnew.ITEM_AMT = dts.ITEM_AMT;
                    formnew.IS_ONLINE = dts.IS_ONLINE;
                    formnew.PRICE = dts.PRICE;
                    formnew.IS_DISCOUNT = "N";
                    var prodDt = db.mt_prod.Find(dts.PROD_ID);
                    if (prodDt != null)
                    {
                        formnew.PRICE_ORI = prodDt.PRICE;

                    }
                    formnew.TRANS_ID = formids;
                    db.mt_trans_dtl.Add(formnew);

                }
            }
      
            if (errmsg == "ok")
            {
                try
                {
                    //SMSFDb.SMSF_KINERJA_KONTRAKTOR.Add(formDt);
                    db.Entry(formDt).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    errmsg = ex.ToString();
                }
                return Json(errmsg);

            }
            else
            {
                return Json(errmsg);

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
                    mt_trans_hdr transdt = db.mt_trans_hdr.Where(x => x.TRANS_ID == ids).FirstOrDefault<mt_trans_hdr>();
                    try
                    {
                        db.mt_trans_hdr.Remove(transdt);
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