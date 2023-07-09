using Tic_Tac_again.Models;
using Tic_Tac_again.Models.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddTransient<Game>();
builder.Services.AddTransient<ClientService>();
builder.Services.AddTransient<TicTacToeService>();
//builder.Services.AddTransient<Game>();

builder.Services.AddCors(options => {
    options.AddPolicy("MyPolicy", builder => {
        builder.AllowAnyOrigin();
        builder.AllowAnyMethod();
        builder.AllowAnyHeader();
    });

});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("MyPolicy");

//app.UseCors(builder => builder.AllowAnyOrigin());

//app.UseEndpoints(endpoints =>
//{
//    //endpoints.MapGet("/", async context =>
//    //{
//    //    await context.Response.WriteAsync("Hello World!");
//    //});
//});

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
