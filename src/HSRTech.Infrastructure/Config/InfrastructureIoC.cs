using HSRTech.Domain.Contracts.Repositories.Livro;
using HSRTech.Domain.Contracts.Repositories.Tag;
using HSRTech.Domain.Contracts.Repositories.TipoEncadernacao;
using HSRTech.Infrastructure.EntitiesModels;
using HSRTech.Infrastructure.Repositories.Livro;
using HSRTech.Infrastructure.Repositories.Tag;
using HSRTech.Infrastructure.Repositories.TipoEncadernacao;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Data.SqlClient;

namespace HSRTech.Infrastructure.Config
{
    public static class InfrastructureIoC
    {
        public static IServiceCollection AddInfrastructureIoC(this IServiceCollection services, IConfigurationManager config)
        {
            services.AddScoped<IDbConnection>(src => new SqlConnection(config.GetConnectionString("Default")));
            services.AddScoped<ITagRepository<TagModel>, TagRepository>();
            services.AddScoped<ILivroRepository<LivroModel>, LivroRepository>();
            services.AddScoped<ITipoEncadernacaoRepository<TipoEncadernacaoModel>, TipoEncadernacaoRepository>();

            return services;
        }
    }
}
