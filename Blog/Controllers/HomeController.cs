using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Data;
using Blog.Data.FileManager;
using Blog.Data.Repository;
using Blog.Models;
using Blog.Models.Comments;
using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository _repository;
        private readonly IFileManager _fileManager;

        public HomeController(IRepository repository, IFileManager fileManager)
        {
            _repository = repository;
            _fileManager = fileManager;
        }

        public IActionResult Index(string category)
        {
            var posts = string.IsNullOrEmpty(category)
                ? _repository.GetAllPosts()
                : _repository.GetAllPosts(category);

            return View(posts);
        }

        public IActionResult Post(int id)
        {
            var post = _repository.GetPost(id);
            return View(post);
        }

        [HttpGet("/Image/{image}")]
        public IActionResult Image(string image)
        {
            var mime = image.Substring(image.LastIndexOf('.'));
            return new FileStreamResult(_fileManager.ImageStream(image), $"image/{mime}");
        }

        [HttpPost]
        public async Task<IActionResult> Comment(CommentViewModel commentViewModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Post", new { id = commentViewModel.PostId });
            }

            var post = _repository.GetPost(commentViewModel.PostId);

            if (commentViewModel.MainCommentId == 0)
            {
                post.MainComments = post.MainComments ?? new List<MainComment>();

                post.MainComments.Add(new MainComment
                {
                    Message = commentViewModel.Message,
                    Created = DateTime.Now,
                    UserName = GetCurrentUserName()
                });

                _repository.UpdatePost(post);
            }
            else
            {
                var subComment = new SubComment
                {
                    MainCommentId = commentViewModel.MainCommentId,
                    Message = commentViewModel.Message,
                    UserName = GetCurrentUserName(),
                    Created = DateTime.Now
                };
                _repository.AddSubComment(subComment);

            }

            await _repository.SaveChangesAsync();

            return RedirectToAction("Post", new { id = commentViewModel.PostId });
        }

        public IActionResult ShowPopupForm()
        {
            return View();
        }

        private string GetCurrentUserName()
        {
            return System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').Last();
        }
    }
}
