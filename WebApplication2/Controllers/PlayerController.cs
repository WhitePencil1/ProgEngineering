using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebApplication2.Models;

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
        public IActionResult DeletePlayer()
        {
            // Попытка получить значения roomId и playerId из куки
            if (Request.Cookies.TryGetValue("roomCode", out var roomCode) &&
                Request.Cookies.TryGetValue("playerId", out var playerId))
            {
                var result = _game.DeletePlayerFromRoom(roomCode, Convert.ToInt32(playerId));
                if (result.success)
                {
                    Response.Cookies.Delete("roomCode");
                    Response.Cookies.Delete("playerId");
                    return NoContent();
                }

                return NotFound(result.message);
            }

            return BadRequest("Куки не найдены или неверные данные");


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

                var player = room.GetPlayer(Convert.ToInt32(playerId));
                if (player == null)
                {
                    return NotFound("Игрок не найден");
                }

                return Ok(player);
            }

            return BadRequest("Куки не найдены или неверные данные");
        }

        private void SetRoomCookies(string roomCode, int playerId)
        {
            Response.Cookies.Append("roomCode", roomCode, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None, // Разрешает кросс-доменные запросы
                Secure = true, // Требуется для SameSite.None
                Expires = DateTimeOffset.Now.AddHours(1)
            });

            Response.Cookies.Append("playerId", Convert.ToString(playerId), new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None, // Разрешает кросс-доменные запросы
                Secure = true, // Требуется для SameSite.None
                Expires = DateTimeOffset.Now.AddHours(1)
            });
        }

        //PlayerActions
        [HttpPost("RequestESM")]
        public IActionResult RequestESM([FromBody] ResourceRequest data)
        {
            return UpdatePlayerAction(
                (actions, requestData) =>
                    actions.RequestedESM = (requestData.Count, requestData.Price),
                    data
                );
        }
        [HttpPost("RequestEGP")]
        public IActionResult RequestEGP([FromBody] ResourceRequest data)
        {
            return UpdatePlayerAction(
                (actions, requestData) =>
                    actions.RequestedEGP = (requestData.Count, requestData.Price),
                    data
                );
        }
        [HttpPost("Stage1")]
        public IActionResult Stage1([FromBody] ResourceRequest data, Room room)
        {
            //записываем действия пользователя.
            UpdatePlayerAction(
                (actions, requestData) =>
                    actions.RequestedEGP = (requestData.Count, requestData.Price),
                    data
                );
            room.Stage1();//здесь должен быть метод с барьером, пока он не завершится ответа не будет

            return;//возвращаем ответ с результатами хода
        }
        [HttpPost("FactoriesUpgrade")]
        public IActionResult RequestFactoriesUpgrade([FromBody] List<int> data)
        {
            return UpdatePlayerAction(
                (actions, requestData) =>
                    actions.FactoriesUpgrade = requestData,
                    data
                );
        }
        [HttpPost("FactoriesBuild")]
        public IActionResult RequestFactoriesBuild([FromBody] List<(int, bool)> data)
        {
            return UpdatePlayerAction(
                (actions, requestData) =>
                    actions.FactoriesBuild = requestData,
                    data
                );
        }
        [HttpPost("FactoriesProcess")]
        public IActionResult RequestFactoriesProcess([FromBody] List<(int count, int price)> data)
        {
            return UpdatePlayerAction(
                (actions, requestData) =>
                    actions.FactoriesProcess = requestData,
                    data
                );
        }
        [HttpPost("FactoryCredit")]
        public IActionResult RequestFactoryCredit([FromBody] int data)
        {
            return UpdatePlayerAction(
                (actions, requestData) =>
                    actions.FactoryCredit = requestData,
                    data
                );
        }
        [HttpPost("Surrend")]
        public IActionResult RequestSurrend([FromBody] bool data)
        {
            return UpdatePlayerAction(
                (actions, requestData) =>
                    actions.Surrend = requestData,
                    data
                );
        }
        private (Player player, IActionResult error) GetPlayerFromCookies()
        {
            if (!Request.Cookies.TryGetValue("roomCode", out var roomCode) ||
                !Request.Cookies.TryGetValue("playerId", out var playerId))
            {
                return (null, BadRequest("Некорректные куки"));
            }

            var room = _game.GetRoom(roomCode);
            if (room == null)
            {
                return (null, NotFound("Комната не найдена"));
            }

            var player = room.GetPlayer(Convert.ToInt32(playerId));
            if (player == null)
            {
                return (null, NotFound("Игрок не найден"));
            }

            return (player, null);
        }
        private IActionResult UpdatePlayerAction<T>(Action<Actions, T> updateAction, T data)
        {
            var (player, error) = GetPlayerFromCookies();
            if (error != null) return error;

            updateAction(player.actions, data);
            return Ok();
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
        public int PlayerId { get; set; }
    }
    public class ResourceRequest
    {
        public int Count { get; set; }
        public int Price { get; set; }
    }
}
