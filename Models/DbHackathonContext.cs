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

    public virtual DbSet<TbAuthor> TbAuthors { get; set; }

    public virtual DbSet<TbBlog> TbBlogs { get; set; }

    public virtual DbSet<TbBlogCategory> TbBlogCategories { get; set; }

    public virtual DbSet<TbBook> TbBooks { get; set; }

    public virtual DbSet<TbBorrow> TbBorrows { get; set; }

    public virtual DbSet<TbCart> TbCarts { get; set; }

    public virtual DbSet<TbCategory> TbCategories { get; set; }

    public virtual DbSet<TbChapterBook> TbChapterBooks { get; set; }

    public virtual DbSet<TbComment> TbComments { get; set; }

    public virtual DbSet<TbContact> TbContacts { get; set; }

    public virtual DbSet<TbDownloaded> TbDownloadeds { get; set; }

    public virtual DbSet<TbMenu> TbMenus { get; set; }

    public virtual DbSet<TbPublisher> TbPublishers { get; set; }

    public virtual DbSet<TbReview> TbReviews { get; set; }

    public virtual DbSet<TbRole> TbRoles { get; set; }

    public virtual DbSet<TbSavedBook> TbSavedBooks { get; set; }

    public virtual DbSet<TbTagBlog> TbTagBlogs { get; set; }

    public virtual DbSet<TbUser> TbUsers { get; set; }

    public virtual DbSet<TbUserFile> TbUserFiles { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("data source= QUANGLOCPC\\QUANGLOC; initial catalog=DbHackathon; integrated security=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DocumentChunk>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Document__3214EC0794F9269F");
        });

        modelBuilder.Entity<TbAuthor>(entity =>
        {
            entity.HasKey(e => e.AuthorId);

            entity.ToTable("TB_Author");

            entity.Property(e => e.AuthorId).HasColumnName("AuthorID");
            entity.Property(e => e.Image).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(80);
        });

        modelBuilder.Entity<TbBlog>(entity =>
        {
            entity.HasKey(e => e.BlogId);

            entity.ToTable("TB_Blog");

            entity.Property(e => e.BlogId).HasColumnName("BlogID");
            entity.Property(e => e.BlogCategoryId).HasColumnName("BlogCategoryID");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(300);
            entity.Property(e => e.Image).HasMaxLength(100);
            entity.Property(e => e.TagBlogId).HasColumnName("TagBlogID");
            entity.Property(e => e.Title).HasMaxLength(150);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.BlogCategory).WithMany(p => p.TbBlogs)
                .HasForeignKey(d => d.BlogCategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_TB_Blog_TB_BlogCategory");

            entity.HasOne(d => d.User).WithMany(p => p.TbBlogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_TB_Blog_TB_User");
        });

        modelBuilder.Entity<TbBlogCategory>(entity =>
        {
            entity.HasKey(e => e.BlogCategoryId);

            entity.ToTable("TB_BlogCategory");

            entity.Property(e => e.BlogCategoryId).HasColumnName("BlogCategoryID");
            entity.Property(e => e.Description).HasMaxLength(300);
            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<TbBook>(entity =>
        {
            entity.HasKey(e => e.BookId);

            entity.ToTable("TB_Book");

            entity.Property(e => e.BookId).HasColumnName("BookID");
            entity.Property(e => e.AuthorId).HasColumnName("AuthorID");
            entity.Property(e => e.BookPdf)
                .HasMaxLength(300)
                .HasColumnName("BookPDF");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.PublisherId).HasColumnName("PublisherID");
            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.Author).WithMany(p => p.TbBooks)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_TB_Book_TB_Author");

            entity.HasOne(d => d.Category).WithMany(p => p.TbBooks)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_TB_Book_TB_Category");

            entity.HasOne(d => d.Publisher).WithMany(p => p.TbBooks)
                .HasForeignKey(d => d.PublisherId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_TB_Book_TB_Publisher");
        });

        modelBuilder.Entity<TbBorrow>(entity =>
        {
            entity.HasKey(e => e.BorrowId);

            entity.ToTable("TB_Borrow");

            entity.Property(e => e.BorrowId).HasColumnName("BorrowID");
            entity.Property(e => e.BookId).HasColumnName("BookID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Book).WithMany(p => p.TbBorrows)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_TB_Borrow_TB_Book");

            entity.HasOne(d => d.User).WithMany(p => p.TbBorrows)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_TB_Borrow_TB_User");
        });

        modelBuilder.Entity<TbCart>(entity =>
        {
            entity.HasKey(e => e.CartId);

            entity.ToTable("TB_Cart");

            entity.Property(e => e.CartId)
                .ValueGeneratedNever()
                .HasColumnName("CartID");
            entity.Property(e => e.BookId).HasColumnName("BookID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Book).WithMany(p => p.TbCarts)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_TB_Cart_TB_Book");

            entity.HasOne(d => d.User).WithMany(p => p.TbCarts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_TB_Cart_TB_User");
        });

        modelBuilder.Entity<TbCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId);

            entity.ToTable("TB_Category");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<TbChapterBook>(entity =>
        {
            entity.HasKey(e => e.ChapterBookId);

            entity.ToTable("TB_ChapterBook");

            entity.Property(e => e.ChapterBookId).HasColumnName("ChapterBookID");
            entity.Property(e => e.BookId).HasColumnName("BookID");
            entity.Property(e => e.Subject).HasMaxLength(500);

            entity.HasOne(d => d.Book).WithMany(p => p.TbChapterBooks)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_TB_ChapterBook_TB_Book");
        });

        modelBuilder.Entity<TbComment>(entity =>
        {
            entity.HasKey(e => e.CommentId);

            entity.ToTable("TB_Comment");

            entity.Property(e => e.CommentId).HasColumnName("CommentID");
            entity.Property(e => e.BlogId).HasColumnName("BlogID");
            entity.Property(e => e.Comment).HasMaxLength(300);
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Blog).WithMany(p => p.TbComments)
                .HasForeignKey(d => d.BlogId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_TB_Comment_TB_Blog");
        });

        modelBuilder.Entity<TbContact>(entity =>
        {
            entity.HasKey(e => e.ContactId);

            entity.ToTable("TB_Contact");

            entity.Property(e => e.ContactId).HasColumnName("ContactID");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(80);
            entity.Property(e => e.Time).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(100);
        });

        modelBuilder.Entity<TbDownloaded>(entity =>
        {
            entity.HasKey(e => e.DownloadedId);

            entity.ToTable("TB_Downloaded");

            entity.Property(e => e.DownloadedId).HasColumnName("DownloadedID");
            entity.Property(e => e.BookId).HasColumnName("BookID");
            entity.Property(e => e.Time).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Book).WithMany(p => p.TbDownloadeds)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_TB_Downloaded_TB_Book");

            entity.HasOne(d => d.User).WithMany(p => p.TbDownloadeds)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_TB_Downloaded_TB_User");
        });

        modelBuilder.Entity<TbMenu>(entity =>
        {
            entity.HasKey(e => e.MenuId);

            entity.ToTable("TB_Menu");

            entity.Property(e => e.MenuId).HasColumnName("MenuID");
            entity.Property(e => e.Alias)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.ParentId).HasColumnName("ParentID");
            entity.Property(e => e.Title).HasMaxLength(50);
            entity.Property(e => e.Url)
                .HasMaxLength(100)
                .HasColumnName("URL");
        });

        modelBuilder.Entity<TbPublisher>(entity =>
        {
            entity.HasKey(e => e.PublisherId);

            entity.ToTable("TB_Publisher");

            entity.Property(e => e.PublisherId).HasColumnName("PublisherID");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<TbReview>(entity =>
        {
            entity.HasKey(e => e.ReviewId);

            entity.ToTable("TB_Review");

            entity.Property(e => e.ReviewId).HasColumnName("ReviewID");
            entity.Property(e => e.BookId).HasColumnName("BookID");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Review).HasMaxLength(300);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Book).WithMany(p => p.TbReviews)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_TB_Review_TB_Book");
        });

        modelBuilder.Entity<TbRole>(entity =>
        {
            entity.HasKey(e => e.RoleId);

            entity.ToTable("TB_Role");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Description).HasMaxLength(300);
            entity.Property(e => e.Role).HasMaxLength(30);
        });

        modelBuilder.Entity<TbSavedBook>(entity =>
        {
            entity.HasKey(e => e.SavedBookId);

            entity.ToTable("TB_SavedBook");

            entity.Property(e => e.SavedBookId).HasColumnName("SavedBookID");
            entity.Property(e => e.BookId).HasColumnName("BookID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Book).WithMany(p => p.TbSavedBooks)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_TB_SavedBook_TB_Book");
        });

        modelBuilder.Entity<TbTagBlog>(entity =>
        {
            entity.HasKey(e => e.TagBlogId);

            entity.ToTable("TB_TagBlog");

            entity.Property(e => e.TagBlogId).HasColumnName("TagBlogID");
            entity.Property(e => e.BlogId).HasColumnName("BlogID");
            entity.Property(e => e.Tag).HasMaxLength(20);

            entity.HasOne(d => d.Blog).WithMany(p => p.TbTagBlogs)
                .HasForeignKey(d => d.BlogId)
                .HasConstraintName("FK_TB_TagBlog_TB_Blog");
        });

        modelBuilder.Entity<TbUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_Tb_User");

            entity.ToTable("TB_User");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Avatar).HasMaxLength(100);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Password)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.Role).WithMany(p => p.TbUsers)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_TB_User_TB_Role");
        });

        modelBuilder.Entity<TbUserFile>(entity =>
        {
            entity.HasKey(e => e.UserFileId);

            entity.ToTable("TB_UserFile");

            entity.Property(e => e.UserFileId).HasColumnName("UserFileID");
            entity.Property(e => e.Image).HasMaxLength(200);
            entity.Property(e => e.Path).HasMaxLength(200);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.TbUserFiles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_TB_UserFile_TB_User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
