namespace Creek.Data.Storage
{
    public interface IDataWriter
    {
        void SetData(Data data);

        void WriteData();

        void WriteData(string path);
    }
}
