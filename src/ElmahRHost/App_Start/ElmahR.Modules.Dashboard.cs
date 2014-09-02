using ElmahRHost;

[assembly: WebActivator.PostApplicationStartMethod( typeof(Dashboard), "PostStart")]

namespace ElmahRHost {
    public static class Dashboard {
        public static void PostStart() {
            global::ElmahR.Modules.Dashboard.Modules.Bootstrapper.Bootstrap();
        }
    }
}