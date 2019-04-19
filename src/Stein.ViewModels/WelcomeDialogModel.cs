using NKristek.Smaragd.ViewModels;
using Stein.Localization;

namespace Stein.ViewModels
{
    public class WelcomeDialogModel
        : DialogModel
    {
        /// <inheritdoc />
        public override string Title => Strings.Welcome;

        public string Caption => Strings.WelcomeCaption;
        
        public string Message => Strings.WelcomeMessage;
    }
}
