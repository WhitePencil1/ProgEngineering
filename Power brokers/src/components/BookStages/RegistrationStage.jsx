import {address} from '/src/data.js'
import AvatarList from "./AvatarList"
import BookHeader from "./BookHeader"
import "./Book.css"
import {useState} from "react"
import axios from 'axios'


// eslint-disable-next-line react/prop-types
export default function RegistrationStage({setStage, isRegistered, setIsRegistered, setRoomCode, playerRole}) {
    const [bookState, setBookState] = useState(isRegistered ? "open" : "close");
    const [playAnimation, setPlayAnimation] = useState(false);

    const [avatar, setAvatar] = useState(-1);
    const [nickname, setNickname] = useState("");

    const roomData = {
        roomCode: '',
        playerName: nickname,
        avatar: avatar
    }

    function handleChangeWelcomeStage() {
        setPlayAnimation(true)
        setIsRegistered(false)
        setTimeout(() => setStage("welcome"), 2000)
    }

    const createRoom = async () => {
        try {
          await axios
          .post(`${address}/room/create`)
          .then(data => {
            roomData.roomCode = data.data.code; 
            setRoomCode(roomData.roomCode); 
            joinRoom()
        });

        } catch (error) {
          console.error('Ошибка при создании комнаты', error);
        }
    };


      // Функция для присоединения к комнате
    const joinRoom = async () => {
        try {
            await axios.post(`${address}/room/join`, roomData).then(data => {console.log('Ответ от сервера:', data.data);});
            //await axios.get(`${address}/room/getplayer`).then(data => {console.log('Инфа о игроке: ', data);});

        } catch (error) {
            console.error('Ошибка при присоединении к комнате:', error);
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
                            {playerRole == "create" ? createRoom() : joinRoom()};
                            setStage("roomActivity");
                        }}></button>
                    </div>
                </div>
            }
        </>
        
    )
}