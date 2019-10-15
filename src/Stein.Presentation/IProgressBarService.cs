namespace Stein.Presentation
{
    public interface IProgressBarService
    {
        void SetState(ProgressBarState state);

        void SetProgress(double progress);
    }
}
