﻿using System.Text;

namespace HexBOT.XboxAuth
{
    public static class JsonUtils
    {
        public static JSONData ParseJson(string json)
        {
            int cursorpos = 0;
            return String2Data(json, ref cursorpos);
        }

        public class JSONData
        {
            public enum DataType { Object, Array, String };
            private DataType type;
            public DataType Type { get { return type; } }
            public Dictionary<string, JSONData> Properties;
            public List<JSONData> DataArray;
            public string StringValue;
            public JSONData(DataType datatype)
            {
                type = datatype;
                Properties = new Dictionary<string, JSONData>();
                DataArray = new List<JSONData>();
                StringValue = string.Empty;
            }
        }

        private static JSONData String2Data(string toparse, ref int cursorpos)
        {
            try
            {
                JSONData data;
                SkipSpaces(toparse, ref cursorpos);
                switch (toparse[cursorpos])
                {
                    //Object
                    case '{':
                        data = new JSONData(JSONData.DataType.Object);
                        cursorpos++;
                        SkipSpaces(toparse, ref cursorpos);
                        while (toparse[cursorpos] != '}')
                        {
                            if (toparse[cursorpos] == '"')
                            {
                                JSONData propertyname = String2Data(toparse, ref cursorpos);
                                if (toparse[cursorpos] == ':') { cursorpos++; } else { /* parse error ? */ }
                                JSONData propertyData = String2Data(toparse, ref cursorpos);
                                data.Properties[propertyname.StringValue] = propertyData;
                            }
                            else cursorpos++;
                        }
                        cursorpos++;
                        break;

                    //Array
                    case '[':
                        data = new JSONData(JSONData.DataType.Array);
                        cursorpos++;
                        SkipSpaces(toparse, ref cursorpos);
                        while (toparse[cursorpos] != ']')
                        {
                            if (toparse[cursorpos] == ',') { cursorpos++; }
                            JSONData arrayItem = String2Data(toparse, ref cursorpos);
                            data.DataArray.Add(arrayItem);
                        }
                        cursorpos++;
                        break;

                    //String
                    case '"':
                        data = new JSONData(JSONData.DataType.String);
                        cursorpos++;
                        while (toparse[cursorpos] != '"')
                        {
                            if (toparse[cursorpos] == '\\')
                            {
                                try //Unicode character \u0123
                                {
                                    if (toparse[cursorpos + 1] == 'u'
                                        && IsHex(toparse[cursorpos + 2])
                                        && IsHex(toparse[cursorpos + 3])
                                        && IsHex(toparse[cursorpos + 4])
                                        && IsHex(toparse[cursorpos + 5]))
                                    {
                                        //"abc\u0123abc" => "0123" => 0123 => Unicode char n°0123 => Add char to string
                                        data.StringValue += char.ConvertFromUtf32(int.Parse(toparse.Substring(cursorpos + 2, 4), System.Globalization.NumberStyles.HexNumber));
                                        cursorpos += 6; continue;
                                    }
                                    else if (toparse[cursorpos + 1] == 'n')
                                    {
                                        data.StringValue += '\n';
                                        cursorpos += 2;
                                        continue;
                                    }
                                    else if (toparse[cursorpos + 1] == 'r')
                                    {
                                        data.StringValue += '\r';
                                        cursorpos += 2;
                                        continue;
                                    }
                                    else if (toparse[cursorpos + 1] == 't')
                                    {
                                        data.StringValue += '\t';
                                        cursorpos += 2;
                                        continue;
                                    }
                                    else cursorpos++; //Normal character escapement \"
                                }
                                catch (IndexOutOfRangeException) { cursorpos++; } // \u01<end of string>
                                catch (ArgumentOutOfRangeException) { cursorpos++; } // Unicode index 0123 was invalid
                            }
                            data.StringValue += toparse[cursorpos];
                            cursorpos++;
                        }
                        cursorpos++;
                        break;

                    //Number
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                    case '.':
                    case '-':
                        data = new JSONData(JSONData.DataType.String);
                        StringBuilder sb = new StringBuilder();
                        while ((toparse[cursorpos] >= '0' && toparse[cursorpos] <= '9') || toparse[cursorpos] == '.' || toparse[cursorpos] == '-')
                        {
                            sb.Append(toparse[cursorpos]);
                            cursorpos++;
                        }
                        data.StringValue = sb.ToString();
                        break;

                    //Boolean : true
                    case 't':
                        data = new JSONData(JSONData.DataType.String);
                        cursorpos++;
                        if (toparse[cursorpos] == 'r') { cursorpos++; }
                        if (toparse[cursorpos] == 'u') { cursorpos++; }
                        if (toparse[cursorpos] == 'e') { cursorpos++; data.StringValue = "true"; }
                        break;

                    //Boolean : false
                    case 'f':
                        data = new JSONData(JSONData.DataType.String);
                        cursorpos++;
                        if (toparse[cursorpos] == 'a') { cursorpos++; }
                        if (toparse[cursorpos] == 'l') { cursorpos++; }
                        if (toparse[cursorpos] == 's') { cursorpos++; }
                        if (toparse[cursorpos] == 'e') { cursorpos++; data.StringValue = "false"; }
                        break;

                    //Null field
                    case 'n':
                        data = new JSONData(JSONData.DataType.String);
                        cursorpos++;
                        if (toparse[cursorpos] == 'u') { cursorpos++; }
                        if (toparse[cursorpos] == 'l') { cursorpos++; }
                        if (toparse[cursorpos] == 'l') { cursorpos++; data.StringValue = "null"; }
                        break;

                    //Unknown data
                    default:
                        cursorpos++;
                        return String2Data(toparse, ref cursorpos);
                }
                SkipSpaces(toparse, ref cursorpos);
                return data;
            }
            catch (IndexOutOfRangeException)
            {
                return new JSONData(JSONData.DataType.String);
            }
        }

        private static bool IsHex(char c) { return ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f')); }

        private static void SkipSpaces(string toparse, ref int cursorpos)
        {
            while (cursorpos < toparse.Length
                    && (char.IsWhiteSpace(toparse[cursorpos])
                    || toparse[cursorpos] == '\r'
                    || toparse[cursorpos] == '\n'))
                cursorpos++;
        }
    }
}
