using System.Runtime.CompilerServices;

namespace YFLib.Texture
{
    internal class BGRA8888_Padding256
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int GetSize(int width, int height) => (width * height) << 2;

        public static Bitmap Decode(TextureInfo info)
        {
            int rawwidth = info.Width;
            int height = info.Height;
            int width = rawwidth;
            if ((width & 63) != 0)
            {
                width |= 63;
                width++;
            }
            int deltax = (width - rawwidth) << 2;
            if (info.Data.Length < GetSize(width, height)) return null;
            Bitmap ans = Bitmap.Create(rawwidth, height);
            IntPtr ptr = ans.GetPixels();
            unsafe
            {
                fixed (byte* fixedptr = info.Data)
                {
                    byte* texptr = fixedptr;
                    byte* imgptr = (byte*)ptr.ToPointer();
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < rawwidth; x++)
                        {
                            *imgptr++ = *(texptr + 3);
                            *imgptr++ = *(texptr + 2);
                            *imgptr++ = *(texptr + 1);
                            *imgptr++ = *texptr;
                            texptr += 4;
                        }
                        texptr += deltax;
                    }
                }
            }
            return ans;
        }

        public static TextureInfo Encode(Bitmap bitmap)
        {
            IntPtr ptr = bitmap.GetPixels();
            int rawwidth = bitmap.Width;
            int height = bitmap.Height;
            int width = rawwidth;
            if ((width & 63) != 0)
            {
                width |= 63;
                width++;
            }
            int deltax = (width - rawwidth) << 2;
            byte[] ans = new byte[GetSize(width, height)];
            unsafe
            {
                fixed (byte* fixedptr = ans)
                {
                    byte* texptr = fixedptr;
                    byte* imgptr = (byte*)ptr.ToPointer();
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < rawwidth; x++)
                        {
                            *texptr++ = *(imgptr + 3);
                            *texptr++ = *(imgptr + 2);
                            *texptr++ = *(imgptr + 1);
                            *texptr++ = *imgptr;
                            imgptr += 4;
                        }
                        texptr += deltax;
                    }
                }
            }
            return new TextureInfo
            {
                Width = rawwidth,
                Height = height,
                Data = ans
            };
        }
    }
}