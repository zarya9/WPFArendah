//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WPFArenda.DBModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Object
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Object()
        {
            this.Reviews = new HashSet<Reviews>();
            this.Zayavka = new HashSet<Zayavka>();
        }
    
        public int ID_Object { get; set; }
        public Nullable<int> ID_Owner { get; set; }
        public Nullable<int> ID_Category { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Atribut { get; set; }
        public Nullable<int> Price { get; set; }
        public Nullable<bool> Dostupnost { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public byte[] Image { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reviews> Reviews { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Zayavka> Zayavka { get; set; }
    }
}