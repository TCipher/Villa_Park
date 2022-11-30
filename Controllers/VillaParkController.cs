using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Net;
using VilllaParks.Core.IRepository;
using VilllaParks.Model;
using VilllaParks.Model.Dto;


namespace VilllaParks.Controllers
{
    //Api attribute
    [Route("api/VillaPark")]
    [ApiController]
    public class VillaParkController: ControllerBase
    {
        private readonly IVillaParkRepository _villaPark;
        private readonly IMapper _mapper;
        protected ApiResponse _response;

        public VillaParkController(IVillaParkRepository villaPark,IMapper mapper)
        {
            _villaPark = villaPark;
            _mapper = mapper;
            _response = new();
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetVillaPArks()
        {
            try
            {
                //Get All VillaPArks
                IEnumerable<VillaPark> ParkList = await _villaPark.GetAllAsync();
                _response.Result = _mapper.Map<List<VillaParkDto>>(ParkList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage 
                    = new List<string> { ex.ToString() };
            }
            return _response;
        }
    }
}
