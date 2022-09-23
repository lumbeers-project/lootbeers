import { WalletAdapterNetwork } from "@solana/wallet-adapter-base";
import { ConnectionProvider, useAnchorWallet, useConnection, WalletProvider } from "@solana/wallet-adapter-react";
import { PhantomWalletAdapter } from "@solana/wallet-adapter-phantom";
import { SolletWalletAdapter, SolletExtensionWalletAdapter } from "@solana/wallet-adapter-sollet";
import { SlopeWalletAdapter } from "@solana/wallet-adapter-slope";
import { SolflareWalletAdapter } from "@solana/wallet-adapter-solflare";
import { ReactNode, useMemo } from "react";
import { clusterApiUrl } from "@solana/web3.js";
import { WalletModalProvider } from '@solana/wallet-adapter-react-ui';
import { AnchorProvider } from '@project-serum/anchor';
import { WalletDialogProvider } from '@solana/wallet-adapter-material-ui'
import '../App.css';

type Props = {
    children: ReactNode
}

export function useAnchorProvider() {

    const { connection } = useConnection();
    const wallet = useAnchorWallet();

    const provider = useMemo(() => wallet == null ? null : new AnchorProvider(connection, wallet, {
        commitment: 'confirmed'
    }), [connection, wallet]);

    return provider;
}

export default function WalletProviderComponent({ children }: Props) {
    const network = WalletAdapterNetwork.Devnet;
    const rpc = clusterApiUrl(network);

    const wallets = useMemo(
        () => [
            new PhantomWalletAdapter(),
            new SlopeWalletAdapter(),
            new SolflareWalletAdapter(),
            new SolletWalletAdapter({ network }),
            new SolletExtensionWalletAdapter({ network }),
        ],
        [network]
    );

    return (
        <ConnectionProvider endpoint={rpc} config={{ commitment: "finalized" }}>
            <WalletProvider wallets={wallets} autoConnect={true}>
                <WalletDialogProvider>
                    <WalletModalProvider>
                        <div className="backgroundStyle"></div>
                        {children}
                    </WalletModalProvider>
                </WalletDialogProvider>
            </WalletProvider>
        </ConnectionProvider>
    )
}
