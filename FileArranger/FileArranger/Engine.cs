namespace FileArranger
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Text;
    using System.Threading;

    public class Engine
    {
        private static string path = ConfigurationManager.AppSettings["DirectoryForListening"];
        public void Run()
        {
            CreateSubDirectories();

            FirstArrange();

            ListenToChangesAndArrangeIfNeeded();
        }

        private void CreateSubDirectories()
        {
            DirectoryInfo directory = new DirectoryInfo(path);

            directory.CreateSubdirectory("ExeFiles");
            directory.CreateSubdirectory("Pictures");
            directory.CreateSubdirectory("Torrents");
            directory.CreateSubdirectory("PDF Files");
            directory.CreateSubdirectory("Word Files");
            directory.CreateSubdirectory("Excel Files");
            directory.CreateSubdirectory("MSI Files");
            directory.CreateSubdirectory("ZIP Files");
        }

        private void ListenToChangesAndArrangeIfNeeded()
        {
            using (FileSystemWatcher watcher = new FileSystemWatcher())
            {
                watcher.Path = path;

                watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.LastAccess;

                watcher.Filter = "*.torrent";

                watcher.Created += Watcher_Created;

                watcher.EnableRaisingEvents = true;

                Console.WriteLine("Press 'q' to quit the sample.");

                while (Console.Read() != 'q') ;
            }
        }
        private static void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            FileInfo file = new FileInfo(e.FullPath);

            Thread.Sleep(2000);

            file.MoveTo($"{path}\\Torrents\\{file.Name}", true);
        }
        private void FirstArrange()
        {
            DirectoryInfo directory = new DirectoryInfo(path);

            var fileList = directory.GetFiles("*.*");

            foreach (var file in fileList)
            {
                switch (file.Extension.ToLower())
                {
                    case ".exe":
                        file.MoveTo($"{path}\\ExeFiles\\{file.Name}", true);
                        break;
                    case ".pdf":
                        file.MoveTo($"{path}\\PDF Files\\{file.Name}", true);
                        break;
                    case ".msi":
                        file.MoveTo($"{path}\\MSI Files\\{file.Name}", true);
                        break;
                    case ".docx":
                    case ".doc":
                        file.MoveTo($"{path}\\WORD Files\\{file.Name}", true);
                        break;
                    case ".torrent":
                        file.MoveTo($"{path}\\Torrents\\{file.Name}", true);
                        break;
                    case ".zip":
                        file.MoveTo($"{path}\\ZIP files\\{file.Name}", true);
                        break;
                    case ".jpg":
                        file.MoveTo($"{path}\\Pictures\\{file.Name}", true);
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
