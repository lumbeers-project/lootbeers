use anchor_lang::prelude::*;

use super::{Price, Reward};

#[account]
pub struct RootConfig {
    pub funds_destination: Pubkey,
    pub update_authority: Pubkey,
    pub reward_authority: Pubkey,

    pub lootbox_index: u8,
}

#[account]
pub struct TreasureChest {
    pub bump: u8,
}

#[account]
pub struct LootBox {
    pub reward_index: u8,
    pub bump: u8,
    pub enabled: bool,
    pub root_config_address: Pubkey,
    pub price: Price,
}

#[account]
/// SIZE: 8 + ((1 + 32 + 8) + 2) + 1 + 32
pub struct RewardAccount {
    pub lootbox_address: Pubkey,
    pub reward: Reward,
    pub bump: u8,
}

#[account]
pub struct Ticket {
    pub user: Pubkey,
    pub lootbox_address: Pubkey,
}
