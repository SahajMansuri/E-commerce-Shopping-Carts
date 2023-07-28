using E_Card.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_Card.Controllers
{

        // GET: Product
        public class ProductController : Controller
        {
            private readonly ProductContext _context;


            public ProductController(ProductContext context)
            {
                _context = context;
            }
            public ProductController() : this(new ProductContext())
            {
            }
            ProductContext db = new ProductContext();


            public ActionResult Index()
            {
                List<Product> productList = _context.Products.ToList();
                return View(productList);
            }

            public ActionResult Create()
            {
                return View();
            }


            public ActionResult Display(int id)
            {
                var Products = _context.Products.Find(id);
                if (Products != null)
                {
                    return File(Products.ImageData, Products.ProductImage);
                }
                return HttpNotFound();
            }

            [HttpPost]
            public ActionResult Create(Product product, HttpPostedFileBase imageFile)
            {
                var availablePro = _context.Products.FirstOrDefault(p => p.ProductName.Equals(product.ProductName, StringComparison.OrdinalIgnoreCase));
                if (availablePro != null)
                {
                    ModelState.AddModelError("ProductName", "Prodect Already Added");
                    return View(product);
                }
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    product.ProductImage = imageFile.ContentType;
                    using (var reader = new BinaryReader(imageFile.InputStream))
                    {
                        product.ImageData = reader.ReadBytes(imageFile.ContentLength);
                    }
                }
                else
                {
                    ModelState.AddModelError("ProductImage", "Image must be Select");
                    return View(product);
                }

                _context.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            public ActionResult Edit(int id)
            {
                var products = _context.Products.Find(id);
                if (products == null)
                {
                    return HttpNotFound();
                }

                return View(products);
            }

            [HttpPost]
            public ActionResult Edit(Product product, HttpPostedFileBase imageFile)
            {
                if (ModelState.IsValid)
                {
                    if (imageFile != null && imageFile.ContentLength > 0)
                    {
                        product.ProductImage = imageFile.ContentType;
                        using (var ImageRead = new BinaryReader(imageFile.InputStream))
                        {
                            product.ImageData = ImageRead.ReadBytes(imageFile.ContentLength);
                        }
                    }

                    _context.Entry(product).State = EntityState.Modified;
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(product);
            }

            public ActionResult Delete(int id)
            {
                var products = db.Products.Find(id);
                if (products == null)
                {
                    return HttpNotFound();
                }
                TempData["DeleteMsg"] = "<script>alert('Deleted !');</script>";
                db.Products.Remove(products);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            public ActionResult ProductList()
            {
                List<Product> productList = _context.Products.ToList();
                return View(productList);
            }



            public ActionResult AddToCart(int id)
            {
                var products = _context.Products.Find(id);
                if (products != null)
                {
                    List<Product> cartItems = Session["CartItems"] as List<Product> ?? new List<Product>();

                    var AddedImage = cartItems.FirstOrDefault(p => string.Equals(p.ProductName, products.ProductName, StringComparison.OrdinalIgnoreCase));

                    if (AddedImage == null)
                    {

                        cartItems.Add(products);
                        Session["CartItems"] = cartItems;
                    }

                }

                return RedirectToAction("ProductList");
            }


            public ActionResult Card()
            {
                List<Product> cartItems = Session["CartItems"] as List<Product> ?? new List<Product>();
                return View(cartItems);
            }
            public ActionResult DeleteFromCart(int id)
            {
                List<Product> cartItems = Session["CartItems"] as List<Product> ?? new List<Product>();
                var productToRemove = cartItems.FirstOrDefault(p => p.ProductId == id);

                if (productToRemove != null)
                {
                    cartItems.Remove(productToRemove);
                    Session["CartItems"] = cartItems;
                }

                return RedirectToAction("Card");
            }

        }

}