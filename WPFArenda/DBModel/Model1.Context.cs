﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class WPF6Entities2 : DbContext
    {
        public WPF6Entities2()
            : base("name=WPF6Entities2")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Contract> Contract { get; set; }
        public virtual DbSet<MethodPayment> MethodPayment { get; set; }
        public virtual DbSet<Object> Object { get; set; }
        public virtual DbSet<Payments> Payments { get; set; }
        public virtual DbSet<PaymentStatus> PaymentStatus { get; set; }
        public virtual DbSet<Reviews> Reviews { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Status_Zayavka> Status_Zayavka { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Zayavka> Zayavka { get; set; }
    }
}
