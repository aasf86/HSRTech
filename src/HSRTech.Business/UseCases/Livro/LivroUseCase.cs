using HSRTech.Business.Contracts.UseCases.Livro;
using HSRTech.Business.Dtos;
using HSRTech.Business.Dtos.Livro;
using HSRTech.Domain.Contracts.Entities;
using HSRTech.Domain.Contracts.Repositories.Livro;
using HSRTech.Domain.Contracts.Repositories.TipoEncadernacao;
using HSRTech.Domain.Entities.ValueObjects;
using HSRTech.Infrastructure.EntitiesModels;
using Microsoft.Extensions.Logging;
using System.Data;
using static HSRTech.Domain.Entities.LivroRules;

namespace HSRTech.Business.UseCases.Livro
{
    public class LivroUseCase : UseCaseBase, ILivroUseCase
    {
        private readonly ILivroRepository<LivroModel> _livroRepository;
        private ILivroRepository<LivroModel> LivroRepository => _livroRepository;
        private readonly ITipoEncadernacaoRepository<TipoEncadernacaoModel> _tipoEncadernacaoRepository;
        private ITipoEncadernacaoRepository<TipoEncadernacaoModel> TipoEncadernacaoRepository => _tipoEncadernacaoRepository;

        private readonly Dictionary<eLivroType, Func<LivroCaracteristicaInsert, int, ILivroCaracteristica>> _factoryLivroCaracteristicaInsert = new ()
        {
            {
                eLivroType.Digital,
                (LivroCaracteristicaInsert livro, int codigo) =>
                {
                    return new LivroDigitalModel(codigo, livro.Formato, eLivroType.Digital);
                }
            },
            {
                eLivroType.Impresso,
                (LivroCaracteristicaInsert livro, int codigo) =>
                {
                    return new LivroImpressoModel(0, codigo, livro.Peso, livro.TipoEncadernacaoCodigo, eLivroType.Impresso);
                }
            }
        };

        private readonly Dictionary<eLivroType, Func<LivroCaracteristicaUpdate, int, ILivroCaracteristica>> _factoryLivroCaracteristicaUpdate = new ()
        {
            {
                eLivroType.Digital,
                (LivroCaracteristicaUpdate livro, int codigo) =>
                {
                    return new LivroDigitalModel(codigo, livro.Formato, eLivroType.Digital);
                }
            },
            {
                eLivroType.Impresso,
                (LivroCaracteristicaUpdate livro, int codigo) =>
                {
                    return new LivroImpressoModel(0, codigo, livro.Peso, livro.TipoEncadernacaoCodigo, eLivroType.Impresso);
                }
            }
        };

        public LivroUseCase(
            ILogger<LivroUseCase> logger,
            ILivroRepository<LivroModel> livroRepository,
            ITipoEncadernacaoRepository<TipoEncadernacaoModel> tipoEncadernacaoRepository,
            IDbConnection dbConnection) : base(logger, dbConnection)
        {
            _livroRepository = livroRepository;
            _tipoEncadernacaoRepository = tipoEncadernacaoRepository;
            TransactionAssigner.Add(LivroRepository.SetTransaction);
            TransactionAssigner.Add(TipoEncadernacaoRepository.SetTransaction);
        }

        public async Task<ResponseBase<int>> Insert(RequestBase<LivroInsert> livroInsertRequest)
        {
            try
            {
                var livroInsert = livroInsertRequest.Data;
                var livroInsertResponse = ResponseBase.New(0, livroInsertRequest.RequestId);
                var result = Validate(livroInsert);

                if (!result.IsSuccess)
                {
                    livroInsertResponse.Errors.AddRange(result.Validation.Select(x => x.ErrorMessage).ToList());                    
                    return livroInsertResponse;
                }

                var livroEntity = new LivroModel(
                    0, 
                    livroInsert.Titulo, 
                    livroInsert.Autor, 
                    livroInsert.Lancamento,
                    livroInsert
                    .LivroCaracteristica
                    .Select(item => _factoryLivroCaracteristicaInsert[item.TipoLivro](item, 0))
                    .ToList());

                await UnitOfWorkExecute(async () =>
                {
                    var existsTipoEncadernacao = await ExistsTipoEncadernacao(livroEntity.LivroCaracteristica);

                    if(!existsTipoEncadernacao)
                    {
                        livroInsertResponse.Errors.Add(LivroMsgDialog.NotFoundTipoEncadernacao);
                        return;
                    }

                    await LivroRepository.Insert(livroEntity);
                    livroInsertResponse.Data = livroEntity.Codigo;
                });

                return livroInsertResponse;
            }
            catch (Exception exc)
            {
                "Erro no [Insert] livro: {LivroTitulo}".LogErr(livroInsertRequest.Data.Titulo);
                exc.Message.LogErr(exc);

                var livroInsertResponse = ResponseBase.New(0, livroInsertRequest.RequestId);
#if DEBUG
                livroInsertResponse.Errors.Add(exc.Message);
#endif
                livroInsertResponse.Errors.Add("Erro ao inserir livro.");

                return livroInsertResponse;
            }
        }

        public async Task<ResponseBase<LivroGet>> GetByCodigo(RequestBase<int> livroGetRequest)
        {
            try
            {
                var livroCodigo = livroGetRequest.Data;
                var livroGetResponse = ResponseBase.New(new LivroGet(), livroGetRequest.RequestId);

                if (livroCodigo <= 0) return livroGetResponse;

                await UnitOfWorkExecute(async () =>
                {
                    var livroFromDb = await LivroRepository.GetByKey(livroCodigo);

                    if (livroFromDb is null)
                    {
                        livroGetResponse.Errors.Add(LivroMsgDialog.NotFound);
                        return;
                    }

                    livroGetResponse.Data = new LivroGet
                    {
                        Codigo = livroFromDb.Codigo,
                        Titulo = livroFromDb.Titulo,
                        Lancamento = livroFromDb.Lancamento,
                        Autor = livroFromDb.Autor,
                        LivroCaracteristica = livroFromDb
                        .LivroCaracteristica
                        .Select(x => 
                        {
                            if (x is LivroDigitalModel)
                            {
                                var itemDigital = x as LivroDigitalModel;
                                return new LivroCaracteristicaGet
                                { 
                                    Formato = itemDigital.Formato,
                                    TipoLivro = eLivroType.Digital
                                };
                            }

                            var itemImpresso = x as LivroImpressoModel;

                            return new LivroCaracteristicaGet
                            {
                                Peso = itemImpresso.Peso,
                                TipoEncadernacaoCodigo = itemImpresso.TipoEncadernacaoCodigo
                            };

                        }).ToList()
                    };
                });

                return livroGetResponse;
            }
            catch (Exception exc)
            {
                "Erro no [GetByCodigo] livro: {LivroCodigo}".LogErr(livroGetRequest.Data);
                exc.Message.LogErr(exc);

                var livroGetResponse = ResponseBase.New(new LivroGet(), livroGetRequest.RequestId);
#if DEBUG
                livroGetResponse.Errors.Add(exc.Message);
#endif
                livroGetResponse.Errors.Add("Erro ao obter livro");

                return livroGetResponse;
            }
        }

        public async Task<ResponseBase<bool>> Update(RequestBase<LivroUpdate> livroUpdateRequest)
        {
            try
            {
                var livroUpdate = livroUpdateRequest.Data;
                var livroUpdateResponse = ResponseBase.New(false, livroUpdateRequest.RequestId);
                var result = Validate(livroUpdate);

                if (!result.IsSuccess)
                {
                    livroUpdateResponse.Errors.AddRange(result.Validation.Select(x => x.ErrorMessage).ToList());                    
                    return livroUpdateResponse;
                }

                await UnitOfWorkExecute(async () =>
                {
                    var livroFromDb = await LivroRepository.GetByKey(livroUpdate.Codigo);

                    if (livroFromDb is null)
                    {
                        livroUpdateResponse.Errors.Add(LivroMsgDialog.NotFound);
                        return;
                    }

                    var newLivroUpdated = new LivroModel(
                        livroUpdate.Codigo, 
                        livroUpdate.Titulo, 
                        livroUpdate.Autor, 
                        livroUpdate.Lancamento,
                        livroUpdate
                        .LivroCaracteristica
                        .Select(item => _factoryLivroCaracteristicaUpdate[item.TipoLivro](item, livroUpdate.Codigo))
                        .ToList());

                    var existsTipoEncadernacao = await ExistsTipoEncadernacao(newLivroUpdated.LivroCaracteristica);

                    if (!existsTipoEncadernacao)
                    {
                        livroUpdateResponse.Errors.Add(LivroMsgDialog.NotFoundTipoEncadernacao);
                        return;
                    }

                    var resultRunUpdate = await LivroRepository.Update(newLivroUpdated);

                    livroUpdateResponse.Data = resultRunUpdate;
                });

                return livroUpdateResponse;
            }
            catch (Exception exc)
            {
                "Erro ao [Update] livro: {LivroCodigo}".LogErr(livroUpdateRequest.Data.Codigo);
                exc.Message.LogErr(exc);

                var livroUpdateResponse = ResponseBase.New(false, livroUpdateRequest.RequestId);
#if DEBUG
                livroUpdateResponse.Errors.Add(exc.Message);
#endif
                livroUpdateResponse.Errors.Add("Erro ao alterar tag.");

                return livroUpdateResponse;
            }
        }

        public async Task<ResponseBase<bool>> Delete(RequestBase<int> livroDeleteRequest)
        {
            try
            {
                var livroDeleteCodigo = livroDeleteRequest.Data;
                var livroDeleteResponse = ResponseBase.New(false, livroDeleteRequest.RequestId);

                await UnitOfWorkExecute(async () =>
                {
                    var livroFromDb = await LivroRepository.GetByKey(livroDeleteCodigo);

                    if (livroFromDb is null)
                    {
                        livroDeleteResponse.Errors.Add(LivroMsgDialog.NotFound);
                        return;
                    }

                    await LivroRepository.Delete(livroFromDb);

                    livroDeleteResponse.Data = true;
                });

                return livroDeleteResponse;
            }
            catch (Exception exc)
            {
                "Erro [Delete] livro: {LivroCodigo}".LogErr(livroDeleteRequest.Data);
                exc.Message.LogErr(exc);

                var livroDeleteResponse = ResponseBase.New(false, livroDeleteRequest.RequestId);
#if DEBUG
                livroDeleteResponse.Errors.Add(exc.Message);
#endif
                livroDeleteResponse.Errors.Add("Erro ao excluir livro.");

                return livroDeleteResponse;
            }
        }

        public async Task<ResponseBase<List<LivroList>>> GetListLivro(RequestBase<LivroGetFilter> livroGetAllRequest)
        {
            try
            {
                var livroFiltro = livroGetAllRequest.Data;
                var livroGetAllResponse = ResponseBase.New(new List<LivroList>(), livroGetAllRequest.RequestId);

                var result = Validate(livroFiltro);

                if (!result.IsSuccess)
                {
                    livroGetAllResponse.Errors.AddRange(result.Validation.Select(x => x.ErrorMessage).ToList());
                    return livroGetAllResponse;
                }

                await UnitOfWorkExecute(async () =>
                {
                    var livrosListFromDb = await LivroRepository.GetListLivro(livroFiltro);

                    livroGetAllResponse.Data = livrosListFromDb;

                });

                return livroGetAllResponse;
            }
            catch (Exception exc)
            {
                "Erro no [GetAll] livro: {LivroFiltro}".LogErr(livroGetAllRequest.Data);
                exc.Message.LogErr(exc);

                var livroGetResponse = ResponseBase.New(new List<LivroList>(), livroGetAllRequest.RequestId);
#if DEBUG
                livroGetResponse.Errors.Add(exc.Message);
#endif
                livroGetResponse.Errors.Add("Erro ao obter livros");

                return livroGetResponse;
            }
        }

        private async Task<bool> ExistsTipoEncadernacao(List<ILivroCaracteristica> livroCaracteristicas)
        {
            foreach (var item in livroCaracteristicas)
            {
                if (item is LivroImpressoModel)
                {
                    var livroImpressoModel = (LivroImpressoModel)item;
                    var tipoEncadernacaoFromDb = await TipoEncadernacaoRepository.GetByKey(livroImpressoModel.TipoEncadernacaoCodigo);
                    if (tipoEncadernacaoFromDb is null)
                    {                        
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
