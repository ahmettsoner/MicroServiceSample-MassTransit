using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Product.Microservice.Features.ProductFeatures.Commands;
using Product.Microservice.Features.ProductFeatures.Queries;

namespace Product.Microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IMediator mediator;
        // protected IMediator Mediator => mediator ??= HttpContext.RequestServices.GetService(typeof(IMediator)) as IMediator;

        public ProductController(IMediator Mediator)
        {
            mediator = Mediator;
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateProductCommand command){
            return Ok(await mediator.Send(command));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(){
            var query = new GetAllProductsQuery();
            return Ok(await mediator.Send(query));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id){
            var query = new GetProductByIdQuery() { Id = id};
            return Ok(await mediator.Send(query));
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id){
            var command = new DeleteProductByIdCommand() { Id = id};
            return Ok(await mediator.Send(command));
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateProductCommand command){
            if(id != command.Id){
                return BadRequest();
            }
            return Ok(await mediator.Send(command));
        }
    }
}