using FluentValidation.Results;

namespace Application.Helpers;

internal static class FulentValidationHelper
{
    public static List<string> ConvertErrorsToList(this ValidationResult validationResult)
    {
        return validationResult
            .Errors
            .Where(e => e is not null)
            ?.Select(r => r.ErrorMessage)
            ?.Distinct()
            ?.ToList();
    }

}
