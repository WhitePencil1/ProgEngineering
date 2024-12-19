/* eslint-disable react/prop-types */
import "./GameSection.css"
import CurrentPlayer from "./CurrentPlayer/CurrentPlayer"
import AnotherPlayer from "./AnotherPlayer/AnotherPlayer"
import { instance } from "../../utils/axios";
import { useEffect, useState } from "react";
import { useQuery } from '@tanstack/react-query';
import BlankPicture from "./BlankPicture/BlankPicture";
import { stages } from "../../data";


// const stages = ["Expenses Payment", "Getting a market environment", "Requests for materials", 
//     "Production of products", "Sale of products", "Payment of loan interest", "Obtaining loans", "Construction of factories"];




// eslint-disable-next-line react/prop-types
export default function GameSection({players, setPlayers}) {
    const [gameData, setGameData] = useState([]);
    const [gameStage, setGameStage] = useState(stages.Stage1);
    const [myId, setMyId] = useState();


    //ФУНКЦИЯ ПОЛУЧЕНИЯ ДАННЫХ
    const getRoom = async () => {
            try {
                const players = await instance.get(`room/players`);
                const room = await instance.get(`room`);
                setPlayers(players.data);
                setGameData(room.data);
                return 0;
            } catch (error) {
              console.error('Ошибка при получении комнаты', error);
            }
    }
    //ЗАПРОС ДЛЯ ПОЛУЧЕНИЯ АКТУАЛЬНЫХ ДАННЫХ
    useQuery(
        ['room/players'], // Ключ для кэширования
        getRoom, // Функция для получения данных
        {
            refetchInterval: 20000, // Интервал в миллисекундах (например, 5 секунд)
            refetchOnWindowFocus: false, // Опционально: повторный запрос при возврате к вкладке
            keepPreviousData: true
        }
    );

    useEffect(() => {
        instance.get("player").then(response => setMyId(response.data.id));
    }, [])


    if(players.length == 4) {
        return (
            <section className="players-box">
                <CurrentPlayer player={players[0]} gameData={gameData} setGameStage={() => instance.get(stages.Stage1)}/>
                <AnotherPlayer player={players[1]} position={2}/>
                <AnotherPlayer player={players[2]} position={3}/>
                <AnotherPlayer player={players[3]} position={4}/>
            </section>
        )
    }

    else if(players.length == 2) {
        return (
            <section className="players-box">
                <CurrentPlayer player={players.find(player => player.id === myId)} gameData={gameData} stageTime={10} nextStage={gameStage}/>
                <BlankPicture pictureNum={1}/>
                <AnotherPlayer player={players.find(player => player.id !== myId)} position={3}/>
                <BlankPicture pictureNum={1}/>
            </section>
        )
    }
}