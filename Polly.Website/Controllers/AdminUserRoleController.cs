using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Polly.Data;
using Polly.Website.Models;
using Microsoft.AspNet.Identity.Owin;

namespace Polly.Website.Controllers
{
    [Authorize(Users = "nicholasb.za@gmail.com")]
    public class AdminUserRoleController : Controller
    {
        private PollyDbContext db = new PollyDbContext();
        private ApplicationUserManager _userManager;

        public AdminUserRoleController()
        {
        }

        public AdminUserRoleController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [HttpGet]
        public async Task<ActionResult> LoadTop10Cache()
        {
            await TopTenCache.PopulateTopTenCache();
            return RedirectToAction("Index");
        }

        // GET: AdminUserRole
        public async Task<ActionResult> Index()
        {
            var users = await db.Users.ToListAsync();
            List<AdminUserRoleView> viewUsers = new List<AdminUserRoleView>();
            foreach (var user in users)
            {
                viewUsers.Add(new AdminUserRoleView(user, await UserManager.GetRolesAsync(user.Id)));
            }
            return View(viewUsers);
        }

        // GET: AdminUserRole/Details/5
        public async Task<ActionResult> Details(long id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AdminUserRoleView adminUserRoleView = new AdminUserRoleView(await UserManager.FindByIdAsync(id), (await UserManager.GetRolesAsync(id))?.ToList());

            if (adminUserRoleView == null)
            {
                return HttpNotFound();
            }
            return View(adminUserRoleView);
        }

        // GET: AdminUserRole/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminUserRole/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Email,IsEnabled,EmailConfirmed,Roles")] AdminUserRoleView adminUserRoleView)
        {
            if (ModelState.IsValid)
            {
                var user = new User { UserName = adminUserRoleView.Email, Email = adminUserRoleView.Email, EmailConfirmed = adminUserRoleView.EmailConfirmed, IsEnabled = adminUserRoleView.IsEnabled };
                var result = await UserManager.CreateAsync(user);
                return RedirectToAction("Index");
            }

            return View(adminUserRoleView);
        }

        // GET: AdminUserRole/Edit/5
        public async Task<ActionResult> Edit(long id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminUserRoleView adminUserRoleView = new AdminUserRoleView(await UserManager.FindByIdAsync(id), await UserManager.GetRolesAsync(id));
            if (adminUserRoleView == null)
            {
                return HttpNotFound();
            }
            return View(adminUserRoleView);
        }

        // POST: AdminUserRole/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Email,IsEnabled,EmailConfirmed,Roles")] AdminUserRoleView adminUserRoleView)
        {
            if (ModelState.IsValid)
            {
                var newRoles = adminUserRoleView.Roles?.Split(',') ?? new string[0];
                var currentRoles = await UserManager.GetRolesAsync(adminUserRoleView.Id);
                var sharedRoles = newRoles.Intersect(currentRoles);
                newRoles = newRoles.Except(sharedRoles).ToArray();
                var removeRoles = currentRoles.Except(sharedRoles).ToArray();
                if (newRoles.Length > 0)
                {
                    foreach (var role in newRoles)
                    {
                        await UserManager.AddToRoleAsync(adminUserRoleView.Id, role);
                    }
                    //await UserManager.AddToRolesAsync(adminUserRoleView.Id, newRoles);
                }
                if (removeRoles.Length > 0)
                {
                    await UserManager.RemoveFromRolesAsync(adminUserRoleView.Id, removeRoles);
                }

                var user = db.Users.Find(adminUserRoleView.Id);
                if (!user.IsEnabled && adminUserRoleView.IsEnabled)
                    await Domain.Emailer.Send(new Domain.Emailer.EmailContext() 
                    {
                        Body = "<p>Welcome to PriceBoar</p><p>Please login on the <a href='https://priceboar.com'>PriceBoar</a> website to make use of all available features, including the <a href='https://chrome.google.com/webstore/detail/priceboar/mlodibghfmpfnnljfeljekfhagogpkdd'>chrome extension</a>.</p><p>Kind Regards</p><p>PriceBoar</p>",
                        Subject = "Priceboar account activated!",
                        To = user.Email
                    });
                user.Email = adminUserRoleView.Email;
                user.UserName = adminUserRoleView.Email;
                user.EmailConfirmed = adminUserRoleView.EmailConfirmed;
                user.IsEnabled = adminUserRoleView.IsEnabled;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(adminUserRoleView);
        }

        // GET: AdminUserRole/Delete/5
        public async Task<ActionResult> Delete(long id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdminUserRoleView adminUserRoleView = new AdminUserRoleView(await UserManager.FindByIdAsync(id), null);
            if (adminUserRoleView == null)
            {
                return HttpNotFound();
            }
            return View(adminUserRoleView);
        }

        // POST: AdminUserRole/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            User applicationUser = db.Users.Find(id);
            db.Users.Remove(applicationUser);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
