using System;
using Client.NiceHash;

namespace example {
    class Program {
        static void Main(string[] args) {
            var instance = new NiceHashClient();

            instance.Test();

            Console.WriteLine("Done.");
        }
    }
}
