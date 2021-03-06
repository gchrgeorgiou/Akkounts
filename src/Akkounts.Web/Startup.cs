using Akka.Actor;
using Akka.DI.Core;
using Akka.DI.Ninject;
using Akkounts.DataAccess;
using Akkounts.Domain.Abstract;
using Akkounts.Web.Actors;
using Akkounts.Web.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ninject;

namespace Akkounts.Web
{
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
            services.AddControllers();
            services.AddSignalR();
            services.AddSingleton(provider =>
            {
                var container = new StandardKernel();
                container.Bind<TransactionRepository>().To<TransactionRepositoryImpl>();

                var hubContext = provider.GetService<IHubContext<NotificationHub>>();
                container.Bind<IHubContext<NotificationHub>>().ToConstant(hubContext);

                var actorSystem = ActorSystem.Create("AkkountsSystem");
                var resolver = new NinjectDependencyResolver(container, actorSystem);

                return actorSystem;
                //return ;
            });

            services.AddSingleton<AccountsActorProvider>(provider =>
            {
                var actorSystem = provider.GetService<ActorSystem>();
                //var hubContext = provider.GetService<IHubContext<NotificationHub>>();
                //var logger = provider.GetService<ILogger<AccountController>>(); 

                var props = actorSystem.DI().Props<ConsistentAccountPool>();
                var theActor = actorSystem.ActorOf(props, "MainPoolActor");

                return () => theActor;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseStaticFiles();
            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotificationHub>("/Hubs/notificationHub");
            });

            lifetime.ApplicationStarted.Register(() =>
                app.ApplicationServices.GetService<ActorSystem>());

            lifetime.ApplicationStopping.Register(() =>
                app.ApplicationServices.GetService<ActorSystem>().Terminate().Wait());
        }
    }
}