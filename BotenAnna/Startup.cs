using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.Bot.Builder.BotFramework;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BotenAnna
{
    public class Startup
    {

 
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Env = env;
            /*Insert Knowledge ID*/KnowledgeID = "";
            /*Insert Host Adress*/HostAdress = "";
            /*Insert end point key*/endPoint = "";

        }
        public string KnowledgeID { get; }
        public string HostAdress { get; }
        public string endPoint { get; }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Env { get; }

        

        public void ConfigureServices(IServiceCollection services)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Env.ContentRootPath)
                .AddJsonFile("appsettings.json");

            var configuration = configurationBuilder
                .Build();

            services.AddBot<Bot>((options) => {
                options.CredentialProvider = new ConfigurationCredentialProvider(configuration);
            });

            services.AddSingleton(sp =>
            {
                var qnaService = new QnAMaker(new QnAMakerEndpoint()
                {
  
                    KnowledgeBaseId = KnowledgeID,
                    Host = HostAdress,
                    EndpointKey = endPoint
                });

                return qnaService;
            });

            services.AddMvc();
        }
            

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseBotFramework();
        }
    }
}
