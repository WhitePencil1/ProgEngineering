using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication2
{
    [Route("room")]
    [ApiController]
    public class GameRoomController : ControllerBase
    {
        private readonly GameService game;

        public GameRoomController(GameService gameService)
        {
            game = gameService; // Внедрение зависимости
        }

        [HttpPost("create")]
        public IActionResult CreateRoom()
        {
            var room = game.CreateRoom();
            return Ok(room.Code);
        }

        [HttpPost("join")]
        public IActionResult JoinRoom([FromBody] Dictionary<string, string> data)
        {
            if (data.TryGetValue("roomCode", out var roomCode) &&
                data.TryGetValue("playerName", out var playerName) &&
                data.TryGetValue("avatar", out var avatar))
            {
                var o = game.JoinRoom(roomCode, playerName, avatar);
                if (o.success)
                {
                    // Сохранение ID комнаты и ID игрока в куки
                    Response.Cookies.Append("roomId", roomCode, new CookieOptions
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.Now.AddHours(1) // Время жизни куки - 1 час
                    });

                    Response.Cookies.Append("playerId", o.playerId.ToString(), new CookieOptions
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.Now.AddHours(1)
                    });

                    return Ok(o.message);
                }
                return NotFound(o.message);
            }
            return BadRequest("Неправильные параметры");
        }

        [HttpDelete("delete")]
        public IActionResult DeletePlayer([FromBody] Dictionary<string, string> data)
        {
            if (data.TryGetValue("roomCode", out var roomCode) &&
                data.TryGetValue("playerName", out var playerName))
            {
                var o = game.DeletePlayerFromRoom(roomCode, playerName);
                if (o.success)
                {
                    return Ok(o.message);
                }
                return NotFound(o.message);
            }
            return BadRequest("Неправильные параметры");
        }

        [HttpGet("get/{roomCode}")]
        public IActionResult GetRoom(string roomCode)
        {
            var room = game.GetRoom(roomCode);
            if (room == null)
            {
                return NotFound("Комната не найдена");
            }
            return Ok(room);
        }

        [HttpGet("getplayer")]
        public IActionResult GetPlayer()
        {
            // Попытка получить значения roomId и playerId из куки
            if (Request.Cookies.TryGetValue("roomId", out var roomCode) &&
                Request.Cookies.TryGetValue("playerId", out var playerId))
            {
                var room = game.GetRoom(roomCode);
                if (room == null)
                {
                    return NotFound("Комната не найдена");
                }

                var player = room.GetPlayer(playerId);
                if (player == null)
                {
                    return NotFound("Игрок не найден");
                }

                return Ok(player);
            }

            return BadRequest("Куки не найдены или неверные данные");
        }


        [HttpDelete("delete/{roomCode}")]
        public IActionResult DeleteRoom(string roomCode)
        {
            if (game.DeleteRoom(roomCode)) return Ok();
            return NotFound("Комната не найдена");
        }
    }
}
