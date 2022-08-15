using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MVCPrinciples.Data;
using MVCPrinciples.Models;
using Options = MVCPrinciples.Data.Options;

namespace MVCPrinciples.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductContext _productContext;
        private readonly CategoryContext _categoryContext;
        private readonly SupplierContext _supplierContext;
        private readonly IOptions<Options> _options;

        public ProductsController(
            ProductContext productContext,
            CategoryContext categoryContext,
            SupplierContext supplierContext,
            IOptions<Options> options)
        {
            _productContext = productContext;
            _categoryContext = categoryContext;
            _supplierContext = supplierContext;
            _options = options;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryContext.Categories.ToListAsync();
            ViewBag.Categories = categories
                .OrderBy(i => i.CategoryID)
                .ToList();

            var suppliers = await _supplierContext.Suppliers.ToListAsync();
            ViewBag.Suppliers = suppliers
                .OrderBy(i => i.SupplierID)
                .ToList();

            var products = await _productContext.Products.ToListAsync();

            if (_options.Value.ProductsDisplayQuantity != 0)
            {
                products = products.GetRange(0, 2);
            }

            return _productContext.Products != null ? 
                          View(products) :
                          Problem("Entity set 'ProductContext.Products'  is null.");
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            var categories = _categoryContext.Categories.ToList();
            categories = categories
                .OrderBy(i => i.CategoryID)
                .ToList();

            var suppliers = _supplierContext.Suppliers.ToList();
            suppliers = suppliers
                .OrderBy(i => i.SupplierID)
                .ToList();

            var selectListCategories = new List<SelectListItem> { };
            foreach (var c in categories)
            {
                selectListCategories.Add(new SelectListItem(c.CategoryName, c.CategoryID.ToString()));
            }

            var selectListSuppliers = new List<SelectListItem> { };
            foreach (var s in suppliers)
            {
                selectListSuppliers.Add(new SelectListItem(s.CompanyName, s.SupplierID.ToString()));
            }

            ViewData["SelectListCategories"] = selectListCategories;
            ViewData["SelectListSuppliers"] = selectListSuppliers;

            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,ProductName,SupplierId,CategoryId,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued")] Product product)
        {
            if (ModelState.IsValid)
            {
                _productContext.Add(product);
                await _productContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var categories = _categoryContext.Categories.ToList();
            categories = categories
                .OrderBy(i => i.CategoryID)
                .ToList();

            var suppliers = _supplierContext.Suppliers.ToList();
            suppliers = suppliers
                .OrderBy(i => i.SupplierID)
                .ToList();

            var selectListCategories = new List<SelectListItem> { };
            foreach (var c in categories)
            {
                selectListCategories.Add(new SelectListItem(c.CategoryName, c.CategoryID.ToString()));
            }

            var selectListSuppliers = new List<SelectListItem> { };
            foreach (var s in suppliers)
            {
                selectListSuppliers.Add(new SelectListItem(s.CompanyName, s.SupplierID.ToString()));
            }

            ViewData["SelectListCategories"] = selectListCategories;
            ViewData["SelectListSuppliers"] = selectListSuppliers;

            if (id == null || _productContext.Products == null)
            {
                return NotFound();
            }

            var products = await _productContext.Products.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }
            return View(products);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,ProductName,SupplierId,CategoryId,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued")] Product product)
        {
            if (id != product.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _productContext.Update(product);
                    await _productContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(product.ProductID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
        
        private bool ProductsExists(int id)
        {
          return (_productContext.Products?.Any(e => e.ProductID == id)).GetValueOrDefault();
        }
    }
}
