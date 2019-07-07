using Microsoft.AspNetCore.Components;
using System;
using System.Threading;

namespace ComponentLifecycles.Pages
{
    public class IndexBase : ComponentBase, IDisposable
    {
        protected Timer Timer;

        public void Dispose()
        {
            Timer?.Dispose();
            Timer = null;
            System.Diagnostics.Debug.WriteLine("Disposed");
        }
    }
}
