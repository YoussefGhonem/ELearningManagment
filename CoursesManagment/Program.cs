using CoursesManagment.Geteway.ExceptionHandling;
using CoursesManagment.Geteway.Extensions;
using CoursesManagment.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDBConfiguration(builder.Configuration);

#region .Net services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllerConfiguration();
builder.Services.AddServicesApplication();
builder.Services.AddExceptionHandling();

#endregion

#region Email
//builder.Services.AddSendGrid(builder.Configuration);
#endregion

#region Swagger
builder.Services.AddIdentity(builder.Configuration);
builder.Services.AddSwagger();
#endregion

var app = builder.Build();

var options = ((IApplicationBuilder)app).ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>();

app.UseRequestLocalization(options.Value);

if (!app.Environment.IsDevelopment()) // Development
{
    app.UseDeveloperExceptionPage();
}
else // Production
{
    app.UseExceptionHandler("/Error");
}

#region  Swagger
app.UseBaseSwagger();
#endregion
app.UseExceptionHandling();
app.UseIdentity();
app.UseStaticFiles();
app.UseControllerConfiguration();

using (var scope = app.Services.CreateScope())
{
    await scope.MigrateDatabase();
    await scope.SeedDatabase();
}
app.Run();
