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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace VBV_calc
{
    public partial class SaveWindow : Window
    {
        public enum SaveChoice { Overwrite, SaveAsNew, Cancel }
        public SaveChoice Choice { get; private set; } = SaveChoice.Cancel;

        public SaveWindow(string name)
        {
            InitializeComponent();
            messageTextBlock.Text = $"「{name}」\nは既に存在します。どうしますか？";
        }

        private void OverwriteButton_Click(object sender, RoutedEventArgs e)
        {
            Choice = SaveChoice.Overwrite;
            DialogResult = true;
            Close();
        }

        private void SaveAsNewButton_Click(object sender, RoutedEventArgs e)
        {
            Choice = SaveChoice.SaveAsNew;
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Choice = SaveChoice.Cancel;
            DialogResult = false;
            Close();
        }
    }
}
