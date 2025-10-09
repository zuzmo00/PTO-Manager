using PTO_Manager.Services;

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
            Services.AddScoped<IRequestService, RequestService>();
        }
    }
}
