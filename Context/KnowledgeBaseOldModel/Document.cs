using System;
using System.Collections.Generic;

#nullable disable

namespace KnowledgeBaseConverter.Context.KnowledgeBaseOldModel
{
    public partial class Document
    {
        public Document()
        {
            DocumentTags = new HashSet<DocumentTag>();
        }

        public int DocumentId { get; set; }
        public int? FolderId { get; set; }
        public int? CaseId { get; set; }
        public string DocuName { get; set; }
        public byte[] DocumentFile { get; set; }

        public virtual Case Case { get; set; }
        public virtual Folder Folder { get; set; }
        public virtual ICollection<DocumentTag> DocumentTags { get; set; }
    }
}
