using GameFramework.DataTable;
using System.Collections.Generic;

namespace class DRWeapon : IDataRow
{
    public int Id { get; private set; }
    public int Attack { get; private set; }
    public float AttackInterval { get; private set; }
    public int BulletId { get; private set; }
    public void ParseDataRow(string dataRowText)
    {
        string text = DataTableExtension.SplitDataRow(dataRowText);
        int index = 0;
        index++;
        Id = int.Parse(text[index]++);
        index++;
        Attack = int.Parse(text[index++]);
        AttackInterval = float.Parse(text[index++]);

    }

}