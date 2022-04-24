using FluentValidation;
using FluentValidation.Results;

namespace Bachelor.Thesis.Benchmarking.ParametersPrimitiveAll.Validators;

public class FluentValidator : AbstractValidator<EmployeeDto>
{
    public FluentValidator()
    {
        RuleFor(employee => employee.Id).NotEmpty();
        RuleFor(employee => employee.Name).NotEmpty().Length(2, 80);
        RuleFor(employee => employee.Position).NotEmpty().InclusiveBetween('A', 'Z');
        RuleFor(employee => employee.Department).NotEmpty().InclusiveBetween((short) 100, (short) 999);
        RuleFor(employee => employee.WeeklyWorkingHours).InclusiveBetween(20, 48);
        RuleFor(employee => employee.EmployeeId).NotEmpty().InclusiveBetween(10000L, 99999L);
        RuleFor(employee => employee.ProductivityScore).NotEmpty().InclusiveBetween(0.0, 100.0);
        RuleFor(employee => employee.OvertimeWorked).NotEmpty().InclusiveBetween(-100.0f, 200.0f);
        RuleFor(employee => employee.HourlySalary).NotEmpty().InclusiveBetween(new decimal(12.0), new decimal(999.0));
    }

    protected override bool PreValidate(ValidationContext<EmployeeDto> context, ValidationResult result)
    {
        context.InstanceToValidate.Name = context.InstanceToValidate.Name.Trim();
        return true;
    }
}