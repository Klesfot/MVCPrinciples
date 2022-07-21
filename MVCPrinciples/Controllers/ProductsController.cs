using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCPrinciples.Data;
using MVCPrinciples.Models;

namespace MVCPrinciples.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductContext _productContext;
        private readonly CategoryContext _categoryContext;
        private readonly SupplierContext _supplierContext;

        public ProductsController(ProductContext productContext, CategoryContext categoryContext, SupplierContext supplierContext)
        {
            _productContext = productContext;
            _categoryContext = categoryContext;
            _supplierContext = supplierContext;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryContext.Categories.ToListAsync();
            ViewBag.Categories = categories
                .OrderBy(i => i.CategoryID)
                .ThenBy(i => i.CategoryName)
                .ToList();

            var suppliers = await _supplierContext.Suppliers.ToListAsync();
            ViewBag.Suppliers = suppliers
                .OrderBy(i => i.SupplierID)
                .ThenBy(i => i.CompanyName)
                .ToList();

            return _productContext.Products != null ? 
                          View(await _productContext.Products.ToListAsync()) :
                          Problem("Entity set 'ProductContext.Products'  is null.");
        }

        // GET: Products/Create
        public IActionResult Create()
        {
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
