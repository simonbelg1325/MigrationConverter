using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace KnowledgeBaseConverter.Context.KnowledgeBaseNewModel
{
    public partial class KnowledgeBaseNewContext : DbContext
    {
        public KnowledgeBaseNewContext()
        {
        }

        public KnowledgeBaseNewContext(DbContextOptions<KnowledgeBaseNewContext> options)
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
                optionsBuilder.UseNpgsql("Server=localhost;Database=KnowledgeBase;Port=5433;User Id=postgres;Password=postgres;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Dutch_Belgium.1252");

            modelBuilder.Entity<Case>(entity =>
            {
                entity.HasIndex(e => e.FolderId, "IX_Cases_FolderId");

                entity.HasOne(d => d.Folder)
                    .WithMany(p => p.Cases)
                    .HasForeignKey(d => d.FolderId);
            });

            modelBuilder.Entity<CaseTag>(entity =>
            {
                entity.HasIndex(e => e.CaseId, "IX_CaseTags_CaseId");

                entity.HasIndex(e => e.TagId, "IX_CaseTags_TagId");

                entity.HasOne(d => d.Case)
                    .WithMany(p => p.CaseTags)
                    .HasForeignKey(d => d.CaseId);

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.CaseTags)
                    .HasForeignKey(d => d.TagId);
            });

            modelBuilder.Entity<Document>(entity =>
            {
                entity.HasIndex(e => e.CaseId, "IX_Documents_CaseId");

                entity.HasIndex(e => e.FolderId, "IX_Documents_FolderId");

                entity.HasOne(d => d.Case)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.CaseId);

                entity.HasOne(d => d.Folder)
                    .WithMany(p => p.Documents)
                    .HasForeignKey(d => d.FolderId);
            });

            modelBuilder.Entity<DocumentTag>(entity =>
            {
                entity.HasIndex(e => e.DocumentId, "IX_DocumentTags_DocumentId");

                entity.HasIndex(e => e.TagId, "IX_DocumentTags_TagId");

                entity.HasOne(d => d.Document)
                    .WithMany(p => p.DocumentTags)
                    .HasForeignKey(d => d.DocumentId);

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.DocumentTags)
                    .HasForeignKey(d => d.TagId);
            });

            modelBuilder.Entity<Folder>(entity =>
            {
                entity.HasIndex(e => e.ApplicationId, "IX_Folders_ApplicationId");

                entity.HasIndex(e => e.ParentFolderId, "IX_Folders_ParentFolderId");

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.Folders)
                    .HasForeignKey(d => d.ApplicationId);

                entity.HasOne(d => d.ParentFolder)
                    .WithMany(p => p.InverseParentFolder)
                    .HasForeignKey(d => d.ParentFolderId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
