using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;

namespace CovScan
{
    [DelimitedRecord(";")]
    [IgnoreEmptyLines()]
    [IgnoreFirst()]

    public class Gouv
    {
        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        [FieldNullValue(typeof(string), "0")]
        public string code_departement { get; set; }

        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        [FieldNullValue(typeof(string), "0")]
        public string nom_departement { get; set; }

        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        [FieldNullValue(typeof(string), "0")]
        public string code_region { get; set; }

        [FieldQuoted('"', QuoteMode.OptionalForBoth)]
        [FieldNullValue(typeof(string), "0")]
        public string nom_region { get; set; }
    }
}