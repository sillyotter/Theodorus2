using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Theodorus2.Support;
using UnitTests.ViewModels;
using Xunit;

namespace UnitTests.Support
{
    

    public class DefaultHtmlRendererTests
    {
        private readonly XNamespace _ns = "http://www.w3.org/1999/xhtml";

        [Fact]
        public async Task EmptyResultsTest()
        {
            var r = new DefaultHtmlRenderer( new DummySettingsStorageService());

            var result = await r.RenderResults(new QueryResult[] {});
            var presult = XDocument.Parse(result);
            Assert.Equal(0, presult.Descendants(_ns + "table").Count());
        }

        [Fact]
        public async Task SingleResultTest()
        {
            var r = new DefaultHtmlRenderer(new DummySettingsStorageService());

            var result = await r.RenderResults(new[] { new QueryResult("asdf", 100, new DataSet()) });
            var presult = XDocument.Parse(result);
            Assert.Equal(1, presult.Descendants(_ns + "div").Count(x => x.Attribute("class").Value == "resultset"));
            Assert.Equal(1, presult.Descendants(_ns + "div").Count(x => x.Attribute("class").Value == "query"));
            Assert.Equal(1, presult.Descendants(_ns + "div").Count(x => x.Attribute("class").Value == "duration"));
        }

        [Fact]
        public async Task SingleResultWithDataTest()
        {
            var r = new DefaultHtmlRenderer(new DummySettingsStorageService());
            var ds = new DataSet();
            ds.Tables.Add(new DataTable("ASDF"));

            var result = await r.RenderResults(new[] {new QueryResult("asdf", 100, ds)});
            var presult = XDocument.Parse(result);
            Assert.Equal(1, presult.Descendants(_ns + "table").Count());
        }

        [Fact]
        public async Task SingleResultWithRealValues()
        {
            var r = new DefaultHtmlRenderer(new DummySettingsStorageService());
            var ds = new DataSet();
            var dt = new DataTable("ASDF");
            dt.Columns.Add("ASDF");
            dt.Columns.Add("WQER");
            dt.Columns.Add("TRYTTR");
            dt.Rows.Add(1, 2, 3);
            dt.Rows.Add(3, 4, 5);
            dt.Rows.Add(5,67, 8);
            ds.Tables.Add(dt);
            
            var result = await r.RenderResults(new[] { new QueryResult("asdf", 100, ds) });
            var presult = XDocument.Parse(result);
            Assert.Equal(1, presult.Descendants(_ns + "table").Count());
            Assert.Equal(1, presult.Descendants(_ns + "thead").Count());
            Assert.Equal(1, presult.Descendants(_ns + "tfoot").Count());
            Assert.Equal(5, presult.Descendants(_ns + "tr").Count());
            Assert.Equal(15, presult.Descendants(_ns + "td").Count());
        }

        [Fact]
        public async Task TwoTablesOneDataSetResultWithRealValues()
        {
            var r = new DefaultHtmlRenderer(new DummySettingsStorageService());
            var ds = new DataSet();
            var dt = new DataTable("ASDF");
            dt.Columns.Add("ASDF");
            dt.Columns.Add("WQER");
            dt.Columns.Add("TRYTTR");
            dt.Rows.Add(1, 2, 3);
            dt.Rows.Add(3, 4, 5);
            dt.Rows.Add(5, 67, 8);
            ds.Tables.Add(dt);
            dt = new DataTable("PRT");
            dt.Columns.Add("ASDF");
            dt.Columns.Add("WQER");
            dt.Columns.Add("TRYTTR");
            dt.Rows.Add(1, 2, 3);
            dt.Rows.Add(3, 4, 5);
            dt.Rows.Add(5, 67, 8);
            ds.Tables.Add(dt);

            var result = await r.RenderResults(new[] { new QueryResult("asdf", 100, ds) });
            var presult = XDocument.Parse(result);
            Assert.Equal(2, presult.Descendants(_ns + "table").Count());
            Assert.Equal(2, presult.Descendants(_ns + "thead").Count());
            Assert.Equal(2, presult.Descendants(_ns + "tfoot").Count());
            Assert.Equal(2*5, presult.Descendants(_ns + "tr").Count());
            Assert.Equal(2*15, presult.Descendants(_ns + "td").Count());

            Assert.Equal(1, presult.Descendants(_ns + "div").Count(x => x.Attribute("class").Value == "resultset"));
            Assert.Equal(1, presult.Descendants(_ns + "div").Count(x => x.Attribute("class").Value == "query"));
            Assert.Equal(1, presult.Descendants(_ns + "div").Count(x => x.Attribute("class").Value == "duration"));
        }

        [Fact]
        public async Task TwoTablesTwoDataSetResultWithRealValues()
        {
            var r = new DefaultHtmlRenderer(new DummySettingsStorageService());
            var ds = new DataSet();
            var dt = new DataTable("ASDF");
            dt.Columns.Add("ASDF");
            dt.Columns.Add("WQER");
            dt.Columns.Add("TRYTTR");
            dt.Rows.Add(1, 2, 3);
            dt.Rows.Add(3, 4, 5);
            dt.Rows.Add(5, 67, 8);
            ds.Tables.Add(dt);
            dt = new DataTable("PRT");
            dt.Columns.Add("ASDF");
            dt.Columns.Add("WQER");
            dt.Columns.Add("TRYTTR");
            dt.Rows.Add(1, 2, 3);
            dt.Rows.Add(3, 4, 5);
            dt.Rows.Add(5, 67, 8);
            ds.Tables.Add(dt);

            var result = await r.RenderResults(new[] { new QueryResult("asdf", 100, ds), new QueryResult("ERTER", 100, ds),  });
            var presult = XDocument.Parse(result);
            Assert.Equal(4, presult.Descendants(_ns + "table").Count());
            Assert.Equal(4, presult.Descendants(_ns + "thead").Count());
            Assert.Equal(4, presult.Descendants(_ns + "tfoot").Count());
            Assert.Equal(4 * 5, presult.Descendants(_ns + "tr").Count());
            Assert.Equal(4 * 15, presult.Descendants(_ns + "td").Count());

            Assert.Equal(2, presult.Descendants(_ns + "div").Count(x => x.Attribute("class").Value == "resultset"));
            Assert.Equal(2, presult.Descendants(_ns + "div").Count(x => x.Attribute("class").Value == "query"));
            Assert.Equal(2, presult.Descendants(_ns + "div").Count(x => x.Attribute("class").Value == "duration"));
        
        }
    }
}
