using Light.Validation;
using Light.Validation.Checks;
using Range = Light.Validation.Tools.Range;

namespace Bachelor.Thesis.Benchmarking.FlatEightParametersDto.Validators;

public class LightValidator : Validator<Employee>
{
    protected override Employee PerformValidation(ValidationContext context, Employee employee)
    {
        employee.Id = context.Check(employee.Id)
                             .IsNotEmpty();
        employee.Name = context.Check(employee.Name)
                               .IsNotNullOrWhiteSpace()
                               .HasLengthIn(Range.FromInclusive(2).ToInclusive(80));
        employee.Department = context.Check(employee.Department)
                                     .IsIn(Range.FromInclusive((short) 100).ToInclusive(999));
        employee.WeeklyWorkingHours = context.Check(employee.WeeklyWorkingHours)
                                             .IsIn(Range.FromInclusive(20).ToInclusive(48));
        employee.PhoneNumber = context.Check(employee.PhoneNumber)
                                      .ContainsOnlyDigits();
        employee.OvertimeWorked = context.Check(employee.OvertimeWorked)
                                         .IsIn(Range.FromInclusive(float.MinValue).ToInclusive(float.MaxValue));
        employee.HourlySalary = context.Check(employee.HourlySalary)
                                       .IsIn(Range.FromInclusive(new decimal(12.0)).ToInclusive(new decimal(999.0)));

        return employee;
    }
}