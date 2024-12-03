using System.IO;
using System.Windows;
using Microsoft.Extensions.Configuration;

namespace University;

public partial class App : Application
{
    public App()
    {
        Startup += (_, _) =>
        {
            LoadConfig();
        };
    }

    private void LoadConfig()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        
        #if DEBUG
        var connectionString = config.GetConnectionString("DefaultConnection");//FIXME DefaultConnection -> TestConnection
        #elif RELEASE
        var connectionString = config.GetConnectionString("ProdConnection");
        #endif
        
        this.Resources["ConnectionString"] = connectionString;
    }
}