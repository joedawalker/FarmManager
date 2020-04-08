using EquipmentApi.Classes;
using EquipmentApi.Data;
using Framework.NetCore.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace EquipmentApi
{
	public class Startup
	{
		public Startup( IConfiguration configuration )
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices( IServiceCollection services )
		{
			services.AddControllersWithViews();

			// Add a filter to handle HTTP response exceptions
			services.AddControllers( options =>
				options.Filters.Add( new HttpResponseExceptionFilter() ) );

			// Register the Swagger generator, defining 1 or more Swagger documents
			services.AddSwaggerGen( c =>
			{
				c.SwaggerDoc( "v1", new OpenApiInfo { Title = "Equipment Manager", Version = "v1" } );

				// Set the comments path for the Swagger JSON and UI.
				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine( AppContext.BaseDirectory, xmlFile );
				c.IncludeXmlComments( xmlPath );
			} );

			// In production, the Angular files will be served from this directory
			services.AddSpaStaticFiles( configuration =>
			 {
				 configuration.RootPath = "ClientApp/dist";
			 } );

			// Add database contexts
			DbContextOptionsBuilder<EquipmentApiContext> optionsBuilder = new DbContextOptionsBuilder<EquipmentApiContext>();
			optionsBuilder.UseSqlServer( Configuration.GetConnectionString( "EquipmentApiContext" ) );

			var equipmentApiContext = new EquipmentApiContext( optionsBuilder.Options );
			services.AddSingleton( equipmentApiContext );

			// Add service level dependencies 
			IEquipmentRepository equipmentRepository = new EquipmentRepository( equipmentApiContext );
			IEquipmentManager equipmentManager = new EquipmentManager( equipmentRepository );
			services.AddSingleton( equipmentRepository );
			services.AddSingleton( equipmentManager );

			IUserRepository userRepository = new UserRepository( equipmentApiContext );
			IUserManager userManager = new UserManager( userRepository );
			services.AddSingleton( userRepository );
			services.AddSingleton( userManager );

			services.AddDbContext<EquipmentApiContext>( options =>
					 options.UseSqlServer( Configuration.GetConnectionString( "EquipmentApiContext" ) ) );
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure( IApplicationBuilder app, IWebHostEnvironment env )
		{
			if ( env.IsDevelopment() )
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler( "/Error" );
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();
			if ( !env.IsDevelopment() )
			{
				app.UseSpaStaticFiles();
			}

			// Enable middleware to serve generated Swagger as a JSON endpoint.
			app.UseSwagger();

			// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
			// specifying the Swagger JSON endpoint.
			app.UseSwaggerUI( c =>
			{
				c.SwaggerEndpoint( "/swagger/v1/swagger.json", "Equipment Manager V1" );
				c.RoutePrefix = string.Empty;
			} );

			app.UseRouting();

			app.UseEndpoints( endpoints =>
			 {
				 endpoints.MapControllerRoute(
					 name: "default",
					 pattern: "{controller}/{action=Index}/{id?}" );
			 } );

			app.UseSpa( spa =>
			 {
				 // To learn more about options for serving an Angular SPA from ASP.NET Core,
				 // see https://go.microsoft.com/fwlink/?linkid=864501

				 spa.Options.SourcePath = "ClientApp";

				 if ( env.IsDevelopment() )
				 {
					 spa.UseAngularCliServer( npmScript: "start" );
				 }
			 } );
		}
	}
}
