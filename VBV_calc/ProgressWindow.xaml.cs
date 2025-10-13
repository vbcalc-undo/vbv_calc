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

        public ProgressWindow(MainWindow mainWindow,int i)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            if(i==0)
                Loaded += ProgressWindow_Loaded;
            else if(i==1)
                Loaded += enemy_ProgressWindow_Loaded;
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

            string path = @".\Temp\capture.png";
            string noSpace = "";

            using Bitmap bmp = new Bitmap(path);
            /*
            var cropRect_chara = new System.Drawing.Rectangle(315, 85, 190, 190);//名前
            var cropRect_shogo = new System.Drawing.Rectangle(593, 75, 300, 27);//称号
            var cropRect_equip1 = new System.Drawing.Rectangle(558, 245, 231, 22);//装備1
            var cropRect_equip2 = new System.Drawing.Rectangle(558, 271, 231, 22);//装備2
            var cropRect_ryoshoku = new System.Drawing.Rectangle(558, 297, 231, 22);//糧食
            */
            // 元の解像度
            int originalWidth = 1920;
            int originalHeight = 1080;
            // 実際の解像度
            float scaleX = (float)bmp.Width / originalWidth;
            float scaleY = (float)bmp.Height / originalHeight;

            // スケーリング済みの矩形
            var cropRect_chara = new System.Drawing.Rectangle(
                (int)(315 * scaleX),
                (int)(85 * scaleY),
                (int)(190 * scaleX),
                (int)(190 * scaleY));

            var cropRect_shogo = new System.Drawing.Rectangle(
                (int)(593 * scaleX),
                (int)(75 * scaleY),
                (int)(300 * scaleX),
                (int)(27 * scaleY));

            var cropRect_equip1 = new System.Drawing.Rectangle(
                (int)(558 * scaleX),
                (int)(245 * scaleY),
                (int)(231 * scaleX),
                (int)(22 * scaleY));

            var cropRect_equip2 = new System.Drawing.Rectangle(
                (int)(558 * scaleX),
                (int)(271 * scaleY),
                (int)(231 * scaleX),
                (int)(22 * scaleY));

            var cropRect_ryoshoku = new System.Drawing.Rectangle(
                (int)(558 * scaleX),
                (int)(297 * scaleY),
                (int)(231 * scaleX),
                (int)(22 * scaleY));
            int sw = (int)(315 * scaleX);
            int sh = (int)(85 * scaleY);
            int ew = (int)(190 * scaleX);
            int eh = (int)(190 * scaleY);

            // --- 処理順に呼ぶ ---
            await NextAsync("キャラ 読み込み中...", () => _mainWindow.load_from_game(sw, sh, ew, eh));
            noSpace = await NextAsync_withReturn("装備1 読み込み中...", () => _mainWindow.cropedAndselect(cropRect_equip1, 3, 220));
            if (noSpace != null)
                await NextAsync("装備1 設定中...", () => _mainWindow.SelectMostSimilarEquipment(noSpace, 0,0));
            noSpace = await NextAsync_withReturn("装備2 読み込み中...", () => _mainWindow.cropedAndselect(cropRect_equip2, 3, 220));
            if (noSpace != null)
                await NextAsync("装備2 設定中...", () => _mainWindow.SelectMostSimilarEquipment(noSpace, 1,0));
            noSpace = await NextAsync_withReturn("糧食 読み込み中...", () => _mainWindow.cropedAndselect(cropRect_ryoshoku, 3, 220));
            if (noSpace != null)
                await NextAsync("糧食 設定中...", () => _mainWindow.SelectMostSimilarEquipment(noSpace, 2,0));
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
        private async void enemy_ProgressWindow_Loaded(object sender, RoutedEventArgs e)
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

            string path = @".\Temp\capture.png";
            string noSpace = "";

            using Bitmap bmp = new Bitmap(path);
            /*
            var cropRect_chara = new System.Drawing.Rectangle(315, 85, 190, 190);//名前
            var cropRect_shogo = new System.Drawing.Rectangle(593, 75, 300, 27);//称号
            var cropRect_equip1 = new System.Drawing.Rectangle(558, 245, 231, 22);//装備1
            var cropRect_equip2 = new System.Drawing.Rectangle(558, 271, 231, 22);//装備2
            var cropRect_ryoshoku = new System.Drawing.Rectangle(558, 297, 231, 22);//糧食
            */
            // 元の解像度
            int originalWidth = 1920;
            int originalHeight = 1080;
            // 実際の解像度
            float scaleX = (float)bmp.Width / originalWidth;
            float scaleY = (float)bmp.Height / originalHeight;

            // スケーリング済みの矩形
            var cropRect_chara = new System.Drawing.Rectangle(
                (int)(866 * scaleX),
                (int)(371 * scaleY),
                (int)(209 * scaleX),
                (int)(209 * scaleY));

            var cropRect_equip1 = new System.Drawing.Rectangle(
                (int)(618 * scaleX),
                (int)(646 * scaleY),
                (int)(188 * scaleX),
                (int)(23 * scaleY));

            var cropRect_equip2 = new System.Drawing.Rectangle(
                (int)(618 * scaleX),
                (int)(670 * scaleY),
                (int)(188 * scaleX),
                (int)(23 * scaleY));
            var cropRect_ryoshoku = new System.Drawing.Rectangle(
                (int)(618 * scaleX),
                (int)(694 * scaleY),
                (int)(188 * scaleX),
                (int)(23 * scaleY));
            var cropRect_level = new System.Drawing.Rectangle(
                (int)(620 * scaleX),
                (int)(385 * scaleY),
                (int)(53 * scaleX),
                (int)(27 * scaleY));
            int sw = (int)(865 * scaleX);
            int sh = (int)(370 * scaleY);
            int ew = (int)(210 * scaleX);
            int eh = (int)(210 * scaleY);

            // --- 処理順に呼ぶ ---
            await NextAsync("キャラ 読み込み中...", () => _mainWindow.enemy_load_from_game(sw, sh, ew, eh));
            noSpace = await NextAsync_withReturn("装備1 読み込み中...", () => _mainWindow.cropedAndselect(cropRect_equip1, 3, 220));
            if (noSpace != null)
                await NextAsync("装備1 設定中...", () => _mainWindow.SelectMostSimilarEquipment(noSpace, 0,1));
            noSpace = await NextAsync_withReturn("装備2 読み込み中...", () => _mainWindow.cropedAndselect(cropRect_equip2, 3, 220));
            if (noSpace != null)
                await NextAsync("装備2 設定中...", () => _mainWindow.SelectMostSimilarEquipment(noSpace, 1, 1));
            noSpace = await NextAsync_withReturn("糧食 読み込み中...", () => _mainWindow.cropedAndselect(cropRect_ryoshoku, 3, 220));
            if (noSpace != null)
                await NextAsync("糧食 設定中...", () => _mainWindow.SelectMostSimilarEquipment(noSpace, 2, 1));
            noSpace = await NextAsync_withReturn("レベル 読み込み中...", () => _mainWindow.cropedAndselect_number(cropRect_level, 2, 200));
            if (noSpace != null)
                await NextAsync("レベル 設定中...", () => _mainWindow.Set_SimilarVariable(noSpace, 0));
            // 完了表示
            statusText.Text = "完了しました！";
            progressBar.Value = progressBar.Maximum;

            await Task.Delay(500);
            Close();
        }

    }
}
