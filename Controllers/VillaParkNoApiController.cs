﻿using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VilllaParks.Core.IRepository;
using VilllaParks.Model;
using VilllaParks.Model.Dto;

namespace VilllaParks.Controllers
{
    [Route("api/VillaParkNumberApi")]
    [ApiController]
    public class VillaParkNoApiController : ControllerBase
    {
        private readonly IVillaNumberRepository _villaParkNo;
        private readonly IVillaParkRepository _dbvillaPark;
        private readonly IMapper _mapper;
        protected ApiResponse _response;
        private readonly ILogger<VillaParkAPIController> _logger;

        public VillaParkNoApiController(IVillaNumberRepository villaParkNo, IMapper mapper, ILogger<VillaParkAPIController> logger, IVillaParkRepository dbvillaPark)
        {
            _villaParkNo = villaParkNo;
            _mapper = mapper;
            _response = new();
            _logger = logger;
            _dbvillaPark = dbvillaPark;

        }
        [HttpGet("GetVillaParkNumbers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> GetVillaParkNumbers()
        {
            //_logger.Log("Getting all TouristHubs","");
            try
            {
                IEnumerable<VillaParkNumber> villaParkList = await _villaParkNo.GetAllAsync();
                _response.Result = _mapper.Map<List<VillaParkNumberDto>>(villaParkList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.ErrorMessage
                    = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpGet("{id:int}", Name = "GetVillaParkNumbers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> GetVillaParkNumber(int id)
        {
            try
            {

                if (id == 0)
                {
                    _logger.LogInformation("Get Villa Error with Id " + id, "Error");
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var villaParkNumber = await _villaParkNo.GetAsync(x => x.VillaParkNo == id);
                if (villaParkNumber == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return BadRequest(_response);
                }
                _response.Result = _mapper.Map<VillaParkNumberDto>(villaParkNumber);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.ErrorMessage
                    = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost("CreateVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> CreateVillaNumber([FromBody] VillaParkNoCreatDTO CreateDto)
        {
            try
            {
                if (await _villaParkNo.GetAsync(u => u.VillaParkNo ==
                 CreateDto.VillaParkNo) != null)
                {
                    ModelState.AddModelError("CustomError", "Villa Park No Already Exits!");
                    return BadRequest(ModelState);
                }
                //check if the record exist for the id
                if(await _dbvillaPark.GetAsync(u => u.Id == CreateDto.VillaParkID) == null)
                {
                    ModelState.AddModelError("CustomError", "Villa ID is Invalid");
                    return BadRequest(ModelState);
                }
                //Check to see if the VillaparkDto is null
                if (CreateDto == null)
                {
                    return BadRequest(CreateDto);
                }

                VillaParkNumber villaParkNo = _mapper.Map<VillaParkNumber>(CreateDto);

                await _villaParkNo.CreateAsync(villaParkNo);

                _response.Result = _mapper.Map<VillaParkNumberDto>(villaParkNo);
                _response.StatusCode = HttpStatusCode.Created;


                return CreatedAtRoute("GetVillaParks", new { id = villaParkNo.VillaParkNo }, _response);
            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.ErrorMessage
                    = new List<string>() { ex.ToString() };
            }
            return _response;

        }
        [HttpDelete("{id:int}", Name = "DeleteVillaParkNo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> DeleteVillaParkNo(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var villaParkNumber = await _villaParkNo.GetAsync(x => x.VillaParkNo == id);
                if (villaParkNumber == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return BadRequest(_response);
                }
                await _villaParkNo.RemoveAsync(villaParkNumber);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.ErrorMessage
                    = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPut("{id:int}", Name = "UpdateVillaParkNo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<ApiResponse>> UpdateVillaParkNo(int id, [FromBody] VillaParkNoUpdateDTO updateDto)
        {
            try
            {
                if (updateDto == null || id != updateDto.VillaParkNo)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                //}
                if (await _dbvillaPark.GetAsync(u => u.Id == updateDto.VillaParkID) == null)
                {
                    ModelState.AddModelError("CustomError", "Villa ID is Invalid");
                    return BadRequest(ModelState);
                }
                VillaParkNumber model = _mapper.Map<VillaParkNumber>(updateDto);

                await _villaParkNo.UpdateAsync(model);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.ErrorMessage
                    = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPatch("{id:int}", Name = "UpdateVillaParkNo")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateVillaParkNo(int id, JsonPatchDocument<VillaParkNoUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var villaPark = await _villaParkNo.GetAsync(n => n.VillaParkNo == id, tracked: false);

            VillaParkNoUpdateDTO villaParkNumberUpdateDTO = _mapper.Map<VillaParkNoUpdateDTO>(villaPark);

            if (villaPark == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(villaParkNumberUpdateDTO, ModelState);
            VillaParkNumber model = _mapper.Map<VillaParkNumber>(villaParkNumberUpdateDTO);
            await _villaParkNo.UpdateAsync(model);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();

        }




    }
}
