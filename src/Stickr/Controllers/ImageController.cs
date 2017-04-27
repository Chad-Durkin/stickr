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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Stickr.Controllers
{
    [Authorize]
    public class ImageController : Controller
    {
        private readonly StickrDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private IHostingEnvironment _environment;
        public ImageController(UserManager<ApplicationUser> userManager, StickrDbContext db, IHostingEnvironment environment)
        {
            _environment = environment;
            _db = db;
            _userManager = userManager;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentAccount = _db.Accounts
                .Include(account => account.Images)
                .Include(account => account.User)
                .FirstOrDefault(account => account.User.Id == userId);
            return View(currentAccount);
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

        public IActionResult Upload()
        {
            ViewBag.Title = "";
            ViewBag.Description = "";
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> Upload(ICollection<IFormFile> files)
        {
            string newTitle = this.Request.Form["ImageTitle"];
            string newDescription = this.Request.Form["Description"];

            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentAccount = _db.Accounts
                .Include(accounts => accounts.User)
                .FirstOrDefault(accounts => accounts.User.Id == userId);

            var uploads = Path.Combine(_environment.WebRootPath, "Images");
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                        var newImage = new Image();
                        newImage.Title = newTitle;
                        newImage.Account = currentAccount;
                        newImage.ImageUrl = "/Images/" + file.FileName;
                        _db.Images.Add(newImage);
                        _db.SaveChanges();
                    }
                }
            }
            return RedirectToAction("Index");
        }
    }
}
