using Microsoft.Extensions.DependencyInjection;
using StructureMap;

namespace DiscountStore
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();

            var container = new Container();
            container.Configure(config =>
            {
                config.Scan(x =>
                {
                    x.AssemblyContainingType(typeof(Program));
                    x.WithDefaultConventions();
                });
                
                config.Populate(services);
            });
        }
    }
}
