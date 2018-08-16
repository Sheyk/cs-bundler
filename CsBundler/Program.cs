using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsBundler
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!Config.IsValid(args, out string errorMessage))
            {
                Console.WriteLine(errorMessage);
                return;
            }

            var config = new Config(args);
            var bundler = new Bundler(config);

            bundler.Bundle();
        }
    }
}
