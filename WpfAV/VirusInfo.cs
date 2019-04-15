using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAV
{
    /// <summary>
    ///класс, содержащий информацию о вирусе и зараженном файле
    ///</summary>
    public class VirusInfo
    {
        private string path;
        private string virusName;

        public VirusInfo(string path, string virusName)
        {
            this.path = path;
            this.virusName = virusName;
        }

        public string Path
        {
            get { return path; }
        }

        public string VirusName
        {
            get { return virusName; }
        }
    }
}
