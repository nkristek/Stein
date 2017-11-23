# Stein - Silent Temp Installer

This C# MVVM application makes it easier to install, uninstall or reinstall multiple installers at the same time.

It bundles these installers by folder and by culture (ProductLanguage property on the MSI-file).
The supported folder structure is:

- selected folder
  - tmp01
    - installer1_enUS.msi
    - installer1_deDE.msi
    - installer2_enUS.msi
    - installer2_deDE.msi
  - tmp02
    - installer1_enUS.msi
    - installer1_deDE.msi
    - installer2_enUS.msi
    - installer2_deDE.msi
   
It will generate 4 installer bundles: 
- tmp01 - enUS 
  - tmp01/installer1_enUS.msi
  - tmp01/installer2_enUS.msi
- tmp01 - deDE
  - tmp01/installer1_deDE.msi
  - tmp01/installer2_deDE.msi
- tmp02 - enUS
  - tmp02/installer1_enUS.msi
  - tmp02/installer2_enUS.msi
- tmp02 - deDE
  - tmp02/installer1_deDE.msi
  - tmp02/installer2_deDE.msi
