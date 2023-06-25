using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask_ContractSystems
{
    public class Statistics
    {
        private Dictionary<string, int> _frequencyData;

        public int FilesCount { get; private set; }

        public Statistics(Dictionary<string, int> frequencyData) 
        { 
            _frequencyData = frequencyData;

            FilesCount = GetFilesCount();
        }

        private int GetFilesCount()
        {
            int fc = 0;

            foreach(var e in _frequencyData)
            {
                fc += _frequencyData[e.Key];
            }

            return fc;
        }
    }
}
