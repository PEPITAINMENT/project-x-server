using ComparingEngine;
using ComparingEngine.FuzzyComaprer;
using GameBussinesLogic.Repositories;
using GameBussinesLogic.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjextX.Hubs;
using ProjextX.Services;
using ProjextX.Services.Interfaces;
using Server.HubNotificator;
using System.Diagnostics.CodeAnalysis;

namespace ProjextX
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var compareMatchPercent = int.Parse(Configuration["CompareMatchPercent"]);
            var songDelaySeconds = int.Parse(Configuration["SongDelaySeconds"]);

            services.AddCors();
            services.AddSignalR();
            services.AddControllers();
            services.AddSingleton<IGameRunner, GameRunner>(x => new GameRunner(songDelaySeconds));
            services.AddSingleton<IUserGamesService, UserGamesService>();
            services.AddSingleton<IGameRepository, GameRepository>();
            services.AddSingleton<IGameStatusService, GameStatusService>();
            services.AddSingleton<IHubNotificator, HubNotificator>();
            services.AddTransient<IFuzzyComparer, FuzzyComparer>(x => new FuzzyComparer(compareMatchPercent));
            services.AddTransient<ISongCompareEngine, SongCompareEngine>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseCors(builder => 
                builder.AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials()
                       .SetIsOriginAllowed((host) => true));

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<GameSessionHub>("/game");
                endpoints.MapControllers();
            });
        }
    }
}
