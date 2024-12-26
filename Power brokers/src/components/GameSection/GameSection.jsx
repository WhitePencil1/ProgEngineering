/* eslint-disable react/prop-types */
import "./GameSection.css"
import CurrentPlayer from "./CurrentPlayer/CurrentPlayer"
import AnotherPlayer from "./AnotherPlayer/AnotherPlayer"
import { instance } from "../../utils/axios";
import { useEffect, useState } from "react";
import { useQuery } from '@tanstack/react-query';
import BlankPicture from "./BlankPicture/BlankPicture";
import ModalWindow from "./ModalWindow/ModalWindow";



export default function GameSection({players, setPlayers, setGlobalStage}) {
    const [gameData, setGameData] = useState([]);
    const [curStage, setCurStage] = useState(1); 
    const [myId, setMyId] = useState();
    const [isExitModalOpen, setIsExitModalOpen] = useState(false);


    //ФУНКЦИЯ ПОЛУЧЕНИЯ ДАННЫХ
    const getRoom = async () => {
            try {
                const players = await instance.get(`room/players`);
                const room = await instance.get(`room`);
                setPlayers(players.data);
                setGameData(room.data);
                
                //console.log(players);
                
                return 0;
            } catch (error) {
              console.error('Ошибка при получении комнаты', error);
            }
    }

    
    
    //Проверка на банкроствоство
    useEffect(() => {
        const deleteDefaulters = async () => {
            if(players[myId].money <= 0) {
                console.log("Я банкрот");
                await instance.delete("player");
                
            }
            // if(players[myId]) {
            //     console.log("Я банкрот");
            // }
        }
        deleteDefaulters();
    }, [curStage])



    //ЗАПРОС ДЛЯ ПОЛУЧЕНИЯ АКТУАЛЬНЫХ ДАННЫХ
    useQuery(
        ['room/players'], // Ключ для кэширования
        getRoom, // Функция для получения данных

        {
            refetchInterval: 1000, // Интервал в миллисекундах (например, 5 секунд)
            refetchOnWindowFocus: false, // Опционально: повторный запрос при возврате к вкладке
            keepPreviousData: true
        }
    );

    useEffect(() => {
        instance.get("player").then(response => setMyId(response.data.id));

        const handleBeforeUnload = async (event) => {
            event.preventDefault();
            event.returnValue = ''; // Для браузеров, которые поддерживают обработку
            //setIsExitModalOpen(true);
            await instance.delete().then(console.log("Вы вышли из игры"))
            setGlobalStage("welcome")
            return '';
          };
      
          window.addEventListener('beforeunload', handleBeforeUnload);
      
          // Очистка обработчика при размонтировании
          return () => {
            window.removeEventListener('beforeunload', handleBeforeUnload);
          };
    }, [])



    if(players.length == 4) {
        return (
            <section className="players-box">
                <CurrentPlayer  player={players.find(player => player.id === myId)} setIsOpenModal={setIsExitModalOpen} gameData={gameData} stageTime={10} curStage={curStage} setCurStage={setCurStage}/>
                <AnotherPlayer player={players[1]} position={2}/>
                <AnotherPlayer player={players[2]} position={3}/>
                <AnotherPlayer player={players[3]} position={4}/>
            </section>
        )
    }

    

    else if(players.length == 2) {
        return (
            <section className="players-box">
                <ModalWindow isOpen={isExitModalOpen} onClose={() => setIsExitModalOpen(false)} onSubmit={async () => {
                    await instance.delete("player");
                    setGlobalStage("welcome")
                }}>
                    <h2>Желаете покинуть игру ?</h2>
                    <img className="modal-img" src="/public/img/InGamePictures/notStonks.jpg" alt="" />
                </ModalWindow>
                <CurrentPlayer player={players.find(player => player.id === myId)} setIsOpenModal={setIsExitModalOpen} gameData={gameData} stageTime={10} curStage={curStage} setCurStage={setCurStage} />
                <BlankPicture pictureNum={1}/>
                <AnotherPlayer player={players.find(player => player.id !== myId)} position={3}/>
                <BlankPicture pictureNum={1}/>
            </section>
        )
    }


    //Победа / Поражение
    else if(players.length == 1) {
        if(players[0].id === myId) {
            return (
                <section>
                    <ModalWindow isOpen={true} onSubmit={async () => {
                            await instance.delete("player", {code: gameData.code});
                            setGlobalStage("welcome");
                        }}>

                        <h2>Поздравляем, вы победили!</h2>
                        <img className="modal-img" src="/public/img/InGamePictures/stonks.webp" alt="stonks" />
                    </ModalWindow>
                    
                </section>
            )
        }

        else {
            return (
                <section>
                    <ModalWindow isOpen={true} onSubmit={() => {setGlobalStage("welcome")}}>
                        <h2>К сожалению, вы проиграли</h2>
                        <img className="modal-img" src="/public/img/InGamePictures/notStonks.jpg" alt="not stonks" />
                    </ModalWindow>
                </section>
            )
        }
    }
}