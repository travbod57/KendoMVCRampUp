using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaimsService
{
    public class Repository
    {
        private ClaimsContext context;

        public Repository()
        {
            context = new ClaimsContext();
        }

        public List<ClaimTask> GetTasks()
        {
            return context.ClaimsTasks.ToList();
        }
    }
}
