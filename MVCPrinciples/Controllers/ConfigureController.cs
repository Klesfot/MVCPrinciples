using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Options = MVCPrinciples.Data.Options;

namespace MVCPrinciples.Controllers
{
    public class ConfigureController : Controller
    {
        private IOptions<Options> _options;

        public ConfigureController(IOptions<Options> options)
        {
            _options = options;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Save(string productsQuantity, string dbString)
        {
            _options.Value.ProductsDisplayQuantity = Convert.ToInt32(productsQuantity);
            _options.Value.DbConnectionString = dbString;
            return LocalRedirect("/");
        }
    }
}
