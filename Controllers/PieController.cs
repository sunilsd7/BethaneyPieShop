
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BethanysPieShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapplication2.Models;
using webapplication2.ViewModels;

namespace webapplication2.Controllers
{
    public class PieController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private readonly IPieRepository _pieRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly BethanysPieShopDbContext dbContext;

        public PieController(IPieRepository pieRepository, ICategoryRepository categoryRepository, BethanysPieShopDbContext dbContext)
        {
            _pieRepository = pieRepository;
            _categoryRepository = categoryRepository;
            this.dbContext = dbContext;
        }

        public IActionResult List()
        {

            PieListViewModel piesListViewModel = new PieListViewModel(_pieRepository.AllPies, "Cheese cakes");
            return View(piesListViewModel);
        }
        public IActionResult Details(int id)
        {
            var pie = _pieRepository.GetPieById(id);
            if (pie == null)
                return NotFound();

            return View(pie);
        }

        public IActionResult AddPie()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddPie(AddPieViewModels viewModal)
        {
            var categoryName = viewModal.Category?.CategoryName;
            if (string.IsNullOrEmpty(categoryName))
            {

                return BadRequest("Category name is required.");
            }
            var category = await dbContext.Categories.FirstOrDefaultAsync(c => c.CategoryName == categoryName);
            if (category == null)
            {
                return NotFound($"Category '{categoryName}' not found.");
            }

            var pie = new Pie
            {
                Name = viewModal.Name,
                ShortDescription = viewModal.ShortDescription,
                LongDescription = viewModal.LongDescription,
                AllergyInformation = "",
                Price = 15.99M,
                ImageUrl = viewModal.ImageUrl,
                ImageThumbnailUrl = viewModal.ImageThumbnailUrl,
                IsPieOfTheWeek = true,
                InStock = false,
                CategoryId = category.CategoryId
            };
            await dbContext.Pies.AddAsync(pie);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("List", "Pie");

        }
        [HttpGet]

        public async Task<IActionResult> Update(int id)
        {
            var pie = await dbContext.Pies.FindAsync(id);
            return View(pie);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Pie viewModel)
        {
            var pie = await dbContext.Pies.FindAsync(viewModel.PieId);

            if (pie == null)

            {
                return NotFound();
            }

            pie.Name = viewModel.Name;
            pie.Price = viewModel.Price;
            pie.ShortDescription = viewModel.ShortDescription;
            pie.LongDescription = viewModel.LongDescription;
            pie.ImageThumbnailUrl = viewModel.ImageThumbnailUrl;
            pie.ImageUrl = viewModel.ImageUrl;

            await dbContext.SaveChangesAsync();

            return RedirectToAction("List", "Pie");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Pie viewModel, int id)
        {
            var pie = await dbContext.Pies.AsNoTracking().FirstOrDefaultAsync(c => c.PieId == viewModel.PieId);
            if (pie == null)
            {
                return NotFound();

            }
            dbContext.Pies.Remove(viewModel);

            await dbContext.SaveChangesAsync();

            return RedirectToAction("List", "Pie");
        }


    }
}