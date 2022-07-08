using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipeBookRestApi.Models;
using RecipeBookRestApi.Data;
using Microsoft.EntityFrameworkCore;

namespace RecipeBookRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RecipeBookController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RecipeBookController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Возвращает рецепт по id
        /// </summary>
        /// <param name="id">Уникальный идентификатор рецепта</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> GetById(Guid id)
        {
            var result = await _context.Recipes.FindAsync(id);

            return result == null ? new JsonResult(NotFound()) : new JsonResult(Ok(result));
        }

        /// <summary>
        /// Возвращает рецепты по названию
        /// </summary>
        /// <param name="name">Имя рецепта</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetByName(string name)
        {
            var result = _context.Recipes.Where(r => r.Name.ToLower().Contains(name.ToLower()));
            return result == null ? new JsonResult(NotFound()) : new JsonResult(Ok(result));
        }


        /// <summary>
        /// Возвращает все рецепты из БД
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAll()
        {
            var result = _context.Recipes.ToList();

            return new JsonResult(Ok(result));
        }


        /// <summary>
        /// Создаёт новый рецепт
        /// </summary>
        /// <param name="recipe">Новый рецепт в JSON формате</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> Create(Recipe recipe)
        {
            recipe.Id = Guid.NewGuid();

            await _context.Recipes.AddAsync(recipe);
            await _context.SaveChangesAsync();

            return new JsonResult(Ok(recipe));
        }

        /// <summary>
        /// Обновляет рецепт по айди
        /// </summary>
        /// <param name="id">Guid рецепта</param>
        /// <param name="recipe">Обновлённый рецепт в JSON формате</param>
        /// <returns>Ok</returns>
        [HttpPut("{id}")]
        public async Task<JsonResult> Update(Guid id, Recipe recipe)
        {
            if (id != recipe.Id)
            {
                return new JsonResult(BadRequest());
            }

            _context.Entry(recipe).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _context.Recipes.FindAsync(id) == null)
                {
                    return new JsonResult(NotFound());
                }
                else throw;
            }

            return new JsonResult(Ok());

        }

        /// <summary>
        /// Удаляет рецепт из БД по id
        /// </summary>
        /// <param name="id">Уникальный идентификатор рецепта</param>
        /// <returns>Ok or not found</returns>
        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var result = _context.Recipes.FirstOrDefault(r => r.Id == id);

            if (result == null)
                return new JsonResult(NotFound());

            _context.Remove(result);
            await _context.SaveChangesAsync();

            return new JsonResult(Ok());
        }
    }
}
