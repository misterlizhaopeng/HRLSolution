﻿//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------
 
using HRL.Model;
using HRL.IDAL;

namespace HRL.BLL
{ 	 
	public partial class Base_DeptInfoService : BaseService<Base_DeptInfo>
	{ 
		public override void SetCurrentRepository()
        { 
            this.CurrentRepository = DbSession.Base_DeptInfoRepository;
        }
	}
	 
	public partial class Base_MenuInfoService : BaseService<Base_MenuInfo>
	{ 
		public override void SetCurrentRepository()
        { 
            this.CurrentRepository = DbSession.Base_MenuInfoRepository;
        }
	}
	 
	public partial class Base_RoleMenuInfoService : BaseService<Base_RoleMenuInfo>
	{ 
		public override void SetCurrentRepository()
        { 
            this.CurrentRepository = DbSession.Base_RoleMenuInfoRepository;
        }
	}
	 
	public partial class Base_RolesInfoService : BaseService<Base_RolesInfo>
	{ 
		public override void SetCurrentRepository()
        { 
            this.CurrentRepository = DbSession.Base_RolesInfoRepository;
        }
	}
	 
	public partial class Base_UserInfoService : BaseService<Base_UserInfo>
	{ 
		public override void SetCurrentRepository()
        { 
            this.CurrentRepository = DbSession.Base_UserInfoRepository;
        }
	}
	 
	public partial class Base_UserMenuInfoService : BaseService<Base_UserMenuInfo>
	{ 
		public override void SetCurrentRepository()
        { 
            this.CurrentRepository = DbSession.Base_UserMenuInfoRepository;
        }
	}
	 
	public partial class Base_UserRoleInfoService : BaseService<Base_UserRoleInfo>
	{ 
		public override void SetCurrentRepository()
        { 
            this.CurrentRepository = DbSession.Base_UserRoleInfoRepository;
        }
	}
	 
	public partial class Bus_EffciencyComplainService : BaseService<Bus_EffciencyComplain>
	{ 
		public override void SetCurrentRepository()
        { 
            this.CurrentRepository = DbSession.Bus_EffciencyComplainRepository;
        }
	}
	 
	public partial class Bus_EffComplExeResService : BaseService<Bus_EffComplExeRes>
	{ 
		public override void SetCurrentRepository()
        { 
            this.CurrentRepository = DbSession.Bus_EffComplExeResRepository;
        }
	}
	 
	public partial class Bus_TakeErrorService : BaseService<Bus_TakeError>
	{ 
		public override void SetCurrentRepository()
        { 
            this.CurrentRepository = DbSession.Bus_TakeErrorRepository;
        }
	}
	 
	public partial class Bus_TakeErrorAttributeService : BaseService<Bus_TakeErrorAttribute>
	{ 
		public override void SetCurrentRepository()
        { 
            this.CurrentRepository = DbSession.Bus_TakeErrorAttributeRepository;
        }
	}
	 
	public partial class Sys_LanguageTypeService : BaseService<Sys_LanguageType>
	{ 
		public override void SetCurrentRepository()
        { 
            this.CurrentRepository = DbSession.Sys_LanguageTypeRepository;
        }
	}
	 
	public partial class Sys_MultiLangInfoService : BaseService<Sys_MultiLangInfo>
	{ 
		public override void SetCurrentRepository()
        { 
            this.CurrentRepository = DbSession.Sys_MultiLangInfoRepository;
        }
	}
	 
	public partial class Sys_ProvincesService : BaseService<Sys_Provinces>
	{ 
		public override void SetCurrentRepository()
        { 
            this.CurrentRepository = DbSession.Sys_ProvincesRepository;
        }
	}
}