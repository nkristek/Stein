using System.Text.RegularExpressions;
using NKristek.Smaragd.Validation;

namespace Stein.ViewModels.Types
{
    public class GitHubRepositoryPathValidation
        : IValidation<string, bool>
    {
        private static readonly Regex RepositoryPathRegex = new Regex("^[a-z\\d](?:[a-z\\d]|-(?=[a-z\\d])){0,38}\\/[^\\/]+$", RegexOptions.Compiled);

        /// <inheritdoc />
        public bool Validate(string value)
        {
            return value != null && RepositoryPathRegex.IsMatch(value);
        }
    }
}
