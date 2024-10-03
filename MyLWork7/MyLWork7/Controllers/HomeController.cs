using MyLWork7.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using MyLWork7.Models.Domain;
using Microsoft.EntityFrameworkCore;
using labwork7.Data;
using Tag = MyLWork7.Models.Domain.Tag;
using MyLWork7.Models.ViewModels;

namespace MyLWork7.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BloggieDbContext bloggieDbContext;
        private readonly List<BlogPost> users;
        private readonly List<Tag> tags;

        public HomeController(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
            this.users = new List<BlogPost>();
            this.tags = new List<Tag>();
        }

        public IActionResult GetDataByTagId(int tagId)
        {
            var tag = bloggieDbContext.Tags.ToList();
            ViewBag.Tag = tag;
            var blogPosts = bloggieDbContext.BlogPosts.Where(e => e.TagId == tagId).ToList();
            return View("Index", blogPosts);
        }

        public IActionResult GetDataByTagIdJson(int tagId)
        {
            var blogPosts = bloggieDbContext.BlogPosts.Where(e => e.TagId == tagId).ToList();
            return Json(blogPosts);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var tag = bloggieDbContext.Tags.ToList();
            ViewBag.Tag = tag;
            var blogPosts = bloggieDbContext.BlogPosts.ToList();
            ViewBag.BlogPosts = blogPosts;
            return View(new AddBlogPostRequest());
        }

        [HttpPost]
        public IActionResult Index(AddBlogPostRequest addBlogPostRequest)
        {
            if (ModelState.IsValid)
            {
                var blogPost = new BlogPost
                {
                    Id = Guid.NewGuid(),
                    Heading = addBlogPostRequest.Heading,
                    PageTitle = addBlogPostRequest.PageTitle,
                    Content = addBlogPostRequest.Content,
                    ShortDescription = addBlogPostRequest.ShortDescription,
                    FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
                    UrlHandle = addBlogPostRequest.UrlHandle,
                    PublishedDate = addBlogPostRequest.PublishedDate,
                    Author = addBlogPostRequest.Author,
                    Visible = addBlogPostRequest.Visible,
                    TagId = addBlogPostRequest.TagId
                };

                bloggieDbContext.BlogPosts.Add(blogPost);
                bloggieDbContext.SaveChanges();

                TempData["SuccessMessage"] = "Blog post added successfully!";
                return RedirectToAction("Index");
            }

            ViewBag.BlogPosts = bloggieDbContext.BlogPosts.ToList();
            return View(addBlogPostRequest);
        }

        [HttpGet("/api/users")]
        public IActionResult GetUsers()
        {
            return Json(users);
        }

        [HttpGet("/api/users/{id}")]
        public IActionResult GetUser(string id)
        {
            var blogPost = bloggieDbContext.BlogPosts.FirstOrDefault(e => e.TagId.ToString() == id);
            var blogPost2 = users.FirstOrDefault(u => u.Id.ToString() == id);

            if (blogPost == null)
                return NotFound(new { message = "Пост не найден" });

            return Json(blogPost2);
        }

        [HttpPost("/api/users")]
        public IActionResult CreateUser([FromBody] BlogPost user)
        {
            var blogPost = new BlogPost
            {
                Heading = user.Heading,
                PageTitle = user.PageTitle,
                ShortDescription = user.ShortDescription,
                FeaturedImageUrl = user.FeaturedImageUrl,
                UrlHandle = user.UrlHandle,
                PublishedDate = user.PublishedDate,
                Author = user.Author,
                TagId = user.TagId,
                Content = user.Content,
            };

            bloggieDbContext.BlogPosts.Add(blogPost);
            bloggieDbContext.SaveChanges();
            users.Add(user);

            return Json(user);
        }

        [HttpPost]
        public IActionResult AddTag(MyLWork7.Models.Domain.Tag tag)
        {
            if (ModelState.IsValid)
            {
                bloggieDbContext.Tags.Add(tag);
                bloggieDbContext.SaveChanges();
                return Json(tag);
            }

            return BadRequest("Ошибка при добавлении поста");
        }

        public ActionResult group()
        {
            return View("add");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}