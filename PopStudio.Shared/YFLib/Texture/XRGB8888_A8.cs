using System.Runtime.CompilerServices;

namespace YFLib.Texture
{
    internal class XRGB8888_A8
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int GetSize(int width, int height) => width * height * 5;

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
                    byte* imgptrzero = (byte*)ptr.ToPointer();
                    byte* imgptr = imgptrzero;
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            *imgptr++ = *texptr++;
                            *imgptr++ = *texptr++;
                            *imgptr++ = *texptr++;
                            *imgptr++ = 255;
                            texptr++;
                        }
                    }
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            imgptrzero[3]= *texptr++;
                            imgptrzero += 4;
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
                    byte* imgptrzero = (byte*)ptr.ToPointer();
                    byte* imgptr = imgptrzero;
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            *texptr++ = *imgptr++;
                            *texptr++ = *imgptr++;
                            *texptr++ = *imgptr++;
                            *texptr++ = 255;
                            imgptr++;
                        }
                    }
                    texptr = fixedptr;
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            *texptr++ = imgptrzero[3];
                            imgptrzero += 4;
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