using SkiaSharp;

namespace PopStudio.Image.PtxPS3
{
    /// <summary>
    /// It's dds used DXT5_RGBA Texture
    /// </summary>
    internal static class Ptx
    {
        public static void Encode(string inFile, string outFile)
        {
            using (BinaryStream bs = new BinaryStream(outFile, FileMode.Create))
            {
                using (YFLib.Bitmap sKBitmap = YFLib.Bitmap.Create(inFile))
                {
                    PtxHead head = new PtxHead
                    {
                        width = (ushort)sKBitmap.Width,
                        height = (ushort)sKBitmap.Height
                    };
                    YFLib.Texture.TextureInfo info = sKBitmap.EncodeAsTexture(YFLib.Texture.TextureFormat.RGBA_DXT5);
                    head.Write(bs);
                    bs.Write(info.Data, 0, info.Size);
                }
            }
        }

        public static void Decode(string inFile, string outFile)
        {
            using (BinaryStream bs = new BinaryStream(inFile, FileMode.Open))
            {
                PtxHead head = new PtxHead().Read(bs);
                using (YFLib.Bitmap sKBitmap = YFLib.Bitmap.DecodeTexture(new YFLib.Texture.TextureInfo { Width = head.width, Height = head.height, Format = YFLib.Texture.TextureFormat.RGBA_DXT5, Data = bs.ReadBytesToEnd() }))
                {
                    sKBitmap.Save(outFile);
                }
            }
        }
    }
}