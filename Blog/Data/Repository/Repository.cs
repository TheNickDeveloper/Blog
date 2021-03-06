﻿using Blog.Models;
using Blog.Models.Comments;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.Repository
{
    public class Repository : IRepository
    {
        private readonly AppDbContext _ctx;

        public Repository(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public Post GetPost(int id)
        {
            return _ctx.Posts
                .Include(p=>p.MainComments)
                .ThenInclude(sp=>sp.SubComments)
                .FirstOrDefault(x => x.Id == id);
        }

        public List<Post> GetAllPosts()
        {
            return _ctx.Posts.ToList();
        }

        public List<Post> GetAllPosts(string category)
        {
            return _ctx.Posts.Where(x => x.Category.ToLower() == category.ToLower()).ToList();
        }

        public void RemovePost(int id)
        {
            _ctx.Posts.Remove(GetPost(id));
        }

        public void UpdatePost(Post post)
        {
            _ctx.Posts.Update(post);
        }

        public void AddPost(Post post)
        {
            _ctx.Posts.Add(post);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _ctx.SaveChangesAsync() > 0;
        }

        public void AddSubComment(SubComment subComment)
        {
            _ctx.SubComments.Add(subComment);
        }
    }
}
