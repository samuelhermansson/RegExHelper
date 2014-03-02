using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Text.RegularExpressions;
using RegExHelper.Data;
using RegExHelper.Data.Enums;
using System.Reflection;

namespace RegExHelper.Initialization
{
    public static class Initialize
    {
        public static void UIControls(MainWindow mainWindow)
        {
            mainWindow.lblInput.Content = "Input text:";
            mainWindow.lblResult.Content = "Result:";
            mainWindow.btnClear.Content = "Clear";
            mainWindow.btnHelp.Content = "Help";

            mainWindow.lblStatus.Content = string.Empty;
            mainWindow.lblStatus.Opacity = 0;

            mainWindow.chbOnTop.Content = "Always On Top";
            
            mainWindow.txtInput.Text = string.Empty;
            mainWindow.txtResult.Text = string.Empty;

            mainWindow.history.CreateIsoStoreFiles();
            mainWindow.history.UpdateHistory(mainWindow.cmbRegEx);

            var options = Enum.GetValues(typeof(RegexOptions));

            foreach (var item in options)
            {
                mainWindow.cmbOptions.Items.Add(item);
            }

            mainWindow.cmbOptions.SelectedIndex = 0;

            mainWindow.txtResult.Background = Brushes.Gainsboro;

            mainWindow.txtInput.AcceptsReturn = true;
            mainWindow.cmbRegEx.IsEditable = true;
            mainWindow.cmbRegEx.IsTextSearchEnabled = false;

            mainWindow.txtResult.IsReadOnly = true;
            mainWindow.txtResult.IsReadOnlyCaretVisible = true;
            mainWindow.txtInput.VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto;
            mainWindow.txtResult.VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto;

            mainWindow.txtInput.TabIndex = 0;
            mainWindow.cmbRegEx.TabIndex = 1;
            mainWindow.txtResult.TabIndex = 2;
            mainWindow.cmbOptions.TabIndex = 3;
            mainWindow.btnClear.TabIndex = 4;
            mainWindow.btnHelp.TabIndex = 5;
            mainWindow.chbOnTop.TabIndex = 6;
            mainWindow.txtInput.Focus();

            mainWindow.txtInput.TextChanged += mainWindow.txtInput_TextChanged;

            mainWindow.cmbRegEx.KeyDown += mainWindow.cmbRegEx_KeyDown;
            mainWindow.cmbRegEx.PreviewKeyDown += mainWindow.cmbRegEx_PreviewKeyDown;
            mainWindow.btnClear.Click += mainWindow.btnClear_Click;
            mainWindow.cmbOptions.SelectionChanged += mainWindow.cmbOptions_SelectionChanged;
            mainWindow.chbOnTop.Checked += mainWindow.chbOnTop_Checked;
            mainWindow.chbOnTop.Unchecked += mainWindow.chbOnTop_Unchecked;
            mainWindow.btnHelp.Click += mainWindow.btnHelp_Click;
        }

        public static void HelpText(Help help)
        {
            help.txtHelp.IsReadOnly = true;
            help.txtHelp.Background = Brushes.Gainsboro;
            help.txtHelp.VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto;
            help.lblDescription.Content = Properties.Resources.AppDescription;
            help.lblVersion.Content = Assembly.GetEntryAssembly().GetName().Version.ToString();
            help.lblTitle.Content = Properties.Resources.HelpTitle;
            help.lblTitle.FontWeight = System.Windows.FontWeights.Bold;
            help.txtHelp.Text = Properties.Resources.HelpText;
        }
    }
}
