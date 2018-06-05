using System.Windows;
using Ninject.Modules;
using Stein.Presentation;
using Stein.Services;
using Stein.ViewModels;
using Stein.Views.Services;

namespace Stein
{
    internal class AppModule
        : NinjectModule
    {
        private readonly Window _rootWindow;

        internal AppModule(Window rootWindow)
        {
            _rootWindow = rootWindow;
        }

        public override void Load()
        {
            Bind<Window>().ToConstant(_rootWindow);
            Bind<IDialogService>().To<WpfDialogService>().InSingletonScope();
            Bind<IConfigurationService>().To<ConfigurationService>().InSingletonScope();
            Bind<IInstallService>().To<InstallService>().InSingletonScope();
            Bind<IThemeService>().To<WpfThemeService>().InSingletonScope();
            Bind<IProgressBarService>().To<WpfTaskbarService>().InSingletonScope();
            Bind<IMsiService>().To<MsiService>();
            Bind<IViewModelService>().To<ViewModelService>().InSingletonScope();
        }
    }
}
