using HSRTech.Business.Contracts.UseCases.Tag;
using HSRTech.Business.Dtos;
using HSRTech.Business.Dtos.Tag;
using HSRTech.Domain.Contracts.Repositories.Tag;
using HSRTech.Infrastructure.EntitiesModels;
using Microsoft.Extensions.Logging;
using System.Data;
using static HSRTech.Domain.Entities.Tag;

namespace HSRTech.Business.UseCases.Tag
{
    public class TagUseCase : UseCaseBase, ITagUseCase
    {
        private readonly ITagRepository<TagModel> _tagRepository;
        private ITagRepository<TagModel> TagRepository => _tagRepository;

        public TagUseCase(
            ILogger<TagUseCase> logger,
            ITagRepository<TagModel> tagRepository,
            IDbConnection dbConnection) : base(logger, dbConnection)
        {
            _tagRepository = tagRepository;
            TransactionAssigner.Add(TagRepository.SetTransaction);
        }

        public async Task<ResponseBase<int>> Insert(RequestBase<TagInsert> tagInsertRequest)
        {
            try
            {
                var tagInsert = tagInsertRequest.Data;
                var tagInsertResponse = ResponseBase.New(0, tagInsertRequest.RequestId);
                var result = Validate(tagInsert);

                if (!result.IsSuccess)
                {
                    tagInsertResponse.Errors.AddRange(result.Validation.Select(x => x.ErrorMessage).ToList());                    
                    return tagInsertResponse;
                }

                var tagEntity = new TagModel(0, tagInsert.Descricao, tagInsert.LivroCodigo);

                await UnitOfWorkExecute(async () =>
                {
                    await TagRepository.Insert(tagEntity);
                    tagInsertResponse.Data = tagEntity.Codigo;
                });

                return tagInsertResponse;
            }
            catch (Exception exc)
            {
                "Erro no [Insert] tag: {Tag}".LogErr(tagInsertRequest.Data.Descricao);
                exc.Message.LogErr(exc);

                var tagInsertResponse = ResponseBase.New(0, tagInsertRequest.RequestId);
#if DEBUG
                tagInsertResponse.Errors.Add(exc.Message);
#endif
                tagInsertResponse.Errors.Add("Erro ao inserir tag.");

                return tagInsertResponse;
            }
        }

        public async Task<ResponseBase<TagGet>> GetByCodigo(RequestBase<int> tagGetRequest)
        {
            try
            {
                var tagId = tagGetRequest.Data;
                var tagGetResponse = ResponseBase.New(new TagGet(), tagGetRequest.RequestId);

                if (tagId <= 0) return tagGetResponse;

                await UnitOfWorkExecute(async () =>
                {
                    var tagFromDb = await TagRepository.GetByKey(tagId);

                    if (tagFromDb is null)
                    {
                        tagGetResponse.Errors.Add(TagMsgDialog.NotFound);
                        return;
                    }

                    tagGetResponse.Data = new TagGet
                    {
                        Codigo = tagFromDb.Codigo,
                        LivroCodigo = tagFromDb.LivroCodigo,
                        Descricao = tagFromDb.Descricao
                    };
                });

                return tagGetResponse;
            }
            catch (Exception exc)
            {
                "Erro no [GetByCodigo] tab: {TabCodigo}".LogErr(tagGetRequest.Data);
                exc.Message.LogErr(exc);

                var tagGetResponse = ResponseBase.New(new TagGet(), tagGetRequest.RequestId);
#if DEBUG
                tagGetResponse.Errors.Add(exc.Message);
#endif
                tagGetResponse.Errors.Add("Erro ao obter tag");

                return tagGetResponse;
            }
        }

        public async Task<ResponseBase<bool>> Update(RequestBase<TagUpdate> tagUpdateRequest)
        {
            try
            {
                var tagUpdate = tagUpdateRequest.Data;
                var tagUpdateResponse = ResponseBase.New(false, tagUpdateRequest.RequestId);
                var result = Validate(tagUpdate);

                if (!result.IsSuccess)
                {
                    tagUpdateResponse.Errors.AddRange(result.Validation.Select(x => x.ErrorMessage).ToList());                    
                    return tagUpdateResponse;
                }

                await UnitOfWorkExecute(async () =>
                {
                    var tagFromDb = await TagRepository.GetByKey(tagUpdate.Codigo);

                    if (tagFromDb is null)
                    {
                        tagUpdateResponse.Errors.Add(TagMsgDialog.NotFound);
                        return;
                    }

                    tagFromDb.SetDescricao(tagUpdate.Descricao);                    

                    await TagRepository.Update(tagFromDb);

                    tagUpdateResponse.Data = true;
                });

                return tagUpdateResponse;
            }
            catch (Exception exc)
            {
                "Erro ao [Update] tag: {TagCodigo}".LogErr(tagUpdateRequest.Data.Codigo);
                exc.Message.LogErr(exc);

                var tagUpdateResponse = ResponseBase.New(false, tagUpdateRequest.RequestId);
#if DEBUG
                tagUpdateResponse.Errors.Add(exc.Message);
#endif
                tagUpdateResponse.Errors.Add("Erro ao alterar tag.");

                return tagUpdateResponse;
            }
        }

        public async Task<ResponseBase<bool>> Delete(RequestBase<int> tagDeleteRequest)
        {
            try
            {
                var tagDeleteCodigo = tagDeleteRequest.Data;
                var tagDeleteResponse = ResponseBase.New(false, tagDeleteRequest.RequestId);                

                await UnitOfWorkExecute(async () =>
                {
                    var tagFromDb = await TagRepository.GetByKey(tagDeleteCodigo);

                    if (tagFromDb is null)
                    {
                        tagDeleteResponse.Errors.Add(TagMsgDialog.NotFound);
                        return;
                    }

                    await TagRepository.Delete(tagFromDb);

                    tagDeleteResponse.Data = true;
                });

                return tagDeleteResponse;
            }
            catch (Exception exc)
            {
                "Erro [Delete] tag: {TagCodigo}".LogErr(tagDeleteRequest.Data);
                exc.Message.LogErr(exc);

                var tagDeleteResponse = ResponseBase.New(false, tagDeleteRequest.RequestId);
#if DEBUG
                tagDeleteResponse.Errors.Add(exc.Message);
#endif
                tagDeleteResponse.Errors.Add("Erro ao excluir tag.");

                return tagDeleteResponse;
            }
        }
    }
}
