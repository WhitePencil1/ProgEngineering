namespace WebApplication2.Models
{
    public class GameService
    {
        private static Dictionary<string, Room>? Rooms { get; set; }
        public GameService() { Rooms = new Dictionary<string, Room>(); }
        public Room CreateRoom()
        {
            string code = CodeGenerator.GenerateUniqueCode(Rooms);
            Room room = new Room(code);
            Rooms.Add(code, room);
            return room;
        }
        public (bool success, string message, int playerId) JoinRoom(string roomCode, string playerName, int avatar)
        {
            if (Rooms.ContainsKey(roomCode))
            {
                return Rooms[roomCode].Join(playerName, avatar);
            }
            else return (false, $"комната {roomCode} не найдена", -1);
        }
        public bool DeletePlayerFromRoom(string roomCode, int playerId)
        {
            if (Rooms.ContainsKey(roomCode))
            {
                return Rooms[roomCode].Leave(playerId);
            }
            else return false;
        }
        public Room GetRoom(string roomCode)
        {
            if (Rooms.ContainsKey(roomCode))
            {
                return Rooms[roomCode];
            }
            else return null;
        }
        public bool DeleteRoom(string roomCode)
        {
            if (Rooms.ContainsKey(roomCode))
            {
                return Rooms.Remove(roomCode);
            }
            else return false;
        }
    }
}
