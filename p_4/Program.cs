using Microsoft.Win32;
using System;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        static void Main()
        {
            var subKeyNames = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run");
            string[] drivers = GetRootFolders();
            Console.WriteLine("Назва файлу або папки: ");
            string targetName = Console.ReadLine();
            foreach (string driver in drivers)
            {
                Console.WriteLine("Папка: " + driver);
                Console.WriteLine("--------------------------------------");
                Console.WriteLine("**********************************");

                Action<string> searchAction = new Action<string>(folder =>
                {
                    Search(folder, targetName);
                });

                FolderTraversal(driver, searchAction);

                Console.WriteLine("--------------------------------------");
                Console.WriteLine();
            }
        }

        static string[] GetRootFolders()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            string[] rootFolders = new string[drives.Length];

            for (int i = 0; i < drives.Length; i++)
            {
                rootFolders[i] = drives[i].RootDirectory.FullName;
            }

            return rootFolders;
        }

        static void FolderTraversal(string folder, Action<string> someAction)
        {
            try
            {
                foreach (string file in Directory.GetFiles(folder))
                {
                    someAction(file);
                }

                string[] subdirectories;
                try
                {
                    subdirectories = Directory.GetDirectories(folder);
                }
                catch (UnauthorizedAccessException)
                {
                    return;
                }

                foreach (string subdirectory in subdirectories)
                {
                    someAction(subdirectory);
                    FolderTraversal(subdirectory, someAction);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка: " + ex.Message);
            }
        }

        static void Search(string path, string targetName)
        {
            if (Path.GetFileName(path).Contains(targetName))
            {
                Console.WriteLine(path);
                Console.WriteLine("**********************************");
            }
        }
    }
}
