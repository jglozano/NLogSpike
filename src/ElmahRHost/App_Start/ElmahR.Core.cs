using ElmahRHost;

[assembly: WebActivator.PreApplicationStartMethod( typeof(Core), "PreStart")]
[assembly: WebActivator.PostApplicationStartMethod(typeof(Core), "PostStart")]

namespace ElmahRHost {
    public static class Core {
        private static bool preBootstrapped = false;
    
        public static void PreStart() {
            global::ElmahR.Core.Modules.Bootstrapper.PreBootstrap();
            preBootstrapped = true;
        }
        
        public static void PostStart() {
            if (!preBootstrapped)
                global::ElmahR.Core.Modules.Bootstrapper.PreBootstrap();
            global::ElmahR.Core.Modules.Bootstrapper.Bootstrap();
        }
    }
}