import axios from "axios";


export const instance = axios.create({
    baseURL: 'https://localhost:7269/api/',
    //ПРОД
    //baseURL: 'http://176.113.82.40:7269/api/',
    timeout: 1000,
    headers: {'Content-Type': 'application/json'},
    withCredentials: true
  });