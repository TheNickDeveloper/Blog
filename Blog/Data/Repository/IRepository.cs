using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Models;
using Blog.Models.Comments;

namespace Blog.Data.Repository
{
    public interface IRepository
    {
        List<Post> GetAllPosts();
        List<Post> GetAllPosts(string category);
        Post GetPost(int id);
        void RemovePost(int id);
        void UpdatePost(Post post);
        void AddPost(Post post);
        void AddSubComment(SubComment subComment);

        Task<bool> SaveChangesAsync();
    }
}