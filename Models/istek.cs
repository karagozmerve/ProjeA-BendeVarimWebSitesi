//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace bendevarimproje.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class istek
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public istek()
        {
            this.bridge = new HashSet<bridge>();
        }
    
        public int id { get; set; }
        public string mail { get; set; }
        public string baslik { get; set; }
        public string aciklama { get; set; }
        public string etiketler { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<bridge> bridge { get; set; }
    }
}
