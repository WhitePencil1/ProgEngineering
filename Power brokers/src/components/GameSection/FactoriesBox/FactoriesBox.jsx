/* eslint-disable react/prop-types */
import "./FactoriesBox.css"
import { stages } from "../../../data";
import FactoriesHints from "./FactoriesHints";
import { useState } from "react";
import FactoriesStatusHints from "./FactoriesStatusHints";

// eslint-disable-next-line react/prop-types
export default function FactoriesBox({isMainPlayer, factories, curStage, factoryRequest, setFactoryRequest}) {
    const classes = (isMainPlayer == true ? "factory" : "factory another-player-factory");


    const [showHintId, setShowHintId] = useState(-1);

    // if(curStage) {
    //     if(stages[curStage].playersAction === "muDistribution") {
    //         console.log("РАспределяй");
    //     }
    // }


    const factoriesByLevels = {
        "-1": "./img/Factories/emptyFactory.png",
        0: "./img/Factories/simpleFactoryСonstruction.png",
        1: "./img/Factories/improvedFactoryСonstruction.png",
        2: "./img/Factories/simpleFactory.png",
        4: "./img/Factories/improvedFactory.png"
    };



    if (isMainPlayer) {
        return (
            <ul className="player-factories-box">
                <li onMouseOver={() => setShowHintId(0)} onMouseOut={() => setShowHintId(-1)}>
                    <FactoriesStatusHints factoryData={factories[0]}/>
                    <FactoriesHints factories={factories} id={0} curStage={curStage} isHidden={showHintId} factoryRequest={factoryRequest} setFactoryRequest={setFactoryRequest}/>
                    <img className={classes} src={factoriesByLevels[factories[0].level]} alt="factory"/>
                </li>

                <li onMouseOver={() => setShowHintId(1)} onMouseOut={() => setShowHintId(-1)}>
                    <FactoriesStatusHints factoryData={factories[1]}/>
                    <FactoriesHints factories={factories} id={1} curStage={curStage} isHidden={showHintId} factoryRequest={factoryRequest} setFactoryRequest={setFactoryRequest}/>
                    <img className={classes} src={factoriesByLevels[factories[1].level]} alt="factory"/>
                </li>

                <li onMouseOver={() => setShowHintId(2)} onMouseOut={() => setShowHintId(-1)}>
                    <FactoriesStatusHints factoryData={factories[2]}/>
                    <FactoriesHints factories={factories} id={2} curStage={curStage} isHidden={showHintId} factoryRequest={factoryRequest} setFactoryRequest={setFactoryRequest}/>
                    <img className={classes} src={factoriesByLevels[factories[2].level]} alt="factory" />
                </li>

                <li onMouseOver={() => setShowHintId(3)} onMouseOut={() => setShowHintId(-1)}>
                    <FactoriesStatusHints factoryData={factories[3]}/>
                    <FactoriesHints factories={factories} id={3} curStage={curStage} isHidden={showHintId} factoryRequest={factoryRequest} setFactoryRequest={setFactoryRequest}/>
                    <img className={classes} src={factoriesByLevels[factories[3].level]} alt="factory" />
                </li>

                <li onMouseOver={() => setShowHintId(4)} onMouseOut={() => setShowHintId(-1)}>
                    <FactoriesStatusHints factoryData={factories[4]}/>
                    <FactoriesHints factories={factories} id={4} curStage={curStage} isHidden={showHintId} factoryRequest={factoryRequest} setFactoryRequest={setFactoryRequest}/>
                    <img className={classes} src={factoriesByLevels[factories[4].level]} alt="factory" />
                </li>
            </ul>
        )
    }



    else {
        function getFactoryCounter(factoryLevel) {
            let total = factories.reduce((counter, factory) => {
                if (factory.level === factoryLevel) {
                    counter++; // Увеличиваем счетчик
                }
                return counter;
            }, 0)
            return total;
        }

        return (
            <ul className="player-factories-box another-player-factories-box">
                <li><img className={classes} src="./img/Factories/simpleFactory.png" alt="" /><div className="factory-counter">X{getFactoryCounter(2)}</div></li>
                <li><img className={classes} src="./img/Factories/improvedFactory.png" alt="" /><div className="factory-counter">X{getFactoryCounter(4)}</div></li>
            </ul>
        )
    }
    
}