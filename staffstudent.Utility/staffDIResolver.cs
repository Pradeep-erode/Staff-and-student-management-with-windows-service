using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using staffstudent.Core.IRepository;
using staffstudent.Core.IService;
using staffstudent.Resources.staffRepository;
using staffstudent.Services.StaffService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace staffstudent.Utility
{
    public class staffDIResolver
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            //for accessing the http context by interface in view level
            #region Http context
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            #endregion
            //for service accesssing
            #region Service

            services.AddScoped<IServiceStaff, StaffService>();
            #endregion
            //for database accessing 
            #region Repository

            services.AddScoped<IRepositoryStaff, StaffRepository>();

            #endregion


        }
    }
}
