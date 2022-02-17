using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Customer.Microservice.Features.CustomerFeatures.Commands;
using Customer.Microservice.Features.CustomerFeatures.Queries;
using Dapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Customer.Microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase 
    {
        private readonly IMediator mediator;
        private readonly IConfiguration configuration;
        private string connectionString;

        public CustomerController(IMediator Mediator, IConfiguration configuration)
        {
            this.mediator = Mediator;
            this.configuration = configuration;
            this.connectionString = this.configuration.GetConnectionString("DefaultConnectionString");
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateCustomerCommand command){
            return Ok(await mediator.Send(command));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(){
            var query = new GetAllCustomersQuery();
            return Ok(await mediator.Send(query));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id){
            var query = new GetCustomerByIdQuery() { Id = id};
            return Ok(await mediator.Send(query));
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id){
            var command = new DeleteCustomerByIdCommand() { Id = id};
            return Ok(await mediator.Send(command));
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateCustomerCommand command){
            if(id != command.Id){
                return BadRequest();
            }
            return Ok(await mediator.Send(command));
        }
    }
}