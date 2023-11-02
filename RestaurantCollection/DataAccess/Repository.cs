using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestaurantCollection.WebApi.DTO.Forms;
using RestaurantCollection.WebApi.Models;

namespace RestaurantCollection.WebApi.DataAccess
{
    /// <summary>
    /// Repository
    /// </summary>
    public class Repository : IRepository
    {
        private readonly RestaurantCollectionContext _context;

        public Repository(RestaurantCollectionContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<List<Restaurant>> GetRestaurants()
        {
            IQueryable<Restaurant> restaurants = _context.Restaurants;
            // return await restaurants.OrderByDescending(o => o.Id).ToListAsync();
            return await restaurants.OrderBy(o => o.Id).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Restaurant> AddRestaurant(Restaurant restaurant)
        {
            await _context.Restaurants.AddAsync(restaurant);
            await _context.SaveChangesAsync();

            return restaurant;
        }

        public async Task<Restaurant> DeleteRestaurant(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();

            return restaurant;
        }

        public async Task<List<Restaurant>> GetRestaurantsSorted()
        {
            IQueryable<Restaurant> restaurants = _context.Restaurants;
            return await restaurants.OrderByDescending(o => o.AverageRating).ToListAsync();
        }

        public async Task<List<Restaurant>> GetRestaurants(RestaurantQueryModel query)
        {
            IQueryable<Restaurant> restaurants = _context.Restaurants;
            if (query.City != null)
            {
                restaurants = restaurants.Where(o => o.City == query.City);
            }

            if (query.Id > 0)
            {
                restaurants = restaurants.Where(o => o.Id == query.Id);
            }
            return await restaurants.OrderBy(o => o.Id).ToListAsync();
        }

        public async Task<Restaurant> UpdateRestaurant(int id, UpdateForm updateForm)
        {
            var rest = await _context.Restaurants.FindAsync(id);
            rest.AverageRating=updateForm.Rating;
            rest.Votes = updateForm.Votes;
            _context.Restaurants.Update(rest);
            await _context.SaveChangesAsync();

            return rest;
        }

        public async Task<Restaurant> GetRestaurantById(int id)
        {
            return await _context.Restaurants.FirstOrDefaultAsync(r => r.Id == id);
        }

    }
}
