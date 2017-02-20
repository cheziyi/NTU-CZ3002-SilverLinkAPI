using Microsoft.AspNet.Identity.EntityFramework;
using SilverLinkAPI.DAL;
using SilverLinkAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SilverLinkAPI.Controllers
{
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
            var groups = from g in db.Groups
                          select g;

            return groups.ToList();
        }
    }
}
