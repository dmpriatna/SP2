﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SP2.Data;

namespace SP2.Migrations
{
    [DbContext(typeof(GoLogContext))]
    [Migration("20211121041159_Add_TRANSACTION_INVOICE")]
    partial class Add_TRANSACTION_INVOICE
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("SP2.Data.InvoiceDetailPlatformFee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<double>("InvoiceAmount")
                        .HasColumnType("double precision");

                    b.Property<Guid>("InvoiceId")
                        .HasColumnType("uuid");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("RowStatus")
                        .HasColumnType("boolean");

                    b.Property<Guid>("TransactionTypeId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("InvoiceDetailPlatformFee");
                });

            modelBuilder.Entity("SP2.Data.InvoicePlatformFee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uuid");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<double>("DiscAmount")
                        .HasColumnType("double precision");

                    b.Property<string>("InvoiceNumber")
                        .HasColumnType("text");

                    b.Property<string>("InvoiceStatus")
                        .HasColumnType("text");

                    b.Property<bool>("IsContract")
                        .HasColumnType("boolean");

                    b.Property<string>("JobNumber")
                        .HasColumnType("text");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("PaidThru")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("RowStatus")
                        .HasColumnType("boolean");

                    b.Property<double>("TotalAmount")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.ToTable("InvoicePlatformFee");
                });

            modelBuilder.Entity("SP2.Data.RateContract", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ContractId")
                        .HasColumnType("uuid");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<double>("RateNominal")
                        .HasColumnType("double precision");

                    b.Property<bool>("RowStatus")
                        .HasColumnType("boolean");

                    b.Property<Guid>("TransactionTypeId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("RateContract");
                });

            modelBuilder.Entity("SP2.Data.RatePlateformFee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("RateContractId")
                        .HasColumnType("uuid");

                    b.Property<double>("RateNominal")
                        .HasColumnType("double precision");

                    b.Property<bool>("RowStatus")
                        .HasColumnType("boolean");

                    b.Property<Guid>("TransactionTypeId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("RatePlatformFee");
                });

            modelBuilder.Entity("SP2.Data.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uuid");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("Delegated")
                        .HasColumnType("boolean");

                    b.Property<string>("JobNumber")
                        .HasColumnType("text");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("RowStatus")
                        .HasColumnType("boolean");

                    b.Property<string>("TransactionNumber")
                        .HasColumnType("text");

                    b.Property<Guid>("TransactionTypeId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Transaction");
                });

            modelBuilder.Entity("SP2.Data.TransactionType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("RowStatus")
                        .HasColumnType("boolean");

                    b.Property<string>("TableName")
                        .HasColumnType("text");

                    b.Property<string>("TransactionName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("TransactionType");
                });
#pragma warning restore 612, 618
        }
    }
}