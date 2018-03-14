
using System;
namespace GameFramework.DataTable
{
    public abstract class DataTableBase
    {
        private readonly string m_Name;

        public DataTableBase()
            : this(null)
        {

        }
        public DataTableBase(string name)
        {
            m_Name = name ?? string.Empty;
        }
        public string Name
        {
            get
            {
                return m_Name;
            }
        }
        internal abstract void AddDataRow(string dataRowText);
        internal abstract void Shutdown();
    }
}