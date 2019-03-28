namespace Stein.Views
{
    /// <summary>
    /// This class ensures, that DLL's which are only used in XAML files like converter libraries are copied to the bin folder
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    internal class Ressources
    {
#pragma warning disable 169
        // ReSharper disable once InconsistentNaming
        private readonly NKristek.Wpf.Converters.BoolToInverseBoolConverter _NKristekWpfConverter;
#pragma warning restore 169
    }
}
