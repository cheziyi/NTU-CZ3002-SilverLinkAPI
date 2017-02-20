using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using SilverLinkAPI.DAL;
using SilverLinkAPI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SilverLinkAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Friends")]
    public class FriendsController : ApiController
    {
        private ApplicationDbContext db;
        private ApplicationUserManager manager;

        public FriendsController()
        {
            db = new ApplicationDbContext();
            manager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
        }


        // GET: api/Friends
        public IEnumerable<Friend> GetFriends()
        {
            string user = User.Identity.GetUserId();

            var friends = db.Friends
                           .Where(f => f.UserId1 == user
                           || f.UserId2 == user)
                           .Where(f => f.AcceptedAt != null)
                           .Include(f => f.User)
                           .Include(f => f.UserFriend)
                           .ToList();

            foreach (Friend f in friends)
            {
                if (f.UserId1 == user)
                {
                    f.User = f.UserFriend;
                }
            }

            return friends;
        }

        // GET api/Friends/Requests
        [Route("Requests")]
        public IEnumerable<Friend> GetRequests()
        {
            string user = User.Identity.GetUserId();

            var friends = db.Friends
                         .Where(f => f.UserId2 == user)
                         .Where(f => f.AcceptedAt == null)
                         .Include(f => f.User)
                         .ToList();

            return friends;
        }


        // POST api/Friends/AddFriend
        [Route("AddFriend")]
        public async Task<IHttpActionResult> AddFriend(string userId)
        {

            var friend = new Friend { UserId1 = User.Identity.GetUserId(), UserId2 = userId, RequestedAt = DateTime.UtcNow };

            db.Friends.Add(friend);
            await db.SaveChangesAsync();

            return Ok();
        }


        // POST api/Friends/AcceptRequest
        [Route("AcceptRequest")]
        public async Task<IHttpActionResult> AcceptRequest(int friendId)
        {
            var result = db.Friends.SingleOrDefault(f => f.Id == friendId);
            if (result != null)
            {
                if (result.UserId2 != User.Identity.GetUserId() || result.AcceptedAt != null)
                    return BadRequest();

                result.AcceptedAt = DateTime.UtcNow;
                await db.SaveChangesAsync();
            }

            return Ok();
        }







        //// GET: api/Friends/5
        //[ResponseType(typeof(Friend))]
        //public async Task<IHttpActionResult> GetFriend(int id)
        //{
        //    Friend friend = await db.Friends.FindAsync(id);
        //    if (friend == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(friend);
        //}

        //// PUT: api/Friends/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutFriend(int id, Friend friend)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != friend.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(friend).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!FriendExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //// POST: api/Friends
        //[ResponseType(typeof(Friend))]
        //public async Task<IHttpActionResult> PostFriend(Friend friend)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Friends.Add(friend);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = friend.Id }, friend);
        //}

        //// DELETE: api/Friends/5
        //[ResponseType(typeof(Friend))]
        //public async Task<IHttpActionResult> DeleteFriend(int id)
        //{
        //    Friend friend = await db.Friends.FindAsync(id);
        //    if (friend == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Friends.Remove(friend);
        //    await db.SaveChangesAsync();

        //    return Ok(friend);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FriendExists(int id)
        {
            return db.Friends.Count(e => e.Id == id) > 0;
        }
    }
}