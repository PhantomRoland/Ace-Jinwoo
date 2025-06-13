# Ace-Jinwoo OS


This repository provides a minimal Nix flake that builds an Athena OS based system customised with the **Ace‑Jinwoo** theme.

The configuration reuses [Athena OS](https://github.com/Athena-OS/athena-nix) modules and applies a custom theme defined in `modules/ace-jinwoo-theme`.

## Features

- Deepin desktop with the Ace‑Jinwoo look and feel
- VMware guest integration via `open-vm-tools`

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

VMware guest tools are enabled out of the box so the system integrates smoothly when run inside VMware products. The build sets `virtualisation.vmware.guest.enable = true` so the `open-vm-tools` service starts automatically.

## Customisation

Edit `configuration.nix` to adjust options such as desktop environment or packages. The Ace-Jinwoo theme itself can be tweaked in `modules/ace-jinwoo-theme/default.nix`.

**Ace-Jinwoo OS** is a minimal yet powerful NixOS flake that builds a customized Athena OS environment. It ships with a Deepin desktop themed in the Ace-Jinwoo style, and is tailored for clarity, beauty, and extensibility — whether you're running native or inside VMware.

---

## 🌱 Getting Started

### Prerequisites

- [Nix](https://nixos.org/download.html) must be installed with flakes enabled.
- A system with at least **4 GB RAM** and **20 GB disk** recommended.
- (Optional) [VMware Workstation](https://www.vmware.com/products/workstation-pro.html) for VM deployment.

---

### 🛠 Install Instructions

```bash
# Clone the repo
git clone https://github.com/PhantomRoland/Ace-Jinwoo ace-jinwoo
cd ace-jinwoo

# Switch to the system using nixos-rebuild
sudo nixos-rebuild switch --flake .#acejinwoo --impure

