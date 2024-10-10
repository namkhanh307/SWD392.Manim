﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SWD392.Manim.Repositories.Entity;

#nullable disable

namespace SWD392.Manim.Repositories.Migrations
{
    [DbContext(typeof(Swd392Context))]
    partial class Swd392ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.ApplicationRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<DateTime>("UpdateAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("Roles", (string)null);
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.ApplicationRoleClaims", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("UpdateAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaims", (string)null);
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Avatar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("Gender")
                        .HasColumnType("bigint");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("Status")
                        .HasColumnType("bit");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTime>("UpdateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.ApplicationUserClaims", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("UpdateAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaims", (string)null);
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.ApplicationUserLogins", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdateAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogins", (string)null);
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.ApplicationUserRoles", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("UpdateAt")
                        .HasColumnType("datetime2");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles", (string)null);
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.ApplicationUserTokens", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("UpdateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserTokens", (string)null);
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.Chapter", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Createdby")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<string>("SubjectId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("SubjectId");

                    b.ToTable("Chapters");
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.Deposit", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AccountNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Createdby")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("UserId1")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId1");

                    b.ToTable("Deposits");
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.Parameter", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Createdby")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProblemId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ProblemId");

                    b.ToTable("Parameters", (string)null);
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.Problem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Createdby")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TopicId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("TopicId");

                    b.ToTable("Problems");
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.Solution", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Createdby")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("SolutionTypeId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SolutionTypeId")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Solutions", (string)null);
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.SolutionOutput", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Createdby")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SolutionId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("SolutionId")
                        .IsUnique();

                    b.ToTable("SolutionOutputs", (string)null);
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.SolutionParameter", b =>
                {
                    b.Property<string>("ParameterId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SolutionId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Createdby")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<double>("Value")
                        .HasColumnType("float");

                    b.HasKey("ParameterId", "SolutionId");

                    b.HasIndex("SolutionId");

                    b.ToTable("SolutionParameters", (string)null);
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.SolutionType", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Createdby")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProblemId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ProblemId");

                    b.ToTable("SolutionTypes", (string)null);
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.Subject", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Createdby")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.Topic", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ChapterId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Createdby")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("Number")
                        .HasColumnType("bigint");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ChapterId");

                    b.ToTable("Topics");
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.Transaction", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal?>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("BillingDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Createdby")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("DepositId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SolutionId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WalletId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("DepositId");

                    b.HasIndex("SolutionId");

                    b.HasIndex("WalletId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.Wallet", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal?>("Balance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Createdby")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasFilter("[UserId] IS NOT NULL");

                    b.ToTable("Wallets");
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.ApplicationRoleClaims", b =>
                {
                    b.HasOne("SWD392.Manim.Repositories.Entity.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.ApplicationUserClaims", b =>
                {
                    b.HasOne("SWD392.Manim.Repositories.Entity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.ApplicationUserLogins", b =>
                {
                    b.HasOne("SWD392.Manim.Repositories.Entity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.ApplicationUserRoles", b =>
                {
                    b.HasOne("SWD392.Manim.Repositories.Entity.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SWD392.Manim.Repositories.Entity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.ApplicationUserTokens", b =>
                {
                    b.HasOne("SWD392.Manim.Repositories.Entity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.Chapter", b =>
                {
                    b.HasOne("SWD392.Manim.Repositories.Entity.Subject", "Subject")
                        .WithMany("Chapters")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.Deposit", b =>
                {
                    b.HasOne("SWD392.Manim.Repositories.Entity.ApplicationUser", "User")
                        .WithMany("Deposits")
                        .HasForeignKey("UserId1");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.Parameter", b =>
                {
                    b.HasOne("SWD392.Manim.Repositories.Entity.Problem", "Problem")
                        .WithMany("Parameters")
                        .HasForeignKey("ProblemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Problem");
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.Problem", b =>
                {
                    b.HasOne("SWD392.Manim.Repositories.Entity.Topic", "Topic")
                        .WithMany("Problems")
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Topic");
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.Solution", b =>
                {
                    b.HasOne("SWD392.Manim.Repositories.Entity.SolutionType", "SolutionType")
                        .WithOne("Solution")
                        .HasForeignKey("SWD392.Manim.Repositories.Entity.Solution", "SolutionTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SWD392.Manim.Repositories.Entity.ApplicationUser", "User")
                        .WithMany("Solutions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SolutionType");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.SolutionOutput", b =>
                {
                    b.HasOne("SWD392.Manim.Repositories.Entity.Solution", "Solution")
                        .WithOne("SolutionOutput")
                        .HasForeignKey("SWD392.Manim.Repositories.Entity.SolutionOutput", "SolutionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Solution");
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.SolutionParameter", b =>
                {
                    b.HasOne("SWD392.Manim.Repositories.Entity.Parameter", "Parameter")
                        .WithMany("SolutionParameters")
                        .HasForeignKey("ParameterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SWD392.Manim.Repositories.Entity.Solution", "Solution")
                        .WithMany("SolutionParameters")
                        .HasForeignKey("SolutionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Parameter");

                    b.Navigation("Solution");
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.SolutionType", b =>
                {
                    b.HasOne("SWD392.Manim.Repositories.Entity.Problem", "Problem")
                        .WithMany("SolutionTypes")
                        .HasForeignKey("ProblemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Problem");
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.Topic", b =>
                {
                    b.HasOne("SWD392.Manim.Repositories.Entity.Chapter", "Chapter")
                        .WithMany("Topics")
                        .HasForeignKey("ChapterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Chapter");
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.Transaction", b =>
                {
                    b.HasOne("SWD392.Manim.Repositories.Entity.Deposit", "Deposit")
                        .WithMany("Transactions")
                        .HasForeignKey("DepositId");

                    b.HasOne("SWD392.Manim.Repositories.Entity.Solution", "Solution")
                        .WithMany("Transactions")
                        .HasForeignKey("SolutionId");

                    b.HasOne("SWD392.Manim.Repositories.Entity.Wallet", "Wallet")
                        .WithMany("Transactions")
                        .HasForeignKey("WalletId");

                    b.Navigation("Deposit");

                    b.Navigation("Solution");

                    b.Navigation("Wallet");
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.Wallet", b =>
                {
                    b.HasOne("SWD392.Manim.Repositories.Entity.ApplicationUser", "User")
                        .WithOne("Wallet")
                        .HasForeignKey("SWD392.Manim.Repositories.Entity.Wallet", "UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.ApplicationUser", b =>
                {
                    b.Navigation("Deposits");

                    b.Navigation("Solutions");

                    b.Navigation("Wallet");
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.Chapter", b =>
                {
                    b.Navigation("Topics");
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.Deposit", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.Parameter", b =>
                {
                    b.Navigation("SolutionParameters");
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.Problem", b =>
                {
                    b.Navigation("Parameters");

                    b.Navigation("SolutionTypes");
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.Solution", b =>
                {
                    b.Navigation("SolutionOutput");

                    b.Navigation("SolutionParameters");

                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.SolutionType", b =>
                {
                    b.Navigation("Solution");
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.Subject", b =>
                {
                    b.Navigation("Chapters");
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.Topic", b =>
                {
                    b.Navigation("Problems");
                });

            modelBuilder.Entity("SWD392.Manim.Repositories.Entity.Wallet", b =>
                {
                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
