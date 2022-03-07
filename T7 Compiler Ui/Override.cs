using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Infinity_Loader_3._0
{
    public class OverrideCursor : IDisposable
    {

        public OverrideCursor(Cursor changeToCursor)
        {
            Mouse.OverrideCursor = changeToCursor;
        }

        #region IDisposable Members

        public void Dispose()
        {
            Mouse.OverrideCursor = null;
        }

        #endregion
    }
}
