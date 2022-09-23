using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PYP_MVC_QRcode.Abstraction.Services;
using PYP_MVC_QRcode.DAL;
using PYP_MVC_QRcode.Models;

namespace PYP_MVC_QRcode.Controllers;

public class QRCodeController : Controller
{ 
private readonly Context _context;
private readonly IQrCardCreate _qrCardCreate;
public QRCodeController(Context context, IQrCardCreate qrCardCreate)
{
    _context = context;
    _qrCardCreate = qrCardCreate;
}

// GET: VCards
public async Task<IActionResult> Index()
{
    return View(await _context.Cards.ToListAsync());
}

// GET: VCards/Details/5
public async Task<IActionResult> Details(int? id)
{
    if (id == null || _context.Cards == null)
    {
        return NotFound();
    }

    var Card = await _context.Cards.FirstOrDefaultAsync(c => c.Id == id);
    if (Card == null)
    {
        return NotFound();
    }

    return View(Card);
}

// GET: VCards/Create
public IActionResult Create()
{
    return View();
}

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create([Bind("Id,Firtname,Surname,Email,Phone,Country,City")] CardContact Card)
{
    if (ModelState.IsValid)
    {
        Card.Image = await _qrCardCreate.CreateQrCode(Card);
        _context.Add(Card);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    return View(Card);
}

// GET: VCards/Edit/5
public async Task<IActionResult> Edit(int? id)
{
    if (id == null || _context.Cards == null)
    {
        return NotFound();
    }

    var vCard = await _context.Cards.FindAsync(id);
    if (vCard == null)
    {
        return NotFound();
    }
    return View(vCard);
}

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(int id, [Bind("Id,Firtname,Surname,Email,Phone,Country,City")] CardContact vCard)
{
    if (id != vCard.Id)
    {
        return NotFound();
    }

    if (ModelState.IsValid)
    {
        try
        {
            vCard.Image = await _qrCardCreate.CreateQrCode(vCard);
            _context.Update(vCard);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!VCardExists(vCard.Id))
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
    return View(vCard);
}

// GET: VCards/Delete/5
public async Task<IActionResult> Delete(int? id)
{
    if (id == null || _context.Cards == null)
    {
        return NotFound();
    }

    var vCard = await _context.Cards
        .FirstOrDefaultAsync(m => m.Id == id);
    if (vCard == null)
    {
        return NotFound();
    }

    return View(vCard);
}

// POST: VCards/Delete/5
[HttpPost, ActionName("Delete")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DeleteConfirmed(int id)
{
    if (_context.Cards == null)
    {
        return Problem("Entity set 'VCardContext.VCards'  is null.");
    }
    var vCard = await _context.Cards.FindAsync(id);
    if (vCard != null)
    {
        _context.Cards.Remove(vCard);
    }

    await _context.SaveChangesAsync();
    return RedirectToAction(nameof(Index));
}

private bool VCardExists(int id)
{
    return _context.Cards.Any(e => e.Id == id);
}
}