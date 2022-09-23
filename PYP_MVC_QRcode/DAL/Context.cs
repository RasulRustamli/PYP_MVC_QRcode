using Microsoft.EntityFrameworkCore;
using PYP_MVC_QRcode.Models;

namespace PYP_MVC_QRcode.DAL;

public class Context:DbContext
{
    
        public Context(DbContextOptions<Context> options): base(options) { }

        public DbSet<CardContact> Cards { get; set; }
    
}
