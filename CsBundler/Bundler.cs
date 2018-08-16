using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace CsBundler
{
    public class Bundler
    {
        private readonly Config _config;
        private readonly List<string> _foldersToIgnore = new List<string> {"bin","obj",".vs",".git"};
        private readonly Regex _matchUsings = new Regex("using [\\w\\d.]*;");

        public Bundler(Config config)
        {
            _config = config;
        }

        public void Bundle()
        {
            var bundleFilePath = _config.DirectoryPath + "\\" + _config.OutputFileName;
            if (File.Exists(bundleFilePath))
                File.Delete(bundleFilePath);

            var fileContents = ReadAllFiles().ToList();
            var bundleText = fileContents.Aggregate((current, next) => current + "\r\n" + next);
            bundleText = PullUsingsOnTop(bundleText);

            File.WriteAllText(bundleFilePath, bundleText);

            Console.WriteLine($"\r\nFound {fileContents.Count} files to bundle.\r\n\r\nOutput : {bundleFilePath}\r\n");
        }

        private IEnumerable<string> ReadAllFiles()
            => ReadAllFiles(new List<string>(), _config.DirectoryPath);

        private IEnumerable<string> ReadAllFiles(List<string> list, string directoryPath)
        {
            list.AddRange(Directory.GetFiles(directoryPath)
                .Where(i => i.EndsWith(".cs"))
                .Select(File.ReadAllText));

            foreach (var subDirectoryPath in Directory.GetDirectories(directoryPath).Where(i => !_foldersToIgnore.Any(i.EndsWith)))
                ReadAllFiles(list, subDirectoryPath);

            return list;
        }

        private string PullUsingsOnTop(string text)
        {
            var usings = new List<string>();
            foreach (Match match in _matchUsings.Matches(text))
                usings.Add(match.Value);
            
            return usings.Distinct().Aggregate((current,next) => current + "\r\n" + next) 
                   + _matchUsings.Replace(text, string.Empty);
        }
    }
}
