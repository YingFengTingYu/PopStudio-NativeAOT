namespace PopStudio.Image.Tex
{
    internal static class Tex
    {
        public static void Encode(string inFile, string outFile, int format)
        {
            using (BinaryStream bs = BinaryStream.Create(outFile))
            {
                using (YFLib.Bitmap sKBitmap = YFLib.Bitmap.Create(inFile))
                {
                    TexHead head = new TexHead
                    {
                        width = (ushort)sKBitmap.Width,
                        height = (ushort)sKBitmap.Height
                    };
                    YFLib.Texture.TextureFormat mFormat;
                    switch (format)
                    {
                        case 0:
                            head.format = TexFormat.ABGR8888;
                            mFormat = YFLib.Texture.TextureFormat.ABGR8888;
                            break;
                        case 1:
                            head.format = TexFormat.RGBA4444;
                            mFormat = YFLib.Texture.TextureFormat.RGBA4444;
                            break;
                        case 2:
                            head.format = TexFormat.RGBA5551;
                            mFormat = YFLib.Texture.TextureFormat.RGBA5551;
                            break;
                        case 3:
                            head.format = TexFormat.RGB565;
                            mFormat = YFLib.Texture.TextureFormat.RGB565;
                            break;
                        default:
                            throw new Exception(Str.Obj.UnknownFormat);
                    }
                    YFLib.Texture.TextureInfo info = sKBitmap.EncodeAsTexture(mFormat);
                    head.Write(bs);
                    bs.Write(info.Data, 0, info.Size);
                }
            }
        }

        public static void Decode(string inFile, string outFile)
        {
            using (BinaryStream bs = new BinaryStream(inFile, FileMode.Open))
            {
                TexHead head = new TexHead().Read(bs);
                YFLib.Texture.TextureInfo info = new YFLib.Texture.TextureInfo
                {
                    Width = head.width,
                    Height = head.height,
                    Data = bs.ReadBytesToEnd()
                };
                switch (head.format)
                {
                    case TexFormat.ABGR8888:
                        info.Format = YFLib.Texture.TextureFormat.ABGR8888;
                        break;
                    case TexFormat.RGBA4444:
                        info.Format = YFLib.Texture.TextureFormat.RGBA4444;
                        break;
                    case TexFormat.RGBA5551:
                        info.Format = YFLib.Texture.TextureFormat.RGBA5551;
                        break;
                    case TexFormat.RGB565:
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