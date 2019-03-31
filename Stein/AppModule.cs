using System.Windows;
using Ninject.Modules;
using Stein.Presentation;
using Stein.Services.Configuration;
using Stein.Services.Configuration.Upgrades;
using Stein.Services.InstallerFiles.Base;
using Stein.Services.InstallService;
using Stein.Services.MsiService;
using Stein.Services.ProductService;
using Stein.ViewModels.Services;
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
            Bind<IThemeService>().To<WpfThemeService>().InSingletonScope();
            Bind<IProgressBarService>().To<WpfTaskbarService>().InSingletonScope();

            Bind<IInstallService>().To<InstallService>().InSingletonScope();
            Bind<IMsiService>().To<MsiService>().InSingletonScope();
            Bind<IProductService>().To<ProductService>().InSingletonScope();
            Bind<IInstallerFileBundleProviderFactory>().To<InstallerFileBundleProviderFactory>().InSingletonScope();

            Bind<IViewModelService>().To<ViewModelService>().InSingletonScope();

            Bind<IConfigurationService>().To<ConfigurationService>().InSingletonScope();
            Bind<IConfigurationUpgradeManager>().To<ConfigurationUpgradeManager>().InSingletonScope();
            Bind<IConfigurationUpgraderFactory>().To<ConfigurationUpgraderFactory>().InSingletonScope();
            Bind<IConfigurationFactory>().To<ConfigurationFactory>().InSingletonScope();
        }
    }
}
