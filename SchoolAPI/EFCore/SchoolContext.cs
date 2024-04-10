using SchoolAPI.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SchoolAPI.EFCore
{
    public class SchoolContext : DbContext
    {
        public virtual DbSet<School> Schools { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Class> Classes { get; set; }
    }

}
