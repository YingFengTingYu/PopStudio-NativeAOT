using System.Diagnostics;

namespace PopStudio.ConsoleProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            YFLib.Bitmap.RegistPlatform<Platform.SkiaBitmap>();
            Console.WriteLine("输入rsb位置");
            string s = Console.ReadLine();
            Stopwatch w = new Stopwatch();
            w.Start();
            API.Unpack(s, $"{Path.GetDirectoryName(s)}/{Path.GetFileNameWithoutExtension(s)}_unpack", 1, true, false);
            w.Stop();
            Console.WriteLine(w.ElapsedMilliseconds / 1000m);
            Console.ReadLine();
        }
    }
}