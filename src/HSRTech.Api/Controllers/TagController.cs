using HSRTech.Business.Contracts.UseCases.Tag;
using HSRTech.Business.Dtos;
using HSRTech.Business.Dtos.Tag;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty taskes, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HSRTech.Api.Controllers
{
    /// <summary>
    /// Controller para gestão de cadastros de tags.
    /// </summary>

    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagUseCase _tagUseCase;
        private ITagUseCase TagUseCase => _tagUseCase;

        /// <summary>
        /// Controller para gestão de cadastros de tags.
        /// </summary>
        /// <param name="taskeUseCase"></param>
        public TagController(ITagUseCase taskeUseCase)
        {
            _tagUseCase = taskeUseCase;
        }
        
        /// <summary>
        /// Inserir nova tag.
        /// </summary>
        /// <param name="tagInsert"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] TagInsert tagInsert)
        {
            if (ModelState.IsValid)
            {
                var tagInsertRequest = RequestBase.New(tagInsert, "host:api", "1.0");
                var tagInsertResponse = await TagUseCase.Insert(tagInsertRequest);

                if (tagInsertResponse.IsSuccess)
                    return Ok(tagInsertResponse);

                return BadRequest(tagInsertResponse);
            }

            return BadRequest();
        }

        /// <summary>
        /// Obtem uma tag pelo 'Codigo'.
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        [HttpGet("{codigo}")]
        public async Task<IActionResult> Get(int codigo)
        {
            if (codigo < 1) return BadRequest(ResponseBase.New(new TagGet(), Guid.NewGuid()));

            var tagGet = RequestBase.New(codigo, "host:api", "1.0");
            var tagGetResponse = await TagUseCase.GetByCodigo(tagGet);

            if (tagGetResponse.IsSuccess)
                return Ok(tagGetResponse);

            return NotFound(tagGetResponse);
        }
        
        /// <summary>
        /// Atualizar a tag.
        /// </summary>
        /// <param name="tagUpdate"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] TagUpdate tagUpdate)
        {
            if (ModelState.IsValid)
            {
                var tagRequest = RequestBase.New(tagUpdate, "host:api", "1.0");
                var tagResponse = await TagUseCase.Update(tagRequest);

                if (tagResponse.IsSuccess)
                    return Ok(tagResponse);

                return BadRequest(tagResponse);
            }
            return BadRequest();
        }

        /// <summary>
        /// Remove tag pelo seu 'codigo'.
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        [HttpDelete("{codigo}")]
        public async Task<IActionResult> Delete(int codigo)
        {
            if (codigo < 1) return BadRequest(ResponseBase.New(false, Guid.NewGuid()));

            var tagDeleteRequest = RequestBase.New(codigo, "host:api", "1.0");
            var tagDeleteResponse = await TagUseCase.Delete(tagDeleteRequest);

            if (tagDeleteResponse.IsSuccess)
                return Ok(tagDeleteResponse);

            return BadRequest(tagDeleteResponse);
        }
    }
}
