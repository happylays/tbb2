
using GameFramework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public class DefaultDataTableHelper : DataTableHelperBase
    {
        private DataTableComponent m_DataTableComponent = null;
        private ResourceComponent m_ResourceComponent = null;

        public override string[] SplitToDataRows(string text) {
            List<string> texts = new List<string>();
            string[] rowTexts = Utility.Text.SplitToLines(text);
            for (int i = 0; i < rowTexts.Length; i++)
            {
                if (rowTexts[i].Length <= 0 || rowTexts[i][0] == '#')
                {
                    continue;
                }
                texts.Add(rowTexts[i]);
            }
            return texts.ToArray();
        }
        public override void ReleaseDataTableAsset(object dataTableAsset)
        {
            m_ResourceComponent.UnloadAsset(dataTableAsset);
        }
        protected override bool LoadDataTable(Type dataRowType, string dataTableName, string dataTableNameType, object dataTableAsset, object userData)
        {
            TextAsset textAsset = dataTableAsset as TextAsset;

            m_DataTableComponent.CreateDataTable(dataRowType, dataTableNameInType, textAsset.text);
            return true;
        }
        private void Start()
        {
            m_DataTableComponent = GameEntry.GetComponent<DataTableComponent>();
        }
    }
}