using Microsoft.EntityFrameworkCore;

namespace Scaffold.Expressions
{
    /// <summary>
    /// Статичный класс расширений фильтраций
    /// </summary>
    public static class Filter
    {
        /// <summary>
        /// Фильтрация по свойству из DbSet с применением предиката
        /// </summary>
        /// <typeparam name="IModelContext">тип сравниваемой модели</typeparam>
        /// <param name="dbSet">контекст БД</param>
        /// <param name="predicate">предикат для сравнения</param>
        /// <returns>последовательность отфильтрованных моделей</returns>
        /// <example>
        /// <code>
        /// ctx.Measure.FilterWith((Measure x) => x.StartMeasure == new DateOnly(2019, 6, 3));
        /// </code>
        /// </example>
        public static IEnumerable<IModelContext> FilterWith<IModelContext>(this DbSet<IModelContext> dbSet, Func<IModelContext, bool> predicate) where IModelContext : class
        {
            return dbSet.AsEnumerable()
                .Where(predicate);
        }

        /// <summary>
        /// Фильтрация по свойству из DbSet с применением предиката
        /// </summary>
        /// <typeparam name="IModelContext">тип сравниваемой модели</typeparam>
        /// <param name="dbSet">контекст БД</param>
        /// <param name="predicate">предикат для сравнения используя сравниваемый объект</param>
        /// <param name="compared">сравниваемый объект</param>
        /// <returns>последовательность отфильтрованных моделей</returns>
        /// <example>
        /// <code>
        /// ctx.Measure.FilterWith((Measure x, object compared) => x.Conditions == (string)compared, "value")
        /// </code>
        /// </example>
        public static IEnumerable<IModelContext> FilterWith<IModelContext>(this DbSet<IModelContext> dbSet, Func<IModelContext, object, bool> predicate, object compared) where IModelContext : class
        {
            return dbSet.AsEnumerable()
                .Where(x => predicate(x, compared));
        }

        /// <summary>
        /// Фильтрация по свойству из DbSet
        /// </summary>
        /// <typeparam name="IModelContext">тип сравниваемой модели</typeparam>
        /// <param name="dbSet">контекст БД</param>
        /// <param name="property">сравниваемое поле</param>
        /// <param name="compared">сравниваемый объект</param>
        /// <returns>последовательность отфильтрованных моделей</returns>
        public static IEnumerable<IModelContext> FilterWith<IModelContext>(this DbSet<IModelContext> dbSet, string property, object compared) where IModelContext : class
        {
            return FilterWith(dbSet, (IModelContext model, object compar) => model.IsFilter(property, compar), compared);
        }

        /// <summary>
        /// Фильтрация по текстовому свойству из DbSet
        /// </summary>
        /// <typeparam name="IModelContext">тип сравниваемой модели</typeparam>
        /// <param name="dbSet">контекст БД</param>
        /// <param name="property">сравниваемое поле типа String</param>
        /// <param name="compared">сравниваемый текст</param>
        /// <param name="comparePolicy">политика сравнения</param>
        /// <param name="comparision">вид сравнения</param>
        /// <returns>последовательность отфильтрованных моделей</returns>
        public static IEnumerable<IModelContext> FilterWith<IModelContext>(this DbSet<IModelContext> dbSet, string property, string compared, ComparePolicy comparePolicy = ComparePolicy.Contains, StringComparison comparision = StringComparison.CurrentCultureIgnoreCase) where IModelContext : class
        {
            return FilterWith(dbSet, (IModelContext model) => model.IsFilter(property, compared, comparePolicy, comparision));
        }

        /// <summary>
        /// [NotImplemented] Асинхронный метод расширения для фильтрации по полю из DbSet
        /// </summary>
        /// <typeparam name="IModelContext">тип сравниваемой модели</typeparam>
        /// <param name="dbSet">контекст БД</param>
        /// <param name="property">сравниваемое поле</param>
        /// <param name="compared">сравниваемое значение</param>
        /// <returns>последовательность отфильтрованных моделей</returns>
        /// <exception cref="NotImplementedException"></exception>
        public async static IAsyncEnumerable<IModelContext> FilterWithAsync<IModelContext>(this DbSet<IModelContext> dbSet, string property, object compared) where IModelContext : class
        {
            throw new NotImplementedException("Не реализован");
            var iterator = dbSet.GetAsyncEnumerator();
            while (await iterator.MoveNextAsync())
            {
                if (iterator.Current.IsFilter(property, compared) || true)
                {
                    yield return iterator.Current;
                }
            }
        }

        /// <summary>
        /// Сравнение свойства модели с сравниваемым объектом
        /// </summary>
        /// <typeparam name="IModelContext">тип сравниваемой модели</typeparam>
        /// <param name="model">сравниваемая модель</param>
        /// <param name="property">сравниваемое свойство</param>
        /// <param name="compared">сравниваемое значение</param>
        /// <returns>true, если свойство совпадает, иначе - false</returns>
        public static bool IsFilter<IModelContext>(this IModelContext model, string property, object compared)
        {
            return model.GetRequiredValueByProperty(property)?.Equals(compared) ?? false;
        }

        /// <summary>
        /// Сравнение текстового свойства модели с сравниваемым текстом в соответствии с параметрами сравнения
        /// </summary>
        /// <typeparam name="IModelContext">тип сравниваемой модели</typeparam>
        /// <param name="model">сравниваемая модель</param>
        /// <param name="property">сравниваемое свойство хранящее текст</param>
        /// <param name="compared">сравниваемый текст</param>
        /// <param name="policy">политика сравнения</param>
        /// <param name="comparision">вид сравнения</param>
        /// <returns>true, если свойство подходит под сравнение, иначе - false</returns>
        /// <exception cref="Exceptions.WrongPropertyTypeException">если тип свойства не строка</exception>
        /// <exception cref="Exceptions.WrongComparePolicyException">если для выбраной политики не существует обработчика</exception>
        /// <exception cref="MissingFieldException">если свойство не найдено</exception>
        public static bool IsFilter<IModelContext>(this IModelContext model, string property, string compared, ComparePolicy policy = ComparePolicy.Contains, StringComparison comparision = StringComparison.CurrentCultureIgnoreCase)
        {
            object obj = model.GetRequiredValueByProperty(property);
            if (obj is not string)
            {
                throw new Exceptions.WrongPropertyTypeException(property.GetType(), typeof(String));
            }

            return ((string)obj).ResolveComparePolicy(compared, policy, comparision);
        }

        /// <summary>
        /// Политика сравнения
        /// </summary>
        public enum ComparePolicy
        {
            Equals,
            StartsWith,
            EndsWith,
            Contains
        }

        /// <summary>
        /// Разрешение предиката в зависимости от политики сравнения
        /// </summary>
        /// <param name="value">исходное значение</param>
        /// <param name="compared">сравниваемое значение</param>
        /// <param name="policy">политика сравнения</param>
        /// <param name="comparision">вид сравнения</param>
        /// <returns>true, если сравнение нашло совпадение, иначе - false</returns>
        /// <exception cref="Exceptions.WrongComparePolicyException">если получена неизвестная политика</exception>
        private static bool ResolveComparePolicy(this string value, string compared, ComparePolicy policy, StringComparison comparision = StringComparison.CurrentCultureIgnoreCase)
        {
            switch (policy)
            {
                case ComparePolicy.Equals:
                    return value.Equals(compared, comparision);
                case ComparePolicy.StartsWith:
                    return value.StartsWith(compared, comparision);
                case ComparePolicy.EndsWith:
                    return value.EndsWith(compared, comparision);
                case ComparePolicy.Contains:
                    return value.Contains(compared, comparision);
                default:
                    throw new Exceptions.WrongComparePolicyException(policy);
            }
        }

        /// <summary>
        /// Кастомные ошибки фильтрации
        /// </summary>
        public static class Exceptions
        {
            public class WrongComparePolicyException : Exception
            {
                public WrongComparePolicyException(ComparePolicy policy) : base($"получена неизвестная политика: {policy}. Расширьте метод, добавив обработчик для этой политики.") { }
            }

            public class WrongPropertyTypeException : Exception
            {
                public WrongPropertyTypeException(Type actual, Type expected) : base($"получен неправильный тип свойства: {actual}, ожидался: {expected}") { }
            }

            public class PropertyNotFoundException : Exception
            {
                public PropertyNotFoundException(string property, Type? type) : base($"поле {property} не найдено в классе {type}") { }
            }
        }
    }
}
