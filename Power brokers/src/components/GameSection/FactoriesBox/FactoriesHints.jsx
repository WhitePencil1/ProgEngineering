/* eslint-disable react/prop-types */
import { useState } from "react"
import "./FactoriesBox.css"

export default function FactoriesHints({factories, id, curStage, isHidden, factoryRequest, setFactoryRequest}) {

    const [isBlocked, setIsBlocked] = useState(false);

    function addFactoryRequest(esm) {
        setFactoryRequest([].concat([{id: id, esm: esm}], factoryRequest))
        factories[id].esm == 0 ? setIsBlocked(false) : setIsBlocked(true);
    }
    


    switch(curStage) {
        case 3:
            if(factories[id].level === 2) {
                return(
                    <div className={"factory-hints-container " + (isHidden !== id || isBlocked ? "hide" : "")}>
                        <ul className="factory-hint" onClick={() => addFactoryRequest(1)}>
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
                        <ul className="factory-hint" onClick={() => addFactoryRequest(1)}>
                            <li><img className="factory-hint-item" src="/public/img/moneyIcon.png" alt="" /></li>
                            <li>200 $</li>
                            <li><img className="factory-hint-item" src="/public/img/time.png" alt="" /></li>
                            <li>2 month</li>
                            <li><img className="factory-hint-item" src="/public/img/materialUnitIcon.png" alt="" /></li>
                            <li>1 mu</li>
                        </ul>
                        <ul className="factory-hint" onClick={() => addFactoryRequest(2)}>
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