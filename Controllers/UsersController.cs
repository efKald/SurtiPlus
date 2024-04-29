using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using SurtiPlus.Models;

namespace SurtiPlus.Controllers
{
    public class UsersController : Controller
    {
        private readonly SurtiPlusContext _context;
        public UsersController(SurtiPlusContext context)
        {
            _context = context;
        }
        // GET: Users/Register
        public IActionResult Register()
        {
            return View();
        }
        // redirecttoaction can be used to call a controller that doesn't render a view. may be used for partialv iews.
        // POST: Users/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("UserId,UserPassword,UserName,Email,Phone")] Userv userv)
        {
            if (ModelState.IsValid) 
            {
                var checkId = await _context.User.FindAsync(userv.UserId);
                if (checkId == null)
                {
                    if (userv.UserId >= 0 && userv.UserPassword != null && userv.UserName != null && userv.Email != null && userv.Phone != null)
                    {
                        var firstRow = await _context.Admin.FirstOrDefaultAsync();
                        string? administration = null;
                        if (firstRow != null && firstRow.AdminNameId != null)
                        {
                            administration = firstRow.AdminNameId;
                        }
                        userv.AdminNameId = administration;
                        _context.Add(userv);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Welcome", "Home");
                    }
                    else 
                    {
                        TempData["Message"] = "Llene todos los campos y asegurese que el Id de usuario sea mayor o igual que cerof";

                        return (View(userv));
                    }
                }
                else 
                {
                    TempData["Message"] = "Este usuario ya está registrados";

                    return (View(userv));
                }
            }
            return View(userv);
        }
        // GET: Users/UserLogIn
        public IActionResult UserLogIn()
        {
            return View();
        }
        // POST: Users/UserLogIn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserLogIn(string UserName, string UserPassword) 
        {
            var validateUser = await _context.User.FirstOrDefaultAsync(u => u.UserName == UserName && u.UserPassword == UserPassword);
            if (validateUser != null)
            {
                return (RedirectToAction(nameof(Buy)));
            }
            else 
            {
                ModelState.AddModelError(string.Empty, "Nombre o contraseña incorrectas.");
                return View();
            }
        
        }
        // GET: Users/AdminLogIn
        public IActionResult AdminLogIn() 
        {
            return View();
        }
        // POST: Users/AdminLogIn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminLogIn(string AdminNameId, string AdminPassword)
        {
            var validateAdmin = await _context.Admin.FirstOrDefaultAsync(a => a.AdminNameId == AdminNameId && a.AdminPassword == AdminPassword);
            if (validateAdmin != null)
            {
                return (RedirectToAction(nameof(AdminHandle)));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Nombre o contraseña incorrectas.");
                return View();
            }

        }
        // GET: Users/Buy
        public async Task<IActionResult> Buy()
        {
            return _context.Product != null ?
            View(await _context.Product.ToListAsync()) :
            Problem("Entity set 'SurtiPlusContext.Product'  is null.");
        }
        // POST: Users/Buy
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Buy(BuyValue buyvalue)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = _context.Database.BeginTransaction()) // Start transaction. Database is a property to interact with the database.BeginTransaction() starts a new transaction on the database. The using statement ensures that the transaction object is properly disposed of when it goes out of scope. Used to perform multiple database operations. Ensures all transactions are preformed or none.
                {
                    try
                    {
                        var carnesRow = await _context.Product
                                                        .FirstOrDefaultAsync(m => m.ProductType == "Carnes");
                        var lacteosRow = await _context.Product
                                                        .FirstOrDefaultAsync(m => m.ProductType == "Lacteos");
                        var granosRow = await _context.Product
                                                        .FirstOrDefaultAsync(m => m.ProductType == "Granos");
                        var aseoRow = await _context.Product
                                                        .FirstOrDefaultAsync(m => m.ProductType == "Aseo");

                        if (carnesRow != null && carnesRow.Amount != null && carnesRow.Amount >0)
                        {
                            carnesRow.Amount -= buyvalue.Carnesvalue;
                        }

                        if (lacteosRow != null && lacteosRow.Amount != null && lacteosRow.Amount >0)
                        {
                            lacteosRow.Amount -= buyvalue.Lacteosvalue;
                        }

                        if (granosRow != null && granosRow.Amount != null && granosRow.Amount > 0)
                        {
                            granosRow.Amount -= buyvalue.Granosvalue;
                        }

                        if (aseoRow != null && aseoRow.Amount != null && aseoRow.Amount >0)
                        {
                            aseoRow.Amount -= buyvalue.Aseovalue;
                        }

                        if (carnesRow != null && lacteosRow != null && granosRow != null && aseoRow != null)
                        {
                            _context.UpdateRange(carnesRow, lacteosRow, granosRow, aseoRow); //To update multiple entities.
                        }

                        await _context.SaveChangesAsync(); // Save changes

                        transaction.Commit(); // Commit transaction if successful
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        transaction.Rollback(); // Rollback transaction on exception

                        // Handle concurrency conflict (e.g., reload data, inform user)
                        ModelState.AddModelError("", "Concurrency error occurred. Please refresh the page and try again.");
                        return View(buyvalue); // Re-render view with error message
                    }

                    TempData["Message"] = "¡Compra exitosa!";

                    return (RedirectToAction(nameof(Buy))); // Redirect to success page
                }


            }

            return View(buyvalue); // Return view with validation errors (if any)
        }
        // GET: Users/AdminHandle
        public async Task<IActionResult> AdminHandle()
        {
            
            {
                return _context.Product != null ?
                View(await _context.Product.ToListAsync()) :
                Problem("Entity set 'SurtiPlusContext.Product'  is null.");
            }
        }

        // POST: Users/AdminHandle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminHandle(UpdateValue updatevalue)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = _context.Database.BeginTransaction()) // Start transaction. Database is a property to interact with the database.BeginTransaction() starts a new transaction on the database. The using statement ensures that the transaction object is properly disposed of when it goes out of scope. Used to perform multiple database operations. Ensures all transactions are preformed or none.
                {
                    try
                    {
                        var carnesRow = await _context.Product
                                                     .FirstOrDefaultAsync(m => m.ProductType == "Carnes");
                        var lacteosRow = await _context.Product
                                                     .FirstOrDefaultAsync(m => m.ProductType == "Lacteos");
                        var granosRow = await _context.Product
                                                     .FirstOrDefaultAsync(m => m.ProductType == "Granos");
                        var aseoRow = await _context.Product
                                                     .FirstOrDefaultAsync(m => m.ProductType == "Aseo");

                        if (carnesRow != null && carnesRow.Amount != null)
                        {
                            carnesRow.Amount += updatevalue.Carnesvalue;
                        }

                        if (lacteosRow != null && lacteosRow.Amount != null)
                        {
                            lacteosRow.Amount += updatevalue.Lacteosvalue;
                        }

                        if (granosRow != null && granosRow.Amount != null)
                        {
                            granosRow.Amount += updatevalue.Granosvalue;
                        }

                        if (aseoRow != null && aseoRow.Amount != null)
                        {
                            aseoRow.Amount += updatevalue.Aseovalue;
                        }

                        if (carnesRow != null && lacteosRow != null && granosRow != null && aseoRow != null)
                        {
                            _context.UpdateRange(carnesRow, lacteosRow, granosRow, aseoRow); //To update multiple entities.
                        }

                        await _context.SaveChangesAsync(); // Save changes

                        transaction.Commit(); // Commit transaction if successful
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        transaction.Rollback(); // Rollback transaction on exception

                        // Handle concurrency conflict (e.g., reload data, inform user)
                        ModelState.AddModelError("", "Concurrency error occurred. Please refresh the page and try again.");
                        return View(updatevalue); // Re-render view with error message
                    }

                    TempData["Message"] = "¡Lotes añadidos exitosamente!";

                    return (RedirectToAction(nameof(AdminHandle))); // Redirect to success page
                }


            }

            return View(updatevalue); // Return view with validation errors (if any)
        }
    }
}
