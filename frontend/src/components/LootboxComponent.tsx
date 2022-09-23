import '../App.css';
import '../fonts/fonts.css';
import { useEffect, useState, useRef } from 'react';
import { useLocation } from "react-router-dom";
import $ from 'jquery';
import { WalletAdapterNetwork } from "@solana/wallet-adapter-base";
import { PublicKey } from "@solana/web3.js";
import { Link } from 'react-router-dom';
import LogoImg from '../media/logo1.png';
import JSConfetti from 'js-confetti';
import '../animation.css';
import useLootboxProgram from './hooks/useLootboxProgram';
import useRootConfigFetcher from './hooks/useRootConfigFetcher';
import { RootConfig } from '../types/app_types';
import { getBuyTicketAccounts, redeemTicket } from '../util';

(window as any).global = window;
// @ts-ignore
window.Buffer = window.Buffer || require('buffer').Buffer;

declare global {
    interface Window { phantom: any; }
}

interface CustomizedState {
    picture: any,
    price: Array<number>,
    title: String,
    colors: Array<String>,
    win_variants: Array<Object>,
    lootboxAddress: PublicKey
}

const LootboxComponent = () => {

    const rootConfigFetcher = useRootConfigFetcher();
    const [rootConfig, setRootConfig] = useState<RootConfig | null>();
    const [textState, setTextState] = useState<String>('')

    useEffect(() => {
        async function call() {
            if (rootConfigFetcher && !rootConfig) {
                setRootConfig(await rootConfigFetcher());
            }
        }
        call();
    }, [rootConfigFetcher]);

    const location = useLocation();
    const state = location.state as CustomizedState;
    const { price, picture, title, colors, win_variants, lootboxAddress } = state;
    const [priceState, setPriceState] = useState(price[0]);
    const [value2, setValue2] = useState('');
    const network = WalletAdapterNetwork.Mainnet;
    const jsConfetti = new JSConfetti();
    const program = useLootboxProgram();

    const spinNymber2 = useRef<Number | null>(null);

    useEffect(() => {

        document.querySelectorAll('.typeLootBox_block')[0].addEventListener('click', function () {
            document.querySelectorAll('.typeLootBox_block')[0].classList.add('active');
            document.querySelectorAll('.typeLootBox_block')[1].classList.remove('active');
            document.querySelectorAll('.typeLootBox_block')[2].classList.remove('active');
        })
        document.querySelectorAll('.typeLootBox_block')[1].addEventListener('click', function () {
            document.querySelectorAll('.typeLootBox_block')[1].classList.add('active');
            document.querySelectorAll('.typeLootBox_block')[0].classList.remove('active');
            document.querySelectorAll('.typeLootBox_block')[2].classList.remove('active');
        })
        document.querySelectorAll('.typeLootBox_block')[2].addEventListener('click', function () {
            document.querySelectorAll('.typeLootBox_block')[0].classList.remove('active');
            document.querySelectorAll('.typeLootBox_block')[1].classList.remove('active');
            document.querySelectorAll('.typeLootBox_block')[2].classList.add('active');
        })

        initWheel2();

        function initWheel2() {
            var $wheel = $('.roulette-wrapper2 .wheel2'),
                row = "";
            row += "<div class='row'>";
            for (let i = 0; i < win_variants.length; i++) {
                row += `  <div class='element2 red'>${Object.values(win_variants[i])[1].toString()}<\/div>`;
            }
            row += "<\/div>";
            for (var x = 0; x < 29; x++) {
                $wheel.append(row);
            }
        }
    }, [])

    const spinClick = async () => {
        try {
            if (!program || !rootConfig) {
                return;
            }
            const accounts = await getBuyTicketAccounts(rootConfig, lootboxAddress, program.provider.publicKey!);
            const id = await program.methods
                .buyLootboxTicket()
                .accountsStrict({
                    ...accounts.accounts
                })
                .signers([accounts.ticket])
                .rpc();
            await new Promise(resolve => setTimeout(resolve, 1000));
            const el = document.getElementById("buttonSpin2");
            if (el !== null) {
                el.style.pointerEvents = 'none';
            }
            const i = await redeemTicket(accounts.ticket.publicKey);
            spinNymber2.current = i;
            setValue2(i.toString());
            setTextState(Object.values(win_variants[i])[1].toString())
            spinWheel2(Number(i));
            setTimeout(() => {
                if (el !== null) { el.style.pointerEvents = 'all' };
            }, 3000)
        }
        catch (e) {
            console.log(e)
        }
    }

    function spinWheel2(roll: number) {
        let AnimationLootbeers = document.querySelector('.AnimationLootbeers');
        let AnimationLootbeers_background_block_text = document.querySelector('.AnimationLootbeers_background_block_text');

        if (price.toString() === '0.5' || price.toString() === '0.2') {
            var $wheel = $('.roulette-wrapper2 .wheel2'),
                order = [0, 11, 5, 10, 6, 9, 7, 8, 1, 2, 13, 3, 12, 4],
                position = order.indexOf(roll);

            if (document.body.clientWidth > 910) {
                var rows = 6,
                    card = 150 + 1 * 2,
                    landingPosition = (rows * 14 * card) + (position * card);
                var randomize = Math.floor(Math.random() * 150) - (150 / 2);
            }
            else if (document.body.clientWidth > 710) {
                var rows = 6,
                    card = 100 + 1 * 2,
                    landingPosition = (rows * 14 * card) + (position * card);
                var randomize = Math.floor(Math.random() * 100) - (100 / 2);
            }
            else {
                var rows = 6,
                    card = 75 + 1 * 2,
                    landingPosition = (rows * 14 * card) + (position * card);
                var randomize = Math.floor(Math.random() * 75) - (75 / 2);
            }
        }
        else {
            var $wheel = $('.roulette-wrapper2 .wheel2'),
                order = [5, 6, 7, 8, 9, 10, 0, 1, 2, 3, 4],
                position = order.indexOf(roll);
            if (document.body.clientWidth > 910) {
                var rows = 12,
                    card = 150 + 1 * 2,
                    landingPosition = (rows * 11 * card) + (position * card);
                var randomize = Math.floor(Math.random() * 150) - (150 / 2);
            }
            else if (document.body.clientWidth > 710) {
                var rows = 12,
                    card = 100 + 1 * 2,
                    landingPosition = (rows * 11 * card) + (position * card);
                var randomize = Math.floor(Math.random() * 100) - (100 / 2);
            }
            else {
                var rows = 12,
                    card = 75 + 1 * 2,
                    landingPosition = (rows * 11 * card) + (position * card);
                var randomize = Math.floor(Math.random() * 75) - (75 / 2);
            }
        }

        landingPosition = landingPosition + randomize;

        var object = {
            x: Math.floor(Math.random() * 10) / 100,
            y: Math.floor(Math.random() * 5) / 100
        };

        $wheel.css({
            'transition-timing-function': 'cubic-bezier(0,' + object.x + ',' + object.y + ',1)',
            'transition-duration': '5s',
            'transform': 'translate3d(-' + landingPosition + 'px, 0px, 0px)'
        });

        setTimeout(function () {
            $wheel.css({
                'transition-timing-function': '',
                'transition-duration': '',
            });

            var resetTo = -(position * card + randomize);
            $wheel.css('transform', 'translate3d(' + resetTo + 'px, 0px, 0px)');

            if (AnimationLootbeers !== null) {
                (AnimationLootbeers as HTMLElement).style.display = 'block';
                (AnimationLootbeers_background_block_text as HTMLElement).style.animation = 'AnimationLootbeersKeyframes_text 3s';
                (AnimationLootbeers as HTMLElement).style.animation = 'AnimationLootbeersKeyframes 3s';

                let massColor: Array<string> = [];

                switch (price.toString()) {
                    case '0.05':
                        massColor = ["#7ab0b8", "#6aa7af", "#66a5ad", "#599da6", "#508d95", "#477d85"];
                        break;
                    case '0.2':
                        massColor = ["#f7d722", "#f6d309", "#ebc808", "#ddbd08", "#c5a807", "#ac9306"];
                        break;
                    case '0.5':
                        massColor = ["#9e22f7", "#9309f6", "#8e08eb", "#8508dd", "#7607c5", "#6706ac"];
                        break;
                }

                jsConfetti.addConfetti({
                    confettiNumber: 100,
                    confettiColors: massColor
                })

            }
        }, 5 * 1000);

        setTimeout(function () {
            if (AnimationLootbeers !== null) {
                (AnimationLootbeers as HTMLElement).style.display = 'none';
            }
        }, 8 * 1000)
    }


    const correctPrice = (name: String) => {
        switch (name) {
            case 'NORMIE': {
                setPriceState(price[0]);
                break;
            }
            case 'DEGEN': {
                setPriceState(price[1]);
                break;
            }
            case 'WHALE': {
                setPriceState(price[2]);
                break;
            }
        }
    }

    const style1 = {
        'background': {
            background: `linear-gradient(to right, ${colors[0]}, ${colors[1]}, ${colors[0]})`
        },
        'background2': {
            background: `linear-gradient(to right, ${colors[0]}, ${colors[0]}, ${colors[1]}, ${colors[0]}, ${colors[0]})`
        },
        'backgroundBtn': {
            background: `${colors[1]}`
        },
        'borderOnfoItem': {
            border: `2px solid ${colors[1]}`
        }
    }

    return (
        <div className="LootboxComponent">
            <div className="AnimationLootbeers" style={{ display: 'none' }}>
                <div className="AnimationLootbeers_background">
                    <div className="AnimationLootbeers_background_block">
                        <div className="AnimationLootbeers_background_block_text">{textState}</div>
                    </div>
                </div>
            </div>
            <div className='AppBacdrop_title'>
                <Link to='/lootbeers_ts' className='Loot_color_darkThemeLink_2'>
                    <div className='logo imageLogoAppBacdrop_title'></div>
                    <h1>LOOTBEERS</h1>
                </Link>
            </div>
            <div className='LogoImg'>
                <img src={LogoImg} alt="" className='LogoImg_img' />
            </div>
            <div className='TextMain'>
                <div className='background_TextMain'></div>
                <div className="LootboxBacdrop">
                    <div className='LootboxComponent_title'>
                        <div className='LootboxComponent_title_bottom' style={style1.background}></div>
                        {title}
                    </div>
                    <div className='LootboxComponent_image_block'>
                        <div className='LootboxComponent_image_block_background'>
                            <div className='radial' style={{ background: `radial-gradient(ellipse closest-side at center center, ${colors[1]}, rgba(0, 0, 0, 0))` }}></div>
                        </div>
                        <img src={picture} alt="" className='LootboxComponent_image'></img>
                    </div>
                    <div className='typeLootBox'>
                        <div className='NORMIE typeLootBox_block active' onClick={() => correctPrice('NORMIE')}>NORMIE</div>
                        <div className='DEGEN typeLootBox_block' onClick={() => correctPrice('DEGEN')}>DEGEN</div>
                        <div className='WHALE typeLootBox_block' onClick={() => correctPrice('WHALE')}>WHALE</div>
                    </div>
                    <div className='roulette-wrapper2' style={{
                        border: `3px solid ${colors[1]}`,
                        borderImage: `linear-gradient(90deg, ${colors[0]}, ${colors[1]}, ${colors[0]}) 1`
                    }}>
                        <div className='wheel2'></div>
                        <div className='win2line'></div>
                        <div className='win2'>
                            <svg version="1.0" xmlns="http://www.w3.org/2000/svg"
                                width="30.000000pt" xHeight="30.000000pt" viewBox="0 0 30.000000 30.000000"
                                preserveAspectRatio="xMidYMid meet">
                                <g transform="translate(0.000000,30.000000) scale(0.100000,-0.100000)"
                                    fill="#ffffff3d" stroke="none">
                                    <path d="M90 174 c-30 -26 -56 -55 -58 -65 -5 -27 4 -24 60 21 28 22 55 40 60 40 5 0 31 -18 57 -40 53 -45 61 -48 61 -21 0 14 -107 112 -122 111 -1 0 -28 -21 -58 -46z" />
                                </g>
                            </svg>
                        </div>
                    </div>
                    <div className='LootboxComponent_block_open'>
                        <div className='contentLine' style={style1.background2}></div>
                        <div className='LootboxComponent_price buttonSpin2 right' onClick={spinClick} id='buttonSpin2' style={{ pointerEvents: 'all' }}>
                            <div className='content' style={style1.backgroundBtn}></div>
                            <div className='LootboxComponent_price_position'>
                                OPEN&nbsp;{price}&nbsp;SOL
                            </div>
                        </div>
                        <div className='LootboxComponent_price buttonSpin2 righttwo' onClick={spinClick} id='buttonSpin3' style={{ pointerEvents: 'all' }}>
                            <div className='content righttwo'></div>
                            <div className='LootboxComponent_price_position righttwo'>
                                INCREASE THE CHANCES BY 100 $LUM
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default LootboxComponent;
