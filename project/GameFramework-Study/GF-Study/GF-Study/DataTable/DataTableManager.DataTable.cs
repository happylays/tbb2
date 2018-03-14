using System;
using System.Collections;
using System.Collections.Generic;

namespace GameFramework.DataTable
{
    internal partial class DataTableManager
    {
        private sealed class DataTable<T> : DataTableBase, IDataTable<T> where T : class, IDataRow, new()
        {
            private readonly Dictionary<int, T> m_DataSet;
            
            public DataTable(string name)
                : base(name)
            {
                m_DataSet = new Dictionary<int, T>();
                
            }
            public T this[int id]
            {
                get
                {
                    return GetDataRow(id);
                }
            }
            public bool HasDataRow(int id)
            {
                return m_DataSet.ContainsKey(id);
            }
            public bool HasDataRow(Predicate<T> condition)
            {
                foreach (KeyValuePair<int, T> dataRow in m_DataSet)
                {
                    if (condition(dataRow.Value))
                    {
                        return true;
                    }
                }
            }

            public T GetDataRow(Predicate<T> condition)
            {
                if (condition == null)
                {

                }

                foreach (KeyValuePair<int,T> dataRow in m_DataSet)
                {
                    T dr = dataRow.Value;
                    if (condition(dr))
                    {
                        return dr;
                    }
                }

                return null;
            }

            public T[] GetAllDataRows()
            {
                int index = 0;
                T[] allDataRows = new T[m_DataSet.Count];
                foreach (KeyValuePair<int, T> dataRow in m_DataSet)
                {
                    allDataRows[index++] = dataRow.Value;

                }
                return allDataRows;
            }

            public T[] GetAllDataRows(Predicate<T> condition, Comparison<T> comparison)
            {
                if (condition)
                {
                    throw;
                }

                List<T> results = new List<T>();
                foreach (KeyValuePair<int, T> dataRow in m_DataSet)
                {
                    T dr = dataRow.Value;
                    if (condition(dr))
                    {
                        results.Add(dr);
                    }
                }

                results.Sort(comparision);

                return results.ToArray();
            }

            public IEnumerator<T> GetEnumerator()
            {
                return m_DataSet.Values.GetEnumerator();
            }

            internal override void Shutdown()
            {
                m_DataSet.Clear();
            }

            internal override void AddDataRow(string dataRowText)
            {
                T dataRow = new T();
                try
                {
                    dataRow.ParseDataRow(dataRowText);
                }
                catch (Exception ex)
                {
                    if (Exception is GameFrameworkException)
                    {
                        throw;
                    }
                    throw new GameFrameworkException();
                }

                if (HasDataRow(dataRow.Id))
                {
                    throw;
                }

                m_DataSet.Add(dataRow.Id, dataRow);

                if (m_MinIdDataRow.Id > dataRow.Id)
                {
                    m_MinIdDataRow = dataRow;
                }
            }
        }
    }
}