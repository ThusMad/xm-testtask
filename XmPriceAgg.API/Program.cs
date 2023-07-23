using Serilog;
using XmPriceAgg.API.Middlewares;
using XmPriceAgg.BLL;
using XmPriceAgg.BLL.AggregationAlgorithms;
using XmPriceAgg.BLL.Extensions;
using XmPriceAgg.BLL.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterSqLiteDb();
builder.Services.AddSingleton(new ProviderBuilder()
                                .BuildBitfinexProvider(builder.Configuration.GetSection("Providers:Bitfinex"))
                                .BuildBitstampProvider(builder.Configuration.GetSection("Providers:Bitstamp"))
                                .GetSources());
builder.Services.AddSingleton<IAggregationAlgorithm, AverageAggregationAlgorithm>();
builder.Services.AddTransient<IAggregationService, AggregationService>();

builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(builder.Configuration));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
