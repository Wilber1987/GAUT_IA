using System.Text.Json.Serialization;
using Microsoft.AspNetCore.ResponseCompression;
using APPCORE.Cron.Jobs;
using BusinessLogic.Connection;
using CAPA_NEGOCIO.SystemConfig;
using BusinessLogic.FormsIA;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container. 
builder.Services.AddRazorPages();

builder.Services.AddControllers().AddJsonOptions(JsonOptions =>
		JsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null);

#region CONFIGURACIONES PARA API
builder.Services.AddControllers()
	.AddJsonOptions(JsonOptions => JsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null)// retorna los nombres reales de las propiedades
	.AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = false)// Desactiva la indentación
	.AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddResponseCompression(options =>
{
	options.EnableForHttps = true; // Activa la compresión también para HTTPS
	options.Providers.Add<GzipCompressionProvider>(); // Usar Gzip
	options.Providers.Add<BrotliCompressionProvider>(); // Usar Brotli (más eficiente)
});
builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
	options.Level = System.IO.Compression.CompressionLevel.Fastest; // Puedes ajustar la compresión
});

builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
	options.Level = System.IO.Compression.CompressionLevel.Fastest; // Nivel de compresión para Brotli
});

#endregion


builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(40);
});


#region CRONJOB


#endregion

var app = builder.Build();
new BDConnection().InitMainConnection(app.Environment.IsDevelopment());
SystemConfigImpl.IsDevelopment = app.Environment.IsDevelopment();
// Configure the HTTP request pipeline.
new FormsIAOperation().ProcessForm();

if (!app.Environment.IsDevelopment())
{	
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseDefaultFiles();
app.UseResponseCompression(); // Usa la compresión en la aplicación

app.UseRouting(); 
app.UseSession();

app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();

app.Run();
