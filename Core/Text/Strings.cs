using System.Linq;
using System.Text;

namespace Core.Text
{
    public class Strings
    {
        public static string ToString(bool[] array)
        {
            return string.Join("", array.Select(x => x ? '1' : '0'));
        }

        public static string ToString(byte[] array)
        {
            return string.Join("", array);
        }

        public static string ToString(int[] array)
        {
            return string.Join("", array);
        }

        public static string ToString(char[] array)
        {
            return string.Join("", array);
        }

        public static string ToString(bool[,] array)
        {
            var builder = new StringBuilder();
            for (int y = 0; y < array.GetLength(1); y++)
            {
                for (int x = 0; x < array.GetLength(0); x++)
                {
                    builder.Append(array[x, y] ? '1' : '0');
                }
                builder.AppendLine();
            }
            return builder.ToString();
        }

        public static string ToString(byte[,] array)
        {
            var builder = new StringBuilder();
            for (int y = 0; y < array.GetLength(1); y++)
            {
                for (int x = 0; x < array.GetLength(0); x++)
                {
                    builder.Append(array[x, y]);
                }
                builder.AppendLine();
            }
            return builder.ToString();
        }

        public static string ToString(int[,] array)
        {
            var builder = new StringBuilder();
            for (int y = 0; y < array.GetLength(1); y++)
            {
                for (int x = 0; x < array.GetLength(0); x++)
                {
                    builder.Append(array[x, y]);
                }
                builder.AppendLine();
            }
            return builder.ToString();
        }

        public static string ToString(char[,] array)
        {
            var builder = new StringBuilder();
            for (int y = 0; y < array.GetLength(1); y++)
            {
                for (int x = 0; x < array.GetLength(0); x++)
                {
                    builder.Append(array[x, y]);
                }
                builder.AppendLine();
            }
            return builder.ToString();
        }
    }
}
