using Crayon.API.Middlewares;
using Crayon.Application.Features.Accounts.GetServicesForAccount;
using Crayon.Application.Features.Accounts.OrderServiceForAccount;
using Crayon.Application.Features.Accounts.PatchServiceForAccount;
using Crayon.Application.Features.Customers.GetAccountsForCustomer;
using Crayon.Application.Features.Users.Login;
using Crayon.Application.Features.Users.Register;
using Crayon.Application.Interfaces;
using Crayon.Application.Middlewares;
using Crayon.Domain.Entities;
using Crayon.Infrastructure.Security;
using Crayon.Infrastructure.Services;
using Crayon.Persistence;

using FluentValidation;
using MediatR;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using System.Text;

namespace Crayon.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });

            builder.Services.AddHttpClient();
            builder.Services.AddScoped<IHttpService, HttpService>();
            builder.Services.AddScoped<ICcpApiService, CcpApiService>();
            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddScoped<IDataContext, DataContext>();
            builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(GetServicesForAccountCommand).Assembly));
            builder.Services.AddValidatorsFromAssembly(typeof(GetServicesForAccountCommandValidator).Assembly);
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationHandlingMiddleware<,>));

            var identityBuilderProvider = builder.Services.AddIdentityCore<User>();
            var identityBuilder = new IdentityBuilder(identityBuilderProvider.UserType, identityBuilderProvider.Services);
            identityBuilder.AddEntityFrameworkStores<DataContext>();
            identityBuilder.AddSignInManager<SignInManager<User>>();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"]));

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true, // Validate jwt signature
                        IssuerSigningKey = key, // Specify the value of signature key
                        ValidateAudience = false, // Validate URL the token is comming from
                        ValidateIssuer = false
                    };
                });

            builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();

            var app = builder.Build();
            app.UseMiddleware<ErrorHandlingMiddleware>();

            //using (var scope = app.Services.CreateScope())
            //{
            //    var services = scope.ServiceProvider;

            //    try
            //    {
            //        var context = services.GetRequiredService<DataContext>();
            //        var userManager = services.GetRequiredService<UserManager<User>>();
            //        context.Database.Migrate(); // Automatically add/update our database
            //        Seed.SeedDataAsync(context, userManager).Wait();
            //    }
            //    catch (Exception exc)
            //    {
            //        var logger = services.GetRequiredService<ILogger<Program>>();
            //        logger.LogError(exc, "An error occured during migration");
            //    }
            //}

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}