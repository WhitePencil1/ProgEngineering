import "./FactoriesBox.css"


export default function FactoriesStatusHints() {


    return(
        <ul className="factories-status">
            <li><img src="/public/img/inProduction.png" alt="" />1 mu</li>
            <li><img src="/public/img/underConstruction.png" alt="" />3 month</li>
        </ul>
    )
}