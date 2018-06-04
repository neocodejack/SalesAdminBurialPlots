using CouponCode;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SalesAdminPortal.Controllers
{
    public class CouponController : Controller
    {
        private readonly Options _opts = null;
        private CouponCodeBuilder _ccb = null;
        //Setup Coupon Generator Options
        public CouponController()
        {
            _opts = new Options
            {
                Parts = 2,
                PartLength = 4
            };

            //Initiating Coupon Code Builder
            _ccb = new CouponCodeBuilder();
        }

        
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetCouponAsync()
        {
            var tcs = new TaskCompletionSource<string>();
            await Task.Run(() =>
             {
                 string couponCode = _ccb.Generate(_opts);
                 tcs.SetResult(couponCode);
             });

            return Json(tcs.Task, JsonRequestBehavior.AllowGet);
        }
        
        [Route("api/coupon/{coupon}")]
        [HttpGet]
        public async Task<ActionResult> ValidateCouponAsync(string coupon)
        {
            var tcs = new TaskCompletionSource<bool>();
            await Task.Run(() =>
            {
                bool isValidCode = false;
                if(_ccb.Validate(coupon,_opts) == coupon)
                {
                    isValidCode = true;
                }
                else
                {
                    isValidCode = false;
                }

                tcs.SetResult(isValidCode);
            });

            return Json(tcs.Task, JsonRequestBehavior.AllowGet);
        }
    }
}
