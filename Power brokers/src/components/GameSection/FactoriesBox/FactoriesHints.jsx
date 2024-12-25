/* eslint-disable react/prop-types */
import { useState } from "react"
import { instance } from "../../../utils/axios";
import "./FactoriesBox.css"

export default function FactoriesHints({factories, id, curStage, isHidden, factoryRequest, setFactoryRequest}) {

    const [isBlocked, setIsBlocked] = useState(false);

    async function factoryPutRequest(esmCount) {
        try {
            console.log("Отправка запроса...");
            await instance.post("player/putEsm", {Id: id, Esm: esmCount}).then((request) => console.log(request))
            console.log("Выполнено!");
        } catch(error) {
            console.log(error.response)
        }
    }

    async function getFactoryCredit() {
        try {
            console.log("Отправка запроса на получение ссуды...");
            await instance.post("player/GetCredit", {Id: id}).then((request) => console.log(request))
            console.log("Выполнено!");
        } catch(error) {
            console.log(error.response)
        }
    }

    async function buildFactory(isAutoFactoryRequest) {
        try {
            console.log("Отправка запроса на строительство фабрики...");
            await instance.post("player/buildFactory", {Id: id, Auto: isAutoFactoryRequest}).then((request) => console.log(request))
            console.log("Выполнено!");
        } catch(error) {
            console.log(error.response)
        }
    }
    

    async function upgradeFactory() {
        try {
            console.log("Отправка запроса на строительство фабрики...");
            await instance.post("player/upgradeFactory", {Id: id}).then((request) => console.log(request))
            console.log("Выполнено!");
        } catch(error) {
            console.log(error.response)
        }
    }


    switch(curStage) {
        case 3:
            if(factories[id].level === 2 || factories[id].level === 3) {

                return (factories[id].esm !== 0 ? <></> :
                    <div className={"factory-hints-container " + (isHidden !== id ? "hide" : "")}>
                        <ul className="factory-hint" onClick={() => factoryPutRequest(1)}>
                            <li><img className="factory-hint-item" src="/public/img/moneyIcon.png" alt="" /></li>
                            <li>200 $</li>
                            <li><img className="factory-hint-item" src="/public/img/time.png" alt="" /></li>
                            <li>1 month</li>
                            <li><img className="factory-hint-item" src="/public/img/materialUnitIcon.png" alt="" /></li>
                            <li>1 mu</li>
                        </ul>
                    </div>
                )
            }

            else if(factories[id].level === 4) {
                return(factories[id].esm !== 0 ? <></> :
                    <div className={"factory-hints-container " + (isHidden !== id ? "hide" : "")}>
                        <ul className="factory-hint left" onClick={() => factoryPutRequest(1)}>
                            <li><img className="factory-hint-item" src="/public/img/moneyIcon.png" alt="" /></li>
                            <li>200 $</li>
                            <li><img className="factory-hint-item" src="/public/img/time.png" alt="" /></li>
                            <li>1 month</li>
                            <li><img className="factory-hint-item" src="/public/img/materialUnitIcon.png" alt="" /></li>
                            <li>1 mu</li>
                        </ul>
                        <ul className="factory-hint right" onClick={() => factoryPutRequest(2)}>
                            <li><img className="factory-hint-item" src="/public/img/moneyIcon.png" alt="" /></li>
                            <li>200 $</li>
                            <li><img className="factory-hint-item" src="/public/img/time.png" alt="" /></li>
                            <li>1 month</li>
                            <li><img className="factory-hint-item" src="/public/img/materialUnitIcon.png" alt="" /></li>
                            <li>2 mu</li>
                        </ul>
                    </div>
                )
            }
            break;
            
        


        case 7:
            if(factories[id].level === 2 || factories[id].level === 3) {
                return(factories[id].isCredit ? <></> :
                    <div className={"factory-hints-container " + (isHidden !== id ? "hide" : "")}>
                        <ul className="factory-hint" onClick={() => getFactoryCredit()}>
                            <li><img className="factory-hint-item" src="/public/img/moneyIcon.png" alt="" /></li>
                            <li>1000 $</li>
                        </ul>
                    </div>
                )
            }
            else if(factories[id].level === 4) {
                return(factories[id].isCredit ? <></> :
                    <div className={"factory-hints-container " + (isHidden !== id ? "hide" : "")}>
                        <ul className="factory-hint" onClick={() => getFactoryCredit()}>
                            <li><img className="factory-hint-item" src="/public/img/moneyIcon.png" alt="" /></li>
                            <li>2000 $</li>
                        </ul>
                    </div>
                )
                
            }
            break;
        
        case 8:
            //Не построенный завод
            if(factories[id].level === -1) {
                return (
                    <div className={"factory-hints-container " + (isHidden !== id ? "hide" : "")}>
                        <ul className="factory-hint left" onClick={() => buildFactory(false)}>
                            <img src="/public/img/Factories/simpleFactory.png" alt="" className="factory-img"/>
                            <li><img className="factory-hint-item" src="/public/img/moneyIcon.png" alt="" /></li>
                            <li>200 $</li>
                            <li><img className="factory-hint-item" src="/public/img/time.png" alt="" /></li>
                            <li>6 month</li>
                        </ul>
                        <ul className="factory-hint right" onClick={() => buildFactory(true)}>
                            <img src="/public/img/Factories/improvedFactory.png" alt="" className="factory-img"/>
                            <li><img className="factory-hint-item" src="/public/img/moneyIcon.png" alt="" /></li>
                            <li>400 $</li>
                            <li><img className="factory-hint-item" src="/public/img/time.png" alt="" /></li>
                            <li>13 month</li>
                        </ul>
                    </div>
                );
            } 
            break;


        case 9: 
            if(factories[id].level === 2) {
                return (
                    <div className={"factory-hints-container " + (isHidden !== id ? "hide" : "")}>
                        <ul className="factory-hint" onClick={() => upgradeFactory()}>
                            <img src="/public/img/Factories/improvedFactory.png" alt="" className="factory-img"/>
                            <li><img className="factory-hint-item" src="/public/img/moneyIcon.png" alt="" /></li>
                            <li>400 $</li>
                            <li><img className="factory-hint-item" src="/public/img/time.png" alt="" /></li>
                            <li>13 month</li>
                        </ul>
                    </div>
                )
            }
            break;
    }
}