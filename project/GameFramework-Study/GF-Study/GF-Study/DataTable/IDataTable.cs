
using System;
using System.Collections.Generic;

namespace GameFramework.DataTable
{
    public interface IDataTable<T> : IEnumerable<T> where T : IDataRow
    {
        string name
        {
            get;
        }
        Type type
        {
            get;
        }
        int Count
        {
            get;
        }
        T this[int id]
        {
            get;
        }

        bool HasDataRow(int id);
        bool HasDataRow(Predicate<T> condition);
        T[] GetAllDataRows();

    }
}