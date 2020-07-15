using System;
using System.Collections.Generic;
using System.Text;
using Vinci.FileSaver.Interface;

namespace Vinci.FileSaver
{
    public static class Storage
    {

        static IPathResolver pathResolver;
        public static IPathResolver PathHelper { get => pathResolver ?? (pathResolver = new PathResolver()); }
        static LocalStorage local;
        public static IStorage Local { get => local ?? (local = new LocalStorage()); }



        //public static IStorage Remote { get; set; }
    }
}
