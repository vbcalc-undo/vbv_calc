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

namespace VBV_formation
{

    /// <summary>
    /// SaveWindow.xaml の相互作用ロジック
    /// </summary>
    public enum SaveDialogResult
    {
        Overwrite,
        SaveAsNew,
        Cancel
    }
    public partial class SaveWindow : Window
    {

        public SaveDialogResult Result { get; private set; }

        public SaveWindow(string name, string itemType = "データ")
        {
            InitializeComponent();

            if (!string.IsNullOrEmpty(name))
            {
                MessageTextBlock.Text = $"「{name}」という{itemType}は既に存在します。\nどうしますか？";
            }
            else
            {
                MessageTextBlock.Text = $"同名の{itemType}が存在します。\nどうしますか？";
            }
        }

        private void OverwriteButton_Click(object sender, RoutedEventArgs e)
        {
            Result = SaveDialogResult.Overwrite;
            DialogResult = true;
        }

        private void SaveAsNewButton_Click(object sender, RoutedEventArgs e)
        {
            Result = SaveDialogResult.SaveAsNew;
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Result = SaveDialogResult.Cancel;
            DialogResult = false;
        }
    }
}