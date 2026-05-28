using System.ComponentModel.DataAnnotations;

namespace MuseumExhibits.API.Endpoints;

public static class ValidationHelper
{
    public static bool TryValidate<T>(T model, out Dictionary<string, string[]> errors)
    {
        var ctx     = new ValidationContext(model!);
        var results = new List<ValidationResult>();
        bool valid  = Validator.TryValidateObject(model!, ctx, results, validateAllProperties: true);

        errors = results
            .GroupBy(r => r.MemberNames.FirstOrDefault() ?? string.Empty)
            .ToDictionary(
                g => g.Key,
                g => g.Select(r => r.ErrorMessage ?? string.Empty).ToArray());

        return valid;
    }
}
