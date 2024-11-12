import AvatarList from "./AvatarList"
import BookHeader from "./BookHeader"
import "./Book.css"
import { useState } from "react"


// eslint-disable-next-line react/prop-types
export default function RegistrationStage({setStage, isRegistered, setIsRegistered, setPlayers, playerRole}) {
    const [bookState, setBookState] = useState(isRegistered ? "open" : "close");
    const [playAnimation, setPlayAnimation] = useState(false);

    const [avatar, setAvatar] = useState(-1);
    const [nickname, setNickname] = useState("");
    //let curPlayer = {nickname: "", avatar: avatar, isMainPlayer: playerRole == "create" ? true : false};

    function handleChangeWelcomeStage() {
        setPlayAnimation(true)
        setIsRegistered(false)
        setTimeout(() => setStage("welcome"), 2000)
    }

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
                            setPlayers([{nickname: nickname, avatar: avatar, isMainPlayer: playerRole == "create" ? true : false}])
                            setStage("roomActivity")
                        }}></button>
                    </div>
                </div>
            }
        </>
        
    )
}