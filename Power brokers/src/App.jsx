import { useState } from 'react'
import './App.css'
import Background from './components/Background'
import ConnectStage from './components/BookStages/ConnectStage'
import CreateStage from './components/BookStages/CreateStage'
import WelcomeSection from './components/WelcomeSection/WelcomeSection'
import RegistrationStage from './components/BookStages/RegistrationStage'
import GameSection from './components/GameSection/GameSection'


function App() {
  const [stage, setStage] = useState("game")
  const [isRegistered, setIsRegistered] = useState(false) /*Кастыль для секции регистрации*/ 
  const [playerRole, setPlayerRole] = useState("")

  const players = {
    1: {nickname: "Kiner", avatar: ""},
    2: {nickname: "Oxotnik22012", avatar: ""},
    3: {nickname: "Sfinkterion", avatar: ""},
    4: {nickname: "Erik Penisov", avatar: ""}
  };

  return (
    <>
      {stage != "game" ? <Background /> : <GameSection players={players}/>}

      {stage == "welcome" && <WelcomeSection setNextStage = {setStage} setRole={setPlayerRole}/>}

      {stage == "registration" && <RegistrationStage setStage={setStage} isRegistered={isRegistered} setIsRegistered={setIsRegistered}/>}

      {(stage == "roomActivity" && playerRole == "create") && <CreateStage setStage={setStage}/>}

      {(stage == "roomActivity" && playerRole == "connect") && <ConnectStage setStage={setStage} />}

    </>
  )
}

export default App
