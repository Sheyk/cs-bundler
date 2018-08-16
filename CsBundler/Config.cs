using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            OutputFileName = args[1];
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
