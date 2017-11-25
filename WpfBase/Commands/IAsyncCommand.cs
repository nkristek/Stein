using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfBase.Commands
{
    /// <summary>
    /// Specifies a ICommand that additionally implements a ExecuteAsync() method
    /// </summary>
    public interface IAsyncCommand
        : ICommand
    {
        Task ExecuteAsync(object parameter);
    }
}
