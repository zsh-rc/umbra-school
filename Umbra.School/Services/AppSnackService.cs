using MudBlazor;
using System;
using System.Collections.Generic;
using System.Text;
using static MudBlazor.Defaults.Classes;

namespace Umbra.School.Services
{
    public class AppSnackbarService
    {
        private readonly ISnackbar _snackbar;

        public AppSnackbarService(ISnackbar snackbar)
        {
            _snackbar = snackbar;
            _snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopCenter;
        }

        public void ShowInfo(string message) =>
            _snackbar.Add(string.IsNullOrWhiteSpace(message)
                ? "Information"
                : message, Severity.Info);

        public void ShowSuccess(string message) =>
            _snackbar.Add(string.IsNullOrWhiteSpace(message)
                ? "Operation completed successfully!"
                : message, Severity.Success);

        public void ShowError(string message) =>
            _snackbar.Add(string.IsNullOrWhiteSpace(message)
                ? "Operation failed!"
                : message, Severity.Error);

        public void Show(string message, Severity severity)
        {
            _snackbar.Add(message, severity);
        }
    }

}
