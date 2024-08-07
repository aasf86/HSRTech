using HSRTech.Business.Contracts.UseCases.TipoEncadernacao;
using HSRTech.Business.Dtos;
using HSRTech.Business.Dtos.TipoEncadernacao;
using HSRTech.Domain.Contracts.Repositories.TipoEncadernacao;
using HSRTech.Infrastructure.EntitiesModels;
using Microsoft.Extensions.Logging;
using System.Data;
using static HSRTech.Domain.Entities.TipoEncadernacao;

namespace HSRTech.Business.UseCases.TipoEncadernacao
{
    public class TipoEncadernacaoUseCase : UseCaseBase, ITipoEncadernacaoUseCase
    {
        private readonly ITipoEncadernacaoRepository<TipoEncadernacaoModel> _tipoEncadernacaoRepository;
        private ITipoEncadernacaoRepository<TipoEncadernacaoModel> TipoEncadernacaoRepository => _tipoEncadernacaoRepository;

        public TipoEncadernacaoUseCase(
            ILogger<TipoEncadernacaoUseCase> logger,
            ITipoEncadernacaoRepository<TipoEncadernacaoModel> tipoEncadernacaoRepository,
            IDbConnection dbConnection) : base(logger, dbConnection)
        {
            _tipoEncadernacaoRepository = tipoEncadernacaoRepository;
            TransactionAssigner.Add(TipoEncadernacaoRepository.SetTransaction);
        }

        public async Task<ResponseBase<int>> Insert(RequestBase<TipoEncadernacaoInsert> tipoEncadernacaoInsertRequest)
        {
            try
            {
                var tipoEncadernacaoInsert = tipoEncadernacaoInsertRequest.Data;
                var tipoEncadernacaoInsertResponse = ResponseBase.New(0, tipoEncadernacaoInsertRequest.RequestId);
                var result = Validate(tipoEncadernacaoInsert);

                if (!result.IsSuccess)
                {
                    tipoEncadernacaoInsertResponse.Errors.AddRange(result.Validation.Select(x => x.ErrorMessage).ToList());                    
                    return tipoEncadernacaoInsertResponse;
                }

                var tagEntity = new TipoEncadernacaoModel(
                    0, 
                    tipoEncadernacaoInsert.Nome, 
                    tipoEncadernacaoInsert.Descricao, 
                    tipoEncadernacaoInsert.Formato);

                await UnitOfWorkExecute(async () =>
                {
                    await TipoEncadernacaoRepository.Insert(tagEntity);
                    tipoEncadernacaoInsertResponse.Data = tagEntity.Codigo;
                });

                return tipoEncadernacaoInsertResponse;
            }
            catch (Exception exc)
            {
                "Erro no [Insert] tipo de encadernacao: {TipoEncadernacaoNome}".LogErr(tipoEncadernacaoInsertRequest.Data.Nome);
                exc.Message.LogErr(exc);

                var tipoEncadernacaoInsertResponse = ResponseBase.New(0, tipoEncadernacaoInsertRequest.RequestId);
#if DEBUG
                tipoEncadernacaoInsertResponse.Errors.Add(exc.Message);
#endif
                tipoEncadernacaoInsertResponse.Errors.Add("Erro ao inserir tipo de encadernacao.");

                return tipoEncadernacaoInsertResponse;
            }
        }

        public async Task<ResponseBase<TipoEncadernacaoGet>> GetByCodigo(RequestBase<int> tipoEncadernacaoGetRequest)
        {
            try
            {
                var tipoEncadernacaoId = tipoEncadernacaoGetRequest.Data;
                var tipoEncadernacaoGetResponse = ResponseBase.New(new TipoEncadernacaoGet(), tipoEncadernacaoGetRequest.RequestId);

                if (tipoEncadernacaoId <= 0) return tipoEncadernacaoGetResponse;

                await UnitOfWorkExecute(async () =>
                {
                    var tipoEncadernacaoFromDb = await TipoEncadernacaoRepository.GetByKey(tipoEncadernacaoId);

                    if (tipoEncadernacaoFromDb is null)
                    {
                        tipoEncadernacaoGetResponse.Errors.Add(TipoEncadernacaoMsgDialog.NotFound);
                        return;
                    }

                    tipoEncadernacaoGetResponse.Data = new TipoEncadernacaoGet
                    {
                        Codigo = tipoEncadernacaoFromDb.Codigo,
                        Nome = tipoEncadernacaoFromDb.Nome,
                        Descricao = tipoEncadernacaoFromDb.Descricao,
                        Formato = tipoEncadernacaoFromDb.Formato
                    };
                });

                return tipoEncadernacaoGetResponse;
            }
            catch (Exception exc)
            {
                "Erro no [GetByCodigo] tipo de encadernação: {TipoEncadernacaoNomeCodigo}".LogErr(tipoEncadernacaoGetRequest.Data);
                exc.Message.LogErr(exc);

                var tipoEncadernacaoGetResponse = ResponseBase.New(new TipoEncadernacaoGet(), tipoEncadernacaoGetRequest.RequestId);
#if DEBUG
                tipoEncadernacaoGetResponse.Errors.Add(exc.Message);
#endif
                tipoEncadernacaoGetResponse.Errors.Add("Erro ao obter tipo de encadernação");

                return tipoEncadernacaoGetResponse;
            }
        }

        public async Task<ResponseBase<bool>> Update(RequestBase<TipoEncadernacaoUpdate> tipoEncadernacaoUpdateRequest)
        {
            try
            {
                var tipoEncadernacaoUpdate = tipoEncadernacaoUpdateRequest.Data;
                var tipoEncadernacaoUpdateResponse = ResponseBase.New(false, tipoEncadernacaoUpdateRequest.RequestId);
                var result = Validate(tipoEncadernacaoUpdate);

                if (!result.IsSuccess)
                {
                    tipoEncadernacaoUpdateResponse.Errors.AddRange(result.Validation.Select(x => x.ErrorMessage).ToList());                    
                    return tipoEncadernacaoUpdateResponse;
                }

                await UnitOfWorkExecute(async () =>
                {
                    var tipoEncadernacaoFromDb = await TipoEncadernacaoRepository.GetByKey(tipoEncadernacaoUpdate.Codigo);

                    if (tipoEncadernacaoFromDb is null)
                    {
                        tipoEncadernacaoUpdateResponse.Errors.Add(TipoEncadernacaoMsgDialog.NotFound);
                        return;
                    }

                    var newTipoEncadernacaoUpdated = new TipoEncadernacaoModel(
                        tipoEncadernacaoUpdate.Codigo,
                        tipoEncadernacaoUpdate.Nome,
                        tipoEncadernacaoUpdate.Descricao,
                        tipoEncadernacaoUpdate.Formato);                    

                    tipoEncadernacaoUpdateResponse.Data = await TipoEncadernacaoRepository.Update(newTipoEncadernacaoUpdated);
                });

                return tipoEncadernacaoUpdateResponse;
            }
            catch (Exception exc)
            {
                "Erro ao [Update] tipo de encadernação: {TipoEncadernacaoCodigo}".LogErr(tipoEncadernacaoUpdateRequest.Data.Codigo);
                exc.Message.LogErr(exc);

                var tipoEncadernacaoUpdateResponse = ResponseBase.New(false, tipoEncadernacaoUpdateRequest.RequestId);
#if DEBUG
                tipoEncadernacaoUpdateResponse.Errors.Add(exc.Message);
#endif
                tipoEncadernacaoUpdateResponse.Errors.Add("Erro ao alterar tipo de encadernação.");

                return tipoEncadernacaoUpdateResponse;
            }
        }

        public async Task<ResponseBase<bool>> Delete(RequestBase<int> tipoEncadernacaoDeleteRequest)
        {
            try
            {
                var tipoEncadernacaoDeleteCodigo = tipoEncadernacaoDeleteRequest.Data;
                var tipoEncadernacaoDeleteResponse = ResponseBase.New(false, tipoEncadernacaoDeleteRequest.RequestId);                

                await UnitOfWorkExecute(async () =>
                {
                    var tipoEncadernacaoFromDb = await TipoEncadernacaoRepository.GetByKey(tipoEncadernacaoDeleteCodigo);

                    if (tipoEncadernacaoFromDb is null)
                    {
                        tipoEncadernacaoDeleteResponse.Errors.Add(TipoEncadernacaoMsgDialog.NotFound);
                        return;
                    }                    

                    tipoEncadernacaoDeleteResponse.Data = await TipoEncadernacaoRepository.Delete(tipoEncadernacaoFromDb);
                });

                return tipoEncadernacaoDeleteResponse;
            }
            catch (Exception exc)
            {
                "Erro [Delete] tipo de encadernacao: {TipoEncadernacaoCodigo}".LogErr(tipoEncadernacaoDeleteRequest.Data);
                exc.Message.LogErr(exc);

                var tipoEncadernacaoDeleteResponse = ResponseBase.New(false, tipoEncadernacaoDeleteRequest.RequestId);
#if DEBUG
                tipoEncadernacaoDeleteResponse.Errors.Add(exc.Message);
#endif
                tipoEncadernacaoDeleteResponse.Errors.Add("Erro ao excluir tipo de encadernação.");

                return tipoEncadernacaoDeleteResponse;
            }
        }
    }
}
