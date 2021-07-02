using System.Windows.Media;

namespace NetSpeed.Util
{
    internal static class ColorUtil
    {
        public static Color HexToDecColor(string hexColor)
        {
            return new Color
            {
                A = 255,
                R = (byte)HexToDec(hexColor.Substring(1, 2)),
                G = (byte)HexToDec(hexColor.Substring(3, 2)),
                B = (byte)HexToDec(hexColor.Substring(5, 2))
            };
        }

        public static string DecToHexColor(Color decColor) => $"#{DecToHex(decColor.R)}{DecToHex(decColor.G)}{DecToHex(decColor.B)}";

        private static int HexToDec(string hex)
        {
            int high = hex[0];
            int low = hex[1];
            high -= high > 97 ? 87 : (high >= 65 ? 55 : 48);
            low -= low > 97 ? 87 : (low >= 65 ? 55 : 48);
            return (high * 16) + low;
        }

        private static string DecToHex(byte dec)
        {
            byte high = (byte)((dec & 0xF0) >> 4);
            byte low = (byte)(dec & 0x0F);
            return $"{(char)(high + (high < 10 ? 48 : 55))}{(char)(low + (low < 10 ? 48 : 55))}";
        }
    }
}
