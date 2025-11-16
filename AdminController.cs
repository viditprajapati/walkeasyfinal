using Microsoft.AspNetCore.Mvc;
using walkeasyfinal.Models;

namespace walkeasyfinal.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly object Session;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("Login", "User");
            }

            // Fetch total customers count
            int customerCount = _context.Registrations.Count();

            // Pass data to the view
            ViewBag.CustomerCount = customerCount;

            int feedbackCount = _context.Feedbacks.Count();

            // Pass data to the view
            ViewBag.FeedbackCount = feedbackCount;
            // Count the shoes from the database
            int shoeCount = _context.Shoes.Count();


            // Pass the shoe count to the view
            ViewBag.ShoeCount = shoeCount;


            return View();
        }
        public IActionResult Index()
        {
            var flowers = _context.Shoes.ToList();
            return View(flowers);
        }
    }
}
