using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using RegExHelper.Initialization;
using RegExHelper.Data;
using RegExHelper.Data.Enums;
using System.Diagnostics;

namespace RegExHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static RegexOptions Options;
        public static Help help;
        public IsoStore history = new IsoStore();

        public MainWindow()
        {
            InitializeComponent();
            Initialize.UIControls(this);
        }

        #region Methods
        public void Validate()
        {
            if (string.IsNullOrEmpty(cmbRegEx.Text))
            {
                txtResult.Clear();
            }
            else
            {
                try
                {
                    var matches = Regex.Matches(txtInput.Text, cmbRegEx.Text, Options);

                    StringBuilder sb = new StringBuilder();


                    foreach (Match match in matches)
                    {
                        sb.Append(match.Value);
                        if (match.Index != matches.Count - 1)
                        {
                            sb.Append("\n");
                        }
                    }
                    txtResult.Text = sb.ToString();
                }
                catch (Exception ex)
                {
                    txtResult.Text = ex.Message;
                }
            }
        }

        public void FadeInAndOut(FrameworkElement element)
        {
            Storyboard storyboard = new Storyboard();
            TimeSpan duration = TimeSpan.FromMilliseconds(500);
                
            DoubleAnimation fadeInAnimation = new DoubleAnimation()
                { From = 0.0, To = 1.0, Duration = new Duration(duration) };

            DoubleAnimation fadeOutAnimation = new DoubleAnimation()
                { From = 1.0, To = 0.0, Duration = new Duration(duration) };
            fadeOutAnimation.BeginTime = TimeSpan.FromSeconds(5);

            Storyboard.SetTargetName(fadeInAnimation, element.Name);
            Storyboard.SetTargetProperty(fadeInAnimation, new PropertyPath("Opacity", 1));
            storyboard.Children.Add(fadeInAnimation);
            storyboard.Begin(element);

            Storyboard.SetTargetName(fadeOutAnimation, element.Name);
            Storyboard.SetTargetProperty(fadeOutAnimation, new PropertyPath("Opacity", 0));
            storyboard.Children.Add(fadeOutAnimation);
            storyboard.Begin(element);
        }

        public void StatusNotification(FrameworkElement element, Status status)
        {
            switch (status)
            {
                case Status.Saved:
                    lblStatus.Content = "Saved...";
                    break;
                case Status.Cleared:
                    lblStatus.Content = "Cleared all history...";
                    break;
                case Status.Removed:
                    lblStatus.Content = "Removed itemz...LOL";
                    break;
            }
            FadeInAndOut(element);
        }
        #endregion

        #region EventHandlers
        internal void txtInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            Validate();
        }

        internal void cmbRegEx_TextChanged(object sender, TextChangedEventArgs e)
        {
            Validate();
        }

        internal void cmbRegEx_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !String.IsNullOrWhiteSpace(cmbRegEx.Text) && !cmbRegEx.Items.Contains(cmbRegEx.Text))
            {
                history.StoreHistory(cmbRegEx.Text);
                history.UpdateHistory(cmbRegEx);
                StatusNotification(lblStatus, Status.Saved);
            }
        }

        internal void cmbRegEx_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Delete && cmbRegEx.SelectedItem != null )
            {
                var currentItemIndex = cmbRegEx.SelectedIndex;

                history.RemoveItem(cmbRegEx, cmbRegEx.SelectedItem);
                history.UpdateHistory(cmbRegEx);

                if (cmbRegEx.Items.Count - 1 >= currentItemIndex)
                    cmbRegEx.SelectedIndex = currentItemIndex;
                else
                    cmbRegEx.SelectedIndex = currentItemIndex -1;

                StatusNotification(lblStatus, Status.Removed);
            }
        }

        private void Hyperlink_RequestNavigate_Options(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void Hyperlink_RequestNavigate_RegEx(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        internal void cmbOptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Options = (RegexOptions)cmbOptions.SelectedItem;
            Validate();
        }

        internal void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtInput.Text = string.Empty;
            cmbRegEx.Text = string.Empty;
            history.ClearHistory(cmbRegEx);
            StatusNotification(lblStatus, Status.Cleared);
        }

        internal void chbOnTop_Checked(object sender, RoutedEventArgs e)
        {
            this.Topmost = true;
            if (help != null)
                help.Topmost = true;

        }

        internal void chbOnTop_Unchecked(object sender, RoutedEventArgs e)
        {
            this.Topmost = false;
            if (help != null)
                help.Topmost = false;
        }

        internal void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            if (help == null)
            {
                help = new Help();
                help.Closed += help_Closed;

                Initialize.HelpText(help);
                if (chbOnTop.IsChecked.Value)
                    help.Topmost = true;

                help.Show();
            }
            else
            {
                help.Activate();
            }
        }

        static void help_Closed(object sender, EventArgs e)
        {
            help = null;
        }
        #endregion
    }
}
