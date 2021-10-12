using System;
using System.Collections.Generic;

#nullable disable

namespace KnowledgeBaseConverter.Context.KnowledgeBaseOldModel
{
    public partial class Folder
    {
        public Folder()
        {
            Cases = new HashSet<Case>();
            Documents = new HashSet<Document>();
            InverseParentFolder = new HashSet<Folder>();
        }

        public int FolderId { get; set; }
        public int? ParentFolderId { get; set; }
        public int ApplicationId { get; set; }
        public string FolderName { get; set; }

        public virtual Application Application { get; set; }
        public virtual Folder ParentFolder { get; set; }
        public virtual ICollection<Case> Cases { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<Folder> InverseParentFolder { get; set; }
    }
}
