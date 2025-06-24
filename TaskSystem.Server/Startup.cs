using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Jwt;
using Owin;
using System.Configuration;
using System.Text;
using System.Web.Http;
using TaskSystem.Server;

[assembly: OwinStartup(typeof(TodoApp.Server.Startup))]

namespace TodoApp.Server
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();

            var secret = ConfigurationManager.AppSettings["JwtSecretKey"];
            var key = Encoding.UTF8.GetBytes(secret);

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
            {
                AuthenticationMode = AuthenticationMode.Active,
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true
                }
            });

            // Reutiliza tu configuración WebApi existente
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
        }
    }
}