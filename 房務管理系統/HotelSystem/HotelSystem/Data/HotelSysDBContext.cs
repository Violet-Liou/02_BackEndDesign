using System;
using System.Collections.Generic;
using HotelSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelSystem.Data;

public partial class HotelSysDBContext : DbContext
{
    public HotelSysDBContext(DbContextOptions<HotelSysDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Employee> Employee { get; set; }

    public virtual DbSet<EmployeeRole> EmployeeRole { get; set; }

    public virtual DbSet<Member> Member { get; set; }

    public virtual DbSet<MemberAccount> MemberAccount { get; set; }

    public virtual DbSet<MemberTel> MemberTel { get; set; }

    public virtual DbSet<Order> Order { get; set; }

    public virtual DbSet<OrderDetail> OrderDetail { get; set; }

    public virtual DbSet<OrderStatus> OrderStatus { get; set; }

    public virtual DbSet<PayType> PayType { get; set; }

    public virtual DbSet<ProcessingStatus> ProcessingStatus { get; set; }

    public virtual DbSet<Room> Room { get; set; }

    public virtual DbSet<RoomPhoto> RoomPhoto { get; set; }

    public virtual DbSet<RoomService> RoomService { get; set; }

    public virtual DbSet<RoomStatus> RoomStatus { get; set; }

    public virtual DbSet<view_EmployeeWithRole> view_EmployeeWithRole { get; set; }

    public virtual DbSet<view_RoomWithPhoto> view_RoomWithPhoto { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeID).HasName("PK__Employee__7AD04FF1EC588F11");

            entity.Property(e => e.EmployeeID).IsFixedLength();
            entity.Property(e => e.Account).UseCollation("Latin1_General_CS_AS");
            entity.Property(e => e.RoleCode).IsFixedLength();

            entity.HasOne(d => d.RoleCodeNavigation).WithMany(p => p.Employee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Employee__RoleCo__4CA06362");
        });

        modelBuilder.Entity<EmployeeRole>(entity =>
        {
            entity.HasKey(e => e.RoleCode).HasName("PK__Employee__D62CB59D41DF50FD");

            entity.Property(e => e.RoleCode).IsFixedLength();
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.MemberID).HasName("PK__Member__0CF04B38AB09B511");

            entity.Property(e => e.MemberID).IsFixedLength();
        });

        modelBuilder.Entity<MemberAccount>(entity =>
        {
            entity.HasKey(e => e.Account).HasName("PK__MemberAc__B0C3AC479DBD8199");

            entity.Property(e => e.Account).UseCollation("Latin1_General_CS_AS");
            entity.Property(e => e.MemberID).IsFixedLength();

            entity.HasOne(d => d.Member).WithMany(p => p.MemberAccount)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MemberAcc__Membe__3C69FB99");
        });

        modelBuilder.Entity<MemberTel>(entity =>
        {
            entity.HasKey(e => e.SN).HasName("PK__MemberTe__32151C647DBA71C6");

            entity.Property(e => e.MemberID).IsFixedLength();

            entity.HasOne(d => d.Member).WithMany(p => p.MemberTel)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MemberTel__Membe__398D8EEE");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderID).HasName("PK__Order__C3905BAF1C445F78");

            entity.Property(e => e.OrderID).IsFixedLength();
            entity.Property(e => e.MemberID).IsFixedLength();
            entity.Property(e => e.OrderDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.PayCode).IsFixedLength();
            entity.Property(e => e.StatusCode).IsFixedLength();

            entity.HasOne(d => d.Member).WithMany(p => p.Order)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__MemberID__5441852A");

            entity.HasOne(d => d.PayCodeNavigation).WithMany(p => p.Order)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__PayCode__5535A963");

            entity.HasOne(d => d.StatusCodeNavigation).WithMany(p => p.Order)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__StatusCod__5629CD9C");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.OrderID, e.RoomID }).HasName("PK__OrderDet__40B8383E7175EC8A");

            entity.Property(e => e.OrderID).IsFixedLength();
            entity.Property(e => e.RoomID).IsFixedLength();
            entity.Property(e => e.CheckInTime).HasDefaultValueSql("(NULL)");
            entity.Property(e => e.CheckOutTime).HasDefaultValueSql("(NULL)");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetail)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Order__5BE2A6F2");

            entity.HasOne(d => d.Room).WithMany(p => p.OrderDetail)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__RoomI__5CD6CB2B");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.StatusCode).HasName("PK__OrderSta__6A7B44FDB69D5D7C");

            entity.Property(e => e.StatusCode).IsFixedLength();
        });

        modelBuilder.Entity<PayType>(entity =>
        {
            entity.HasKey(e => e.PayCode).HasName("PK__PayType__914DABCEFFB7195C");

            entity.Property(e => e.PayCode).IsFixedLength();
        });

        modelBuilder.Entity<ProcessingStatus>(entity =>
        {
            entity.HasKey(e => e.PSCode).HasName("PK__Processi__31BD33E038165069");

            entity.Property(e => e.PSCode).IsFixedLength();
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.RoomID).HasName("PK__Room__32863919FC85D379");

            entity.Property(e => e.RoomID).IsFixedLength();
            entity.Property(e => e.Area).IsFixedLength();
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.StatusCode).IsFixedLength();

            entity.HasOne(d => d.StatusCodeNavigation).WithMany(p => p.Room)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Room__StatusCode__440B1D61");
        });

        modelBuilder.Entity<RoomPhoto>(entity =>
        {
            entity.HasKey(e => e.SN).HasName("PK__RoomPhot__32151C6441CDC376");

            entity.Property(e => e.RoomID).IsFixedLength();

            entity.HasOne(d => d.Room).WithMany(p => p.RoomPhoto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoomPhoto_Room");
        });

        modelBuilder.Entity<RoomService>(entity =>
        {
            entity.HasKey(e => e.RoomServiceID).HasName("PK__RoomServ__A11E84A131120E69");

            entity.Property(e => e.RoomServiceID).IsFixedLength();
            entity.Property(e => e.CreatedTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.EmployeeID).IsFixedLength();
            entity.Property(e => e.MemberID).IsFixedLength();
            entity.Property(e => e.PSCode).IsFixedLength();
            entity.Property(e => e.RoomID).IsFixedLength();

            entity.HasOne(d => d.Employee).WithMany(p => p.RoomService).HasConstraintName("FK__RoomServi__Emplo__6383C8BA");

            entity.HasOne(d => d.Member).WithMany(p => p.RoomService)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RoomServi__Membe__628FA481");

            entity.HasOne(d => d.PSCodeNavigation).WithMany(p => p.RoomService)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RoomServi__PSCod__6477ECF3");
        });

        modelBuilder.Entity<RoomStatus>(entity =>
        {
            entity.HasKey(e => e.StatusCode).HasName("PK__RoomStat__6A7B44FD24B65C6A");

            entity.Property(e => e.StatusCode).IsFixedLength();
        });

        modelBuilder.Entity<view_EmployeeWithRole>(entity =>
        {
            entity.ToView("view_EmployeeWithRole");

            entity.Property(e => e.Account).UseCollation("Latin1_General_CS_AS");
            entity.Property(e => e.EmployeeID).IsFixedLength();
            entity.Property(e => e.RoleCode).IsFixedLength();
        });

        modelBuilder.Entity<view_RoomWithPhoto>(entity =>
        {
            entity.ToView("view_RoomWithPhoto");

            entity.Property(e => e.Area).IsFixedLength();
            entity.Property(e => e.RoomID).IsFixedLength();
            entity.Property(e => e.StatusCode).IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
