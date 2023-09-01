using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SampleWebApiAspNetCore.Dtos;
using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Helpers;
using SampleWebApiAspNetCore.Services;
using SampleWebApiAspNetCore.Models;
using SampleWebApiAspNetCore.Repositories;
using System.Text.Json;

namespace SampleWebApiAspNetCore.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CharasController : ControllerBase
    {
        private readonly ICharasRepository _charasRepository;
        private readonly IMapper _mapper;
        private readonly ILinkService<CharasController> _linkService;

        public CharasController(
            ICharasRepository charasRepository,
            IMapper mapper,
            ILinkService<CharasController> linkService)
        {
            _charasRepository = charasRepository;
            _mapper = mapper;
            _linkService = linkService;
        }

        [HttpGet(Name = nameof(GetAllCharas))]
        public ActionResult GetAllCharas(ApiVersion version, [FromQuery] QueryParameters queryParameters)
        {
            List<CharasEntity> charasItems = _charasRepository.GetAll(queryParameters).ToList();

            var allItemCount = _charasRepository.Count();

            var paginationMetadata = new
            {
                totalCount = allItemCount,
                pageSize = queryParameters.PageCount,
                currentPage = queryParameters.Page,
                totalPages = queryParameters.GetTotalPages(allItemCount)
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var links = _linkService.CreateLinksForCollection(queryParameters, allItemCount, version);
            var toReturn = charasItems.Select(x => _linkService.ExpandSingleCharaItem(x, x.Id, version));

            return Ok(new
            {
                value = toReturn,
                links = links
            });
        }

        [HttpGet]
        [Route("{id:int}", Name = nameof(GetSingleChara))]
        public ActionResult GetSingleChara(ApiVersion version, int id)
        {
            CharasEntity charaItem = _charasRepository.GetSingle(id);

            if (charaItem == null)
            {
                return NotFound();
            }

            CharasDto item = _mapper.Map<CharasDto>(charaItem);

            return Ok(_linkService.ExpandSingleCharaItem(item, item.Id, version));
        }

        [HttpPost(Name = nameof(AddChara))]
        public ActionResult<CharasDto> AddChara(ApiVersion version, [FromBody] CharaCreateDto charaCreateDto)
        {
            if (charaCreateDto == null)
            {
                return BadRequest();
            }

            CharasEntity toAdd = _mapper.Map<CharasEntity>(charaCreateDto);

            _charasRepository.Add(toAdd);

            if (!_charasRepository.Save())
            {
                throw new Exception("Creating a charaitem failed on save.");
            }

            CharasEntity newCharaItem = _charasRepository.GetSingle(toAdd.Id);
            CharasDto charasDto = _mapper.Map<CharasDto>(newCharaItem);

            return CreatedAtRoute(nameof(GetSingleChara),
                new { version = version.ToString(), id = newCharaItem.Id },
                _linkService.ExpandSingleCharaItem(charasDto, charasDto.Id, version));
        }

        [HttpPatch("{id:int}", Name = nameof(PartiallyUpdateChara))]
        public ActionResult<CharasDto> PartiallyUpdateChara(ApiVersion version, int id, [FromBody] JsonPatchDocument<CharasUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            CharasEntity existingEntity = _charasRepository.GetSingle(id);

            if (existingEntity == null)
            {
                return NotFound();
            }

            CharasUpdateDto charasUpdateDto = _mapper.Map<CharasUpdateDto>(existingEntity);
            patchDoc.ApplyTo(charasUpdateDto);

            TryValidateModel(charasUpdateDto);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(charasUpdateDto, existingEntity);
            CharasEntity updated = _charasRepository.Update(id, existingEntity);

            if (!_charasRepository.Save())
            {
                throw new Exception("Updating a charasitem failed on save.");
            }

            CharasDto charasDto = _mapper.Map<CharasDto>(updated);

            return Ok(_linkService.ExpandSingleCharaItem(charasDto, charasDto.Id, version));
        }

        [HttpDelete]
        [Route("{id:int}", Name = nameof(RemoveChara))]
        public ActionResult RemoveChara(int id)
        {
            CharasEntity charaItem = _charasRepository.GetSingle(id);

            if (charaItem == null)
            {
                return NotFound();
            }

            _charasRepository.Delete(id);

            if (!_charasRepository.Save())
            {
                throw new Exception("Deleting a charasitem failed on save.");
            }

            return NoContent();
        }

        [HttpPut]
        [Route("{id:int}", Name = nameof(UpdateChara))]
        public ActionResult<CharasDto> UpdateChara(ApiVersion version, int id, [FromBody] CharasUpdateDto charasUpdateDto)
        {
            if (charasUpdateDto == null)
            {
                return BadRequest();
            }

            var existingCharaItem = _charasRepository.GetSingle(id);

            if (existingCharaItem == null)
            {
                return NotFound();
            }

            _mapper.Map(charasUpdateDto, existingCharaItem);

            _charasRepository.Update(id, existingCharaItem);

            if (!_charasRepository.Save())
            {
                throw new Exception("Updating a charasitem failed on save.");
            }

            CharasDto charasDto = _mapper.Map<CharasDto>(existingCharaItem);

            return Ok(_linkService.ExpandSingleCharaItem(charasDto, charasDto.Id, version));
        }

        [HttpGet("GetRandomChara", Name = nameof(GetRandomChara))]
        public ActionResult GetRandomChara()
        {
            ICollection<CharasEntity> charaItems = _charasRepository.GetRandomChara();

            IEnumerable<CharasDto> dtos = charaItems.Select(x => _mapper.Map<CharasDto>(x));

            var links = new List<LinkDto>();

            // self 
            links.Add(new LinkDto(Url.Link(nameof(GetRandomChara), null), "self", "GET"));

            return Ok(new
            {
                value = dtos,
                links = links
            });
        }
    }
}
