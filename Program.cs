using KnowledgeBaseConverter.Context.KnowledgeBaseNewModel;
using KnowledgeBaseConverter.Context.KnowledgeBaseOldModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KnowledgeBaseConverter
{
    static class Program
    {
        private static Dictionary<int, int> ApplicationKeys = new Dictionary<int, int>();
        private static Dictionary<int, int> CaseKeys = new Dictionary<int, int>();
        private static Dictionary<int, int> CaseTagKeys = new Dictionary<int, int>();
        private static Dictionary<int, int> DocumentKeys = new Dictionary<int, int>();
        private static Dictionary<int, int> DocumentTagKeys = new Dictionary<int, int>();
        private static Dictionary<int, int> FolderKeys = new Dictionary<int, int>();
        private static Dictionary<int, int> TagKeys = new Dictionary<int, int>();
        static void Main(string[] args)
        {
            Start();
        }

        private static void Start()
        {
            using (var oldContext = new KnowlegdeBaseOldContext())
            {
                using (var newContext = new KnowledgeBaseNewContext())
                {
                    Console.WriteLine("Start");
                    using var transaction = newContext.Database.BeginTransaction();
                    MigrateApplications(oldContext, newContext, ApplicationKeys);
                    MigrateFolders(oldContext, newContext, FolderKeys);
                    MigrateCases(oldContext, newContext, CaseKeys);
                    MigrateDocuments(oldContext, newContext, DocumentKeys);
                    MigrateTags(oldContext, newContext, TagKeys);
                    MigrateDocumentTags(oldContext, newContext, DocumentTagKeys);
                    MigrateCaseTags(oldContext, newContext, CaseTagKeys);
                    transaction.Commit();
                    Console.WriteLine("Done");
                }
            }
        }

        private static void MigrateApplications(KnowlegdeBaseOldContext oldContext, KnowledgeBaseNewContext newContext, Dictionary<int, int> applicationsKeys)
        {
            Console.WriteLine("Migrating application...");
            foreach (var oldApplication in oldContext.Applications)
            {

                var newApplication = new Context.KnowledgeBaseNewModel.Application
                {
                    ApplicationDescription = oldApplication.ApplicationDescription,
                    ApplicationIsAdminOnly = oldApplication.ApplicationIsAdminOnly != null && (bool)oldApplication.ApplicationIsAdminOnly,
                    ApplicationName = oldApplication.ApplicationName
                };
                newContext.Applications.Add(newApplication);
                newContext.SaveChanges();
                applicationsKeys.Add(oldApplication.ApplicationId, newApplication.ApplicationId);
            }

        }

        private static void MigrateCases(KnowlegdeBaseOldContext oldContext, KnowledgeBaseNewContext newContext, Dictionary<int, int> caseKeys)
        {
            Console.WriteLine("Migrating case...");

            foreach (var oldCase in oldContext.Cases)
            {
                if (!FolderKeys.TryGetValue((int)oldCase.FolderId, out var folderId))
                {
                    throw new Exception("Error when getting Folder");
                }


                var newCase = new Context.KnowledgeBaseNewModel.Case
                {
                    CaseDescription = oldCase.CaseDescription,
                    CaseName = oldCase.CaseName,
                    CaseSolution = oldCase.CaseSolution,
                    CaseDate = oldCase.CaseDate,
                    FolderId = folderId
                };
                newContext.Cases.Add(newCase);
                newContext.SaveChanges();
                caseKeys.Add(oldCase.CaseId, newCase.CaseId);

            }

        }

        private static void MigrateCaseTags(KnowlegdeBaseOldContext oldContext, KnowledgeBaseNewContext newContext,
            Dictionary<int, int> caseTagKeys)
        {
            Console.WriteLine("Migrating caseTag...");
            foreach (var oldCaseTag in oldContext.CaseTags)
            {
                if (!CaseKeys.TryGetValue((int)oldCaseTag.CaseId, out var caseId))
                {
                    throw new Exception("Error when getting Case");
                }

                if (!TagKeys.TryGetValue((int)oldCaseTag.TagId, out var tagId))
                {
                    throw new Exception("Error when getting Tag");
                }

                var newCaseTag = new Context.KnowledgeBaseNewModel.CaseTag
                {
                    CaseId = caseId,
                    TagId = tagId
                };
                newContext.CaseTags.Add(newCaseTag);
                newContext.SaveChanges();
                caseTagKeys.Add(oldCaseTag.CaseTagId, newCaseTag.CaseTagId);
            }
        }

        private static void MigrateFolders(KnowlegdeBaseOldContext oldContext, KnowledgeBaseNewContext newContext,
            Dictionary<int, int> foldersKeys)
        {
            Console.WriteLine("Migrating folder...");
            var lookup = oldContext.Folders.ToLookup(x => x.ParentFolderId);
            var res = lookup[null].SelectRecursively(x => lookup[x.FolderId].ToList());


            foreach (var oldFolder in res)
            {
                if (!ApplicationKeys.TryGetValue((int)oldFolder.ApplicationId, out var applicationId))
                {
                    throw new Exception("Error when getting Application");
                }

                if (!FolderKeys.TryGetValue(oldFolder.ParentFolderId ?? -1, out var parentFolderId))
                {
                }

                var newFolder = new Context.KnowledgeBaseNewModel.Folder
                {
                    ApplicationId = applicationId,
                    ParentFolderId = parentFolderId > 0 ? parentFolderId : (int?)null,
                    FolderName = oldFolder.FolderName,
                };
                newContext.Folders.Add(newFolder);
                newContext.SaveChanges();
                foldersKeys.Add(oldFolder.FolderId, newFolder.FolderId);
            }


        }

        public static IEnumerable<T> SelectRecursively<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> selector)
        {
            foreach (var parent in source)
            {
                yield return parent;

                var children = selector(parent);
                foreach (var child in SelectRecursively(children, selector))
                    yield return child;
            }
        }

        private static void MigrateDocuments(KnowlegdeBaseOldContext oldContext, KnowledgeBaseNewContext newContext,
            Dictionary<int, int> documentKeys)
        {
            Console.WriteLine("Migrating document...");
            foreach (var oldDocument in oldContext.Documents)
            {

                if (!CaseKeys.TryGetValue(oldDocument.CaseId ?? -1, out var caseId)) { }
                if (!FolderKeys.TryGetValue(oldDocument.FolderId ?? -1, out var folderId)) { }


                if (oldDocument.FolderId == null) continue;
                var newDocument = new Context.KnowledgeBaseNewModel.Document
                {
                    FolderId = folderId,
                    CaseId = caseId > 0 ? caseId : (int?)null,
                    DocuName = oldDocument.DocuName,
                    DocumentFile = oldDocument.DocumentFile
                };

                newContext.Documents.Add(newDocument);
                newContext.SaveChanges();
                documentKeys.Add(oldDocument.DocumentId, newDocument.DocumentId);
            }
        }

        private static void MigrateTags(KnowlegdeBaseOldContext oldContext, KnowledgeBaseNewContext newContext,
            Dictionary<int, int> tagKeys)
        {
            Console.WriteLine("Migrating tag...");
            foreach (var oldTag in oldContext.Tags)
            {
                var newTag = new Context.KnowledgeBaseNewModel.Tag
                {
                    TagName = oldTag.TagName
                };

                newContext.Tags.Add(newTag);
                newContext.SaveChanges();
                tagKeys.Add(oldTag.TagId, newTag.TagId);
            }
        }

        private static void MigrateDocumentTags(KnowlegdeBaseOldContext oldContext, KnowledgeBaseNewContext newContext,
            Dictionary<int, int> documentTagKeys)
        {
            Console.WriteLine("Migrating documentTag...");
            foreach (var oldDocumentTag in oldContext.DocumentTags)
            {
                if (!DocumentKeys.TryGetValue((int)oldDocumentTag.DocumentId, out var documentId))
                {
                    throw new Exception("Error when getting Document");
                }

                if (!TagKeys.TryGetValue((int)oldDocumentTag.TagId, out var tagId))
                {
                    throw new Exception("Error when getting Tag");
                }

                var newDocumentTag = new Context.KnowledgeBaseNewModel.DocumentTag
                {
                    DocumentId = documentId,
                    TagId = tagId
                };

                newContext.DocumentTags.Add(newDocumentTag);
                newContext.SaveChanges();
                documentTagKeys.Add(oldDocumentTag.DocumentTagId, newDocumentTag.DocumentTagId);
            }
        }
    }
}
