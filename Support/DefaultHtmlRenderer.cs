using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Automation.Peers;
using System.Xml;
using System.Xml.Linq;
using Theodorus2.Interfaces;

namespace Theodorus2.Support
{
    public static class XDocumentEx
    {
        public static string ToStringWithDeclaration(this XDocument doc)
        {
            if (doc == null)
            {
                throw new ArgumentNullException("doc");
            }

            using (var ms = new MemoryStream())
            using (var xw = XmlWriter.Create(ms,
                new XmlWriterSettings
                {
                    ConformanceLevel = ConformanceLevel.Document,
                    Encoding = Encoding.UTF8,
                    Indent = true,
                    OmitXmlDeclaration = false,
                }))
            {
                doc.Save(xw);
                xw.Flush();
                ms.Flush();
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
    }

    public class DefaultHtmlRenderer : IResultRenderer
    {
        private static string styleData = @"
		    body {
		        font-size: 12px;
		    }
			
			pre {
				font-family: ""Consolas"", monospace;
				color: #444444;
				margin-left: 1em;
				font-size: 12px;
			}
		
			h3 {
				font-family: ""Calibri"", sans-serif;
				font-size: 12pt;
				border: 1px solid black;
				padding-left: 2px;
				background-color: #EEEEFF;
				clear: both;
			}
					
			table {
				margin-left: 1em;
				border-collapse: collapse;
				border-style: solid;
				border-color: black;
				border-width: 2px;
				clear: both;
				width: auto;
				margin-bottom: 1em;
				margin-right: 1em;
			}
			
			hr {
			    color: Black; 
			}
			
			table tbody {
				font-family: ""Consolas"", monospace;
				font-size: 12px;
			}
			
			table thead, table tfoot {
				font-family: ""Calibri"", sans-serif;
				background-color: #6688CC;
				color: white;
			}
			
			table thead {
				border-bottom-style: solid;
				border-bottom-color: black;
				border-bottom-width: 2px;
			}
			table tfoot {
				border-top-style: solid;
				border-top-color: black;
				border-top-width: 2px;
			}
			table th, table td {
				text-align: left;
				border-width: 1px;
				border-left-style: solid;
				border-right-style: solid;
				border-left-color: black;
				border-right-color: black;
				padding: 2px;
				padding-left: 1em;
				padding-right: 1em;
			}
			table th {
				text-align: center;
			}
		
			.error {
			    color: Red;
			}
			
			.answer {
			    border-bottom: 2px solid black;
			}
			
			.result	{
			    margin: 10px;
			}
";

        // TODO: Contact options dband find limits, take only that manyfrom each datatable
        public string RenderResults(IEnumerable<IQueryResult> input)
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
                                new XAttribute("http-equiv", "Content-Type"),
                                new XAttribute("content", "text/html; charset=utf-8")
                                )),
                        new XElement(ns + "body",
                            from item in input
                            select new XElement(ns + "div", new XAttribute("class", "answer"),
                                new XElement(ns + "h3", new XAttribute("class", "queryheader"), "Query:"),
                                new XElement(ns + "div", new XAttribute("class", "result"),
                                    new XElement(ns + "pre", item.Query),
                                    new XElement(ns + "h3", new XAttribute("class", "resulthreader"),
                                        new XElement(ns + "span",
                                            new XAttribute("style", "font-size: small; float: right;"),
                                            String.Format("(Elapsed {0} ms.)", item.Duration)), 
                                            "Results:"),
                                    from table in item.Results.Tables.Cast<DataTable>()
                                    select
                                        new XElement(ns + "table",
                                            new XAttribute("cellpadding", "0"),
                                            new XAttribute("cellspacing", "0"),
                                            new XAttribute("border", "0"),
                                            new XAttribute("class", "display"),
                                            new XElement(ns + "thead",
                                                new XElement(ns + "tr",
                                                    from coldef in table.Columns.Cast<DataColumn>()
                                                    select new XElement(ns + "td", coldef.ColumnName)
                                                    )),
                                            new XElement(ns + "tbody",
                                                from row in table.Rows.Cast<DataRow>()
                                                select new XElement(ns + "tr",
                                                    from col in row.ItemArray
                                                    select new XElement(ns + "td", col.ToString()))),
                                            new XElement(ns + "tfoot",
                                                new XElement(ns + "tr",
                                                    from coldef in table.Columns.Cast<DataColumn>()
                                                    select new XElement(ns + "td", coldef.ColumnName)
                                                    ))
                                            ))))));

            return result.ToStringWithDeclaration();
        }
    }
}
