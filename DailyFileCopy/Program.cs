#define DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DailyFileCopy
{
    class Program
    {
        static DateTime baseDateTime;
        static string sourcePath;
        static string targetPath;
        static int copyCtr;
        static string userMsg;
        static string[] theFiles;


        static void Main(string[] args)
        {
            sourcePath = @"C:\Users\Gail\Desktop\FolderA";
            targetPath = @"C:\Users\Gail\Desktop\FolderB";
            baseDateTime = new DateTime();
            baseDateTime = (DateTime.Now.AddDays(-1));
            userMsg = "Something went wrong!";

            if (!LoadFiles())
            {
                Console.WriteLine(userMsg);
            }
            ProcessFiles();
            Console.WriteLine(userMsg);
# if DEBUG
            Console.ReadLine();
# endif
        }

        static bool LoadFiles()
        {
            try 
            {
                if (!System.IO.Directory.Exists(sourcePath))
                {
                    userMsg = $"Directory Path not found: {sourcePath}";
                    return false;
                }
                // get the filenames from the source directory / or return false
                theFiles = System.IO.Directory.GetFiles(sourcePath);
                if (theFiles.Count() == 0)
                {
                    userMsg = $"No files found in source directory: {sourcePath}";
                    return false;
                }
            }
            catch (Exception e)
            {
                userMsg = e.Message;
                return false;
            }

            return true;
        }

        static bool ProcessFiles()
        {  
            //iterate through the array of filenames and copy the files that meet the datetime criteria
            string fname,targetFile;
            copyCtr = 0;
            try
            {
                foreach (string file in theFiles)
                {
                    fname = System.IO.Path.GetFileName(file);
                    DateTime fileModDate = File.GetLastWriteTime(file);
                    int result = DateTime.Compare(fileModDate, baseDateTime);
                    if (result > 0) //fileModDate is later than baseDateTime
                    {
                        targetFile = System.IO.Path.Combine(targetPath, fname);
                        System.IO.File.Copy(file, targetFile, true);
                        copyCtr++;
                    }
                }
                userMsg = $"File Copy is complete. {copyCtr.ToString()} files copied.";
                return true;
            }
            catch (Exception e)
            {
                userMsg = e.Message;
                return false;
            }

        }
    }
}
