using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = bool.Parse(builder.Configuration["Authentication:RequireHttpsMetadata"]);
    options.Authority = builder.Configuration["Authentication:Authority"];
    options.IncludeErrorDetails = bool.Parse(builder.Configuration["Authentication:IncludeErrorDetails"]);
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidateAudience = bool.Parse(builder.Configuration["Authentication:ValidateAudience"]),

        ValidAudience = builder.Configuration["Authentication:ValidAudience"],

        ValidateIssuerSigningKey = bool.Parse(builder.Configuration["Authentication:ValidateIssuerSigningKey"]),

        ValidateIssuer = bool.Parse(builder.Configuration["Authentication:ValidateIssuer"]),

        ValidIssuer = builder.Configuration["Authentication:ValidIssuer"],

        ValidateLifetime = bool.Parse(builder.Configuration["Authentication:ValidateLifetime"])

    };
    options.Events = new JwtBearerEvents()
    {
        OnAuthenticationFailed = e =>
        {
            e.NoResult();
            e.Response.StatusCode = StatusCodes.Status401Unauthorized;

            return Task.CompletedTask;
        }
    };
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
