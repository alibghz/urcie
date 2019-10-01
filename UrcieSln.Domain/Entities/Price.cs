using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace UrcieSln.Domain.Entities
{
	[TypeConverter(typeof(PriceTypeConverter))]
	[Serializable()]
	public class Price : ISerializable
	{
		public Price(SerializationInfo info, StreamingContext context)
		{
			Id = (Guid)info.GetValue("Id", typeof(Guid));
			Purchase = (float)info.GetValue("Purchase", typeof(float));
			Sale = (float)info.GetValue("Sale", typeof(float));
			Currency = (Currency)info.GetValue("Currency", typeof(Currency));
		}
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Id", Id);
			info.AddValue("Purchase", Purchase);
			info.AddValue("Sale", Sale);
			info.AddValue("Currency", Currency);
		}
		public Price()
		{
			Currency = Currency.USD;
		}

		public Guid Id { get; set; }
		public float Purchase { get; set; }
		public float Sale { get; set; }
		public virtual Currency Currency { get; set; }

		#region Operators
		#region Addition
		public static Price operator +(Price one, Price two)
		{
			if (one.Currency != two.Currency)
				ConvertCurrency(ref one, ref two);
			if (one.Currency != two.Currency)
				throw new CurrencyMismatch("Currency Mismatch exception occured. First Currency is \"" + one.Currency + "\" and Second is \"" + two.Currency + "\"");

			return new Price { Purchase = one.Purchase + two.Purchase, Sale = one.Sale + two.Sale, Currency = one.Currency };


		}

		public static Price operator +(Price one, float two)
		{
			return new Price { Purchase = one.Purchase + two, Sale = one.Sale + two, Currency = one.Currency };
		}
		#endregion

		#region Subtract
		public static Price operator -(Price one, Price two)
		{
			if (one.Currency != two.Currency)
				ConvertCurrency(ref one, ref two);
			if (one.Currency != two.Currency)
				throw new CurrencyMismatch("Currency Mismatch exception occured. First Currency is \"" + one.Currency + "\" and Second is \"" + two.Currency + "\"");

			return new Price { Purchase = one.Purchase - two.Purchase, Sale = one.Sale - two.Sale, Currency = one.Currency };
		}
		public static Price operator -(Price one, float two)
		{
			return new Price { Purchase = one.Purchase - two, Sale = one.Sale - two, Currency = one.Currency };
		}
		#endregion

		#region Multiply
		public static Price operator *(Price one, Price two)
		{
			if (one.Currency != two.Currency)
				ConvertCurrency(ref one, ref two);
			if (one.Currency != two.Currency)
				throw new CurrencyMismatch("Currency Mismatch exception occured. First Currency is \"" + one.Currency + "\" and Second is \"" + two.Currency + "\"");

			return new Price { Purchase = one.Purchase * two.Purchase, Sale = one.Sale * two.Sale, Currency = one.Currency };
		}
		public static Price operator *(Price one, float two)
		{
			return new Price { Purchase = one.Purchase * two, Sale = one.Sale * two, Currency = one.Currency };
		}
		#endregion

		#region Division
		public static Price operator /(Price one, Price two)
		{
			if (two.Purchase == 0 || two.Sale == 0)
				throw new DivideByZeroException();
			if (one.Currency != two.Currency)
				ConvertCurrency(ref one, ref two);
			if (one.Currency != two.Currency)
				throw new CurrencyMismatch("Currency Mismatch exception occured. First Currency is \"" + one.Currency + "\" and Second is \"" + two.Currency + "\"");

			return new Price { Purchase = one.Purchase / two.Purchase, Sale = one.Sale / two.Sale, Currency = one.Currency };
		}
		public static Price operator /(Price one, float two)
		{
			if (two == 0)
				throw new DivideByZeroException();
			return new Price { Purchase = one.Purchase / two, Sale = one.Sale / two, Currency = one.Currency };
		}
		public static Price operator /(float one, Price two)
		{
			if (two.Purchase == 0 || two.Sale == 0)
				throw new DivideByZeroException();
			return new Price { Purchase = one / two.Purchase, Sale = one / two.Sale, Currency = two.Currency };
		}
		#endregion

		private static void ConvertCurrency(ref Price one, ref Price two)
		{
			if (two.Purchase == 0 && two.Sale == 0)
				two.Currency = one.Currency;
			else if (one.Purchase == 0 && one.Sale == 0)
				one.Currency = two.Currency;
			//else
			//should be implemented
		}

		[global::System.Serializable]
		public class CurrencyMismatch : Exception
		{
			public CurrencyMismatch() { }
			public CurrencyMismatch(string message) : base(message) { }
			public CurrencyMismatch(string message, Exception inner) : base(message, inner) { }
			protected CurrencyMismatch(
			System.Runtime.Serialization.SerializationInfo info,
			System.Runtime.Serialization.StreamingContext context)
				: base(info, context) { }
		}
		#endregion

		public override bool Equals(object obj)
		{
			Price p = obj as Price;
			if (p == null) return false;
			if (p.Id != Id) return false;
			if (p.Sale != Sale) return false;
			if (p.Currency != Currency) return false;
			return true;
		}

		public override string ToString()
		{
			return Sale.ToString() + " " + Currency.ToString();
		}
	}
}
