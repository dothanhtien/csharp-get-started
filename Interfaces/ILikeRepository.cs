using CSharpGetStarted.DTOs;
using CSharpGetStarted.Entities;
using CSharpGetStarted.Helpers;

namespace CSharpGetStarted.Interfaces
{
    public interface ILikeRepository
    {
        Task<UserLike> GetUserLike(int sourceUserId, int targetUserId);
        Task<AppUser> GetUserWithLikes(int userId);
        Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams);
    }
}
