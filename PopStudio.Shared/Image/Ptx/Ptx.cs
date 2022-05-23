namespace PopStudio.Image.Ptx
{
    internal static class Ptx
    {
        public static void Encode(string inFile, string outFile, int format)
        {
            using (BinaryStream bs = BinaryStream.Create(outFile))
            {
                using (YFLib.Bitmap sKBitmap = YFLib.Bitmap.Create(inFile))
                {
                    YFLib.Texture.TextureFormat mFormat;
                    PtxHead head = new PtxHead
                    {
                        width = sKBitmap.Width,
                        height = sKBitmap.Height
                    };
                    switch (format)
                    {
                        case 0:
                            head.check = head.width << 2;
                            head.format = PtxFormat.ARGB8888;
                            mFormat = YFLib.Texture.TextureFormat.ARGB8888;
                            break;
                        case 1:
                            head.check = head.width << 2;
                            head.format = PtxFormat.ARGB8888;
                            mFormat = YFLib.Texture.TextureFormat.ABGR8888;
                            break;
                        case 2:
                            head.check = head.width << 1;
                            head.format = PtxFormat.RGBA4444;
                            mFormat = YFLib.Texture.TextureFormat.RGBA4444;
                            break;
                        case 3:
                            head.check = head.width << 1;
                            head.format = PtxFormat.RGB565;
                            mFormat = YFLib.Texture.TextureFormat.RGB565;
                            break;
                        case 4:
                            head.check = head.width << 1;
                            head.format = PtxFormat.RGBA5551;
                            mFormat = YFLib.Texture.TextureFormat.RGBA5551;
                            break;
                        case 5:
                            if ((head.width & 31) != 0)
                            {
                                head.width |= 31;
                                head.width++;
                            }
                            if ((head.height & 31) != 0)
                            {
                                head.height |= 31;
                                head.height++;
                            }
                            head.check = head.width << 1;
                            head.format = PtxFormat.RGBA4444_Block;
                            mFormat = YFLib.Texture.TextureFormat.RGBA4444_Block32;
                            break;
                        case 6:
                            if ((head.width & 31) != 0)
                            {
                                head.width |= 31;
                                head.width++;
                            }
                            if ((head.height & 31) != 0)
                            {
                                head.height |= 31;
                                head.height++;
                            }
                            head.check = head.width << 1;
                            head.format = PtxFormat.RGB565_Block;
                            mFormat = YFLib.Texture.TextureFormat.RGB565_Block32;
                            break;
                        case 7:
                            if ((head.width & 31) != 0)
                            {
                                head.width |= 31;
                                head.width++;
                            }
                            if ((head.height & 31) != 0)
                            {
                                head.height |= 31;
                                head.height++;
                            }
                            head.check = head.width << 1;
                            head.format = PtxFormat.RGBA5551_Block;
                            mFormat = YFLib.Texture.TextureFormat.RGBA5551_Block32;
                            break;
                        case 8:
                            head.check = head.width << 3;
                            head.format = PtxFormat.RGBA5551_Block;
                            mFormat = YFLib.Texture.TextureFormat.RGBA5551_Block32;
                            break;
                        case 9:
                            bs.Endian = Endian.Big;
                            head.check = head.width << 2;
                            head.format = PtxFormat.ARGB8888;
                            mFormat = YFLib.Texture.TextureFormat.BGRA8888;
                            break;
                        case 10:
                            bs.Endian = Endian.Big;
                            int w = head.width;
                            if ((w & 63) != 0)
                            {
                                w |= 63;
                                w++;
                            }
                            head.check = w << 2;
                            head.format = PtxFormat.ARGB8888;
                            mFormat = YFLib.Texture.TextureFormat.BGRA8888_Padding256;
                            break;
                        case 11:
                            int w2 = head.width;
                            if ((w2 & 3) != 0)
                            {
                                w2 |= 3;
                                w2++;
                            }
                            head.check = w2 >> 1;
                            head.format = PtxFormat.DXT1_RGB;
                            mFormat = YFLib.Texture.TextureFormat.RGB_DXT1;
                            break;
                        case 12:
                            int w3 = head.width;
                            if ((w3 & 3) != 0)
                            {
                                w3 |= 3;
                                w3++;
                            }
                            head.check = w3;
                            head.format = PtxFormat.DXT3_RGBA;
                            mFormat = YFLib.Texture.TextureFormat.RGBA_DXT3;
                            break;
                        case 13:
                            int w4 = head.width;
                            if ((w4 & 3) != 0)
                            {
                                w4 |= 3;
                                w4++;
                            }
                            head.check = w4;
                            head.format = PtxFormat.DXT5_RGBA;
                            mFormat = YFLib.Texture.TextureFormat.RGBA_DXT5;
                            break;
                        case 14:
                            int w5 = head.width;
                            if ((w5 & 3) != 0)
                            {
                                w5 |= 3;
                                w5++;
                            }
                            head.check = w5;
                            head.format = PtxFormat.DXT5;
                            mFormat = YFLib.Texture.TextureFormat.RGBA_DXT5;
                            break;
                        case 15:
                            bs.Endian = Endian.Big;
                            int w6 = head.width;
                            if ((w6 & 3) != 0)
                            {
                                w6 |= 3;
                                w6++;
                            }
                            head.check = w6;
                            head.format = PtxFormat.DXT5;
                            mFormat = YFLib.Texture.TextureFormat.RGBA_DXT5;
                            break;
                        case 16:
                            int w7 = head.width;
                            if ((w7 & 3) != 0)
                            {
                                w7 |= 3;
                                w7++;
                            }
                            head.check = w7 >> 1;
                            head.format = PtxFormat.ETC1_RGB;
                            mFormat = YFLib.Texture.TextureFormat.RGB_ETC1;
                            break;
                        case 17:
                            head.check = head.width << 2;
                            head.format = PtxFormat.ETC1_RGB_A8;
                            mFormat = YFLib.Texture.TextureFormat.RGB_ETC1_A8;
                            break;
                        case 18:
                            head.check = head.width << 2;
                            head.format = PtxFormat.ETC1_RGB_A8;
                            mFormat = YFLib.Texture.TextureFormat.RGB_ETC1_A_Palette;
                            int n = head.width * head.height;
                            if ((n & 1) != 0) n++;
                            head.alphaSize = (n >> 1) + 17;
                            head.alphaFormat = 0x64;
                            break;
                        case 19:
                            head.check = head.width << 2;
                            head.format = PtxFormat.ETC1_RGB_A_Palette;
                            mFormat = YFLib.Texture.TextureFormat.RGB_ETC1_A_Palette;
                            int n2 = head.width * head.height;
                            if ((n2 & 1) != 0) n2++;
                            head.alphaSize = (n2 >> 1) + 17;
                            head.alphaFormat = 0x64;
                            break;
                        case 20:
                            if ((head.width & (head.width - 1)) != 0)
                            {
                                int i = 1;
                                while (i < head.width) i <<= 1;
                                head.width = i;
                            }
                            if ((head.height & (head.height - 1)) != 0)
                            {
                                int i = 1;
                                while (i < head.height) i <<= 1;
                                head.height = i;
                            }
                            if (head.width != head.height)
                            {
                                head.width = head.width > head.height ? head.width : head.height;
                                head.height = head.width;
                            }
                            head.check = head.width >> 1;
                            head.format = PtxFormat.PVRTC_4BPP_RGBA;
                            mFormat = YFLib.Texture.TextureFormat.RGBA_PVRTCI_4BPP;
                            break;
                        case 21:
                            if ((head.width & (head.width - 1)) != 0)
                            {
                                int i = 1;
                                while (i < head.width) i <<= 1;
                                head.width = i;
                            }
                            if ((head.height & (head.height - 1)) != 0)
                            {
                                int i = 1;
                                while (i < head.height) i <<= 1;
                                head.height = i;
                            }
                            if (head.width != head.height)
                            {
                                head.width = head.width > head.height ? head.width : head.height;
                                head.height = head.width;
                            }
                            head.check = head.width << 2;
                            head.format = PtxFormat.PVRTC_4BPP_RGB_A8;
                            mFormat = YFLib.Texture.TextureFormat.RGB_PVRTCI_4BPP_A8;
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

        public static void Encode(string inFile, string outFile, PtxFormat format, Endian encodeendian, bool chinesemode)
        {
            using (BinaryStream bs = BinaryStream.Create(outFile))
            {
                bs.Endian = encodeendian;
                using (YFLib.Bitmap sKBitmap = YFLib.Bitmap.Create(inFile))
                {
                    YFLib.Texture.TextureFormat mFormat;
                    PtxHead head = new PtxHead
                    {
                        width = sKBitmap.Width,
                        height = sKBitmap.Height,
                        format = format
                    };
                    if (bs.Endian == Endian.Small)
                    {
                        switch (format)
                        {
                            case PtxFormat.ARGB8888:
                                if (Setting.RsbPtxABGR8888Mode)
                                {
                                    head.check = head.width << 2;
                                    mFormat = YFLib.Texture.TextureFormat.ABGR8888;
                                }
                                else
                                {
                                    head.check = head.width << 2;
                                    mFormat = YFLib.Texture.TextureFormat.ARGB8888;
                                }
                                break;
                            case PtxFormat.RGBA4444:
                                head.check = head.width << 1;
                                mFormat = YFLib.Texture.TextureFormat.RGBA4444;
                                break;
                            case PtxFormat.RGB565:
                                head.check = head.width << 1;
                                mFormat = YFLib.Texture.TextureFormat.RGB565;
                                break;
                            case PtxFormat.RGBA5551:
                                head.check = head.width << 1;
                                mFormat = YFLib.Texture.TextureFormat.RGBA5551;
                                break;
                            case PtxFormat.DXT5:
                                int w5 = head.width;
                                if ((w5 & 3) != 0)
                                {
                                    w5 |= 3;
                                    w5++;
                                }
                                head.check = w5;
                                mFormat = YFLib.Texture.TextureFormat.RGBA_DXT5;
                                break;
                            case PtxFormat.RGBA4444_Block:
                                if ((head.width & 31) != 0)
                                {
                                    head.width |= 31;
                                    head.width++;
                                }
                                if ((head.height & 31) != 0)
                                {
                                    head.height |= 31;
                                    head.height++;
                                }
                                head.check = head.width << 1;
                                mFormat = YFLib.Texture.TextureFormat.RGBA4444_Block32;
                                break;
                            case PtxFormat.RGB565_Block:
                                if ((head.width & 31) != 0)
                                {
                                    head.width |= 31;
                                    head.width++;
                                }
                                if ((head.height & 31) != 0)
                                {
                                    head.height |= 31;
                                    head.height++;
                                }
                                head.check = head.width << 1;
                                mFormat = YFLib.Texture.TextureFormat.RGB565_Block32;
                                break;
                            case PtxFormat.RGBA5551_Block:
                                if ((head.width & 31) != 0)
                                {
                                    head.width |= 31;
                                    head.width++;
                                }
                                if ((head.height & 31) != 0)
                                {
                                    head.height |= 31;
                                    head.height++;
                                }
                                head.check = head.width << 1;
                                mFormat = YFLib.Texture.TextureFormat.RGBA5551_Block32;
                                break;
                            case PtxFormat.PVRTC_4BPP_RGBA:
                                if ((head.width & (head.width - 1)) != 0)
                                {
                                    int i = 1;
                                    while (i < head.width) i <<= 1;
                                    head.width = i;
                                }
                                if ((head.height & (head.height - 1)) != 0)
                                {
                                    int i = 1;
                                    while (i < head.height) i <<= 1;
                                    head.height = i;
                                }
                                if (head.width != head.height)
                                {
                                    head.width = head.width > head.height ? head.width : head.height;
                                    head.height = head.width;
                                }
                                head.check = head.width >> 1;
                                mFormat = YFLib.Texture.TextureFormat.RGBA_PVRTCI_4BPP;
                                break;
                            case PtxFormat.ETC1_RGB:
                                int w1 = head.width;
                                if ((w1 & 3) != 0)
                                {
                                    w1 |= 3;
                                    w1++;
                                }
                                head.check = w1 >> 1;
                                mFormat = YFLib.Texture.TextureFormat.RGB_ETC1;
                                break;
                            case PtxFormat.DXT1_RGB:
                                int w4 = head.width;
                                if ((w4 & 3) != 0)
                                {
                                    w4 |= 3;
                                    w4++;
                                }
                                head.check = w4 >> 1;
                                mFormat = YFLib.Texture.TextureFormat.RGB_DXT1;
                                break;
                            case PtxFormat.DXT3_RGBA:
                                int w3 = head.width;
                                if ((w3 & 3) != 0)
                                {
                                    w3 |= 3;
                                    w3++;
                                }
                                head.check = w3;
                                mFormat = YFLib.Texture.TextureFormat.RGBA_DXT3;
                                break;
                            case PtxFormat.DXT5_RGBA:
                                int w2 = head.width;
                                if ((w2 & 3) != 0)
                                {
                                    w2 |= 3;
                                    w2++;
                                }
                                head.check = w2;
                                mFormat = YFLib.Texture.TextureFormat.RGBA_DXT5;
                                break;
                            case PtxFormat.ETC1_RGB_A8:
                                if (chinesemode)
                                {
                                    head.check = head.width << 2;
                                    mFormat = YFLib.Texture.TextureFormat.RGB_ETC1_A_Palette;
                                    int n2 = head.width * head.height;
                                    if ((n2 & 1) != 0) n2++;
                                    head.alphaSize = (n2 >> 1) + 17;
                                    head.alphaFormat = 0x64;
                                }
                                else
                                {
                                    head.check = head.width << 2;
                                    mFormat = YFLib.Texture.TextureFormat.RGB_ETC1_A8;
                                }
                                break;
                            case PtxFormat.PVRTC_4BPP_RGB_A8:
                                head.check = head.width << 2;
                                mFormat = YFLib.Texture.TextureFormat.RGB_PVRTCI_4BPP_A8;
                                break;
                            case PtxFormat.XRGB8888_A8:
                                head.check = head.width << 3;
                                mFormat = YFLib.Texture.TextureFormat.XRGB8888_A8;
                                break;
                            case PtxFormat.ETC1_RGB_A_Palette:
                                head.check = head.width << 2;
                                mFormat = YFLib.Texture.TextureFormat.RGB_ETC1_A_Palette;
                                int n22 = head.width * head.height;
                                if ((n22 & 1) != 0) n22++;
                                head.alphaSize = (n22 >> 1) + 17;
                                head.alphaFormat = 0x64;
                                break;
                            default:
                                throw new Exception(Str.Obj.UnknownFormat);
                        }
                    }
                    else
                    {
                        switch (format)
                        {
                            case PtxFormat.ARGB8888:
                                if (Setting.RsbPtxARGB8888PaddingMode)
                                {
                                    int w = head.width;
                                    if ((w & 63) != 0)
                                    {
                                        w |= 63;
                                        w++;
                                    }
                                    head.check = w << 2;
                                    mFormat = YFLib.Texture.TextureFormat.BGRA8888_Padding256;
                                }
                                else
                                {
                                    head.check = head.width << 2;
                                    mFormat = YFLib.Texture.TextureFormat.BGRA8888;
                                }
                                break;
                            case PtxFormat.DXT5:
                                int w5 = head.width;
                                if ((w5 & 3) != 0)
                                {
                                    w5 |= 3;
                                    w5++;
                                }
                                head.check = w5;
                                mFormat = YFLib.Texture.TextureFormat.RGBA_DXT5;
                                break;
                            default:
                                throw new Exception(Str.Obj.UnknownFormat);
                        }
                    }
                    YFLib.Texture.TextureInfo info = sKBitmap.EncodeAsTexture(mFormat);
                    head.Write(bs);
                    bs.Write(info.Data, 0, info.Size);
                }
            }
        }

        public static void Decode(string inFile, string outFile, bool fromrsb = false)
        {
            using (BinaryStream bs = BinaryStream.Open(inFile))
            {
                PtxHead head = new PtxHead().Read(bs);
                YFLib.Texture.TextureInfo info = new YFLib.Texture.TextureInfo
                {
                    Width = head.width,
                    Height = head.height,
                    Data = bs.ReadBytesToEnd()
                };
                if (bs.Endian == Endian.Small)
                {
                    switch (head.format)
                    {
                        case PtxFormat.ARGB8888:
                            if ((fromrsb && Setting.RsbPtxABGR8888Mode) || ((!fromrsb) && Setting.PtxABGR8888Mode))
                            {
                                info.Format = YFLib.Texture.TextureFormat.ABGR8888;
                            }
                            else
                            {
                                info.Format = YFLib.Texture.TextureFormat.ARGB8888;
                            }
                            break;
                        case PtxFormat.RGBA4444:
                            info.Format = YFLib.Texture.TextureFormat.RGBA4444;
                            break;
                        case PtxFormat.RGB565:
                            info.Format = YFLib.Texture.TextureFormat.RGB565;
                            break;
                        case PtxFormat.RGBA5551:
                            info.Format = YFLib.Texture.TextureFormat.RGBA5551;
                            break;
                        case PtxFormat.DXT5:
                            info.Format = YFLib.Texture.TextureFormat.RGBA_DXT5;
                            break;
                        case PtxFormat.RGBA4444_Block:
                            info.Format = YFLib.Texture.TextureFormat.RGBA4444_Block32;
                            break;
                        case PtxFormat.RGB565_Block:
                            info.Format = YFLib.Texture.TextureFormat.RGB565_Block32;
                            break;
                        case PtxFormat.RGBA5551_Block:
                            info.Format = YFLib.Texture.TextureFormat.RGBA5551_Block32;
                            break;
                        case PtxFormat.PVRTC_4BPP_RGBA:
                            info.Format = YFLib.Texture.TextureFormat.RGBA_PVRTCI_4BPP;
                            break;
                        case PtxFormat.ETC1_RGB:
                            info.Format = YFLib.Texture.TextureFormat.RGB_ETC1;
                            break;
                        case PtxFormat.DXT1_RGB:
                            info.Format = YFLib.Texture.TextureFormat.RGB_DXT1;
                            break;
                        case PtxFormat.DXT3_RGBA:
                            info.Format = YFLib.Texture.TextureFormat.RGBA_DXT3;
                            break;
                        case PtxFormat.DXT5_RGBA:
                            info.Format = YFLib.Texture.TextureFormat.RGBA_DXT5;
                            break;
                        case PtxFormat.ETC1_RGB_A8:
                            if (head.alphaFormat == 0x64)
                            {
                                info.Format = YFLib.Texture.TextureFormat.RGB_ETC1_A_Palette;
                            }
                            else
                            {
                                if (head.alphaFormat != 0x0) throw new Exception(Str.Obj.UnknownFormat);
                                info.Format = YFLib.Texture.TextureFormat.RGB_ETC1_A8;
                            }
                            break;
                        case PtxFormat.PVRTC_4BPP_RGB_A8:
                            info.Format = YFLib.Texture.TextureFormat.RGB_PVRTCI_4BPP_A8;
                            break;
                        case PtxFormat.XRGB8888_A8:
                            info.Format = YFLib.Texture.TextureFormat.XRGB8888_A8;
                            break;
                        case PtxFormat.ETC1_RGB_A_Palette:
                            info.Format = YFLib.Texture.TextureFormat.RGB_ETC1_A_Palette;
                            break;
                        default:
                            throw new Exception(Str.Obj.UnknownFormat);
                    }
                }
                else
                {
                    switch (head.format)
                    {
                        case PtxFormat.ARGB8888:
                            if ((fromrsb && Setting.RsbPtxARGB8888PaddingMode) || ((!fromrsb) && Setting.PtxARGB8888PaddingMode))
                            {
                                info.Format = YFLib.Texture.TextureFormat.BGRA8888;
                            }
                            else
                            {
                                info.Format = YFLib.Texture.TextureFormat.BGRA8888_Padding256;
                            }
                            break;
                        case PtxFormat.DXT5:
                            info.Format = YFLib.Texture.TextureFormat.RGBA_DXT5;
                            break;
                    }
                }
                using (YFLib.Bitmap map = YFLib.Bitmap.DecodeTexture(info))
                {
                    map.Save(outFile);
                }
            }
        }
    }
}