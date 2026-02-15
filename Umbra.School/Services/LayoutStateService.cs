namespace Umbra.School.Services
{
    public class LayoutStateService
    {
        private bool _drawerOpen = false;
        private bool _drawerMini = true;
        private bool _darkMode = false;

        public bool DrawerOpen => _drawerOpen;
        public bool DrawerMini => _drawerMini;
        public bool DarkMode => _darkMode;

        public event Action? OnChange;

        public void ToggleDrawer()
        {
            _drawerOpen = !_drawerOpen;
            Notify();
        }

        public void SetDrawer(bool open)
        {
            _drawerOpen = open;
            Notify();
        }

        public void ToggleMini()
        {
            _drawerMini = !_drawerMini;
            Notify();
        }

        public void ToggleTheme()
        {
            _darkMode = !_darkMode;
            Notify();
        }

        private void Notify() => OnChange?.Invoke();
    }

}
