/* eslint-disable react/prop-types */
import BookHeader from "./BookHeader"
import PlayersList from "./PlayersList"
import { instance } from '../../utils/axios';
import { useQuery } from '@tanstack/react-query';

export default function CreateStage({setStage, players, setPlayers, roomCode}) {
    
    const getRoom = async () => {
        try {
            const response = await instance.get(`room`);
            console.log(response.data);
        } catch (error) {
          console.error('Ошибка при получении комнаты', error);
        }
    }
    const { data } = useQuery(
          ['room'], // Ключ для кэширования
          getRoom, // Функция для получения данных
          {
            refetchInterval: 5000, // Интервал в миллисекундах (например, 5 секунд)
            //refetchOnWindowFocus: true, // Опционально: повторный запрос при возврате к вкладке
          }
        );

    return(
        <>
            <div className="book-background">
                <div className="book-page">
                    <BookHeader>Your room key</BookHeader>
                    <h2 className="room-key centered">{roomCode}</h2>
                    <button className="book-btn book-back-btn" onClick={() => {setStage("registration")}}></button>
                </div>
                <div className="book-page">
                    <div>
                        <BookHeader isCreateRole = {true}>Players list</BookHeader>
                        <p className="book-content">Competitors {players.length} of 4</p>
                    </div>
                    <PlayersList playersData={players}/>
                    <button className="book-btn book-start-btn"></button>
                </div>
            </div>
        </>
    )
}