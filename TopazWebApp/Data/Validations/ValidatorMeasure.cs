using FluentValidation;
using Scaffold.Model;

namespace Topaz.Data.Validations;

public class ValidatorMeasure : AbstractValidator<Measure>
{
    public ValidatorMeasure()
    {
        RuleFor(user => user.Place)
            .RuleNotEmpty();

        RuleFor(user => user.Conditions)
            .RuleNotEmpty();

        RuleFor(user => user.EndMeasure)
            .RuleNotEmpty()
            .GreaterThanOrEqualTo(user => user.StartMeasure)
            .WithMessage("Конец времени проведения контроля не может быть в будущем.");

        RuleFor(user => user.StartMeasure)
            .RuleNotEmpty()
            .LessThanOrEqualTo(user => user.EndMeasure)
            .WithMessage(
                "Начало времени проведения контроля должно быть больше или равно времени окончания проведения контроля.");
    }
}

public static class ValidationsExpression
{
    public static IRuleBuilderOptions<T, TProperty> RuleNotEmpty<T, TProperty>(
        this IRuleBuilderInitial<T, TProperty> ruleBuilderInitial)
    {
        return ruleBuilderInitial
            .NotEmpty()
            .WithMessage("Поле не может быть пустым.");
    }
}