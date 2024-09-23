﻿// <auto-generated />
using AspNetCoreSearchApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AspNetCoreSearchApp.Migrations
{
    [DbContext(typeof(SearchContext))]
    [Migration("20240922155541_AddJsonSchema")]
    partial class AddJsonSchema
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("AspNetCoreSearchApp.Models.JsonDoc", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("DocumentName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("JsonData")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("JsonSchemaId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("JsonSchemaId");

                    b.ToTable("JsonDocuments");
                });

            modelBuilder.Entity("AspNetCoreSearchApp.Models.JsonSchema", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("SchemaData")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SchemaName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("JsonSchemas");
                });

            modelBuilder.Entity("AspNetCoreSearchApp.Models.JsonDoc", b =>
                {
                    b.HasOne("AspNetCoreSearchApp.Models.JsonSchema", "JsonSchema")
                        .WithMany()
                        .HasForeignKey("JsonSchemaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("JsonSchema");
                });
#pragma warning restore 612, 618
        }
    }
}
