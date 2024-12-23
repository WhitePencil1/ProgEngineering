using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
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
                var (player, error) = GetPlayerFromCookies();
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
                var (player, error) = GetPlayerFromCookies();
                if (_game.DeletePlayerFromRoom(roomCode, Convert.ToInt32(playerId)))
                {
                    Response.Cookies.Delete("roomCode");
                    Response.Cookies.Delete("playerId");
                    return NoContent();
                }
                return NotFound();
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
        [HttpPost("putEsm")]
        public IActionResult PutESM([FromBody] PutESMData data)
        {
            var (player, error) = GetPlayerFromCookies();
            if (error != null) return error;
            string logMes = $"{ player.Name } хочет обработать { data.Esm} ЕСМ на фабрике №{ data.Id}.";
            // Обработка ЕСМ
            int result = player.ProcessESM(data.Id, data.Esm);
            switch (result)//-4 - не хватает денег, -3 - не хватает ESM, -2 - не построен, -1 - нет места
            {
                case -4:
                    player.Room.AddLog($"{logMes} Не хватает денег.");
                    return StatusCode(-4, new
                    {
                        ErrorCode = "FailMoney",
                        Message = "Не хватает денег."
                    });
                case -3:
                    player.Room.AddLog($"{logMes} Не хватает ESM.");
                    return StatusCode(-3, new
                    {
                        ErrorCode = "FailESM",
                        Message = "Не хватает ESM."
                    });
                case -2:
                    player.Room.AddLog($"{logMes} Завод не построен.");
                    return StatusCode(-2, new
                    {
                        ErrorCode = "NotBuild",
                        Message = "Завод не построен."
                    });
                case -1:
                    player.Room.AddLog($"{logMes} Завод уже заполнен.");
                    return StatusCode(-1, new
                    {
                        ErrorCode = "AlreadyFull",
                        Message = "Завод уже заполнен."
                    });
                default:
                    player.Room.AddLog($"{logMes} ЕСМ успешно добавлены.");
                    return Ok(new
                    {
                        Message = "ЕСМ добавлены."
                    });
            }
        }

        [HttpPost("Stage4")]
        public async Task<IActionResult> Stage4()//[FromBody] List<(int id, int esm)> data
        {
            var (player, error) = GetPlayerFromCookies();
            if (error != null) return error;

            var room = GetPlayerFromCookies().player.Room; // Получаем комнату текущего игрока

            //player.actions.FactoriesProcess = data;

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
        [HttpPost("GetCredit")]
        public IActionResult GetCredit([FromBody] FactoryIdData data)
        {
            var (player, error) = GetPlayerFromCookies();
            if (error != null) return error;

            // Обработка ЕСМ
            
            (int result, string message)= player.GetCredit(data.Id);

            string logMes = $"{player.Name} хочет взять кредит под фабрику №{data.Id}.";

            switch (result)//-2 - не построен, -1 - нет места
            {
                case -1:
                    player.Room.AddLog($"{logMes} Капитала для обеспечения кредита недостаточно.");
                    return StatusCode(-1, new
                    {
                        ErrorCode = "NotCapital",
                        Message = "Капитала для обеспечения кредита недостаточно."
                    });
                case -2:
                    player.Room.AddLog($"{logMes} Капитала для обеспечения кредита недостаточно.");
                    return StatusCode(-2, new
                    {
                        ErrorCode = "AlreadyCredit",
                        Message = "Завод уже заложен."
                    });
                default:
                    player.Room.AddLog($"{logMes} Капитала для обеспечения кредита недостаточно.");
                    return Ok(new
                    {
                        Message = "Кредит взят."
                    });
            }
        }
        [HttpPost("Stage8")]
        public async Task<IActionResult> Stage8()
        {
            var (player, error) = GetPlayerFromCookies();
            if (error != null) return error;

            var room = GetPlayerFromCookies().player.Room; // Получаем комнату текущего игрока
            
            // Отмечаем, что игрок завершил стадию
            player.IsStageCompleted = true;

            // Уведомляем комнату, если стадия завершена
            bool resetLock = player.Room.NotifyStageCompletion(Stage.Stage8);

            // Дожидаемся обработки стадии
            await room.WaitForStageCompletion();

            if (resetLock) room.ResetStage(); // Подготовка к следующей стадии
            return Ok();
        }
        [HttpPost("buildFactory")]
        public IActionResult BuildFactory([FromBody] BuildData data)
        {
            var (player, error) = GetPlayerFromCookies();
            if (error != null) return error;

            (int result, int cost) = player.BuildFactory(data.Id, data.Auto);
            string auto = data.Auto ? "автоматическую" : "обычную";
            string logMes = $"Игрок {player.Name} хочет построить {auto} фабрику на месте №{data.Id}.";

            switch (result)//-1 - не хватает денег, -2 - уже построен
            {
                case -1:
                    player.Room.AddLog($"{logMes} Не хватает денег.");
                    return StatusCode(-1, new
                    {
                        ErrorCode = "FailMoney",
                        Message = "Не хватает денег."
                    });
                case -2:
                    player.Room.AddLog($"{logMes} Завод уже построен.");
                    return StatusCode(-2, new
                    {
                        ErrorCode = "AlreadyBuild",
                        Message = "Завод уже построен."
                    });
                default:
                    player.Room.AddLog($"{logMes} Завод начал строительство.");
                    return Ok(new
                    {
                        Message = "Завод начал строительство."
                    });
            }
        }
        [HttpPost("Stage90")]
        public async Task<IActionResult> Stage90()
        {
            var (player, error) = GetPlayerFromCookies();
            if (error != null) return error;

            var room = GetPlayerFromCookies().player.Room; // Получаем комнату текущего игрока

            //player.actions.FactoriesBuild = data;

            // Отмечаем, что игрок завершил стадию
            player.IsStageCompleted = true;

            // Уведомляем комнату, если стадия завершена
            bool resetLock = player.Room.NotifyStageCompletion(Stage.Stage90);

            // Дожидаемся обработки стадии
            await room.WaitForStageCompletion();

            if (resetLock) room.ResetStage(); // Подготовка к следующей стадии
            return Ok();
        }
        [HttpPost("upgradeFactory")]
        public IActionResult UpgradeFactory([FromBody] FactoryIdData data)
        {
            var (player, error) = GetPlayerFromCookies();
            if (error != null) return error;

            (int result, int cost) = player.UpgradeFactory(data.Id);

            string logMes = $"Игрок {player.Name} хочет улучшить фабрику №{data.Id}.";

            switch (result)// -2 - не может быть улучшена в данный момент, -1 - не хватает денег
            {
                case -1:
                    player.Room.AddLog($"{logMes} Не хватает денег.");
                    return StatusCode(-1, new
                    {
                        ErrorCode = "FailMoney",
                        Message = "Не хватает денег."
                    });
                case -2:
                    player.Room.AddLog($"{logMes} Не может быть улучшена в данный момент.");
                    return StatusCode(-2, new
                    {
                        ErrorCode = "AlreadyBuild",
                        Message = "Не может быть улучшена в данный момент."
                    });
                default:
                    player.Room.AddLog($"{logMes} Завод начал улучшение.");
                    return Ok(new
                    {
                        Message = "Завод начал улучшение."
                    });
            }
        }
        [HttpPost("Stage91")]
        public async Task<IActionResult> Stage91()//[FromBody] UpgradeData data
        {
            var (player, error) = GetPlayerFromCookies();
            if (error != null) return error;

            var room = GetPlayerFromCookies().player.Room; // Получаем комнату текущего игрока

            //player.actions.FactoriesUpgrade = data;

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
    public class PutESMData
    {
        public int Id { get; set; }
        public int Esm { get; set; }
    }
    public class BuildData
    {
        public int Id { get; set; }
        public bool Auto { get; set; }
    }
    public class FactoryIdData
    {
        public int Id { get; set; }
    }
}
