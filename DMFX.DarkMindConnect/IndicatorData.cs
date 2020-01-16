using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMFX.DarkMindConnect
{
    class IndicatorData
    {
        public string Name
        {
            get;
            set;
        }

        public string Code
        {
            get;
            set;
        }

        public Dictionary<string, IndicatorSeriesData> Series
        {
            get;
            set;
        }
        public Dictionary<int, string> SeriesNames
        {
            get;
            set;
        }

        public IndicatorData()
        {
            Series = new Dictionary<string, IndicatorSeriesData>();
            SeriesNames = new Dictionary<int, string>();
        }

        public void SetColumns(IList<string> columns)
        {
            Series.Clear();
            SeriesNames.Clear();
            for (int i = 0; i < columns.Count; ++i)
            {
                IndicatorSeriesData data = new IndicatorSeriesData();
                data.Name = columns[i];
                Series.Add(columns[i], data);
                SeriesNames.Add(i, columns[i]);
            }
        }
    }
}
