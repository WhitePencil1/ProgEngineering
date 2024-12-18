import { useState } from 'react'
import './App.css'
import Background from './components/Background'
import ConnectStage from './components/BookStages/ConnectStage'
import CreateStage from './components/BookStages/CreateStage'
import WelcomeSection from './components/WelcomeSection/WelcomeSection'
import RegistrationStage from './components/BookStages/RegistrationStage'
import GameSection from './components/GameSection/GameSection'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';

const queryClient = new QueryClient();


function App() {
  const [stage, setStage] = useState("welcome")
  const [isRegistered, setIsRegistered] = useState(false) /*Кастыль для секции регистрации*/ 
  const [playerRole, setPlayerRole] = useState("create")

  const [roomCode, setRoomCode] = useState("");

  const [players, setPlayers] = useState([]);


  return (
    <QueryClientProvider client={queryClient}>
      {stage != "game" ? <Background /> : <GameSection players={players} setPlayers={setPlayers}/>}

      {stage == "welcome" && <WelcomeSection setNextStage = {setStage} setRole={setPlayerRole}/>}

      {stage == "registration" && <RegistrationStage playerRole={playerRole} setStage={setStage} isRegistered={isRegistered} setIsRegistered={setIsRegistered} players={players} setPlayers={setPlayers} setRoomCode={setRoomCode}/>}

      {(stage == "roomActivity" && playerRole == "create") && <CreateStage setStage={setStage} players={players} setPlayers={setPlayers} roomCode={roomCode}/>}

      {(stage == "roomActivity" && playerRole == "connect") && <ConnectStage setStage={setStage} setPlayers={setPlayers} players={players} roomCode={roomCode} setRoomCode={setRoomCode} />}

    </QueryClientProvider>
  )
}

export default App
