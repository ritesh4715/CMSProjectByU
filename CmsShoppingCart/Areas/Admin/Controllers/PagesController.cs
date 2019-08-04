using CmsShoppingCart.Models.Data;
using CmsShoppingCart.Models.ViewModel.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CmsShoppingCart.Areas.Dashboard.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            //Declare list of pagevm
            List<PageVM> PageList;
            using (Db db = new Db())
            {
                //Init list
                PageList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();
            }
            //Return view with list
            return View(PageList);
        }
        // GET: Admin/Pages/AddPage
        public ActionResult AddPage()
        {
            return View();
        }
        // Post: Admin/Pages/AddPage
        [HttpPost]
        public ActionResult AddPage(PageVM model)
        {
            //check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (Db db = new Db())
            {
                //Declare slug
                string slug;
                //Init PageDTO
                PageDTO dto = new PageDTO();
                //DTO title
                dto.Title = model.Title;
                //Check for and set slug if need be
                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }
                //Make sure title and slug are unique
                if (db.Pages.Any(x => x.Title == model.Title) || db.Pages.Any(x => x.Slug == model.Slug))
                {
                    ModelState.AddModelError("", "The title or slug alerady exisists.");
                    return View(model);
                }
                //DTO the rest
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
                dto.Sorting = 100;
                //Save DTO
                db.Pages.Add(dto);
                db.SaveChanges();
            }
            //Set TempData message
            TempData["SM"] = "You have added a new page.";
            //Redirect
            return RedirectToAction("AddPage");
        }
        // GET: Admin/Pages/EditPage/id
        public ActionResult EditPage(int id)
        {
            //Declare PageVM
            PageVM model;
            using (Db db = new Db())
            {
                //Get the page
                PageDTO dto = db.Pages.Find(id);
                //Confirm page exisists
                if (dto == null)
                {
                    return Content("The page does not exisists.");
                }
                //Init PageVM
                model = new PageVM(dto);
            }
            //Return view with model
            return View(model);
        }
        // Post: Admin/Pages/AddPage
        [HttpPost]
        public ActionResult EditPage(PageVM model)
        {
            //check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (Db db = new Db())
            {
                //Get page id
                int id = model.Id;
                //Declare slug
                string slug;
                //Init PageDTO
                PageDTO dto = db.Pages.Find(id);
                //DTO title
                dto.Title = model.Title;
                //Check for and set slug if need be
                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }
                //Make sure title and slug are unique
                if (db.Pages.Where(x => x.Id != id).Any(x => x.Title == model.Title) ||
                    db.Pages.Where(x => x.Id != id).Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "The title or slug alerady exisists.");
                    return View(model);
                }
                //DTO the rest
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
                dto.Sorting = 100;
                //Save DTO
                db.SaveChanges();
            }
            //Set TempData message
            TempData["SM"] = "You have edited the page!";
            //Redirect
            return RedirectToAction("EditPage");
        }
        // GET: Admin/Pages/DetailPage/id
        public ActionResult PageDetails(int id)
        {
            //Declare PageVM
            PageVM model;
            using (Db db = new Db())
            {
                //Get the page
                PageDTO dto = db.Pages.Find(id);
                //Confirm page exisists
                if (dto == null)
                {
                    return Content("The page does not exisists.");
                }
                //Init PageVM
                model = new PageVM(dto);
            }
            //Return view with model
            return View(model);
        }
        // Post: Admin/Pages/DeletePage/id
        public ActionResult DeletePage(int id)
        {
            using (Db db = new Db())
            {
                //Get the page
                PageDTO dto = db.Pages.Find(id);
                //Remove the page
                db.Pages.Remove(dto);
                //Save
                db.SaveChanges();
            }
            //Redirect
            return RedirectToAction("Index");
        }
        public ActionResult ReorderPages(int[] id)
        {
            using (Db db = new Db())
            {
                //set initial count
                int count = 0;

                //Declare PageDTO
                PageDTO dto;

                //Set sorting for each pages
                foreach (var item in id)
                {
                    dto = db.Pages.Find(item);
                    dto.Sorting = count;

                    db.SaveChanges();
                    count++;
                }
            }
            return View();
        }
    }
}