using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Expenses.DAL.Models
{
    public partial class ExpenseManagerContext : DbContext
    {
        public ExpenseManagerContext()
        {
        }

        public ExpenseManagerContext(DbContextOptions<ExpenseManagerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Expenseentry> Expenseentries { get; set; } = null!;
        public virtual DbSet<Expensetype> Expensetypes { get; set; } = null!;
        public virtual DbSet<Monthlyexpense> Monthlyexpenses { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Server=localhost;Database=ExpenseManager;Username=postgres;Password=S@1990t");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Expenseentry>(entity =>
            {
                entity.ToTable("expenseentry");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn()
                    .HasIdentityOptions(1000L);

                entity.Property(e => e.Additionalremarks)
                    .HasMaxLength(300)
                    .HasColumnName("additionalremarks");

                entity.Property(e => e.Createddate)
                    .HasColumnName("createddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Dueamount)
                    .HasPrecision(18, 2)
                    .HasColumnName("dueamount");

                entity.Property(e => e.Duedate).HasColumnName("duedate");

                entity.Property(e => e.Expensepaymentstatusid).HasColumnName("expensepaymentstatusid");

                entity.Property(e => e.Expensetypeid).HasColumnName("expensetypeid");

                entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");

                entity.Property(e => e.Issplittedpayment).HasColumnName("issplittedpayment");

                entity.Property(e => e.Modifieddate).HasColumnName("modifieddate");

                entity.Property(e => e.Monthlyexpenseid).HasColumnName("monthlyexpenseid");

                entity.Property(e => e.Paymentamount)
                    .HasPrecision(18, 2)
                    .HasColumnName("paymentamount");

                entity.Property(e => e.Paymentdate).HasColumnName("paymentdate");

                entity.HasOne(d => d.Expensetype)
                    .WithMany(p => p.Expenseentries)
                    .HasForeignKey(d => d.Expensetypeid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_expenseentry_exptypid_expensetype_id");

                entity.HasOne(d => d.Monthlyexpense)
                    .WithMany(p => p.Expenseentries)
                    .HasForeignKey(d => d.Monthlyexpenseid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_expenseentry_monthlyexpid_monthlyexpense_id");
            });

            modelBuilder.Entity<Expensetype>(entity =>
            {
                entity.ToTable("expensetype");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn()
                    .HasIdentityOptions(1000L);

                entity.Property(e => e.Createddate)
                    .HasColumnName("createddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Defaultdueamount)
                    .HasPrecision(18, 2)
                    .HasColumnName("defaultdueamount");

                entity.Property(e => e.Defaultduedateinmonth).HasColumnName("defaultduedateinmonth");

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .HasColumnName("description");

                entity.Property(e => e.Isactive)
                    .IsRequired()
                    .HasColumnName("isactive")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.Isrecurring).HasColumnName("isrecurring");

                entity.Property(e => e.Modifieddate).HasColumnName("modifieddate");

                entity.Property(e => e.Recurringintervaltypeid).HasColumnName("recurringintervaltypeid");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Expensetypes)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_expensetype_users_id");
            });

            modelBuilder.Entity<Monthlyexpense>(entity =>
            {
                entity.ToTable("monthlyexpense");

                entity.HasIndex(e => new { e.Userid, e.Billmonth, e.Billyear }, "monthlyexpense_userid_billmonth_billyear_key")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn()
                    .HasIdentityOptions(1000L);

                entity.Property(e => e.Additionalremarks)
                    .HasMaxLength(300)
                    .HasColumnName("additionalremarks");

                entity.Property(e => e.Billmonth).HasColumnName("billmonth");

                entity.Property(e => e.Billyear).HasColumnName("billyear");

                entity.Property(e => e.Createddate)
                    .HasColumnName("createddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Dueamount)
                    .HasPrecision(18, 2)
                    .HasColumnName("dueamount");

                entity.Property(e => e.Modifieddate).HasColumnName("modifieddate");

                entity.Property(e => e.Monthlypaymentstatusid).HasColumnName("monthlypaymentstatusid");

                entity.Property(e => e.Paidamount)
                    .HasPrecision(18, 2)
                    .HasColumnName("paidamount");

                entity.Property(e => e.Totalamount)
                    .HasPrecision(18, 2)
                    .HasColumnName("totalamount");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Monthlyexpenses)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_monthlyexpense_userid_users_id");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn()
                    .HasIdentityOptions(1000L);

                entity.Property(e => e.Accepttandc).HasColumnName("accepttandc");

                entity.Property(e => e.Createddate)
                    .HasColumnName("createddate")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Email)
                    .HasMaxLength(200)
                    .HasColumnName("email");

                entity.Property(e => e.Firstname)
                    .HasMaxLength(100)
                    .HasColumnName("firstname");

                entity.Property(e => e.Isverified).HasColumnName("isverified");

                entity.Property(e => e.Lastlogin)
                    .HasColumnName("lastlogin")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Lastname)
                    .HasMaxLength(100)
                    .HasColumnName("lastname");

                entity.Property(e => e.Mobile).HasColumnName("mobile");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .HasColumnName("password");

                entity.Property(e => e.Socialprovider)
                    .HasMaxLength(50)
                    .HasColumnName("socialprovider");

                entity.Property(e => e.Socialuserid)
                    .HasMaxLength(100)
                    .HasColumnName("socialuserid");

                entity.Property(e => e.Updateddate).HasColumnName("updateddate");

                entity.Property(e => e.Username)
                    .HasMaxLength(200)
                    .HasColumnName("username");

                entity.Property(e => e.Userstatusid).HasColumnName("userstatusid");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
