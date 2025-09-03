using BusinessSolutionChatGpt.Core.DTO.Product;
using BusinessSolutionChatGpt.Core.Interfaces;
using BusinessSolutionChatGpt.Core.Validators;
using BusinessSolutionChatGpt.Validators;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace BusinessSolutionChatGpt.Api.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartController : Controller
    {
        private readonly IShopCartManager shopCartManager;
        private readonly AddProductValidator addProductValidator;
        private readonly ProductExistValidator productExistValidator;

        public CartController(IShopCartManager shopCartManager, AddProductValidator addProductValidator, ProductExistValidator productExistValidator)
        {
            this.shopCartManager = shopCartManager;
            this.addProductValidator = addProductValidator;
            this.productExistValidator = productExistValidator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddProductDTO addProduct)
        {
            var result = addProductValidator.Validate(addProduct);
            if (result.IsValid)
            {
                shopCartManager.Add(addProduct);
                return Created();
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            return Ok(shopCartManager.GetAll());
        }

        [HttpGet]
        [Route("total")]
        public async Task<IActionResult> Total()
        {
            return Ok(new ProductCostSummaryDTO { Total = shopCartManager.GetTotalCost() });
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = productExistValidator.Validate(id);
            if (result.IsValid)
            {
                shopCartManager.Delete(id);
                return Ok(id);
            } else
            {
                return NotFound();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            shopCartManager.DeleteAll();
            return Ok();
        }
    }
}
