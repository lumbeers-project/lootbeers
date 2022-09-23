﻿// <auto-generated />
using LootbeersBackend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LootbeersBackend.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20220920190031_IndexedLootbox")]
    partial class IndexedLootbox
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("LootbeersBackend.Models.Dao.Lootbox", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("SolanaLootboxAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("SolanaLootboxAddress");

                    b.ToTable("Lootboxes");
                });

            modelBuilder.Entity("LootbeersBackend.Models.Dao.Reward", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("LootboxId")
                        .HasColumnType("integer");

                    b.Property<int>("RewardType")
                        .HasColumnType("integer");

                    b.Property<int>("Weight")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("LootboxId");

                    b.ToTable("Rewards");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Reward");
                });

            modelBuilder.Entity("LootbeersBackend.Models.Dao.NftReward", b =>
                {
                    b.HasBaseType("LootbeersBackend.Models.Dao.Reward");

                    b.Property<string>("MintAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasDiscriminator().HasValue("NftReward");
                });

            modelBuilder.Entity("LootbeersBackend.Models.Dao.NoneReward", b =>
                {
                    b.HasBaseType("LootbeersBackend.Models.Dao.Reward");

                    b.HasDiscriminator().HasValue("NoneReward");
                });

            modelBuilder.Entity("LootbeersBackend.Models.Dao.SolReward", b =>
                {
                    b.HasBaseType("LootbeersBackend.Models.Dao.Reward");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric(20,0)");

                    b.HasDiscriminator().HasValue("SolReward");
                });

            modelBuilder.Entity("LootbeersBackend.Models.Dao.TokenReward", b =>
                {
                    b.HasBaseType("LootbeersBackend.Models.Dao.Reward");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric(20,0)")
                        .HasColumnName("TokenReward_Amount");

                    b.Property<string>("MintAddress")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("TokenReward_MintAddress");

                    b.HasDiscriminator().HasValue("TokenReward");
                });

            modelBuilder.Entity("LootbeersBackend.Models.Dao.Reward", b =>
                {
                    b.HasOne("LootbeersBackend.Models.Dao.Lootbox", "Lootbox")
                        .WithMany("Rewards")
                        .HasForeignKey("LootboxId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lootbox");
                });

            modelBuilder.Entity("LootbeersBackend.Models.Dao.Lootbox", b =>
                {
                    b.Navigation("Rewards");
                });
#pragma warning restore 612, 618
        }
    }
}
