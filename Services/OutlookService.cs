namespace IncidentTracker.Services
{
    public static class OutlookService
    {
        public static string GetCurrentUserName()
        {
            try
            {
                var outlookType = Type.GetTypeFromProgID("Outlook.Application");
                if (outlookType != null)
                {
                    dynamic? outlook = Activator.CreateInstance(outlookType);
                    if (outlook != null)
                    {
                        string name = outlook.Session.CurrentUser.Name;
                        if (!string.IsNullOrWhiteSpace(name))
                            return name;
                    }
                }
            }
            catch { /* Outlook not available */ }

            // Fallback to Windows display name
            try
            {
                var fullName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                var parts = fullName.Split('\\');
                if (parts.Length > 1) return parts[^1];
                return fullName;
            }
            catch { }

            return Environment.UserName;
        }
    }
}
