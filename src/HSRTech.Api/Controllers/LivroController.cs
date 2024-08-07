using HSRTech.Business.Contracts.UseCases.Livro;
using HSRTech.Business.Contracts.UseCases.Tag;
using HSRTech.Business.Dtos;
using HSRTech.Business.Dtos.Livro;
using HSRTech.Business.Dtos.Tag;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty taskes, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HSRTech.Api.Controllers
{
    /// <summary>
    /// Controller para gestão de cadastros de livros.
    /// </summary>

    [Route("api/[controller]")]
    [ApiController]
    public class LivroController : ControllerBase
    {
        private readonly ILivroUseCase _livroUseCase;
        private ILivroUseCase LivroUseCase => _livroUseCase;

        /// <summary>
        /// Controller para gestão de cadastros de livros.
        /// </summary>
        /// <param name="taskeUseCase"></param>
        public LivroController(ILivroUseCase taskeUseCase)
        {
            _livroUseCase = taskeUseCase;
        }
        
        /// <summary>
        /// Inserir novo livro.
        /// </summary>
        /// <param name="livroInsert"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] LivroInsert livroInsert)
        {
            if (ModelState.IsValid)
            {
                var livroInsertRequest = RequestBase.New(livroInsert, "host:api", "1.0");
                var livroInsertResponse = await LivroUseCase.Insert(livroInsertRequest);

                if (livroInsertResponse.IsSuccess)
                    return Ok(livroInsertResponse);

                return BadRequest(livroInsertResponse);
            }

            return BadRequest();
        }

        /// <summary>
        /// Obtem um livro pelo 'Codigo'.
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        [HttpGet("{codigo}")]
        public async Task<IActionResult> Get(int codigo)
        {
            if (codigo < 1) return BadRequest(ResponseBase.New(new LivroGet(), Guid.NewGuid()));

            var livroGet = RequestBase.New(codigo, "host:api", "1.0");
            var livroGetResponse = await LivroUseCase.GetByCodigo(livroGet);

            if (livroGetResponse.IsSuccess)
                return Ok(livroGetResponse);

            return NotFound(livroGetResponse);
        }
        
        /// <summary>
        /// Atualizar o livro.
        /// </summary>
        /// <param name="livroUpdate"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] LivroUpdate livroUpdate)
        {
            if (ModelState.IsValid)
            {
                var livroRequest = RequestBase.New(livroUpdate, "host:api", "1.0");
                var livroResponse = await LivroUseCase.Update(livroRequest);

                if (livroResponse.IsSuccess)
                    return Ok(livroResponse);

                return BadRequest(livroResponse);
            }
            return BadRequest();
        }

        /// <summary>
        /// Remove o livro pelo seu 'codigo'.
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        [HttpDelete("{codigo}")]
        public async Task<IActionResult> Delete(int codigo)
        {
            if (codigo < 1) return BadRequest(ResponseBase.New(false, Guid.NewGuid()));

            var livroDeleteRequest = RequestBase.New(codigo, "host:api", "1.0");
            var livroDeleteResponse = await LivroUseCase.Delete(livroDeleteRequest);

            if (livroDeleteResponse.IsSuccess)
                return Ok(livroDeleteResponse);

            return BadRequest(livroDeleteResponse);
        }

        /// <summary>
        /// Busca os livros pelos filtros de ano e mês.
        /// </summary>
        /// <param name="livroGetAll"></param>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery] int ano, int mes)
        {
            var livroGetAll = new LivroGetAll { Ano = ano, Mes = mes };

            var result = LivroUseCase.Validate(livroGetAll);

            if (result.IsSuccess)
            {
                var LivroGetAllRequest = RequestBase.New(livroGetAll, "host:api", "1.0");
                var livroGetResponse = await LivroUseCase.GetAll(LivroGetAllRequest);

                if (livroGetResponse.IsSuccess)
                    return Ok(livroGetResponse);

                return BadRequest(livroGetResponse);
            }
            return BadRequest();
        }
    }
}
