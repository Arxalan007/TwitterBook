using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cosmonaut;
using Cosmonaut.Extensions;
using TwitterBook.Domain;

namespace TwitterBook.Services
{
    public class CosmosPostService : IPostService
    {
        private readonly ICosmosStore<CosmosPostDto> _cosmosStore;

        public CosmosPostService(ICosmosStore<CosmosPostDto> cosmosStore)
        {
            _cosmosStore = cosmosStore;
        }

        public async Task<List<Post>> GetPostsAsync()
        {
            var posts = await _cosmosStore.Query().ToListAsync();

            return posts.Select(x => new Post { Id = Guid.Parse(x.Id), Name = x.Name }).ToList();
        }

        public async Task<Post> GetPostByIdAsync(Guid postId)
        {
            var cosmosPost = await _cosmosStore.FindAsync(postId.ToString(),postId.ToString());

            return cosmosPost == null ? null : new Post { Id = Guid.Parse(cosmosPost.Id), Name = cosmosPost.Name };
        }

        public async Task<bool> CreatePostAsync(Post postToCreate)
        {
            var cosmosPost = new CosmosPostDto
            {
                Id = Guid.NewGuid().ToString(),
                Name = postToCreate.Name
            };

            var response = _cosmosStore.AddAsync(cosmosPost);
            postToCreate.Id = Guid.Parse(cosmosPost.Id);
            return response.IsCompletedSuccessfully;
        }

        public async Task<bool> UpdatePostAsync(Post postToUpdate)
        {
            var cosmosPost = new CosmosPostDto
            {
                Id = postToUpdate.Id.ToString(),
                Name = postToUpdate.Name
            };

            var response = _cosmosStore.UpdateAsync(cosmosPost);
            return response.IsCompletedSuccessfully;
        }

        public async Task<bool> DeletePostAsync(Guid postId)
        {
            var reponse = await _cosmosStore.RemoveByIdAsync(postId.ToString(), postId.ToString());
            return reponse.IsSuccess;
        }
    }
}