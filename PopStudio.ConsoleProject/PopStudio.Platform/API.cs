using System.Text;

namespace PopStudio.Plugin
{
    public static partial class API
    {
        public static partial void Print(params object[] os)
        {
            StringBuilder str = new StringBuilder();
            if (os.Length != 0)
            {
                string nil = "nil";
                for (int i = 0; i < os.Length; i++)
                {
                    str.Append((os[i]?.ToString()) ?? nil);
                    str.Append(' ');
                }
                str.Remove(str.Length - 1, 1);
            }
            Console.WriteLine(str);
        }

        public static partial bool? Alert(string text, string title, bool ask)
        {
            Console.WriteLine(title);
            Console.WriteLine(text);
            if (ask)
            {
                Console.WriteLine("按下y键确定，按下其他键取消");
                return Console.ReadKey().KeyChar.ToString().ToLower() == "y";
            }
            else
            {
                Console.WriteLine("按下任意键继续");
                Console.ReadKey();
                return null;
            }
        }

        public static partial string Prompt(string text, string title, string defaulttext)
        {
            Console.WriteLine(title);
            Console.WriteLine(text);
            return Console.ReadLine();
        }

        public static partial string Sheet(string title, params string[] items)
        {
            return null;
        }

        public static partial string ChooseFolder()
        {
            Console.WriteLine("请输入文件夹名");
            return Console.ReadLine();
        }

        public static partial string ChooseOpenFile()
        {
            Console.WriteLine("请输入文件名");
            return Console.ReadLine();
        }

        public static partial string ChooseSaveFile()
        {
            Console.WriteLine("请输入文件名");
            return Console.ReadLine();
        }

        public static partial void OpenUrl(string url)
        {

        }
    }
}
