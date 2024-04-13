using Microsoft.AspNetCore.Mvc.Rendering;

namespace SchoolManagementApp.MVC.Utilities
{
    public class StaticData
    {
        public const string role_cust = "Customer";
        public const string role_admin = "Admin";

        public static List<SelectListItem> GetRolesForDropDown(bool isAdmin)
        {
            if (isAdmin)
            {
                return new List<SelectListItem>
                {
                    new SelectListItem{Value=role_admin,Text=role_admin}
                };
            }
            else
            {
                return new List<SelectListItem>
                {
                    new SelectListItem{Value=role_cust,Text=role_cust}
                };
            }
        }
    }
}
