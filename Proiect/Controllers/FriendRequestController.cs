using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proiect.Data;
using Proiect.DataModels;
using Proiect.DataModels.DTO;

namespace Proiect.Controllers
{
    public class FriendRequestController : Controller
    {
        private readonly SocialDbContext context;

        public FriendRequestController(SocialDbContext context)
        {
            this.context = context;
        }
        [HttpGet("GetFriendRequest")]
        public IActionResult Index(int userId)
        {
            try
            {
               var request=context.FriendRequests
                    .Include(r=>r.Sender)
                    .Where(r=>r.ReciverId == userId)
                    .Select(r=>new RequestDTO
                    {
                        ReciverId=r.ReciverId,
                        SenderId=r.SenderId,
                        SenderName=r.Sender.Name,
                        SenderEmail=r.Sender.Email
                    })
                    .ToList<RequestDTO>();
                return Ok(request);
            }
            catch (Exception e) { return StatusCode(500, "A aparut o eroare la server!"); }
        }
        [HttpPost("Sent")]
        public IActionResult Sent(int userId,int receiverId)
        {
            try
            {
                var friendRequest = new FriendRequest
                {
                    SenderId = userId,
                    ReciverId = receiverId,
                    Date = DateTime.Now
                };
                context.FriendRequests.Add(friendRequest);
                context.SaveChanges();
                return Ok(friendRequest);

            }
            catch(Exception ex) { return StatusCode(500, "A aparut o eroare la server!"); }
        }
        [HttpPost ("Accept")]
        public IActionResult Accept(int senderId,int receiverId)
        {
            try
            {
                var request=context.FriendRequests.SingleOrDefault(r=>r.SenderId == senderId && r.ReciverId==receiverId);
                if (request == null)
                {
                    return BadRequest();
                }
                var friendship = new Friendship
                {
                    User1Id = request.ReciverId,
                    User2Id = request.SenderId,
                    Date = DateTime.Now
                };
                var friendship2 = new Friendship
                {
                    User1Id = request.SenderId,
                    User2Id = request.ReciverId,
                    Date = DateTime.Now
                };
                context.Friendships.Add(friendship);
                context.Friendships.Add(friendship2);
                context.FriendRequests.Remove(request);
                context.SaveChanges();
                return Ok();
            }
            catch(Exception e) { return StatusCode(500, "A aparut o eroare la server!"); }
        }
        [HttpDelete("Reject")]
        public IActionResult Reject(int  senderId,int receiverId)
        {
            try
            {
                var request = context.FriendRequests.SingleOrDefault(r => r.SenderId == senderId && r.ReciverId == receiverId);
                if(request == null)
                {
                    return BadRequest();
                }
                context.FriendRequests.Remove(request);
                context.SaveChanges();
                return Ok();
            }
            catch(Exception e) { return StatusCode(500, "A aparut o eroare la server!"); }
        }
    }
}
