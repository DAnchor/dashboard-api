using FluentValidation;
using Dashboard.Dtos.Payload.Task;

namespace Dashboard.Api.Validations.Task;

public class TaskUpdateRequestValidator : AbstractValidator<TaskUpdateRequestDto>
{
    public TaskUpdateRequestValidator()
    {
        RuleFor(x => x.Name).Custom((str, context) =>
        {
            if (str.ToString().Length == 0)
            {
                context.AddFailure("Name: field should not be empty.");
            }
            else if (str.ToString().Length > 50)
            {
                context.AddFailure("Name: must not exceed 50 characters length.");
            }
        });
        RuleFor(x => x.Description).Custom((str, context) =>
        {
            if (!string.IsNullOrEmpty(str))
            {
                if (str.Length > 250)
                {
                    context.AddFailure("Description: must not exceed 250 characters length.");
                }
            }
        });
        RuleFor(x => x.DueDate).Custom((str, context) =>
        {
            if (str.ToString().Length == 0)
            {
                context.AddFailure("Due date: field should not be empty.");
            }
        });
        RuleFor(x => (int)x.Priority).Custom((num, context) =>
        {
            if (num < 1 || num > 3)
            {
                context.AddFailure("Priority: must be in range between 1 and 3.");
            }
        });
        RuleFor(x => (int)x.Status).Custom((num, context) =>
        {
            if (num < 1 || num > 4)
            {
                context.AddFailure($"{context.PropertyName}: must be in range between 1 and 4.");
            }
        });
    }
}