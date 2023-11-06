using MongoDBCRUD.Models;

using MongoDBCRUD.Services;


var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<BooksDatabaseSettings>(
    builder.Configuration.GetSection("BooksDatabase"));

builder.Services.Configure<AuthorDatabaseSettings>(
    builder.Configuration.GetSection("AuthorDatabase"));


builder.Services.AddSingleton<BooksService>();
builder.Services.AddSingleton<AuthorService>();

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