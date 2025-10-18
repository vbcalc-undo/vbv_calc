using JsonFileIO.Jsons;
using JsonFileIO.Jsons;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Shapes;
using Tesseract;
using VBV_calc.Helpers;
using VBV_calc.Models;
using Windows.Devices.Sensors;
using Windows.Media.Ocr;
using Windows.UI.ViewManagement;


namespace VBV_Screen2Wiki
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            all_equipments = new Dictionary<string, List<EquipmentJson>>();
            all_equipment1 = new List<ItemSet>();
            all_equipment2 = new List<ItemSet>();
            passib1_skill = new List<ItemSet>();
            passib2_skill = new List<ItemSet>();
            passib3_skill = new List<ItemSet>();
            passib4_skill = new List<ItemSet>();
            passib5_skill = new List<ItemSet>();
            passib6_skill = new List<ItemSet>();
            passib7_skill = new List<ItemSet>();
            passib8_skill = new List<ItemSet>();
            all_ryoshoku = new List<ItemSet>();

            load_json_equipment();
            EquipmentBox1.ItemsSource = all_equipment1;
            EquipmentBox1.DisplayMemberPath = "ItemDisp";
            EquipmentBox1.SelectedValuePath = "ItemValue";

            EquipmentBox2.ItemsSource = all_equipment2;
            EquipmentBox2.DisplayMemberPath = "ItemDisp";
            EquipmentBox2.SelectedValuePath = "ItemValue";

            ryoshokuBox.ItemsSource = all_ryoshoku;
            ryoshokuBox.DisplayMemberPath = "ItemDisp";
            ryoshokuBox.SelectedValuePath = "ItemValue";

            _engine_number.SetVariable("tessedit_char_whitelist", "0123456789"); // 数字のみ
            _engine_shuzoku.SetVariable("tessedit_char_whitelist", "男女人魔神獣陸樹海竜器死蟲炎雷氷毒飛騎夜超全"); // 特定の漢字のみ

            string jsonPath_element = @"feature_extraction/element_feature.json";              // Python特徴量DB
            string jsonPath_job = @"feature_extraction/job_feature.json";              // Python特徴量DB
            string jsonPath_equipment = @"feature_extraction/equipment_feature.json";              // Python特徴量DB
            string jsonPathleader = @"feature_extraction/leader_feature.json";              // Python特徴量DB
            string csvPath_element = @"feature_extraction/element_list.csv";                   // ID→名前
            string csvPath_job = @"feature_extraction/job_list.csv";                   // ID→名前
            string csvPath_equipment = @"feature_extraction/equipment_list.csv";                   // ID→名前
            string csvPath_leader = @"feature_extraction/leader_list.csv";                   // ID→名前
            string json_element = File.ReadAllText(jsonPath_element);
            string json_job = File.ReadAllText(jsonPath_job);
            string json_equipment = File.ReadAllText(jsonPath_equipment);
            string json_leader = File.ReadAllText(jsonPathleader);
            featureDict_element = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, float[]>>(json_element);
            featureDict_job = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, float[]>>(json_job);
            featureDict_equipment = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, float[]>>(json_equipment);
            featureDict_leader = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, float[]>>(json_leader);
            idNameMap_element = File.ReadAllLines(csvPath_element)
            .Skip(1)
            .Select(line => line.Split(','))
            .ToDictionary(parts => parts[1], parts => parts[2]);
            features_element = featureDict_element.Select(kv => new ImageFeature { Id = kv.Key, Feature = kv.Value }).ToList();
            idNameMap_job = File.ReadAllLines(csvPath_job)
            .Skip(1)
            .Select(line => line.Split(','))
            .ToDictionary(parts => parts[1], parts => parts[2]);
            features_job = featureDict_job.Select(kv => new ImageFeature { Id = kv.Key, Feature = kv.Value }).ToList();
            idNameMap_equipment = File.ReadAllLines(csvPath_equipment)
            .Skip(1)
            .Select(line => line.Split(','))
            .ToDictionary(parts => parts[1], parts => parts[2]);
            features_equipment = featureDict_equipment.Select(kv => new ImageFeature { Id = kv.Key, Feature = kv.Value }).ToList();
            idNameMap_leader = File.ReadAllLines(csvPath_leader)
            .Skip(1)
            .Select(line => line.Split(','))
            .ToDictionary(parts => parts[1], parts => parts[2]);
            features_leader = featureDict_leader.Select(kv => new ImageFeature { Id = kv.Key, Feature = kv.Value }).ToList();

            inputName_element = session.InputMetadata.Keys.First();
            inputName_job = session.InputMetadata.Keys.First();
            inputName_equipment = session.InputMetadata.Keys.First();
            inputName_leader = session.InputMetadata.Keys.First();
        }


        private static InferenceSession session = new InferenceSession(@"feature_extraction/mobilenetv2.onnx");
        private static readonly TesseractEngine _engine = new TesseractEngine(@"./tessdata", "jpn", EngineMode.LstmOnly);
        private static readonly TesseractEngine _engine_shuzoku = new TesseractEngine(@"./tessdata", "jpn", EngineMode.LstmOnly);
        private static readonly TesseractEngine _engine_number = new TesseractEngine(@"./tessdata", "digits", EngineMode.Default);
        private Bitmap reusableBitmap_job = new Bitmap(224, 224);
        private float[] reusableTensorBuffer_job = new float[3 * 224 * 224]; // RGB 224x224
        private Bitmap reusableBitmap_leader = new Bitmap(224, 224);
        private float[] reusableTensorBuffer_leader = new float[3 * 224 * 224]; // RGB 224x224
        private Bitmap reusableBitmap_equipment = new Bitmap(224, 224);
        private float[] reusableTensorBuffer_equipment = new float[3 * 224 * 224]; // RGB 224x224
        private Bitmap reusableBitmap_element = new Bitmap(224, 224);
        private float[] reusableTensorBuffer_element = new float[3 * 224 * 224]; // RGB 224x224
        private static Dictionary<string, float[]> featureDict_job;
        private static Dictionary<string, float[]> featureDict_element;
        private static Dictionary<string, float[]> featureDict_equipment;
        private static Dictionary<string, float[]> featureDict_leader;
        List<ImageFeature> features_job;
        List<ImageFeature> features_element;
        List<ImageFeature> features_equipment;
        List<ImageFeature> features_leader;
        private static Dictionary<string, string> idNameMap_job;
        private static Dictionary<string, string> idNameMap_element;
        private static Dictionary<string, string> idNameMap_equipment;
        private static Dictionary<string, string> idNameMap_leader;
        private string inputName_element;
        private string inputName_equipment;
        private string inputName_job;
        private string inputName_leader;

        class ImageFeature
        {
            public string Id { get; set; }
            public float[] Feature { get; set; }
        }


        List<ItemSet> all_equipment1;
        List<ItemSet> all_equipment2;
        List<ItemSet> passib1_skill;
        List<ItemSet> passib2_skill;
        List<ItemSet> passib3_skill;
        List<ItemSet> passib4_skill;
        List<ItemSet> passib5_skill;
        List<ItemSet> passib6_skill;
        List<ItemSet> passib7_skill;
        List<ItemSet> passib8_skill;
        List<ItemSet> all_ryoshoku;
        List<string> all_skils = new List<string> {" ", "火炎砲弾", "水流砲弾", "氷撃砲弾", "雷撃砲弾", "毒気砲弾", "神術砲弾", "魔術砲弾", "強酸砲弾", "超火炎砲", "超水流砲", "超氷撃砲", "超雷撃砲", "超毒気砲", "超神術砲", "超魔術砲", "超強酸砲", "竜の吐息", "砲撃障壁", "砲撃結界", "砲撃反射", "砲撃吸収", "解毒抗体", "酸化抗体", "四法結界", "火炎放射", "水流放射", "氷撃放射", "雷撃放射", "毒気放射", "神術放射", "魔術放射", "強酸放射", "大火炎陣", "大水流陣", "大氷撃陣", "大雷撃陣", "大毒気陣", "大神術陣", "大魔術陣", "大強酸陣", "超火炎陣", "超水流陣", "超氷撃陣", "超雷撃陣", "超毒気陣", "超神術陣", "超魔術陣", "超強酸陣", "竜の吐息", "対術障壁", "対術結界", "対術反射", "対術吸収", "解毒抗体", "酸化抗体", "四法結界", "自己治癒", "対象治癒", "全体治癒", "平等治癒", "魔族医療", "軍団治癒", "日中再生", "夜間再生", "回帰治癒", "グルメ魂", "術式増幅", "運命改変", "運命の輪", "城壁構築", "城壁崩し", "バリアー", "解毒治療", "解呪治療", "麻痺治療", "削減治療", "絶対治療", "異常耐性", "勇猛果敢", "思考停止", "虹の毒撃", "毒化攻撃", "麻痺攻撃", "呪の一撃", "魅了攻撃", "封印攻撃", "解除攻撃", "攻撃削減", "防御削減", "速度削減", "知力削減", "吸血攻撃", "凍結攻撃", "停止攻撃", "挑発行為", "強制異常", "貫通攻撃", "扇形攻撃", "十字攻撃", "全域攻撃", "軍団攻撃", "側面無効", "遠隔無効", "貫通無効", "扇形無効", "十字無効", "全域無効", "範囲無効", "側面攻撃", "遠隔攻撃", "確率追撃", "必殺増加", "致命必殺", "カブト割", "全力攻撃", "反撃耐性", "多段攻撃", "心核穿ち", "不殺の誓", "無効喰い", "巨人狩り", "闇雲攻撃", "暴走攻撃", "次元斬撃", "集約攻撃", "疾風迅雷", "乾坤一擲", "連携攻撃", "火事場力", "捨身の備", "聖戦の導", "凍傷気流", "特攻防御", "無貌の血", "パリング", "ブロック", "恐怖の瞳", "イベイド", "専守防衛", "前進防御", "標的後逸", "堅守体躯", "神魔体躯", "矮小体躯", "竜鱗守護", "リカバリ", "リバイブ", "必殺耐性", "致命耐性", "受け流し", "無形体躯", "反撃倍加", "先陣の誉", "次元障壁", "至高の盾", "追撃阻止", "火事場力", "不動の備", "聖戦の導", "凍傷気流", "自決自爆", "自爆障壁", "自爆結界", "男性活性", "女性活性", "人間活性", "魔族活性", "神族活性", "獣族活性", "陸生活性", "樹霊活性", "海洋活性", "竜族活性", "器兵活性", "死者活性", "蟲族活性", "炎霊活性", "雷霊活性", "氷霊活性", "毒性活性", "飛行活性", "騎士活性", "夜行活性", "超越活性", "師団活性", "軍団活性", "火属活性", "水属活性", "風属活性", "土属活性", "光属活性", "闇属活性", "男性指揮", "女性指揮", "人間指揮", "魔族指揮", "神族指揮", "獣族指揮", "陸生指揮", "樹霊指揮", "海洋指揮", "竜族指揮", "器兵指揮", "死者指揮", "蟲族指揮", "炎霊指揮", "雷霊指揮", "氷霊指揮", "毒性指揮", "飛行指揮", "騎士指揮", "夜行指揮", "超越指揮", "師団指揮", "軍団指揮", "火属指揮", "水属指揮", "風属指揮", "土属指揮", "光属指揮", "闇属指揮", "攻撃指揮", "防御指揮", "速度指揮", "知力指揮", "攻撃布陣", "防御布陣", "速度布陣", "知力布陣", "攻勢転化", "守勢転化", "速勢転化", "知勢転化", "武具研磨", "狂奔の牙", "報復の牙", "背水の陣", "竜歌覚醒", "竜歌共鳴", "狂戦士化", "加速進化", "英雄覇気", "血の起源", "太陽信仰", "夜行生物", "日中適応", "夜戦適応", "男性弱体", "女性弱体", "人間弱体", "魔族弱体", "神族弱体", "獣族弱体", "陸生弱体", "樹霊弱体", "海洋弱体", "竜族弱体", "器兵弱体", "死者弱体", "蟲族弱体", "炎霊弱体", "雷霊弱体", "氷霊弱体", "毒性弱体", "飛行弱体", "騎士弱体", "夜行弱体", "超越弱体", "師団弱体", "軍団弱体", "火属弱体", "水属弱体", "風属弱体", "土属弱体", "光属弱体", "闇属弱体", "攻撃弱体", "防御弱体", "速度弱体", "知力弱体", "男性支配", "女性支配", "人間支配", "魔族支配", "神族支配", "獣族支配", "陸生支配", "樹霊支配", "海洋支配", "竜族支配", "器兵支配", "死者支配", "蟲族支配", "炎霊支配", "雷霊支配", "氷霊支配", "毒性支配", "飛行支配", "騎士支配", "夜行支配", "超越支配", "師団支配", "軍団支配", "火属支配", "水属支配", "風属支配", "土属支配", "光属支配", "闇属支配", "攻撃支配", "防御支配", "速度支配", "知力支配", "死の軍勢", "戦術障壁", "戦術結界", "戦術妨害", "戦術補助", "戦意高揚", "奇襲戦法", "奇襲警戒", "行動増加", "行動阻害", "トレハン", "撃破金運", "グルメ魂", "エリート", "サボリ癖", "資源工面", "愚者の嘘", "兵士運搬", "地形無効", "正々堂々", "決戦領域" };


        string hozon_text ="";
        List<string> STANCE_LIST = new List<string> { "", "防備", "計略", "進撃", "乱戦" };
        List<string> SHOKUGYO_LIST = new List<string> { "", "ブレイダー", "ランサー", "シューター", "キャスター", "ガーダー", "デストロイヤー", "ヒーラー" };
        Dictionary<string, List<EquipmentJson>> all_equipments; // ここで宣言

        int status_hp = 0;
        int status_kougeki = 0;
        int status_bougyo = 0;
        int status_sokudo = 0;
        int status_chiryoku = 0;
        int status_level = 0;
        string status_rank = "";
        int status_cost = 0;

        public class ItemSet
        {
            // DisplayMemberとValueMemberにはプロパティで指定する仕組み
            public String ItemDisp { get; set; }
            public string ItemValue { get; set; }

            // プロパティをコンストラクタでセット
            public ItemSet(String v, String s)
            {
                ItemDisp = s;
                ItemValue = v;
            }
        }
        private void load_json_equipment_func(string filename, string equipmentname)
        {         // ここにJSON読み込みのコードを追加
            List<EquipmentJson> equipments = null; // ここで宣言
            using (var sr = new StreamReader(filename, System.Text.Encoding.UTF8))
            {
                var jsonReadData = sr.ReadToEnd();

                equipments = JsonConvert.DeserializeObject<List<EquipmentJson>>(jsonReadData);
                all_equipments.Add(equipmentname, equipments);
                foreach (var item in equipments)
                {
                    if (equipmentname != "糧食")
                    {
                        all_equipment1.Add(new ItemSet(item.名称, item.名称));
                        all_equipment2.Add(new ItemSet(item.名称, item.名称));
                    }
                    else
                    {
                        all_ryoshoku.Add(new ItemSet(item.名称, item.名称));
                    }
                }
            }
        }

        private void load_json_equipment()
        {         // ここにJSON読み込みのコードを追加
            all_equipment1.Add(new ItemSet(null, null));
            all_equipment2.Add(new ItemSet(null, null));
            all_ryoshoku.Add(new ItemSet(null, null));
            load_json_equipment_func(@"./json/equipments\soubi_katate.json", "片手");
            load_json_equipment_func(@"./json/equipments\soubi_ryoute.json", "両手");
            load_json_equipment_func(@"./json/equipments\soubi_onoyari.json", "斧槍");
            load_json_equipment_func(@"./json/equipments\soubi_yumiya.json", "射撃");
            load_json_equipment_func(@"./json/equipments\soubi_tue.json", "杖");
            load_json_equipment_func(@"./json/equipments\soubi_muchi.json", "鞭");
            load_json_equipment_func(@"./json/equipments\soubi_tume.json", "爪");
            load_json_equipment_func(@"./json/equipments\soubi_tate.json", "盾");
            load_json_equipment_func(@"./json/equipments\soubi_juso.json", "獣装");
            load_json_equipment_func(@"./json/equipments\soubi_yoroi.json", "鎧");
            load_json_equipment_func(@"./json/equipments\soubi_houi.json", "法衣");
            load_json_equipment_func(@"./json/equipments\soubi_soushoku.json", "装飾");
            load_json_equipment_func(@"./json/equipments\soubi_dougu.json", "道具");
            load_json_equipment_func(@"./json/equipments\soubi_ryoushoku.json", "糧食");
        }

        private void box_to_markdown(bool save_flag)
        {
            string name = Name_box.Text?.Trim() ?? "";
            string kago = kago_box.Text?.Trim() ?? "";
            string shokugyou = shokugyou_box.Text?.Trim() ?? "";
            string hp = hp_box.Text?.Trim() ?? "";
            string kougeki = kougeki_box.Text?.Trim() ?? "";
            string bougyo = bougyo_box.Text?.Trim() ?? "";
            string sokudo = sokudo_box.Text?.Trim() ?? "";
            string chiryoku = chiryoku_box.Text?.Trim() ?? "";
            string shuzoku = shuzoku_box.Text?.Trim() ?? "";
            string tokkou = tokkou_box.Text?.Trim() ?? "";
            string soubi = soubi_box.Text?.Trim() ?? "";
            string rank = rank_box.Text?.Trim() ?? "";
            string cost = cost_box.Text?.Trim() ?? "";
            string passive1 = passive1_box.Text?.Trim() ?? "";
            string passive2 = passive2_box.Text?.Trim() ?? "";
            string passive3 = passive3_box.Text?.Trim() ?? "";
            string passive4 = passive4_box.Text?.Trim() ?? "";
            string passive5 = passive5_box.Text?.Trim() ?? "";
            string passive6 = passive6_box.Text?.Trim() ?? "";
            string passive7 = passive7_box.Text?.Trim() ?? "";
            string passive8 = passive8_box.Text?.Trim() ?? "";
            string leader1 = leader1_box.Text?.Trim() ?? "";
            string leader2 = leader2_box.Text?.Trim() ?? "";
            string assist = assist_box.Text?.Trim() ?? "";
            string naisei = naisei_box.Text?.Trim() ?? "";
            string sutance = stance_box.Text?.Trim() ?? "";
            string bikou = bikou_box.Text?.Trim() ?? "";
            int[] passive_figs = new int[8];
            TextBox[] passive_boxes =
            {
                passive1_box, passive2_box, passive3_box, passive4_box,
                passive5_box, passive6_box, passive7_box, passive8_box
            };
            for (int i = 0; i < passive_boxes.Length; i++)
            {
                int.TryParse(passive_boxes[i].Text?.Trim() ?? "", out passive_figs[i]);
            }

            wikitext_box.Text = hozon_text+"|"+name;
            wikitext_box.Text += "|" + kago;
            wikitext_box.Text += "|" + shokugyou;
            wikitext_box.Text += "|" + hp;
            wikitext_box.Text += "|" + kougeki;
            wikitext_box.Text += "|" + bougyo;
            wikitext_box.Text += "|" + sokudo;
            wikitext_box.Text += "|" + chiryoku;
            wikitext_box.Text += "|" + shuzoku;
            wikitext_box.Text += "|" + tokkou;
            wikitext_box.Text += "|" + soubi;
            wikitext_box.Text += "|" + rank;
            wikitext_box.Text += "|" + cost;
            wikitext_box.Text += "|" + passive1;
            if (passive_figs[0] != 0)
            {
                wikitext_box.Text += "[" + passive_figs[0] + "]";
            }
            wikitext_box.Text += "&br;" + passive2;
            if (passive_figs[1] != 0)
            {
                wikitext_box.Text += "[" + passive_figs[1] + "]";
            }
            wikitext_box.Text += "&br;" + passive3;
            if (passive_figs[2] != 0)
            {
                wikitext_box.Text += "[" + passive_figs[2] + "]";
            }
            wikitext_box.Text += "&br;" + passive4;
            if (passive_figs[3] != 0)
            {
                wikitext_box.Text += "[" + passive_figs[3] + "]";
            }
            wikitext_box.Text += "|" + passive5;
            if (passive_figs[4] != 0)
            {
                wikitext_box.Text += "[" + passive_figs[4] + "]";
            }
            wikitext_box.Text += "&br;" + passive6;
            if (passive_figs[5] != 0)
            {
                wikitext_box.Text += "[" + passive_figs[5] + "]";
            }
            wikitext_box.Text += "&br;" + passive7;
            if (passive_figs[6] != 0)
            {
                wikitext_box.Text += "[" + passive_figs[6] + "]";
            }
            wikitext_box.Text += "&br;" + passive8;
            if (passive_figs[7] != 0)
            {
                wikitext_box.Text += "[" + passive_figs[7] + "]";
            }
            wikitext_box.Text += "|" + leader1;
            wikitext_box.Text += "&br;" + leader2;
            wikitext_box.Text += "|" + assist;
            wikitext_box.Text += "|" + naisei;
            wikitext_box.Text += "|" + sutance;
            wikitext_box.Text += "|" + bikou;
            wikitext_box.Text += "|\n";
            if (save_flag)
            {
                hozon_text = wikitext_box.Text;
            }
        }
        public static class CaptureWrapper
        {
            [DllImport("CaptureWrapper.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int CaptureWindowByTitle(string windowTitle, string outputPath);
        }

        private void Name_box_TextChanged(object sender, TextChangedEventArgs e)
        {
            box_to_markdown(false);
        }
        // グレースケール変換
        private Bitmap ToGrayscale(Bitmap original)
        {
            Bitmap gray = new Bitmap(original.Width, original.Height);
            using (Graphics g = Graphics.FromImage(gray))
            {
                var cm = new ColorMatrix(new float[][]
                {
                    new float[]{0.299f, 0.299f, 0.299f, 0, 0},
                    new float[]{0.587f, 0.587f, 0.587f, 0, 0},
                    new float[]{0.114f, 0.114f, 0.114f, 0, 0},
                    new float[]{0, 0, 0, 1, 0},
                    new float[]{0, 0, 0, 0, 1}
                });
                using var ia = new ImageAttributes();
                ia.SetColorMatrix(cm);
                g.DrawImage(original, new System.Drawing.Rectangle(0, 0, gray.Width, gray.Height),
                            0, 0, original.Width, original.Height,
                            GraphicsUnit.Pixel, ia);
            }
            return gray;
        }

        // NearestNeighborでの拡大（線のぼやけ防止）
        private Bitmap ResizeBitmap(Bitmap bmp, int scale)
        {
            Bitmap result = new Bitmap(bmp.Width * scale, bmp.Height * scale);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                g.DrawImage(bmp, new System.Drawing.Rectangle(0, 0, result.Width, result.Height));
            }
            return result;
        }
        // 線が太くなりすぎない軽い二値化
        private Bitmap AdaptiveThreshold(Bitmap gray, int i_threshold)
        {
            Bitmap bin = new Bitmap(gray.Width, gray.Height, PixelFormat.Format24bppRgb);

            for (int y = 0; y < gray.Height; y++)
            {
                for (int x = 0; x < gray.Width; x++)
                {
                    var c = gray.GetPixel(x, y);
                    int v = c.R;
                    // 周辺明度を考慮した軽めの閾値補正
                    int threshold = i_threshold;
                    byte val = (byte)(v > threshold ? 255 : 0);
                    bin.SetPixel(x, y, System.Drawing.Color.FromArgb(val, val, val));
                }
            }
            return bin;
        }

        bool IsBinarizedImageMostlyBlack(Bitmap binarized, double blackRatioThreshold = 0.99)
        {
            int blackPixels = 0;
            int totalPixels = binarized.Width * binarized.Height;

            for (int y = 0; y < binarized.Height; y++)
            {
                for (int x = 0; x < binarized.Width; x++)
                {
                    Color c = binarized.GetPixel(x, y);
                    // 白か黒だけの画像を想定
                    if (c.R < 128) // 0〜127を黒とみなす
                        blackPixels++;
                }
            }
            double blackRatio = (double)blackPixels / totalPixels;
            return blackRatio > blackRatioThreshold;
        }
        private Pix BitmapToPix(Bitmap bmp)
        {
            using MemoryStream ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;
            return Pix.LoadFromMemory(ms.ToArray());
        }


        public string cropedAndselect(System.Drawing.Rectangle cropRect, int kakudai, int threathold)
        {
            string path = @".\Temp\capture_2wiki.png";
            using Bitmap bmp = new Bitmap(path);
            using Bitmap cropped = bmp.Clone(cropRect, bmp.PixelFormat);
            // グレースケール
            using Bitmap gray = ToGrayscale(cropped);
            // 漢字対応：2倍拡大（3倍だと潰れる）
            using Bitmap enlarged = ResizeBitmap(gray, kakudai);
            // 軽め二値化（線を太らせない）
            using Bitmap binarized = AdaptiveThreshold(enlarged, threathold);
            if (IsBinarizedImageMostlyBlack(enlarged))
                return "";
            // OCR処理
            using var pix = BitmapToPix(binarized);
            _engine.DefaultPageSegMode = PageSegMode.SingleLine;
            using var page = _engine.Process(pix);
            string text = page.GetText();
            string noSpace = text.Replace(" ", "");  // 半角スペースを削除
            Debug.WriteLine($"OCR結果: {noSpace}");
            string inputText = noSpace;
            string debugPath = @".\Temp\cropped_debug.png";
            binarized.Save(debugPath, System.Drawing.Imaging.ImageFormat.Png);
            return noSpace;
        }
        public string cropedAndselect_number(System.Drawing.Rectangle cropRect, int kakudai, int threathold)
        {
            string path = @".\Temp\capture_2wiki.png";
            string debugPath = @".\Temp\cropped_debug.png";
            using Bitmap bmp = new Bitmap(path);
            using Bitmap cropped = bmp.Clone(cropRect, bmp.PixelFormat);
            // グレースケール
            using Bitmap gray = ToGrayscale(cropped);
            // 漢字対応：2倍拡大（3倍だと潰れる）
            using Bitmap enlarged = ResizeBitmap(gray, kakudai);
            // 軽め二値化（線を太らせない）
            using Bitmap binarized = AdaptiveThreshold(enlarged, threathold);
            /*
            if (IsBinarizedImageMostlyBlack(enlarged))
            {
                binarized.Save(debugPath, System.Drawing.Imaging.ImageFormat.Png);
                return "";
            }*/

            // OCR処理
            using var pix = BitmapToPix(binarized);
            _engine_number.DefaultPageSegMode = PageSegMode.SingleLine;
            using var page = _engine_number.Process(pix);
            string text = page.GetText();
            string noSpace = text.Replace(" ", "");  // 半角スペースを削除
            Debug.WriteLine($"OCR結果: {noSpace}");
            string inputText = noSpace;
            binarized.Save(debugPath, System.Drawing.Imaging.ImageFormat.Png);
            return noSpace;
        }

        public string cropedAndselect_shuzoku(System.Drawing.Rectangle cropRect, int kakudai, int threathold)
        {
            string path = @".\Temp\capture_2wiki.png";
            string debugPath = @".\Temp\cropped_debug.png";
            using Bitmap bmp = new Bitmap(path);
            using Bitmap cropped = bmp.Clone(cropRect, bmp.PixelFormat);
            // グレースケール
            using Bitmap gray = ToGrayscale(cropped);
            // 漢字対応：2倍拡大（3倍だと潰れる）
            using Bitmap enlarged = ResizeBitmap(gray, kakudai);
            // 軽め二値化（線を太らせない）
            using Bitmap binarized = AdaptiveThreshold(enlarged, threathold);
            /*
            if (IsBinarizedImageMostlyBlack(enlarged))
            {
                binarized.Save(debugPath, System.Drawing.Imaging.ImageFormat.Png);
                return "";
            }*/

            // OCR処理
            using var pix = BitmapToPix(binarized);
            _engine_shuzoku.DefaultPageSegMode = PageSegMode.SingleLine;
            using var page = _engine_shuzoku.Process(pix);
            string text = page.GetText();
            string noSpace = text.Replace(" ", "");  // 半角スペースを削除
            Debug.WriteLine($"OCR結果: {noSpace}");
            string inputText = noSpace;
            binarized.Save(debugPath, System.Drawing.Imaging.ImageFormat.Png);
            return noSpace;
        }

        private double JaroWinklerSimilarity(string s1, string s2)
        {
            if (string.IsNullOrEmpty(s1) || string.IsNullOrEmpty(s2))
                return 0.0;

            int len1 = s1.Length;
            int len2 = s2.Length;

            int matchDistance = Math.Max(len1, len2) / 2 - 1;

            bool[] s1Matches = new bool[len1];
            bool[] s2Matches = new bool[len2];

            int matches = 0;
            for (int i = 0; i < len1; i++)
            {
                int start = Math.Max(0, i - matchDistance);
                int end = Math.Min(i + matchDistance + 1, len2);

                for (int j = start; j < end; j++)
                {
                    if (s2Matches[j]) continue;
                    if (s1[i] != s2[j]) continue;
                    s1Matches[i] = s2Matches[j] = true;
                    matches++;
                    break;
                }
            }

            if (matches == 0) return 0.0;

            double t = 0;
            int k = 0;
            for (int i = 0; i < len1; i++)
            {
                if (!s1Matches[i]) continue;
                while (!s2Matches[k]) k++;
                if (s1[i] != s2[k]) t++;
                k++;
            }

            t /= 2.0;
            double jaro = ((matches / (double)len1) + (matches / (double)len2) + ((matches - t) / matches)) / 3.0;

            // Winkler補正（先頭の一致を優遇）
            int prefix = 0;
            for (int i = 0; i < Math.Min(4, Math.Min(s1.Length, s2.Length)); i++)
            {
                if (s1[i] == s2[i]) prefix++;
                else break;
            }
            return jaro + 0.1 * prefix * (1 - jaro);
        }
        private string Normalize(string s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;

            // 全角→半角、カタカナ→ひらがな、英字→小文字
            s = s.Trim().ToLowerInvariant();

            s = s.Replace('　', ' '); // 全角スペース
            s = new string(s
                .Normalize(NormalizationForm.FormKC) // Unicode正規化
                .Select(c => c == 'ヴ' ? 'う' : c)   // 簡易変換例
                .ToArray());

            return s;
        }


        public void SelectMostSimilarEquipment(string input, int shurui)
        {
            System.Collections.IEnumerable target;
            if (shurui == 0)
            {
                target = EquipmentBox1.ItemsSource;
            }
            else if (shurui == 1)
            {
                target = EquipmentBox2.ItemsSource;
            }
            else
            {
                target = ryoshokuBox.ItemsSource;

            }
            if (string.IsNullOrWhiteSpace(input) || target is not IEnumerable<ItemSet> items)
                return;

            input = Normalize(input);

            var bestMatch = items
                .Where(e => !string.IsNullOrWhiteSpace(e?.ItemDisp))
                .Select(e => new
                {
                    Item = e,
                    Score = JaroWinklerSimilarity(Normalize(e.ItemDisp), input)
                })
                .OrderByDescending(x => x.Score) // スコアが高いほど似ている
                .FirstOrDefault()?.Item;

            if (bestMatch != null)
            {
                if (shurui == 0)
                    EquipmentBox1.SelectedItem = bestMatch;
                else if (shurui == 1)
                    EquipmentBox2.SelectedItem = bestMatch;
                else if (shurui == 2)
                    ryoshokuBox.SelectedItem = bestMatch;
            }
        }

        public void SelectMostSimilarSkill(string input, int skillnum)
        {
            if (string.IsNullOrWhiteSpace(input) )
                return;

            input = Normalize(input);

            var bestMatch = all_skils
                .Where(e => !string.IsNullOrWhiteSpace(e))
                .Select(e => new
                {
                    Item = e,
                    Score = JaroWinklerSimilarity(Normalize(e), input)
                })
                .OrderByDescending(x => x.Score)
                .FirstOrDefault()?.Item;
            if (bestMatch != null)
            {
                ((TextBox)FindName($"passive{skillnum}_box"))!.Text = bestMatch;
            }
            else
            {
                ((TextBox)FindName($"passive{skillnum}_box"))!.Text = "";
            }
        }


        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            box_to_markdown(true);
        }

        private void ComboBox_SelectionChanged_EquipmentBox1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged_EquipmentBox2(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged_ryoshokuBox(object sender, SelectionChangedEventArgs e)
        {

        }

        public string LoadFromGame_MobileNetV2_NCHW(int sw, int sh, int ew, int eh, int getFlag, float colorWeight = 1.0f)
        {
            // 1. 元画像ロード & クロップ
            using var tempBmp = new Bitmap(@".\Temp\capture_2wiki.png");
            using var cropped = tempBmp.Clone(new System.Drawing.Rectangle(sw, sh, ew, eh), tempBmp.PixelFormat);

            // デバッグ用
            cropped.Save(@".\Temp\capture_2_icon.png", System.Drawing.Imaging.ImageFormat.Png);

            // 2. getFlagに応じたデータセット切り替え
            Bitmap reusableBitmap;
            float[] reusableTensorBuffer;
            string inputName;
            Dictionary<string, string> idNameMap;
            List<ImageFeature> features;

            switch (getFlag)
            {
                case 0:
                    reusableBitmap = reusableBitmap_job;
                    reusableTensorBuffer = reusableTensorBuffer_job;
                    inputName = inputName_job;
                    features = features_job;
                    idNameMap = idNameMap_job;
                    break;
                case 1:
                    reusableBitmap = reusableBitmap_equipment;
                    reusableTensorBuffer = reusableTensorBuffer_equipment;
                    inputName = inputName_equipment;
                    features = features_equipment;
                    idNameMap = idNameMap_equipment;
                    break;
                case 2:
                    reusableBitmap = reusableBitmap_leader;
                    reusableTensorBuffer = reusableTensorBuffer_leader;
                    inputName = inputName_leader;
                    features = features_leader;
                    idNameMap = idNameMap_leader;
                    break;
                case 3:
                default:
                    reusableBitmap = reusableBitmap_element;
                    reusableTensorBuffer = reusableTensorBuffer_element;
                    inputName = inputName_element;
                    features = features_element;
                    idNameMap = idNameMap_element;
                    break;
            }

            // 3. 224x224にリサイズ
            using var g = Graphics.FromImage(reusableBitmap);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.DrawImage(cropped, 0, 0, 224, 224);

            // 4. 軽いシャープを適用
            using var sharpened = ApplySharpenLight(reusableBitmap);

            // 5. Tensor変換 (NCHW, MobileNetV2 [-1,1])
            int width = 224;
            int height = 224;
            int hw = width * height;
            int idxR = 0;
            int idxG = hw;
            int idxB = hw * 2;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color c = sharpened.GetPixel(x, y);
                    reusableTensorBuffer[idxR++] = c.R / 127.5f - 1f;
                    reusableTensorBuffer[idxG++] = c.G / 127.5f - 1f;
                    reusableTensorBuffer[idxB++] = c.B / 127.5f - 1f;
                }
            }

            // 6. CNN特徴抽出 (MobileNetV2)
            using var results = session.Run(new[] {
        NamedOnnxValue.CreateFromTensor(inputName,
            new DenseTensor<float>(reusableTensorBuffer, new[] {1, 3, 224, 224}))
    });
            float[] cnnFeature = results.First().AsEnumerable<float>().ToArray();
            cnnFeature = Normalize_chara(cnnFeature);

            // 7. 色特徴抽出
            float[] colorFeature = GetColorFeatureExtended(reusableBitmap); // RGB平均 + RGBヒスト + HSVヒスト
            colorFeature = Normalize_chara(colorFeature);

            // 8. CNN + 色特徴結合
            float[] queryFeature = colorWeight <= 0f
                ? cnnFeature
                : Normalize_chara(cnnFeature.Concat(colorFeature.Select(v => v * colorWeight)).ToArray());

            // 9. 類似度計算
            float bestScore = float.MinValue;
            string bestName = "";
            foreach (var f in features)
            {
                if (f.Feature.Length != queryFeature.Length) continue;
                float score = Cosine(f.Feature, queryFeature);
                if (score > bestScore)
                {
                    bestScore = score;
                    bestName = idNameMap[f.Id];
                }
                Debug.WriteLine($"{idNameMap[f.Id]} : {score}");
            }
            Debug.WriteLine($"CNN feature: min={cnnFeature.Min()}, max={cnnFeature.Max()}");
            Debug.WriteLine($"Color feature: min={colorFeature.Min()}, max={colorFeature.Max()}");
            Debug.WriteLine($"Query feature length: {queryFeature.Length}");
            if (bestScore < 0.3f)
                bestName = "";

            return bestName;
        }


        public string load_from_game(int sw, int sh, int ew, int eh, int get_flag, float colorWeight = 1.0f)
        {
            // 1. 元画像ロード & クロップ
            using var tempBmp = new Bitmap(@".\Temp\capture_2wiki.png");
            string debugPath = @".\Temp\capture_2_icon.png";
            using var cropped = tempBmp.Clone(new System.Drawing.Rectangle(sw, sh, ew, eh), tempBmp.PixelFormat);
            cropped.Save(debugPath, System.Drawing.Imaging.ImageFormat.Png);

            Bitmap reusableBitmap = reusableBitmap_job;
            float[] reusableTensorBuffer= reusableTensorBuffer_job;
            string inputName="";
            Dictionary<string, string> idNameMap= idNameMap_element;

            List<ImageFeature> features = features_leader;
            ;
            if (get_flag == 0)
            {
                reusableBitmap = reusableBitmap_job;
                reusableTensorBuffer = reusableTensorBuffer_job;
                inputName = inputName_job;
                features = features_job;
                idNameMap = idNameMap_job;
            }
            else if (get_flag == 1)
            {
                reusableBitmap = reusableBitmap_equipment;
                reusableTensorBuffer = reusableTensorBuffer_equipment;
                inputName = inputName_equipment;
                features = features_equipment;
                idNameMap = idNameMap_equipment;
            }
            else if (get_flag == 2)
            {
                reusableBitmap = reusableBitmap_leader;
                reusableTensorBuffer = reusableTensorBuffer_leader;
                inputName = inputName_leader;
                features = features_leader;
                idNameMap = idNameMap_leader;
            }
            else if (get_flag == 3)
            {
                reusableBitmap = reusableBitmap_element;
                reusableTensorBuffer = reusableTensorBuffer_element;
                inputName = inputName_element;
                features = features_element;
                idNameMap = idNameMap_element;
            }
            using var g = Graphics.FromImage(reusableBitmap);
            //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic; 
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.DrawImage(cropped, 0, 0, 224, 224);
            /*
            using var temp64 = new Bitmap(64, 64);
            using (var g1 = Graphics.FromImage(temp64))
            {
                g1.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g1.DrawImage(cropped, 0, 0, 64, 64);
            }
            using var g = Graphics.FromImage(reusableBitmap);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(temp64, 0, 0, 224, 224);

            using var sharpened = Sharpen(reusableBitmap);
            BitmapToTensor_KerasCaffe_inplace(sharpened, reusableTensorBuffer);
            */
            // 2. Tensor 変換
            using var sharpened = ApplySharpenLight(reusableBitmap);

            BitmapToTensor_KerasCaffe_inplace(sharpened, reusableTensorBuffer);

            // 3. CNN特徴抽出
            /*
            using var results = session.Run(new[] {
            NamedOnnxValue.CreateFromTensor(inputName, new DenseTensor<float>(reusableTensorBuffer, new[] { 1, 224, 224, 3 }))
            });
            float[] cnnFeature = results.First().AsEnumerable<float>().ToArray();
            cnnFeature = Normalize_chara(cnnFeature);
            */
            using var results = session.Run(new[] {
                NamedOnnxValue.CreateFromTensor(inputName,
                    new DenseTensor<float>(reusableTensorBuffer, new[] {1, 224, 224, 3}))
            });
            float[] cnnFeature = results.First().AsEnumerable<float>().ToArray();
            cnnFeature = Normalize_chara(cnnFeature);
            // 4. 色特徴抽出
            float[] colorFeature = GetColorFeatureExtended(reusableBitmap); // RGB平均 + RGBヒスト + HSVヒスト
            colorFeature = Normalize_chara(colorFeature);

            // 5. CNN主軸＋色補助を結合
            float[] queryFeature;
            if (colorWeight <= 0f)
            {
                queryFeature = cnnFeature; // 色補助なし
            }
            else
            {
                queryFeature = cnnFeature.Concat(colorFeature.Select(v => v * colorWeight)).ToArray();
                queryFeature = Normalize_chara(queryFeature); // 結合後正規化
            }
            // 6. 類似度計算
            float bestScore = float.MinValue;
            string bestName = null;
            int bestId = -1;

            foreach (var f in features)
            {
                if (f.Feature.Length != queryFeature.Length) continue; // 念のため
                float score = Cosine(f.Feature, queryFeature);
                if (score > bestScore)
                {
                    bestScore = score;
                    bestName = idNameMap[f.Id];
                }
                Debug.WriteLine(idNameMap[f.Id] + ":" + score);
            }
            if (bestScore < 0.3)
                bestName = "";
            return bestName;
            // 7. 選択反映
            //var match = characters.FirstOrDefault(c => c.名称 == bestName);
            //if (match != null)
            //    CharacterBox.SelectedItem = match;
        }
        void BitmapToTensor_MobileNetV2(Bitmap bmp, float[] buffer)
        {
            int w = bmp.Width, h = bmp.Height, i = 0;
            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                {
                    Color c = bmp.GetPixel(x, y);
                    buffer[i++] = (c.R / 127.5f) - 1f;
                    buffer[i++] = (c.G / 127.5f) - 1f;
                    buffer[i++] = (c.B / 127.5f) - 1f;
                }
        }

        Bitmap ApplySharpenLight(Bitmap src)
        {
            var dst = new Bitmap(src.Width, src.Height);
            using (var g = Graphics.FromImage(dst))
            {
                var matrix = new System.Drawing.Imaging.ColorMatrix(new float[][]
                {
            new float[] {1.1f, 0, 0, 0, 0},
            new float[] {0, 1.1f, 0, 0, 0},
            new float[] {0, 0, 1.1f, 0, 0},
            new float[] {0, 0, 0, 1, 0},
            new float[] {-0.05f, -0.05f, -0.05f, 0, 1}
                });
                var attr = new System.Drawing.Imaging.ImageAttributes();
                attr.SetColorMatrix(matrix);
                g.DrawImage(src, new System.Drawing.Rectangle(0, 0, src.Width, src.Height),
                    0, 0, src.Width, src.Height, GraphicsUnit.Pixel, attr);
            }
            return dst;
        }
        static float Cosine(float[] a, float[] b)
        {
            float dot = 0;
            float normA = 0;
            float normB = 0;

            for (int i = 0; i < a.Length; i++)
            {
                dot += a[i] * b[i];
                normA += a[i] * a[i];
                normB += b[i] * b[i];
            }

            return dot / ((float)Math.Sqrt(normA) * (float)Math.Sqrt(normB));
        }
        private float[] GetColorFeatureExtended(Bitmap bmp)
        {
            int width = bmp.Width, height = bmp.Height;
            var arr = new float[width, height, 3];

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    var c = bmp.GetPixel(x, y);
                    arr[x, y, 0] = c.R / 255f;
                    arr[x, y, 1] = c.G / 255f;
                    arr[x, y, 2] = c.B / 255f;
                }

            // RGB平均
            float rMean = 0, gMean = 0, bMean = 0;
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    rMean += arr[x, y, 0];
                    gMean += arr[x, y, 1];
                    bMean += arr[x, y, 2];
                }
            float total = width * height;
            rMean /= total; gMean /= total; bMean /= total;

            // RGBヒストグラム 8ビン
            int binsRGB = 8;
            float[] rHist = new float[binsRGB];
            float[] gHist = new float[binsRGB];
            float[] bHist = new float[binsRGB];
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    rHist[Math.Clamp((int)(arr[x, y, 0] * binsRGB), 0, binsRGB - 1)]++;
                    gHist[Math.Clamp((int)(arr[x, y, 1] * binsRGB), 0, binsRGB - 1)]++;
                    bHist[Math.Clamp((int)(arr[x, y, 2] * binsRGB), 0, binsRGB - 1)]++;
                }
            NormalizeHist(rHist);
            NormalizeHist(gHist);
            NormalizeHist(bHist);

            // HSVヒストグラム 16ビン
            int binsHSV = 16;
            float[] hHist = new float[binsHSV];
            float[] sHist = new float[binsHSV];
            float[] vHist = new float[binsHSV];
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    ColorToHSV(bmp.GetPixel(x, y), out float h, out float s, out float v);
                    hHist[Math.Clamp((int)(h / 360f * binsHSV), 0, binsHSV - 1)]++;
                    sHist[Math.Clamp((int)(s * binsHSV), 0, binsHSV - 1)]++;
                    vHist[Math.Clamp((int)(v * binsHSV), 0, binsHSV - 1)]++;
                }
            NormalizeHist(hHist);
            NormalizeHist(sHist);
            NormalizeHist(vHist);

            // 結合 (RGB平均3 + RGBヒスト24 + HSVヒスト48 = 75次元)
            return (new float[] { rMean, gMean, bMean })
                .Concat(rHist).Concat(gHist).Concat(bHist)
                .Concat(hHist).Concat(sHist).Concat(vHist)
                .ToArray();
        }
        /// <summary>HSV変換</summary>
        private static void ColorToHSV(System.Drawing.Color color, out float h, out float s, out float v)
        {
            float r = color.R / 255f;
            float g = color.G / 255f;
            float b = color.B / 255f;

            float max = Math.Max(r, Math.Max(g, b));
            float min = Math.Min(r, Math.Min(g, b));
            v = max;

            float delta = max - min;
            if (max != 0)
                s = delta / max;
            else
            {
                s = 0;
                h = 0;
                return;
            }

            if (r == max)
                h = (g - b) / delta;
            else if (g == max)
                h = 2 + (b - r) / delta;
            else
                h = 4 + (r - g) / delta;

            h *= 60;
            if (h < 0)
                h += 360;
        }

        private static void NormalizeHist(float[] hist)
        {
            float sum = hist.Sum();
            if (sum > 0)
            {
                for (int i = 0; i < hist.Length; i++)
                    hist[i] /= sum;
            }
        }
        private void BitmapToTensor_KerasCaffe_inplace(Bitmap bmp, float[] buffer)
        {
            // bmp は 224x224 RGB
            for (int y = 0; y < 224; y++)
            {
                for (int x = 0; x < 224; x++)
                {
                    var color = bmp.GetPixel(x, y);
                    int idx = (y * 224 + x) * 3;
                    // Keras Caffeモード: BGR - mean subtraction
                    buffer[idx + 0] = color.B - 103.939f;
                    buffer[idx + 1] = color.G - 116.779f;
                    buffer[idx + 2] = color.R - 123.68f;
                }
            }
        }
        static float[] Normalize_chara(float[] vec)
        {
            double norm = Math.Sqrt(vec.Select(v => v * v).Sum());
            return vec.Select(v => (float)(v / norm)).ToArray();
        }


        private void Captuer_Button_Click(object sender, RoutedEventArgs e)
        {
            string path = @".\Temp\capture_2wiki.png";
            int hr = CaptureWrapper.CaptureWindowByTitle("VenusBloodVALKYRIE", path);
            if (hr != 0)
            {
                MessageBox.Show($"キャプチャ失敗: {hr}");
                return;
            }

            using Bitmap bmp = new Bitmap(path);
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
            var cropRect_hp = new System.Drawing.Rectangle(
                (int)(620 * scaleX),
                (int)(410 * scaleY),
                (int)(100 * scaleX),
                (int)(28 * scaleY));
            var cropRect_kougeki = new System.Drawing.Rectangle(
                (int)(647 * scaleX),
                (int)(475 * scaleY),
                (int)(70 * scaleX),
                (int)(27 * scaleY));
            var cropRect_bougyo = new System.Drawing.Rectangle(
                (int)(647 * scaleX),
                (int)(500 * scaleY),
                (int)(70 * scaleX),
                (int)(27 * scaleY));
            var cropRect_sokudo = new System.Drawing.Rectangle(
                (int)(647 * scaleX),
                (int)(525 * scaleY),
                (int)(70 * scaleX),
                (int)(27 * scaleY));
            var cropRect_chiryoku = new System.Drawing.Rectangle(
                (int)(647 * scaleX),
                (int)(550 * scaleY),
                (int)(70 * scaleX),
                (int)(27 * scaleY));
            var cropRect_kago1 = new System.Drawing.Rectangle(
                (int)(589 * scaleX),
                (int)(326 * scaleY),
                (int)(27 * scaleX),
                (int)(27 * scaleY));
            var cropRect_kago2 = new System.Drawing.Rectangle(
                (int)(616 * scaleX),
                (int)(326 * scaleY),
                (int)(27 * scaleX),
                (int)(27 * scaleY));
            var cropRect_shokugyo = new System.Drawing.Rectangle(
                (int)(865 * scaleX),
                (int)(366 * scaleY),
                (int)(27 * scaleX),
                (int)(27 * scaleY));
            var cropRect_stance = new System.Drawing.Rectangle(
                (int)(620 * scaleX),
                (int)(385 * scaleY),
                (int)(30 * scaleX),
                (int)(30 * scaleY));
            var cropRect_equip1_shurui = new System.Drawing.Rectangle(
                (int)(595 * scaleX),
                (int)(645 * scaleY),
                (int)(22 * scaleX),
                (int)(22 * scaleY));
            var cropRect_equip2_shurui = new System.Drawing.Rectangle(
                (int)(595 * scaleX),
                (int)(670 * scaleY),
                (int)(22 * scaleX),
                (int)(22 * scaleY));
            var cropRect_cost = new System.Drawing.Rectangle(
                (int)(955 * scaleX),
                (int)(694 * scaleY),
                (int)(50 * scaleX),
                (int)(22 * scaleY));
            var cropRect_rank = new System.Drawing.Rectangle(
                (int)(955 * scaleX),
                (int)(613 * scaleY),
                (int)(30 * scaleX),
                (int)(24 * scaleY));
            var cropRect_shuzoku = new System.Drawing.Rectangle(
                (int)(647 * scaleX),
                (int)(582 * scaleY),
                (int)(200 * scaleX),
                (int)(24 * scaleY));
            var cropRect_tokkou = new System.Drawing.Rectangle(
                (int)(647 * scaleX),
                (int)(607 * scaleY),
                (int)(200 * scaleX),
                (int)(27 * scaleY));
            int[] yPositions = { 326, 354, 382, 410, 438, 463, 491, 520 };
            var cropRects = yPositions
                .Select(y => new System.Drawing.Rectangle(
                    (int)(1140 * scaleX),
                    (int)(y * scaleY),
                    (int)(100 * scaleX),
                    (int)(27 * scaleY)))
                .ToArray();
            var cropRects_figure = yPositions
                .Select(y => new System.Drawing.Rectangle(
                    (int)(1238 * scaleX),
                    (int)(y * scaleY),
                    (int)(50 * scaleX),
                    (int)(27 * scaleY)))
                .ToArray();


            int sw = (int)(865 * scaleX);
            int sh = (int)(370 * scaleY);
            int ew = (int)(210 * scaleX);
            int eh = (int)(210 * scaleY);

            string noSpace = "";

            // --- 処理順に呼ぶ ---
            noSpace = cropedAndselect(cropRect_equip1, 3, 220);
            SelectMostSimilarEquipment(noSpace, 0);
            noSpace = cropedAndselect(cropRect_equip2, 3, 220);
            SelectMostSimilarEquipment(noSpace, 1);
            noSpace = cropedAndselect(cropRect_ryoshoku, 3, 220);
            SelectMostSimilarEquipment(noSpace, 2);
            noSpace = cropedAndselect_number(cropRect_level, 2, 200);
            int.TryParse(noSpace, out status_level);
            noSpace = cropedAndselect_number(cropRect_hp, 2, 200);
            int.TryParse(noSpace, out status_hp);
            noSpace = cropedAndselect_number(cropRect_kougeki, 2, 75);
            int.TryParse(noSpace, out status_kougeki);
            noSpace = cropedAndselect_number(cropRect_bougyo, 2, 140);
            int.TryParse(noSpace, out status_bougyo);
            noSpace = cropedAndselect_number(cropRect_sokudo, 2, 150);
            int.TryParse(noSpace, out status_sokudo);
            noSpace = cropedAndselect_number(cropRect_chiryoku, 2, 120);
            int.TryParse(noSpace, out status_chiryoku);
            noSpace = cropedAndselect_number(cropRect_cost, 2, 200);
            int.TryParse(noSpace, out status_cost);
            noSpace = cropedAndselect_shuzoku(cropRect_shuzoku, 3,210);
            shuzoku_box.Text = noSpace;
            noSpace = cropedAndselect_shuzoku(cropRect_tokkou, 3, 210);
            tokkou_box.Text = noSpace;
            _engine.SetVariable("tessedit_char_whitelist", "S,A,B,C,D,E"); // 
            int skill_th = 210;
            noSpace = cropedAndselect(cropRect_rank, 3, skill_th);
            rank_box.Text = noSpace;
            _engine.SetVariable("tessedit_char_whitelist", ""); // 
            for (int i = 0; i < cropRects.Length; i++)
            {
                var noSpace_in = cropedAndselect(cropRects[i], 2, skill_th);
                SelectMostSimilarSkill(noSpace_in, i + 1);
            }
            var passiveFigures = new[]{
                passive1_figure,
                passive2_figure,
                passive3_figure,
                passive4_figure,
                passive5_figure,
                passive6_figure,
                passive7_figure,
                passive8_figure
            };
            for (int i = 0; i < cropRects_figure.Length; i++)
            {
                var noSpace_in = cropedAndselect_number(cropRects_figure[i], 2, 200);
                passiveFigures[i].Text = noSpace_in;
            }
            string temp = LoadFromGame_MobileNetV2_NCHW(590, 330, 25, 25, 3,1);
            kago_box.Text = temp;
            temp = LoadFromGame_MobileNetV2_NCHW(615, 330, 25, 25, 3,1);
            kago_box.Text += temp;

            temp = LoadFromGame_MobileNetV2_NCHW(590, 355, 25, 25, 0, 1);
            shokugyou_box.Text = temp;

            temp = LoadFromGame_MobileNetV2_NCHW(865, 370, 30, 30, 2, 1);
            stance_box.Text = temp;

            temp = LoadFromGame_MobileNetV2_NCHW(594, 645, 25, 25, 1, 1);
            soubi_box.Text = temp;

            temp = LoadFromGame_MobileNetV2_NCHW(594, 670, 25, 25, 1, 1);
            soubi_box.Text += "&br;" + temp;

            hp_box.Text = status_hp.ToString();
            kougeki_box.Text = status_kougeki.ToString();
            bougyo_box.Text = status_bougyo.ToString();
            sokudo_box.Text = status_sokudo.ToString();
            chiryoku_box.Text = status_chiryoku.ToString();
            cost_box.Text = status_cost.ToString();
            box_to_markdown(false);
        }

        private void passive6_box_TextChanged(object sender, object e)
        {

        }

        private void sutance_box_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}