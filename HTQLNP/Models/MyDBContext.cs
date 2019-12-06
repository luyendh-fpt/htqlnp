using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HTQLNP.Models
{

    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class MyDBContext : DbContext
    {
        public MyDBContext() : base("name=MyContext")
        {

        }
        public DbSet<Student> Students { get; set; }
    }
}