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
    
    public partial class bridgeb
    {
        public int id { get; set; }
        public int etiketid { get; set; }
        public int bagisid { get; set; }
    
        public virtual bagis bagis { get; set; }
        public virtual etiket etiket { get; set; }
    }
}
