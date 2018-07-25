/***
 * This algorithm will generate the COUPON CODE
 * Credit goes to https://github.com/rebeccapowell/csharp-algorithm-coupon-code
 * **/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CouponCode
{
    public class CouponCodeBuilder
    {
        // The symbols dictionary.
        private readonly Dictionary<char, int> symbolsDictionary = new Dictionary<char, int>();

        // The random number generator.
        private readonly RandomNumberGenerator randomNumberGenerator;

        // The symbols array.
        private char[] symbols;

        // Gets or Sets the Bad Word List
        public string[] BadWordsList { get; set; }

        // Initializes a new instance of the CouponCodeBuilder class.
        public CouponCodeBuilder()
        {
            this.BadWordsList = "SHPX PHAG JNAX JNAT CVFF PBPX FUVG GJNG GVGF SNEG URYY ZHSS QVPX XABO NEFR FUNT GBFF FYHG GHEQ FYNT PENC CBBC OHGG SRPX OBBO WVFZ WVMM CUNG'".Split(' ');
            this.SetupSymbolsDictionary();
            this.randomNumberGenerator = new SecureRandom();
        }

        // Primary Method will be responsible to generate the COUPON CODE
        public string Generate(Options opts)
        {
            var parts = new List<string>();

            // if  plaintext wasn't set then override
            if (string.IsNullOrEmpty(opts.Plaintext))
            {
                // not yet implemented
                opts.Plaintext = this.GetRandomPlaintext(8);
            }

            // generate parts and combine
            do
            {
                for (var i = 0; i < opts.Parts; i++)
                {
                    var sb = new StringBuilder();
                    for (var j = 0; j < opts.PartLength - 1; j++)
                    {
                        sb.Append(this.GetRandomSymbol());
                    }

                    var part = sb.ToString();
                    sb.Append(this.CheckDigitAlg(part, i + 1));
                    parts.Add(sb.ToString());
                }
            }
            while (this.ContainsBadWord(string.Join(string.Empty, parts.ToArray())));

            return string.Join("-", parts.ToArray());
        }

        // Method to Validate the Generated COUPON CODE
        public string Validate(string code, Options opts)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new Exception("Provide a code to be validated");
            }

            // uppercase the code, replace OIZS with 0125
            code = new string(Array.FindAll(code.ToCharArray(), char.IsLetterOrDigit))
                .ToUpper()
                .Replace("O", "0")
                .Replace("I", "1")
                .Replace("Z", "2")
                .Replace("S", "5");

            // split in the different parts
            var parts = new List<string>();
            var tmp = code;
            while (tmp.Length > 0)
            {
                parts.Add(tmp.Substring(0, opts.PartLength));
                tmp = tmp.Substring(opts.PartLength);
            }

            // make sure we have been given the same number of parts as we are expecting
            if (parts.Count != opts.Parts)
            {
                return string.Empty;
            }

            // validate each part
            for (var i = 0; i < parts.Count; i++)
            {
                var part = parts[i];

                // check this part has 4 chars
                if (part.Length != opts.PartLength)
                {
                    return string.Empty;
                }

                // split out the data and the check
                var data = part.Substring(0, opts.PartLength - 1);
                var check = part.Substring(opts.PartLength - 1, 1);

                if (Convert.ToChar(check) != this.CheckDigitAlg(data, i + 1))
                {
                    return string.Empty;
                }
            }

            // everything looked ok with this code
            return string.Join("-", parts.ToArray());
        }

        // Responsible to Get Random Plain Text
        private string GetRandomPlaintext(int maxSize)
        {
            var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            var data = new byte[1];
            this.randomNumberGenerator.GetNonZeroBytes(data);

            data = new byte[maxSize];
            this.randomNumberGenerator.GetNonZeroBytes(data);

            var result = new StringBuilder(maxSize);
            foreach (var b in data)
            {
                result.Append(chars[b % chars.Length]);
            }

            return result.ToString();
        }

        // To Get Random Symbol
        private char GetRandomSymbol()
        {
            var rng = new SecureRandom();
            var pos = rng.Next(this.symbols.Length);
            return this.symbols[pos];
        }

        // Alg to Check Random Digit 
        private char CheckDigitAlg(string data, int check)
        {
            // check's initial value is the part number (e.g. 3 or above)
            // loop through the data chars
            Array.ForEach(
                data.ToCharArray(),
                v =>
                {
                    var k = this.symbolsDictionary[v];
                    check = (check * 19) + k;
                });

            return this.symbols[check % 31];
        }

        // Ensure generated COUPON CODE shouldn't contain any Bad word
        private bool ContainsBadWord(string code)
        {
            return this.BadWordsList.Any(t => code.ToUpper().IndexOf(t, StringComparison.Ordinal) > -1);
        }

        // Setting up symbol dictionary
        private void SetupSymbolsDictionary()
        {
            const string AvailableSymbols = "0123456789ABCDEFGHJKLMNPQRTUVWXY";
            this.symbols = AvailableSymbols.ToCharArray();
            for (var i = 0; i < this.symbols.Length; i++)
            {
                this.symbolsDictionary.Add(this.symbols[i], i);
            }
        }
    }
}
