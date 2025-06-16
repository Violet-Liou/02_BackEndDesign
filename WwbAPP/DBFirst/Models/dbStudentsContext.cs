using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DBFirst.Models;

public partial class dbStudentsContext : DbContext
{
    //建構子
    public dbStudentsContext()
    {

    }
    //public dbStudentsContext(DbContextOptions<dbStudentsContext> options)
    //    : base(options)
    //{
    //}

    //1.2.4 在dbStudentsContext.cs裡撰寫連線到資料庫的程式
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer("Data Source=C501A105;Database=dbStudents;TrustServerCertificate=True;User ID=abcd;Password=1234");



    //屬性
    public virtual DbSet<tStudent> tStudent { get; set; }

    //覆寫 DbContext 中的OnConfiguring方法
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<tStudent>(entity =>
        {
            //資料欄位
            entity.HasKey(e => e.fStuId).HasName("PK__tStudent__08E5BA9564030369");

            entity.Property(e => e.fStuId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.fEmail).HasMaxLength(40);
            entity.Property(e => e.fName).HasMaxLength(30);
        });

        OnModelCreatingPartial(modelBuilder);
    }


    //建構新方法 OnModelCreatingPartial
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
