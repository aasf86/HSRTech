using HSRTech.Business.Dtos;
using HSRTech.Business.Dtos.TipoEncadernacao;

namespace HSRTech.Business.Contracts.UseCases.TipoEncadernacao
{
    public interface ITipoEncadernacaoUseCase : IValidators
    {
        Task<ResponseBase<int>> Insert(RequestBase<TipoEncadernacaoInsert> request);
        Task<ResponseBase<TipoEncadernacaoGet>> GetByCodigo(RequestBase<int> request);
        Task<ResponseBase<bool>> Update(RequestBase<TipoEncadernacaoUpdate> request);
        Task<ResponseBase<bool>> Delete(RequestBase<int> request);
    }
}
