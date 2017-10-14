using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SearchEngineService.Publishers;

namespace SearchEngineService
{
    public class Startup
    {
        SearchRequestPublisher searchRequestService;
		SearchRequestPublisher searchRequestServiceAsync;


		public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            searchRequestService = new SearchRequestPublisher();
            searchRequestServiceAsync = new SearchRequestPublisher();

            //start message queue receiver for search queries
            searchRequestService.SendMessage();
            searchRequestServiceAsync.SendMessageAsync();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime life)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            life.ApplicationStopped.Register(DisposeResources);
        }

        private void DisposeResources()
        {
            //close connections of search queries
            searchRequestService.CloseConnection();
            searchRequestServiceAsync.CloseConnection();
        }
    }
}
