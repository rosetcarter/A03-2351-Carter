using Day21Demo.Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false; // required - if this were true, Microsoft would add a long URI to the claim
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "https://demo-issuer.example.com",
            ValidateAudience = true,
            ValidAudience = "grpc-demo-api",
            ValidateLifetime = true,    // Reject an expired token 
            ClockSkew = TimeSpan.Zero,  // Expired token rejects immediately 
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsADemoSigningKeyForDay21AtLeast32Chars!")),
        };
    });

builder.Services.AddAuthorizationBuilder();

var app = builder.Build();

// Authentication first, then authorization 
app.UseAuthentication();

app.UseAuthorization();

app.MapGrpcService<ProductCatalogService>();

app.Run();
