using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SilverLinkAPI.DAL;
using SilverLinkAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SilverLinkAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api")]
    public class MessagesController : ApiController
    {

        private ApplicationDbContext db;
        private ApplicationUserManager manager;

        public MessagesController()
        {
            db = new ApplicationDbContext();
            manager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
        }

        // GET api/Groups/{groupId}/Messages/{since}
        [Route("Groups/{groupId}/Messages/{since}")]
        public IEnumerable<Message> GetGroupMessages(int groupId, DateTime since)
        {

            var messages = db.Messages
                         .OfType<GroupMessage>()
                         .Where(m => m.GroupId == groupId)
                         .Where(m => m.SentAt > since)
                         .Include(m => m.SentBy)
                         .OrderBy(m => m.Id)
                         .ToList();

            return messages;
        }

        // GET api/Friends/{friendId}/Messages/{since}
        [Route("Friends/{friendId}/Messages/{since}")]
        public IEnumerable<Message> GetFriendMessages(int friendId, DateTime since)
        {

            var messages = db.Messages
                         .OfType<FriendMessage>()
                         .Where(m => m.FriendId == friendId)
                         .Where(m => m.SentAt > since)
                         .Include(m => m.SentBy)
                         .OrderBy(m => m.Id)
                         .ToList();

            return messages;
        }

        // POST api/Groups/{groupId}/Messages
        [Route("Groups/{groupId}/Messages")]
        public async Task<IHttpActionResult> SendGroupMessage(int groupId, Message message)
        {
            var user = manager.FindById(User.Identity.GetUserId());

            GroupMessage msg = new GroupMessage();
            msg.GroupId = groupId;
            msg.SentBy = (SilverUser)user;
            msg.SentAt = DateTime.Now;
            msg.MessageData = message.MessageData;
            msg.Type = MessageType.Voice;

            db.Messages.Add(msg);
            await db.SaveChangesAsync();

            var group = await db.Groups
              .Where(g => g.Id == groupId)
              .Include(g => g.Members)
              .FirstOrDefaultAsync();

            message.MessageData = null;

            foreach (var member in group.Members)
            {
                FirebaseController.Notify(member, "New Message from " + group.Name + "!", message.MessageText, FCMType.GroupMessage, groupId);
            }


            return Ok();
        }

        // POST api/Friends/{friendId}/Messages
        [Route("Friends/{friendId}/Messages")]
        public async Task<IHttpActionResult> SendFriendMessage(int friendId, Message message)
        {

            var user = manager.FindById(User.Identity.GetUserId());

            FriendMessage msg = new FriendMessage();
            msg.SentBy = (SilverUser)user;
            msg.SentAt = DateTime.Now;
            msg.FriendId = friendId;
            msg.MessageData = message.MessageData;
            msg.Type = MessageType.Voice;

            db.Messages.Add(msg);
            await db.SaveChangesAsync();

            var friend = await db.Friends
                   .Where(f => f.Id == friendId)
                   .Include(f => f.User)
                   .Include(f => f.UserFriend)
                   .FirstOrDefaultAsync();

            if (friend.UserId1 == user.Id)
            {
                friend.User = friend.UserFriend;
            }

            message.MessageData = null;

            FirebaseController.Notify(friend.User, "New Message from " + user.FullName + "!", message.MessageText, FCMType.FriendMessage, friendId);


            return Ok();
        }
    }
}
