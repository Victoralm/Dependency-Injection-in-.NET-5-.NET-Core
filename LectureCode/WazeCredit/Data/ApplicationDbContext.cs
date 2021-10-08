using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WazeCredit.Models;

namespace WazeCredit.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<CreditApplication> CreditApplicationModel {  get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
