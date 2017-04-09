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
    [RoutePrefix("api/Groups")]
    public class GroupsController : ApiController
    {
        private readonly ApplicationDbContext db;
        private readonly ApplicationUserManager manager;

        public GroupsController()
        {
            db = new ApplicationDbContext();
            manager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
        }

        // GET: api/Groups
        public IEnumerable<Group> GetGroups()
        {
            var groups = db.Groups
                .OrderBy(g => g.Name)
                .ToList();

            return groups;
        }


        // GET: api/Groups/{groupId}
        [Route("{groupId}")]
        [ResponseType(typeof(Group))]
        public async Task<IHttpActionResult> GetGroup(int groupId)
        {
            var group = await db.Groups
                .Where(g => g.Id == groupId)
                .Include(g => g.Members)
                .FirstOrDefaultAsync();

            await db.Entry(group)
                .Collection(g => g.Messages)
                .Query().OrderByDescending(m => m.SentAt).Take(1)
                .LoadAsync();

            return Ok(group);
        }


        // GET api/Groups/Starred
        [Route("Starred")]
        public IEnumerable<Group> GetStarredGroups()
        {
            var user = User.Identity.GetUserId();

            // Get groups where user has added
            var groups = db.Groups
                .Where(g => g.Members.Any
                    (f => f.Id == user))
                .Include(g => g.Members)
                .OrderBy(g => g.Name)
                .ToList();

            // Sort
            foreach (var group in groups)
                db.Entry(group)
                    .Collection(g => g.Messages)
                    .Query().OrderByDescending(m => m.SentAt).Take(1)
                    .Load();

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

            // Add user to members
            group.Members.Add((SilverUser) user);

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

            // Remove user from members
            group.Members.Remove((SilverUser) user);

            db.Entry(group).State = EntityState.Modified;

            await db.SaveChangesAsync();

            return Ok();
        }
    }
}