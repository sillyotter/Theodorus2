using System;
using System.Data.SQLite;

namespace Theodorus2.SqliteExtensions
{
	[SQLiteFunction(Arguments = 1, FuncType = FunctionType.Scalar, Name = "HexEncode")]
	public class HexEncode : SQLiteFunction
	{
		public override object Invoke(object[] args)
		{
			try
			{
				if (args[0] == DBNull.Value) return null;

				var rawData = args[0] as byte[];
				if (rawData == null) return new ArgumentException("Can't hexencode a non byte[]");

				var c = new char[rawData.Length*2];
				for (var i = 0; i < rawData.Length; ++i)
				{
					var b = ((byte) (rawData[i] >> 4));
					c[i*2] = (char) (b > 9 ? b + 0x57 : b + 0x30);
					b = ((byte) (rawData[i] & 0xF));
					c[i*2 + 1] = (char) (b > 9 ? b + 0x57 : b + 0x30);
				}

				return new string(c);
			}
			catch (Exception e)
			{
				return e;
			}
		}
	}
}