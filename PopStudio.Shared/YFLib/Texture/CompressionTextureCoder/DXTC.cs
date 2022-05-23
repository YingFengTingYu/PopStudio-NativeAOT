using System.Runtime.CompilerServices;

namespace YFLib.Texture.CompressionTextureCoder
{
    /// <summary>
    /// refer to Real-Time-Dxt-Compression
    /// </summary>
    internal class DXTC
    {
        #region Decode
        /// <summary>
        /// Decode DXT Color Word to BBGGRRAA
        /// </summary>
        /// <param name="block_word">Memory space with a length of 64 bits to show the block word</param>
        /// <param name="color_pos">Memory space with a length of 64 bytes to save the color</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void DecodeLinearColorWord_SmallEndian(byte* block_word, byte** color_pos)
        {
            ushort* ushortptr = (ushort*)block_word;
            bool c1dayuc2 = (*ushortptr) > (*(ushortptr + 1));
            uint r1 = (uint)((*ushortptr) >> 11);
            uint g1 = (uint)(((*ushortptr) >> 5) & 0b111111);
            uint b1 = (uint)((*ushortptr++) & 0b11111);
            r1 = (r1 >> 2) | (r1 << 3);
            g1 = (g1 >> 4) | (g1 << 2);
            b1 = (b1 >> 2) | (b1 << 3);
            uint r2 = (uint)((*ushortptr) >> 11);
            uint g2 = (uint)(((*ushortptr) >> 5) & 0b111111);
            uint b2 = (uint)((*ushortptr++) & 0b11111);
            r2 = (r2 >> 2) | (r2 << 3);
            g2 = (g2 >> 4) | (g2 << 2);
            b2 = (b2 >> 2) | (b2 << 3);
            uint* colors_ptr = stackalloc uint[4];
            *colors_ptr = 0xFF000000 | (r1 << 16) | (g1 << 8) | b1;
            *(colors_ptr + 1) = 0xFF000000 | (r2 << 16) | (g2 << 8) | b2;
            if (c1dayuc2)
            {
                *(colors_ptr + 2) = 0xFF000000 | ((((r1 << 1) + r2) / 3) << 16) | ((((g1 << 1) + g2) / 3) << 8) | (((b1 << 1) + b2) / 3);
                *(colors_ptr + 3) = 0xFF000000 | ((((r2 << 1) + r1) / 3) << 16) | ((((g2 << 1) + g1) / 3) << 8) | (((b2 << 1) + b1) / 3);
            }
            else
            {
                *(colors_ptr + 2) = 0xFF000000 | (((r1 + r2) >> 1) << 16) | (((g1 + g2) >> 1) << 8) | ((b1 + b2) >> 1);
                *(colors_ptr + 3) = 0x00000000;
            }
            uint colorFlags = *(uint*)ushortptr;
            for (int i = 0; i < 16; i++)
            {
                *(uint*)*color_pos = *(colors_ptr + (colorFlags & 0b11));
                color_pos++;
                colorFlags >>= 2;
            }
        }

        /// <summary>
        /// Decode DXT Alpha Word to 000000AA
        /// </summary>
        /// <param name="block_word">Memory space with a length of 64 bits to show the block word</param>
        /// <param name="color_pos">Memory space with a length of 64 bytes to save the alpha color</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void DecodeLinearAlphaWord_SmallEndian(byte* block_word, byte** color_pos)
        {
            byte* alpha = stackalloc byte[8];
            byte a1 = *block_word;
            byte a2 = *(block_word + 1);
            *alpha = a1;
            *(alpha + 1) = a2;
            if (a1 > a2)
            {
                *(alpha + 2) = (byte)((6 * a1 + a2) / 7);
                *(alpha + 3) = (byte)((5 * a1 + (a2 << 1)) / 7);
                *(alpha + 4) = (byte)(((a1 << 2) + 3 * a2) / 7);
                *(alpha + 5) = (byte)((3 * a1 + (a2 << 2)) / 7);
                *(alpha + 6) = (byte)(((a1 << 1) + 5 * a2) / 7);
                *(alpha + 7) = (byte)((a1 + 6 * a2) / 7);
            }
            else
            {
                *(alpha + 2) = (byte)(((a1 << 2) + a2) / 5);
                *(alpha + 3) = (byte)((3 * a1 + (a2 << 1)) / 5);
                *(alpha + 4) = (byte)(((a1 << 1) + 3 * a2) / 5);
                *(alpha + 5) = (byte)((a1 + (a2 << 2)) / 5);
                *(alpha + 6) = 0;
                *(alpha + 7) = 255;
            }
            ulong alpha_flags = *(ulong*)block_word;
            alpha_flags >>= 16;
            for (int i = 0; i < 16; i++)
            {
                *((*color_pos++) + 3) = *(alpha + (alpha_flags & 0b111));
                alpha_flags >>= 3;
            }
        }

        /// <summary>
        /// Decode A4(Each alpha with 4 bits) Word to 000000AA
        /// </summary>
        /// <param name="block_word">Memory space with a length of 64 bits to show the block word</param>
        /// <param name="color_pos">Memory space with a length of 64 bytes to save the alpha color</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void DecodeExplicitAlphaWord_SmallEndian(byte* block_word, byte** color_pos)
        {
            ulong ulong_value = *(ulong*)block_word;
            ulong tempvalue;
            for (int i = 0; i < 16; i++)
            {
                tempvalue = ulong_value & 0xF;
                *((*color_pos++) + 3) = (byte)(tempvalue << 4 | tempvalue);
                ulong_value >>= 4;
            }
        }

        /// <summary>
        /// Recover the alpha-premultiplied color to simple color
        /// </summary>
        /// <param name="color_pos">Memory space with a length of 64 bytes which saves the color</param>
        [YFAttribute.Untested("YFLib.Texture.CompressionTextureCoder.DXTC.RecoverPremultipliedAlpha")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void RecoverPremultipliedAlpha(byte** color_pos)
        {
            byte* c;
            byte c3;
            for (int i = 0; i < 16; i++)
            {
                c = *color_pos++;
                c3 = *(c + 3);
                if (c3 != 0)
                {
                    *c = ToByte((*c << 8) / c3);
                    c++;
                    *c = ToByte((*c << 8) / c3);
                    c++;
                    *c = ToByte((*c << 8) / c3);
                }
            }
        }
        #endregion

        #region Private

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static byte ToByte(int v)
        {
            if (v >= 255) return 255;
            if (v <= 0) return 0;
            return (byte)v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe ushort ColorToRGB565(byte* inPtr) => (ushort)(((*inPtr++) >> 3) | ((*inPtr++) >> 2 << 5) | ((*inPtr++) >> 3 << 11));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe int ColorDistance_RGB(byte* c1, byte* c2) => (((*c1) - (*c2)) * ((*c1) - (*c2))) + (((*(c1 + 1)) - (*(c2 + 1))) * ((*(c1 + 1)) - (*(c2 + 1)))) + (((*(c1 + 2)) - (*(c2 + 2))) * ((*(c1 + 2)) - (*(c2 + 2))));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe void SwapColors_RGB(byte* c1, byte* c2)
        {
            byte* tm = stackalloc byte[3];
            MemoryCopy(tm, c1, 3);
            MemoryCopy(c1, c2, 3);
            MemoryCopy(c2, tm, 3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe void MemoryCopy(byte* a1, byte* a2, int length)
        {
            for (int i = 0; i < length; i++)
            {
                *a1++ = *a2++;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe void GetMinMaxColorsByDistance_A(byte** colorBlock, byte* minAlpha, byte* maxAlpha)
        {
            *minAlpha = 255;
            *maxAlpha = 0;
            for (int i = 0; i < 16; i++)
            {
                if ((*(*colorBlock + 3)) < (*minAlpha)) *minAlpha = *(*colorBlock + 3);
                if ((*(*colorBlock + 3)) > (*maxAlpha)) *maxAlpha = *(*colorBlock + 3);
                colorBlock++;
            }
            byte inset = (byte)(((*maxAlpha) - (*minAlpha)) >> 4);
            *minAlpha = ToByte(*minAlpha + inset);
            *maxAlpha = ToByte(*maxAlpha - inset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe void GetMinMaxColorsByDistance_RGB(byte** colorBlock, byte* minColor, byte* maxColor)
        {
            int maxDistance = -1;
            for (int i = 0; i < 15; i++)
            {
                for (int j = i + 1; j < 16; j++)
                {
                    int distance = ColorDistance_RGB(*(colorBlock + i), *(colorBlock + j));
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                        MemoryCopy(minColor, *(colorBlock + i), 3);
                        MemoryCopy(maxColor, *(colorBlock + j), 3);
                    }
                }
            }
            if (ColorToRGB565(maxColor) < ColorToRGB565(minColor))
            {
                SwapColors_RGB(minColor, maxColor);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe int ColorLuminance_RGB(byte* color) => *color++ + ((*color++) << 1) + *color++;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe void GetMinMaxColorsByLuminance_RGB(byte** colorBlock, byte* minColor, byte* maxColor)
        {
            int maxLuminance = -1, minLuminance = int.MaxValue;
            for (int i = 0; i < 16; i++)
            {
                int luminance = ColorLuminance_RGB(*(colorBlock + i));
                if (luminance > maxLuminance)
                {
                    maxLuminance = luminance;
                    MemoryCopy(maxColor, *(colorBlock + i), 3);
                }
                if (luminance < minLuminance)
                {
                    minLuminance = luminance;
                    MemoryCopy(minColor, *(colorBlock + i), 3);
                }
            }
            if (ColorToRGB565(maxColor) < ColorToRGB565(minColor))
            {
                SwapColors_RGB(minColor, maxColor);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe void GetMinMaxColorsByBox_RGBA(byte** colorBlock, byte* minColor, byte* maxColor)
        {
            byte* inset = stackalloc byte[3];
            *minColor = 255;
            *(minColor + 1) = 255;
            *(minColor + 2) = 255;
            *maxColor = 0;
            *(maxColor + 1) = 0;
            *(maxColor + 2) = 0;
            for (int i = 0; i < 16; i++)
            {
                if (((*(*colorBlock + 3)) & 0b10000000) == 0) continue;
                if ((**colorBlock) < (*minColor)) *minColor = **colorBlock;
                if ((*(*colorBlock + 1)) < (*(minColor + 1))) *(minColor + 1) = *(*colorBlock + 1);
                if ((*(*colorBlock + 2)) < (*(minColor + 2))) *(minColor + 2) = *(*colorBlock + 2);
                if ((**colorBlock) > (*maxColor)) *maxColor = **colorBlock;
                if ((*(*colorBlock + 1)) > (*(maxColor + 1))) *(maxColor + 1) = *(*colorBlock + 1);
                if ((*(*colorBlock + 2)) > (*(maxColor + 2))) *(maxColor + 2) = *(*colorBlock + 2);
                colorBlock++;
            }
            *inset = (byte)(((*maxColor) - (*minColor)) >> 4);
            *(inset + 1) = (byte)(((*(maxColor + 1)) - (*(minColor + 1))) >> 4);
            *(inset + 2) = (byte)(((*(maxColor + 2)) - (*(minColor + 2))) >> 4);
            *minColor = ToByte(*minColor + *inset);
            minColor++;
            *minColor = ToByte(*minColor + *(inset + 1));
            minColor++;
            *minColor = ToByte(*minColor + *(inset + 2));
            *maxColor = ToByte(*maxColor - *inset);
            maxColor++;
            *maxColor = ToByte(*maxColor - *(inset + 1));
            maxColor++;
            *maxColor = ToByte(*maxColor - *(inset + 2));
            //Maybe all the pixels are transform
            if (ColorToRGB565(maxColor) < ColorToRGB565(minColor))
            {
                SwapColors_RGB(minColor, maxColor);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe void GetMinMaxColorsByBox_RGB(byte** colorBlock, byte* minColor, byte* maxColor)
        {
            byte* inset = stackalloc byte[3];
            *minColor = 255;
            *(minColor + 1) = 255;
            *(minColor + 2) = 255;
            *maxColor = 0;
            *(maxColor + 1) = 0;
            *(maxColor + 2) = 0;
            for (int i = 0; i < 16; i++)
            {
                if ((**colorBlock) < (*minColor)) *minColor = **colorBlock;
                if ((*(*colorBlock + 1)) < (*(minColor + 1))) *(minColor + 1) = *(*colorBlock + 1);
                if ((*(*colorBlock + 2)) < (*(minColor + 2))) *(minColor + 2) = *(*colorBlock + 2);
                if ((**colorBlock) > (*maxColor)) *maxColor = **colorBlock;
                if ((*(*colorBlock + 1)) > (*(maxColor + 1))) *(maxColor + 1) = *(*colorBlock + 1);
                if ((*(*colorBlock + 2)) > (*(maxColor + 2))) *(maxColor + 2) = *(*colorBlock + 2);
                colorBlock++;
            }
            *inset = (byte)(((*maxColor) - (*minColor)) >> 4);
            *(inset + 1) = (byte)(((*(maxColor + 1)) - (*(minColor + 1))) >> 4);
            *(inset + 2) = (byte)(((*(maxColor + 2)) - (*(minColor + 2))) >> 4);
            *minColor = ToByte(*minColor + *inset);
            minColor++;
            *minColor = ToByte(*minColor + *(inset + 1));
            minColor++;
            *minColor = ToByte(*minColor + *(inset + 2));
            *maxColor = ToByte(*maxColor - *inset);
            maxColor++;
            *maxColor = ToByte(*maxColor - *(inset + 1));
            maxColor++;
            *maxColor = ToByte(*maxColor - *(inset + 2));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe void EmitColorIndicesByDistance_RGB(byte* block_word, byte** colorBlock, byte* minColor, byte* maxColor)
        {
            byte* colors = stackalloc byte[16];
            int result = 0;
            colors[0] = (byte)((maxColor[0] & 0xF8) | (maxColor[0] >> 5));
            colors[1] = (byte)((maxColor[1] & 0xFC) | (maxColor[1] >> 6));
            colors[2] = (byte)((maxColor[2] & 0xF8) | (maxColor[2] >> 5));
            colors[4] = (byte)((minColor[0] & 0xF8) | (minColor[0] >> 5));
            colors[5] = (byte)((minColor[1] & 0xFC) | (minColor[1] >> 6));
            colors[6] = (byte)((minColor[2] & 0xF8) | (minColor[2] >> 5));
            colors[8] = (byte)(((colors[0] << 1) + colors[4]) / 3);
            colors[9] = (byte)(((colors[1] << 1) + colors[5]) / 3);
            colors[10] = (byte)(((colors[2] << 1) + colors[6]) / 3);
            colors[12] = (byte)((colors[0] + (colors[4] << 1)) / 3);
            colors[13] = (byte)((colors[1] + (colors[5] << 1)) / 3);
            colors[14] = (byte)((colors[2] + (colors[6] << 1)) / 3);
            int currentValue = 0;
            for (int i = 15; i >= 0; i--)
            {
                int minDistance = int.MaxValue;
                for (int j = 0; j < 4; j++)
                {
                    int dist = ColorDistance_RGB(*(colorBlock + i), colors + (j << 2));
                    if (dist < minDistance)
                    {
                        minDistance = dist;
                        currentValue = j;
                    }
                }
                result |= (currentValue << (i << 1));
            }
            *(((int*)block_word) + 1) = result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int Abs(int a) => a < 0 ? (-a) : a;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe void GetMinMaxColors(byte** colorBlock, byte* minColor, byte* maxColor)
        {
            int maxDistance = -1;
            for (int i = 0; i < 15; i++)
            {
                for (int j = i + 1; j < 16; j++)
                {
                    int distance = ColorDistance_RGB(colorBlock[i], colorBlock[j]);
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                        MemoryCopy(minColor, colorBlock[i], 3);
                        MemoryCopy(maxColor, colorBlock[j], 3);
                    }
                }
            }
            if (ColorToRGB565(maxColor) < ColorToRGB565(minColor))
            {
                SwapColors_RGB(minColor, maxColor);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe void EmitColorIndicesByAbs_RGBA(byte* block_word, byte** colorBlock, byte* minColor, byte* maxColor)
        {
            byte* colors = stackalloc byte[12];
            int result = 0;
            colors[0] = (byte)((maxColor[0] & 0xF8) | (maxColor[0] >> 5));
            colors[1] = (byte)((maxColor[1] & 0xFC) | (maxColor[1] >> 6));
            colors[2] = (byte)((maxColor[2] & 0xF8) | (maxColor[2] >> 5));
            colors[4] = (byte)((minColor[0] & 0xF8) | (minColor[0] >> 5));
            colors[5] = (byte)((minColor[1] & 0xFC) | (minColor[1] >> 6));
            colors[6] = (byte)((minColor[2] & 0xF8) | (minColor[2] >> 5));
            colors[8] = (byte)(((colors[0] ) + colors[4]) / 3);
            colors[9] = (byte)(((colors[1] ) + colors[5]) / 3);
            colors[10] = (byte)(((colors[2] ) + colors[6]) / 3);
            for (int i = 15; i >= 0; i--)
            {
                if ((colorBlock[i][3] & 0b10000000) == 0)
                {
                    result |= 0b11 << (i << 1);
                }
                else
                {
                    int c0 = colorBlock[i][0];
                    int c1 = colorBlock[i][1];
                    int c2 = colorBlock[i][2];
                    int d0 = Abs(colors[0] - c0) + Abs(colors[1] - c1) + Abs(colors[2] - c2);
                    int d1 = Abs(colors[4] - c0) + Abs(colors[5] - c1) + Abs(colors[6] - c2);
                    int d2 = Abs(colors[8] - c0) + Abs(colors[9] - c1) + Abs(colors[10] - c2);
                    if (d1 > d2 && d0 > d2)
                    {
                        result |= 0b10 << (i << 1);
                    }
                    else if (d1 > d2)
                    {
                        result |= 0b01 << (i << 1);
                    }
                    else
                    {
                        result |= 0b00 << (i << 1);
                    }
                }
            }
            *(((int*)block_word) + 1) = result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe void EmitColorIndicesByAbs_RGB(byte* block_word, byte** colorBlock, byte* minColor, byte* maxColor)
        {
            byte* colors = stackalloc byte[16];
            int result = 0;
            colors[0] = (byte)((maxColor[0] & 0xF8) | (maxColor[0] >> 5));
            colors[1] = (byte)((maxColor[1] & 0xFC) | (maxColor[1] >> 6));
            colors[2] = (byte)((maxColor[2] & 0xF8) | (maxColor[2] >> 5));
            colors[4] = (byte)((minColor[0] & 0xF8) | (minColor[0] >> 5));
            colors[5] = (byte)((minColor[1] & 0xFC) | (minColor[1] >> 6));
            colors[6] = (byte)((minColor[2] & 0xF8) | (minColor[2] >> 5));
            colors[8] = (byte)(((colors[0] << 1) + colors[4]) / 3);
            colors[9] = (byte)(((colors[1] << 1) + colors[5]) / 3);
            colors[10] = (byte)(((colors[2] << 1) + colors[6]) / 3);
            colors[12] = (byte)((colors[0] + (colors[4] << 1)) / 3);
            colors[13] = (byte)((colors[1] + (colors[5] << 1)) / 3);
            colors[14] = (byte)((colors[2] + (colors[6] << 1)) / 3);
            for (int i = 15; i >= 0; i--)
            {
                int c0 = colorBlock[i][0];
                int c1 = colorBlock[i][1];
                int c2 = colorBlock[i][2];
                int d0 = Abs(colors[0] - c0) + Abs(colors[1] - c1) + Abs(colors[2] - c2);
                int d1 = Abs(colors[4] - c0) + Abs(colors[5] - c1) + Abs(colors[6] - c2);
                int d2 = Abs(colors[8] - c0) + Abs(colors[9] - c1) + Abs(colors[10] - c2);
                int d3 = Abs(colors[12] - c0) + Abs(colors[13] - c1) + Abs(colors[14] - c2);
                int b0 = d0 > d3 ? 1 : 0;
                int b1 = d1 > d2 ? 1 : 0;
                int b2 = d0 > d2 ? 1 : 0;
                int b3 = d1 > d3 ? 1 : 0;
                int b4 = d2 > d3 ? 1 : 0;
                int x0 = b1 & b2;
                int x1 = b0 & b3;
                int x2 = b0 & b4;
                result |= (x2 | ((x0 | x1) << 1)) << (i << 1);
            }
            *(((int*)block_word) + 1) = result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe void EmitAlphaIndices(byte* block_word, byte** colorBlock, byte minAlpha, byte maxAlpha)
        {
            byte* indices = stackalloc byte[16];
            byte mid = ToByte((maxAlpha - minAlpha) / 14);
            byte ab1 = ToByte(minAlpha + mid);
            byte ab2 = ToByte((6 * maxAlpha + 1 * minAlpha) / 7 + mid);
            byte ab3 = ToByte((5 * maxAlpha + 2 * minAlpha) / 7 + mid);
            byte ab4 = ToByte((4 * maxAlpha + 3 * minAlpha) / 7 + mid);
            byte ab5 = ToByte((3 * maxAlpha + 4 * minAlpha) / 7 + mid);
            byte ab6 = ToByte((2 * maxAlpha + 5 * minAlpha) / 7 + mid);
            byte ab7 = ToByte((1 * maxAlpha + 6 * minAlpha) / 7 + mid);
            ulong ans = 0;
            for (int i = 15; i >= 0; i--)
            {
                ans <<= 3;
                byte a = (*colorBlock++)[3];
                uint b1 = (a <= ab1) ? 1u : 0u;
                uint b2 = (a <= ab2) ? 1u : 0u;
                uint b3 = (a <= ab3) ? 1u : 0u;
                uint b4 = (a <= ab4) ? 1u : 0u;
                uint b5 = (a <= ab5) ? 1u : 0u;
                uint b6 = (a <= ab6) ? 1u : 0u;
                uint b7 = (a <= ab7) ? 1u : 0u;
                uint index = (b1 + b2 + b3 + b4 + b5 + b6 + b7 + 1) & 7;
                ans |= index ^ ((2 > index) ? 1u : 0u);
            }
            ans <<= 16;
            *(ulong*)block_word |= ans;
        }
        #endregion


        #region Encode

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void EncodeLinearColorWord_SmallEndian_RGBA_FastestSpeed(byte* block_word, byte** color_pos)
        {
            //GetMinMaxColor
            byte* maxColor = stackalloc byte[4];
            byte* minColor = stackalloc byte[4];
            GetMinMaxColorsByBox_RGBA(color_pos, minColor, maxColor);
            EmitColorIndicesByAbs_RGBA(block_word, color_pos, minColor, maxColor);
            ushort* ushort_ptr = (ushort*)block_word;
            *ushort_ptr++ = ColorToRGB565(minColor);
            *ushort_ptr = ColorToRGB565(maxColor);
        }

        /// <summary>
        /// Encode BBGGRR00 to DXT Color Word by Fastest Speed mode
        /// </summary>
        /// <param name="block_word">Memory space with a length of 64 bits to show the block word</param>
        /// <param name="color_pos">Memory space with a length of 64 bytes to save the color</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void EncodeLinearColorWord_SmallEndian_RGB_FastestSpeed(byte* block_word, byte** color_pos)
        {
            //GetMinMaxColor
            byte* maxColor = stackalloc byte[4];
            byte* minColor = stackalloc byte[4];
            GetMinMaxColorsByBox_RGB(color_pos, minColor, maxColor);
            EmitColorIndicesByAbs_RGB(block_word, color_pos, minColor, maxColor);
            ushort* ushort_ptr = (ushort*)block_word;
            *ushort_ptr++ = ColorToRGB565(maxColor);
            *ushort_ptr = ColorToRGB565(minColor);
        }

        /// <summary>
        /// Encode BBGGRR00 to DXT Color Word by Optimal mode
        /// </summary>
        /// <param name="block_word">Memory space with a length of 64 bits to show the block word</param>
        /// <param name="color_pos">Memory space with a length of 64 bytes to save the color</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void EncodeLinearColorWord_SmallEndian_RGB_Optimal(byte* block_word, byte** color_pos)
        {
            //GetMinMaxColor
            byte* maxColor = stackalloc byte[4];
            byte* minColor = stackalloc byte[4];
            GetMinMaxColorsByLuminance_RGB(color_pos, minColor, maxColor);
            EmitColorIndicesByAbs_RGB(block_word, color_pos, minColor, maxColor);
            ushort* ushort_ptr = (ushort*)block_word;
            *ushort_ptr++ = ColorToRGB565(maxColor);
            *ushort_ptr = ColorToRGB565(minColor);
        }

        /// <summary>
        /// Encode BBGGRR00 to DXT Color Word by Best Quality mode
        /// </summary>
        /// <param name="block_word">Memory space with a length of 64 bits to show the block word</param>
        /// <param name="color_pos">Memory space with a length of 64 bytes to save the color</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void EncodeLinearColorWord_SmallEndian_RGB_BestQuality(byte* block_word, byte** color_pos)
        {
            //GetMinMaxColor
            byte* maxColor = stackalloc byte[4];
            byte* minColor = stackalloc byte[4];
            GetMinMaxColorsByDistance_RGB(color_pos, minColor, maxColor);
            EmitColorIndicesByDistance_RGB(block_word, color_pos, minColor, maxColor);
            ushort* ushort_ptr = (ushort*)block_word;
            *ushort_ptr++ = ColorToRGB565(maxColor);
            *ushort_ptr = ColorToRGB565(minColor);
        }

        /// <summary>
        /// Encode 000000AA to A4 Color
        /// </summary>
        /// <param name="block_word"></param>
        /// <param name="color_pos"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void EncodeExplicitAlphaWord_SmallEndian(byte* block_word, byte** color_pos)
        {
            ulong ulong_value = 0;
            for (int i = 15; i >= 0; i--)
            {
                ulong_value <<= 4;
                ulong_value |= (uint)((*((*color_pos++) + 3)) >> 4);
            }
            *(ulong*)block_word = ulong_value;
        }

        /// <summary>
        /// Encode 000000AA to DXT Alpha Word
        /// </summary>
        /// <param name="block_word"></param>
        /// <param name="color_pos"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void EncodeLinearAlphaWord_SmallEndian(byte* block_word, byte** color_pos)
        {
            byte maxAlpha;
            byte minAlpha;
            GetMinMaxColorsByDistance_A(color_pos, &minAlpha, &maxAlpha);
            EmitAlphaIndices(block_word, color_pos, minAlpha, maxAlpha);
            *block_word++ = maxAlpha;
            *block_word = minAlpha;
        }
        /// <summary>
        /// Premultiply alpha with color
        /// </summary>
        /// <param name="color_pos">Memory space with a length of 64 bytes which saves in color</param>
        /// <param name="ans_pos">Memory space with a length of 64 bytes which saves out color</param>
        [YFAttribute.Untested("YFLib.Texture.CompressionTextureCoder.DXTC.PremultiplyAlpha")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void PremultiplyAlpha(byte** color_pos, byte** ans_pos)
        {
            byte c3;
            for (int i = 0; i < 16; i++)
            {
                c3 = (*color_pos)[3];
                for (int j = 0; j < 3; j++)
                {
                    (*ans_pos)[j] = (byte)(((*color_pos)[j] * c3) >> 8);
                }
                (*ans_pos)[3] = c3;
                ans_pos++;
                color_pos++;
            }
        }
        #endregion
    }
}