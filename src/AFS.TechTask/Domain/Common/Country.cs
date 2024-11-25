﻿namespace AFS.TechTask.Domain.Common
{
    /// <summary>
    /// Represents the country that a property is situated.
    /// </summary>
    public class Country
    {
        public const string UK = "United Kingdom";
        public const string ROI = "Republic of Ireland";

        /// <summary>
        /// The name of the country
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// The symbol used to represent the country's currency
        /// </summary>
        public char CurrencySymbol { get; init; }

        /// <summary>
        /// The 3-letter acronym used to represent the country's currency
        /// </summary>
        public string CurrencyAcronym { get; init; }

        public Country(string country)
        {
            this.Name = country;

            (this.CurrencySymbol, this.CurrencyAcronym) = country switch
            {
                UK => ('£', "GBP"),
                ROI => ('€', "EUR"),
                _ => throw new InvalidCountryException(country)
            };
        }
    }
}
