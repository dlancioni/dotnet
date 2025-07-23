// Services/UserStateService.cs
using System;

namespace BlazorApp1.Services // Adjust namespace to your project's namespace
{
    public class UserStateService
    {
        private string _username = "";

        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    NotifyStateChanged(); // Notify subscribers when the username changes
                }
            }
        }

        // Event to notify components when the state changes
        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();

        // Optional: Method to clear the user state (e.g., on logout)
        public void ClearUser()
        {
            Username = ""; // Set to default or empty
        }
    }
}