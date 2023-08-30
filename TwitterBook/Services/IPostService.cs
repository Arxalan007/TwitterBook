using System;
using System.Collections.Generic;
using TwitterBook.Domain;

namespace TwitterBook.Services
{
    public interface IPostService
    {
        List<Post> GetPosts();

        Post GetPostById(Guid postId);
        
        bool UpdatePost(Post postToUpdate);
    }
}