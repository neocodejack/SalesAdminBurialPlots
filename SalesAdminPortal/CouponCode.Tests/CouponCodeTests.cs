using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CouponCode.Tests
{
    [TestClass]
    public class CouponCodeTests
    {
        [TestMethod]
        public void GenerateCoupon()
        {
            var opt = new Options
            {
                Parts = 2,
                PartLength = 4
            };

            var ccb = new CouponCodeBuilder();
            var coupon = ccb.Generate(opt);

            Assert.IsNotNull(coupon);
            Assert.AreEqual(coupon.Split('-').Length, 2);
            var splitCoupon = coupon.Split('-');
            Assert.AreEqual(splitCoupon[0].Length, 4);
            Assert.AreEqual(splitCoupon[1].Length, 4);
        }

        [TestMethod]
        public void ValidateCoupon()
        {
            var opt = new Options
            {
                Parts = 2,
                PartLength = 4
            };

            string couponCode = "KM5H-PDN1";
            var ccb = new CouponCodeBuilder();
            string verifiedCoupon = ccb.Validate(couponCode,opt);

            Assert.AreEqual(couponCode, verifiedCoupon);
        }
    }
}
