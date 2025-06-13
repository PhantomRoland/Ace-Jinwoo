{ config, pkgs, ... }:
{
  imports = [ ];

  athena = {
    enable = true;
    baseConfiguration = true;
    baseSoftware = true;
    baseLocale = true;
    homeManagerUser = "ace";
    desktopManager = "gnome";
    displayManager = "sddm";
    mainShell = "fish";
    terminal = "kitty";
    browser = "firefox";
    bootloader = "systemd";
    theme = "ace-jinwoo";
  };
}
