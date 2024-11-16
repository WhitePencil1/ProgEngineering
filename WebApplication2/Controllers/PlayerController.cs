using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WebApplication2.Controllers
{
    [Route("api/player")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly GameService _game;

        public PlayerController(GameService gameService)
        {
            _game = gameService; // Внедрение зависимости
        }

        [HttpPost]
        public IActionResult JoinRoom([FromBody] JoinRoomRequest data)
        {
            if (data == null || string.IsNullOrEmpty(data.RoomCode) || string.IsNullOrEmpty(data.PlayerName))
            {
                return BadRequest("Неправильные параметры");
            }

            var result = _game.JoinRoom(data.RoomCode, data.PlayerName, data.Avatar);
            if (result.success)
            {
                // Сохраняем куки с ID комнаты и игрока
                SetRoomCookies(data.RoomCode, result.playerId);

                return Ok(result.message);
            }

            return NotFound(result.message);
        }

        [HttpDelete]
        public IActionResult DeletePlayer([FromBody] DeletePlayerRequest data)
        {
            if (data == null || string.IsNullOrEmpty(data.RoomCode) || string.IsNullOrEmpty(data.PlayerId))
            {
                return BadRequest("Неправильные параметры");
            }

            var result = _game.DeletePlayerFromRoom(data.RoomCode, data.PlayerId);
            if (result.success)
            {
                return Ok(result.message);
            }

            return NotFound(result.message);
        }

        [HttpGet]
        public IActionResult GetPlayer()
        {
            // Попытка получить значения roomId и playerId из куки
            if (Request.Cookies.TryGetValue("roomCode", out var roomCode) &&
                Request.Cookies.TryGetValue("playerId", out var playerId))
            {
                var room = _game.GetRoom(roomCode);
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

        private void SetRoomCookies(string roomCode, string playerId)
        {
            Response.Cookies.Append("roomCode", roomCode, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None, // Разрешает кросс-доменные запросы
                Secure = true, // Требуется для SameSite.None
                Expires = DateTimeOffset.Now.AddHours(1)
            });

            Response.Cookies.Append("playerId", playerId, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None, // Разрешает кросс-доменные запросы
                Secure = true, // Требуется для SameSite.None
                Expires = DateTimeOffset.Now.AddHours(1)
            });
        }
    }

    public class JoinRoomRequest
    {
        public string RoomCode { get; set; }
        public string PlayerName { get; set; }
        public int Avatar { get; set; }
    }

    public class DeletePlayerRequest
    {
        public string RoomCode { get; set; }
        public string PlayerId { get; set; }
    }
}
