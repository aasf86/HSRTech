using System.Reflection;

namespace HSRTech.Api.Config
{
    public static class SwaggerConfig
    {
        public static void AddSwaggerGen_HSRTech(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {                
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
        }        
    }
}