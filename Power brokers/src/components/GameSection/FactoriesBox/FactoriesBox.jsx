import "./FactoriesBox.css"

// eslint-disable-next-line react/prop-types
export default function FactoriesBox({isMainPlayer}) {
    const classes = (isMainPlayer == true ? "factory" : "factory another-player-factory");
    if (isMainPlayer) {
        return (
            <ul className="player-factories-box">
                <li><img className={classes} src="./img/simpleFactory.png" alt="" /></li>
                <li><img className={classes} src="./img/simpleFactory.png" alt="" /></li>
                <li><img className={classes} src="./img/improvedFactory.png" alt="" /></li>
                <li><img className={classes} src="./img/emptyFactory.png" alt="" /></li>
                <li><img className={classes} src="./img/simpleFactory.png" alt="" /></li>
            </ul>
        )
    }

    else {
        return (
            <ul className="player-factories-box another-player-factories-box">
                <li><img className={classes} src="./img/simpleFactory.png" alt="" /><div className="factory-counter">X2</div></li>
                <li><img className={classes} src="./img/improvedFactory.png" alt="" /><div className="factory-counter">X2</div></li>
            </ul>
        )
    }
    
}