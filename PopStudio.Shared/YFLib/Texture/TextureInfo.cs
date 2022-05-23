namespace YFLib.Texture
{
    public class TextureInfo
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public byte[] Data { get; set; }
        public TextureFormat Format { get; set; }
        public int Size => Data?.Length ?? -1;

        public TextureInfo SetFormat(TextureFormat f)
        {
            Format = f;
            return this;
        }

        ~TextureInfo() => Data = null;
    }
}
