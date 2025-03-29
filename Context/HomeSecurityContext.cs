using Microsoft.EntityFrameworkCore;
using Smart_Electric_Metering_System_BackEnd.Entities;
using Smart_Electric_Metering_System_BackEnd.Entities.Identity;
using System.Collections.Generic;

namespace Smart_Electric_Metering_System_BackEnd.Context;

public class SmartElectricMeteringContext : DbContext
{
    public SmartElectricMeteringContext(DbContextOptions<SmartElectricMeteringContext> optionsBuilder): base(optionsBuilder)
    {
    }
    public DbSet<Address> Address { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Meter> Meters { get; set; }
    public DbSet<MeterUnitAllocation> MeterUnitAllocations { get; set; }
    public DbSet<MeterUnits> MeterUnits { get; set; }
    public DbSet<Prices> Prices { get; set; }
    public DbSet<MeterPrompt> MeterPrompts { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
}
