using Ninject.Modules;
using Stein.Common.Configuration;
using Stein.Common.InstallerFiles;
using Stein.Common.InstallService;
using Stein.Common.IOService;
using Stein.Common.MsiService;
using Stein.Common.ProductService;
using Stein.Common.UpdateService;
using Stein.Presentation;
using Stein.Services.Configuration;
using Stein.Services.Configuration.Upgrades;
using Stein.Services.InstallerFiles.Base;
using Stein.Services.InstallService;
using Stein.Services.IOService;
using Stein.Services.MsiService;
using Stein.Services.ProductService;
using Stein.Services.UpdateService;
using Stein.ViewModels.Services;
using Stein.Views.Services;

namespace Stein
{
    internal class AppModule
        : NinjectModule
    {
        public override void Load()
        {
            // Views
            Bind<IDialogService>().To<WpfDialogService>().InSingletonScope();
            Bind<IThemeService>().To<WpfThemeService>().InSingletonScope();
            Bind<IProgressBarService>().To<WpfTaskbarService>().InSingletonScope();
            Bind<IUriService>().To<WpfUriService>().InSingletonScope();
            Bind<IClipboardService>().To<WpfClipboardService>().InSingletonScope();
            Bind<INotificationService>().To<WpfNotificationService>().InSingletonScope();

            // Services
            Bind<IConfigurationService>().To<ConfigurationService>().InSingletonScope();
            Bind<IConfigurationUpgradeManager>().To<ConfigurationUpgradeManager>().InSingletonScope();
            Bind<IConfigurationUpgraderFactory>().To<ConfigurationUpgraderFactory>().InSingletonScope();
            Bind<IConfigurationFactory>().To<ConfigurationFactory>().InSingletonScope();
            Bind<IUpdateService>().To<UpdateService>().InSingletonScope();
            Bind<IInstallService>().To<InstallService>().InSingletonScope();
            Bind<IMsiService>().To<MsiService>().InSingletonScope();
            Bind<IProductService>().To<ProductService>().InSingletonScope();
            Bind<IInstallerFileBundleProviderFactory>().To<InstallerFileBundleProviderFactory>().InSingletonScope();
            Bind<IIOService>().To<IOService>().InSingletonScope();

            // ViewModels
            Bind<IViewModelService>().To<ViewModelService>().InSingletonScope();
        }
    }
}
