using System;
using System.Media;
using System.Windows;

namespace KyberBrowser.Dialogs {
    /// <summary>
    /// Interaction logic for ExceptionWindow.xaml
    /// </summary>
    public partial class ExceptionDialog : Window {
        public ExceptionDialog(Exception ex, string title, bool isUnhandled, string messagePrefix) {
            InitializeComponent();

            Title = title;
            Window mainWindow = Application.Current.MainWindow;
            Icon = mainWindow.Icon;

            // Play according sounds
            if (isUnhandled) 
                SystemSounds.Hand.Play();
            else 
                SystemSounds.Exclamation.Play();

            // Create exception message
            string message = ex.Message;
            if (messagePrefix != null) 
                message = messagePrefix + Environment.NewLine + message;
            if (ex.InnerException != null)
                message += Environment.NewLine + ex.InnerException;
            message += Environment.NewLine + ex.StackTrace;
            ExceptionText.Text = message;

            CloseButton.Click += (s, e) => Close();
            CopyButton.Click += (s, e) => Clipboard.SetDataObject(message);
        }

        public static void Show(Exception ex, string title, bool isUnhandled = false, string messagePrefix = null) {
            Application.Current.Dispatcher.Invoke(() => {
                ExceptionDialog window = new(ex, title, isUnhandled, messagePrefix);
                window.ShowDialog();
            });
        }
    }
}
