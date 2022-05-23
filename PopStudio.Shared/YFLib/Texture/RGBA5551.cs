using System.Runtime.CompilerServices;

namespace YFLib.Texture
{
    internal class RGBA5551
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int GetSize(int width, int height) => (width * height) << 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static byte From5Bits(ushort color, int b)
        {
            int k = (color >> b) & 0b11111;
            return (byte)((k >> 2) | (k << 3));
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
                    ushort* texptr = (ushort*)fixedptr;
                    byte* imgptr = (byte*)ptr.ToPointer();
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            *imgptr++ = From5Bits(*texptr, 1);
                            *imgptr++ = From5Bits(*texptr, 6);
                            *imgptr++ = From5Bits(*texptr, 11);
                            *imgptr++ = (byte)((*texptr++ & 1) == 0 ? 0 : 255);
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
                    ushort* texptr = (ushort*)fixedptr;
                    byte* imgptr = (byte*)ptr.ToPointer();
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            *texptr++ = (ushort)(((*imgptr++) >> 3 << 1) | ((*imgptr++) >> 3 << 6) | ((*imgptr++) >> 3 << 11) | ((*imgptr++) >> 7));
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
