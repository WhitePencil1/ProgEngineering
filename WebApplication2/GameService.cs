namespace WebApplication2
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
        public (bool success, string message, string playerId) JoinRoom(string roomCode, string playerName, int avatar)
        {
            if (Rooms.ContainsKey(roomCode))
            {
                return Rooms[roomCode].Join(playerName, avatar);
            }
            else return (false, $"комната {roomCode} не найдена", "");
        }
        public (bool success, string message) DeletePlayerFromRoom(string roomCode, string playerName)
        {
            if (Rooms.ContainsKey(roomCode))
            {
                return Rooms[roomCode].Leave(playerName);
            }
            else return (false, $"комната {roomCode} не найдена");
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
