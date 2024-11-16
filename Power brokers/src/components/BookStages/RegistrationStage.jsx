import AvatarList from "./AvatarList"
import BookHeader from "./BookHeader"
import "./Book.css"
import { useState } from 'react';
import { instance } from '../../utils/axios';
// import { useEffect } from "react";


// eslint-disable-next-line react/prop-types
export default function RegistrationStage({setStage, isRegistered, setIsRegistered, playerRole, setRoomCode, setPlayers}) {
    const [bookState, setBookState] = useState(isRegistered ? "open" : "close");
    const [playAnimation, setPlayAnimation] = useState(false);

    const [avatar, setAvatar] = useState(-1);
    const [nickname, setNickname] = useState("");

    //Функция для возврата в главное меню
    function handleChangeWelcomeStage() {
        setPlayAnimation(true)
        setIsRegistered(false)
        setTimeout(() => setStage("welcome"), 2000)
    }

    const createRoom = async () => {
        try {
            const response = await instance.post(`room`);
            console.log("Код комнаты из ответа:", response.data.code);
            setRoomCode(response.data.code);
            return response.data.code;
        } catch (error) {
          console.error('Ошибка при создании комнаты', error);
        }
    };

      const joinRoom = async (roomCode) => {
        try {
            const roomData = {
                roomCode: roomCode,
                playerName: nickname,
                avatar: avatar
            }
            console.log("Присоединение с данными:", roomData);
            const response = await instance.post(`player`, roomData);
            console.log(response);
        } catch (error) {
            console.error('Ошибка при присоединении к комнате:', error);
        }
    };

    const сreateAndJoin = async () => {
        try {
            const roomCode = await createRoom();
            await joinRoom(roomCode);
        } catch (error) {
            console.error("Ошибка при создании или присоединении:", error);
        }
    };

    return(
        <>
            {bookState === 'close' &&
                <img src="/img/closeBook.png" alt="" className="close-book" onAnimationEnd={() => {setBookState('open')}}/>
            }
            {bookState === 'open' &&
                <div className={playAnimation ? "book-background sailAway" : "book-background"}>
                    <div className="book-page">
                        <BookHeader>Choose your avatar</BookHeader>
                        <AvatarList setCurPlayerAvatar= {setAvatar}/>
                        <button className="book-btn book-back-btn" onClick={() => handleChangeWelcomeStage()}></button>
                    </div>
                    <div className="book-page">
                        <BookHeader>Enter your nickname</BookHeader>
                        <div className="book-content">
                            <p>As of today, my business is being placed in the hands of the most reliable and suitable person I know. I believe this is just the beginning of your journey,</p>
                            <input type="text" className="player-nickname" maxLength={12} placeholder="Your name" onChange={(evt) => setNickname(evt.target.value)}/>
                        </div>
                        <button className="book-btn book-next-btn" onClick={() => {
                            setIsRegistered(true);
                            // сreateAndJoin();
                            //if(playerRole == "create") сreateAndJoin()
                            {playerRole == "create" ? сreateAndJoin() : setPlayers([{name: nickname, avatar: avatar, isMainPlayer: false}])};
                            setStage("roomActivity");
                        }}></button>
                    </div>
                </div>
            }
        </>
        
    )
}