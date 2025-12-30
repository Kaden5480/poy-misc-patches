let
    pkgs = import <nixpkgs> {};
in
    pkgs.mkShell {
        buildInputs = with pkgs; [
            bash
            dotnet-sdk
            git
            zip
        ];

        shellHook = ''
        ./build.sh
        '';
    }
