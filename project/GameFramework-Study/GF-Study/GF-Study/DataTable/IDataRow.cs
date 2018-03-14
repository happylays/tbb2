
namespace GameFramewrok.DataTable
{
    public interface IDataRow
    {
        int Id
        {
            get;
        }
        void ParseDataRow(string dataRowText);
    }
}