using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Transport_licensing_tax_management.Data;

namespace Transport_licensing_tax_management.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _usermanager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public DashboardController(ApplicationDbContext context, UserManager<IdentityUser> usermanager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _usermanager = usermanager;
            _roleManager = roleManager;
        }
       
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(); // Sign out the user
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(string approle)
        {
            approle = "User";
            string msg = "";
            if (!string.IsNullOrEmpty(approle))
            {

                //// This line of code should be written as: 
                bool exist = await _roleManager.RoleExistsAsync(approle).ConfigureAwait(false);
                //  // Missing await keyword before _roleManager.RoleExistsAsync(approle);
                if (!exist)
                {
                    IdentityRole role = new IdentityRole(approle);
                    await _roleManager.CreateAsync(role);
                    msg = "Role [" + approle + "] has been created successfully.";

                }
                else
                {
                    msg = "Role [" + approle + "] already exists. Try for another name.";
                }
            }
            else
            {
                msg = "Invalid role name is provided.";
            }
            ViewBag.msg = msg;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SetRole(string appuser, string approle)
        {
            appuser = "user@user.com";
            approle = "USER";
            string msg = "";
            if (!string.IsNullOrEmpty(appuser))
            {
                var usr = await _usermanager.FindByEmailAsync(appuser);
                if (usr != null)
                {
                    if (!string.IsNullOrEmpty(approle))
                    {
                        bool accgiven = await _usermanager.IsInRoleAsync(usr, approle);
                        if (accgiven)
                        {
                            msg = "Role is already assigned to this user.";
                        }
                        else
                        {
                            await _usermanager.AddToRoleAsync(usr, approle);
                            msg = "Role has been assigned to this user.";
                        }
                    }
                    else
                    {
                        msg = "Please select a Role.";
                    }
                }
                else
                {
                    msg = "Sorry ! user does not exist.";
                }
            }
            else
            {
                msg = "Please select a User.";
            }
            ViewBag.msg = msg;

            ViewBag.users = _usermanager.Users;
            ViewBag.roles = _roleManager.Roles;

            return View();
        }

    }
}
