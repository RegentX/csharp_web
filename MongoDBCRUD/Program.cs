using MongoDBCRUD.Models;

using MongoDBCRUD.Services;


var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<TrainerDatabaseSettings>(
    builder.Configuration.GetSection("TrainersDatabase"));

builder.Services.Configure<DesignerDatabaseSettings>(
    builder.Configuration.GetSection("DesignerDatabase"));


builder.Services.AddSingleton<TrainersService>();
builder.Services.AddSingleton<DesignerService>();

builder.Services.AddControllers()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();