using SkiaSharp;

namespace PopStudio.Image.Txz
{
    internal static class Txz
    {
        public static void Encode(string inFile, string outFile, int format)
        {
            using (BinaryStream bs = BinaryStream.Create(outFile))
            {
                using (YFLib.Bitmap sKBitmap = YFLib.Bitmap.Create(inFile))
                {
                    TxzHead head = new TxzHead
                    {
                        width = (ushort)sKBitmap.Width,
                        height = (ushort)sKBitmap.Height
                    };
                    YFLib.Texture.TextureFormat mFormat;
                    switch (format)
                    {
                        case 0:
                            head.format = TxzFormat.ABGR8888;
                            mFormat = YFLib.Texture.TextureFormat.ABGR8888;
                            break;
                        case 1:
                            head.format = TxzFormat.RGBA4444;
                            mFormat = YFLib.Texture.TextureFormat.RGBA4444;
                            break;
                        case 2:
                            head.format = TxzFormat.RGBA5551;
                            mFormat = YFLib.Texture.TextureFormat.RGBA5551;
                            break;
                        case 3:
                            head.format = TxzFormat.RGB565;
                            mFormat = YFLib.Texture.TextureFormat.RGB565;
                            break;
                        default:
                            throw new Exception(Str.Obj.UnknownFormat);
                    }
                    YFLib.Texture.TextureInfo info = sKBitmap.EncodeAsTexture(mFormat);
                    head.Write(bs);
                    using (BinaryStream bs2 = new BinaryStream(info.Data))
                    {
                        using (ZLibStream zLibStream = new ZLibStream(bs, CompressionMode.Compress, true))
                        {
                            bs2.CopyTo(zLibStream);
                        }
                    }
                }
            }
        }

        public static void Decode(string inFile, string outFile)
        {
            using (BinaryStream bs = new BinaryStream())
            {
                TxzHead head;
                using (BinaryStream bs_file = new BinaryStream(inFile, FileMode.Open))
                {
                    head = new TxzHead().Read(bs_file);
                    using (ZLibStream zLibStream = new ZLibStream(bs_file, CompressionMode.Decompress))
                    {
                        zLibStream.CopyTo(bs);
                    }
                }
                bs.Position = 0;
                YFLib.Texture.TextureInfo info = new YFLib.Texture.TextureInfo
                {
                    Width = head.width,
                    Height = head.height,
                    Data = bs.ReadBytesToEnd()
                };
                switch (head.format)
                {
                    case TxzFormat.ABGR8888:
                        info.Format = YFLib.Texture.TextureFormat.ABGR8888;
                        break;
                    case TxzFormat.RGBA4444:
                        info.Format = YFLib.Texture.TextureFormat.RGBA4444;
                        break;
                    case TxzFormat.RGBA5551:
                        info.Format = YFLib.Texture.TextureFormat.RGBA5551;
                        break;
                    case TxzFormat.RGB565:
                        info.Format = YFLib.Texture.TextureFormat.RGB565;
                        break;
                    default:
                        throw new Exception(Str.Obj.UnknownFormat);
                }
                using (YFLib.Bitmap sKBitmap = YFLib.Bitmap.DecodeTexture(info))
                {
                    sKBitmap.Save(outFile);
                }
            }
        }
    }
}