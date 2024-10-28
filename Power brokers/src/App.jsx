import { useState } from 'react'
import './App.css'
import Background from './components/Background'
import ConnectStage from './components/BookStages/ConnectStage'
import CreateStage from './components/BookStages/CreateStage'
import WelcomeSection from './components/WelcomeSection/WelcomeSection'
import RegistrationStage from './components/BookStages/RegistrationStage'

function App() {
  const [stage, setStage] = useState("welcome")
  const [isRegistered, setIsRegistered] = useState(false) /*Кастыль для секции регистрации*/ 
  const [playerRole, setPlayerRole] = useState("")

  return (
    <>
      <Background />

      {stage == "welcome" && <WelcomeSection setNextStage = {setStage} setRole={setPlayerRole}/>}

      {stage == "registration" && <RegistrationStage setStage={setStage} isRegistered={isRegistered} setIsRegistered={setIsRegistered}/>}

      {(stage == "roomActivity" && playerRole == "create") && <CreateStage setStage={setStage}/>}

      {(stage == "roomActivity" && playerRole == "connect") && <ConnectStage />}

    </>
  )
}

export default App
