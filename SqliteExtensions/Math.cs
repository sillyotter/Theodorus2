using System;
using System.Data.SQLite;

namespace Theodorus2.SqliteExtensions
{
	[SQLiteFunction(Arguments = 1, FuncType = FunctionType.Scalar, Name = "Acos")]
	public class Acos : SQLiteFunction
	{
		public override object Invoke(object[] args)
		{
			try
			{
				if (args[0].Equals(DBNull.Value)) return null;
				dynamic val = args[0];
				return Math.Acos((double)val);
			}
			catch (Exception e)
			{
				return e;
			}
		}
	}

	[SQLiteFunction(Arguments = 1, FuncType = FunctionType.Scalar, Name = "Asin")]
	public class Asin : SQLiteFunction
	{
		public override object Invoke(object[] args)
		{
			try
			{
				if (args[0].Equals(DBNull.Value)) return null;

				dynamic val = args[0];
				return Math.Asin((double)val);
			}
			catch (Exception e)
			{
				return e;
			}
		}
	}

	[SQLiteFunction(Arguments = 1, FuncType = FunctionType.Scalar, Name = "Atan")]
	public class Atan : SQLiteFunction
	{
		public override object Invoke(object[] args)
		{
			try
			{
				if (args[0].Equals(DBNull.Value)) return null;

				dynamic val = args[0];
				return Math.Atan((double)val);
			}
			catch (Exception e)
			{
				return e;
			}
		}
	}

	[SQLiteFunction(Arguments = 2, FuncType = FunctionType.Scalar, Name = "	Atan2")]
	public class Atan2 : SQLiteFunction
	{
		public override object Invoke(object[] args)
		{
			try
			{
				if (args[0].Equals(DBNull.Value)) return null;
				if (args[1].Equals(DBNull.Value)) return null;

				dynamic val = args[0];
				dynamic val2 = args[1];
				return Math.Atan2((double)val, (double)val2);
			}
			catch (Exception e)
			{
				return e;
			}
		}
	}
	[SQLiteFunction(Arguments = 1, FuncType = FunctionType.Scalar, Name = "Ceiling")]
	public class Ceiling : SQLiteFunction
	{
		public override object Invoke(object[] args)
		{
			try
			{
				if (args[0].Equals(DBNull.Value)) return null;

				dynamic val = args[0];
				return Math.Ceiling((double)val);
			}
			catch (Exception e)
			{
				return e;
			}
		}
	}

	[SQLiteFunction(Arguments = 1, FuncType = FunctionType.Scalar, Name = "Cos")]
	public class Cos : SQLiteFunction
	{
		public override object Invoke(object[] args)
		{
			try
			{
				if (args[0].Equals(DBNull.Value)) return null;

				dynamic val = args[0];
				return Math.Cos((double)val);
			}
			catch (Exception e)
			{
				return e;
			}
		}
	}


	[SQLiteFunction(Arguments = 1, FuncType = FunctionType.Scalar, Name = "Cosh")]
	public class Cosh : SQLiteFunction
	{
		public override object Invoke(object[] args)
		{
			try
			{
				if (args[0].Equals(DBNull.Value)) return null;

				dynamic val = args[0];
				return Math.Cosh((double)val);
			}
			catch (Exception e)
			{
				return e;
			}
		}
	}

	[SQLiteFunction(Arguments = 1, FuncType = FunctionType.Scalar, Name = "Exp")]
	public class Exp : SQLiteFunction
	{
		public override object Invoke(object[] args)
		{
			try
			{
				if (args[0].Equals(DBNull.Value)) return null;

				dynamic val = args[0];
				return Math.Exp((double)val);
			}
			catch (Exception e)
			{
				return e;
			}
		}
	}

	[SQLiteFunction(Arguments = 1, FuncType = FunctionType.Scalar, Name = "Floor")]
	public class Floor : SQLiteFunction
	{
		public override object Invoke(object[] args)
		{
			try
			{
				if (args[0].Equals(DBNull.Value)) return null;

				dynamic val = args[0];
				return Math.Floor((double)val);
			}
			catch (Exception e)
			{
				return e;
			}
		}
	}

	[SQLiteFunction(Arguments = 2, FuncType = FunctionType.Scalar, Name = "IEEERemainder")]
	public class IEEERemainder : SQLiteFunction
	{
		public override object Invoke(object[] args)
		{
			try
			{
				if (args[0].Equals(DBNull.Value)) return null;
				if (args[1].Equals(DBNull.Value)) return null;

				dynamic val = args[0];
				dynamic val2 = args[1];
				return Math.IEEERemainder((double)val, (double)val2);
			}
			catch (Exception e)
			{
				return e;
			}
		}
	}
	[SQLiteFunction(Arguments = 1, FuncType = FunctionType.Scalar, Name = "Log")]
	public class Log : SQLiteFunction
	{
		public override object Invoke(object[] args)
		{
			try
			{
				if (args[0].Equals(DBNull.Value)) return null;

				dynamic val = args[0];
				return Math.Log((double)val);
			}
			catch (Exception e)
			{
				return e;
			}
		}
	}

	[SQLiteFunction(Arguments = 2, FuncType = FunctionType.Scalar, Name = "LogBase")]
	public class LogBase : SQLiteFunction
	{
		public override object Invoke(object[] args)
		{
			try
			{
				if (args[0].Equals(DBNull.Value)) return null;

				dynamic val = args[0];
				dynamic val2 = args[1];
				return Math.Log((double)val, (double)val2);
			}
			catch (Exception e)
			{
				return e;
			}
		}
	}

	[SQLiteFunction(Arguments = 1, FuncType = FunctionType.Scalar, Name = "Log10")]
	public class Log10 : SQLiteFunction
	{
		public override object Invoke(object[] args)
		{
			try
			{
				if (args[0].Equals(DBNull.Value)) return null;

				dynamic val = args[0];
				return Math.Log10((double)val);
			}
			catch (Exception e)
			{
				return e;
			}
		}
	}

	[SQLiteFunction(Arguments = 2, FuncType = FunctionType.Scalar, Name = "Pow")]
	public class Pow : SQLiteFunction
	{
		public override object Invoke(object[] args)
		{
			try
			{
				if (args[0].Equals(DBNull.Value)) return null;
				if (args[1].Equals(DBNull.Value)) return null;

				dynamic val = args[0];
				dynamic val2 = args[1];

				return Math.Pow((double)val, (double)val2);
			}
			catch (Exception e)
			{
				return e;
			}
		}
	}

	[SQLiteFunction(Arguments = 1, FuncType = FunctionType.Scalar, Name = "Round")]
	public class Round : SQLiteFunction
	{
		public override object Invoke(object[] args)
		{
			try
			{
				if (args[0].Equals(DBNull.Value)) return null;

				dynamic val = args[0];
				return Math.Round((double)val);
			}
			catch (Exception e)
			{
				return e;
			}
		}
	}


	[SQLiteFunction(Arguments = 2, FuncType = FunctionType.Scalar, Name = "RoundTo")]
	public class RoundTo : SQLiteFunction
	{
		public override object Invoke(object[] args)
		{
			try
			{
				if (args[0].Equals(DBNull.Value)) return null;
				if (args[1].Equals(DBNull.Value)) return null;

				dynamic val = args[0];
				dynamic to = args[1];
				return Math.Round((double)val, (int)to);
			}
			catch (Exception e)
			{
				return e;
			}
		}
	}

	[SQLiteFunction(Arguments = 1, FuncType = FunctionType.Scalar, Name = "Sign")]
	public class Sign : SQLiteFunction
	{
		public override object Invoke(object[] args)
		{
			try
			{
				if (args[0].Equals(DBNull.Value)) return null;

				dynamic val = args[0];
				return Math.Sign((double)val);
			}
			catch (Exception e)
			{
				return e;
			}
		}
	}

	[SQLiteFunction(Arguments = 1, FuncType = FunctionType.Scalar, Name = "Sin")]
	public class Sin : SQLiteFunction
	{
		public override object Invoke(object[] args)
		{
			try
			{
				if (args[0].Equals(DBNull.Value)) return null;

				dynamic val = args[0];
				return Math.Sin((double)val);
			}
			catch (Exception e)
			{
				return e;
			}
		}
	}

	[SQLiteFunction(Arguments = 1, FuncType = FunctionType.Scalar, Name = "Sinh")]
	public class Sinh : SQLiteFunction
	{
		public override object Invoke(object[] args)
		{
			try
			{
				if (args[0].Equals(DBNull.Value)) return null;

				dynamic val = args[0];
				return Math.Sinh((double)val);
			}
			catch (Exception e)
			{
				return e;
			}
		}
	}

	[SQLiteFunction(Arguments = 1, FuncType = FunctionType.Scalar, Name = "Sqrt")]
	public class Sqrt : SQLiteFunction
	{
		public override object Invoke(object[] args)
		{
			try
			{
				if (args[0].Equals(DBNull.Value)) return null;

				dynamic val = args[0];
				return Math.Sqrt((double)val);
			}
			catch (Exception e)
			{
				return e;
			}
		}
	}

	[SQLiteFunction(Arguments = 1, FuncType = FunctionType.Scalar, Name = "Tan")]
	public class Tan : SQLiteFunction
	{
		public override object Invoke(object[] args)
		{
			try
			{
				if (args[0].Equals(DBNull.Value)) return null;

				dynamic val = args[0];
				return Math.Tan((double)val);
			}
			catch (Exception e)
			{
				return e;
			}
		}
	}

	[SQLiteFunction(Arguments = 1, FuncType = FunctionType.Scalar, Name = "Tanh")]
	public class Tanh : SQLiteFunction
	{
		public override object Invoke(object[] args)
		{
			try
			{
				if (args[0].Equals(DBNull.Value)) return null;

				dynamic val = args[0];
				return Math.Tanh((double)val);
			}
			catch (Exception e)
			{
				return e;
			}
		}
	}

	[SQLiteFunction(Arguments = 1, FuncType = FunctionType.Scalar, Name = "Truncate")]
	public class Truncate : SQLiteFunction
	{
		public override object Invoke(object[] args)
		{
			try
			{
				if (args[0].Equals(DBNull.Value)) return null;

				dynamic val = args[0];
				return Math.Truncate((double)val);
			}
			catch (Exception e)
			{
				return e;
			}
		}
	}
}