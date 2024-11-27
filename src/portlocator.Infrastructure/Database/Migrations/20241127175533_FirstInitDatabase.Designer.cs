﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using portlocator.Infrastructure.Database;

#nullable disable

namespace portlocator.Infrastructure.Database.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241127175533_FirstInitDatabase")]
    partial class FirstInitDatabase
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("portlocator.Domain.Ports.Port", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<double>("Latitude")
                        .HasPrecision(9, 6)
                        .HasColumnType("double precision")
                        .HasColumnName("latitude");

                    b.Property<double>("Longitude")
                        .HasPrecision(9, 6)
                        .HasColumnType("double precision")
                        .HasColumnName("longitude");

                    b.Property<string>("PortName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("port_name");

                    b.HasKey("Id")
                        .HasName("pk_ports");

                    b.ToTable("ports", (string)null);
                });

            modelBuilder.Entity("portlocator.Domain.Roles.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("role_name");

                    b.HasKey("Id")
                        .HasName("pk_roles");

                    b.ToTable("roles", (string)null);
                });

            modelBuilder.Entity("portlocator.Domain.ShipCrews.ShipCrew", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<Guid>("ShipId")
                        .HasColumnType("uuid")
                        .HasColumnName("ship_id");

                    b.HasKey("UserId", "ShipId")
                        .HasName("pk_ship_crews");

                    b.HasIndex("ShipId")
                        .HasDatabaseName("ix_ship_crews_ship_id");

                    b.ToTable("ship_crews", (string)null);
                });

            modelBuilder.Entity("portlocator.Domain.Ships.Ship", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<double>("Latitude")
                        .HasPrecision(9, 6)
                        .HasColumnType("double precision")
                        .HasColumnName("latitude");

                    b.Property<double>("Longitude")
                        .HasPrecision(9, 6)
                        .HasColumnType("double precision")
                        .HasColumnName("longitude");

                    b.Property<string>("ShipName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("ship_name");

                    b.Property<double>("Velocity")
                        .HasPrecision(10, 2)
                        .HasColumnType("double precision")
                        .HasColumnName("velocity");

                    b.HasKey("Id")
                        .HasName("pk_ships");

                    b.ToTable("ships", (string)null);
                });

            modelBuilder.Entity("portlocator.Domain.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("name");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid")
                        .HasColumnName("role_id");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("RoleId")
                        .HasDatabaseName("ix_users_role_id");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("portlocator.Domain.ShipCrews.ShipCrew", b =>
                {
                    b.HasOne("portlocator.Domain.Ships.Ship", "Ship")
                        .WithMany("ShipCrews")
                        .HasForeignKey("ShipId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_ship_crews_ships_ship_id");

                    b.HasOne("portlocator.Domain.Users.User", "User")
                        .WithMany("ShipCrews")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_ship_crews_users_user_id");

                    b.Navigation("Ship");

                    b.Navigation("User");
                });

            modelBuilder.Entity("portlocator.Domain.Users.User", b =>
                {
                    b.HasOne("portlocator.Domain.Roles.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_users_roles_role_id");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("portlocator.Domain.Ships.Ship", b =>
                {
                    b.Navigation("ShipCrews");
                });

            modelBuilder.Entity("portlocator.Domain.Users.User", b =>
                {
                    b.Navigation("ShipCrews");
                });
#pragma warning restore 612, 618
        }
    }
}
