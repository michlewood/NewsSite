using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsSite.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NewsSite.Controllers
{
    [Route("check")]
    public class CheckController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private ApplicationDbContext _context;

        public CheckController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, 
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;

            //CreateUserRoles();
            //AddAllUsers();

        }
        
        private async Task CreateUserRoles()
        {
            await _roleManager.CreateAsync(new IdentityRole("Administrator"));
            await _roleManager.CreateAsync(new IdentityRole("Publisher"));
            await _roleManager.CreateAsync(new IdentityRole("Subscriber"));
        }

        [HttpPost, Route("view/login")]
        async public Task<IActionResult> Login(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            await _signInManager.SignInAsync(user, true);
            //var principal = await _signInManager.CreateUserPrincipalAsync(user);
            //var result = _signInManager.IsSignedIn(principal);


            return Ok(user.UserName);
        }

        [HttpGet, Route("view/claim")]
        async public Task<IActionResult> AddClaim()
        {
            var user = await _userManager.FindByEmailAsync("xerxes@gmail.com");
            //await _userManager.RemoveClaimsAsync(user, await _userManager.GetClaimsAsync(user));
            //await _userManager.AddClaimAsync(user, new Claim("Shoesize", "42"));
            return Ok(await _userManager.GetRolesAsync(user));
        }

        [AllowAnonymous]
        [HttpGet, Route("view/AddAll")]
        async public Task<IActionResult> AddAllUsers()
        {
            //CreateUserRoles();

            await _roleManager.CreateAsync(new IdentityRole("Administrator"));
            await _roleManager.CreateAsync(new IdentityRole("Publisher"));
            await _roleManager.CreateAsync(new IdentityRole("Subscriber"));

            string[] users = { "adam@gmail.com", "", "Administrator",
                "peter@gmail.com", "", "Publisher",
                "susan@gmail.com", "48", "Subscriber",
                "viktor@gmail.com", "15", "Subscriber",
                "xerxes@gmail.com", "", "" };

            for (int i = 0; i < users.Length; i += 3)
            {
                ApplicationUser newUser = new ApplicationUser
                {
                    Email = users[i],
                    UserName = users[i]
                };
                if (users[i + 1] != "") newUser.Age = int.Parse(users[i + 1]);


                var resultNewUser = await _userManager.CreateAsync(newUser);
                if (resultNewUser.Succeeded && users[i + 2] != "") await _userManager.AddToRoleAsync(newUser, users[i + 2]);

                if ((await _userManager.GetRolesAsync(newUser)).Contains("Administrator")
                    || (await _userManager.GetRolesAsync(newUser)).Contains("Publisher")
                    || (newUser.Age != null && newUser.Age >= 20))
                    await _userManager.AddClaimAsync(newUser, new Claim("MinimumAge", "true"));

                //var newUser = await _userManager.FindByEmailAsync(users[i]);

                if ((await _userManager.GetRolesAsync(newUser)).Contains("Administrator"))
                    await _userManager.AddClaimAsync(newUser, new Claim("Publish", "all"));

                if (users[i] == "peter@gmail.com")
                {
                    await _userManager.AddClaimAsync(newUser, new Claim("Publish", "sport"));
                    await _userManager.AddClaimAsync(newUser, new Claim("Publish", "economy"));
                }
            }

            return Ok(_userManager.Users);
        }

        [HttpGet, Route("view/showAll")]
        async public Task<IActionResult> ShowAllUsers()
        {
            var user = await _userManager.FindByEmailAsync("peter@gmail.com");
            var claims = await _userManager.GetClaimsAsync(user);
            return Ok(claims);
        }
        
        [HttpGet, Route("view/open")]
        public IActionResult CanViewOpenNews()
        {

            return Ok("Open");
        }

        [Authorize(Policy = "HiddenNews")]
        [HttpGet, Route("view/hidden")]
        public IActionResult CanViewHiddenNews()
        {

            return Ok("Hidden");
        }

        [Authorize(Policy = "HiddenNews")]
        [Authorize(Policy = "isOfAge")]
        [HttpGet, Route("view/hiddenAndAge")]
        public IActionResult CanViewHiddenNewsAndOfAge()
        {

            return Ok("Hidden Age");
        }

        [Authorize(Policy = "canPublishEconomy")]
        [HttpGet, Route("view/Economy")]
        public IActionResult CanPublishEconomy()
        {
            return Ok("Economy");
        }

        [Authorize(Policy = "canPublishSport")]
        [HttpGet, Route("view/Sport")]
        public IActionResult CanPublishSport()
        {
            return Ok("Sport");
        }

        [Authorize(Policy = "canPublishCulture")]
        [HttpGet, Route("view/culture")]
        public IActionResult CanPublishCulture()
        {
            return Ok("Culture");
        }

        [HttpGet, Route("view/Recover")]
        async public Task<IActionResult> RecoverUsers()
        {


            _context.RemoveRange(_userManager.Users.ToList());
            _context.SaveChanges();

            await CreateUserRoles();

            string[] users = { "adam@gmail.com", "", "Administrator",
                "peter@gmail.com", "", "Publisher",
                "susan@gmail.com", "48", "Subscriber",
                "viktor@gmail.com", "15", "Subscriber",
                "xerxes@gmail.com", "", "" };

            for (int i = 0; i < users.Length; i += 3)
            {
                ApplicationUser newUser = new ApplicationUser
                {
                    Email = users[i],
                    UserName = users[i]
                };
                if (users[i + 1] != "") newUser.Age = int.Parse(users[i + 1]);


                var resultNewUser = await _userManager.CreateAsync(newUser);
                if (resultNewUser.Succeeded && users[i + 2] != "") await _userManager.AddToRoleAsync(newUser, users[i + 2]);

                if ((await _userManager.GetRolesAsync(newUser)).Contains("Administrator")
                    || (await _userManager.GetRolesAsync(newUser)).Contains("Publisher")
                    || (newUser.Age != null && newUser.Age >= 20))
                    await _userManager.AddClaimAsync(newUser, new Claim("MinimumAge", "true"));

                //var newUser = await _userManager.FindByEmailAsync(users[i]);

                if ((await _userManager.GetRolesAsync(newUser)).Contains("Administrator"))
                    await _userManager.AddClaimAsync(newUser, new Claim("Publish", "all"));

                if (users[i] == "peter@gmail.com")
                {
                    await _userManager.AddClaimAsync(newUser, new Claim("Publish", "sport"));
                    await _userManager.AddClaimAsync(newUser, new Claim("Publish", "economy"));
                }
            }

            return Ok(_userManager.Users);
        }

        [HttpGet, Route("view/getAll")]
        public List<ApplicationUser> GetAllUsers()
        {
            return _userManager.Users.ToList();
        }

        [HttpGet, Route("view/getAllWithClaims")]
        public async Task<List<ReturnModel>> GetAllUsersWithClaims()
        {

            var returnList = new List<ReturnModel>();
            foreach (var user in _userManager.Users)
            {
                var claimsToThisUser = await _userManager.GetClaimsAsync(user);

                var returnModel = new ReturnModel()
                {
                    Claims = claimsToThisUser,
                    User = user
                };
                returnList.Add(returnModel);
            }
            return returnList;
        }

    }
}
