using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GameBussinesLogic.IServices;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;

namespace Server.Controlles
{
    [Route("room")]
    public class RoomController : Controller
    {
        private readonly IRoomService _roomService;
        public RoomController(IRoomService roomService) 
        {
            _roomService = roomService;
        }

        [Authorize]
        [HttpPost("create")]
        public IActionResult CreateRoom()
        {
            var identity = Request.HttpContext.User.Identity;
            var name = Request.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Name)?.Value;
            var roomId = _roomService.Create(identity.Name, name);

            return Json(roomId);
        }
    }
}
