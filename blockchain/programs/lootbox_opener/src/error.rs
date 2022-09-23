use anchor_lang::error_code;

#[error_code]
pub enum LootbeerError {
    #[msg("Wrong update authority provided")]
    WrongUpdateAuthority,

    #[msg("Root config provided does not match the required one")]
    WrongRootConfigAddress,

    #[msg("You need to disable the lootbox in order to perform this action")]
    LootboxNotDisabled,

    #[msg("This price is not yet supported, you can only use native SOL price")]
    PriceNotSupported,

    #[msg("You need to enable the lootbox in order to perform this action")]
    LootboxNotEnabled,

    #[msg("Wrong funds destination provided")]
    WrongFundsDestination,

    #[msg("Wrong reward authority provided")]
    WrongRewardAuthority,

    #[msg("Sorry, NFT payouts are not supported at the moment")]
    NftPayoutsNotSupported,
}
