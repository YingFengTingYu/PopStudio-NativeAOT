namespace YFLib.Texture
{
    internal class Coder
    {
        public static Bitmap Decode(TextureInfo info) => info.Format switch
        {
            TextureFormat.ABGR8888 => ABGR8888.Decode(info),
            TextureFormat.ARGB8888 => ARGB8888.Decode(info),
            TextureFormat.RGBA4444 => RGBA4444.Decode(info),
            TextureFormat.ARGB4444 => ARGB4444.Decode(info),
            TextureFormat.RGBA5551 => RGBA5551.Decode(info),
            TextureFormat.ARGB1555 => ARGB1555.Decode(info),
            TextureFormat.RGB565 => RGB565.Decode(info),
            TextureFormat.RGB888 => RGB888.Decode(info),
            TextureFormat.XRGB8888 => XRGB8888.Decode(info),
            TextureFormat.XRGB8888_A8 => XRGB8888_A8.Decode(info),
            TextureFormat.BGRA8888 => BGRA8888.Decode(info),
            TextureFormat.BGRA8888_Padding256 => BGRA8888_Padding256.Decode(info),
            TextureFormat.LA88 => LA88.Decode(info),
            TextureFormat.LA44 => LA44.Decode(info),
            TextureFormat.L8 => L8.Decode(info),
            TextureFormat.A8 => A8.Decode(info),
            TextureFormat.RGBA4444_Block32 => RGBA4444_Block32.Decode(info),
            TextureFormat.RGBA5551_Block32 => RGBA5551_Block32.Decode(info),
            TextureFormat.RGB565_Block32 => RGB565_Block32.Decode(info),
            TextureFormat.RGB_DXT1 => RGB_DXT1.Decode(info),
            TextureFormat.RGBA_DXT1 => RGBA_DXT1.Decode(info),
            TextureFormat.RGBA_DXT2 => RGBA_DXT2.Decode(info),
            TextureFormat.RGBA_DXT3 => RGBA_DXT3.Decode(info),
            TextureFormat.RGBA_DXT4 => RGBA_DXT4.Decode(info),
            TextureFormat.RGBA_DXT5 => RGBA_DXT5.Decode(info),
            TextureFormat.RGB_ETC1 => RGB_ETC1.Decode(info),
            TextureFormat.RGB_ETC1_A8 => RGB_ETC1_A8.Decode(info),
            TextureFormat.RGB_ETC1_A_Palette => RGB_ETC1_A_Palette.Decode(info),
            TextureFormat.RGBA_PVRTCI_4BPP => RGBA_PVRTCI_4BPP.Decode(info),
            _ => throw new Exception()
        };

        public static TextureInfo Encode(Bitmap bitmap, TextureFormat format) => format switch
        {
            TextureFormat.ABGR8888 => ABGR8888.Encode(bitmap).SetFormat(format),
            TextureFormat.ARGB8888 => ARGB8888.Encode(bitmap).SetFormat(format),
            TextureFormat.RGBA4444 => RGBA4444.Encode(bitmap).SetFormat(format),
            TextureFormat.ARGB4444 => ARGB4444.Encode(bitmap).SetFormat(format),
            TextureFormat.RGBA5551 => RGBA5551.Encode(bitmap).SetFormat(format),
            TextureFormat.ARGB1555 => ARGB1555.Encode(bitmap).SetFormat(format),
            TextureFormat.RGB565 => RGB565.Encode(bitmap).SetFormat(format),
            TextureFormat.RGB888 => RGB888.Encode(bitmap).SetFormat(format),
            TextureFormat.XRGB8888 => XRGB8888.Encode(bitmap).SetFormat(format),
            TextureFormat.XRGB8888_A8 => XRGB8888_A8.Encode(bitmap).SetFormat(format),
            TextureFormat.BGRA8888 => BGRA8888.Encode(bitmap).SetFormat(format),
            TextureFormat.BGRA8888_Padding256 => BGRA8888_Padding256.Encode(bitmap).SetFormat(format),
            TextureFormat.LA88 => LA88.Encode(bitmap).SetFormat(format),
            TextureFormat.LA44 => LA44.Encode(bitmap).SetFormat(format),
            TextureFormat.L8 => L8.Encode(bitmap).SetFormat(format),
            TextureFormat.A8 => A8.Encode(bitmap).SetFormat(format),
            TextureFormat.RGBA4444_Block32 => RGBA4444_Block32.Encode(bitmap).SetFormat(format),
            TextureFormat.RGBA5551_Block32 => RGBA5551_Block32.Encode(bitmap).SetFormat(format),
            TextureFormat.RGB565_Block32 => RGB565_Block32.Encode(bitmap).SetFormat(format),
            TextureFormat.RGB_DXT1 => RGB_DXT1.Encode(bitmap).SetFormat(format),
            TextureFormat.RGBA_DXT1 => RGBA_DXT1.Encode(bitmap).SetFormat(format),
            TextureFormat.RGBA_DXT2 => RGBA_DXT2.Encode(bitmap).SetFormat(format),
            TextureFormat.RGBA_DXT3 => RGBA_DXT3.Encode(bitmap).SetFormat(format),
            TextureFormat.RGBA_DXT4 => RGBA_DXT4.Encode(bitmap).SetFormat(format),
            TextureFormat.RGBA_DXT5 => RGBA_DXT5.Encode(bitmap).SetFormat(format),
            TextureFormat.RGB_ETC1 => RGB_ETC1.Encode(bitmap).SetFormat(format),
            TextureFormat.RGB_ETC1_A8 => RGB_ETC1_A8.Encode(bitmap).SetFormat(format),
            TextureFormat.RGB_ETC1_A_Palette => RGB_ETC1_A_Palette.Encode(bitmap).SetFormat(format),
            TextureFormat.RGBA_PVRTCI_4BPP => RGBA_PVRTCI_4BPP.Encode(bitmap).SetFormat(format),
            _ => throw new Exception()
        };
    }
}
