using System;
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

            var sb = new StringBuilder();
            using (var xw = XmlWriter.Create(sb, new XmlWriterSettings
            {
                Indent=true, 
                OmitXmlDeclaration = false, 
            }))
            {
                doc.Save(xw);                
            }
            return sb.ToString();
        }
    }
}