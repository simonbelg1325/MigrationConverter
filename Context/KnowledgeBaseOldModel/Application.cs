using System;
using System.Collections.Generic;

#nullable disable

namespace KnowledgeBaseConverter.Context.KnowledgeBaseOldModel
{
    public partial class Application
    {
        public Application()
        {
            Folders = new HashSet<Folder>();
        }

        public int ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public string ApplicationDescription { get; set; }
        public bool? ApplicationIsAdminOnly { get; set; }

        public virtual ICollection<Folder> Folders { get; set; }
    }
}
