using System;
using System.Data.SQLite;
using System.IO;
using System.IO.Compression;

namespace Theodorus2.SqliteExtensions
{
	[SQLiteFunction(Arguments = 1, FuncType = FunctionType.Scalar, Name = "Compress")]
	public class Compress : SQLiteFunction
	{
		public override object Invoke(object[] args)
		{
			try
			{
				if (args[0] == DBNull.Value) return null;

				var byteData = args[0] as byte[];
				if (byteData == null) return new ArgumentException("Can't compress non byte[]");

				using (var ms = new MemoryStream())
				{
					var lenBuf = BitConverter.GetBytes(byteData.Length);
					ms.Write(lenBuf, 0, sizeof (int));
					using (var gs = new GZipStream(ms, CompressionMode.Compress, false))
					{
						gs.Write(byteData, 0, byteData.Length);
					}
					ms.Flush();
					return ms.ToArray();
				}
			}
			catch (Exception e)
			{
				return e;
			}
		}
	}
}