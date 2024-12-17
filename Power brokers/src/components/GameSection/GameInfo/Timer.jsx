/* eslint-disable react/prop-types */
import { useEffect, useState } from "react";


export default function Timer ({ initialTime, onTimeEnd }) {
    const [time, setTime] = useState(initialTime); // Устанавливаем начальное время из пропса

    useEffect(() => {
        if (time === 0) {
            if (onTimeEnd) {
                onTimeEnd(); // Вызываем переданную функцию, если время закончилось
            } 
            return; // Прекращаем выполнение, чтобы не запускать таймер
        }

        // Запускаем таймер
        const timerId = setInterval(() => {
            
            setTime((prevTime) => Math.max(prevTime - 1, 0));
            
        }, 1000);

        // Очищаем таймер при размонтировании компонента
        return () => clearInterval(timerId);
    }, [time, onTimeEnd]);

    return (
        <li>
            Time left: {time}s
        </li>
    );
};

    