def main():
    path: str = r"E:\Portfolio\internal\002-code_of_chaos\0003-cs-aterra_engine\examples\workfloor\Workfloor-AterraCore.Plugin\Assets"

    header: str = ("using AterraCore.Contracts.Nexities.Assets;using AterraCore.Nexities.Components;"
                   "using JetBrains.Annotations;namespace Workfloor_AterraCore.Plugin.Assets;\n")

    with open(fr"{path}\PyGenComponents.cs", "w+") as f:
        f.write(header)

        for i in range(10, 100_001):
            f.write(f"""[Component("{i}")] [UsedImplicitly] public class TestComponent{i}(IAssetDto assetDto) : Component<IAssetDto>(assetDto);\n""")


if __name__ == '__main__':
    main()
