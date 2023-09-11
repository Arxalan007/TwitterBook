using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TwitterBook.Domain;

namespace TwitterBook.Services
{
    public interface IPostService
    {
        Task<List<Post>> GetPostsAsync();

        Task<Post> GetPostByIdAsync(Guid postId);

        Task<bool> CreatePostAsync(Post postToCreate);
        
        Task<bool> UpdatePostAsync(Post postToUpdate);
        
        Task<bool> DeletePostAsync(Guid postId);
        Task<bool> UserOwnsPostAsync(Guid postId, string userId);
        // Task<List<Tag>> GetAllTagsAsync();
    }
}