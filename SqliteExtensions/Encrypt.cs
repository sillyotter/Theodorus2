using System;
using System.Data.SQLite;
using System.IO;
using System.Security.Cryptography;

namespace Theodorus2.SqliteExtensions
{
	[SQLiteFunction(Arguments = 2, FuncType = FunctionType.Scalar, Name = "Encrypt")]
	public class Encrypt : SQLiteFunction
	{
		public override object Invoke(object[] args)
		{
			try
			{
				if (args[0] == DBNull.Value) return null;

				var byteData = args[0] as byte[];
				if (byteData == null) return new ArgumentException("Can only encrypt byte[] data");

				var keyData = args[1] as byte[];
				if (keyData == null) return new ArgumentException("Must provide a byte[] key");

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

				using (var encData = new MemoryStream())
				{
					using (var cs = new CryptoStream(encData, alg.CreateEncryptor(), CryptoStreamMode.Write))
					{
						cs.Write(byteData, 0, byteData.Length);
					}
					encData.Flush();
					return encData.ToArray();
				}
			}
			catch (Exception e)
			{
				return e;
			}
		}
	}
}