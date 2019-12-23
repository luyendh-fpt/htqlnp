using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HTQLNP.Models
{

   // [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class MyDBContext : IdentityDbContext<Account>
    {
        public MyDBContext() : base("name=NewConnectionString")
        {

        }
        public DbSet<Student> Students { get; set; }

    }
}