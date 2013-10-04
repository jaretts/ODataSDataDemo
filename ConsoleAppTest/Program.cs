using CodeFirstModel;
using DemoModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new SalesModelContext())
            {
                /*
                var customer = new Customer { 
                    Name = "Jarett",
                    Id = Guid.NewGuid(),
                    Tenant_Id = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now
                };

                context.Customers.Add(customer);
                context.SaveChanges();
                */
                var customers = context.Customers.ToList();

                foreach (var acustomer in customers)
                {
                    Console.WriteLine(acustomer.Name);
                }

                Console.ReadLine();
            }
        }
    }
}
