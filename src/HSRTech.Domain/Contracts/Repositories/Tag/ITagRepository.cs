using Entity = HSRTech.Domain.Entities;

namespace HSRTech.Domain.Contracts.Repositories.Tag
{
    public interface ITagRepository<TTag> : IRepository<TTag> where TTag : Entity.Tag { }
    
}
