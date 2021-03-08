using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic; 
using System.Threading.Tasks; 
using RestaurantCollection.WebApi.DTO.Forms;  
using RestaurantCollection.WebApi.Services;
using static Microsoft.AspNetCore.Http.StatusCodes;
using AutoMapper;
using AutoWrapper.Extensions;
using AutoWrapper.Wrappers;
using RestaurantCollection.WebApi.Models;
using RestaurantCollection.WebApi.DataAccess;

namespace RestaurantCollection.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IRepository _restaurantManager;
        private readonly IMapper _mapper;

        public RestaurantController(IRepository restaurantManager, IMapper mapper)
        {
            _restaurantManager = restaurantManager;
            _mapper = mapper;
        }
 

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RestaurantQueryResponse>), Status200OK)]
        public async Task<IEnumerable<RestaurantQueryResponse>> Get()
        {
            var data = await _restaurantManager.GetRestaurants();
            var restaurant = _mapper.Map<IEnumerable<RestaurantQueryResponse>>(data);

            return restaurant;
        }

        [Route("query")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RestaurantQueryResponse>), Status200OK)]
        public async Task<IEnumerable<RestaurantQueryResponse>> GetRestaurant([FromQuery(Name = "city")] string city,
                                                                [FromQuery(Name = "id")] int id)
        { 
            var payload = new RestaurantQueryModel
            {
                City = city,
                Id = id
            };
            var data = await _restaurantManager.GetRestaurants(payload);
            var restaurants = _mapper.Map<IEnumerable<RestaurantQueryResponse>>(data); 
            return restaurants;
        }

        [Route("sort")]
        [HttpGet]
        [ProducesResponseType(typeof(RestaurantQueryResponse), Status200OK)]
        [ProducesResponseType(typeof(RestaurantQueryResponse), Status404NotFound)]
        public async Task<IEnumerable<RestaurantQueryResponse>> GetSort()
        {
            var restaurants = await _restaurantManager.GetRestaurantsSorted();
            return restaurants != null ? _mapper.Map<IEnumerable<RestaurantQueryResponse>>(restaurants)
                                  : throw new ApiProblemDetailsException($"Empty Record.", Status404NotFound);
        }

        [Route("{id:int}")]
        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse), Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), Status422UnprocessableEntity)]
        public async Task<ApiResponse> Put(int id, [FromBody] UpdateForm updateRequest)
        {
            if (!ModelState.IsValid) { throw new ApiProblemDetailsException(ModelState); }

            var restaurant = _mapper.Map<Restaurant>(updateRequest);
            restaurant.Id = id;
            restaurant.AverageRating = updateRequest.Rating;

            var result = await _restaurantManager.UpdateRestaurant(restaurant);
            if (result != null)
            {
                return new ApiResponse($"Record with Id: {id} sucessfully updated.", true);
            }
            else
            {
                throw new ApiProblemDetailsException($"Record with Id: {id} does not exist.", Status404NotFound);
            }
        }

        [Route("{id:int}")]
        [HttpDelete]
        [ProducesResponseType(typeof(ApiResponse), Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), Status404NotFound)]
        public async Task<ApiResponse> Delete(int id)
        {
            if (!ModelState.IsValid) { throw new ApiProblemDetailsException(ModelState); }

            var payload = new Querys
            { 
                id = id
            };

            var restaurant = _mapper.Map<Restaurant>(payload);
            var response = await _restaurantManager.DeleteRestaurant(restaurant);
            if (response != null)
            {
                return new ApiResponse($"Record with Id: {id} sucessfully deleted.", true);
            }
            else
            {
                throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", Status404NotFound);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse), Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), Status422UnprocessableEntity)]
        public async Task<ApiResponse> Post([FromBody] CreateForm createRequest)
        {
            if (!ModelState.IsValid) { throw new ApiProblemDetailsException(ModelState); }

            var restaurant = _mapper.Map<Restaurant>(createRequest);
            restaurant.AverageRating = createRequest.Rating;
            return new ApiResponse("Record successfully created.", await _restaurantManager.AddRestaurant(restaurant), Status201Created);
        }





    }

}
