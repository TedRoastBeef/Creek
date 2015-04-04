using System.Collections.Generic;
using System.IO;

namespace Creek.Tools
{
    public class Wallet
    {
        private readonly List<Transaction> Transactions = new List<Transaction>();

        public void Add(string id, double price)
        {
            Transactions.Add(new Transaction {Id = id, Price = price});
        }

        public void Save(Stream s)
        {
            var bw = new BinaryWriter(s);
            bw.Write(Transactions.Count);
            foreach (Transaction transaction in Transactions)
            {
                bw.Write(transaction.Id);
                bw.Write(transaction.Price);
            }
            bw.Flush();
            bw.Close();
        }

        public static Wallet Load(Stream s)
        {
            var br = new BinaryReader(s);
            var r = new Wallet();

            int i = br.ReadInt32();
            for (int j = 0; j < i; j++)
            {
                r.Transactions.Add(new Transaction {Id = br.ReadString(), Price = br.ReadInt32()});
            }

            return r;
        }
    }

    public class Transaction
    {
        public string Id;
        public double Price;
    }
}