/* eslint-disable react/prop-types */
import "./FactoriesBox.css"

// eslint-disable-next-line react/prop-types
export default function FactoriesBox({isMainPlayer, factories}) {
    const classes = (isMainPlayer == true ? "factory" : "factory another-player-factory");

    const factoriesByLevels = {
        "-1": "./img/Factories/emptyFactory.png",
        2: "./img/Factories/simpleFactory.png",
        4: "./img/Factories/improvedFactory.png"
    };



    if (isMainPlayer) {
        return (
            <ul className="player-factories-box">
                <li><img className={classes} src={factoriesByLevels[factories[0].level]} alt="factory" /></li>
                <li><img className={classes} src={factoriesByLevels[factories[1].level]} alt="factory" /></li>
                <li><img className={classes} src={factoriesByLevels[factories[2].level]} alt="factory" /></li>
                <li><img className={classes} src={factoriesByLevels[factories[3].level]} alt="factory" /></li>
                <li><img className={classes} src={factoriesByLevels[factories[4].level]} alt="factory" /></li>
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