using System;
using System.Collections.Generic;
using System.Text;

namespace Vinci.FileSaver
{
    public static class Storage
    {
        static LocalStorage local;
        public static LocalStorage Local { get => local ?? (local = new LocalStorage()); }
    }
}
