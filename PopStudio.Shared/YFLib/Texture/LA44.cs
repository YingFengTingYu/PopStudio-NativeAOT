using System.Runtime.CompilerServices;

namespace YFLib.Texture
{
    internal class LA44
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int GetSize(int width, int height) => width * height;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static byte From4Bits(byte color, int b)
        {
            int k = (color >> b) & 0b1111;
            return (byte)(k | (k << 4));
        }

        public static Bitmap Decode(TextureInfo info)
        {
            int width = info.Width;
            int height = info.Height;
            if (info.Data.Length < GetSize(width, height)) return null;
            Bitmap ans = Bitmap.Create(width, height);
            IntPtr ptr = ans.GetPixels();
            unsafe
            {
                fixed (byte* fixedptr = info.Data)
                {
                    byte* texptr = fixedptr;
                    byte* imgptr = (byte*)ptr.ToPointer();
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            byte l = From4Bits(*texptr, 4);
                            *imgptr++ = l;
                            *imgptr++ = l;
                            *imgptr++ = l;
                            *imgptr++ = From4Bits(*texptr++, 0);
                        }
                    }
                }
            }
            return ans;
        }

        public static TextureInfo Encode(Bitmap bitmap)
        {
            IntPtr ptr = bitmap.GetPixels();
            int width = bitmap.Width;
            int height = bitmap.Height;
            byte[] ans = new byte[GetSize(width, height)];
            unsafe
            {
                fixed (byte* fixedptr = ans)
                {
                    byte* texptr = fixedptr;
                    byte* imgptr = (byte*)ptr.ToPointer();
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            *texptr++ = (byte)(((*imgptr++ + (*imgptr++ << 1) + *imgptr++) >> 6 << 4) | (*imgptr++ >> 4));
                        }
                    }
                }
            }
            return new TextureInfo
            {
                Width = width,
                Height = height,
                Data = ans
            };
        }
    }
}