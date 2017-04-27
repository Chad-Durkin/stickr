using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stickr.Models;
using Microsoft.AspNetCore.Identity;
using Stickr.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Stickr.Controllers
{
    public class ImageController : Controller
    {
        private readonly StickrDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public ImageController(UserManager<ApplicationUser> userManager, StickrDbContext db)
        {
            _db = db;
            _userManager = userManager;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Image image)
        {
            Image newImage = image;
            var id = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentAccount = _db.Accounts
                .Include(account => account.User.Id)
                .FirstOrDefault(account => account.User.Id == id);
            newImage.Account = currentAccount;
            _db.Images.Add(newImage);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
