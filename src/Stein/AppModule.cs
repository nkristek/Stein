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
        public override void Load()
        {
            Bind<IDialogService>().To<WpfDialogService>().InSingletonScope();
            Bind<IThemeService>().To<WpfThemeService>().InSingletonScope();
            Bind<IProgressBarService>().To<WpfTaskbarService>().InSingletonScope();

            Bind<IInstallService>().To<InstallService>().InSingletonScope();
            Bind<IMsiService>().To<MsiService>().InSingletonScope();
            Bind<IProductService>().To<ProductService>().InSingletonScope();
            Bind<IInstallerFileBundleProviderFactory>().To<InstallerFileBundleProviderFactory>().InSingletonScope();

            Bind<IViewModelService>().To<ViewModelService>().InSingletonScope();
            Bind<IUriService>().To<UriService>().InSingletonScope();

            Bind<IConfigurationService>().To<ConfigurationService>().InSingletonScope();
            Bind<IConfigurationUpgradeManager>().To<ConfigurationUpgradeManager>().InSingletonScope();
            Bind<IConfigurationUpgraderFactory>().To<ConfigurationUpgraderFactory>().InSingletonScope();
            Bind<IConfigurationFactory>().To<ConfigurationFactory>().InSingletonScope();
        }
    }
}
