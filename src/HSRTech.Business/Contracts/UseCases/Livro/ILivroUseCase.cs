using HSRTech.Business.Dtos;
using HSRTech.Business.Dtos.Livro;

namespace HSRTech.Business.Contracts.UseCases.Livro
{
    public interface ILivroUseCase : IValidators
    {
        Task<ResponseBase<int>> Insert(RequestBase<LivroInsert> request);
        Task<ResponseBase<LivroGet>> GetByCodigo(RequestBase<int> request);
        Task<ResponseBase<bool>> Update(RequestBase<LivroUpdate> request);
        Task<ResponseBase<bool>> Delete(RequestBase<int> request);
        Task<ResponseBase<List<LivroGet>>> GetAll(RequestBase<LivroGetAll> request);
    }
}
