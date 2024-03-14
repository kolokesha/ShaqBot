using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace ShaqBot;

public class Startup
{
    
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public void ConfigureServices(IServiceCollection services)
    {

        
        services.AddControllers();
        services.AddDbContext<DataContext>(opt => opt.UseSqlServer(_configuration.GetConnectionString("Db")));
        services.AddSingleton<TelegramBot>();
        services.AddSingleton<ReceiverOptions>();
        services.AddSingleton<CurrentQuestion>();
        services.AddControllers().AddNewtonsoftJson();

    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}