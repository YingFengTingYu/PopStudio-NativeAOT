using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace PopStudio.PopAnim
{
    /// <summary>
    /// It's all from Disassembling PVZ2 and Zuma's Revenge!
    /// </summary>
    internal class Pam
    {
        public static void Encode(string inFile, string outFile)
        {
            PopAnimInfo pam = new PopAnimInfo();
            using (BinaryStream bs = new BinaryStream(inFile, FileMode.Open))
            {
                using (JsonDocument json = JsonDocument.Parse(bs))
                {
                    pam = ReadPamJson(json.RootElement);
                }
                    //pam = JsonSerializer.Deserialize<PopAnimInfo>(bs, new JsonSerializerOptions { AllowTrailingCommas = true });
            }
            using (BinaryStream bs = new BinaryStream(outFile, FileMode.Create))
            {
                pam.Write(bs);
            }
        }

        static PopAnimInfo ReadPamJson(JsonElement ele)
        {
            PopAnimInfo pam = new PopAnimInfo();
            JsonElement value;
            if (ele.TryGetProperty("version"u8, out value))
            {
                if (value.TryGetInt32(out int v))
                {
                    pam.version = v;
                }
            }
            if (ele.TryGetProperty("frame_rate"u8, out value))
            {
                if (value.TryGetByte(out byte v))
                {
                    pam.frame_rate = v;
                }
            }
            if (ele.TryGetProperty("position"u8, out value))
            {
                if (value.ValueKind == JsonValueKind.Array)
                {
                    pam.position = ReadDoubleArray(value);
                }
            }
            if (ele.TryGetProperty("size"u8, out value))
            {
                if (value.ValueKind == JsonValueKind.Array)
                {
                    pam.size = ReadDoubleArray(value);
                }
            }
            if (ele.TryGetProperty("image"u8, out value))
            {
                if (value.ValueKind == JsonValueKind.Array)
                {
                    pam.image = ReadImageInfoArray(value);
                }
            }
            if (ele.TryGetProperty("sprite"u8, out value))
            {
                if (value.ValueKind == JsonValueKind.Array)
                {
                    pam.sprite = ReadSpriteInfoArray(value);
                }
            }
            if (ele.TryGetProperty("main_sprite"u8, out value))
            {
                pam.main_sprite = ReadSpriteInfo(value);
            }
            return pam;
        }

        static SpriteInfo[] ReadSpriteInfoArray(JsonElement value)
        {
            int l = value.GetArrayLength();
            SpriteInfo[] ans = new SpriteInfo[l];
            for (int i = 0; i < l; i++)
            {
                ans[i] = ReadSpriteInfo(value[i]);
            }
            return ans;
        }

        static SpriteInfo ReadSpriteInfo(JsonElement value)
        {
            if (value.ValueKind == JsonValueKind.Null) return null;
            JsonElement tv;
            SpriteInfo info = new SpriteInfo();
            if (value.TryGetProperty("name"u8, out tv))
            {
                info.name = tv.GetString();
            }
            if (value.TryGetProperty("description"u8, out tv))
            {
                info.description = tv.GetString();
            }
            if (value.TryGetProperty("frame_rate"u8, out tv))
            {
                if (tv.TryGetDouble(out double b))
                {
                    info.frame_rate = b;
                }
            }
            if (value.TryGetProperty("work_area"u8, out tv))
            {
                if (tv.ValueKind == JsonValueKind.Array)
                {
                    info.work_area = ReadInt32Array(tv);
                }
            }
            if (value.TryGetProperty("frame"u8, out tv))
            {
                if (tv.ValueKind == JsonValueKind.Array)
                {
                    info.frame = ReadFrameInfoArray(tv);
                }
            }
            return info;
        }

        static FrameInfo[] ReadFrameInfoArray(JsonElement value)
        {
            int l = value.GetArrayLength();
            FrameInfo[] ans = new FrameInfo[l];
            for (int i = 0; i < l; i++)
            {
                ans[i] = ReadFrameInfo(value[i]);
            }
            return ans;
        }

        static FrameInfo ReadFrameInfo(JsonElement value)
        {
            if (value.ValueKind == JsonValueKind.Null) return null;
            JsonElement tv;
            FrameInfo info = new FrameInfo();
            if (value.TryGetProperty("label"u8, out tv))
            {
                info.label = tv.GetString();
            }
            if (value.TryGetProperty("stop"u8, out tv))
            {
                info.stop = tv.GetBoolean();
            }
            if (value.TryGetProperty("command"u8, out tv))
            {
                if (tv.ValueKind == JsonValueKind.Array)
                {
                    info.command = ReadCommandsInfoArray(tv);
                }
            }
            if (value.TryGetProperty("remove"u8, out tv))
            {
                if (tv.ValueKind == JsonValueKind.Array)
                {
                    info.remove = ReadRemovesInfoArray(tv);
                }
            }
            if (value.TryGetProperty("append"u8, out tv))
            {
                if (tv.ValueKind == JsonValueKind.Array)
                {
                    info.append = ReadAddsInfoArray(tv);
                }
            }
            if (value.TryGetProperty("change"u8, out tv))
            {
                if (tv.ValueKind == JsonValueKind.Array)
                {
                    info.change = ReadMovesInfoArray(tv);
                }
            }
            return info;
        }

        static FrameInfo.MovesInfo[] ReadMovesInfoArray(JsonElement value)
        {
            int l = value.GetArrayLength();
            FrameInfo.MovesInfo[] ans = new FrameInfo.MovesInfo[l];
            for (int i = 0; i < l; i++)
            {
                ans[i] = ReadMovesInfo(value[i]);
            }
            return ans;
        }

        static FrameInfo.MovesInfo ReadMovesInfo(JsonElement value)
        {
            if (value.ValueKind == JsonValueKind.Null) return null;
            JsonElement tv;
            FrameInfo.MovesInfo info = new FrameInfo.MovesInfo();
            if (value.TryGetProperty("index"u8, out tv))
            {
                if (tv.TryGetInt32(out int v))
                {
                    info.index = v;
                }
            }
            if (value.TryGetProperty("transform"u8, out tv))
            {
                if (tv.ValueKind == JsonValueKind.Array)
                {
                    info.transform = ReadDoubleArray(tv);
                }
            }
            if (value.TryGetProperty("color"u8, out tv))
            {
                if (tv.ValueKind == JsonValueKind.Array)
                {
                    info.color = ReadDoubleArray(tv);
                }
            }
            if (value.TryGetProperty("src_rect"u8, out tv))
            {
                if (tv.ValueKind == JsonValueKind.Array)
                {
                    info.src_rect = ReadInt32Array(tv);
                }
            }
            if (value.TryGetProperty("anim_frame_num"u8, out tv))
            {
                if (tv.TryGetInt32(out int v))
                {
                    info.anim_frame_num = v;
                }
            }
            return info;
        }

        static FrameInfo.AddsInfo[] ReadAddsInfoArray(JsonElement value)
        {
            int l = value.GetArrayLength();
            FrameInfo.AddsInfo[] ans = new FrameInfo.AddsInfo[l];
            for (int i = 0; i < l; i++)
            {
                ans[i] = ReadAddsInfo(value[i]);
            }
            return ans;
        }

        static FrameInfo.AddsInfo ReadAddsInfo(JsonElement value)
        {
            if (value.ValueKind == JsonValueKind.Null) return null;
            JsonElement tv;
            FrameInfo.AddsInfo info = new FrameInfo.AddsInfo();
            if (value.TryGetProperty("index"u8, out tv))
            {
                if (tv.TryGetInt32(out int v))
                {
                    info.index = v;
                }
            }
            if (value.TryGetProperty("name"u8, out tv))
            {
                info.name = tv.GetString();
            }
            if (value.TryGetProperty("resource"u8, out tv))
            {
                if (tv.TryGetInt32(out int v))
                {
                    info.resource = v;
                }
            }
            if (value.TryGetProperty("sprite"u8, out tv))
            {
                info.sprite = tv.GetBoolean();
            }
            if (value.TryGetProperty("additive"u8, out tv))
            {
                info.additive = tv.GetBoolean();
            }
            if (value.TryGetProperty("preload_frames"u8, out tv))
            {
                if (tv.TryGetInt32(out int v))
                {
                    info.preload_frames = v;
                }
            }
            if (value.TryGetProperty("timescale"u8, out tv))
            {
                if (tv.TryGetDouble(out double v))
                {
                    info.timescale = (float)v;
                }
            }
            return info;
        }

        static FrameInfo.RemovesInfo[] ReadRemovesInfoArray(JsonElement value)
        {
            int l = value.GetArrayLength();
            FrameInfo.RemovesInfo[] ans = new FrameInfo.RemovesInfo[l];
            for (int i = 0; i < l; i++)
            {
                ans[i] = ReadRemovesInfo(value[i]);
            }
            return ans;
        }

        static FrameInfo.RemovesInfo ReadRemovesInfo(JsonElement value)
        {
            if (value.ValueKind == JsonValueKind.Null) return null;
            JsonElement tv;
            FrameInfo.RemovesInfo info = new FrameInfo.RemovesInfo();
            if (value.TryGetProperty("index"u8, out tv))
            {
                if (tv.TryGetInt32(out int v))
                {
                    info.index = v;
                }
            }
            return info;
        }

        static FrameInfo.CommandsInfo[] ReadCommandsInfoArray(JsonElement value)
        {
            int l = value.GetArrayLength();
            FrameInfo.CommandsInfo[] ans = new FrameInfo.CommandsInfo[l];
            for (int i = 0; i < l; i++)
            {
                ans[i] = ReadCommandsInfo(value[i]);
            }
            return ans;
        }

        static FrameInfo.CommandsInfo ReadCommandsInfo(JsonElement value)
        {
            if (value.ValueKind == JsonValueKind.Null) return null;
            JsonElement tv;
            FrameInfo.CommandsInfo info = new FrameInfo.CommandsInfo();
            if (value.TryGetProperty("command"u8, out tv))
            {
                info.command = tv.GetString();
            }
            if (value.TryGetProperty("parameter"u8, out tv))
            {
                info.parameter = tv.GetString();
            }
            return info;
        }

        static ImageInfo[] ReadImageInfoArray(JsonElement value)
        {
            int l = value.GetArrayLength();
            ImageInfo[] ans = new ImageInfo[l];
            for (int i = 0; i < l; i++)
            {
                ans[i] = ReadImageInfo(value[i]);
            }
            return ans;
        }

        static ImageInfo ReadImageInfo(JsonElement value)
        {
            if (value.ValueKind == JsonValueKind.Null) return null;
            JsonElement tv;
            ImageInfo info = new ImageInfo();
            if (value.TryGetProperty("name"u8, out tv))
            {
                info.name = tv.GetString();
            }
            if (value.TryGetProperty("size"u8, out tv))
            {
                info.size = ReadInt32Array(tv);
            }
            if (value.TryGetProperty("transform"u8, out tv))
            {
                info.transform = ReadDoubleArray(tv);
            }
            return info;
        }

        static int[] ReadInt32Array(JsonElement value)
        {
            int l = value.GetArrayLength();
            int[] ans = new int[l];
            for (int i = 0; i < l; i++)
            {
                ans[i] = value[i].GetInt32();
            }
            return ans;
        }

        static double[] ReadDoubleArray(JsonElement value)
        {
            int l = value.GetArrayLength();
            double[] ans = new double[l];
            for (int i = 0; i < l; i++)
            {
                ans[i] = value[i].GetDouble();
            }
            return ans;
        }

        static string DoubleToString(double value)
        {
            string ans = value.ToString("F7");
            int i = 1;
            while (true)
            {
                char c = ans[^i];
                if (c == '0')
                {
                    i++;
                }
                else if (c == '.')
                {
                    i -= 2;
                    break;
                }
                else
                {
                    i--;
                    break;
                }
            }
            return ans[..^i];
        }

        public static void WriteRawDoubleArray(Utf8JsonWriter writer, double[] doubleValue)
        {
            if (doubleValue == null)
            {
                writer.WriteNullValue();
                return;
            }
            writer.WriteStartArray();
            if (doubleValue.Length != 0)
            {
                int depth = writer.CurrentDepth;
                StringBuilder sb = new StringBuilder();
                sb.Append(Environment.NewLine);
                for (int i = 0; i < depth; i++)
                {
                    sb.Append("  ");
                }
                for (int i = 0; i < doubleValue.Length; i++)
                {
                    writer.WriteRawValue($"{sb}{DoubleToString(doubleValue[i])}");
                }
            }
            writer.WriteEndArray();
        }

        public static void WriteSprite(Utf8JsonWriter writer, SpriteInfo info)
        {
            if (info == null)
            {
                writer.WriteNullValue();
                return;
            }
            writer.WriteStartObject();
            writer.WriteString("name"u8, info.name);
            writer.WriteString("description"u8, info.description);
            writer.WritePropertyName("frame_rate"u8);
            writer.WriteRawValue(DoubleToString(info.frame_rate));
            writer.WritePropertyName("work_area"u8);
            if (info.work_area == null)
            {
                writer.WriteNullValue();
            }
            else
            {
                writer.WriteStartArray();
                for (int i = 0; i < info.work_area.Length; i++)
                {
                    writer.WriteNumberValue(info.work_area[i]);
                }
                writer.WriteEndArray();
            }
            writer.WritePropertyName("frame"u8);
            if (info.frame == null)
            {
                writer.WriteNullValue();
            }
            else
            {
                FrameInfo[] tempFrameInfo = info.frame;
                int length = tempFrameInfo.Length;
                writer.WriteStartArray();
                for (int i = 0; i < length; i++)
                {
                    FrameInfo tempFrame = tempFrameInfo[i];
                    if (tempFrame == null)
                    {
                        writer.WriteNullValue();
                    }
                    else
                    {
                        writer.WriteStartObject();
                        writer.WriteString("label"u8, tempFrame.label);
                        writer.WriteBoolean("stop"u8, tempFrame.stop);
                        writer.WritePropertyName("command"u8);
                        if (tempFrame.command == null)
                        {
                            writer.WriteNullValue();
                        }
                        else
                        {
                            FrameInfo.CommandsInfo[] tempCommandsInfo = tempFrame.command;
                            int clength = tempCommandsInfo.Length;
                            writer.WriteStartArray();
                            for (int j = 0; j < clength; j++)
                            {
                                FrameInfo.CommandsInfo tempCommands = tempCommandsInfo[j];
                                if (tempCommands == null)
                                {
                                    writer.WriteNullValue();
                                }
                                else
                                {
                                    writer.WriteStartObject();
                                    writer.WriteString("command"u8, tempCommands.command);
                                    writer.WriteString("parameter"u8, tempCommands.parameter);
                                    writer.WriteEndObject();
                                }
                            }
                            writer.WriteEndArray();
                        }
                        writer.WritePropertyName("remove"u8);
                        if (tempFrame.remove == null)
                        {
                            writer.WriteNullValue();
                        }
                        else
                        {
                            FrameInfo.RemovesInfo[] tempRemovesInfo = tempFrame.remove;
                            int clength = tempRemovesInfo.Length;
                            writer.WriteStartArray();
                            for (int j = 0; j < clength; j++)
                            {
                                FrameInfo.RemovesInfo tempRemoves = tempRemovesInfo[j];
                                if (tempRemoves == null)
                                {
                                    writer.WriteNullValue();
                                }
                                else
                                {
                                    writer.WriteStartObject();
                                    writer.WriteNumber("index"u8, tempRemoves.index);
                                    writer.WriteEndObject();
                                }
                            }
                            writer.WriteEndArray();
                        }
                        writer.WritePropertyName("append"u8);
                        if (tempFrame.append == null)
                        {
                            writer.WriteNullValue();
                        }
                        else
                        {
                            FrameInfo.AddsInfo[] tempAddsInfo = tempFrame.append;
                            int clength = tempAddsInfo.Length;
                            writer.WriteStartArray();
                            for (int j = 0; j < clength; j++)
                            {
                                FrameInfo.AddsInfo tempAdds = tempAddsInfo[j];
                                if (tempAdds == null)
                                {
                                    writer.WriteNullValue();
                                }
                                else
                                {
                                    writer.WriteStartObject();
                                    writer.WriteNumber("index"u8, tempAdds.index);
                                    writer.WriteString("name"u8, tempAdds.name);
                                    writer.WriteNumber("resource"u8, tempAdds.resource);
                                    writer.WriteBoolean("sprite"u8, tempAdds.sprite);
                                    writer.WriteBoolean("additive"u8, tempAdds.additive);
                                    writer.WriteNumber("preload_frames"u8, tempAdds.preload_frames);
                                    writer.WritePropertyName("timescale"u8);
                                    writer.WriteRawValue(DoubleToString(tempAdds.timescale));
                                    writer.WriteEndObject();
                                }
                            }
                            writer.WriteEndArray();
                        }
                        writer.WritePropertyName("change"u8);
                        if (tempFrame.change == null)
                        {
                            writer.WriteNullValue();
                        }
                        else
                        {
                            FrameInfo.MovesInfo[] tempMovesInfo = tempFrame.change;
                            int clength = tempMovesInfo.Length;
                            writer.WriteStartArray();
                            for (int j = 0; j < clength; j++)
                            {
                                FrameInfo.MovesInfo tempMoves = tempMovesInfo[j];
                                if (tempMoves == null)
                                {
                                    writer.WriteNullValue();
                                }
                                else
                                {
                                    writer.WriteStartObject();
                                    writer.WriteNumber("index"u8, tempMoves.index);
                                    writer.WritePropertyName("transform"u8);
                                    WriteRawDoubleArray(writer, tempMoves.transform);
                                    writer.WritePropertyName("color"u8);
                                    WriteRawDoubleArray(writer, tempMoves.color);
                                    writer.WritePropertyName("src_rect"u8);
                                    if (tempMoves.src_rect == null)
                                    {
                                        writer.WriteNullValue();
                                    }
                                    else
                                    {
                                        writer.WriteStartArray();
                                        for (int k = 0; k < tempMoves.src_rect.Length; k++)
                                        {
                                            writer.WriteNumberValue(tempMoves.src_rect[k]);
                                        }
                                        writer.WriteEndArray();
                                    }
                                    writer.WriteNumber("anim_frame_num"u8, tempMoves.anim_frame_num);
                                    writer.WriteEndObject();
                                }
                            }
                            writer.WriteEndArray();
                        }
                        writer.WriteEndObject();
                    }
                }
                writer.WriteEndArray();
            }
            writer.WriteEndObject();
        }

        public static void Decode(string inFile, string outFile)
        {
            PopAnimInfo pam = new PopAnimInfo();
            using (BinaryStream bs = new BinaryStream(inFile, FileMode.Open))
            {
                pam.Read(bs);
            }
            using (BinaryStream bs = new BinaryStream(outFile, FileMode.Create))
            {
                using (Utf8JsonWriter u8w = new Utf8JsonWriter(bs, new JsonWriterOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, Indented = true }))
                {
                    u8w.WriteStartObject();
                    u8w.WriteNumber("version"u8, pam.version);
                    u8w.WriteNumber("frame_rate"u8, pam.frame_rate);
                    u8w.WritePropertyName("position"u8);
                    WriteRawDoubleArray(u8w, pam.position);
                    u8w.WritePropertyName("size"u8);
                    WriteRawDoubleArray(u8w, pam.size);
                    u8w.WritePropertyName("image"u8);
                    if (pam.image == null)
                    {
                        u8w.WriteNullValue();
                    }
                    else
                    {
                        ImageInfo[] tempImageInfo = pam.image;
                        int n = tempImageInfo.Length;
                        u8w.WriteStartArray();
                        for (int i = 0; i < n; i++)
                        {
                            ImageInfo tempImage = tempImageInfo[i];
                            u8w.WriteStartObject();
                            u8w.WriteString("name"u8, tempImage.name);
                            u8w.WritePropertyName("size"u8);
                            if (tempImage.size == null)
                            {
                                u8w.WriteNullValue();
                            }
                            else
                            {
                                u8w.WriteStartArray();
                                for (int j = 0; j < tempImage.size.Length; j++)
                                {
                                    u8w.WriteNumberValue(tempImage.size[j]);
                                }
                                u8w.WriteEndArray();
                            }
                            u8w.WritePropertyName("transform"u8);
                            WriteRawDoubleArray(u8w, tempImage.transform);
                            u8w.WriteEndObject();
                        }
                        u8w.WriteEndArray();
                    }
                    u8w.WritePropertyName("sprite"u8);
                    if (pam.sprite == null)
                    {
                        u8w.WriteNullValue();
                    }
                    else
                    {
                        SpriteInfo[] tempSpriteInfo = pam.sprite;
                        int n = tempSpriteInfo.Length;
                        u8w.WriteStartArray();
                        for (int i = 0; i < n; i++)
                        {
                            WriteSprite(u8w, tempSpriteInfo[i]);
                        }
                        u8w.WriteEndArray();
                    }
                    u8w.WritePropertyName("main_sprite"u8);
                    WriteSprite(u8w, pam.main_sprite);
                    u8w.WriteEndObject();
                }

                //var setting = new JsonSerializerOptions
                //{
                //    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                //    WriteIndented = true
                //};
                //setting.Converters.Add(new Float64WriteOnlyConverter());
                //setting.Converters.Add(new Float64ArrayWriteOnlyConverter());
                //JsonSerializer.Serialize(bs, pam, setting);
            }
        }
    }
}
