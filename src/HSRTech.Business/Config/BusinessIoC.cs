using HSRTech.Business.Contracts.UseCases.Livro;
using HSRTech.Business.Contracts.UseCases.Tag;
using HSRTech.Business.Contracts.UseCases.TipoEncadernacao;
using HSRTech.Business.UseCases.Livro;
using HSRTech.Business.UseCases.Tag;
using HSRTech.Business.UseCases.TipoEncadernacao;
using Microsoft.Extensions.DependencyInjection;

namespace HSRTech.Business.Config
{
    public static class BusinessIoC
    {
        public static IServiceCollection AddBusinessIoC(this IServiceCollection services)
        {
            services.AddScoped<ITagUseCase, TagUseCase>();
            services.AddScoped<ILivroUseCase, LivroUseCase>();
            services.AddScoped<ITipoEncadernacaoUseCase, TipoEncadernacaoUseCase>();

            return services;
        }
    }
}
