{
  description = "Ace-Jinwoo customized Athena OS configuration";

  inputs = {
    nixpkgs.url = "github:NixOS/nixpkgs/nixos-unstable";
    athena.url = "github:Athena-OS/athena-nix";
    home-manager = {
      url = "github:nix-community/home-manager";
      inputs.nixpkgs.follows = "nixpkgs";
    };
  };

  outputs = { self, nixpkgs, athena, home-manager }:
    let
      system = "x86_64-linux";
    in {
      nixosConfigurations = {
        acejinwoo = nixpkgs.lib.nixosSystem {
          inherit system;
          modules = [
            athena.nixosModules.athena
            home-manager.nixosModules.home-manager
            ./configuration.nix
            ./modules/ace-jinwoo-theme/default.nix
          ];
          specialArgs = { inherit athena; };
        };
      };
    };
}
