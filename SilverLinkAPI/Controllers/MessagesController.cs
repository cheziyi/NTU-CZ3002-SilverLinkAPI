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
    [RoutePrefix("api/Messages")]
    public class MessagesController : ApiController
    {

        private ApplicationDbContext db;
        private ApplicationUserManager manager;

        public MessagesController()
        {
            db = new ApplicationDbContext();
            manager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
        }

        // GET api/Messages/GroupMessages
        [Route("GroupMessages")]
        public IEnumerable<Message> GetGroupMessages(int groupId, DateTime since)
        {

            var messages = db.Messages
                         .OfType<GroupMessage>()
                         .Where(m => m.GroupId == groupId)
                         .Where(m => m.SentAt > since)
                         .Include(m => m.SentBy)
                         .ToList();

            return messages;
        }

        // GET api/Messages/FriendMessages
        [Route("FriendMessages")]
        public IEnumerable<Message> GetFriendMessages(int friendId, DateTime since)
        {

            var messages = db.Messages
                         .OfType<FriendMessage>()
                         .Where(m => m.FriendId == friendId)
                         .Where(m => m.SentAt > since)
                         .Include(m => m.SentBy)
                         .ToList();

            return messages;
        }

        // POST api/Messages/SendGroupMessage
        [Route("SendGroupMessage")]
        public async Task<IHttpActionResult> SendGroupMessage(int groupId, Message message)
        {
            GroupMessage msg = (GroupMessage)message;
            msg.GroupId = groupId;

            db.Messages.Add(msg);
            await db.SaveChangesAsync();

            return Ok();
        }

        // POST api/Messages/SendFriendMessage
        [Route("SendFriendMessage")]
        public async Task<IHttpActionResult> SendFriendMessage(int friendId, Message message)
        {
            FriendMessage msg = (FriendMessage)message;
            msg.FriendId = friendId;

            db.Messages.Add(msg);
            await db.SaveChangesAsync();

            return Ok();
        }
    }
}
