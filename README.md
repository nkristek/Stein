# Stein - Silent Temp Installer

![.NET Framework version: >= 4.7.2](https://img.shields.io/badge/.NET%20Framework-%3E%3D%204.7.2-green.svg)
[![GitHub license](https://img.shields.io/github/license/nkristek/Stein.svg)](https://github.com/nkristek/Stein/blob/master/LICENSE)

This application makes it easier to install, uninstall or reinstall multiple MSI-installers. 
It was built for an environment with regular builds of an application.

**Please note:** This application needs administrator privileges to operate on installers without UI.

![screenshot_light](images/Screenshot.PNG)

Support for dark mode

![screenshot_dark](images/Screenshot_dark.PNG)

## Available options

### No UI during install

It will start installers with the "/QN" option which means it will be installed in the background with default options.
When this option is set, the [Disable reboot after installation](#disable-reboot-after-installation) option should be strongly considered especially when operating with multiple installers at once.

### Disable reboot after installation

It will start installers with the "/norestart" option which means the installer should not automatically reboot the system after installation. 

### Filter duplicate installers

If enabled, only one installer per application will be installed. This is usefull, when there are multiple installers which install the same application but different languages.

### Install log

It will start installers with the "/L*V LOGPATH" option which means the installer should log all operations.
You can optionally set a limit on how many log files should be kept. After an operation, the oldest log files will be deleted.

### Additional arguments

When performing a custom operation you can define additional arguments for each installer. 

## Contribution

If you find a bug feel free to open an issue. Contributions are also appreciated.

**Please note**: 
You have to have the [Wix Toolset](http://wixtoolset.org) installed in order to compile the [Stein.Services](../blob/master/Stein.Services) project. The version currently used to build this application is [3.11.1](http://wixtoolset.org/releases/v3.11.1/stable).

## Other

Most of the icons used by this application are from [fontawesome](https://fontawesome.com) ([licence](https://fontawesome.com/license)) and are modified to be used in a WPF environment.
