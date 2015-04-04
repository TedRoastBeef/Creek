namespace Creek.I18N.Internal
{
    internal class BinaryRuntime
    {
        private static TypeBinaryDict data = new TypeBinaryDict();

        public static void Add(Creek.I18N.Internal.Binary b)
        {
            data.AddC(b.GetType(), b);
        }
        public static void Add<T>() where T : Creek.I18N.Internal.Binary, new()
        {
            Add(new T());
        }

        public static TypeBinaryDict Gets()
        {
            return data;
        }

    }
}
