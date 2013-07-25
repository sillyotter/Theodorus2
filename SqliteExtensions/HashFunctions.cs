using System;
using System.Data.SQLite;
using System.Security.Cryptography;

namespace Theodorus2.SqliteExtensions
{
	public static class HashHelper
	{
		public static object DoHash(object[] args, HashAlgorithm hasher)
		{
			if (args[0] == DBNull.Value) return null;

			var byteData = args[0] as byte[];
			if (byteData == null) return new ArgumentException("Cant Hash a non byte array");

			return hasher.ComputeHash(byteData);
		}
	}


	[SQLiteFunction(Arguments = 1, FuncType = FunctionType.Scalar, Name = "CRC32")]
	public class CRC32 : SQLiteFunction
	{
		public override object Invoke(object[] args)
		{
			try
			{
				return HashHelper.DoHash(args, new Crc32());
			}
			catch (Exception e)
			{
				return e;
			}
		}
	}

	[SQLiteFunction(Arguments = 1, FuncType = FunctionType.Scalar, Name = "MD5")]
	public class MD5 : SQLiteFunction
	{
		public override object Invoke(object[] args)
		{
			try
			{
				return HashHelper.DoHash(args, new MD5CryptoServiceProvider());
			}
			catch (Exception e)
			{
				return e;
			}
		}
	}

	[SQLiteFunction(Arguments = 1, FuncType = FunctionType.Scalar, Name = "RIPEMD160")]
	public class RIPEMD160 : SQLiteFunction
	{
		public override object Invoke(object[] args)
		{
			try
			{
				return HashHelper.DoHash(args, new RIPEMD160Managed());
			}
			catch (Exception e)
			{
				return e;
			}
		}
	}

	[SQLiteFunction(Arguments = 1, FuncType = FunctionType.Scalar, Name = "SHA1")]
	public class SHA1 : SQLiteFunction
	{
		public override object Invoke(object[] args)
		{
			try
			{
				return HashHelper.DoHash(args, new SHA1CryptoServiceProvider());
			}
			catch (Exception e)
			{
				return e;
			}
		}
	}

	[SQLiteFunction(Arguments = 1, FuncType = FunctionType.Scalar, Name = "SHA256")]
	public class SHA256 : SQLiteFunction
	{
		public override object Invoke(object[] args)
		{
			try
			{
				return HashHelper.DoHash(args, new SHA256CryptoServiceProvider());
			}
			catch (Exception e)
			{
				return e;
			}
		}
	}

	[SQLiteFunction(Arguments = 1, FuncType = FunctionType.Scalar, Name = "SHA384")]
	public class SHA384 : SQLiteFunction
	{
		public override object Invoke(object[] args)
		{
			try
			{
				return HashHelper.DoHash(args, new SHA384CryptoServiceProvider());
			}
			catch (Exception e)
			{
				return e;
			}
		}
	}


	[SQLiteFunction(Arguments = 1, FuncType = FunctionType.Scalar, Name = "SHA512")]
	public class SHA512 : SQLiteFunction
	{
		public override object Invoke(object[] args)
		{
			try
			{
				return HashHelper.DoHash(args, new SHA512CryptoServiceProvider());
			}
			catch (Exception e)
			{
				return e;
			}
		}
	}

}