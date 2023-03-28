using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NorthwindApp.Entities;

namespace NorthwindApp.Controllers
{
  public class CustomersController : Controller
  {
    private readonly NORTHWNDContext _context;

    public CustomersController(NORTHWNDContext context)
    {
      _context = context;
    }

    // GET: Customers
    public async Task<IActionResult> Index()
    {



      return _context.Customers != null ?
                  View(await _context.Customers.ToListAsync()) :
                  Problem("Entity set 'NORTHWNDContext.Customers'  is null.");
    }

    // GET: Customers/Details/5

    // http.get('api/1').then(response).catch(err).finaly()

    // try
    // var rsponse = await http.get('api/1');
    // catch
    // throw err
    //
    public async Task<IActionResult> Details(string id)
    {
      if (id == null || _context.Customers == null)
      {
        return NotFound();
      }

      try
      {
        var customer2 = _context.Customers
           .FirstOrDefaultAsync(m => m.CustomerId == id).GetAwaiter().GetResult();
      }
      catch (Exception ex)
      {

        throw;
      }


      var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.CustomerId == id);

      if (customer == null)
      {
        return NotFound();
      }

      return View(customer);
    }

    // GET: Customers/Create
    public IActionResult Create()
    {
      return View();
    }

    // POST: Customers/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CustomerId,CompanyName,ContactName,ContactTitle,Address,City,Region,PostalCode,Country,Phone,Fax")] Customer customer)
    {
      if (ModelState.IsValid)
      {
         await _context.AddAsync(customer);
         await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }
      return View(customer);
    }

    // GET: Customers/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
      if (id == null || _context.Customers == null)
      {
        return NotFound();
      }

      var customer = await _context.Customers.FindAsync(id);
      if (customer == null)
      {
        return NotFound();
      }
      return View(customer);
    }

    // POST: Customers/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("CustomerId,CompanyName,ContactName,ContactTitle,Address,City,Region,PostalCode,Country,Phone,Fax")] Customer customer)
    {
      if (id != customer.CustomerId)
      {
        return NotFound();
      }

      if (ModelState.IsValid)
      {
        try
        {
          _context.Update(customer);
          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!CustomerExists(customer.CustomerId))
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
      return View(customer);
    }

    // GET: Customers/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
      if (id == null || _context.Customers == null)
      {
        return NotFound();
      }

      // InMemory çalışırken async yapısına ihtiyaç.
      // async tak yapısını unmanagement yönetilemeyen kaynakların daha performaslı olarak kullanılması için tercih ediyoruz.
      // Amazon S3 file upload async olmalı
      // IO işlemlerini async tercih etmemliyiz.
      // HttpCLient ile Api ve MVC bağlantıları async task olmalır.
      //var clist = await _context.Customers.ToListAsync();

      var customer = await _context.Customers
          .FirstOrDefaultAsync(m => m.CustomerId == id);
      if (customer == null)
      {
        return NotFound();
      }

      return View(customer);
    }

    // POST: Customers/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
      if (_context.Customers == null)
      {
        return Problem("Entity set 'NORTHWNDContext.Customers'  is null.");
      }
      var customer = await _context.Customers.FindAsync(id);
      if (customer != null)
      {
        _context.Customers.Remove(customer);
      }

      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    private bool CustomerExists(string id)
    {
      return (_context.Customers?.Any(e => e.CustomerId == id)).GetValueOrDefault();
    }
  }
}
