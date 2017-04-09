using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SilverLinkAPI.DAL;
using SilverLinkAPI.Models;

namespace SilverLinkAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Friends")]
    public class FriendsController : ApiController
    {
        private readonly ApplicationDbContext db;
        private readonly ApplicationUserManager manager;

        public FriendsController()
        {
            db = new ApplicationDbContext();
            manager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
        }

        // Get Friend relationship from friendId
        // GET: api/Friends/{friendId}
        [Route("{friendId}")]
        [ResponseType(typeof(Friend))]
        public async Task<IHttpActionResult> GetFriend(int friendId)
        {
            var user = User.Identity.GetUserId();

            var friend = await db.Friends
                .Where(f => f.Id == friendId)
                .Include(f => f.User)
                .Include(f => f.UserFriend)
                .FirstOrDefaultAsync();

            // Replace user with friend
            if (friend.UserId1 == user)
                friend.User = friend.UserFriend;

            // Add latest message
            await db.Entry(friend)
                .Collection(f => f.Messages)
                .Query().OrderByDescending(m => m.SentAt).Take(1)
                .LoadAsync();

            return Ok(friend);
        }

        // GET: api/Friends
        public IEnumerable<Friend> GetFriends()
        {
            return GetFriends(false);
        }

        // Get friends of user
        public IEnumerable<Friend> GetFriends(bool recent)
        {
            var user = User.Identity.GetUserId();

            // Get friend relationships where user is in
            var friends = db.Friends
                .Where(f => f.UserId1 == user
                            || f.UserId2 == user)
                .Where(f => f.AcceptedAt != null)
                .Include(f => f.User)
                .Include(f => f.UserFriend)
                .ToList();

            // Replace user with friend and add latest message
            foreach (var f in friends)
            {
                if (f.UserId1 == user)
                    f.User = f.UserFriend;
                db.Entry(f)
                    .Collection(g => g.Messages)
                    .Query().OrderByDescending(m => m.SentAt).Take(1)
                    .Load();
            }

            // Sort
            List<Friend> sortedFriends;
            if (recent)
                sortedFriends = friends.OrderByDescending(f => f.Messages).ToList();
            else
                sortedFriends = friends.OrderBy(f => f.User.FullName).ToList();


            return sortedFriends;
        }

        // GET api/Friends/Requests
        [Route("Requests")]
        public IEnumerable<Friend> GetRequests()
        {
            var user = User.Identity.GetUserId();

            // Get friend relationships which is not accepted
            var friends = db.Friends
                .Where(f => f.UserId2 == user)
                .Where(f => f.AcceptedAt == null)
                .Include(f => f.User)
                .ToList();

            return friends;
        }


        // POST api/Friends/{userId}/Add
        [Route("{userId}/Add")]
        public async Task<IHttpActionResult> AddFriend(string userId)
        {
            var user = manager.FindById(User.Identity.GetUserId());
            var friend = manager.FindById(userId);

            // Check if user is already a friend
            var existingFriend = db.Friends
                .Where(f => f.UserId1 == user.Id && f.UserId2 == userId
                            || f.UserId1 == userId && f.UserId2 == user.Id)
                .FirstOrDefault();

            if (existingFriend != null)
                return BadRequest();

            // Do not require accepting friend for prototype
            var newFriend = new Friend
            {
                UserId1 = user.Id,
                UserId2 = userId,
                RequestedAt = DateTime.Now,
                AcceptedAt = DateTime.Now
            };

            db.Friends.Add(newFriend);
            await db.SaveChangesAsync();

            // Set friend notification
            FirebaseController.Notify(friend, user.FullName + " added you as a friend!", "", FCMType.FriendAdded, 0);

            return Ok();
        }


        // POST api/Friends/Requests/{friendId}/Accept
        [Route("Requests/{friendId}/Accept")]
        public async Task<IHttpActionResult> AcceptRequest(int friendId)
        {
            var result = db.Friends.SingleOrDefault(f => f.Id == friendId);
            if (result != null)
            {
                if (result.UserId2 != User.Identity.GetUserId() || result.AcceptedAt != null)
                    return BadRequest();

                result.AcceptedAt = DateTime.Now;
                await db.SaveChangesAsync();
            }

            return Ok();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }

        private bool FriendExists(int id)
        {
            return db.Friends.Count(e => e.Id == id) > 0;
        }
    }
}