//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;
//using NLog;
//using NLog.Web;
//using WebApiTemplate.Database;
//using Microsoft.EntityFrameworkCore;

//var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
//logger.Debug("init main");

//try
//{
//    var builder = WebApplication.CreateBuilder(args);

//    // Add services to the container.
//    builder.Services.AddControllers();
//    builder.Services.AddEndpointsApiExplorer();
//    builder.Services.AddSwaggerGen();

//    builder.Logging.ClearProviders();
//    builder.Host.UseNLog();

//    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
//    {
//        options.RequireHttpsMetadata = false;
//        options.SaveToken = true;
//        options.TokenValidationParameters = new TokenValidationParameters()
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidAudience = builder.Configuration["Jwt:Audience"],
//            ValidIssuer = builder.Configuration["Jwt:Issuer"],
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//        };
//    });
//    builder.Services.AddEndpointsApiExplorer();
//    builder.Services.AddSwaggerGen();

//    builder.Services.AddDbContext<UserContext>(options =>
//      options.UseSqlServer(builder.Configuration.GetConnectionString("EFIntroContext")));

//    builder.Services.AddCors(options =>
//    {

//        options.AddPolicy("AllowLocalhost4200",
//           // builder => builder.SetIsOriginAllowed(origin => new Uri(origin).Host== "localhost:7115")
//           builder => builder.AllowAnyOrigin()
//                              .AllowAnyHeader()
//                              .AllowAnyMethod()
//                              .SetIsOriginAllowedToAllowWildcardSubdomains());
//    });

//    try
//    {
//        var app = builder.Build();
//        //if (app.Environment.IsDevelopment())
//        //{
//        //    app.UseSwagger();
//        //    app.UseSwaggerUI();
//        //}
//        app.UseSwagger();
//        app.UseSwaggerUI();
//        app.UseHttpsRedirection();

//        app.UseCors("Policy");

//        app.UseAuthentication();

//        app.UseAuthorization();

//        app.MapControllers();

//        app.Run();
//    }
//    catch (Exception ex)
//    {
//        logger.Error(ex, "Programa detenido porque tiene excepciones");
//    }
//}
//catch (Exception e)
//{
//    logger.Error(e, "Programa detenido porque tiene excepciones");
//    throw;
//}

//finally
//{
//    NLog.LogManager.Shutdown();
//}