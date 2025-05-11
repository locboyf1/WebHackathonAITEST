using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebHackathon.Models;

public partial class DbHackathonContext : DbContext
{
    public DbHackathonContext()
    {
    }

    public DbHackathonContext(DbContextOptions<DbHackathonContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DocumentChunk> DocumentChunks { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("data source= QUANGLOCPC\\QUANGLOC; initial catalog=DbHackathon; integrated security=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DocumentChunk>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Document__3214EC0794F9269F");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
