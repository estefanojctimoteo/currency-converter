﻿// <auto-generated />
using System;
using Currency_Converter.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Currency_Converter.Infra.Data.Migrations.Conversion
{
    [DbContext(typeof(ConversionContext))]
    [Migration("20220521202353_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.6");

            modelBuilder.Entity("Currency_Converter.Domain.Conversions.Conversion", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("AmountFrom")
                        .HasColumnType("decimal(18,4)");

                    b.Property<string>("CurrencyFrom")
                        .IsRequired()
                        .HasColumnType("varchar(3)");

                    b.Property<string>("CurrencyTo")
                        .IsRequired()
                        .HasColumnType("varchar(3)");

                    b.Property<DateTimeOffset?>("DateTimeUtc")
                        .IsRequired()
                        .HasColumnType("datetimeoffset(7)");

                    b.Property<decimal?>("Fee")
                        .IsRequired()
                        .HasColumnType("decimal(18,4)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Conversion");
                });

            modelBuilder.Entity("Currency_Converter.Domain.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(200)");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Currency_Converter.Domain.Conversions.Conversion", b =>
                {
                    b.HasOne("Currency_Converter.Domain.Users.User", "User")
                        .WithMany("Conversion")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
