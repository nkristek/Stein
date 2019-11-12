# Stein - Silent Temp Installer

This application makes it easier to install, uninstall or reinstall multiple MSI-installers. 
It was built for an environment with regular builds of an application.

**Please note:** This application needs administrator privileges to operate on installers without UI.

Light theme | Dark theme
:----------:|:----------:
![](images/Screenshot.PNG) | ![](images/Screenshot_dark.PNG)

## Available options

### No UI during install

It will start the installation with the "/QN" option which means it will be installed in the background with default options.
When this option is set, the [Disable reboot after installation](#disable-reboot-after-installation) option should be strongly considered especially when operating with multiple installers at once.

### Disable reboot after installation

It will start the installation with the "/norestart" option which means the installer should not automatically reboot the system after installation. 

### Filter duplicate installers

If enabled, only one installer per application will be installed. This is usefull, when there are multiple installers which install the same application but different languages.

### Install log

It will start the installation with the "/L*V LOGPATH" option which means the installer should log all operations.
You can optionally set a limit on how many log files should be kept. After an operation, the oldest log files will be deleted.

### Additional arguments

When performing a custom operation you can define additional arguments for each installer. 

## Contribution

If you find a bug feel free to open an issue. Contributions are also appreciated. Special thanks to [benruehl](https://github.com/benruehl/) for all of his UI related work and to [heckenr](https://github.com/heckenr) for his suggestion to name this application Stein (**S**ilent **Te**mp **In**staller).

**Please note**: 
You have to have the [Wix Toolset](http://wixtoolset.org) installed in order to compile the [Stein.Services](../blob/master/Stein.Services) project. The version currently used to build this application is [3.11.1](http://wixtoolset.org/releases/v3.11.1/stable).
