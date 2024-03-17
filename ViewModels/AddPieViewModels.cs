using webapplication2.Models;

namespace webapplication2.ViewModels
{
    public class AddPieViewModels
    {
        public string Name { get; set; } = string.Empty;
        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }


        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }

        public string? ImageThumbnailUrl { get; set; }

        public Category Category { get; set; } = default!;
    }
}
