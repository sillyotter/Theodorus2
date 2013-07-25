using System;
using System.Data.SQLite;
using System.Text.RegularExpressions;

namespace Theodorus2.SqliteExtensions
{
	[SQLiteFunction(FuncType = FunctionType.Scalar, Name = "Replace")]
	public class Replace : SQLiteFunction
	{
		public override object Invoke(object[] args)
		{
			try
			{
				if (args.Length < 3) return new ArgumentException("Not enough parameters to Replace.");

				if (args[0].Equals(DBNull.Value)) return null;
				var col = args[0] as string;
				var exp = args[1] as string;
				var rep = args[2] as string;

				var opt = args.Length == 4 ? args[3] as string : "None";

				if (String.IsNullOrEmpty(col)) return null;
				if (String.IsNullOrEmpty(exp)) return new ArgumentException("Can't match a null regexp.");
				if (String.IsNullOrEmpty(rep)) return new ArgumentException("Can't replace with a null replacement string.");

				var options = RegexOptions.None;
				if (!String.IsNullOrEmpty(opt))
				{
					foreach (var item in opt.Split('|'))
					{
						RegexOptions res;
						if (Enum.TryParse(item, true, out res))
						{
							options |= res;
						}
					}
				}

				return Regex.Replace(col, exp, rep, options);
			}
			catch (Exception e)
			{
				return e;
			}
		}
	}
}