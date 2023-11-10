using Microsoft.EntityFrameworkCore;

namespace Scaffold;

public abstract class AbstractContext<T>(DbContextOptions<T> options) : DbContext(options)
    where T : DbContext
{
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