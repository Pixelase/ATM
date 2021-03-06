﻿using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using ATM.Core;

namespace ATM.Input
{
    public class MoneyReaderXml : IMoneyReader
    {
        private readonly string _path;

        public MoneyReaderXml(string path)
        {
            _path = path;
        }

        public Money ReadMoney()
        {
            Stream stream = new FileStream(_path, FileMode.Open);
            var ds = new DataContractSerializer(typeof (Money));
            var money = (Money) ds.ReadObject(stream);
            var banknotes = new SortedList<Banknote, int>(new DescendingComparer<Banknote>());
            foreach (var banknote in money.Banknotes)
            {
                banknotes.Add(banknote.Key, banknote.Value);
            }
            money.Banknotes = banknotes;
            stream.Close();
            return money;
        }
    }
}
