using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using AngularHazi.Data.Models;
using AngularHazi.Web.Services.Identity;
using Autofac;
using Autofac.Integration.Web;
using Autofac.Integration.WebApi;
using AutoMapper;
using AutoMapper.Mappers;
using Microsoft.Owin.Security;

namespace AngularHazi.Web.App_Start
{
    public class DependencyInjection
    {
        public static ContainerProvider Setup()
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(Assembly.Load("AngularHazi.Data")).Where(t => t.Name.EndsWith("Store")).AsImplementedInterfaces().InstancePerRequest();

            // Web API controllers
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // Db context
            builder.RegisterType<ApplicationDbContext>().InstancePerRequest();

            // AspNet Identity
            builder.Register<IAuthenticationManager>(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
            builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();

            // AutoMapper config
            builder.RegisterType<EntityMappingProfile>().As<Profile>();
            builder.Register(ctx => new ConfigurationStore(new TypeMapFactory(), MapperRegistry.Mappers))
                .AsImplementedInterfaces().SingleInstance()
                .OnActivating(x =>
                {
                    foreach (var profile in x.Context.Resolve<IEnumerable<Profile>>())
                    {
                        x.Instance.AddProfile(profile);
                    }
                });
            builder.RegisterType<MappingEngine>().As<IMappingEngine>().SingleInstance();

            // build-eljük a containert
            var container = builder.Build();

            // adjuk át a webapinak a DI resolvert
            //var config = GlobalConfiguration.Configuration;
            //config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            // majd adjuk vissza a providert a webforms-nak
            return new ContainerProvider(container);
        }
    }
}