using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NKristek.Smaragd.Validation;

namespace Stein.ViewModels.Types
{
    public class GitHubRepositoryPathValidation
        : Validation<string>
    {
        private static readonly Regex RepositoryPathRegex = new Regex("^[a-z\\d](?:[a-z\\d]|-(?=[a-z\\d])){0,38}\\/[^\\/]+$", RegexOptions.Compiled);

        private readonly string _errorMessage;

        public GitHubRepositoryPathValidation(string errorMessage)
        {
            _errorMessage = errorMessage;
        }

        /// <inheritdoc />
        public override bool IsValid(string value, out string errorMessage)
        {
            if (value != null && RepositoryPathRegex.IsMatch(value))
            {
                errorMessage = null;
                return true;
            }

            errorMessage = _errorMessage;
            return false;
        }
    }
}
