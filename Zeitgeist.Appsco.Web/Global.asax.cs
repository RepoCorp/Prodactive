using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ServiceStack.CacheAccess;
using ServiceStack.CacheAccess.Providers;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.PostgreSQL;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.WebHost.Endpoints;
using Zeitgeist.Appsco.Web.Api;



namespace Zeitgeist.Appsco.Web
{
    // Nota: para obtener instrucciones sobre cómo habilitar el modo clásico de IIS6 o IIS7, 
    // visite http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {

        public class ServicioAppHost : AppHostBase
        {
            public ServicioAppHost() : base("Servicio Prueba", typeof(TestService).Assembly) { }

            public override void Configure(Funq.Container container)
            {
         
                //DbRegistration
                var connectionString = "";// ConfigurationManager.ConnectionStrings["Bd"].ConnectionString;
                container.Register<IDbConnectionFactory>(new OrmLiteConnectionFactory(connectionString, PostgreSqlDialect.Provider));
                //IoC
                //IoC                
                //container.RegisterAutoWired<Negocio>();

                //Autentication
                Plugins.Add(new AuthFeature(() => new AuthUserSession(), new IAuthProvider[] { new BasicAuthProvider() }));
                container.Register<ICacheClient>(new MemoryCacheClient());

                var userRepository = new InMemoryAuthRepository();
                container.Register<IUserAuthRepository>(userRepository);
                string hash;
                string salt;
                var usuario = "A1b2c3d4"; //ConfigurationManager.AppSettings["user"].ToString();
                var pass = "A1b2c3d4";//ConfigurationManager.AppSettings["pass"].ToString();

                new SaltedHash().GetHashAndSaltString(pass, out hash, out salt);
                userRepository.CreateUserAuth(new UserAuth() { DisplayName = "Admin", Email = "admin@admin.com", FirstName = "pepe", LastName = "otro", UserName = usuario, PasswordHash = hash, Salt = salt }, pass);
                
                //Routes
                //    .Add<Peticion>("/Peticion")
                //    .Add<Peticion>("/Peticion/{A}");
            }
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            new ServicioAppHost().Init();
            Application["OnlineUsers"] = 0;
        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started
            Application.Lock();
            Application["OnlineUsers"] = (int)Application["OnlineUsers"] + 1;
            Application.UnLock();
        }
        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode is set to InProc in the Web.config file. If session mode is set to StateServer or SQLServer, the event is not raised.
            Application.Lock();
            Application["OnlineUsers"] = (int)Application["OnlineUsers"] - 1;
            Application.UnLock();
        }
    }
}