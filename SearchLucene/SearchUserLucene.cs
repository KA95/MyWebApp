using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using MyWebApp.Models;

namespace MyWebApp.SearchLucene
{
    public class SearchUserLucene
    {
        private ApplicationUserManager UserManager;

        public SearchUserLucene(ApplicationUserManager UserManager)
        {
            this.UserManager = UserManager;
            BuildIndexUser();
        }

        public static string path = Path.Combine(System.Web.HttpContext.Current.Request.PhysicalApplicationPath,
            "LuceneUserIndex");

        public Document GetDocumentUser(ApplicationUser applicationUser)
        {
            var document = new Document();
            document.Add(new Field("User", applicationUser.UserName, Field.Store.NO, Field.Index.ANALYZED));
            document.Add(new Field("Id", value: applicationUser.Id.ToString(CultureInfo.InvariantCulture),
                store: Field.Store.YES, index: Field.Index.NO));
            return document;
        }


        public void BuildIndexUser()
        {
            var users = UserManager.Users;
            var directory = FSDirectory.Open(new DirectoryInfo(path));
            var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            var indexWriter = new IndexWriter(directory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED);
            foreach (var user in users)
            {
                indexWriter.AddDocument(GetDocumentUser(user));
            }
            indexWriter.Optimize();
            indexWriter.Dispose();
        }

        public List<string> GetSearchUserByField(string searchString, string field)
        {
            var directory = FSDirectory.Open(new DirectoryInfo(path));
            var reader = IndexReader.Open(directory, true);
            var searcher = new IndexSearcher(reader);
            Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            //        var parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "Title", analyzer);
            var query = new FuzzyQuery(new Term(field, searchString), 0.45f);
            var collector = TopScoreDocCollector.Create(100, true);
            searcher.Search(query, collector);
            var hits = collector.TopDocs().ScoreDocs;
            var searchResult = new List<string>();
            foreach (var scoreDoc in hits)
            {
                //Get the document that represents the search result.
                var document = searcher.Doc(scoreDoc.Doc);
                string elementId = document.Get("Id");
                //The same document can be returned multiple times within the4 search results.
                if (!searchResult.Contains(elementId))
                {
                    searchResult.Add(elementId);
                }
            }
            //Now that we have the product Ids representing our search results, retrieve the products from the database.
            reader.Dispose();
            searcher.Dispose();
            analyzer.Close();
            return searchResult;
        }

        public List<string> SearchResult(string searchString)
        {
            var searchResult = GetSearchUserByField(searchString, "User");
            return searchResult;
        }
    }
}