using System;
using System.IO;
using System.Linq;

namespace CsBundler
{
    public class Config
    {
        public string DirectoryPath { get; }
        public string OutputFileName { get; }

        public Config(string[] args)
        {
            if(!IsValid(args))
                throw new ArgumentException(nameof(args));

            DirectoryPath = args[0];
            OutputFileName = args.Length > 1 ? args[1] : "bundle.cs";

            if (!OutputFileName.EndsWith(".cs"))
                OutputFileName = OutputFileName + ".cs";
            if (DirectoryPath.EndsWith("\\") || DirectoryPath.EndsWith("/"))
                DirectoryPath = DirectoryPath.TrimEnd('/','\\').Replace("/", "\\");
        }

        public static bool IsValid(string[] args, out string errorMessage)
        {
            errorMessage = string.Empty;
            var hasError = true;

            if (args == null || args.Any())
                errorMessage = "Please give a directory path. Ex : csbundler.exe \"C:\\RepositoryToBundle\"";
            else if (args.Length > 2)
                errorMessage = "Too many input parameters.";
            else if (!Directory.Exists(args[0]))
                errorMessage = "The given directory doesn't exist";
            else
                hasError = false;

            return hasError;
        }

        public static bool IsValid(string[] args)
            => IsValid(args, out string unused);
    }
}
