using System;
using System.Collections.Generic;

#nullable disable

namespace KnowledgeBaseConverter.Context.KnowledgeBaseOldModel
{
    public partial class Case
    {
        public Case()
        {
            CaseTags = new HashSet<CaseTag>();
            Documents = new HashSet<Document>();
        }

        public int CaseId { get; set; }
        public string CaseSolution { get; set; }
        public string CaseName { get; set; }
        public int FolderId { get; set; }
        public string CaseDescription { get; set; }
        public DateTime? CaseDate { get; set; }

        public virtual Folder Folder { get; set; }
        public virtual ICollection<CaseTag> CaseTags { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
    }
}
