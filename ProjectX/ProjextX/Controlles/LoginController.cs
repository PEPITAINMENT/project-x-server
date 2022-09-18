using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controlles
{
    [Route("/")]
    public class LoginController : Controller
    {
        [HttpGet("login")]
        public IActionResult Login()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Challenge("Spotify");
            }

            HttpContext.Response.Cookies.Append("spotify_username", User.Identity.Name);
            HttpContext.SignInAsync(User);
            return View();
        }

        [HttpPost("callback")]
        public IActionResult Callback()
        {
            return new EmptyResult();
        }

        [HttpGet("some")]
        public IActionResult Some()
        {
            return new EmptyResult();
        }
    }
}
