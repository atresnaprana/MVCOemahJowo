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
    
    public partial class mt_customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public mt_customer()
        {
            this.mt_transaction = new HashSet<mt_transaction>();
        }
    
        public int CUST_ID { get; set; }
        public string CUST_NAME { get; set; }
        public string ADDRESS { get; set; }
        public System.DateTime ENTRY_DATE { get; set; }
        public string ENTRY_USER { get; set; }
        public System.DateTime UPDATE_DATE { get; set; }
        public string UPDATE_USER { get; set; }
        public string PHONE { get; set; }
        public string EMAIL { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<mt_transaction> mt_transaction { get; set; }
    }
}
