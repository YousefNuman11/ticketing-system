using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;

namespace TicketingSystem.Repository.Search
{
    public class LuceneUserSearchService : IUserSearchService, IDisposable
    {
        private const LuceneVersion AppLuceneVersion = LuceneVersion.LUCENE_48;

        private readonly FSDirectory _directory;
        private readonly StandardAnalyzer _analyzer;
        private readonly IndexWriter _writer;
        private readonly object _lock = new();

        public LuceneUserSearchService(string indexPath)
        {
            if (!System.IO.Directory.Exists(indexPath))
                System.IO.Directory.CreateDirectory(indexPath);

            _directory = FSDirectory.Open(indexPath);
            _analyzer = new StandardAnalyzer(AppLuceneVersion);

            var config = new IndexWriterConfig(AppLuceneVersion, _analyzer)
            {
                OpenMode = OpenMode.CREATE_OR_APPEND
            };

            _writer = new IndexWriter(_directory, config);
        }

        public void IndexUser(Guid id, string fullName, string email, string? address, string role)
        {
            lock (_lock)
            {
                var doc = new Document
                {
                    new StringField("id", id.ToString(), Field.Store.YES),
                    new TextField("fullName", fullName ?? string.Empty, Field.Store.YES),
                    new TextField("email", email ?? string.Empty, Field.Store.YES),
                    new TextField("address", address ?? string.Empty, Field.Store.YES),
                    new StringField("role", role ?? string.Empty, Field.Store.YES),
                };

                _writer.UpdateDocument(new Term("id", id.ToString()), doc);
                _writer.Flush(triggerMerge: false, applyAllDeletes: false);
                _writer.Commit();
            }
        }

        public void DeleteUser(Guid id)
        {
            lock (_lock)
            {
                _writer.DeleteDocuments(new Term("id", id.ToString()));
                _writer.Commit();
            }
        }

        public List<Guid> Search(string queryText, string? roleFilter = null, int maxResults = 200)
        {
            if(string.IsNullOrWhiteSpace(queryText))
                return new List<Guid>();

            lock (_lock)
            {
                using var reader = _writer.GetReader(applyAllDeletes: true);
                var searcher = new IndexSearcher(reader);

                var escaped = QueryParserBase.Escape(queryText.Trim());

                var parser = new MultiFieldQueryParser(
                    AppLuceneVersion,
                     new[] { "fullName", "email", "address" },
                    _analyzer);
                parser.DefaultOperator = QueryParserBase.OR_OPERATOR;

                var rawQuery = string.Join(" ", escaped
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(term => $"{term}*"));

                Query query = parser.Parse(rawQuery);

                if (!string.IsNullOrWhiteSpace(roleFilter))
                {
                    var roleQuery = new TermQuery(new Term("role", roleFilter));
                    var combined = new BooleanQuery
                    {
                        { query, Occur.MUST },
                        { roleQuery, Occur.MUST }
                    };
                    query = combined;
                }

                var hits = searcher.Search(query, maxResults);

                return hits.ScoreDocs
                    .Select(sd => searcher.Doc(sd.Doc))
                    .Select(d => Guid.Parse(d.Get("id")))
                    .ToList();
            }
        }
        public void Dispose()
        {
            _writer?.Dispose();
            _directory?.Dispose();
            _analyzer?.Dispose();
        }
    }
}
