using Microsoft.EntityFrameworkCore;
using PetProject.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddDbContext<MainDB>(opts =>
{
    opts.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});


var app = builder.Build();

// я не совсем пониаю вещи ниже. ћне нужно было чтобы RouteAttribute работал пустой.
// Ќашел, как мне кажетс€ решение.

app.UseRouting();

#pragma warning disable ASP0014
app.UseEndpoints(ep =>
{
    ep.MapControllers();
});
#pragma warning restore ASP0014

///////////////////////////////////

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger().UseSwaggerUI(opts => { opts.DocumentTitle = "ToDo API"; });
}

app.Run();