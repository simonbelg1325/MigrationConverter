using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace KnowledgeBaseConverter.Context.KnowledgeBaseOldModel
{
    public partial class KnowlegdeBaseOldContext : DbContext
    {
        public KnowlegdeBaseOldContext()
        {
        }

        public KnowlegdeBaseOldContext(DbContextOptions<KnowlegdeBaseOldContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<Case> Cases { get; set; }
        public virtual DbSet<CaseTag> CaseTags { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<DocumentTag> DocumentTags { get; set; }
        public virtual DbSet<Folder> Folders { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=KnowledgeBase;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<Application>(entity =>
            {
                entity.ToTable("Application");

                entity.Property(e => e.ApplicationId).HasColumnName("applicationId");

                entity.Property(e => e.ApplicationDescription)
                    .HasColumnType("text")
                    .HasColumnName("applicationDescription");

                entity.Property(e => e.ApplicationIsAdminOnly)
                    .IsRequired()
                    .HasColumnName("applicationIsAdminOnly")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.ApplicationName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("applicationName");
            });

            modelBuilder.Entity<Case>(entity =>
            {
                entity.ToTable("Case");

                entity.Property(e => e.CaseId).HasColumnName("caseId");

                entity.Property(e => e.CaseDate)
                    .HasColumnType("datetime")
                    .HasColumnName("caseDate");

                entity.Property(e => e.CaseDescription)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("caseDescription");

                entity.Property(e => e.CaseName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("caseName");

                entity.Property(e => e.CaseSolution)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("caseSolution");

                entity.Property(e => e.FolderId).HasColumnName("folderId");

                entity.HasOne(d => d.Folder)
                    .WithMany(p => p.Cases)
                    .HasForeignKey(d => d.FolderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Case_Folder");
            });

            modelBuilder.Entity<CaseTag>(entity =>
            {
                entity.ToTable("CaseTag");

                entity.Property(e => e.CaseTagId).HasColumnName("caseTagId");

                entity.Property(e => e.CaseId).HasColumnName("caseId");

                entity.Property(e => e.TagId).HasColumnName("tagId");

                entity.HasOne(d => d.Case)
                    .WithMany(p => p.CaseTags)
                    .HasForeignKey(d => d.CaseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CaseTag_Case");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.CaseTags)
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CaseTag_CaseTag");
            });

            modelBuilder.Entity<Document>(entity =>
            {
                entity.Property(e => e.DocumentId).HasColumnName("documentId");

                entity.Property(e => e.CaseId).HasColumnName("caseId");

                entity.Property(e => e.DocuName)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("docuName");

                entity.Property(e => e.FolderId).HasColumnName("folderId");

                entity.HasOne(d => d.Case)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.CaseId)
                    .HasConstraintName("FK_Documents_Case");

                entity.HasOne(d => d.Folder)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.FolderId)
                    .HasConstraintName("FK_Documents_Folder");
            });

            modelBuilder.Entity<DocumentTag>(entity =>
            {
                entity.ToTable("DocumentTag");

                entity.Property(e => e.DocumentTagId).HasColumnName("documentTagId");

                entity.Property(e => e.DocumentId).HasColumnName("documentId");

                entity.Property(e => e.TagId).HasColumnName("tagId");

                entity.HasOne(d => d.Document)
                    .WithMany(p => p.DocumentTags)
                    .HasForeignKey(d => d.DocumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DocumentTag_Documents");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.DocumentTags)
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DocumentTag_Tags");
            });

            modelBuilder.Entity<Folder>(entity =>
            {
                entity.ToTable("Folder");

                entity.Property(e => e.FolderId).HasColumnName("folderId");

                entity.Property(e => e.ApplicationId).HasColumnName("applicationId");

                entity.Property(e => e.FolderName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("folderName");

                entity.Property(e => e.ParentFolderId).HasColumnName("parentFolderId");

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.Folders)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Folder_Application");

                entity.HasOne(d => d.ParentFolder)
                    .WithMany(p => p.InverseParentFolder)
                    .HasForeignKey(d => d.ParentFolderId)
                    .HasConstraintName("FK_Folder_Folder");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.Property(e => e.TagId).HasColumnName("tagId");

                entity.Property(e => e.TagName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("tagName");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
