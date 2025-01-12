using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;

namespace Discounts.Configuration;

public class DatabaseConfiguration
{
    [Required]
    public required string ConnectionString { get; set; }

    [Required]
    public required bool Migrate { get; set; }
}

[OptionsValidator]
internal partial class DatabaseConfigurationValidator : IValidateOptions<DatabaseConfiguration>;
