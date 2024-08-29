using Dapper;
using FluentValidation;
using GameStore.Api.SharedKernel.Mediator.Behaviors;
using GameStore.Api.SharedKernel.Persistence.TypeMappers;
using GameStore.Api.Users;
using GameStore.Api.Users.Connect;
using Microsoft.AspNetCore.Builder;

SqlMapper.AddTypeHandler(new SqlDateOnlyTypeHandler());
SqlMapper.AddTypeHandler(new SqlTimeOnlyTypeHandler());

ValidatorOptions.Global.LanguageManager.Enabled = false;

var builder = WebApplication.CreateBuilder();

var assembly = UsersAssembly.Instance;

builder.Services.AddFastEndpoints(options =>
{
    options.Assemblies = new[] { assembly };
});
builder.Services.AddMediatR(config => 
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(ExceptionBehavior<,>));
});


builder.Services.AddUsersModule();
builder.Services.AddUsersConnect();

var app = builder.Build();
app.UseFastEndpoints();

app.Run();

public partial class TestHost { }