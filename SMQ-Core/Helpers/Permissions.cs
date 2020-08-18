namespace SMQCore.Helpers
{
    public sealed class Permissions
    {
        private Permissions()
        {
        }

        public static string SuperUser => "SuperUser";

        public static string AppAdmin => "AppAdmin";

        public static string User => "User";
    }
}