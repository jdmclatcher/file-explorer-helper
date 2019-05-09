using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorerHelper
{
    class FindAndReplace : Util
    {
        private Util util;
        public FindAndReplace(Util utilClass)
        {
            util = utilClass;
        }

        public void FindAndReplaceFiles(string toRemove, string toReplace)
        {
            util.ReplaceAllFiles(toRemove, toReplace);
        }
    }
}
