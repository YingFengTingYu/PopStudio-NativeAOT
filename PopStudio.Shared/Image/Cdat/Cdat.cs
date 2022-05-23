using System.Text;

namespace PopStudio.Image.Cdat
{
    internal static class Cdat
    {
        public static void Encode(string inFile, string outFile)
        {
            byte[] code = Encoding.UTF8.GetBytes(Setting.CdatKey);
            using (BinaryStream bs2 = BinaryStream.Create(outFile))
            {
                using (BinaryStream bs = BinaryStream.Open(inFile))
                {
                    CdatHead head = new CdatHead
                    {
                        size = bs.Length
                    };
                    if (head.size >= 0x100)
                    {
                        head.Write(bs2);
                        int index = 0;
                        int arysize = code.Length;
                        byte[] tempArr = new byte[0x100];
                        bs.Read(tempArr, 0, 0x100);
                        for (int i = 0; i < 0x100; i++)
                        {
                            tempArr[i] ^= code[index++];
                            index %= arysize;
                        }
                        bs2.Write(tempArr, 0, 0x100);
                    }
                    bs.CopyTo(bs2);
                }
            }
        }

        public static void Decode(string inFile, string outFile)
        {
            byte[] code = Encoding.UTF8.GetBytes(Setting.CdatKey);
            using (BinaryStream bs = BinaryStream.Open(inFile))
            {
                CdatHead head = new CdatHead().Read(bs);
                using (BinaryStream bs2 = BinaryStream.Create(outFile))
                {
                    if (bs.Length >= 0x112)
                    {
                        int index = 0;
                        int arysize = code.Length;
                        byte[] tempArr = new byte[0x100];
                        bs.Read(tempArr, 0, 0x100);
                        for (int i = 0; i < 0x100; i++)
                        {
                            tempArr[i] ^= code[index++];
                            index %= arysize;
                        }
                        bs2.Write(tempArr, 0, 0x100);
                    }
                    bs.CopyTo(bs2);
                }
            }
        }
    }
}
