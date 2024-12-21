/* eslint-disable react/prop-types */
import { useEffect, useState } from "react";
import { instance } from "../../../utils/axios";
import { stages } from "../../../data";

export default function Timer ({ curStage, setCurStage, isSubmit, setIsSubmit}) {
    
    instance.defaults.timeout = 0;


    async function changeStage() {
        try {
            switch(curStage) {
                case 0:
                case 1:
                    console.log("Запрос " + stages[curStage].api + " отправлен");
                    await instance.post(stages[curStage].api).then(response => console.log(response));
                    await instance.get("room/players");
                    console.log("Запрос выполнен");
                    break;


                //Покупка ЕСМ
                case 2:
                    // if(!isSubmit) {
                    //     console.log("Запрос " + stages[curStage].api + "Count=0&Price=0" + " отправлен");
                    //     await instance.post(stages[curStage].api, {Count: 0, Price: 0}).then(response => console.log(response));
                    //     console.log("Запрос выполнен");
                    //     break;
                    // }
                    // setIsSubmit(false);
                    // break;
                    console.log("Запрос " + stages[curStage].api + "Count=0&Price=0" + " отправлен");
                    await instance.post(stages[curStage].api, {Count: 0, Price: 0}).then(response => console.log(response));
                    console.log("Запрос выполнен");
                    break;

                case 3:
                    console.log("Время распределения ЕСМ вышло! Переключаю на этап продажи...")
                    await instance.post(stages[curStage].api).then(response => console.log(response));
                    console.log("Этап изменен");
                    break;

                case 4:
                    console.log("Запрос " + stages[curStage].api + "Count=0&Price=0" + " отправлен");
                    await instance.post(stages[curStage].api, {Count: 0, Price: 0}).then(response => console.log(response));
                    console.log("Запрос выполнен");
                    break;

                case 5:
                    console.log("Запрос этапа ВЫПЛАТЫ ПРОЦЕНТОВ отправлен");
                    await instance.post(stages[curStage].api).then(response => console.log(response));
                    console.log("Запрос выполнен");
                    break;

                case 6:
                    console.log("Запрос этапа ВЫПЛАТЫ ССУДЫ отправлен");
                    await instance.post(stages[curStage].api).then(response => console.log(response));
                    console.log("Запрос выполнен");
                    break;

                case 7:
                    console.log("Запрос окончания этапа ПОЛУЧЕНИЯ ССУДЫ отправлен");
                    await instance.post(stages[curStage].api).then(response => console.log(response));
                    console.log("Запрос выполнен");
                    break;

            }
        } catch(error) {
            console.error(error);
        } finally {
            setCurStage(curStage + 1)
            setTime(stages[curStage+1].stageTime); // Сбрасываем таймер
        }




        //     console.log("Запрос " + stages[curStage].api + " отправлен");
        //     await instance.post(stages[curStage].api)
        //         .then(response => console.log(response));
        //     await instance.get("players");
        //     console.log("Запрос выполнен");
        // } catch(error) {
        //     console.error(error);
        // }finally {
        //     setTime(stages[curStage].stageTime); // Сбрасываем таймер
        // }
    }
    

    const [time, setTime] = useState(stages[curStage].stageTime); // Устанавливаем начальное время из пропса

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

    