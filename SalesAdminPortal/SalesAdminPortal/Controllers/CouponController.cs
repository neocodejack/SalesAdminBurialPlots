using CouponCode;
using System.Threading.Tasks;
using System.Web.Http;

namespace SalesAdminPortal.Controllers
{
    public class CouponController : ApiController
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

        [Route("api/coupon/")]
        [HttpGet]
        //[Authorize]
        public async Task<IHttpActionResult> GetCouponAsync()
        {
            var tcs = new TaskCompletionSource<string>();
            await Task.Run(() =>
             {
                 string couponCode = _ccb.Generate(_opts);
                 tcs.SetResult(couponCode);
             });

            return Ok(tcs.Task);
        }
        
        [Route("api/coupon/{coupon}")]
        [HttpGet]
        public async Task<IHttpActionResult> ValidateCouponAsync(string coupon)
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

            return Ok(tcs.Task);
        }
    }
}
