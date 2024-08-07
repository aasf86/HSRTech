using HSRTech.Business.Contracts.UseCases.TipoEncadernacao;
using HSRTech.Business.Dtos;
using HSRTech.Business.Dtos.TipoEncadernacao;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty taskes, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HSRTech.Api.Controllers
{
    /// <summary>
    /// Controller para gestão de tipos de encadernação.
    /// </summary>

    [Route("api/[controller]")]
    [ApiController]
    public class TipoEncadernacaoController : ControllerBase
    {
        private readonly ITipoEncadernacaoUseCase _tipoEncadernacaoUseCase;
        private ITipoEncadernacaoUseCase TipoEncadernacaoUseCase => _tipoEncadernacaoUseCase;

        /// <summary>
        /// Controller para gestão de cadastros de livros.
        /// </summary>
        /// <param name="taskeUseCase"></param>
        public TipoEncadernacaoController(ITipoEncadernacaoUseCase taskeUseCase)
        {
            _tipoEncadernacaoUseCase = taskeUseCase;
        }

        /// <summary>
        /// Inserir um novo tipo de encadernação.
        /// </summary>
        /// <param name="tipoEncaderncaoInsert"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] TipoEncadernacaoInsert tipoEncaderncaoInsert)
        {
            if (ModelState.IsValid)
            {
                var livroInsertRequest = RequestBase.New(tipoEncaderncaoInsert, "host:api", "1.0");
                var livroInsertResponse = await TipoEncadernacaoUseCase.Insert(livroInsertRequest);

                if (livroInsertResponse.IsSuccess)
                    return Ok(livroInsertResponse);

                return BadRequest(livroInsertResponse);
            }

            return BadRequest();
        }

        /// <summary>
        /// Obtem um tipo de encadernação pelo 'Codigo'.
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        [HttpGet("{codigo}")]
        public async Task<IActionResult> Get(int codigo)
        {
            if (codigo < 1) return BadRequest(ResponseBase.New(new TipoEncadernacaoGet(), Guid.NewGuid()));

            var livroGet = RequestBase.New(codigo, "host:api", "1.0");
            var livroGetResponse = await TipoEncadernacaoUseCase.GetByCodigo(livroGet);

            if (livroGetResponse.IsSuccess)
                return Ok(livroGetResponse);

            return NotFound(livroGetResponse);
        }

        /// <summary>
        /// Atualizar uma tipo de encadernação.
        /// </summary>
        /// <param name="tipoEncadernacaoUpdate"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] TipoEncadernacaoUpdate tipoEncadernacaoUpdate)
        {
            if (ModelState.IsValid)
            {
                var livroRequest = RequestBase.New(tipoEncadernacaoUpdate, "host:api", "1.0");
                var livroResponse = await TipoEncadernacaoUseCase.Update(livroRequest);

                if (livroResponse.IsSuccess)
                    return Ok(livroResponse);

                return BadRequest(livroResponse);
            }
            return BadRequest();
        }

        /// <summary>
        /// Remove um tipo de encadernação pelo seu 'codigo'.
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        [HttpDelete("{codigo}")]
        public async Task<IActionResult> Delete(int codigo)
        {
            if (codigo < 1) return BadRequest(ResponseBase.New(false, Guid.NewGuid()));

            var livroDeleteRequest = RequestBase.New(codigo, "host:api", "1.0");
            var livroDeleteResponse = await TipoEncadernacaoUseCase.Delete(livroDeleteRequest);

            if (livroDeleteResponse.IsSuccess)
                return Ok(livroDeleteResponse);

            return BadRequest(livroDeleteResponse);
        }
    }
}
