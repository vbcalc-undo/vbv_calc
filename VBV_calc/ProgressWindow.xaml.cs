using System.Drawing;
using System.Windows;

namespace VBV_calc
{
    /// <summary>
    /// ProgressWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ProgressWindow : Window
    {
        private readonly MainWindow _mainWindow;
        private int step = 0;

        public ProgressWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            Loaded += ProgressWindow_Loaded;
        }

        private async void ProgressWindow_Loaded(object sender, RoutedEventArgs e)
        {
            progressBar.Value = 0;

            // --- ヘルパー関数 ---
            async Task NextAsync(string message, Action action)
            {
                step++;
                progressBar.Value = step;
                statusText.Text = message;

                // UIスレッドで処理
                Dispatcher.Invoke(() => action());

                // UI反映のため少し待つ
                await Task.Delay(50);
            }
            async Task<TResult> NextAsync_withReturn<TResult>(string message, Func<TResult> func)
            {
                step++;
                progressBar.Value = step;
                statusText.Text = message;

                TResult result; // 戻り値を保持する変数を宣言

                // UIスレッドで処理を実行し、戻り値を変数に格納
                // Action() ではなく func() を呼び出す
                result = Dispatcher.Invoke(() => func());

                // UI反映のため少し待つ
                await Task.Delay(50);

                // 戻り値を返す
                return result;
            }

            //string path = @"C:\Temp\capture.png";
            string noSpace = "";

            //using Bitmap bmp = new Bitmap(path);

            var cropRect_chara = new System.Drawing.Rectangle(315, 85, 190, 190);//名前
            var cropRect_shogo = new System.Drawing.Rectangle(593, 75, 300, 27);//称号
            var cropRect_equip1 = new System.Drawing.Rectangle(558, 245, 231, 22);//装備1
            var cropRect_equip2 = new System.Drawing.Rectangle(558, 271, 231, 22);//装備2
            var cropRect_ryoshoku = new System.Drawing.Rectangle(558, 297, 231, 22);//糧食

            // --- 処理順に呼ぶ ---
            await NextAsync("キャラ 読み込み中...", () => _mainWindow.load_from_game(315, 85, 190, 190));
            noSpace = await NextAsync_withReturn("装備1 読み込み中...", () => _mainWindow.cropedAndselect(cropRect_equip1, 3, 220));
            if (noSpace != null)
                await NextAsync("装備1 設定中...", () => _mainWindow.SelectMostSimilarEquipment(noSpace, 0));
            noSpace = await NextAsync_withReturn("装備2 読み込み中...", () => _mainWindow.cropedAndselect(cropRect_equip2, 3, 220));
            if (noSpace != null)
                await NextAsync("装備2 設定中...", () => _mainWindow.SelectMostSimilarEquipment(noSpace, 1));
            noSpace = await NextAsync_withReturn("糧食 読み込み中...", () => _mainWindow.cropedAndselect(cropRect_ryoshoku, 3, 220));
            if (noSpace != null)
                await NextAsync("糧食 設定中...", () => _mainWindow.SelectMostSimilarEquipment(noSpace, 2));
            noSpace = await NextAsync_withReturn("称号 読み込み中...", () => _mainWindow.cropedAndselect(cropRect_shogo, 3, 220));
            if (noSpace != null)
            {
                await NextAsync("称号 設定中...", () => _mainWindow.best_match_shogo(noSpace));
            }

            // 完了表示
            statusText.Text = "完了しました！";
            progressBar.Value = progressBar.Maximum;

            await Task.Delay(500);
            Close();
        }
    }
}
