﻿//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace HRL.IDAL
{ 
	public partial interface IDbSession
	{
		IBase_DeptInfoRepository Base_DeptInfoRepository { get; set; }
		IBase_MenuInfoRepository Base_MenuInfoRepository { get; set; }
		IBase_RoleMenuInfoRepository Base_RoleMenuInfoRepository { get; set; }
		IBase_RolesInfoRepository Base_RolesInfoRepository { get; set; }
		IBase_UserInfoRepository Base_UserInfoRepository { get; set; }
		IBase_UserMenuInfoRepository Base_UserMenuInfoRepository { get; set; }
		IBase_UserRoleInfoRepository Base_UserRoleInfoRepository { get; set; }
		IBus_EffciencyComplainRepository Bus_EffciencyComplainRepository { get; set; }
		IBus_EffComplExeResRepository Bus_EffComplExeResRepository { get; set; }
		IBus_TakeErrorRepository Bus_TakeErrorRepository { get; set; }
		IBus_TakeErrorAttributeRepository Bus_TakeErrorAttributeRepository { get; set; }
		ISys_LanguageTypeRepository Sys_LanguageTypeRepository { get; set; }
		ISys_MultiLangInfoRepository Sys_MultiLangInfoRepository { get; set; }
		ISys_ProvincesRepository Sys_ProvincesRepository { get; set; }
	}
}