using System.Runtime.CompilerServices;

namespace YFLib.Texture
{
    internal class RGBA5551_Block32
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
            if (info.Data.Length < GetSize(width, height) || (width & 31) != 0 || (height & 31) != 0) return null;
            Bitmap ans = Bitmap.Create(width, height);
            IntPtr ptr = ans.GetPixels();
            unsafe
            {
                fixed (byte* fixedptr = info.Data)
                {
                    ushort* texptr = (ushort*)fixedptr;
                    byte* zeroimgptr = (byte*)ptr.ToPointer();
                    byte* imgptr = zeroimgptr;
                    for (int i = 0; i < height; i += 32)
                    {
                        for (int w = 0; w < width; w += 32)
                        {
                            for (int j = 0; j < 32; j++)
                            {
                                for (int k = 0; k < 32; k++)
                                {
                                    imgptr = zeroimgptr + (((i + j) * width + w + k) << 2);
                                    *imgptr++ = From5Bits(*texptr, 1);
                                    *imgptr++ = From5Bits(*texptr, 6);
                                    *imgptr++ = From5Bits(*texptr, 11);
                                    *imgptr++ = (byte)((*texptr++ & 1) == 0 ? 0 : 255);
                                }
                            }
                        }
                    }
                }
            }
            return ans;
        }

        public static TextureInfo Encode(Bitmap bitmap)
        {
            IntPtr ptr = bitmap.GetPixels();
            int rawwidth = bitmap.Width;
            int rawheight = bitmap.Height;
            int width = rawwidth;
            int height = rawheight;
            if ((width & 31) != 0)
            {
                width |= 31;
                width++;
            }
            if ((height & 31) != 0)
            {
                height |= 31;
                height++;
            }
            byte[] ans = new byte[GetSize(width, height)];
            unsafe
            {
                fixed (byte* fixedptr = ans)
                {
                    ushort* texptr = (ushort*)fixedptr;
                    byte* zeroimgptr = (byte*)ptr.ToPointer();
                    byte* imgptr = zeroimgptr;
                    for (int i = 0; i < height; i += 32)
                    {
                        for (int w = 0; w < width; w += 32)
                        {
                            for (int j = 0; j < 32; j++)
                            {
                                for (int k = 0; k < 32; k++)
                                {
                                    if ((i + j) >= rawheight || (w + k) >= rawwidth)
                                    {
                                        *texptr++ = 0;
                                        continue;
                                    }
                                    imgptr = zeroimgptr + (((i + j) * rawwidth + w + k) << 2);
                                    *texptr++ = (ushort)(((*imgptr++) >> 3 << 1) | ((*imgptr++) >> 3 << 6) | ((*imgptr++) >> 3 << 11) | ((*imgptr++) >> 7));
                                }
                            }
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
