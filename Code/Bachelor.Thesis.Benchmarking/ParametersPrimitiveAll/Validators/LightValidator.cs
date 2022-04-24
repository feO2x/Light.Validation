using Light.Validation;
using Light.Validation.Checks;
using Range = Light.Validation.Tools.Range;

namespace Bachelor.Thesis.Benchmarking.ParametersPrimitiveAll.Validators;

public class LightValidator : Validator<EmployeeDto>
{
    protected override EmployeeDto PerformValidation(ValidationContext context, EmployeeDto employeeDto)
    {
        employeeDto.Id = context.Check(employeeDto.Id)
                                .IsNotEmpty();
        employeeDto.Name = context.Check(employeeDto.Name)
                                  .IsNotNullOrWhiteSpace()
                                  .HasLengthIn(Range.FromInclusive(2).ToInclusive(80));
        employeeDto.Position = context.Check(employeeDto.Position)
                                      .IsIn(Range.FromInclusive('A').ToInclusive('Z'));
        employeeDto.Department = context.Check(employeeDto.Department)
                                        .IsIn(Range.FromInclusive((short) 100).ToInclusive(999));
        employeeDto.WeeklyWorkingHours = context.Check(employeeDto.WeeklyWorkingHours)
                                                .IsIn(Range.FromInclusive(20).ToInclusive(48));
        employeeDto.EmployeeId = context.Check(employeeDto.EmployeeId)
                                        .IsIn(Range.FromInclusive(10000L).ToInclusive(99999L));
        employeeDto.ProductivityScore = context.Check(employeeDto.ProductivityScore)
                                               .IsIn(Range.FromInclusive(0.0).ToInclusive(100.0));
        employeeDto.OvertimeWorked = context.Check(employeeDto.OvertimeWorked)
                                            .IsIn(Range.FromInclusive(-100.0f).ToInclusive(200.0f));
        employeeDto.HourlySalary = context.Check(employeeDto.HourlySalary)
                                          .IsIn(Range.FromInclusive(new decimal(12.0)).ToInclusive(new decimal(999.0)));

        return employeeDto;
    }
}