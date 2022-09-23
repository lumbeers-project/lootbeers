import './loot_block.css';
import { FC } from 'react';
import { Link } from 'react-router-dom';
import React, { useEffect, useState } from 'react';
import JSConfetti from 'js-confetti';
import useRewardFetcher from '../hooks/useRewardFetcher';
import { NftReward, NoneReward, Reward, SolReward, TokenReward } from '../../types/app_types';
import { mapRewards } from '../../util';
import { useAnchorProvider } from '../../provider/walletProvider';
import { PublicKey } from '@solana/web3.js';

// interface Props {
//     picture: any,
//     price: number,
//     title: String,
//     colors: Array<string>,
//     text: Array<string>,
//     win_variants: Array<Object>
// }

interface Props {
    picture: any,
    price: number,
    title: String,
    colors: Array<string>,
    text: Array<string>,
    winVariants: any,
    lootboxAddress: PublicKey
}

const Loot_block: FC<Props> = ({ picture, price, title, colors, text, winVariants, lootboxAddress }) => {

    const provider = useAnchorProvider();
    const el = document.querySelector('.Loot_color_darkTheme');
    const [isActive, setIsActive] = useState(false);

    const [colorInfoItem, setcolorInfoItem] = useState<String>(`#FFFFFF`);
    // const [opacityInfoItem, setopacityInfoItem] = useState<String>('0.3')

    useEffect(() => {
        const infoItem = document.querySelectorAll(".infoItem");
        const darkThemeHover = document.querySelectorAll(".darkThemeHover");
        const darkThemeHover_text = document.querySelectorAll(".darkThemeHover_text");
        const Loot_color_darkTheme = document.querySelectorAll(".Loot_color_darkTheme");
        const Loot_color_darkTheme_button = document.querySelectorAll(".Loot_color_darkTheme_button");

        if (Loot_color_darkTheme !== null) {
            for (let i = 0; i <= Loot_color_darkTheme.length; i++) {
                if (Loot_color_darkTheme[i]) {
                    Loot_color_darkTheme[i].addEventListener("mouseover", function () {
                        (darkThemeHover[i] as HTMLElement).style.opacity = "0.9";
                        // console.log((Loot_color_darkTheme_button[i] as HTMLElement).style.visibility);
                        (Loot_color_darkTheme_button[i] as HTMLElement).style.visibility = "visible";
                        (Loot_color_darkTheme_button[i] as HTMLElement).style.display = "block";
                        (Loot_color_darkTheme_button[i] as HTMLElement).style.opacity = "1";
                        setIsActive(true)
                    });

                    Loot_color_darkTheme[i].addEventListener("click", function () {
                        (darkThemeHover[i] as HTMLElement).style.opacity = "0.9";
                        if ((Loot_color_darkTheme_button[i] as HTMLElement).style.visibility !== 'visible') {
                            setIsActive(true)
                        }
                        (Loot_color_darkTheme_button[i] as HTMLElement).style.visibility = "visible";
                        (Loot_color_darkTheme_button[i] as HTMLElement).style.display = "block";
                        (Loot_color_darkTheme_button[i] as HTMLElement).style.opacity = "1";
                    });

                    Loot_color_darkTheme[i].addEventListener("mouseout", function () {
                        (darkThemeHover[i] as HTMLElement).style.opacity = "0";
                        (Loot_color_darkTheme_button[i] as HTMLElement).style.visibility = "hidden";
                        (Loot_color_darkTheme_button[i] as HTMLElement).style.display = "none";
                        (Loot_color_darkTheme_button[i] as HTMLElement).style.opacity = "0";
                        setIsActive(false)
                    });
                }
            }
        }

        if (infoItem !== null && darkThemeHover !== null && darkThemeHover_text !== null) {
            for (let i = 0; i <= infoItem.length; i++) {
                let opacityInfoItem = '0.3';
                let colorInfoItem2 = `${colors[1]}`;
                if (infoItem[i]) {
                    infoItem[i].addEventListener("mouseover", function () {
                        setcolorInfoItem(`${colors[1]}`);
                        // (infoItem[i] as HTMLElement).style.color = `red`;
                        (infoItem[i] as HTMLElement).style.opacity = "1";
                        (darkThemeHover[i] as HTMLElement).style.opacity = "0.9";
                        (darkThemeHover_text[i] as HTMLElement).style.opacity = "1";
                        // (darkThemeHover_text[i] as HTMLElement).innerText = `${text}`;
                        // setopacityInfoItem('1')
                        infoItem[i].classList.add('activeinfoItem');
                    });

                    infoItem[i].addEventListener("click", function () {
                        setcolorInfoItem(`${colors[1]}`);
                        // (infoItem[i] as HTMLElement).style.color = `red`;
                        (infoItem[i] as HTMLElement).style.opacity = "1";
                        (darkThemeHover[i] as HTMLElement).style.opacity = "0.9";
                        (darkThemeHover_text[i] as HTMLElement).style.opacity = "1";
                        // (darkThemeHover_text[i] as HTMLElement).innerText = `${text}`;
                        // setopacityInfoItem('1')
                        infoItem[i].classList.add('activeinfoItem');
                    });
                    infoItem[i].addEventListener("mouseout", function () {
                        setcolorInfoItem('#FFFFFF');
                        // (infoItem[i] as HTMLElement).style.color = `#FFFFFF`;
                        (infoItem[i] as HTMLElement).style.opacity = "0.3";
                        (darkThemeHover[i] as HTMLElement).style.opacity = "0";
                        (darkThemeHover_text[i] as HTMLElement).style.opacity = "0";

                        // setcolorInfoItem(`white`)
                        // setopacityInfoItem('0.3')
                        infoItem[i].classList.remove('activeinfoItem');
                    });
                }
            }
        }
    }, [])

    // if (el !== null) {
    // el.addEventListener('mouseover', function () {
    //     console.log(1)
    // })
    // el.addEventListener('mouseout', function () {
    //     console.log(2)
    // })
    // }

    const clickInfoBtn = () => {
        // console.log('click')
    }

    const stylefirstInfo = {
        'background': {
            backgroundColor: `#000000ab`,
            opacity: '0'
        }
    }

    const style1 = {
        'background': {
            background: `linear-gradient(to right, ${colors[0]}, ${colors[1]}, ${colors[0]})`
        },
        'backgroundBtn': {
            background: `${colors[1]}`
        },
        'borderOnfoItem': {
            border: `2px solid ${colors[1]}`,
            // color: `${colorInfoItem}`,
            // opacity: `${opacityInfoItem}`
        }
    }

    return (
        <div className="Loot_block_content">
            <div className="Loot_block">
                <div className='Loot_color' id='Loot_color'>
                    <div className='Loot_color_bottom' style={style1.background}></div>
                    <div className='content'>
                        <div className='Loot_color_TextMain'></div>
                        <img src={picture} alt="" className='background_image' />
                        {/* <div className='title_image'>Loot_block</div> */}
                        <div className='infoItem' onClick={clickInfoBtn} style={{ color: '#FFFFFF', border: `2px solid ${colors[1]}`, opacity: '0.3' }}>
                            i
                        </div>
                        <div className='Loot_color_darkTheme'>
                            <div className='darkThemeHover' style={{ opacity: '0' }}>
                                {/* <div className='darkThemeHover_text' style={{ opacity: '0' }}>{text[0]}<br />{text[1]}<br />{text[2]}<br />{text[3]}<br />{text[4]}<br />{text[5]}<br />{text[6]}<br />{text[7]}<br />{text[8]}</div> */}
                                <div className='darkThemeHover_text' style={{ opacity: '0' }}>{title}</div>
                            </div>
                            <Link to={isActive ? '/lootbeers_ts/LootboxComponent' : '#'} state={{
                                price: price,
                                picture: picture,
                                title: title,
                                colors: colors,
                                win_variants: winVariants,
                                lootboxAddress: lootboxAddress
                            }} className='Loot_color_darkThemeLink'><div className='Loot_color_darkTheme_button' style={{ color: `${colors[1]}`, backgroundColor: `${colors[2]}`, visibility: 'hidden', display: 'none' }}>OPEN</div></Link>
                        </div>
                    </div>
                    <div className='module-border-wrap-content'>
                        <div className='module-border-wrap' style={style1.backgroundBtn}>
                            <div className='price'>{price} SOL</div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default Loot_block;
