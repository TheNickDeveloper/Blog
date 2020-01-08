using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Data;
using Blog.Data.Repository;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository _repository;

        public HomeController(IRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            var posts = _repository.GetAllPosts();
            return View(posts);
        }

        public IActionResult Post(int id)
        {
            var post = _repository.GetPost(id);
            return View(post);
        }

    }
}
