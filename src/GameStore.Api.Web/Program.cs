using Dapper;
using FastEndpoints;
using FastEndpoints.Swagger;
using FluentValidation;
using GameStore.Api.SharedKernel.Mediator.Behaviors;
using GameStore.Api.SharedKernel.Persistence.TypeMappers;
using GameStore.Api.Users;
using GameStore.Api.Users.Connect;

SqlMapper.AddTypeHandler(new SqlDateOnlyTypeHandler());
SqlMapper.AddTypeHandler(new SqlTimeOnlyTypeHandler());

ValidatorOptions.Global.LanguageManager.Enabled = false;

var builder = WebApplication.CreateBuilder();

var connectionString = builder.Configuration.GetConnectionString("GameStoreDb");
ArgumentNullException.ThrowIfNull(connectionString);

var assembly = UsersAssembly.Instance;

builder.Services.AddFastEndpoints(options =>
{
    options.Assemblies = new[] { assembly };
})
.SwaggerDocument(opt =>
{
    opt.DocumentSettings = s =>
    {
        s.Title = "GameStore.Api";
        s.Description = "Rest API for the GameStore application.";
        s.Version = "v1";
    };
    opt.ShortSchemaNames = true;
    opt.AutoTagPathSegmentIndex = 2;
});

builder.Services.AddNpgsqlDataSource(connectionString);
builder.Services.AddMediatR(config => 
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(ExceptionBehavior<,>));
});


builder.Services.AddUsersModule();
builder.Services.AddUsersConnect();


var app = builder.Build();
app.UseFastEndpoints().UseSwaggerGen(uiConfig: opt =>
{
    // This removes the botton 'Models' section from the swagger UI
    opt.DefaultModelsExpandDepth = -1;
});

app.Run();

public partial class Program { }
