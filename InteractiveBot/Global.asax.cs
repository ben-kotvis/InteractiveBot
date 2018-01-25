using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.ConnectorEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.Dialogs.Internals;
using System.Globalization;
using System.Reflection;
using Microsoft.Bot.Connector;
using Microsoft.WindowsAzure.Storage;

namespace InteractiveBot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            
            //Using the local DocDbEmulator, which should be installed and started. Otherwise, edit the docDbEmulatorUri and docDbEmulatorKey variables to your DocDb database
            //Reference: https://docs.microsoft.com/en-us/azure/documentdb/documentdb-nosql-local-emulator

            //Fixed docDb emulator local Uri.
            Uri docDbEmulatorUri = new Uri("https://localhost:8081");

            //Fixed docDb emulator key
            const string docDbEmulatorKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

            var globalConfig = GlobalConfiguration.Configuration;

            Conversation.UpdateContainer(
                builder =>
                {
                    builder.RegisterModule(new AzureModule(Assembly.GetExecutingAssembly()));

                    var store = new DocumentDbBotDataStore(docDbEmulatorUri, docDbEmulatorKey);

                    builder.Register(c => store)
                        .Keyed<IBotDataStore<BotData>>(AzureModule.Key_DataStore)
                        .AsSelf()
                        .SingleInstance();

                        // Register your Web API controllers.
                        builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
                    builder.RegisterWebApiFilterProvider(globalConfig);

                });



            globalConfig.DependencyResolver = new AutofacWebApiDependencyResolver(Conversation.Container);
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<ActivityLogger>().AsImplementedInterfaces().InstancePerDependency();
            containerBuilder.Update(Conversation.Container);
        }

        public static ILifetimeScope FindContainer()
        {
            var config = GlobalConfiguration.Configuration;
            var resolver = (AutofacWebApiDependencyResolver)config.DependencyResolver;
            return resolver.Container;
        }



    }
}
