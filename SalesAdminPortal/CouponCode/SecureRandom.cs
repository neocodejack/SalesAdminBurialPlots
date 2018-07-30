using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CouponCode
{
    public class SecureRandom : RandomNumberGenerator
    {
        // The random number generator.
        private readonly RandomNumberGenerator rng = new RNGCryptoServiceProvider();

        // The Next First Overload
        public int Next()
        {
            var data = new byte[sizeof(int)];
            this.rng.GetBytes(data);
            return BitConverter.ToInt32(data, 0) & (int.MaxValue - 1);
        }

        // The Next Second Overload
        public int Next(int maxValue)
        {
            return this.Next(0, maxValue);
        }

        // The Next Third Overload
        public int Next(int minValue, int maxValue)
        {
            if (minValue > maxValue)
            {
                throw new ArgumentOutOfRangeException("minValue", minValue, "minValue cannot be greater than maxValue");
            }

            return (int)Math.Floor((minValue + ((double)maxValue) - minValue) * this.NextDouble());
        }

        // Double Return of Next Function
        public double NextDouble()
        {
            var data = new byte[sizeof(uint)];
            this.rng.GetBytes(data);
            var randUint = BitConverter.ToUInt32(data, 0);
            return randUint / (uint.MaxValue + 1.0);
        }

        // Overriding GetBytes
        public override void GetBytes(byte[] data)
        {
            this.rng.GetBytes(data);
        }

        // Overriding NonZero GetBytes
        public override void GetNonZeroBytes(byte[] data)
        {
            this.rng.GetNonZeroBytes(data);
        }
    }
}
