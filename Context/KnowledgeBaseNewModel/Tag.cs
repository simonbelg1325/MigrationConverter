using System;
using System.Collections.Generic;

#nullable disable

namespace KnowledgeBaseConverter.Context.KnowledgeBaseNewModel
{
    public partial class Tag
    {
        public Tag()
        {
            CaseTags = new HashSet<CaseTag>();
            DocumentTags = new HashSet<DocumentTag>();
        }

        public int TagId { get; set; }
        public string TagName { get; set; }

        public virtual ICollection<CaseTag> CaseTags { get; set; }
        public virtual ICollection<DocumentTag> DocumentTags { get; set; }
    }
}
