using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["IdentityServiceUrl"];
        options.Audience = "auctionApp";
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters.ValidateAudience = false;
        options.TokenValidationParameters.NameClaimType = "username";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("customPolicy", policy => policy.RequireAuthenticatedUser());
});

builder.Services.AddCors(opts =>
{
    opts.AddPolicy(
        "customPolicy",
        b =>
        {
            b
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowAnyOrigin();
        }
    );
});

var app = builder.Build();

app.UseCors();

app.MapReverseProxy();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
