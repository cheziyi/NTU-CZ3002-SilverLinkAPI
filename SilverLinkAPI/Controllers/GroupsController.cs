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
using System.Web.Http.Description;

namespace SilverLinkAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Groups")]
    public class GroupsController : ApiController
    {
        private ApplicationDbContext db;
        private ApplicationUserManager manager;

        public GroupsController()
        {
            db = new ApplicationDbContext();
            manager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
        }

        // GET: api/Groups
        public IEnumerable<Group> GetGroups()
        {
            var groups = db.Groups.ToList();
            return groups;
        }

        // GET api/Groups/Starred
        [Route("Starred")]
        public IEnumerable<Group> GetStarredGroups()
        {
            string user = User.Identity.GetUserId();

            var groups = db.Groups
                         .Where(g => g.Members.Any
                         (f => f.Id == user))
                         .Include(g => g.Members)
                         .ToList();

            return groups;
        }

        // POST api/Groups/{groupId}/Star
        [Route("{groupId}/Star")]
        public async Task<IHttpActionResult> StarGroup(int groupId)
        {
            var user = manager.FindById(User.Identity.GetUserId());

            var group = db.Groups
                   .Where(g => g.Id == groupId)
                   .Include(g => g.Members)
                   .FirstOrDefault();

            group.Members.Add((SilverUser)user);

            db.Entry(group).State = EntityState.Modified;

            await db.SaveChangesAsync();

            return Ok();
        }

        // POST: api/Groups/{groupId}/Unstar
        [Route("{groupId}/Unstar")]
        public async Task<IHttpActionResult> UnstarGroup(int groupId)
        {
            var user = manager.FindById(User.Identity.GetUserId());

            var group = db.Groups
                   .Where(g => g.Id == groupId)
                   .Include(g => g.Members)
                   .FirstOrDefault();

            group.Members.Remove((SilverUser)user);

            db.Entry(group).State = EntityState.Modified;

            await db.SaveChangesAsync();

            return Ok();
        }
    }
}
