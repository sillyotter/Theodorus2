using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Nustache.Core;
using Theodorus2.Interfaces;

namespace Theodorus2.Support
{
    public class DefaultHtmlRenderer : IResultRenderer
    {
        public string RenderResults(IEnumerable<IQueryResult> input)
        {
            var tempalte = new Template();
            var sb = new StringBuilder();
            using (var sr = new StringReader("template"))
            using (var sw = new StringWriter(sb))
            {
                tempalte.Load(sr);
                tempalte.Render(input, sw, name => new Template());
            }
            return sb.ToString();
        }
    }
}