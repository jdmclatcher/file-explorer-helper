using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorerHelper
{
    // for all helper methods

    // also for storing root path to folder
    // and other constant data
    public class Util
    {
        private string rootPath;
        public Util()
        {
            rootPath = null;
        }
        public string getRootPath()
        {
            return this.rootPath;
        }

        public void setRootPath(string rootPath)
        {
            this.rootPath = rootPath;
        }
    }
}
