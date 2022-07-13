using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hubtel.eCommerce.Cart.Api.Models;
using System.Diagnostics;

namespace Hubtel.eCommerce.Cart.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        private readonly CartContext _context;

        public CartItemsController(CartContext context)
        {
            _context = context;
        }

        // GET: api/CartItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartItems>>> GetCartItems()
        {
            return await _context.CartItems.ToListAsync();
        }

        // GET: api/CartItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CartItems>> GetCartItems(int id)
        {
            var cartItems = await _context.CartItems.FindAsync(id);

            if (cartItems == null)
            {
                return NotFound();
            }

            return cartItems;
        }

        // GET: api/CartItems/5
        [HttpGet]
        // public async Task<ActionResult<CartItems>> GetCartItems([FromQuery] CartItems cartItems)
        public async Task<ActionResult<CartItems>> GetCartItems(string searching)
        {


            if (!String.IsNullOrEmpty(searching))
            {
                var Items = await _context.CartItems.Where(s => s.ItemName.Contains(searching)).ToListAsync();

                return Ok(Items);
            }
            else
            {
                return NotFound();
            }

            



            // Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            // _logger.LogInfo($"Returned {Items.TotalCount} owners from database.");
        }


        



        // PUT: api/CartItems/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCartItems(int id, CartItems cartItems)
        {
            if (id != cartItems.Id)
            {
                return BadRequest();
            }

            _context.Entry(cartItems).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartItemsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CartItems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CartItems>> PostCartItems(CartItems cartItems)
        {

            if (_context.CartItems.Any(e => e.ItemID == cartItems.ItemID))
            {
                var Item = _context.CartItems.FirstOrDefault(u => u.ItemID == cartItems.ItemID);
                Debug.WriteLine("Item Data:  " + Item);

                cartItems.Quantity = cartItems.Quantity + Item.Quantity;
               // Debug.WriteLine("New Quantity:  " + NewQuantity);


               // var CartItem = new CartItems { ItemID = cartItems.ItemID, ItemName = cartItems.ItemName, Quantity = NewQuantity, UnitPrice = cartItems.UnitPrice };
                //Debug.WriteLine("New CartItems Data:  " + CartItem);

                _context.CartItems.Update(cartItems);
            }
            else
            {
                
                _context.CartItems.Add(cartItems);
            }
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCartItems", new { id = cartItems.Id }, cartItems);
        }

        // DELETE: api/CartItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CartItems>> DeleteCartItems(int id)
        {
            var cartItems = await _context.CartItems.FindAsync(id);
            if (cartItems == null)
            {
                return NotFound();
            }

            _context.CartItems.Remove(cartItems);
            await _context.SaveChangesAsync();

            return cartItems;
        }

        private bool CartItemsExists(int id)
        {
            return _context.CartItems.Any(e => e.Id == id);
        }
    }
}
