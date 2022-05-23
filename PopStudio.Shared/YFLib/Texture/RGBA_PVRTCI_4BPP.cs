using System.Runtime.CompilerServices;

namespace YFLib.Texture
{
    internal class RGBA_PVRTCI_4BPP
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int GetSize(int width, int height) => (width * height) >> 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int GetNextPowerOf2(int n)
        {
            int i = 1;
            while (i < n)
            {
                i <<= 1;
            }
            return i;
        }

        public static Bitmap Decode(TextureInfo info)
        {
            int rawwidth = info.Width;
            int rawheight = info.Height;
            bool CutImage = false;
            int width = rawwidth;
            int height = rawheight;
            if ((width & (width - 1)) != 0)
            {
                CutImage = true;
                width = GetNextPowerOf2(width);
            }
            if ((height & (height - 1)) != 0)
            {
                CutImage = true;
                height = GetNextPowerOf2(height);
            }
            if (width != height)
            {
                width = width > height ? width : height;
                height = width;
                CutImage = true;
            }
            if (info.Data.Length < GetSize(width, height)) return null;
            if (CutImage)
            {
                using (Bitmap temp = Bitmap.Create(width, height))
                {
                    IntPtr ptr = temp.GetPixels();
                    unsafe
                    {
                        fixed (byte* t = info.Data)
                        {
                            CompressionTextureCoder.PVRTC.DecodePVRTCI_4BPP((byte*)ptr.ToPointer(), width, t);
                        }
                    }
                    return temp.Cut(0, 0, rawwidth, rawheight);
                }
            }
            else
            {
                Bitmap ans = Bitmap.Create(width, height);
                IntPtr ptr = ans.GetPixels();
                unsafe
                {
                    fixed (byte* t = info.Data)
                    {
                        CompressionTextureCoder.PVRTC.DecodePVRTCI_4BPP((byte*)ptr.ToPointer(), width, t);
                    }
                }
                return ans;
            }
        }

        public static TextureInfo Encode(Bitmap bitmap)
        {
           
            int rawwidth = bitmap.Width;
            int rawheight = bitmap.Height;
            bool CutImage = false;
            int width = rawwidth;
            int height = rawheight;
            if ((width & (width - 1)) != 0)
            {
                CutImage = true;
                width = GetNextPowerOf2(width);
            }
            if ((height & (height - 1)) != 0)
            {
                CutImage = true;
                height = GetNextPowerOf2(height);
            }
            if (width != height)
            {
                width = width > height ? width : height;
                height = width;
                CutImage = true;
            }
            byte[] ans = new byte[GetSize(width, height)];
            Console.WriteLine($"{width} {height} {CutImage}");
            if (CutImage)
            {
                using (Bitmap ansmap = bitmap.Cut(0, 0, width, height))
                {
                    IntPtr ptr = ansmap.GetPixels();
                    unsafe
                    {
                        fixed (byte* t = ans)
                        {
                            CompressionTextureCoder.PVRTC.EncodePVRTCI_4BPP_RGBA((byte*)ptr.ToPointer(), width, t);
                        }
                    }
                }
            }
            else
            {
                IntPtr ptr = bitmap.GetPixels();
                unsafe
                {
                    fixed (byte* t = ans)
                    {
                        CompressionTextureCoder.PVRTC.EncodePVRTCI_4BPP_RGBA((byte*)ptr.ToPointer(), width, t);
                    }
                }
            }
            return new TextureInfo
            {
                Width = rawwidth,
                Height = rawheight,
                Data = ans
            };
        }
    }
}
