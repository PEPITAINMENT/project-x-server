using ComparingEngine;
using ComparingEngine.FuzzyComaprer;
using GameBussinesLogic.Repositories;
using GameBussinesLogic.Runner;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
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
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System;

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
            services.AddMvc();
            services.AddSignalR();
            services.AddControllers();
            services.AddSingleton<IGameRunner, GameRunner>(x => new GameRunner(songDelaySeconds));
            services.AddSingleton<IUserGamesService, UserGamesService>();
            services.AddSingleton<IGameRepository, GameRepository>();
            services.AddSingleton<IGameStatusService, GameStatusService>();
            services.AddSingleton<IHubNotificator, HubNotificator>();
            services.AddTransient<IFuzzyComparer, FuzzyComparer>(x => new FuzzyComparer(compareMatchPercent));
            services.AddTransient<ISongCompareEngine, SongCompareEngine>();
            AddAuth(services);
        }

        public void AddAuth(IServiceCollection services) {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Spotify", policy =>
                {
                    policy.AuthenticationSchemes.Add("Spotify");
                    policy.RequireAuthenticatedUser();
                });
            });
            services
              .AddAuthentication(options =>
              {
                  options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
              })
              .AddCookie(options =>
              {
                  options.ExpireTimeSpan = TimeSpan.FromMinutes(50);
              })
              .AddSpotify(options =>
              {
                  options.ClientId = Configuration["ClientId"];
                  options.ClientSecret = Configuration["ClientSecret"];
                  options.CallbackPath = "/callback";
                  options.SaveTokens = true;

                  var scopes = new List<string> {
                      "playlist-read-private", "playlist-modify-private", "user-library-read"
                  };
                  options.Scope.Add(string.Join(",", scopes));
                  options.Events = new OAuthEvents
                  {
                      OnCreatingTicket = GetUserCompanyInfoAsync
                  };
              });
        }

        private static async Task GetUserCompanyInfoAsync(OAuthCreatingTicketContext context)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

            var response = await context.Backchannel.SendAsync(request,
                HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);

            var user = JObject.Parse(await response.Content.ReadAsStringAsync());
            if (user.TryGetValue("company", out var value))
            {
                var company = user["company"].ToString();
                var companyIdentity = new ClaimsIdentity(new[]
                {
                    new Claim("Company", company)
                });
                context.Principal.AddIdentity(companyIdentity);
            }
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
