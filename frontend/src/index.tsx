import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import WalletProviderComponent from './provider/walletProvider';
import reportWebVitals from './reportWebVitals';
import { BrowserRouter, Routes, Route } from "react-router-dom";
import Wallet from './components/Wallet/Wallet';

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);

root.render(
  // <React.StrictMode>
  <WalletProviderComponent>
    <App />
  </WalletProviderComponent>
  // </React.StrictMode>
);

reportWebVitals();