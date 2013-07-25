using System;
using System.Data.SQLite;

namespace Theodorus2.SqliteExtensions
{
	[SQLiteFunction(Arguments = 1, FuncType = FunctionType.Scalar, Name = "HexDecode")]
	public class HexDecode : SQLiteFunction
	{
		public override object Invoke(object[] args)
		{
			try
			{
				if (args[0] == DBNull.Value) return null;

				var stringData = args[0] as string;
				if (String.IsNullOrEmpty(stringData)) return new ArgumentException("Can't hexdecode a non string");

				var byteData = new byte[stringData.Length/2];
				var charData = stringData.ToCharArray();
				for (var i = 0; i < byteData.Length; ++i)
				{
					var ub = charData[i*2];
					var lb = charData[(i*2) + 1];

					byteData[i] = (byte) (((ub >= 97 ? (byte) (ub - 87) : (byte) (ub - 48)) << 4) |
					                      (lb >= 97 ? (byte) (lb - 87) : (byte) (lb - 48)));
				}
				return byteData;
			}
			catch (Exception e)
			{
				return e;
			}
		}
	}
}