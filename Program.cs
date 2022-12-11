using Microsoft.Extensions.Diagnostics.HealthChecks;
using SecuringSQL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHealthChecks().AddCheck<SQLConnectionHealthCheck>("SQLDB Connection Check");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseHealthChecks("/healthcheck", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions()
{
    ResponseWriter = (context, result) => context.Response.WriteAsync(result.Entries.Values.First().Description)
});

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapHealthChecks("/healthcheck");

app.Run();