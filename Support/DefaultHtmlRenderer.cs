using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Theodorus2.Interfaces;

namespace Theodorus2.Support
{
    public class DefaultHtmlRenderer : IResultRenderer
    {
        private readonly ISettingsStorageService _settings;

        public DefaultHtmlRenderer(ISettingsStorageService settings)
        {
            _settings = settings;
        }

        public Task<string> RenderResults(IEnumerable<IQueryResult> resultSets)
        {
            var styleData = _settings.GetValue<string>("ResultStyle");

            return Task.Run(() =>
            {
                XNamespace ns = "http://www.w3.org/1999/xhtml";
                var result =
                    new XDocument(
                        new XDeclaration("1.0", "utf-8", "yes"),
                        new XDocumentType("html", "-//W3C//DTD XHTML 1.1//EN",
                            "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd", null),
                        new XElement(ns + "html",
                            new XAttribute("xmlns", ns),
                            new XAttribute(XNamespace.Xml + "lang", "en-us"),
                            new XAttribute("lang", "en-us"),
                            new XElement(ns + "head",
                                new XElement(ns + "style",
                                    new XAttribute("type", "text/css"), styleData),
                                new XElement(ns + "title", "Results"),
                                new XElement(ns + "meta",
                                    new XAttribute("http-equiv", "X-UA-Compatible"),
                                    new XAttribute("content", "IE=edge")
                                    )),
                            new XElement(ns + "body",
                                from resultSet in resultSets
                                select
                                    new XElement(ns + "div", new XAttribute("class", "resultset"),
                                        new XElement(ns + "div", new XAttribute("class", "query"), resultSet.Query),
                                        new XElement(ns + "div", new XAttribute("class", "duration"), resultSet.Duration),
                                        from table in
                                            resultSet.Results != null
                                                ? resultSet.Results.Tables.Cast<DataTable>()
                                                : new DataTable[0]
                                        select
                                            new XElement(ns + "table",
                                                new XElement(ns + "thead",
                                                    new XElement(ns + "tr",
                                                        from coldef in table.Columns.Cast<DataColumn>()
                                                        select new XElement(ns + "td", coldef.ColumnName))),
                                                new XElement(ns + "tbody",
                                                    from row in table.Rows.Cast<DataRow>()
                                                    select new XElement(ns + "tr",
                                                        from col in row.ItemArray
                                                        select new XElement(ns + "td", col.ToString()))),
                                                new XElement(ns + "tfoot",
                                                    new XElement(ns + "tr",
                                                        from coldef in table.Columns.Cast<DataColumn>()
                                                        select new XElement(ns + "td", coldef.ColumnName)))),
                                        resultSet.Exception != null
                                            ? new XElement(ns + "div", new XAttribute("class", "error"),
                                                resultSet.Exception.Message)
                                            : new XElement(ns + "div", new XAttribute("class", "message"), "OK")
                                        ))));

                return result.ToStringWithDeclaration();
            });
        }
    }
}
