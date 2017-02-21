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
    [RoutePrefix("api")]
    public class UsersController : ApiController
    {
        private ApplicationDbContext db;
        private ApplicationUserManager manager;

        public UsersController()
        {
            db = new ApplicationDbContext();
            manager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
        }

        // GET: api/Users/{phoneNo}
        [Route("Users/{phoneNo}")]
        [ResponseType(typeof(ApplicationUser))]
        public async Task<IHttpActionResult> GetUser(string phoneNo)
        {
            var user = await manager.FindByNameAsync(phoneNo);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/User
        [Route("User")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUser(ApplicationUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (User.Identity.GetUserId() != user.Id)
            {
                return BadRequest();
            }

            var origUser = manager.FindById(User.Identity.GetUserId());

            origUser.FullName = user.FullName;
            origUser.ProfilePicture = user.ProfilePicture;

            await manager.UpdateAsync(user);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }


        // POST api/User/Carers/{userId}
        [Route("User/Carers/{userId}")]
        public async Task<IHttpActionResult> RegisterCarer(string userId)
        {
            var user = manager.FindById(User.Identity.GetUserId());
            var carer = manager.FindById(userId);

            if (user.Role != UserRole.Silver || carer.Role != UserRole.Carer)
            {
                return BadRequest();
            }

            ((CarerUser)carer).Care = ((SilverUser)user);

            await manager.UpdateAsync(carer);
            await db.SaveChangesAsync();

            return Ok();
        }

        // GET: api/User/Carers
        [Route("User/Carers")]
        public IEnumerable<CarerUser> GetCarers()
        {
            string userId = User.Identity.GetUserId();
            var user = db.SilverUsers
                          .Where(u => u.Id == userId)
                          .Include(u => u.Carers)
                          .FirstOrDefault();

            return user.Carers.ToList();
        }

    }
}
