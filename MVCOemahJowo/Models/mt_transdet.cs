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
    
    public partial class mt_transdet
    {
        public int TRANS_ID { get; set; }
        public int SHOPPING_ID { get; set; }
        public double AMOUNT { get; set; }
        public string ISDEBIT { get; set; }
        public System.DateTime ENTRY_DATE { get; set; }
        public string ENTRY_USER { get; set; }
        public System.DateTime UPDATE_DATE { get; set; }
        public string UPDATE_USER { get; set; }
        public int ID { get; set; }
        public int ITEM_AMT { get; set; }
        public string IS_GOJEK { get; set; }
    }
}
