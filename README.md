# Ace-Jinwoo OS

This repository provides a minimal Nix flake that builds an Athena OS based system customised with the **Ace‑Jinwoo** theme.

The configuration reuses [Athena OS](https://github.com/Athena-OS/athena-nix) modules and applies a custom theme defined in `modules/ace-jinwoo-theme`.

## Getting Started

1. Ensure [Nix](https://nixos.org/) is installed.
2. Clone this repository:
   ```bash
   git clone <this repo url> ace-jinwoo
   cd ace-jinwoo
   ```
3. Build or install the system using `nixos-rebuild`:
   ```bash
   sudo nixos-rebuild switch --flake .#acejinwoo --impure
   ```
   The `--impure` flag allows using your local `hardware-configuration.nix`.

The default user configured is `ace` with the Ace-Jinwoo themed Deepin desktop.

## Customisation

Edit `configuration.nix` to adjust options such as desktop environment or packages. The Ace-Jinwoo theme itself can be tweaked in `modules/ace-jinwoo-theme/default.nix`.
