import './WelcomeButton.css'

// eslint-disable-next-line react/prop-types
export default function WelcomeButton ({ children, onClick }) {
    return (
        <button className="welcome-button" onClick={onClick}>{children}</button>
    )
}