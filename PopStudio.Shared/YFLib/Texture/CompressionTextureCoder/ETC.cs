using System.Runtime.CompilerServices;

namespace YFLib.Texture.CompressionTextureCoder
{
    internal class ETC
    {
        static readonly int[,] ETC1Modifiers =
        {
            { 2, 8 },
            { 5, 17 },
            { 9, 29 },
            { 13, 42 },
            { 18, 60 },
            { 24, 80 },
            { 33, 106 },
            { 47, 183 }
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static byte ToByte(int v)
        {
            if (v >= 255) return 255;
            if (v <= 0) return 0;
            return (byte)v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static byte ToByte(float v)
        {
            if (v >= 255) return 255;
            if (v <= 0) return 0;
            return (byte)v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void SetTable1(ref ulong Data, int Table)
        {
            Data &= ~(7ul << 37);
            Data |= (ulong)(Table & 0x7) << 37;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void SetTable2(ref ulong Data, int Table)
        {
            Data &= ~(7ul << 34);
            Data |= (ulong)(Table & 0x7) << 34;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void SetFlipMode(ref ulong Data, bool Mode)
        {
            Data &= ~(1ul << 32);
            Data |= (Mode ? 1ul : 0ul) << 32;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SetDiffMode(ref ulong Data, bool Mode)
        {
            Data &= ~(1ul << 33);
            Data |= (Mode ? 1ul : 0ul) << 33;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe void GetLeftColors(byte** Left, byte** Pixels)
        {
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 2; x++)
                {
                    Left[(y << 1) | x] = Pixels[(y << 2) | x];
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe void GetRightColors(byte** Right, byte** Pixels)
        {
            for (int y = 0; y < 4; y++)
            {
                for (int x = 2; x < 4; x++)
                {
                    Right[(y << 1) | (x - 2)] = Pixels[(y << 2) | x];
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe void GetTopColors(byte** Top, byte** Pixels)
        {
            for (int y = 0; y < 2; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    Top[(y << 2) | x] = Pixels[(y << 2) | x];
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe void GetBottomColors(byte** Bottom, byte** Pixels)
        {
            for (int y = 2; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    Bottom[((y - 2) << 2) | x] = Pixels[(y << 2) | x];
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe int GenModifier(byte* BaseColor, byte** Pixels)
        {
            byte* Max = stackalloc byte[3];
            byte* Min = stackalloc byte[3];
            Max[0] = Max[1] = Max[2] = 255;
            Min[0] = Min[1] = Min[2] = 0;
            int MinY = int.MaxValue;
            int MaxY = int.MinValue;
            for (int i = 0; i < 8; i++)
            {
                int Y = (Pixels[i][2] + Pixels[i][1] + Pixels[i][0]) / 3;
                if (Y > MaxY)
                {
                    MaxY = Y;
                    Max = Pixels[i];
                }
                if (Y < MinY)
                {
                    MinY = Y;
                    Min = Pixels[i];
                }
            }
            int DiffMean = (Max[2] - Min[2] + Max[1] - Min[1] + Max[0] - Min[0]) / 3;
            int ModDiff = int.MaxValue;
            int Modifier = -1;
            int Mode = -1;
            for (int i = 0; i < 8; i++)
            {
                int SS = ETC1Modifiers[i, 0] << 1;
                int SB = ETC1Modifiers[i, 0] + ETC1Modifiers[i, 1];
                int BB = ETC1Modifiers[i, 1] << 1;
                if (SS > 255) SS = 255;
                if (SB > 255) SB = 255;
                if (BB > 255) BB = 255;
                if (Math.Abs(DiffMean - SS) < ModDiff)
                {
                    ModDiff = Math.Abs(DiffMean - SS);
                    Modifier = i;
                    Mode = 0;
                }
                if (Math.Abs(DiffMean - SB) < ModDiff)
                {
                    ModDiff = Math.Abs(DiffMean - SB);
                    Modifier = i;
                    Mode = 1;
                }
                if (Math.Abs(DiffMean - BB) < ModDiff)
                {
                    ModDiff = Math.Abs(DiffMean - BB);
                    Modifier = i;
                    Mode = 2;
                }
            }
            if (Mode == 1)
            {
                float div1 = ETC1Modifiers[Modifier, 0] / (float)ETC1Modifiers[Modifier, 1];
                float div2 = 1f - div1;
                BaseColor[0] = ToByte(Min[0] * div1 + Max[0] * div2);
                BaseColor[1] = ToByte(Min[1] * div1 + Max[1] * div2);
                BaseColor[2] = ToByte(Min[2] * div1 + Max[2] * div2);
            }
            else
            {
                BaseColor[0] = ToByte((Min[0] + Max[0]) >> 1);
                BaseColor[1] = ToByte((Min[1] + Max[1]) >> 1);
                BaseColor[2] = ToByte((Min[2] + Max[2]) >> 1);
            }
            return Modifier;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe void GenPixDiff(ref ulong Data, byte** Pixels, byte* BaseColor, int Modifier, int XOffs, int XEnd, int YOffs, int YEnd)
        {
            int BaseMean = (BaseColor[2] + BaseColor[1] + BaseColor[0]) / 3;
            int i = 0;
            for (int yy = YOffs; yy < YEnd; yy++)
            {
                for (int xx = XOffs; xx < XEnd; xx++)
                {
                    int Diff = ((Pixels[i][2] + Pixels[i][1] + Pixels[i][0]) / 3) - BaseMean;

                    if (Diff < 0) Data |= 1ul << ((xx << 2) + yy + 16);
                    int tbldiff1 = Math.Abs(Diff) - ETC1Modifiers[Modifier, 0];
                    int tbldiff2 = Math.Abs(Diff) - ETC1Modifiers[Modifier, 1];

                    if (Math.Abs(tbldiff2) < Math.Abs(tbldiff1)) Data |= 1ul << ((xx << 2) + yy);
                    i++;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe void SetBaseColors(ref ulong Data, byte* Color1, byte* Color2)
        {
            int R1 = Color1[2];
            int G1 = Color1[1];
            int B1 = Color1[0];
            int R2 = Color2[2];
            int G2 = Color2[1];
            int B2 = Color2[0];
            //First look if differencial is possible.
            int RDiff = (R2 - R1) >> 3;
            int GDiff = (G2 - G1) >> 3;
            int BDiff = (B2 - B1) >> 3;
            if (RDiff > -4 && RDiff < 3 && GDiff > -4 && GDiff < 3 && BDiff > -4 && BDiff < 3)
            {
                SetDiffMode(ref Data, true);
                Data |= (ulong)(R1 >> 3) << 59;
                Data |= (ulong)(G1 >> 3) << 51;
                Data |= (ulong)(B1 >> 3) << 43;
                Data |= (ulong)(RDiff & 0x7) << 56;
                Data |= (ulong)(GDiff & 0x7) << 48;
                Data |= (ulong)(BDiff & 0x7) << 40;
            }
            else
            {
                Data |= (ulong)(R1 >> 4) << 60;
                Data |= (ulong)(G1 >> 4) << 52;
                Data |= (ulong)(B1 >> 4) << 44;

                Data |= (ulong)(R2 >> 4) << 56;
                Data |= (ulong)(G2 >> 4) << 48;
                Data |= (ulong)(B2 >> 4) << 40;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe ulong GenHorizontal(byte** Colors)
        {
            ulong data = 0;
            SetFlipMode(ref data, false);
            //Left
            byte** Left = stackalloc byte*[8];
            GetLeftColors(Left, Colors);
            byte* basec1 = stackalloc byte[3];
            int mod = GenModifier(basec1, Left);
            SetTable1(ref data, mod);
            GenPixDiff(ref data, Left, basec1, mod, 0, 2, 0, 4);
            //Right
            byte** Right = Left;
            GetRightColors(Right, Colors);
            byte* basec2 = stackalloc byte[3];
            mod = GenModifier(basec2, Right);
            SetTable2(ref data, mod);
            GenPixDiff(ref data, Right, basec2, mod, 2, 4, 0, 4);
            SetBaseColors(ref data, basec1, basec2);
            return data;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe ulong GenVertical(byte** Colors)
        {
            ulong data = 0;
            SetFlipMode(ref data, true);
            //Top
            byte** Top = stackalloc byte*[8];
            GetTopColors(Top, Colors);
            byte* basec1 = stackalloc byte[3];
            int mod = GenModifier(basec1, Top);
            SetTable1(ref data, mod);
            GenPixDiff(ref data, Top, basec1, mod, 0, 4, 0, 2);
            //Bottom
            byte** Bottom = Top;
            GetBottomColors(Bottom, Colors);
            byte* basec2 = stackalloc byte[3];
            mod = GenModifier(basec2, Bottom);
            SetTable2(ref data, mod);
            GenPixDiff(ref data, Bottom, basec2, mod, 0, 4, 2, 4);
            SetBaseColors(ref data, basec1, basec2);
            return data;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe ulong EncodeETC1ColorWord(byte** color_block, byte** buffer)
        {
            ulong Horizontal = GenHorizontal(color_block);
            ulong Vertical = GenVertical(color_block);
            int HorizontalScore = GetScore(color_block, DecodeETC1ColorWord_Internal(buffer, Horizontal));
            int VerticalScore = GetScore(color_block, DecodeETC1ColorWord_Internal(buffer, Vertical));
            return (HorizontalScore < VerticalScore) ? Horizontal : Vertical;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe int GetScore(byte** Original, byte** Encode)
        {
            int Diff = 0;
            for (int i = 0; i < 16; i++)
            {
                Diff += Math.Abs(Encode[i][2] - Original[i][2]);
                Diff += Math.Abs(Encode[i][1] - Original[i][1]);
                Diff += Math.Abs(Encode[i][0] - Original[i][0]);
            }
            return Diff;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void DecodeETC1ColorWord(byte** color_block, ulong etc_word) => DecodeETC1ColorWord_Internal(color_block, etc_word);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe byte** DecodeETC1ColorWord_Internal(byte** color_block, ulong etc_word)
        {
            bool diffbit = ((etc_word >> 33) & 1) == 1;
            bool flipbit = ((etc_word >> 32) & 1) == 1;
            int r1, r2, g1, g2, b1, b2;
            if (diffbit)
            {
                int r = (int)((etc_word >> 59) & 0x1F);
                int g = (int)((etc_word >> 51) & 0x1F);
                int b = (int)((etc_word >> 43) & 0x1F);
                r1 = (r << 3) | ((r & 0x1C) >> 2);
                g1 = (g << 3) | ((g & 0x1C) >> 2);
                b1 = (b << 3) | ((b & 0x1C) >> 2);
                r += (int)((etc_word >> 56) & 0x7) << 29 >> 29;
                g += (int)((etc_word >> 48) & 0x7) << 29 >> 29;
                b += (int)((etc_word >> 40) & 0x7) << 29 >> 29;
                r2 = (r << 3) | ((r & 0x1C) >> 2);
                g2 = (g << 3) | ((g & 0x1C) >> 2);
                b2 = (b << 3) | ((b & 0x1C) >> 2);
            }
            else
            {
                r1 = (int)((etc_word >> 60) & 0xF) * 0x11;
                g1 = (int)((etc_word >> 52) & 0xF) * 0x11;
                b1 = (int)((etc_word >> 44) & 0xF) * 0x11;
                r2 = (int)((etc_word >> 56) & 0xF) * 0x11;
                g2 = (int)((etc_word >> 48) & 0xF) * 0x11;
                b2 = (int)((etc_word >> 40) & 0xF) * 0x11;
            }
            int Table1 = (int)((etc_word >> 37) & 0x7);
            int Table2 = (int)((etc_word >> 34) & 0x7);
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int val = (int)((etc_word >> ((j << 2) | i)) & 0x1);
                    bool neg = ((etc_word >> (((j << 2) | i) + 16)) & 0x1) == 1;
                    if ((flipbit && i < 2) || (!flipbit && j < 2))
                    {
                        int add = ETC1Modifiers[Table1, val] * (neg ? -1 : 1);
                        byte* c = *(color_block + ((i << 2) | j));
                        c[0] = ToByte(b1 + add);
                        c[1] = ToByte(g1 + add);
                        c[2] = ToByte(r1 + add);
                        c[3] = 255;
                    }
                    else
                    {
                        int add = ETC1Modifiers[Table2, val] * (neg ? -1 : 1);
                        byte* c = *(color_block + ((i << 2) | j));
                        c[0] = ToByte(b2 + add);
                        c[1] = ToByte(g2 + add);
                        c[2] = ToByte(r2 + add);
                        c[3] = 255;
                    }
                }
            }
            return color_block;
        }
    }
}
