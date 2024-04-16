using Microsoft.EntityFrameworkCore;
using PetProject.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddDbContext<ToDoDB>();

var app = builder.Build();

app.UseRouting();

#pragma warning disable ASP0014
app.UseEndpoints(ep =>
{
    ep.MapControllers();
});
#pragma warning restore ASP0014

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger().UseSwaggerUI(opts => { opts.DocumentTitle = "ToDoDTO API"; });
}

app.Run();