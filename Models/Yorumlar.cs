//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace YemekTarifleri.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Yorumlar
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public System.DateTime OTarihi { get; set; }
        public int TarifId { get; set; }
        public string UserId { get; set; }
    
        public virtual Tarifler Tarifler { get; set; }
    }
}