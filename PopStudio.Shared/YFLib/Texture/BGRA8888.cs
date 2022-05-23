using System.Runtime.CompilerServices;

namespace YFLib.Texture
{
    internal class BGRA8888
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int GetSize(int width, int height) => (width * height) << 2;

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
                            *imgptr++ = *(texptr + 3);
                            *imgptr++ = *(texptr + 2);
                            *imgptr++ = *(texptr + 1);
                            *imgptr++ = *texptr;
                            texptr += 4;
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
                            *texptr++ = *(imgptr + 3);
                            *texptr++ = *(imgptr + 2);
                            *texptr++ = *(imgptr + 1);
                            *texptr++ = *imgptr;
                            imgptr += 4;
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