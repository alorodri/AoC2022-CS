using static AdventOfCode2022.utils.Utils;

namespace AdventOfCode2022.day7
{
    [ProblemDay("day7")]
    [ReturnType(typeof(int), typeof(int))]
    [TestResult(95437, 24933642)]
    internal class Solver : Problem
    {

        enum Commands
        {
            ls,
            cd
        }

        class Folder
        {
            public string name;
            public string path;
            public Folder? parent = null;
            public List<Folder> folders = new();
            public List<File> files = new();
            public int? level;
            public bool sizeChecked = false;
            public int totalSize = 0;

            public Folder(string name)
            {
                this.name = name;
            }

            public override string ToString()
            {
                return path;
            }
        }

        class File
        {
            public string name;
            public int size;
        }

        public override T Solve<T>(ProblemChoice pc)
        {
            List<string> lines = ReadLinesAs<string>();
            Dictionary<string, Folder?> folders = new();
            Folder? currentFolder = null;
            int currentLevel = 0;
            foreach (var line in lines)
            {
                if (IsCommand(line, out string? command, out string commandText))
                {
                    Enum.TryParse<Commands>(command, out Commands parsedCommand);
                    switch (parsedCommand)
                    {
                        case Commands.ls:
                            break;
                        case Commands.cd:
                            if (commandText == "..")
                            {
                                if (currentFolder == null) throw new Exception("Folder null here");
                                if (currentFolder.parent != null)
                                {
                                    currentLevel--;
                                    currentFolder = currentFolder.parent;
                                    currentFolder.level = currentLevel;
                                }
                            } else
                            {
                                currentLevel = commandText == "/" ? 0 : currentLevel + 1;
                                string folderName = commandText;
                                Folder? lastFolder = currentFolder;
                                string currentPath = currentLevel == 0 ? folderName : lastFolder.path + "-" + folderName;
                                folders.TryGetValue(currentPath, out currentFolder);
                                if (currentFolder == null)
                                {
                                    Folder fnew = new(folderName);
                                    if (folderName != "/")
                                    {
                                        fnew.parent = lastFolder;
                                        fnew.parent?.folders.Add(fnew);
                                    }
                                    fnew.path = currentPath;
                                    currentFolder = fnew;
                                    folders.Add(currentPath, fnew);
                                }
                                currentFolder.level = currentLevel;
                            }
                            break;
                        default:
                            throw new Exception($"Comando no controlado: {command}");
                    }
                } else
                {
                    if (commandText.StartsWith("dir"))
                    {
                        // dir
                        if (currentFolder == null) throw new Exception("CurrentFolder no debería ser nulo aquí");
                        var dirname = commandText[4..];
                        string dirPath = currentFolder.path + "-" + dirname;
                        if (folders.ContainsKey(dirPath)) continue;
                        Folder newdir = new(dirname)
                        {
                            parent = currentFolder,
                            level = currentLevel + 1,
                            path = dirPath
                        };
                        currentFolder.folders.Add(newdir);
                        folders.Add(dirPath, newdir);
                    } else
                    {
                        // file
                        if (currentFolder == null) throw new Exception("CurrentFolder no debería ser nulo aquí");
                        var (filesize, ptr) = GetUntilSpaceAs<int>(commandText, 0);
                        var (filename, _) = GetUntilSpaceAs<string>(commandText, ptr);
                        currentFolder.files.Add(new() { name = filename, size = filesize });
                        currentFolder.totalSize += filesize;
                    }
                }
            }

            int result = 0;
            const int SPACE_AVAILABLE = 70000000;
            const int UNUSED_SPACE_REQUIRED = 30000000;
            int totalDiskSpaceUsed = ReturnSizeRecursively(folders.GetValueOrDefault("/"), ref result, true);
            RestartCheckedFolders(folders.Values);
            int spaceRequired = Math.Abs(SPACE_AVAILABLE - UNUSED_SPACE_REQUIRED - totalDiskSpaceUsed);
            int biggerFileSizeRequired = 0;
            result = 0;
            foreach (var folder in folders.Values)
            {
                if (folder == null) throw new Exception("Folder null here");
                int folderSize = ReturnSizeRecursively(folder, ref result, false);
                if (biggerFileSizeRequired == 0 || folderSize >= spaceRequired && folderSize < biggerFileSizeRequired) biggerFileSizeRequired = folderSize;
            }

            RestartCheckedFolders(folders.Values);

            return pc == ProblemChoice.A ? Cast<T>(result) : Cast<T>(biggerFileSizeRequired);
        }

        private static void RestartCheckedFolders(Dictionary<string, Folder?>.ValueCollection folders) { 
            foreach (var folder in folders)
            {
                folder.sizeChecked = false;
            }
        }

        private static int ReturnSizeRecursively(Folder f, ref int result, bool oneDir)
        {
            int subdirsTotalSize = 0;
            foreach(var f2 in f.folders)
            {
                subdirsTotalSize += ReturnSizeRecursively(f2, ref result, oneDir);
            }

            int totalSize = subdirsTotalSize + f.totalSize;
            if (!f.sizeChecked && (totalSize <= 100000 || oneDir)) result += totalSize;
            f.sizeChecked = true;
            return totalSize;
        }

        private static bool IsCommand(string line, out string? command, out string commandText)
        {
            if (line.StartsWith('$'))
            {
                (command, int ptr) = GetUntilSpaceAs<string>(line, 1);
                commandText = line[ptr..];
                return true;
            }

            command = null;
            commandText = line;
            return false;
        }
    }
}
