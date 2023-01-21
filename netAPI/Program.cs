using netAPI.Helpers;
using netAPI.RouteGroups;
using netAPI.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// add services to DI container
{
    var services = builder.Services;
    var env = builder.Environment;

    services.AddDbContext<DataContext>();
    services.AddCors();
    services.AddControllers().AddJsonOptions(x =>
    {
        // serialize enums as strings in api responses (e.g. Role)
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

        // ignore omitted parameters on models to enable optional params (e.g. User update)
        x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    // configure DI for application services
    services.AddScoped<UsersRouteGroup>();
    services.AddScoped<IUserService, UserService>();
}

// Add services to the container.
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
// global cors policy
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
// global error handler
app.UseMiddleware<ErrorHandlerMiddleware>();

#region API
var scope = app.Services.CreateScope();

#region Users
var usersRouteGroup = scope.ServiceProvider.GetRequiredService<UsersRouteGroup>();
app.MapGet("/Users", usersRouteGroup.GetAll);
app.MapGet("/Users/{id}", usersRouteGroup.GetById);
app.MapPost("/Users", usersRouteGroup.Create);
app.MapPut("/Users/{id}", usersRouteGroup.Update);
app.MapDelete("/Users/{id}", usersRouteGroup.Delete);
#endregion

#endregion
app.Run();
