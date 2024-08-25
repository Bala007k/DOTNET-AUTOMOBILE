using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Data;
using Microsoft.Extensions.Hosting.Internal;
using TopSpeedAutomobile.Data;
using TopSpeedAutomobile.Migrations;
using TopSpeedAutomobile.Models;
using Microsoft.Identity.Client;
using Microsoft.EntityFrameworkCore;

namespace TopSpeedAutomobile.Controllers
{

    public class BrandController : Controller

    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BrandController(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<Brand> brands = _dbContext.Brand.ToList();
            return View(brands);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Brand brand)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            var file = HttpContext.Request.Form.Files;
            if (file.Count > 0)
            {
                string newFileName = Guid.NewGuid().ToString();
                var upload = Path.Combine(webRootPath, @"images\brand");
                var extension = Path.GetExtension(file[0].FileName);
                using (var fileStream = new FileStream(Path.Combine(upload, newFileName + extension), FileMode.Create))
                {
                    file[0].CopyTo(fileStream);
                }
                brand.Brandlogo = @"\images\brand\" + newFileName + extension;
            }

            if (ModelState.IsValid)
            {
                _dbContext.Brand.Add(brand);
                _dbContext.SaveChanges();
                TempData["success"] = "Record Created Successfully";
                return RedirectToAction(nameof(Index));



            }
            return View();



        }
        [HttpGet]
        public IActionResult Details(Guid id)
        {
            Brand brand = _dbContext.Brand.FirstOrDefault(x => x.Id == id);
            return View(brand);
        }
        [HttpGet]
        public IActionResult Edit(Guid id) { Brand brand = _dbContext.Brand.FirstOrDefault(x => x.Id == id); return View(brand); }
        [HttpPost]
        public IActionResult Edit(Brand brand)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            var file = HttpContext.Request.Form.Files;
            if (file.Count > 0)
            {
                string newFileName = Guid.NewGuid().ToString();
                var upload = Path.Combine(webRootPath, @"images\brand");
                var extension = Path.GetExtension(file[0].FileName);
                var objfromdb = _dbContext.Brand.AsNoTracking().FirstOrDefault(x => x.Id == brand.Id);

                var oldimgpath = Path.Combine(webRootPath);

                if (System.IO.File.Exists(oldimgpath))
                {
                    oldimgpath = Path.Combine(webRootPath, objfromdb.Brandlogo.Trim('\\'));
                    System.IO.File.Delete(oldimgpath);

                }
                using (var fileStream = new FileStream(Path.Combine(upload, newFileName + extension), FileMode.Create))
                {
                    file[0].CopyTo(fileStream);
                }
                brand.Brandlogo = @"\images\brand\" + newFileName + extension;
            }

            if (ModelState.IsValid)
            {
                var objfromdb = _dbContext.Brand.AsNoTracking().FirstOrDefault(x => x.Id == brand.Id);
                objfromdb.Name = brand.Name;
                objfromdb.EstablishedYear = brand.EstablishedYear;
                if (brand.Brandlogo != null)
                {
                    objfromdb.Brandlogo = brand.Brandlogo;
                }
                _dbContext.Brand.Update(objfromdb);
                _dbContext.SaveChanges();
                TempData["warning"] = "Updated";
                return RedirectToAction(nameof(Index));

            }
            return View();

        }
        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            Brand brand = _dbContext.Brand.FirstOrDefault(x => x.Id == id);
            return View(brand);

        }
        [HttpPost]
        public IActionResult Delete(Brand brand)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            if (string.IsNullOrEmpty(brand.Brandlogo))
            {

                var objfromdb = _dbContext.Brand.AsNoTracking().FirstOrDefault(x => x.Id == brand.Id);

                var oldimgpath = Path.Combine(webRootPath);

                if (System.IO.File.Exists(oldimgpath))
                {
                    oldimgpath = Path.Combine(webRootPath, objfromdb.Brandlogo.Trim('\\'));
                    System.IO.File.Delete(oldimgpath);

                }

            }
            _dbContext.Brand.Remove(brand);
            _dbContext.SaveChanges();
            TempData["erase"] = "Deleted";
            return RedirectToAction(nameof(Index));


        }
    }

}

