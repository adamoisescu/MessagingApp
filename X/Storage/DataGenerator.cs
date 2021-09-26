using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace X.Storage
{
    public class DataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MessagingDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<MessagingDbContext>>()))
            {
                if (context.Users.Any())
                {
                    return;   // Data was already seeded
                }

                context.Users.AddRange(
                    new Models.User
                    {
                        Id = new Guid("56fbb55f-2895-4dbd-a6a3-c78155240470"),
                        Name = "John Stewart"
                    },
                    new Models.User
                    {
                        Id = new Guid("83af8ec7-1cc3-4147-9d97-4a5364f396a6"),
                        Name = "Adeline Stew"
                    },
                    new Models.User
                    {
                        Id = new Guid("5c4b76a0-7c93-413a-a332-8339b0b87fd5"),
                        Name = "Madd Art"
                    },
                   new Models.User
                   {
                       Id = new Guid("99be865a-5a90-4ebe-8397-44add088b40a"),
                       Name = "Oct Wart"
                   },
                   new Models.User
                   {
                       Id = new Guid("85490809-0b0e-409b-9337-e39d8697c893"),
                       Name = "Essa Louse"
                   });

                context.SaveChanges();
            }
        }
    }
}
