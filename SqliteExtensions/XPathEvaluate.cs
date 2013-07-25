using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Theodorus2.SqliteExtensions
{
	[SQLiteFunction(Arguments = 2, FuncType = FunctionType.Scalar, Name = "XPath")]
	public class XPathEvaluate : SQLiteFunction
	{
		public override object Invoke(object[] args)
		{
			try
			{
				if (args[0] == DBNull.Value) return null;

				var docString = args[0] as string;
				var quertString = args[1] as string;

				if (String.IsNullOrEmpty(docString)) return null;
				if (String.IsNullOrEmpty(quertString)) return new ArgumentException("Invalid XPath expression");

				var doc = XElement.Parse(docString);
				var results = doc.XPathEvaluate(quertString);

				var list = results as IEnumerable<Object>;
				if (list != null)
				{
					var listAsList = list.ToList();
					if (listAsList.Count > 0)
					{
						return listAsList.Select(item => item.ToString()).Aggregate((a, b) => a + Environment.NewLine + b);
					}
				}
			}
			catch (Exception e)
			{
				return e;
			}
			return null;
		}
	}
}