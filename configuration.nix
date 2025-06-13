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
<<<<<<< zmxy5i-codex/set-up-custom-ace-jinwoo-os

  # Enable VMware guest integration
  virtualisation.vmware.guest.enable = true;
=======
>>>>>>> main
}
