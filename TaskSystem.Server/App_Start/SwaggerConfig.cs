using System.Web.Http;
using WebActivatorEx;
using TaskSystem.Server;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace TaskSystem.Server
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "TaskSystem.Server");
                })
                .EnableSwaggerUi(); // Interfaz gráfica
        }
    }
}
