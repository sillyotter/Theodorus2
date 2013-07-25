using System;
using System.Data.SQLite;

namespace Theodorus2.SqliteExtensions
{
	[SQLiteFunction(Arguments = 1, FuncType = FunctionType.Scalar, Name = "Base64Encode")]
	public class Base64Encode : SQLiteFunction
	{
		public override object Invoke(object[] args)
		{
			try
			{
				if (args[0] == DBNull.Value) return null;

				var byteData = args[0] as byte[];
				if (byteData == null) return new ArgumentException("Can't encode non byte[]");

				return Convert.ToBase64String(byteData);
			}
			catch (Exception e)
			{
				return e;
			}
		}
	}
}