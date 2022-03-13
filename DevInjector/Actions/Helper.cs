using System;
using System.Threading;

namespace DevInjector.Actions
{
    internal class Helper
    {
        public static void StaThreadWrapper(Action action)
        {
            var t = new Thread(o =>
            {
                action();
                System.Windows.Threading.Dispatcher.Run();
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }
    }
}
