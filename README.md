# Ace-Jinwoo OS

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
