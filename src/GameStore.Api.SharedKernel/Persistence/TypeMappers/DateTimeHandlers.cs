using System.Data;
using Dapper;

namespace GameStore.Api.SharedKernel.Persistence.TypeMappers;

public class SqlDateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
{
    public override void SetValue(IDbDataParameter parameter, DateOnly date) 
        => parameter.Value = date.ToDateTime(new TimeOnly(0, 0));
    public override DateOnly Parse(object value) => DateOnly.FromDateTime((DateTime)value);
}

public class SqlTimeOnlyTypeHandler : SqlMapper.TypeHandler<TimeOnly>
{
    public override void SetValue(IDbDataParameter parameter, TimeOnly time)
    {
        parameter.Value = time.ToString();
    }

    public override TimeOnly Parse(object value) => TimeOnly.FromTimeSpan((TimeSpan)value);
}