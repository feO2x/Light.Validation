using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Results;

namespace Bachelor.Thesis.Benchmarking.FlatEightParametersDto.Validators;

public class FluentValidator : AbstractValidator<Employee>
{
    public FluentValidator()
    {
        RuleFor(employee => employee.Id).NotEmpty();
        RuleFor(employee => employee.Name).NotEmpty().Length(2, 80);
        RuleFor(employee => employee.Department).InclusiveBetween((short) 100, (short) 999);
        RuleFor(employee => employee.WeeklyWorkingHours).InclusiveBetween(20, 48);
        RuleFor(employee => employee.PhoneNumber).Matches(new Regex(@"^[0-9]"));
        RuleFor(employee => employee.OvertimeWorked).InclusiveBetween(float.MinValue, float.MaxValue);
        RuleFor(employee => employee.HourlySalary).InclusiveBetween(new decimal(12.0), new decimal(999.0));
    }

    protected override bool PreValidate(ValidationContext<Employee> context, ValidationResult result)
    {
        context.InstanceToValidate.Name = context.InstanceToValidate.Name.Trim();
        return true;
    }
}