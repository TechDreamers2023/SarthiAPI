using Sarthi.Infrastructure.ServiceExtension;
using Sarthi.Services;
using Sarthi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDIServices(builder.Configuration);

// Add services to the container.
builder.Services.AddTransient<ICommonService, CommonService>();
builder.Services.AddTransient<IRequestService, RequestService>();
builder.Services.AddTransient<IVendorService, VendorService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
  
//}

app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseCors("corsapp");
app.UseAuthorization();
app.MapControllerRoute(
    name: "defualt",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );
app.MapControllers();

app.Run();
