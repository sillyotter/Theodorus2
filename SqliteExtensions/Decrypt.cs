using System;
using System.Data.SQLite;
using System.IO;
using System.Security.Cryptography;

namespace Theodorus2.SqliteExtensions
{
	[SQLiteFunction(Arguments = 2, FuncType = FunctionType.Scalar, Name = "Decrypt")]
	public class Decrypt : SQLiteFunction
	{
		public override object Invoke(object[] args)
		{
			try
			{
				if (args[0] == DBNull.Value) return null;

				var rawData = args[0] as byte[];
				if (rawData == null) return new ArgumentException("Cant decrypt non byte[] data");

				var keyData = args[1] as byte[];
				if (keyData == null) return new ArgumentException("Must provide a valid byte[] key");

				var pdb = new Rfc2898DeriveBytes(
					keyData,
					new byte[]
						{
							0x49, 0x76, 0x61, 0x6f, 0x20, 0x4d,
							0x65, 0x64, 0x76, 0x65, 0xa4, 0x65,
							0x76
						},
					1000);

				var key = pdb.GetBytes(32);
				var iv = pdb.GetBytes(16);
				var alg = Rijndael.Create();

				alg.Key = key;
				alg.IV = iv;

				using (var clearData = new MemoryStream())
				{
					using (var cs = new CryptoStream(clearData, alg.CreateDecryptor(), CryptoStreamMode.Write))
					{
						var encBuf = rawData;
						cs.Write(encBuf, 0, encBuf.Length);
					}
					clearData.Flush();
					return clearData.ToArray();
				}
			}
			catch (Exception e)
			{
				return e;
			}

		}
	}
}