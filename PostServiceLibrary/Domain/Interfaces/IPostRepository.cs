using PostServiceLibrary.Application.Requests;
using PostServiceLibrary.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostServiceLibrary.Domain.Interfaces
{
    public interface IPostRepository
    {
        Task<Post> GetPostByIdAsync(string id);
        Task<IEnumerable<Post>> GetAllPostsAsync();
        Task<IEnumerable<Post>> GetAllUserPostAsync(string id);
        Task UpdatePostAsync(string id, UpdatePostDTO updatedPost);
        Task DeletePostAsync(string id);
    }
}
