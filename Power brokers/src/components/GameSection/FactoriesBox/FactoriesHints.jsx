/* eslint-disable react/prop-types */
import { useState } from "react"
import { instance } from "../../../utils/axios";
import "./FactoriesBox.css"

export default function FactoriesHints({factories, id, curStage, isHidden, factoryRequest, setFactoryRequest}) {

    const [isBlocked, setIsBlocked] = useState(false);

    async function factoryPutRequest(esmCount) {
        try {
            console.log("Отправка запроса...");
            await instance.post("player/putEsm", {id: id, esm: esmCount}).then((request) => console.log(request))
            console.log("Выполнено!");
        } catch(error) {
            console.error(error)
        } finally {
            await instance.get("room/players").then((request) => console.log(request))
        }
    }
    


    switch(curStage) {
        case 3:
            if(factories[id].level === 2) {
                return(
                    <div className={"factory-hints-container " + (isHidden !== id || isBlocked ? "hide" : "")}>
                        <ul className="factory-hint" onClick={() => factoryPutRequest(1)}>
                            <li><img className="factory-hint-item" src="/public/img/moneyIcon.png" alt="" /></li>
                            <li>200 $</li>
                            <li><img className="factory-hint-item" src="/public/img/time.png" alt="" /></li>
                            <li>2 month</li>
                            <li><img className="factory-hint-item" src="/public/img/materialUnitIcon.png" alt="" /></li>
                            <li>1 mu</li>
                        </ul>
                    </div>
                )
            }
            else {
                return(
                    <div className={"factory-hints-container " + (isHidden !== id ? "hide" : "")}>
                        <ul className="factory-hint" onClick={() => factoryPutRequest(1)}>
                            <li><img className="factory-hint-item" src="/public/img/moneyIcon.png" alt="" /></li>
                            <li>200 $</li>
                            <li><img className="factory-hint-item" src="/public/img/time.png" alt="" /></li>
                            <li>2 month</li>
                            <li><img className="factory-hint-item" src="/public/img/materialUnitIcon.png" alt="" /></li>
                            <li>1 mu</li>
                        </ul>
                        <ul className="factory-hint" onClick={() => factoryPutRequest(2)}>
                            <li><img className="factory-hint-item" src="/public/img/moneyIcon.png" alt="" /></li>
                            <li>200 $</li>
                            <li><img className="factory-hint-item" src="/public/img/time.png" alt="" /></li>
                            <li>2 month</li>
                            <li><img className="factory-hint-item" src="/public/img/materialUnitIcon.png" alt="" /></li>
                            <li>1 mu</li>
                        </ul>
                    </div>
                )
            }
            
        default:
            break;
    }
}