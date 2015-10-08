using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ClaimsContext : DbContext
    {
        public ClaimsContext() : base("myCon")
        {
            Database.SetInitializer<ClaimsContext>(new ClaimsDBInitializer());

        }
        public DbSet<ClaimTask> ClaimsTasks { get; set; }
    }

    public class ClaimsDBInitializer : DropCreateDatabaseAlways<ClaimsContext>
    {
        protected override void Seed(ClaimsContext context)
        {
            context.ClaimsTasks.Add(new ClaimTask() { Name = "New Loss", CompletionDate = DateTime.Now, Owner = "Alex" });
            context.ClaimsTasks.Add(new ClaimTask() { Name = "Further Advice", CompletionDate = DateTime.Now.AddDays(12), Owner = "Alex" });
            context.ClaimsTasks.Add(new ClaimTask() { Name = "Adhoc task", CompletionDate = DateTime.Now.AddMonths(1), Owner = "Rachel" });
            context.ClaimsTasks.Add(new ClaimTask() { Name = "New Loss", CompletionDate = DateTime.Now.AddDays(23), Owner = "Rachel" });

            context.ClaimsTasks.Add(new ClaimTask() { Name = "New Loss", CompletionDate = DateTime.Now, Owner = "Alex" });
            context.ClaimsTasks.Add(new ClaimTask() { Name = "Further Advice", CompletionDate = DateTime.Now.AddDays(12), Owner = "Alex" });
            context.ClaimsTasks.Add(new ClaimTask() { Name = "Adhoc task", CompletionDate = DateTime.Now.AddMonths(1), Owner = "Rachel" });
            context.ClaimsTasks.Add(new ClaimTask() { Name = "New Loss", CompletionDate = DateTime.Now.AddDays(23), Owner = "Rachel" });

            base.Seed(context);
        }
    }
}
