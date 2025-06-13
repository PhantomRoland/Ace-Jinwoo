{ config, pkgs, ... }:
{
  imports = [ ];

  athena = {
    enable = true;
    baseConfiguration = true;
    baseSoftware = true;
    baseLocale = true;
    homeManagerUser = "ace";
    desktopManager = "deepin";
    displayManager = "sddm";
    mainShell = "fish";
    terminal = "kitty";
    browser = "firefox";
    bootloader = "systemd";
    theme = "ace-jinwoo";
  };


  # Enable VMware guest integration
  virtualisation.vmware.guest.enable = true;


  # Enable VMware guest integration
  virtualisation.vmware.guest.enable = true;

}
