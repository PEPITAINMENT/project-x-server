using GameBussinesLogic.Repositories;
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
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Server;
using GameBussinesLogic.Models;
using SongsProvider.Spotify;
using Server.OAuthService;
using SongsProvider.Spotify.Interfaces;
using GameBussinesLogic.IServices;
using GameBussinesLogic.Services;
using GameBussinesLogic.Comparer.FuzzyComaprer;
using GameBussinesLogic.Comparer;

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
            var oAuth = new OAuthService(
                   "c004920e4d3b45f183a9bdf18fd3517b",
                   "6e391f8122fe456a83189a7f8786d544",
                   "https://accounts.spotify.com/api/token");
            var compareMatchPercent = int.Parse(Configuration["CompareMatchPercent"]);
            var songDelaySeconds = int.Parse(Configuration["SongDelaySeconds"]);
            var resultsDelaySeconds = int.Parse(Configuration["ResultsDelaySeconds"]);
            AddAuth(services);

            services.AddCors();
            services.AddMvc();
            services.AddSignalR();
            services.AddControllers();
            services.AddTransient<ISpotifyApiService, SpotifyApiService>(x => new SpotifyApiService(oAuth.GetToken()));
            services.AddTransient<ISongProvider, SpotifySongProvider>();
            services.AddSingleton<IUserGamesService, UserGamesService>();

            services.AddSingleton<IRoomRepository, RoomRepository>();
            services.AddTransient<IRoomService, RoomService>();

            services.AddSingleton<IGameStatusService, GameStatusService>();


            services.AddTransient<IHubNotificator, HubNotificator>();
            services.AddTransient<IFuzzyComparer, FuzzyComparer>(x => new FuzzyComparer(compareMatchPercent));
            services.AddTransient<ISongCompareEngine, SongCompareEngine>();
            services.AddTransient<IGuessService, GuessService>();
        }

        public void AddAuth(IServiceCollection services) {
            services.AddAuthorization();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = Configuration.GetSection("JWT:Issuer").Value,
                            ValidateAudience = true,
                            ValidAudience = Configuration.GetSection("JWT:Audience").Value,
                            ValidateLifetime = true,
                            IssuerSigningKey = 
                                AuthOptions.GetSymmetricSecurityKey(
                                    Configuration.GetSection("JWT:Secret").Value),
                            ValidateIssuerSigningKey = true,
                        };
                        options.Events = new JwtBearerEvents
                        {
                            OnMessageReceived = context =>
                            {
                                var accessToken = context.Request.Query["access_token"];

                                var path = context.HttpContext.Request.Path;
                                if (!string.IsNullOrEmpty(accessToken) &&
                                    (path.StartsWithSegments("/game")))
                                {
                                    context.Token = accessToken;
                                }
                                return Task.CompletedTask;
                            }
                        };
                    });
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<GameSessionHub>("/game");
                endpoints.MapControllers();
            });
        }
    }
}
