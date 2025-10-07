using PTO_Manager.Additional;
using PTO_Manager.Services;

namespace PTO_Manager
{
    public static class AddServices
    {
        public static void AddServicess(this IServiceCollection Services)
        {
            Services.AddEndpointsApiExplorer();
            Services.AddHttpClient();
            Services.AddScoped<IUserServices, UserServices>();
        }
    }
}
