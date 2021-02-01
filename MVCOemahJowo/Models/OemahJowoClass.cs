using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCOemahJowo.Models;
using System.Data.Entity;
using System.Data;

namespace MVCOemahJowo.Models
{
    public class OemahJowoClass
    {
        public int TRANS_ID { get; set; }
        public string DESCRIPTION { get; set; }
        public int PROD_ID { get; set; }
        public string PROD_NAME { get; set; }
        public string CUST_NAME { get; set; }
        public double debit { get; set; }
        public double credit { get; set; }
        public int phone { get; set; }

        public double PRICE { get; set; }
        public int CUST_ID { get; set; }
        public System.DateTime ENTRY_DATE { get; set; }
        public string ENTRY_USER { get; set; }
        public string TRANS_DATE_STR { get; set; }
        public string datefromstr { get; set; }
        public string datetostr { get; set; }
        public string catfilter { get; set; }

        public System.DateTime UPDATE_DATE { get; set; }
        public string UPDATE_USER { get; set; }
        public System.DateTime TRANS_DATE { get; set; }
        public int ITEM_AMT { get; set; }
        public string IS_GOJEK { get; set; }
        public bool IS_ONLINE { get; set; }
        public List<mt_customer> custDD { get; set; }
        public List<mt_prod> prodDD { get; set; }
        public mt_trans_hdr transhdr { get; set; }
        public List<mt_trans_dtl> transDtl { get; set; }
        public mt_transaction fieldEdit { get; set; }
        public List<TransRptField> transrptdt { get; set; }
        public List<custcat> custcatdt { get; set; }
        public List<mt_prod_cat> prodCatDt { get; set; }
    }
    public class prodClass
    {
        public int PROD_ID { get; set; }
        public string PROD_NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public double PRICE { get; set; }
        public System.DateTime ENTRY_DATE { get; set; }
        public string ENTRY_USER { get; set; }
        public System.DateTime UPDATE_DATE { get; set; }
        public string UPDATE_USER { get; set; }
        public int prod_cat_id { get; set; }
        public List<mt_prod_cat> prodCatDD { get; set; }

    }
    public class mg_shoplist2
    {
        public int ID { get; set; }
        public string NOTES { get; set; }
        public double AMOUNT { get; set; }
        public System.DateTime UPDATE_DATE { get; set; }
        public string TRANS_DATE_STR { get; set; }
        public string UPDATE_USER { get; set; }
        public System.DateTime ENTRY_DATE { get; set; }
        public string ENTRY_USER { get; set; }
        public System.DateTime TRANS_DATE { get; set; }
    }
    public class custcat
    {
        public string catid { get; set; }
        public string catname { get; set;  }
    }
    public class TransRptField
    {
        public System.DateTime TRANS_DATE { get; set; }

        public string DESCRIPTION { get; set; }
        public string CUST_NAME { get; set; }
        public double PRICE { get; set; }
        public string phone { get; set; }

        public double debit { get; set; }
        public double credit { get; set; }
    }
    public class dashboardprod
    {
        public int prodcat_id { get; set; }
        public string prodcat_name { get; set; }
        public int total_sold { get; set; }
    }
    public class dashboardcust
    {
        public int cust_id { get; set; }
        public string cust_name { get; set; }
        public double total_purchase { get; set; }
    }
    public class dashboardcost
    {
        public string Monthyear { get; set; }
        public double totalcost { get; set; }
    }
    public class dashboardincome
    {
        public string Monthyear { get; set; }
        public double totalincome { get; set; }
    }
    public class TransForm
    {
        public mt_transaction form { get; set; }
        public List<mt_customer> custDD { get; set; }
        public List<mt_prod> prodDD { get; set; }
    }
}