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
    
    public partial class mt_prod_cat
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public mt_prod_cat()
        {
            this.mt_prod = new HashSet<mt_prod>();
        }
    
        public int id { get; set; }
        public string category_name { get; set; }
        public int amount { get; set; }
        public System.DateTime entry_date { get; set; }
        public string entry_user { get; set; }
        public System.DateTime update_date { get; set; }
        public string update_user { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<mt_prod> mt_prod { get; set; }
    }
}
