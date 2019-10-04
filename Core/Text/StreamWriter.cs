using System;
using System.IO;

namespace Core.Text
{
    public class StreamWriter
    {
        public TextWriter OutputStream { get; set; }

        public StreamWriter()
        {
            OutputStream = Console.Out;
        }

        public void WriteLine(bool[] array)
        {
            OutputStream.WriteLine(Strings.ToString(array));
        }

        public void WriteLine(byte[] array)
        {
            OutputStream.WriteLine(Strings.ToString(array));
        }

        public void WriteLine(int[] array)
        {
            OutputStream.WriteLine(Strings.ToString(array));
        }

        public void WriteLine(char[] array)
        {
            OutputStream.WriteLine(Strings.ToString(array));
        }

        public void WriteLine(bool[,] array)
        {
            OutputStream.WriteLine(Strings.ToString(array));
        }

        public void WriteLine(byte[,] array)
        {
            OutputStream.WriteLine(Strings.ToString(array));
        }

        public void WriteLine(int[,] array)
        {
            OutputStream.WriteLine(Strings.ToString(array));
        }

        public void WriteLine(char[,] array)
        {
            OutputStream.WriteLine(Strings.ToString(array));
        }
    }
}
