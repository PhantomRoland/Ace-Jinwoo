{ lib, pkgs, config, ... }:
let
  # Ace-Jinwoo theme components. Adjust packages as desired.
  theme-components = {
    gtk-theme = "Tokyonight-Dark";
    icon-theme = "Papirus-Dark";
    cursor-theme = "Bibata-Modern-Classic";
    background = "arch-ascii.png"; # shipped by athena-blue-base
  };
  gtkTheme = "${theme-components.gtk-theme}";
  gtkIconTheme = "${theme-components.icon-theme}";
  gtkCursorTheme = "${theme-components.cursor-theme}";
in {
  config = lib.mkIf (config.athena.theme == "ace-jinwoo") {
    athena.theme-components = theme-components;

    environment.systemPackages = with pkgs; [
      (callPackage (athena + "/nixos/pkgs/themes/athena-blue-base/package.nix") { })
    ];

    home-manager.users.${config.athena.homeManagerUser} = { pkgs, ... }: {
      home.sessionVariables.GTK_THEME = gtkTheme;
      gtk = {
        enable = true;
        gtk3.extraConfig.gtk-decoration-layout = "menu:";
        theme = {
          name = gtkTheme;
          package = pkgs.tokyonight-gtk-theme.override {
            colorVariants = [ "dark" ];
            tweakVariants = [ "macos" ];
          };
        };
        iconTheme = {
          name = gtkIconTheme;
          package = pkgs.papirus-icon-theme;
        };
        cursorTheme = {
          name = gtkCursorTheme;
          package = pkgs.bibata-cursors;
        };
      };
      programs = {
        kitty.themeFile = "tokyo_night_storm";
        vscode = {
          profiles.default.extensions = with pkgs.vscode-extensions; [
            enkia.tokyo-night
          ];
          profiles.default.userSettings = { "workbench.colorTheme" = "Tokyo Night Storm"; };
        };
      };
    };
  };
}
