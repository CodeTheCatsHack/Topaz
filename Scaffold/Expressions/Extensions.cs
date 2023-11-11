using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Scaffold.Expressions.Filter;

namespace Scaffold.Expressions
{
    internal static class Extensions
    {
        /// <summary>
        /// Получение значения по полю из объекта
        /// </summary>
        /// <typeparam name="IModelContext">тип модели</typeparam>
        /// <param name="model">экземпляр модели</param>
        /// <param name="property">требуемое свойство</param>
        /// <returns>значение свойства</returns>
        /// <exception cref="Exceptions.PropertyNotFoundException">если свойство не найдено</exception>
        internal static object GetRequiredValueByProperty<IModelContext>(this IModelContext model, string property)
        {
            return model?.GetType().GetProperty(property)?.GetValue(model, null) ?? throw new Exceptions.PropertyNotFoundException(property, model?.GetType());
        }
    }
}
