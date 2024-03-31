using Common;
using CreateAndUpdateKmlLib;
using KmlSerializer;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMvc().AddNewtonsoftJson();

builder.Services.AddSingleton<IUpdateKmlIfExistsOrCreateNewIfNot, UpdateKmlIfExistsOrCreateNewIfNot>();
builder.Services.AddSingleton<IUpdateKml, UpdateKml>();
builder.Services.AddSingleton<IKmlSerializer>(_ => new KmlSerializerTextWriter(typeof(KmlModel.Kml)));
builder.Services.AddSingleton<ICreateKml>(_ => new CreateKml("test", "test"));

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory())),
    RequestPath = "",
    ServeUnknownFileTypes = true,
    DefaultContentType = "text/plain"
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
