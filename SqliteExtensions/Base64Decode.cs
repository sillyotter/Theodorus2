using System;
using System.Data.SQLite;

namespace Theodorus2.SqliteExtensions
{
	[SQLiteFunction(Arguments = 1, FuncType = FunctionType.Scalar, Name = "Base64Decode")]
	public class Base64Decode : SQLiteFunction
	{
		public override object Invoke(object[] args)
		{
			try
			{
				if (args[0] == DBNull.Value) return null;

				var encodedData = args[0] as string;
				if (String.IsNullOrEmpty(encodedData)) return new ArgumentException("Can't decode a non string");

				return Convert.FromBase64String(encodedData);
			}
			catch (Exception e)
			{
				return e;
			}
		}
	}
}