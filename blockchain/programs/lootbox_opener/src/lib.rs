use anchor_lang::prelude::*;
use anchor_spl::token::Token;
use error::LootbeerError;
use state::{
    LootBox,
    RewardAccount,
    RootConfig,
    RootConfigData,
    Price,
    TreasureChest,
    Reward,
    Ticket,
    PayoutReward
};

pub mod error;
pub mod state;
pub mod logic;

declare_id!("2qPL6aunS5v636TymUKmQAG2iW6gn7q94vShQb51iyMW");

#[program]
pub mod lootbox_opener {

    use anchor_lang::system_program::Transfer;

    use crate::logic::TokenChecks;

    use super::*;

    pub fn initialize(ctx: Context<Initialize>, root_config_data: RootConfigData) -> Result<()> {
        let root_config = &mut ctx.accounts.root_config;
        root_config.lootbox_index = 0;
        root_config.update_authority = root_config_data.update_authority;
        root_config.funds_destination = root_config_data.funds_destination;
        root_config.reward_authority = root_config_data.reward_authority;

        let treasure_chest = &mut ctx.accounts.treasure_chest;
        treasure_chest.bump = *ctx.bumps.get("treasure_chest").unwrap();

        Ok(())
    }

    pub fn create_lootbox(ctx: Context<CreateLootbox>, price: Price) -> Result<()> {
        let root_config = &mut ctx.accounts.root_config;
        let lootbox = &mut ctx.accounts.lootbox;

        lootbox.reward_index = 0;
        lootbox.bump = *ctx.bumps.get("lootbox").unwrap();
        lootbox.price = price;
        lootbox.enabled = false;
        lootbox.root_config_address = root_config.key();

        root_config.lootbox_index += 1;

        Ok(())
    }

    pub fn set_lootbox_enabled(ctx: Context<SetLootboxEnabled>, is_enabled: bool) -> Result<()> {
        let lootbox = &mut ctx.accounts.lootbox;
        lootbox.enabled = is_enabled;

        Ok(())
    }

    pub fn add_reward(ctx: Context<AddReward>, reward: Reward) -> Result<()> {

        let lootbox = &mut ctx.accounts.lootbox;
        let reward_acc = &mut ctx.accounts.reward;

        reward_acc.reward = reward;
        reward_acc.bump = *ctx.bumps.get("reward").unwrap();
        reward_acc.lootbox_address = lootbox.key();

        lootbox.reward_index += 1;

        Ok(())
    }

    pub fn buy_lootbox_ticket(ctx: Context<BuyLootboxTicket>) -> Result<()> {

        let lootbox = &ctx.accounts.lootbox;
        let funds_destination = &mut ctx.accounts.funds_destination;
        let ticket = &mut ctx.accounts.ticket;
        let user = &mut ctx.accounts.user;
        let system = &ctx.accounts.system_program;

        if lootbox.price.mint.is_some() {
            return err!(LootbeerError::PriceNotSupported);
        }

        let lamports = lootbox.price.amount;

        ticket.lootbox_address = lootbox.key();
        ticket.user = user.key();

        let cpi = CpiContext::new(system.to_account_info().clone(), Transfer {
            from: user.to_account_info().clone(),
            to: funds_destination.to_account_info().clone()
        });

        anchor_lang::system_program::transfer(cpi, lamports)?;

        Ok(())
    }

    pub fn redeem_ticket(ctx: Context<RedeemTicket>, reward: PayoutReward) -> Result<()> {

        let user = &mut ctx.accounts.user;
        let token_account = &mut ctx.accounts.user_token;
        let source_token_account = &mut ctx.accounts.source_token;
        let treasure_chest = &mut ctx.accounts.treasure_chest;
        let ticket = &mut ctx.accounts.ticket;

        TokenChecks::check_reward_dest_accounts(user, token_account, &treasure_chest.to_account_info(), source_token_account, reward)?;

        let root_config = &mut ctx.accounts.root_config;
        let token_program = &mut ctx.accounts.token_program;

        let chest = TreasureChest {
            bump: treasure_chest.bump
        };

        chest.send_reward(
            treasure_chest.clone().to_account_info(),
            user.clone(),
            root_config.clone(),
            token_account.clone(),
            source_token_account.clone(),
            reward,
            token_program.clone())?;

        Ticket::remove(ticket.clone(), user.clone().to_account_info())?;
        Ok(())
    }
}

#[derive(Accounts)]
#[instruction(root_config: RootConfigData)]
pub struct Initialize<'info> {
    #[account(mut)]
    pub user: Signer<'info>,

    #[account(
        init,
        payer = user,
        space = 8 + 32 + 32 + 32 + 1
    )]
    pub root_config: Account<'info, RootConfig>,

    #[account(
        init,
        payer = user,
        space = 8 + 1,
        seeds = [b"treasure_chest".as_ref(), root_config.key().as_ref()], bump
    )]
    pub treasure_chest: Account<'info, TreasureChest>,

    pub system_program: Program<'info, System>,
}

#[derive(Accounts)]
#[instruction(price: Price)]
pub struct CreateLootbox<'info> {
    #[account(mut)]
    pub root_config: Account<'info, RootConfig>,

    #[account(
        address = root_config.update_authority @ LootbeerError::WrongUpdateAuthority
    )]
    pub update_authority: Signer<'info>,

    #[account(mut)]
    pub payer: Signer<'info>,

    #[account(
        init,
        payer = payer,
        space = 8 + 1 + 1 + 1 + 32 + (1 + 32 + 8),
        seeds = [b"lootbox".as_ref(), root_config.key().as_ref(), &[root_config.lootbox_index]], bump,
        constraint = price.mint.is_none() @ LootbeerError::PriceNotSupported
    )]
    pub lootbox: Account<'info, LootBox>,

    pub system_program: Program<'info, System>,
}

#[derive(Accounts)]
pub struct SetLootboxEnabled<'info> {
    #[account(mut)]
    pub lootbox: Account<'info, LootBox>,

    #[account(
        address = lootbox.root_config_address @ LootbeerError::WrongRootConfigAddress
    )]
    pub root_config: Account<'info, RootConfig>,

    #[account(
        address = root_config.update_authority @ LootbeerError::WrongUpdateAuthority
    )]
    pub update_authority: Signer<'info>,
}

#[derive(Accounts)]
#[instruction(reward: Reward)]
pub struct AddReward<'info> {
    #[account(
        address = lootbox.root_config_address @ LootbeerError::WrongRootConfigAddress
    )]
    pub root_config: Account<'info, RootConfig>,

    #[account(
        mut,
        constraint = !lootbox.enabled @ LootbeerError::LootboxNotDisabled    
    )]
    pub lootbox: Account<'info, LootBox>,

    #[account(
        address = root_config.update_authority @ LootbeerError::WrongUpdateAuthority
    )]
    pub update_authority: Signer<'info>,

    #[account(mut)]
    pub payer: Signer<'info>,

    #[account(
        init,
        payer = payer,
        space = 8 + ((1 + 32 + 8) + 2) + 1 + 32,
        seeds = [b"reward", lootbox.key().as_ref(), &[lootbox.reward_index]], bump
    )]
    pub reward: Account<'info, RewardAccount>,

    pub system_program: Program<'info, System>,
}

#[derive(Accounts)]
pub struct BuyLootboxTicket<'info> {

    #[account(mut)]
    pub user: Signer<'info>,

    #[account(
        address = lootbox.root_config_address @ LootbeerError::WrongRootConfigAddress
    )]
    pub root_config: Account<'info, RootConfig>,

    #[account(
        constraint = lootbox.enabled @ LootbeerError::LootboxNotEnabled
    )]
    pub lootbox: Account<'info, LootBox>,

    #[account(
        mut,
        address = root_config.funds_destination @ LootbeerError::WrongFundsDestination
    )]
    /// CHECK: Only used to send sol there
    pub funds_destination: UncheckedAccount<'info>,

    #[account(
        init,
        payer = user,
        space = 8 + 32 + 32
    )]
    pub ticket: Account<'info, Ticket>,

    pub system_program: Program<'info, System>
}

#[derive(Accounts)]
#[instruction(payout_reward: PayoutReward)]
pub struct RedeemTicket<'info> {
    #[account(
        address = root_config.reward_authority @ LootbeerError::WrongRewardAuthority
    )]
    pub reward_authority: Signer<'info>,

    #[account(
        address = lootbox.root_config_address @ LootbeerError::WrongRootConfigAddress
    )]
    pub root_config: Account<'info, RootConfig>,

    #[account()]
    pub lootbox: Account<'info, LootBox>,

    #[account(
        mut,
        seeds = [b"treasure_chest".as_ref(), root_config.key().as_ref()], bump = treasure_chest.bump
    )]
    pub treasure_chest: Account<'info, TreasureChest>,

    #[account(
        mut,
        constraint = ticket.lootbox_address == lootbox.key(),
        constraint = ticket.user == user.key()
    )]
    pub ticket: Account<'info, Ticket>,

    #[account(mut)]
    /// CHECK: Only used as SOL destination
    pub user: UncheckedAccount<'info>,

    #[account(mut)]
    /// CHECK: Checked manually in a program. You can provide NULL address if you are not giving SPL tokens
    pub user_token: UncheckedAccount<'info>,

    #[account(mut)]
    /// CHECK: Checked manually in a program. You can provide NULL address if you are not giving SPL tokens
    pub source_token: UncheckedAccount<'info>,

    pub system_program: Program<'info, System>,

    pub token_program: Program<'info, Token>
}