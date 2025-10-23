using System.Windows;

namespace VBV_formation
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

            int selectedNumber = _mainWindow.shidan_capture_selectedNum;

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

            // --- 処理順に呼ぶ ---
            await NextAsync("キャラ1 読み込み中...", () => _mainWindow.load_from_game(80, 400 + selectedNumber * 110, 83, 60, 1));
            await NextAsync("キャラ2 読み込み中...", () => _mainWindow.load_from_game(230, 400 + selectedNumber * 110, 83, 60, 2));
            await NextAsync("キャラ3 読み込み中...", () => _mainWindow.load_from_game(381, 400 + selectedNumber * 110, 83, 60, 3));
            await NextAsync("キャラ4 読み込み中...", () => _mainWindow.load_from_game(531, 400 + selectedNumber * 110, 83, 60, 4));
            await NextAsync("キャラ5 読み込み中...", () => _mainWindow.load_from_game(682, 400 + selectedNumber * 110, 83, 60, 5));
            await NextAsync("キャラ6 読み込み中...", () => _mainWindow.load_from_game(833, 400 + selectedNumber * 110, 83, 60, 6));
            await NextAsync("アシスト更新中...", () => _mainWindow.resync_assist_skill());
            await NextAsync("活性更新中...", () => _mainWindow.character_kassei_update());
            await NextAsync("指揮更新中...", () => _mainWindow.character_shiki_update());
            await NextAsync("加護計算中...", () => _mainWindow.kago_calc());

            // 完了表示
            statusText.Text = "完了しました！";
            progressBar.Value = progressBar.Maximum;

            await Task.Delay(500);
            Close();
        }
    }
}
