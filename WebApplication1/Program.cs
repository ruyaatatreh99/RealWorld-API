using WebApplication1.Repos;
using WebApplication1.middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opton => {
    opton.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SXkSqsKyNUyvGbnHs7ke2NCq8zQzNLW7mPmHbnZZ")),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
     opton => {
         opton.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
         {
             Description = "Standard Authorization header using the secret",
             In = ParameterLocation.Header,
             Name = "Authorization",
             Type = SecuritySchemeType.ApiKey
         });
         opton.OperationFilter<SecurityRequirementsOperationFilter>();
     });
builder.Services.AddDbContext<userContext>();
builder.Services.AddScoped<IUser, UserPepos>();
builder.Services.AddScoped<IArticle, ArticleRepos>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseMiddleware<Middleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();