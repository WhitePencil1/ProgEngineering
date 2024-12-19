/* eslint-disable react/prop-types */
import { useEffect, useState } from "react";
import { instance } from "../../../utils/axios";

export default function Timer ({ initialTime, nextStage }) {
    const [time, setTime] = useState(initialTime); // Устанавливаем начальное время из пропса

    instance.defaults.timeout = 0;

    async function changeStage() {
        try {
            console.log("Запрос " + nextStage + " начат");
            await instance.post(nextStage)
                .then(response => console.log(response));
            //await instance.get("players");
            console.log("Запрос выполнен");
        } catch(error) {
            console.error(error);
        }finally {
            setTime(initialTime); // Сбрасываем таймер
        }
    }

    useEffect(() => {
        if (time <= 0) {
            changeStage()
            return; // Прекращаем выполнение, чтобы не запускать таймер
        }
        // Запускаем таймер
        const timerId = setInterval(() => {
            setTime((prevTime) => prevTime - 1);
        }, 1000);

        // Очищаем таймер при размонтировании компонента
        return () => clearInterval(timerId);
    }, [time]);


    return (
        <li>
            Time left: {time}s
        </li>
    );
};

    