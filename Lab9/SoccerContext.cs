using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Runtime.Remoting.Contexts;
//using Microsoft.EntityFrameworkCore;

namespace Lab9
{
    public class SoccerContext : DbContext
    {

        public SoccerContext() : base("SoccerDB2")
        { }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
    }
}
