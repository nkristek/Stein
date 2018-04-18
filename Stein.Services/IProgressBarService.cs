using Stein.Services.Types;

namespace Stein.Services
{
    public interface IProgressBarService
    {
        void SetState(ProgressBarState state);
        void SetProgress(double progress);
    }
}
