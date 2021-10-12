using System;
using System.Collections.Generic;

#nullable disable

namespace KnowledgeBaseConverter.Context.KnowledgeBaseOldModel
{
    public partial class Tag
    {
        public Tag()
        {
            CaseTags = new HashSet<CaseTag>();
            DocumentTags = new HashSet<DocumentTag>();
        }

        public string TagName { get; set; }
        public int TagId { get; set; }

        public virtual ICollection<CaseTag> CaseTags { get; set; }
        public virtual ICollection<DocumentTag> DocumentTags { get; set; }
    }
}
