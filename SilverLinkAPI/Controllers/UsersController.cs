using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SilverLinkAPI.DAL;
using SilverLinkAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace SilverLinkAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Users")]
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
        [Route("{phoneNo}")]
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

        // PUT: api/Users
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


        // POST api/Users/{userId}/RegisterCarer
        [Route("{userId}/RegisterCarer")]
        public async Task<IHttpActionResult> RegisterCarer(string userId)
        {
            var user = manager.FindById(User.Identity.GetUserId());
            var carer = manager.FindById(userId);
            
            if (user.Role != UserRole.Silver || carer.Role != UserRole.Carer)
            {
                return BadRequest();
            }

            if (((SilverUser)user).Carers == null)
            {
                ((SilverUser)user).Carers = new List<CarerUser>();
            }

            ((SilverUser)user).Carers.Add((CarerUser)carer);

            await manager.UpdateAsync(user);
            await db.SaveChangesAsync();

            return Ok();
        }


    }
}
