using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RestaurantCollection.WebApi.DataAccess;
//using RestaurantCollection.WebApi.DTO.Common;
using RestaurantCollection.WebApi.DTO.Forms;
//using RestaurantCollection.WebApi.DTO.ViewModels;
using RestaurantCollection.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace RestaurantCollection.WebApi.Controllers
{
    [ApiController]
    [Route("api/")]
    public class RestaurantController : ControllerBase
    {
        private readonly IRepository _repository;

        public RestaurantController(IRepository repository)
        {
            _repository = repository;

        }

        [HttpPost]
        [Route("restaurant")]
        public async Task<ActionResult<Restaurant>> Post([FromBody] CreateForm restaurantForm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var newRestaurant = new Restaurant
            {
                Name = restaurantForm.Name,
                City = restaurantForm.City,
                EstimatedCost = restaurantForm.Cost,
                AverageRating = restaurantForm.Rating,
                Votes = restaurantForm.Votes
            };
            try
            {
                await _repository.AddRestaurant(newRestaurant);

                return Created(uri: $"api/restaurante/{newRestaurant.Id}", newRestaurant);
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }


        [HttpPut]
        [Route("restaurant/{id}")]
        public async Task<ActionResult<Restaurant>> UpdateRestaurant([FromRoute] int id, [FromBody] UpdateForm updateForm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var restaurant = await _repository.GetRestaurantById(id);

            if (restaurant == null)
            {
                return NotFound();
            }

            restaurant.AverageRating = updateForm.Rating;
            restaurant.Votes = updateForm.Votes;

            var updatedRestaurant = await _repository.UpdateRestaurant(id, updateForm);

            return Ok(updatedRestaurant);
        }


        [HttpGet]
        [Route("restaurant")]
        public async Task<ActionResult<List<Restaurant>>> Get()
        {

            var restaurants = await _repository.GetRestaurants();

            return Ok(restaurants);

        }

        [HttpGet("restaurant/query")]
        public async Task<ActionResult<List<Restaurant>>> GetRestaurantsByCity([FromQuery] string city)
        {
            try
            {
                var restaurants = await _repository.GetRestaurants(new RestaurantQueryModel { City = city });
                return Ok(restaurants);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("restaurant/{id}")]
        public async Task<ActionResult<Restaurant>> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var restaurant = await _repository.GetRestaurantById(id);

            if (restaurant == null)
            {
                return NotFound();
            }

            return Ok(restaurant);
        }

        [HttpDelete]
        [Route("restaurant/{id}")]
        public async Task<ActionResult<Restaurant>> DeleteRestaurant(int id)
        {
            var restaurant = await _repository.DeleteRestaurant(id);

            if (restaurant == null)
            {
                return NotFound();
            }

            return NoContent();
        }


        [HttpGet("restaurant/sort")]
        public async Task<ActionResult<List<Restaurant>>> GetSortedRestaurants()
        {
            try
            {
                var sortedRestaurants = await _repository.GetRestaurantsSorted();
                return Ok(sortedRestaurants);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}