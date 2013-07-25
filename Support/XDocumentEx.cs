using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

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
}