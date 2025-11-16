using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using walkeasyfinal.Models;
using walkeasyfinal.Models;




namespace walkeasyfinal.Controllers
    {
        public class HomeController : Controller
        {
            private readonly ILogger<HomeController> _logger;
            private readonly AppDbContext _context;

            public HomeController(ILogger<HomeController> logger, AppDbContext context)
            {
                _logger = logger;
                _context = context;
            }
        [HttpGet]
        public JsonResult SearchSuggestions(string term)
        {
            var suggestions = _context.Shoes
                .Where(s => s.ShoesName.Contains(term))
                .Select(s => new {
                    label = s.ShoesName,
                    value = s.Id
                })
                .Take(5)
                .ToList();

            return Json(suggestions);
        }
        public IActionResult Display(int id)
        {
            var flower = _context.Shoes
                .Where(f => f.Id == id)
                .Select(f => new ShoeViewModel
                {
                    Id = f.Id,
                   ShoesName = f.ShoesName,
                    Image = f.Image,
                    Amount = f.Amount,
                    Category = f.Category,
                    Description = f.Description
                })
                .FirstOrDefault();

            if (flower == null)
            {
                return NotFound();
            }

            return View(flower);
        }

        [HttpGet]
        public IActionResult EnterQuantity()
        {
            ViewBag.ShoesId = TempData["ShoesId"];
            ViewBag.ShoesName = TempData["ShoesName"];
            ViewBag.Category = TempData["Category"];
            ViewBag.Amount = TempData["Amount"]; // Stored as "Rs.500" or similar
            return View();
        }
        public IActionResult Home()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")))
            {
                return RedirectToAction("Login", "User");
            }

            var shoes = _context.Shoes
                .Select(s => new ShoeViewModel
                {
                    Id = s.Id,
                    ShoesName = s.ShoesName,
                    Image = s.Image,
                    Amount = s.Amount
                })
                .ToList();

            return View(shoes);
        }
        [HttpPost]
        public IActionResult GenerateBill(string ShoesName, string Category, string Amount, int Quantity)
        {
            var user = _context.Registrations.FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            string numericAmount = new string(Amount.Where(c => char.IsDigit(c) || c == '.').ToArray());
            decimal parsedAmount = decimal.TryParse(numericAmount, NumberStyles.Any, CultureInfo.InvariantCulture, out var amt) ? amt : 0;

            decimal totalAmount = parsedAmount * Quantity;

            var bill = new BillViewModel
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Address = user.Address,
                MobileNo = user.Mobilenumber,
                Gender = user.Gender,

                ShoesName = ShoesName,
                Category = Category,
                Quantity = Quantity,
                Amount = totalAmount
            };

            ViewBag.OriginalAmount = Amount; // For display
            return View("GenerateBill", bill); // Make sure this view name matches
        }


        public IActionResult Index()
            {
            var registration = _context.Registrations.ToList();
                return View(registration);
            }

            [HttpGet]
            public IActionResult Create()
            {
                return View();
            }

            [HttpPost]
            public IActionResult Create(Registration registration)
            {
                try
                {
                    // Check if email already exists
                    if (_context.Registrations.Any(u => u.Email == registration.Email))
                    {
                        ModelState.AddModelError("Email", "Email already exists.");
                    }

                    if (!ModelState.IsValid)
                    {
                        foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                        {
                            Console.WriteLine(error.ErrorMessage);
                        }
                        return View(registration);
                    }

                    _context.Registrations.Add(registration);
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Registered successfully!";
                    return RedirectToAction("Login", "User");
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine($"DbUpdateException: {ex.Message}");
                    Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
                    ModelState.AddModelError("", "An error occurred while saving. Please try again.");
                    return View(registration);
                }
            }
    

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

      
        public IActionResult BuyNow(int id)
        {
            var shoe = _context.Shoes.FirstOrDefault(f => f.Id == id);
            if (shoe == null) return NotFound();

            TempData["ShoesId"] = shoe.Id;
            TempData["ShoesName"] = shoe.ShoesName;
            TempData["Amount"] = shoe.Amount; // e.g., "Rs.500"
            TempData["Category"] = shoe.Category;

            return RedirectToAction("EnterQuantity");
        }

        public IActionResult List()
        {
            var shoes = _context.Shoes.ToList();
            return View(shoes);
        }

        public IActionResult Welcome()
        {
            return View();
        }

    }
}
