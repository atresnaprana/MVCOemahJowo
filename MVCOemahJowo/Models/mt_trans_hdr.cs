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
    
    public partial class mt_trans_hdr
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public mt_trans_hdr()
        {
            this.mt_trans_dtl = new HashSet<mt_trans_dtl>();
        }
    
        public int TRANS_ID { get; set; }
        public int CUST_ID { get; set; }
        public string DESCRIPTION { get; set; }
        public System.DateTime TRANS_DATE { get; set; }
        public System.DateTime ENTRY_DATE { get; set; }
        public string ENTRY_USER { get; set; }
        public System.DateTime UPDATE_DATE { get; set; }
        public string UPDATE_USER { get; set; }
    
        public virtual mt_customer mt_customer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<mt_trans_dtl> mt_trans_dtl { get; set; }
    }
}
