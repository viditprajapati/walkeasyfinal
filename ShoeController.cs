using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using walkeasyfinal.Models;
using static NuGet.Packaging.PackagingConstants;

namespace walkeasyfinal.Controllers
{
    public class ShoeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ShoeController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: AddShoe
        [HttpGet]
        public IActionResult AddShoe()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddShoe(Shoes model)
        {
            if (ModelState.IsValid)
            {
                // Check if an image was uploaded
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_environment.WebRootPath, "images");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder); // Ensure folder exists
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ImageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(fileStream);
                    }

                    // Store as: images/filename.ext
                    model.Image = Path.Combine("images", uniqueFileName).Replace("\\", "/");
                }
                else
                {
                    model.Image = null; // Optional image
                }

                _context.Shoes.Add(model);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Shoes added successfully!";
                return RedirectToAction("AddShoe");
            }

            return View(model); // If model state invalid, show form again
        }
        // View all shoes
        public IActionResult Index()
        {
            var shoes = _context.Shoes.ToList();
            return View(shoes);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var shoes = _context.Shoes.FirstOrDefault(f => f.Id == id);
            if (shoes == null)
            {
                return NotFound();
            }

            return View(shoes);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id,Shoes model)
        {
            if (id != model.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                var shoes = await _context.Shoes.FindAsync(id);
                if (shoes == null)
                    return NotFound();

                // Update other fields
                shoes.ShoesName = model.ShoesName;
                shoes.Category = model.Category;
                shoes.Amount = model.Amount;
                shoes.Description = model.Description;

                // Handle image upload
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    // Delete old image if exists
                    if (!string.IsNullOrEmpty(shoes.Image))
                    {
                        string oldImagePath = Path.Combine(_environment.WebRootPath, shoes.Image);
                        if (System.IO.File.Exists(oldImagePath))
                            System.IO.File.Delete(oldImagePath);
                    }

                    // Save new image
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ImageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }

                    shoes.Image = Path.Combine("images", uniqueFileName).Replace("\\", "/");
                }
                else
                {
                    // Keep old image
                    shoes.Image = model.Image;
                }

                await _context.SaveChangesAsync();
                TempData["Message"] = "Shoes updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }



        [HttpGet]
        public IActionResult Delete(int id)
        {
            var shoes = _context.Shoes.FirstOrDefault(f => f.Id == id);
            if (shoes == null)
            {
                return NotFound();
            }

            return View(shoes);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shoes = await _context.Shoes.FindAsync(id);
            if (shoes == null)
            {
                return NotFound();
            }

            _context.Shoes.Remove(shoes);
            await _context.SaveChangesAsync();
            TempData["Message"] = "shoes deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
