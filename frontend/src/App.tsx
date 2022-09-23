import Wallet from "./components/Wallet/Wallet";
import './animation.css';
import React, { useEffect } from 'react';

function App() {

  return (
    <div className="MainBrowserComp" id="MainBrowserComp">
      <div className="StatLootbeers">
        Total opened:&nbsp;<span style={{ color: '#ffc000', marginRight: '30px' }}>0 LOOTS</span> 0h volume:&nbsp;<span style={{ color: '#ffc000', marginRight: '30px' }}>0 SOL</span> All-time volume:&nbsp;<span style={{ color: '#ffc000', marginRight: '30px' }}>0 SOL</span> 7-days active users:&nbsp;<span style={{ color: '#ffc000', marginRight: '0px' }}>0</span>
      </div>
      {/* <div className="AnimationLootbeers" style={{ display: 'none' }}>
        <div className="AnimationLootbeers_background"></div>
        <div className="AnimationLootbeers_background_block"> */}
          {/* <div className="linzeBlock">
            <div className="lens-center"></div>
            <div className="circle-1"></div>
            <div className="circle-2"></div>
          </div> */}
          {/* <div id="light"></div> */}
          {/* <div className="AnimationLootbeers_background_block_text">LootboxTime</div>
        </div>
      </div> */}
      <div style={{ position: 'relative', top: 0, right: 0, width: '100%', height: '100%' }}>
        <Wallet />
      </div>
    </div>
  );
}

export default App;