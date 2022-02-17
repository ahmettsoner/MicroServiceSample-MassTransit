using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using System.Data.SqlClient;
using Dapper;

namespace Customer.Microservice.Features.CustomerFeatures.Commands
{
    public class CreateCustomerCommand : IRequest
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }

        public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand>{
            private readonly ConnectionConfiguration connectionConfiguration;

            public CreateCustomerCommandHandler(ConnectionConfiguration connectionConfiguration)
            {
                this.connectionConfiguration = connectionConfiguration;
            }

            public async Task<Unit> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
            {
                var customer = new Models.Customer();
                customer.Name = request.Name;
                customer.Address = request.Address;
                customer.Telephone = request.Telephone;
                customer.Email = request.Email;

                using (SqlConnection con = new SqlConnection(this.connectionConfiguration.ConnectionString))
                {
                    await con.OpenAsync();
                    var sqlQuery = $"Insert Into Customer (Name, Address, Telephone, Email) Values (@Name, @Address, @Telephone, @Email)";
                    await con.ExecuteAsync(sqlQuery, customer);
                }

                return new Unit();
            }

        }
    }
}