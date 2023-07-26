using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(opts =>
{
    //opts.AddDefaultPolicy(builder =>
    //{
    //    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    //});
    //opts.AddPolicy("AllowSites", builder =>
    //{
    //    builder.WithOrigins("https://localhost:7090", "https://mysite.com").AllowAnyHeader().AllowAnyMethod();
    //});

    //opts.AddPolicy("AllowSites2", builder =>
    //{
    //    builder.WithOrigins("https://mysite2.com").WithHeaders(HeaderNames.ContentType, "x-custom-header");
    //});

    opts.AddPolicy("AllowSites", builder =>
    {
        // Domainin altýndaki tüm subdomainleri kabul et.
        builder.WithOrigins("https://*.example.com").SetIsOriginAllowedToAllowWildcardSubdomains();
    });

    opts.AddPolicy("AllowSites2", builder =>
    {
        builder.WithOrigins("https://www.example.com").WithMethods("POST", "GET").AllowAnyHeader();   
    });

});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();
//app.UseCors("AllowSites2");

app.UseAuthorization();

app.MapControllers();

app.Run();
