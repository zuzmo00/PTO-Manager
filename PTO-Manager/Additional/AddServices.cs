using PTO_Manager.Services;
using SzabadsagKezeloWebApp.Services;

namespace PTO_Manager.Additional
{
    public static class AddServices
    {
        public static void AddServicess(this IServiceCollection Services)
        {
            Services.AddEndpointsApiExplorer();
            Services.AddHttpClient();
            Services.AddScoped<IUserServices, UserServices>();
            Services.AddScoped<ISpecialDaysService, SpecialDaysService>();
            Services.AddScoped<IDepartmentService, DepartmentService>();
            Services.AddScoped<IAdminService, AdminService>();
            Services.AddScoped<IRequestService, RequestService>();
            Services.AddScoped<IAktualisFelhasznaloService, AktualisFelhasznaloService>();
            Services.AddHttpContextAccessor();
        }
    }
}
