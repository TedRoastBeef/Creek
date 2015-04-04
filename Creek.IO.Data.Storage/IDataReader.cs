namespace Creek.Data.Storage
{
    public interface IDataReader
    {
        void SetData(Data data);

        void ReadData();

        void ReadData(string path);
    }
}
