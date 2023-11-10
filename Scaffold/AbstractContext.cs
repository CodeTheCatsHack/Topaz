using Microsoft.EntityFrameworkCore;

namespace Scaffold;

public abstract class AbstractContext<T> : DbContext
    where T : DbContext
{
    public AbstractContext(DbContextOptions<T> options) : base(options)
    {
    }

    public IQueryable<string> SelectExpression(string nameFunction, List<string> parametrList)
    {
        return Database.SqlQuery<string>(
            $"SELECT `{nameFunction}`({parametrList.Aggregate((x, y) => $"\"{x}\",\"{y}\"")});");
    }

    public IQueryable<string> SelectExpression(string nameFunction, string parametr)
    {
        return Database.SqlQuery<string>($"SELECT `{nameFunction}`(\"{parametr}\");");
    }
}