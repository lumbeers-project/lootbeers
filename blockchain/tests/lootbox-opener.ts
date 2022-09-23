import * as anchor from "@project-serum/anchor";
import { Program } from "@project-serum/anchor";
import { rpc } from "@project-serum/anchor/dist/cjs/utils";
import { BN } from "bn.js";
import { LootboxOpener } from '../target/types/lootbox_opener';

describe("lootbox_opener", () => {

    anchor.setProvider(anchor.AnchorProvider.env());
    const program = anchor.workspace.LootboxOpener as Program<LootboxOpener>;

    const rootConfig = new anchor.web3.Keypair();
    const fundsDestination = new anchor.web3.Keypair();
    const updateAuthority = new anchor.web3.Keypair();
    const rewardAuthority = new anchor.web3.Keypair();

    const [lootbox] = anchor.web3.PublicKey.findProgramAddressSync([
        anchor.utils.bytes.utf8.encode("lootbox"),
        rootConfig.publicKey.toBuffer(),
        new BN(0).toBuffer('le', 1)
    ],
        program.programId)

    console.log("Root config private key ", JSON.stringify(rootConfig.secretKey))

    const [treasureChest] = anchor.web3.PublicKey.findProgramAddressSync([
        anchor.utils.bytes.utf8.encode("treasure_chest"),
        rootConfig.publicKey.toBuffer()
    ],
        program.programId);

    const [reward] = anchor.web3.PublicKey.findProgramAddressSync([
        anchor.utils.bytes.utf8.encode("reward"),
        lootbox.toBuffer(),
        new BN(0).toBuffer('le', 1)
    ],
        program.programId);

    it('Initialize root', async () => {

        const id = await program.methods
            .initialize({
                fundsDestination: fundsDestination.publicKey,
                updateAuthority: updateAuthority.publicKey,
                rewardAuthority: rewardAuthority.publicKey
            })
            .accountsStrict({
                user: program.provider.publicKey,
                rootConfig: rootConfig.publicKey,
                treasureChest,
                systemProgram: anchor.web3.SystemProgram.programId,
            })
            .signers([rootConfig])
            .rpc();

        console.log("Your transaction id ", id);

    });

    it("Create lootbox", async () => {

        await new Promise(resolve => setTimeout(resolve, 1000))

        try {
            let id = await program.methods
                .createLootbox({
                    amount: new BN(0.05 * anchor.web3.LAMPORTS_PER_SOL),
                    mint: null
                })
                .accounts({
                    lootbox: lootbox,
                    payer: program.provider.publicKey,
                    updateAuthority: updateAuthority.publicKey,
                    rootConfig: rootConfig.publicKey,
                    systemProgram: anchor.web3.SystemProgram.programId
                })
                .signers([updateAuthority])
                .rpc();

            console.log("Create lootbox tx id ", id);
        } catch (e) {
            console.error(e)
            throw e;
        }
    });

    it("Add rewards", async () => {

        await new Promise(resolve => setTimeout(resolve, 1000))

        try {
            let id = await program.methods
                // @ts-ignore-next-line
                .addReward({
                    rewardType: {
                        solReward: {
                            amount: new BN(10000000, 'le')
                        },
                    },
                    weight: 100
                })
                .accounts({
                    payer: program.provider.publicKey,
                    lootbox: lootbox,
                    rootConfig: rootConfig.publicKey,
                    reward: reward,
                    systemProgram: anchor.web3.SystemProgram.programId,
                    updateAuthority: updateAuthority.publicKey
                })
                .signers([updateAuthority])
                .rpc();

            console.log("Add reward tx id ", id);
        } catch (e) {
            console.error(e)
            throw e;
        }
    });

    it("Enable lootbox", async () => {

        await new Promise(resolve => setTimeout(resolve, 1000))

        try {

            const id = await program.methods
                .setLootboxEnabled(true)
                .accounts({
                    lootbox: lootbox,
                    rootConfig: rootConfig.publicKey,
                    updateAuthority: updateAuthority.publicKey
                })
                .signers([updateAuthority])
                .rpc();

            console.log("Enable lootbox ", id);
        } catch (e) {
            console.error(e)
            throw e;
        }

    });
});
