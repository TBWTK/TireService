//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TireService.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Products
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Products()
        {
            this.Baskets = new HashSet<Baskets>();
        }
    
        public int Id { get; set; }
        public int TypeProduct { get; set; }
        public string NameProduct { get; set; }
        public double WeightProduct { get; set; }
        public int QuantityProduct { get; set; }
        public string DescriptionProduct { get; set; }
        public decimal Price { get; set; }
        public byte[] Photo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Baskets> Baskets { get; set; }
        public virtual TypeProducts TypeProducts { get; set; }
    }
}
