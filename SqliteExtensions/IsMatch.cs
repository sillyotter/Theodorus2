using System;
using System.Data.SQLite;
using System.Text.RegularExpressions;

namespace Theodorus2.SqliteExtensions
{
	[SQLiteFunction(FuncType = FunctionType.Scalar, Name = "IsMatch")]
	public class IsMatch : SQLiteFunction
	{
		public override object Invoke(object[] args)
		{
			try
			{
				if (args.Length < 2) return new ArgumentException("Not enough parameters to IsMatch");
				if (args[0].Equals(DBNull.Value)) return null;
				var col = args[0] as string;
				var exp = args[1] as string;
				var opt = args.Length == 3 ? args[2] as string : "None";

				if (String.IsNullOrEmpty(col)) return null;
				if (String.IsNullOrEmpty(exp)) return new ArgumentException("Can't match a null regexp");

				RegexOptions options = RegexOptions.None;
				if (!String.IsNullOrEmpty(opt))
				{
					foreach(var item in opt.Split('|'))
					{
						RegexOptions res;
						if(Enum.TryParse(item, true, out res))
						{
							options |= res;
						}
					}
				}

				return Regex.IsMatch(col, exp, options);
			}
			catch (Exception e)
			{
				return e;
			}
		}
	}
}