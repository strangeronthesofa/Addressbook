using Interfaces.Interfaces;
using Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace Interfaces
{
    internal class Program
    {
       static void Main(string[] args)
        {
            var builder = Host.CreateDefaultBuilder().ConfigureServices(services =>
            {
                string filePath = @"G:\ECWin23\C-Sharp\FilePaths\test.json";

                services.AddSingleton<IFileServices>(provider => new FileServices(filePath));

                services.AddSingleton<List<IPerson>>(provider => LoadListFromJson(provider.GetRequiredService<IFileServices>()));

                services.AddSingleton<PersonService>();

                services.AddTransient<MenuServices>();
            }).Build();

            using (var serviceScope = builder.Services.CreateScope())
            {
                var serviceProvider = serviceScope.ServiceProvider;

                var menuServices = serviceProvider.GetRequiredService<MenuServices>();
                menuServices.Menu();
            }

        }

        /// <summary>
        /// Loads a list of persons from JSON data using the provided FileServices.
        /// If the JSON data is not available, creates and returns a new empty list.
        /// </summary>
        /// <param name="fileServices">FileServices instance for loading JSON data</param>
        /// <returns>List of persons loaded from JSON or a new empty list</returns>
        static List<IPerson> LoadListFromJson(IFileServices fileServices)
        {
            return fileServices.LoadListFromJson() ?? new List<IPerson>();
        }
    }
}
