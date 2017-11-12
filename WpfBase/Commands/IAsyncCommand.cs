using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfBase.Commands
{
    public interface IAsyncCommand
        : ICommand
    {
        Task ExecuteAsync(object parameter);
    }
}
