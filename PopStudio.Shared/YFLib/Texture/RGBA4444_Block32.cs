using System.Runtime.CompilerServices;

namespace YFLib.Texture
{
    internal class RGBA4444_Block32
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int GetSize(int width, int height) => (width * height) << 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static byte From4Bits(ushort color, int b)
        {
            int k = (color >> b) & 0b1111;
            return (byte)(k | (k << 4));
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
                                    *imgptr++ = From4Bits(*texptr, 4);
                                    *imgptr++ = From4Bits(*texptr, 8);
                                    *imgptr++ = From4Bits(*texptr, 12);
                                    *imgptr++ = From4Bits(*texptr++, 0);
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
                                    *texptr++ = (ushort)(((*imgptr++) >> 4 << 4) | ((*imgptr++) >> 4 << 8) | ((*imgptr++) >> 4 << 12) | ((*imgptr++) >> 4));
                                }
                            }
                        }
                    }
                }
            }
            return new TextureInfo
            {
                Width = width, //Block32 did not support rawwidth
                Height = height,
                Data = ans
            };
        }
    }
}
