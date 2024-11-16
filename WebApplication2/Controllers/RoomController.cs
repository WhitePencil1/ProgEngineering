using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers
{
    [Route("api/room")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly GameService _game;

        public RoomController(GameService gameService)
        {
            _game = gameService; // Внедрение зависимости через конструктор
        }

        [HttpPost]
        public ActionResult CreateRoom()
        {
            var room = _game.CreateRoom();

            // Возвращаем только код комнаты в теле ответа
            return CreatedAtAction(nameof(GetRoom), new { code = room.Code }, new { Code = room.Code });
        }

        [HttpGet("{code}")]
        public IActionResult GetRoom(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return BadRequest("Код комнаты не может быть пустым.");

            var room = _game.GetRoom(code);
            if (room == null)
                return NotFound("Комната не найдена.");

            //var roomDto = new RoomDto
            //{
            //    Code = room.Code,
            //    Players = room.Players.Select(p => new PlayerDto { Name = p.Name, Avatar = p.Avatar }).ToList()
            //};

            return Ok(room);
        }

        [HttpGet]
        public IActionResult GetRoom()
        {
            // Попытка получить значения roomId и playerId из куки
            if (Request.Cookies.TryGetValue("roomCode", out var roomCode))
            {
                var room = _game.GetRoom(roomCode);
                if (room == null)
                {
                    return NotFound("Комната не найдена");
                }
                return Ok(room);
            }

            return BadRequest("Куки не найдены или неверные данные");
        }
        [HttpGet("players")]
        public IActionResult GetPlayers()
        {
            if (Request.Cookies.TryGetValue("roomCode", out var roomCode))
            {
                var room = _game.GetRoom(roomCode);
                if (room == null)
                {
                    return NotFound("Комната не найдена");
                }
                return Ok(room.Players);
            }

            return BadRequest("Куки не найдены или неверные данные");
        }

        [HttpDelete("{code}")]
        public IActionResult DeleteRoom(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return BadRequest("Код комнаты не может быть пустым.");

            var success = _game.DeleteRoom(code);
            if (!success)
                return NotFound("Комната не найдена.");

            return Ok($"Комната {code} удалена.");
        }
    }

    // DTOs
    //public class RoomDto
    //{
    //    public string Code { get; set; }
    //    public List<PlayerDto> Players { get; set; }
    //}

    //public class PlayerDto
    //{
    //    public string Name { get; set; }
    //    public int Avatar { get; set; }
    //}
}
