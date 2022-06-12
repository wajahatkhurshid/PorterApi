using System;
using System.Globalization;


namespace Gyldendal.Porter.SolrMonitoring.Product.Models
{
    public class Money
    {
        /// <summary>Nominal value</summary>
        public readonly Decimal Value;
        /// <summary>Currency code (e.g. USD)</summary>
        public readonly string Currency;

        /// <summary>Money type, for use with Solr CurrencyField</summary>
        /// <param name="value">Nominal value</param>
        /// <param name="currency">Currency code (e.g. USD)</param>
        public Money(Decimal value, string currency)
        {
            this.Value = value;
            this.Currency = currency;
        }

        public override string ToString() => string.Format((IFormatProvider)CultureInfo.InvariantCulture, "{0}{1}{2}", (object)this.Value, string.IsNullOrEmpty(this.Currency) ? (object)"" : (object)",", (object)this.Currency);
    }
}
