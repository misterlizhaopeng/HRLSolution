﻿//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace HRL.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class HRLEntities : DbContext
    {
        public HRLEntities()
            : base("name=HRLEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Base_DeptInfo> Base_DeptInfo { get; set; }
        public DbSet<Base_MenuInfo> Base_MenuInfo { get; set; }
        public DbSet<Base_RoleMenuInfo> Base_RoleMenuInfo { get; set; }
        public DbSet<Base_RolesInfo> Base_RolesInfo { get; set; }
        public DbSet<Base_UserInfo> Base_UserInfo { get; set; }
        public DbSet<Base_UserMenuInfo> Base_UserMenuInfo { get; set; }
        public DbSet<Base_UserRoleInfo> Base_UserRoleInfo { get; set; }
        public DbSet<Sys_MultiLangInfo> Sys_MultiLangInfo { get; set; }
        public DbSet<Sys_Provinces> Sys_Provinces { get; set; }
        public DbSet<Bus_TakeErrorAttribute> Bus_TakeErrorAttribute { get; set; }
        public DbSet<Sys_LanguageType> Sys_LanguageType { get; set; }
        public DbSet<Bus_TakeError> Bus_TakeError { get; set; }
        public DbSet<Bus_EffComplExeRes> Bus_EffComplExeRes { get; set; }
        public DbSet<Bus_EffciencyComplain> Bus_EffciencyComplain { get; set; }
    }
}
