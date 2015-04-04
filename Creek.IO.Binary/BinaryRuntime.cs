namespace Creek.IO.Binary
{
    public class BinaryRuntime
    {
        private static TypeBinaryDict data = new TypeBinaryDict();

        public static void Add(IBinary b)
        {
            data.AddC(b);
        }
        public static void Add<T>() where T : IBinary, new()
        {
            Add(new T());
        }

        public static TypeBinaryDict Gets()
        {
            return data;
        }

    }
}
