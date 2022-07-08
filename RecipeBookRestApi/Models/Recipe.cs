    namespace RecipeBookRestApi.Models
{
    public class Recipe
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? IngredientCount { get; set; }
        public int? NumberOfServings { get; set; }
        public string? RecipeText { get; set; }
        public string Author { get; set; }
    }
}
