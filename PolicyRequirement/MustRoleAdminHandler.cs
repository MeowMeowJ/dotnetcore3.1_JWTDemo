using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jwt.PolicyRequirement
{
    // 自定义策略授权
    //public class MustRoleAdminHandler: IAuthorizationHandler
    //{
    //    public Task HandleAsync(AuthorizationHandlerContext context)
    //    {
    //        var requirement = context.Requirements.FirstOrDefault();

    //        context.Succeed(requirement);
    //        // context.Fail();

    //        return Task.CompletedTask;
    //    }
    //}

    public class MustRoleAdminHandler : AuthorizationHandler<AdminRequirement>
    {
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            var requirement = context.Requirements.FirstOrDefault();

            context.Succeed(requirement);
            // context.Fail();

            return Task.CompletedTask;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            AdminRequirement requirement)
        {
            context.Succeed(requirement);
            // context.Fail();

            return Task.CompletedTask;
        }
    }
}
