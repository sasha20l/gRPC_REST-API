﻿// <auto-generated />
using System;
using Infrastructure.DbCont;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(GameServiceDbContext))]
    partial class GameServiceDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Infrastructure.GameTransactions", b =>
                {
                    b.Property<long>("GameTransactionsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("GameTransactionsId"));

                    b.Property<double>("Amount")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<long>("fkFromUserId")
                        .HasColumnType("bigint");

                    b.Property<long>("fkToUserId")
                        .HasColumnType("bigint");

                    b.HasKey("GameTransactionsId");

                    b.HasIndex("fkFromUserId");

                    b.HasIndex("fkToUserId");

                    b.ToTable("GameTransactions");
                });

            modelBuilder.Entity("Infrastructure.MatchHistory", b =>
                {
                    b.Property<long>("MatchHistoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("MatchHistoryId"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<double>("Stake")
                        .HasColumnType("double precision");

                    b.Property<long>("fkPlayer1Id")
                        .HasColumnType("bigint");

                    b.Property<long?>("fkPlayer2Id")
                        .HasColumnType("bigint");

                    b.Property<long?>("fkWinnerId")
                        .HasColumnType("bigint");

                    b.HasKey("MatchHistoryId");

                    b.HasIndex("fkPlayer1Id");

                    b.HasIndex("fkPlayer2Id");

                    b.HasIndex("fkWinnerId");

                    b.ToTable("MatchHistory");
                });

            modelBuilder.Entity("Infrastructure.User", b =>
                {
                    b.Property<long>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("UserId"));

                    b.Property<double>("Balance")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("UserId");

                    b.HasIndex("UserName");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Infrastructure.GameTransactions", b =>
                {
                    b.HasOne("Infrastructure.User", "FromUser")
                        .WithMany("ListFromUser")
                        .HasForeignKey("fkFromUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Infrastructure.User", "ToUser")
                        .WithMany("ListToUser")
                        .HasForeignKey("fkToUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FromUser");

                    b.Navigation("ToUser");
                });

            modelBuilder.Entity("Infrastructure.MatchHistory", b =>
                {
                    b.HasOne("Infrastructure.User", "Player1")
                        .WithMany("ListPlayer1")
                        .HasForeignKey("fkPlayer1Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Infrastructure.User", "Player2")
                        .WithMany("ListPlayer2")
                        .HasForeignKey("fkPlayer2Id");

                    b.HasOne("Infrastructure.User", "Winner")
                        .WithMany("ListWinner")
                        .HasForeignKey("fkWinnerId");

                    b.Navigation("Player1");

                    b.Navigation("Player2");

                    b.Navigation("Winner");
                });

            modelBuilder.Entity("Infrastructure.User", b =>
                {
                    b.Navigation("ListFromUser");

                    b.Navigation("ListPlayer1");

                    b.Navigation("ListPlayer2");

                    b.Navigation("ListToUser");

                    b.Navigation("ListWinner");
                });
#pragma warning restore 612, 618
        }
    }
}
