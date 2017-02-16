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
    public class UsersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Users/5
        [ResponseType(typeof(ApplicationUser))]
        public async Task<IHttpActionResult> GetUser(string phoneNo)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            var user = await manager.FindByNameAsync(phoneNo);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}
