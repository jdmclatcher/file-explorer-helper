using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorerHelper
{
    class FindAndRemove
    {
        private Util util;
        public FindAndRemove(Util utilClass)
        {
            util = utilClass;
        }

        public void FindAndRemoveFiles(string toRemove)
        {
            // use "" as string to replace with to implement a removal
            util.ReplaceAllFiles(toRemove, "");
        }

    }
}
