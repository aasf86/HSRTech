using HSRTech.Business.Dtos;
using HSRTech.Business.Dtos.Tag;

namespace HSRTech.Business.Contracts.UseCases.Tag
{
    public interface ITagUseCase : IValidators
    {
        Task<ResponseBase<int>> Insert(RequestBase<TagInsert> request);
        Task<ResponseBase<TagGet>> GetByCodigo(RequestBase<int> request);
        Task<ResponseBase<bool>> Update(RequestBase<TagUpdate> request);
        Task<ResponseBase<bool>> Delete(RequestBase<int> request);
    }
}
