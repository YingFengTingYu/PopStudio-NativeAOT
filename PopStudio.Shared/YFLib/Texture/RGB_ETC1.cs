using System.Runtime.CompilerServices;

namespace YFLib.Texture
{
    internal class RGB_ETC1
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int GetSize(int width, int height) => (width * height) >> 1;

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
                            CompressionTextureCoder.ETC.DecodeETC1ColorWord(color_ptr, (ulong)(uint)(texptr[3] | texptr[2] << 8 | texptr[1] << 16 | texptr[0] << 24) << 32 | (uint)(texptr[7] | texptr[6] << 8 | texptr[5] << 16 | texptr[4] << 24));
                            texptr += 8;
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
                fixed (byte* fixedptr = ans)
                {
                    byte* texptr = fixedptr;
                    byte* zeroimgptr = (byte*)ptr.ToPointer();
                    ulong ans_buffer;
                    //Alloc a buffer
                    byte** color_buffer_ptr = stackalloc byte*[16];
                    byte* color_buffer = stackalloc byte[64];
                    for (int i = 0; i < 16; i++)
                    {
                        color_buffer_ptr[i] = color_buffer + (i << 2);
                    }
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
                            ans_buffer = CompressionTextureCoder.ETC.EncodeETC1ColorWord(color_ptr, color_buffer_ptr);
                            *texptr++ = (byte)(ans_buffer >> 56);
                            *texptr++ = (byte)(ans_buffer >> 48);
                            *texptr++ = (byte)(ans_buffer >> 40);
                            *texptr++ = (byte)(ans_buffer >> 32);
                            *texptr++ = (byte)(ans_buffer >> 24);
                            *texptr++ = (byte)(ans_buffer >> 16);
                            *texptr++ = (byte)(ans_buffer >> 8);
                            *texptr++ = (byte)ans_buffer;
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
