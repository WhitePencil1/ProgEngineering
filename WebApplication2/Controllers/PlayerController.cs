using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebApplication2.Models;
using static WebApplication2.Models.Room;

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
        [HttpPost] public IActionResult JoinRoom([FromBody] JoinRoomRequest data)
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
        [HttpDelete] public IActionResult DeletePlayer()
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

        [HttpGet] public IActionResult GetPlayer()
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

        [HttpPost("Start")]
        public async Task<IActionResult> Start()
        {
            var (player, error) = GetPlayerFromCookies();
            if (error != null) return error;

            var room = GetPlayerFromCookies().player.Room; // Получаем комнату текущего игрока

            // Отмечаем, что игрок завершил стадию
            player.IsStageCompleted = true;

            if (player.Id == 0) room._MainPlayerStart = true; //если мэйн, то готовы начать игру

            // Уведомляем комнату, если стадия завершена
            bool resetLock = player.Room.NotifyStageCompletion(Stage.Start);

            // Дожидаемся обработки стадии
            await room.WaitForStageCompletion();

            if (resetLock) room.ResetStage(); // Подготовка к следующей стадии. _MainPlayerStart не сбрасывается для комнаты, если игра началась.
            return Ok();
        }

        [HttpPost("Stage1")]
        public async Task<IActionResult> Stage1()//[FromBody] ResourceRequest data)
        {
            var (player, error) = GetPlayerFromCookies();
            if (error != null) return error;

            var room = GetPlayerFromCookies().player.Room; // Получаем комнату текущего игрока

            // Отмечаем, что игрок завершил стадию
            player.IsStageCompleted = true;

            // Уведомляем комнату, если стадия завершена
            bool resetLock = player.Room.NotifyStageCompletion(Stage.Stage1);

            // Дожидаемся обработки стадии
            await room.WaitForStageCompletion();

            if (resetLock) room.ResetStage(); // Подготовка к следующей стадии
            return Ok();
        }
        [HttpPost("Stage2")] public async Task<IActionResult> Stage2()
        {
            var (player, error) = GetPlayerFromCookies();
            if (error != null) return error;

            var room = GetPlayerFromCookies().player.Room; // Получаем комнату текущего игрока

            // Отмечаем, что игрок завершил стадию
            player.IsStageCompleted = true;

            // Уведомляем комнату, если стадия завершена
            bool resetLock = player.Room.NotifyStageCompletion(Stage.Stage2);

            // Дожидаемся обработки стадии
            await room.WaitForStageCompletion();

            if (resetLock) room.ResetStage(); // Подготовка к следующей стадии
            return Ok();
        }
        [HttpPost("Stage3")]
        public async Task<IActionResult> Stage3([FromBody] ResourceRequest data)
        {
            var (player, error) = GetPlayerFromCookies();
            if (error != null) return error;

            var room = GetPlayerFromCookies().player.Room; // Получаем комнату текущего игрока

            player.actions.RequestedESM = (data.Count, data.Price);

            // Отмечаем, что игрок завершил стадию
            player.IsStageCompleted = true;

            // Уведомляем комнату, если стадия завершена
            bool resetLock = player.Room.NotifyStageCompletion(Stage.Stage3);

            // Дожидаемся обработки стадии
            await room.WaitForStageCompletion();

            if (resetLock) room.ResetStage(); // Подготовка к следующей стадии
            return Ok();
        }
        [HttpPost("Stage4")]
        public async Task<IActionResult> Stage4([FromBody] List<(int id, int esm)> data)
        {
            var (player, error) = GetPlayerFromCookies();
            if (error != null) return error;

            var room = GetPlayerFromCookies().player.Room; // Получаем комнату текущего игрока

            player.actions.FactoriesProcess = data;

            // Отмечаем, что игрок завершил стадию
            player.IsStageCompleted = true;

            // Уведомляем комнату, если стадия завершена
            bool resetLock = player.Room.NotifyStageCompletion(Stage.Stage4);

            // Дожидаемся обработки стадии
            await room.WaitForStageCompletion();

            if (resetLock) room.ResetStage(); // Подготовка к следующей стадии
            return Ok();
        }
        [HttpPost("Stage5")]
        public async Task<IActionResult> Stage5([FromBody] ResourceRequest data)
        {
            var (player, error) = GetPlayerFromCookies();
            if (error != null) return error;

            var room = GetPlayerFromCookies().player.Room; // Получаем комнату текущего игрока

            player.actions.RequestedEGP = (data.Count, data.Price);

            // Отмечаем, что игрок завершил стадию
            player.IsStageCompleted = true;

            // Уведомляем комнату, если стадия завершена
            bool resetLock = player.Room.NotifyStageCompletion(Stage.Stage5);

            // Дожидаемся обработки стадии
            await room.WaitForStageCompletion();

            if (resetLock) room.ResetStage(); // Подготовка к следующей стадии
            return Ok();
        }
        [HttpPost("Stage6")]
        public async Task<IActionResult> Stage6()
        {
            var (player, error) = GetPlayerFromCookies();
            if (error != null) return error;

            var room = GetPlayerFromCookies().player.Room; // Получаем комнату текущего игрока

            // Отмечаем, что игрок завершил стадию
            player.IsStageCompleted = true;

            // Уведомляем комнату, если стадия завершена
            bool resetLock = player.Room.NotifyStageCompletion(Stage.Stage6);

            // Дожидаемся обработки стадии
            await room.WaitForStageCompletion();

            if (resetLock) room.ResetStage(); // Подготовка к следующей стадии
            return Ok();
        }
        [HttpPost("Stage7")]
        public async Task<IActionResult> Stage7()
        {
            var (player, error) = GetPlayerFromCookies();
            if (error != null) return error;

            var room = GetPlayerFromCookies().player.Room; // Получаем комнату текущего игрока

            // Отмечаем, что игрок завершил стадию
            player.IsStageCompleted = true;

            // Уведомляем комнату, если стадия завершена
            bool resetLock = player.Room.NotifyStageCompletion(Stage.Stage7);

            // Дожидаемся обработки стадии
            await room.WaitForStageCompletion();

            if (resetLock) room.ResetStage(); // Подготовка к следующей стадии
            return Ok();
        }
        [HttpPost("Stage8")]
        public async Task<IActionResult> Stage8([FromBody] int data)
        {
            var (player, error) = GetPlayerFromCookies();
            if (error != null) return error;

            var room = GetPlayerFromCookies().player.Room; // Получаем комнату текущего игрока

            player.actions.FactoryCredit = data;

            // Отмечаем, что игрок завершил стадию
            player.IsStageCompleted = true;

            // Уведомляем комнату, если стадия завершена
            bool resetLock = player.Room.NotifyStageCompletion(Stage.Stage8);

            // Дожидаемся обработки стадии
            await room.WaitForStageCompletion();

            if (resetLock) room.ResetStage(); // Подготовка к следующей стадии
            return Ok();
        }
        [HttpPost("Stage90")]
        public async Task<IActionResult> Stage90([FromBody] List<(int, bool)> data)
        {
            var (player, error) = GetPlayerFromCookies();
            if (error != null) return error;

            var room = GetPlayerFromCookies().player.Room; // Получаем комнату текущего игрока

            player.actions.FactoriesBuild = data;

            // Отмечаем, что игрок завершил стадию
            player.IsStageCompleted = true;

            // Уведомляем комнату, если стадия завершена
            bool resetLock = player.Room.NotifyStageCompletion(Stage.Stage90);

            // Дожидаемся обработки стадии
            await room.WaitForStageCompletion();

            if (resetLock) room.ResetStage(); // Подготовка к следующей стадии
            return Ok();
        }
        [HttpPost("Stage91")]
        public async Task<IActionResult> Stage91([FromBody] List<int> data)
        {
            var (player, error) = GetPlayerFromCookies();
            if (error != null) return error;

            var room = GetPlayerFromCookies().player.Room; // Получаем комнату текущего игрока

            player.actions.FactoriesUpgrade = data;

            // Отмечаем, что игрок завершил стадию
            player.IsStageCompleted = true;

            // Уведомляем комнату, если стадия завершена
            bool resetLock = player.Room.NotifyStageCompletion(Stage.Stage91);

            // Дожидаемся обработки стадии
            await room.WaitForStageCompletion();

            if (resetLock) room.ResetStage(); // Подготовка к следующей стадии
            return Ok();
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
