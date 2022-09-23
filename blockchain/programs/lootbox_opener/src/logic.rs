use anchor_lang::prelude::*;
use anchor_spl::token::{self, Token};

use crate::{
    error::LootbeerError,
    state::{PayoutReward, RootConfig, Ticket, TreasureChest},
};

pub struct TokenChecks;

impl TokenChecks {
    fn assert_owner(account: &UncheckedAccount, program_id: Pubkey) -> Result<()> {
        // msg!("{} {} {}", *account.key, *account.owner, program_id.key());
        if *account.to_account_info().owner == program_id {
            Ok(())
        } else {
            return err!(AccountOwnedByWrongProgram);
        }
    }

    fn assert_derived(token_account: &AccountInfo, mint: Pubkey, user: &AccountInfo) -> Result<()> {
        let expected_address =
            anchor_spl::associated_token::get_associated_token_address(user.key, &mint);

        if expected_address == token_account.key() {
            Ok(())
        } else {
            return err!(ConstraintSeeds);
        }
    }

    pub fn check_reward_dest_accounts(
        user: &UncheckedAccount,
        token_account: &UncheckedAccount,
        treasure_chest: &AccountInfo,
        source_token_account: &UncheckedAccount,
        reward: PayoutReward,
    ) -> Result<()> {
        match reward {
            PayoutReward::TokenPayout { mint, amount: _ } => {
                Self::assert_owner(token_account, Token::id())?;
                Self::assert_owner(source_token_account, Token::id())?;
                Self::assert_derived(token_account, mint, user)?;
                Self::assert_derived(source_token_account, mint, treasure_chest)?;
                Ok(())
            }
            PayoutReward::SolPayout { amount: _ } => Ok(()),
            PayoutReward::NftPayout { mint } => {
                Self::assert_owner(token_account, Token::id())?;
                Self::assert_owner(source_token_account, Token::id())?;
                Self::assert_derived(token_account, mint, user)?;
                Self::assert_derived(source_token_account, mint, treasure_chest)?;
                Ok(())
            }

            PayoutReward::NonePayout => Ok(()),
        }
    }
}

impl TreasureChest {
    pub fn send_reward<'a>(
        &self,
        self_info: AccountInfo<'a>,
        user: UncheckedAccount<'a>,
        root_config: Account<RootConfig>,
        token_account: UncheckedAccount<'a>,
        source_token_account: UncheckedAccount<'a>,
        reward: PayoutReward,
        token_program: Program<'a, Token>,
    ) -> Result<()> {
        let root_config_key = &root_config.key();

        let treasure_chest_seeds = [
            b"treasure_chest".as_ref(),
            root_config_key.as_ref(),
            &[self.bump],
        ];

        let seeds_slice = treasure_chest_seeds.as_slice();
        let seeds = &[seeds_slice];

        match reward {
            PayoutReward::SolPayout { amount } => {
                **user.lamports.borrow_mut() = user.lamports().checked_add(amount).unwrap();

                **self_info.lamports.borrow_mut() =
                    self_info.lamports().checked_sub(amount).unwrap();

                msg!("You won SOL reward");
                Ok(())
            }
            PayoutReward::TokenPayout { mint: _, amount } => {
                let cpi = CpiContext::new(
                    token_program.clone().to_account_info(),
                    token::Transfer {
                        authority: self_info.clone().to_account_info(),
                        from: source_token_account.clone().to_account_info(),
                        to: token_account.clone().to_account_info(),
                    },
                )
                .with_signer(seeds);

                token::transfer(cpi, amount)?;

                msg!("You won token reward");
                Ok(())
            }
            PayoutReward::NftPayout { mint: _ } => {
                msg!("Sending NFTs is not yet implemented");
                return err!(LootbeerError::NftPayoutsNotSupported);
            }
            PayoutReward::NonePayout => {
                msg!("You lost, sorry");
                Ok(())
            }
        }
    }
}

impl Ticket {
    pub fn remove<'a>(ticket: Account<'a, Ticket>, destination: AccountInfo<'a>) -> Result<()> {
        let ticket_info = &mut ticket.to_account_info();
        let balance = ticket_info.lamports();

        **destination.lamports.borrow_mut() = destination.lamports().checked_add(balance).unwrap();
        **ticket_info.lamports.borrow_mut() = ticket_info.lamports().checked_sub(balance).unwrap();

        Ok(())
    }
}
