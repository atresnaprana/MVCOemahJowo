//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVCOemahJowo.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class mt_trans_dtl
    {
        public int DTL_ID { get; set; }
        public int TRANS_ID { get; set; }
        public int PROD_ID { get; set; }
        public double PRICE { get; set; }
        public int ITEM_AMT { get; set; }
        public string IS_ONLINE { get; set; }
        public double DISCOUNT { get; set; }
        public double DISCOUNT_PERC { get; set; }
        public string IS_DISCOUNT { get; set; }
        public double PRICE_ORI { get; set; }
    
        public virtual mt_prod mt_prod { get; set; }
        public virtual mt_trans_hdr mt_trans_hdr { get; set; }
    }
}
