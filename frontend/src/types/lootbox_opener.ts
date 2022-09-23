export type LootboxOpener = {
  "version": "0.1.0",
  "name": "lootbox_opener",
  "instructions": [
    {
      "name": "initialize",
      "accounts": [
        {
          "name": "user",
          "isMut": true,
          "isSigner": true
        },
        {
          "name": "rootConfig",
          "isMut": true,
          "isSigner": true
        },
        {
          "name": "treasureChest",
          "isMut": true,
          "isSigner": false
        },
        {
          "name": "systemProgram",
          "isMut": false,
          "isSigner": false
        }
      ],
      "args": [
        {
          "name": "rootConfigData",
          "type": {
            "defined": "RootConfigData"
          }
        }
      ]
    },
    {
      "name": "createLootbox",
      "accounts": [
        {
          "name": "rootConfig",
          "isMut": true,
          "isSigner": false
        },
        {
          "name": "updateAuthority",
          "isMut": false,
          "isSigner": true
        },
        {
          "name": "payer",
          "isMut": true,
          "isSigner": true
        },
        {
          "name": "lootbox",
          "isMut": true,
          "isSigner": false
        },
        {
          "name": "systemProgram",
          "isMut": false,
          "isSigner": false
        }
      ],
      "args": [
        {
          "name": "price",
          "type": {
            "defined": "Price"
          }
        }
      ]
    },
    {
      "name": "setLootboxEnabled",
      "accounts": [
        {
          "name": "lootbox",
          "isMut": true,
          "isSigner": false
        },
        {
          "name": "rootConfig",
          "isMut": false,
          "isSigner": false
        },
        {
          "name": "updateAuthority",
          "isMut": false,
          "isSigner": true
        }
      ],
      "args": [
        {
          "name": "isEnabled",
          "type": "bool"
        }
      ]
    },
    {
      "name": "addReward",
      "accounts": [
        {
          "name": "rootConfig",
          "isMut": false,
          "isSigner": false
        },
        {
          "name": "lootbox",
          "isMut": true,
          "isSigner": false
        },
        {
          "name": "updateAuthority",
          "isMut": false,
          "isSigner": true
        },
        {
          "name": "payer",
          "isMut": true,
          "isSigner": true
        },
        {
          "name": "reward",
          "isMut": true,
          "isSigner": false
        },
        {
          "name": "systemProgram",
          "isMut": false,
          "isSigner": false
        }
      ],
      "args": [
        {
          "name": "reward",
          "type": {
            "defined": "Reward"
          }
        }
      ]
    },
    {
      "name": "buyLootboxTicket",
      "accounts": [
        {
          "name": "user",
          "isMut": true,
          "isSigner": true
        },
        {
          "name": "rootConfig",
          "isMut": false,
          "isSigner": false
        },
        {
          "name": "lootbox",
          "isMut": false,
          "isSigner": false
        },
        {
          "name": "fundsDestination",
          "isMut": true,
          "isSigner": false
        },
        {
          "name": "ticket",
          "isMut": true,
          "isSigner": true
        },
        {
          "name": "systemProgram",
          "isMut": false,
          "isSigner": false
        }
      ],
      "args": []
    },
    {
      "name": "redeemTicket",
      "accounts": [
        {
          "name": "rewardAuthority",
          "isMut": false,
          "isSigner": true
        },
        {
          "name": "rootConfig",
          "isMut": false,
          "isSigner": false
        },
        {
          "name": "lootbox",
          "isMut": false,
          "isSigner": false
        },
        {
          "name": "treasureChest",
          "isMut": true,
          "isSigner": false
        },
        {
          "name": "ticket",
          "isMut": true,
          "isSigner": false
        },
        {
          "name": "user",
          "isMut": true,
          "isSigner": false
        },
        {
          "name": "userToken",
          "isMut": true,
          "isSigner": false
        },
        {
          "name": "sourceToken",
          "isMut": true,
          "isSigner": false
        },
        {
          "name": "systemProgram",
          "isMut": false,
          "isSigner": false
        },
        {
          "name": "tokenProgram",
          "isMut": false,
          "isSigner": false
        }
      ],
      "args": [
        {
          "name": "reward",
          "type": {
            "defined": "PayoutReward"
          }
        }
      ]
    }
  ],
  "accounts": [
    {
      "name": "rootConfig",
      "type": {
        "kind": "struct",
        "fields": [
          {
            "name": "fundsDestination",
            "type": "publicKey"
          },
          {
            "name": "updateAuthority",
            "type": "publicKey"
          },
          {
            "name": "rewardAuthority",
            "type": "publicKey"
          },
          {
            "name": "lootboxIndex",
            "type": "u8"
          }
        ]
      }
    },
    {
      "name": "treasureChest",
      "type": {
        "kind": "struct",
        "fields": [
          {
            "name": "bump",
            "type": "u8"
          }
        ]
      }
    },
    {
      "name": "lootBox",
      "type": {
        "kind": "struct",
        "fields": [
          {
            "name": "rewardIndex",
            "type": "u8"
          },
          {
            "name": "bump",
            "type": "u8"
          },
          {
            "name": "enabled",
            "type": "bool"
          },
          {
            "name": "rootConfigAddress",
            "type": "publicKey"
          },
          {
            "name": "price",
            "type": {
              "defined": "Price"
            }
          }
        ]
      }
    },
    {
      "name": "rewardAccount",
      "docs": [
        "SIZE: 8 + ((1 + 32 + 8) + 2) + 1 + 32"
      ],
      "type": {
        "kind": "struct",
        "fields": [
          {
            "name": "lootboxAddress",
            "type": "publicKey"
          },
          {
            "name": "reward",
            "type": {
              "defined": "Reward"
            }
          },
          {
            "name": "bump",
            "type": "u8"
          }
        ]
      }
    },
    {
      "name": "ticket",
      "type": {
        "kind": "struct",
        "fields": [
          {
            "name": "user",
            "type": "publicKey"
          },
          {
            "name": "lootboxAddress",
            "type": "publicKey"
          }
        ]
      }
    }
  ],
  "types": [
    {
      "name": "RootConfigData",
      "type": {
        "kind": "struct",
        "fields": [
          {
            "name": "fundsDestination",
            "type": "publicKey"
          },
          {
            "name": "updateAuthority",
            "type": "publicKey"
          },
          {
            "name": "rewardAuthority",
            "type": "publicKey"
          }
        ]
      }
    },
    {
      "name": "Reward",
      "docs": [
        "SIZE: (1 + 32 + 8) + 2"
      ],
      "type": {
        "kind": "struct",
        "fields": [
          {
            "name": "rewardType",
            "type": {
              "defined": "RewardType"
            }
          },
          {
            "name": "weight",
            "type": "u16"
          }
        ]
      }
    },
    {
      "name": "Price",
      "docs": [
        "SIZE = 1 + 32 + 8"
      ],
      "type": {
        "kind": "struct",
        "fields": [
          {
            "name": "mint",
            "type": {
              "option": "publicKey"
            }
          },
          {
            "name": "amount",
            "type": "u64"
          }
        ]
      }
    },
    {
      "name": "RewardType",
      "docs": [
        "SIZE: 1 + 32 + 8"
      ],
      "type": {
        "kind": "enum",
        "variants": [
          {
            "name": "TokenReward",
            "fields": [
              {
                "name": "mint",
                "type": "publicKey"
              },
              {
                "name": "amount",
                "type": "u64"
              }
            ]
          },
          {
            "name": "SolReward",
            "fields": [
              {
                "name": "amount",
                "type": "u64"
              }
            ]
          },
          {
            "name": "NftReward"
          },
          {
            "name": "NoneReward"
          }
        ]
      }
    },
    {
      "name": "PayoutReward",
      "type": {
        "kind": "enum",
        "variants": [
          {
            "name": "TokenPayout",
            "fields": [
              {
                "name": "mint",
                "type": "publicKey"
              },
              {
                "name": "amount",
                "type": "u64"
              }
            ]
          },
          {
            "name": "SolPayout",
            "fields": [
              {
                "name": "amount",
                "type": "u64"
              }
            ]
          },
          {
            "name": "NftPayout",
            "fields": [
              {
                "name": "mint",
                "type": "publicKey"
              }
            ]
          },
          {
            "name": "NonePayout"
          }
        ]
      }
    }
  ],
  "errors": [
    {
      "code": 6000,
      "name": "WrongUpdateAuthority",
      "msg": "Wrong update authority provided"
    },
    {
      "code": 6001,
      "name": "WrongRootConfigAddress",
      "msg": "Root config provided does not match the required one"
    },
    {
      "code": 6002,
      "name": "LootboxNotDisabled",
      "msg": "You need to disable the lootbox in order to perform this action"
    },
    {
      "code": 6003,
      "name": "PriceNotSupported",
      "msg": "This price is not yet supported, you can only use native SOL price"
    },
    {
      "code": 6004,
      "name": "LootboxNotEnabled",
      "msg": "You need to enable the lootbox in order to perform this action"
    },
    {
      "code": 6005,
      "name": "WrongFundsDestination",
      "msg": "Wrong funds destination provided"
    },
    {
      "code": 6006,
      "name": "WrongRewardAuthority",
      "msg": "Wrong reward authority provided"
    },
    {
      "code": 6007,
      "name": "NftPayoutsNotSupported",
      "msg": "Sorry, NFT payouts are not supported at the moment"
    }
  ],
  "metadata": {
    "address": "2qPL6aunS5v636TymUKmQAG2iW6gn7q94vShQb51iyMW"
  }
};

export const IDL: LootboxOpener = {
  "version": "0.1.0",
  "name": "lootbox_opener",
  "instructions": [
    {
      "name": "initialize",
      "accounts": [
        {
          "name": "user",
          "isMut": true,
          "isSigner": true
        },
        {
          "name": "rootConfig",
          "isMut": true,
          "isSigner": true
        },
        {
          "name": "treasureChest",
          "isMut": true,
          "isSigner": false
        },
        {
          "name": "systemProgram",
          "isMut": false,
          "isSigner": false
        }
      ],
      "args": [
        {
          "name": "rootConfigData",
          "type": {
            "defined": "RootConfigData"
          }
        }
      ]
    },
    {
      "name": "createLootbox",
      "accounts": [
        {
          "name": "rootConfig",
          "isMut": true,
          "isSigner": false
        },
        {
          "name": "updateAuthority",
          "isMut": false,
          "isSigner": true
        },
        {
          "name": "payer",
          "isMut": true,
          "isSigner": true
        },
        {
          "name": "lootbox",
          "isMut": true,
          "isSigner": false
        },
        {
          "name": "systemProgram",
          "isMut": false,
          "isSigner": false
        }
      ],
      "args": [
        {
          "name": "price",
          "type": {
            "defined": "Price"
          }
        }
      ]
    },
    {
      "name": "setLootboxEnabled",
      "accounts": [
        {
          "name": "lootbox",
          "isMut": true,
          "isSigner": false
        },
        {
          "name": "rootConfig",
          "isMut": false,
          "isSigner": false
        },
        {
          "name": "updateAuthority",
          "isMut": false,
          "isSigner": true
        }
      ],
      "args": [
        {
          "name": "isEnabled",
          "type": "bool"
        }
      ]
    },
    {
      "name": "addReward",
      "accounts": [
        {
          "name": "rootConfig",
          "isMut": false,
          "isSigner": false
        },
        {
          "name": "lootbox",
          "isMut": true,
          "isSigner": false
        },
        {
          "name": "updateAuthority",
          "isMut": false,
          "isSigner": true
        },
        {
          "name": "payer",
          "isMut": true,
          "isSigner": true
        },
        {
          "name": "reward",
          "isMut": true,
          "isSigner": false
        },
        {
          "name": "systemProgram",
          "isMut": false,
          "isSigner": false
        }
      ],
      "args": [
        {
          "name": "reward",
          "type": {
            "defined": "Reward"
          }
        }
      ]
    },
    {
      "name": "buyLootboxTicket",
      "accounts": [
        {
          "name": "user",
          "isMut": true,
          "isSigner": true
        },
        {
          "name": "rootConfig",
          "isMut": false,
          "isSigner": false
        },
        {
          "name": "lootbox",
          "isMut": false,
          "isSigner": false
        },
        {
          "name": "fundsDestination",
          "isMut": true,
          "isSigner": false
        },
        {
          "name": "ticket",
          "isMut": true,
          "isSigner": true
        },
        {
          "name": "systemProgram",
          "isMut": false,
          "isSigner": false
        }
      ],
      "args": []
    },
    {
      "name": "redeemTicket",
      "accounts": [
        {
          "name": "rewardAuthority",
          "isMut": false,
          "isSigner": true
        },
        {
          "name": "rootConfig",
          "isMut": false,
          "isSigner": false
        },
        {
          "name": "lootbox",
          "isMut": false,
          "isSigner": false
        },
        {
          "name": "treasureChest",
          "isMut": true,
          "isSigner": false
        },
        {
          "name": "ticket",
          "isMut": true,
          "isSigner": false
        },
        {
          "name": "user",
          "isMut": true,
          "isSigner": false
        },
        {
          "name": "userToken",
          "isMut": true,
          "isSigner": false
        },
        {
          "name": "sourceToken",
          "isMut": true,
          "isSigner": false
        },
        {
          "name": "systemProgram",
          "isMut": false,
          "isSigner": false
        },
        {
          "name": "tokenProgram",
          "isMut": false,
          "isSigner": false
        }
      ],
      "args": [
        {
          "name": "reward",
          "type": {
            "defined": "PayoutReward"
          }
        }
      ]
    }
  ],
  "accounts": [
    {
      "name": "rootConfig",
      "type": {
        "kind": "struct",
        "fields": [
          {
            "name": "fundsDestination",
            "type": "publicKey"
          },
          {
            "name": "updateAuthority",
            "type": "publicKey"
          },
          {
            "name": "rewardAuthority",
            "type": "publicKey"
          },
          {
            "name": "lootboxIndex",
            "type": "u8"
          }
        ]
      }
    },
    {
      "name": "treasureChest",
      "type": {
        "kind": "struct",
        "fields": [
          {
            "name": "bump",
            "type": "u8"
          }
        ]
      }
    },
    {
      "name": "lootBox",
      "type": {
        "kind": "struct",
        "fields": [
          {
            "name": "rewardIndex",
            "type": "u8"
          },
          {
            "name": "bump",
            "type": "u8"
          },
          {
            "name": "enabled",
            "type": "bool"
          },
          {
            "name": "rootConfigAddress",
            "type": "publicKey"
          },
          {
            "name": "price",
            "type": {
              "defined": "Price"
            }
          }
        ]
      }
    },
    {
      "name": "rewardAccount",
      "docs": [
        "SIZE: 8 + ((1 + 32 + 8) + 2) + 1 + 32"
      ],
      "type": {
        "kind": "struct",
        "fields": [
          {
            "name": "lootboxAddress",
            "type": "publicKey"
          },
          {
            "name": "reward",
            "type": {
              "defined": "Reward"
            }
          },
          {
            "name": "bump",
            "type": "u8"
          }
        ]
      }
    },
    {
      "name": "ticket",
      "type": {
        "kind": "struct",
        "fields": [
          {
            "name": "user",
            "type": "publicKey"
          },
          {
            "name": "lootboxAddress",
            "type": "publicKey"
          }
        ]
      }
    }
  ],
  "types": [
    {
      "name": "RootConfigData",
      "type": {
        "kind": "struct",
        "fields": [
          {
            "name": "fundsDestination",
            "type": "publicKey"
          },
          {
            "name": "updateAuthority",
            "type": "publicKey"
          },
          {
            "name": "rewardAuthority",
            "type": "publicKey"
          }
        ]
      }
    },
    {
      "name": "Reward",
      "docs": [
        "SIZE: (1 + 32 + 8) + 2"
      ],
      "type": {
        "kind": "struct",
        "fields": [
          {
            "name": "rewardType",
            "type": {
              "defined": "RewardType"
            }
          },
          {
            "name": "weight",
            "type": "u16"
          }
        ]
      }
    },
    {
      "name": "Price",
      "docs": [
        "SIZE = 1 + 32 + 8"
      ],
      "type": {
        "kind": "struct",
        "fields": [
          {
            "name": "mint",
            "type": {
              "option": "publicKey"
            }
          },
          {
            "name": "amount",
            "type": "u64"
          }
        ]
      }
    },
    {
      "name": "RewardType",
      "docs": [
        "SIZE: 1 + 32 + 8"
      ],
      "type": {
        "kind": "enum",
        "variants": [
          {
            "name": "TokenReward",
            "fields": [
              {
                "name": "mint",
                "type": "publicKey"
              },
              {
                "name": "amount",
                "type": "u64"
              }
            ]
          },
          {
            "name": "SolReward",
            "fields": [
              {
                "name": "amount",
                "type": "u64"
              }
            ]
          },
          {
            "name": "NftReward"
          },
          {
            "name": "NoneReward"
          }
        ]
      }
    },
    {
      "name": "PayoutReward",
      "type": {
        "kind": "enum",
        "variants": [
          {
            "name": "TokenPayout",
            "fields": [
              {
                "name": "mint",
                "type": "publicKey"
              },
              {
                "name": "amount",
                "type": "u64"
              }
            ]
          },
          {
            "name": "SolPayout",
            "fields": [
              {
                "name": "amount",
                "type": "u64"
              }
            ]
          },
          {
            "name": "NftPayout",
            "fields": [
              {
                "name": "mint",
                "type": "publicKey"
              }
            ]
          },
          {
            "name": "NonePayout"
          }
        ]
      }
    }
  ],
  "errors": [
    {
      "code": 6000,
      "name": "WrongUpdateAuthority",
      "msg": "Wrong update authority provided"
    },
    {
      "code": 6001,
      "name": "WrongRootConfigAddress",
      "msg": "Root config provided does not match the required one"
    },
    {
      "code": 6002,
      "name": "LootboxNotDisabled",
      "msg": "You need to disable the lootbox in order to perform this action"
    },
    {
      "code": 6003,
      "name": "PriceNotSupported",
      "msg": "This price is not yet supported, you can only use native SOL price"
    },
    {
      "code": 6004,
      "name": "LootboxNotEnabled",
      "msg": "You need to enable the lootbox in order to perform this action"
    },
    {
      "code": 6005,
      "name": "WrongFundsDestination",
      "msg": "Wrong funds destination provided"
    },
    {
      "code": 6006,
      "name": "WrongRewardAuthority",
      "msg": "Wrong reward authority provided"
    },
    {
      "code": 6007,
      "name": "NftPayoutsNotSupported",
      "msg": "Sorry, NFT payouts are not supported at the moment"
    }
  ],
  "metadata": {
    "address": "2qPL6aunS5v636TymUKmQAG2iW6gn7q94vShQb51iyMW"
  }
};
