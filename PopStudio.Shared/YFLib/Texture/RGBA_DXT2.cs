using System.Runtime.CompilerServices;

namespace YFLib.Texture
{
    internal class RGBA_DXT2
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int GetSize(int width, int height) => width * height;

        public static Bitmap Decode(TextureInfo info)
        {
            int rawwidth = info.Width;
            int rawheight = info.Height;
            int width = rawwidth;
            int height = rawheight;
            if ((rawwidth & 3) != 0)
            {
                width |= 3;
                width++;
            }
            if ((rawheight & 3) != 0)
            {
                height |= 3;
                height++;
            }
            if (info.Data.Length < GetSize(width, height)) return null;
            Bitmap ans = Bitmap.Create(rawwidth, rawheight);
            IntPtr ptr = ans.GetPixels();
            unsafe
            {
                byte** color_ptr = stackalloc byte*[16];
                byte* emptycolor = stackalloc byte[4];
                fixed (byte* fixedptr = info.Data)
                {
                    byte* texptr = fixedptr;
                    byte* zeroimgptr = (byte*)ptr.ToPointer();
                    for (int i = 0; i < height; i += 4)
                    {
                        for (int w = 0; w < width; w += 4)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                for (int k = 0; k < 4; k++)
                                {
                                    if ((i + j) < rawheight && (w + k) < rawwidth)
                                    {
                                        *(color_ptr + ((j << 2) | k)) = zeroimgptr + (((i + j) * rawwidth + w + k) << 2);
                                    }
                                    else
                                    {
                                        *(color_ptr + ((j << 2) | k)) = emptycolor;
                                    }
                                }
                            }
                            CompressionTextureCoder.DXTC.DecodeLinearColorWord_SmallEndian(texptr + 8, color_ptr);
                            CompressionTextureCoder.DXTC.DecodeExplicitAlphaWord_SmallEndian(texptr, color_ptr);
                            CompressionTextureCoder.DXTC.RecoverPremultipliedAlpha(color_ptr);
                            texptr += 16;
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
            if ((rawwidth & 3) != 0)
            {
                width |= 3;
                width++;
            }
            if ((rawheight & 3) != 0)
            {
                height |= 3;
                height++;
            }
            int rawwidth1 = rawwidth - 1;
            int rawheight1 = rawheight - 1;
            byte[] ans = new byte[GetSize(width, height)];
            unsafe
            {
                byte** color_ptr = stackalloc byte*[16];
                byte** color_buf_ptr = stackalloc byte*[16];
                byte* color_buf = stackalloc byte[64];
                for (int i = 0; i < 16; i++)
                {
                    color_buf_ptr[i] = color_buf + (i << 2);
                }
                fixed (byte* fixedptr = ans)
                {
                    byte* texptr = fixedptr;
                    byte* zeroimgptr = (byte*)ptr.ToPointer();
                    for (int i = 0; i < height; i += 4)
                    {
                        for (int w = 0; w < width; w += 4)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                for (int k = 0; k < 4; k++)
                                {
                                    if ((i + j) >= rawheight)
                                    {
                                        *(color_ptr + ((j << 2) | k)) = zeroimgptr + ((rawheight1 * rawwidth + w + k) << 2);
                                    }
                                    else if ((w + k) >= rawwidth)
                                    {
                                        *(color_ptr + ((j << 2) | k)) = zeroimgptr + (((i + j) * rawwidth + rawwidth1) << 2);
                                    }
                                    else
                                    {
                                        *(color_ptr + ((j << 2) | k)) = zeroimgptr + (((i + j) * rawwidth + w + k) << 2);
                                    }
                                }
                            }
                            CompressionTextureCoder.DXTC.PremultiplyAlpha(color_ptr, color_buf_ptr);
                            CompressionTextureCoder.DXTC.EncodeLinearColorWord_SmallEndian_RGB_FastestSpeed(texptr + 8, color_buf_ptr);
                            CompressionTextureCoder.DXTC.EncodeExplicitAlphaWord_SmallEndian(texptr, color_buf_ptr);
                            texptr += 16;
                        }
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
