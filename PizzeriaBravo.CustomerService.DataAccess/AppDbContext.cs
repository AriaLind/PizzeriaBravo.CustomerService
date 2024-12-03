using Microsoft.EntityFrameworkCore;
using PizzeriaBravo.CustomerService.DataAccess.Entities;

namespace PizzeriaBravo.CustomerService.DataAccess;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers { get; set; }
}