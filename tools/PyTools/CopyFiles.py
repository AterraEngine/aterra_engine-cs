import shutil


def main():
    path_source: str = r"E:\Portfolio\internal\002-code_of_chaos\0003-cs-aterra_engine\examples\workfloor\Workfloor-AterraCore\bin\Debug\net8.0\plugins\Workfloor-AterraCore.Plugin.zip"
    for i in range(11):
        shutil.copy(path_source,
                    fr"E:\Portfolio\internal\002-code_of_chaos\0003-cs-aterra_engine\examples\workfloor\Workfloor-AterraCore\bin\Debug\net8.0\plugins\Workfloor-AterraCore.Plugin{i:02}.zip")


if __name__ == '__main__':
    main()
