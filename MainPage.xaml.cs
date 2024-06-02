using System.Collections.Generic;
using System.Timers;
namespace PhonePadApp;

public partial class MainPage : ContentPage
{
    // Variables to store the current text, number of key presses, and a flag for first key press
    private string _currentText = string.Empty;
    private int _times = 0;
    private bool _isFirst = true;

    // Timer for debouncing key presses
    private System.Timers.Timer _debounceTimer;

    // Dictionary to map button keys to corresponding letters
    private Dictionary<string, List<string>> _buttonMappings = new Dictionary<string, List<string>>()
    {
        { "1", new List<string> { "&", "'", "(" } },
        { "2", new List<string> { "A", "B", "C" } },
        { "3", new List<string> { "D", "E", "F" } },
        { "4", new List<string> { "G", "H", "I" } },
        { "5", new List<string> { "J", "K", "L" } },
        { "6", new List<string> { "M", "N", "O" } },
        { "7", new List<string> { "P", "Q", "R", "S" } },
        { "8", new List<string> { "T", "U", "V" } },
        { "9", new List<string> { "W", "X", "Y", "Z" } },
        { "0", new List<string> { " " } },
        { "*", new List<string> { "x" } },
        { "#", new List<string> { "send" } },
    };

    public MainPage()
    {
        InitializeComponent();
         // Initialize the timer with a 1000ms (1 second) interval
        _debounceTimer = new System.Timers.Timer(1000);
        _debounceTimer.Elapsed += OnDebounceTimerElapsed!;
    }

    // Event handler for button clicks
    private void OnButtonClicked(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            HandleKeyPress(button.Text[0]);
        }
    }

    private void OnButtonPressed(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                // Change the button's background color to indicate it's being pressed
                button.BackgroundColor = Colors.Gray;
                button.TextColor = Colors.White;
            }
        }

        private void OnButtonReleased(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                // Revert the button's background color to its original state
                button.BackgroundColor = Colors.White;
                button.TextColor = Colors.DarkBlue;
            }
        }


    // Method to handle key presses
    private void HandleKeyPress(char key)
    {
        if (key == '*')
        {
            if (!string.IsNullOrEmpty(_currentText))
            {
                _currentText = _currentText.Substring(0, _currentText.Length - 1);
            }
        }
        else if (key == '#')
        {
            DisplayAlert("Send", $"You've sent {_currentText}", "OK");
            _currentText = string.Empty;
        }
        else
        {
            // Handle character input based on the key
            string keyString = key.ToString();
            List<string> letters = _buttonMappings[keyString];

            // Check if the current text's last character is in the letters list
            if (!string.IsNullOrEmpty(_currentText) && letters.Contains(_currentText[^1].ToString()))
            {
                if (_isFirst)
                {
                    // Append the first letter on first key press
                    _currentText += letters[0];
                    _isFirst = false;
                }
                else
                {
                    // Cycle through the letters on subsequent presses
                    _times++;
                    _currentText = _currentText.Substring(0, _currentText.Length - 1);
                    _currentText += letters[_times % letters.Count];
                }
            }
            else
            {
                 // start with the first letter for new key
                _isFirst = false;
                _times = 0;
                _currentText += letters[0];
            }
        }

        // Restart the debounce timer to reset the state after the interval
        _debounceTimer.Stop();
        _debounceTimer.Start();

        DisplayLabel.Text = _currentText;
    }

    // Event handler for the debounce timer elapsed event
    private void OnDebounceTimerElapsed(object sender, ElapsedEventArgs e)
    {
        _debounceTimer.Stop();
        _times = 0;
        _isFirst = true;
    }
}

