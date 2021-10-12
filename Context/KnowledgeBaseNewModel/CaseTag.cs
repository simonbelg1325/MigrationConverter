using System;
using System.Collections.Generic;

#nullable disable

namespace KnowledgeBaseConverter.Context.KnowledgeBaseNewModel
{
    public partial class CaseTag
    {
        public int CaseTagId { get; set; }
        public int CaseId { get; set; }
        public int TagId { get; set; }

        public virtual Case Case { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
