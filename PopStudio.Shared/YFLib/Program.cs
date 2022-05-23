using System.Runtime.InteropServices;
using YFLib.Texture;

namespace YFLib
{
    internal class Program
    {
        static void MainProgram(string[] args)
        {
            while (true)
            {
                //try
                {
                    string[] m_args = GetArguments(args, 3, new string[3] { "输入文件位置", "输入运行模式", "输入纹理格式" });
                    string filePath = m_args[0];
                    int mode = int.Parse(m_args[1]);
                    TextureFormat format = (TextureFormat)int.Parse(m_args[2]);
                    switch (mode)
                    {
                        case 1:
                            using (BinaryStream bs = new BinaryStream(filePath, FileMode.Open))
                            {
                                TextureInfo info = new TextureInfo();
                                info.Width = bs.ReadInt32();
                                info.Height = bs.ReadInt32();
                                info.Format = (TextureFormat)bs.ReadInt32();
                                info.Data = bs.ReadBytes((int)(bs.Length - 12));
                                using (Bitmap map = Bitmap.DecodeTexture(info))
                                {
                                    map.Save(filePath + ".png");
                                }
                            }
                            break;
                        case 2:
                            using (Bitmap map = Bitmap.Create(filePath))
                            {
                                TextureInfo info = map.EncodeAsTexture(format);
                                byte[] b = info.Data;
                                using (BinaryStream bs = new BinaryStream(filePath + ".ptx", FileMode.Create))
                                {
                                    bs.WriteInt32(info.Width);
                                    bs.WriteInt32(info.Height);
                                    bs.WriteInt32((int)info.Format);
                                    bs.Write(b, 0, b.Length);
                                }
                            }
                            break;
                        case 3:
                            TestSpeed();
                            break;
                    }
                }
                //catch (Exception ex)
                //{
                //    Console.WriteLine(ex.Message);
                //}
                args = null;
            }
        }

        static void TestSpeed<T>() where T : Bitmap, new()
        {
            Bitmap.RegistPlatform<T>();
            TestSpeed();
        }

        public static void TestSpeed(string v = null)
        {
            Console.WriteLine("请输入ptx路径，文件夹内必须只有ptx");
            string filepath = v ?? Console.ReadLine();
            string[] f = Directory.GetFiles(filepath);
            var st = new System.Diagnostics.Stopwatch();
            st.Start();
            foreach (string s in f)
            {
                if (Path.GetExtension(s).ToLower() != ".ptx") continue;
                using (BinaryStream bs = new BinaryStream(s, FileMode.Open))
                {
                    TextureInfo info = new TextureInfo();
                    string magic = bs.ReadString(4);
                    bool bigendian = magic == "ptx1";
                    bs.Endian = bigendian ? Endian.Big : Endian.Small;
                    bs.ReadInt32();
                    info.Width = bs.ReadInt32();
                    info.Height = bs.ReadInt32();
                    bs.ReadInt32();
                    info.Format = GetFormatFromRsbPtx(bs.ReadInt32(), bigendian);
                    bs.ReadInt64();
                    info.Data = bs.ReadBytes((int)(bs.Length - 12));
                    using (Bitmap map = Bitmap.DecodeTexture(info))
                    {
                        map.Save(s + ".png");
                    }
                }
            }
            st.Stop();
            Console.WriteLine(st.ElapsedMilliseconds / 1000d);
        }

        static string[] GetArguments(string[] raw, int num, string[] tishi)
        {
            raw ??= Array.Empty<string>();
            string[] args = new string[num];
            if (num > raw.Length)
            {
                Array.Copy(raw, 0, args, 0, raw.Length);
                for (int i = raw.Length; i < num; i++)
                {
                    if (args[i] == null)
                    {
                        Console.WriteLine(tishi[i]);
                        string[] g = Console.ReadLine().Split(' ');
                        if (g.Length + i >= num)
                        {
                            Array.Copy(g, 0, args, i, num - i);
                            break;
                        }
                        else
                        {
                            Array.Copy(g, 0, args, i, g.Length);
                        }
                    }
                }
            }
            else
            {
                Array.Copy(raw, 0, args, 0, num);
            }
            return args;
        }

        static TextureFormat GetFormatFromRsbPtx(int v, bool bigendian) => bigendian ? v switch
        {
            0 => TextureFormat.BGRA8888_Padding256,
            5 => TextureFormat.RGBA_DXT5,
            _ => throw new NotImplementedException()
        } : v switch
        {
            0 => TextureFormat.ABGR8888,
            1 => TextureFormat.RGBA4444,
            2 => TextureFormat.RGB565,
            3 => TextureFormat.RGBA5551,
            21 => TextureFormat.RGBA4444_Block32,
            22 => TextureFormat.RGB565_Block32,
            23 => TextureFormat.RGBA5551_Block32,
            30 => TextureFormat.RGBA_PVRTCI_4BPP,
            147 => TextureFormat.RGB_ETC1_A8,
            _ => throw new NotImplementedException()
        };
    }
}