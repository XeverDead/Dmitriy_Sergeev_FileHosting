using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using System.Text;

namespace Web
{
    public class RoleAuthorizeAttribute : AuthorizeAttribute
    {
        public RoleAuthorizeAttribute(params Roles[] roles)
        {
            var rolesBuilder = new StringBuilder();

            foreach (var role in roles)
            {
                rolesBuilder.Append(role);
                rolesBuilder.Append(",");
            }

            rolesBuilder.Remove(rolesBuilder.Length - 1, 1);

            Roles = rolesBuilder.ToString();
        }
    }
}
