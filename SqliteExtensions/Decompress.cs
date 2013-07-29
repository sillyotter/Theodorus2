using System;
using System.Data.SQLite;
using System.IO;
using System.IO.Compression;

namespace Theodorus2.SqliteExtensions
{
	[SQLiteFunction(Arguments = 1, FuncType = FunctionType.Scalar, Name = "Decompress")]
	public class Decompress : SQLiteFunction
	{
		public override object Invoke(object[] args)
		{
			try
			{
				if (args[0] == DBNull.Value) return null;

				var byteArray = args[0] as byte[];
				if (byteArray == null) return new ArgumentException("Can't decompress a non byte[]");
				
				using (var ms = new MemoryStream(byteArray))
				{
					var lenBuffer = new byte[sizeof (int)];
					ms.Read(lenBuffer, 0, sizeof (int));
					var len = BitConverter.ToInt32(lenBuffer, 0);
					using (var gs = new GZipStream(ms, CompressionMode.Decompress, true))
					{
						var decompBuf = new byte[len];
						gs.Read(decompBuf, 0, len); // TODO: really ought to make sure that it read the whole thing...
					    return decompBuf;
					}
				}
			}
			catch (Exception e)
			{
				return e;
			}
		}
	}
}