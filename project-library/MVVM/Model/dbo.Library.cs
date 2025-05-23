﻿//------------------------------------------------------------------------------
// This is auto-generated code.
//------------------------------------------------------------------------------
// This code was generated by Entity Developer tool using EF Core template.
// Code is generated on: 2025. 04. 28. 15:44:15
//
// Changes to this file may cause incorrect behavior and will be lost if
// the code is regenerated.
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Library
{

    public partial class LibraryDbContext : DbContext
    {

        public LibraryDbContext() :
            base()
        {
            OnCreated();
        }

        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) :
            base(options)
        {
            OnCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured ||
                (!optionsBuilder.Options.Extensions.OfType<RelationalOptionsExtension>().Any(ext => !string.IsNullOrEmpty(ext.ConnectionString) || ext.Connection != null) &&
                 !optionsBuilder.Options.Extensions.Any(ext => !(ext is RelationalOptionsExtension) && !(ext is CoreOptionsExtension))))
            {
                optionsBuilder.UseSqlServer(@"Server=FGP-LAPTOP\SQLEXPRESS;Database=library;Trusted_Connection=True;TrustServerCertificate=True;");
            }
            CustomizeConfiguration(ref optionsBuilder);
            base.OnConfiguring(optionsBuilder);
        }

        partial void CustomizeConfiguration(ref DbContextOptionsBuilder optionsBuilder);

        public virtual DbSet<Authors> Authors
        {
            get;
            set;
        }

        public virtual DbSet<Books> Books
        {
            get;
            set;
        }

        public virtual DbSet<Transactions> Transactions
        {
            get;
            set;
        }

        public virtual DbSet<Members> Members
        {
            get;
            set;
        }

        public virtual DbSet<AuthorBook> AuthorBooks
        {
            get;
            set;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            this.AuthorsMapping(modelBuilder);
            this.CustomizeAuthorsMapping(modelBuilder);

            this.BooksMapping(modelBuilder);
            this.CustomizeBooksMapping(modelBuilder);

            this.TransactionsMapping(modelBuilder);
            this.CustomizeTransactionsMapping(modelBuilder);

            this.MembersMapping(modelBuilder);
            this.CustomizeMembersMapping(modelBuilder);

            this.AuthorBookMapping(modelBuilder);
            this.CustomizeAuthorBookMapping(modelBuilder);

            RelationshipsMapping(modelBuilder);
            CustomizeMapping(ref modelBuilder);
        }

        #region Authors Mapping

        private void AuthorsMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Authors>().ToTable(@"Authors", @"dbo");
            modelBuilder.Entity<Authors>().Property(x => x.author_id).HasColumnName(@"author_id").IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<Authors>().Property(x => x.name).HasColumnName(@"name").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Authors>().Property(x => x.nationality).HasColumnName(@"nationality").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Authors>().Property(x => x.birthdate).HasColumnName(@"birthdate").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Authors>().HasKey(@"author_id");
        }

        partial void CustomizeAuthorsMapping(ModelBuilder modelBuilder);

        #endregion

        #region Books Mapping

        private void BooksMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Books>().ToTable(@"Books", @"dbo");
            modelBuilder.Entity<Books>().Property(x => x.book_id).HasColumnName(@"book_id").IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<Books>().Property(x => x.genre).HasColumnName(@"genre").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Books>().Property(x => x.title).HasColumnName(@"title").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Books>().Property(x => x.published_year).HasColumnName(@"published_year").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Books>().Property(x => x.availability).HasColumnName(@"availability").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Books>().Property(x => x.author_id).HasColumnName(@"author_id").ValueGeneratedNever();
            modelBuilder.Entity<Books>().Property(x => x.pic_name).HasColumnName(@"pic_name").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Books>().Property(x => x.summary).HasColumnName(@"summary").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Books>().Property(x => x.publisher).HasColumnName(@"publisher").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Books>().Property(x => x.deleted).HasColumnName(@"deleted").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Books>().HasKey(@"book_id");
        }

        partial void CustomizeBooksMapping(ModelBuilder modelBuilder);

        #endregion

        #region Transactions Mapping

        private void TransactionsMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transactions>().ToTable(@"Transactions", @"dbo");
            modelBuilder.Entity<Transactions>().Property(x => x.transaction_id).HasColumnName(@"transaction_id").IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<Transactions>().Property(x => x.borrow_date).HasColumnName(@"borrow_date").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Transactions>().Property(x => x.return_date).HasColumnName(@"return_date").ValueGeneratedNever();
            modelBuilder.Entity<Transactions>().Property(x => x.book_id).HasColumnName(@"book_id").ValueGeneratedNever();
            modelBuilder.Entity<Transactions>().Property(x => x.member_id).HasColumnName(@"member_id").ValueGeneratedNever();
            modelBuilder.Entity<Transactions>().HasKey(@"transaction_id");
        }

        partial void CustomizeTransactionsMapping(ModelBuilder modelBuilder);

        #endregion

        #region Members Mapping

        private void MembersMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Members>().ToTable(@"Members", @"dbo");
            modelBuilder.Entity<Members>().Property(x => x.member_id).HasColumnName(@"member_id").IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<Members>().Property(x => x.e_mail).HasColumnName(@"e_mail").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Members>().Property(x => x.name).HasColumnName(@"name").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Members>().Property(x => x.username).HasColumnName(@"username").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Members>().Property(x => x.password).HasColumnName(@"password").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Members>().Property(x => x.is_admin).HasColumnName(@"is_admin").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<Members>().HasKey(@"member_id");
        }

        partial void CustomizeMembersMapping(ModelBuilder modelBuilder);

        #endregion

        #region AuthorBook Mapping

        private void AuthorBookMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuthorBook>().ToTable(@"AuthorBooks", @"dbo");
            modelBuilder.Entity<AuthorBook>().Property(x => x.author_id).HasColumnName(@"author_id").ValueGeneratedNever();
            modelBuilder.Entity<AuthorBook>().Property(x => x.book_id).HasColumnName(@"book_id").ValueGeneratedNever();
            modelBuilder.Entity<AuthorBook>().Property(x => x.author_order).HasColumnName(@"author_order").IsRequired().ValueGeneratedNever();
            modelBuilder.Entity<AuthorBook>().HasKey(@"author_id", @"book_id");
        }

        partial void CustomizeAuthorBookMapping(ModelBuilder modelBuilder);

        #endregion

        private void RelationshipsMapping(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Authors>().HasMany(x => x.AuthorBooks).WithOne(op => op.Authors1).HasForeignKey(@"author_id").IsRequired(true);

            modelBuilder.Entity<Books>().HasMany(x => x.Transactions).WithOne(op => op.Books2).HasForeignKey(@"book_id").IsRequired(true);
            modelBuilder.Entity<Books>().HasMany(x => x.AuthorBooks).WithOne(op => op.Books).HasForeignKey(@"book_id").IsRequired(true);

            modelBuilder.Entity<Transactions>().HasOne(x => x.Books2).WithMany(op => op.Transactions).HasForeignKey(@"book_id").IsRequired(true);
            modelBuilder.Entity<Transactions>().HasOne(x => x.Members).WithMany(op => op.Transactions).HasForeignKey(@"member_id").IsRequired(true);

            modelBuilder.Entity<Members>().HasMany(x => x.Transactions).WithOne(op => op.Members).HasForeignKey(@"member_id").IsRequired(true);

            modelBuilder.Entity<AuthorBook>().HasOne(x => x.Authors1).WithMany(op => op.AuthorBooks).HasForeignKey(@"author_id").IsRequired(true);
            modelBuilder.Entity<AuthorBook>().HasOne(x => x.Books).WithMany(op => op.AuthorBooks).HasForeignKey(@"book_id").IsRequired(true);
        }

        partial void CustomizeMapping(ref ModelBuilder modelBuilder);

        public bool HasChanges()
        {
            return ChangeTracker.Entries().Any(e => e.State == Microsoft.EntityFrameworkCore.EntityState.Added || e.State == Microsoft.EntityFrameworkCore.EntityState.Modified || e.State == Microsoft.EntityFrameworkCore.EntityState.Deleted);
        }

        partial void OnCreated();
    }
}
