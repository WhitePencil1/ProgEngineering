/* eslint-disable react/prop-types */
import { useState, useEffect } from "react"
import BookHeader from "./BookHeader"
import PlayersList from "./PlayersList"
import { instance } from "../../utils/axios"
import { useQuery } from '@tanstack/react-query';
import "./GameLoader.css"


export default function ConnectStage({setStage, players, setPlayers, roomCode, setRoomCode, myId, setMyId}) {
    instance.defaults.timeout = 0;
    const [connectStage, setConnectStage] = useState("Search");

    const getRoom = async () => {
        try {
            const response = await instance.get(`room/players`);
            
            setPlayers(response.data);
            return response
        } catch (error) {
          console.error('Ошибка при получении комнаты', error);
        }
    }


    useQuery(
        ['room/players'], // Ключ для кэширования
        getRoom, // Функция для получения данных
        {
            refetchInterval: 2000, // Интервал в миллисекундах (например, 5 секунд)
            refetchOnWindowFocus: true, // Опционально: повторный запрос при возврате к вкладке,
            keepPreviousData: false
        }
    );

    async function startGame() {
        try{
            await instance.post("player/Start").then(request => console.log(request));
        } catch(error) {
            console.error(error)
        } finally {
            setStage("game")
        }
    }


    const joinRoom = async () => {
        try {
            const roomData = {
                roomCode: roomCode,
                playerName: players[0].name,
                avatar: players[0].avatar
            }
            console.log("Присоединение с данными:", roomData);
            const response = await instance.post(`player`, roomData).then(getRoom());
            setConnectStage("Waiting");
            console.log(response);
        } catch (error) {
            console.error('Ошибка при присоединении к комнате:', error);
        }
        finally {
            startGame()
        }
    };

    //TEST FUNCTION
    // function StartLoading() {
    //     setConnectStage("RoomSearch");
    //     setTimeout(() => {
    //         setPlayers([players[0], {nickname: "CoolBoy", avatar: 3, isMainPlayer: true}, {nickname: "CoolMan", avatar: 7, isMainPlayer: false}]);
    //         setConnectStage("GameWaiting")}, 4000);
    // }


    return(
        <>
            <div className="book-background">
                <div className="book-page">
                    <BookHeader>Enter room key</BookHeader>
                    <input type="text" id="room-key-input" className="player-nickname" maxLength={5} onChange={(evt) => setRoomCode(evt.target.value)}/>
                    <button className="book-btn book-back-btn" onClick={() => {setStage("registration"); instance.delete("player")}}></button>
                </div>
                <div className="book-page">
                    <div>
                        <BookHeader>Players list</BookHeader>
                        <p className="book-content">Competitors {players.length} of 4</p>
                    </div>
                    <PlayersList playersData={players}/>
                    <button className= {
                        connectStage === "Waiting" ? "book-btn book-next-btn hide" : "book-btn book-next-btn"
                    } onClick={() => {
                        joinRoom();
                    }}></button>
                </div>
            </div>
        </>
    )
}