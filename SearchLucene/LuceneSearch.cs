using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using MyWebApp.Models;
using MyWebApp.Repositories.Interfaces;

namespace MyWebApp.SearchLucene
{
    public class LuceneSearch
    {
        private ApplicationUserManager UserManager;

        private IProblemRepository problemRepository;

        public LuceneSearch(IProblemRepository problemRepository, ApplicationUserManager UserManager)
        {
            this.problemRepository = problemRepository;
            this.UserManager = UserManager;
            BuildIndex();

        }

        public static string path = Path.Combine(System.Web.HttpContext.Current.Request.PhysicalApplicationPath, "LuceneIndex");
        //Take data from database and copied in documents
        public Document GetDocument(Problem problem)
        {
            var document = new Document();
            document.Add(new Field("Title", problem.Name, Field.Store.NO, Field.Index.ANALYZED));
            document.Add(new Field("Content", problem.Text, Field.Store.NO, Field.Index.ANALYZED));
            var array = String.Join(" ", problem.Comments.Select(x => x.Text).ToArray());
            document.Add(new Field("Comments", array, Field.Store.NO, Field.Index.ANALYZED));
            document.Add(new Field("Id", value: problem.Id.ToString(CultureInfo.InvariantCulture), store: Field.Store.YES, index: Field.Index.NO));
            return document;
        }

        public void BuildIndex()
        {
            List<Problem> problems = problemRepository.Get().ToList();
            var directory = FSDirectory.Open(new DirectoryInfo(path));
            var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
            var indexWriter = new IndexWriter(directory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED);
            foreach (var problem in problems)
            {
                indexWriter.AddDocument(GetDocument(problem));
            }
            indexWriter.Optimize();
            indexWriter.Dispose();
        }

        public HashSet<int> GetSearchByField(string searchString, string field)
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
            var searchResult = new HashSet<int>();
            foreach (var scoreDoc in hits)
            {
                //Get the document that represents the search result.
                var document = searcher.Doc(scoreDoc.Doc);
                int elementId = int.Parse(document.Get("Id"));
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

        public List<int> SearchResult(string searchString)
        {
            var searchResult = GetSearchByField(searchString, "Title");
            searchResult.UnionWith(GetSearchByField(searchString, "Content"));
            searchResult.UnionWith(GetSearchByField(searchString, "Comments"));
            return searchResult.ToList();   
        }


    }
}