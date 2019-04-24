using System.Data.Entity;

namespace FindCustomObjects
{
    public class MyDbContext : DbContext
    {
        public MyDbContext() : base("EPiServerDB")
        {

        }

       
    }
}