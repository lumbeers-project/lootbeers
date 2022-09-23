use anchor_lang::prelude::*;

#[derive(AnchorSerialize, AnchorDeserialize, Clone, Copy)]
pub struct RootConfigData {
    pub funds_destination: Pubkey,
    pub update_authority: Pubkey,
    pub reward_authority: Pubkey,
}

#[derive(AnchorSerialize, AnchorDeserialize, Clone, Copy)]
/// SIZE: 1 + 32 + 8
pub enum RewardType {
    TokenReward { mint: Pubkey, amount: u64 },
    SolReward { amount: u64 },
    NftReward,
    NoneReward,
}

#[derive(AnchorSerialize, AnchorDeserialize, Clone, Copy)]
/// SIZE: (1 + 32 + 8) + 2
pub struct Reward {
    pub reward_type: RewardType,
    pub weight: u16,
}

#[derive(AnchorSerialize, AnchorDeserialize, Clone, Copy)]
/// SIZE = 1 + 32 + 8
pub struct Price {
    pub mint: Option<Pubkey>,
    pub amount: u64,
}

#[derive(AnchorSerialize, AnchorDeserialize, Clone, Copy)]
pub enum PayoutReward {
    TokenPayout { mint: Pubkey, amount: u64 },
    SolPayout { amount: u64 },
    NftPayout { mint: Pubkey },
    NonePayout,
}
