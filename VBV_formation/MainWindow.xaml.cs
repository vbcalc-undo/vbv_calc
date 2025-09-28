using JsonFileIO.Jsons;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VBV_calc.Helpers;
using VBV_calc.Models;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static VBV_calc.MainWindow;
using static VBV_formation.MainWindow;

namespace VBV_formation
{
    public partial class MainWindow : Window
    {
        public class param_status
        {
            public int hp;
            public int kougeki;
            public int bougyo;
            public int sokudo;
            public int chiryoku;
        }

        private void load_json_shogo_func(string filename, string medalname)
        {         // ここにJSON読み込みのコードを追加
            List<ShogoJson> shogo = null; // ここで宣言
            using (var sr = new StreamReader(filename, System.Text.Encoding.UTF8))
            {
                var jsonReadData = sr.ReadToEnd();

                shogo = JsonConvert.DeserializeObject<List<ShogoJson>>(jsonReadData);
                all_shogo.Add(medalname, shogo);
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
            }
        }
        private void load_json_equipment()
        {         // ここにJSON読み込みのコードを追加
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

        public class character_info
        {
            public string character_id;
            public string character_name;
            public Dictionary<string, int> character_skill = new Dictionary<string, int>();
            public string character_shogo1;
            public string character_shogo2;
            public string character_equipment1;
            public string character_equipment2;
            public string character_ryoshoku;
            public string character_shuzoku;
            public string character_kago;

            public Dictionary<string, int> leader_skill = new Dictionary<string, int>();
            public int buko;
            public int cost;
            public string rank;
            public param_status soubi_status;
            public param_status base_status;
            public param_status shogo_status;
            public bool leader_flag;
            public string stance;
            public Dictionary<string, int> character_status = new Dictionary<string, int>();
            public Dictionary<string, int> character_current_status = new Dictionary<string, int>();
            public string assist_skill;
            public bool assist_skill_flag;

            public character_info DeepCopy()
            {
                return new character_info
                {
                    character_id = this.character_id,
                    character_name = this.character_name,
                    character_skill = this.character_skill.ToDictionary(
                       kvp => kvp.Key,
                        kvp => kvp.Value
                    ),
                    character_shogo1 = this.character_shogo1,
                    character_shogo2 = this.character_shogo2,
                    character_equipment1 = this.character_equipment1,
                    character_equipment2 = this.character_equipment2,
                    character_ryoshoku = this.character_ryoshoku,
                    character_shuzoku = this.character_shuzoku,
                    character_kago = this.character_kago,
                    leader_skill = this.leader_skill.ToDictionary(
                       kvp => kvp.Key,
                        kvp => kvp.Value
                    ),
                    buko = this.buko,
                    cost = this.cost,
                    rank = this.rank,
                    soubi_status = this.soubi_status,
                    base_status = this.base_status,
                    shogo_status = this.shogo_status,
                    leader_flag = this.leader_flag,
                    stance = this.stance,
                    character_status = this.character_status.ToDictionary(
                       kvp => kvp.Key,
                        kvp => kvp.Value
                    ),
                    character_current_status = this.character_current_status.ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value
                    ),
                    assist_skill = this.assist_skill,
                    assist_skill_flag = this.assist_skill_flag,
                    // 必要に応じて他のプロパティもコピー
                };
            }
        }
        private void load_json_shogo()
        {         // ここにJSON読み込みのコードを追加
            load_json_shogo_func(@"./json/medallion\mu.json", "無");
            load_json_shogo_func(@"./json/medallion\tikara.json", "力");
            load_json_shogo_func(@"./json/medallion\ti.json", "知");
            load_json_shogo_func(@"./json/medallion\waza.json", "技");
            load_json_shogo_func(@"./json/medallion\kemono.json", "獣");
            load_json_shogo_func(@"./json/medallion\ja.json", "邪");
            load_json_shogo_func(@"./json/medallion\i.json", "異");
            load_json_shogo_func(@"./json/medallion\riku.json", "陸");
            load_json_shogo_func(@"./json/medallion\umi.json", "海");
            load_json_shogo_func(@"./json/medallion\sora.json", "空");
            load_json_shogo_func(@"./json/medallion\tou.json", "闘");
            load_json_shogo_func(@"./json/medallion\shin.json", "信");
            load_json_shogo_func(@"./json/medallion\ku.json", "苦");
            load_json_shogo_func(@"./json/medallion\maboroshi.json", "幻");
            load_json_shogo_func(@"./json/medallion\kyojin.json", "巨人");
            load_json_shogo_func(@"./json/medallion\akuma.json", "悪魔");
            load_json_shogo_func(@"./json/medallion\zouma.json", "造魔");
            load_json_shogo_func(@"./json/medallion\ryu.json", "竜");
            load_json_shogo_func(@"./json/medallion\rekisen.json", "歴戦");
            load_json_shogo_func(@"./json/medallion\kyo.json", "凶");
            load_json_shogo_func(@"./json/medallion\mushi.json", "蟲");
            load_json_shogo_func(@"./json/medallion\eiyu.json", "英雄");
            load_json_shogo_func(@"./json/medallion\kenja.json", "賢者");
            load_json_shogo_func(@"./json/medallion\inochi.json", "命");
            load_json_shogo_func(@"./json/medallion\ou.json", "王");
            load_json_shogo_func(@"./json/medallion\mukuro.json", "骸");
            load_json_shogo_func(@"./json/medallion\shito.json", "使徒");
            load_json_shogo_func(@"./json/medallion\kakushin.json", "革新");
            load_json_shogo_func(@"./json/medallion\oni.json", "鬼");
            load_json_shogo_func(@"./json/medallion\teiou.json", "帝王");
            load_json_shogo_func(@"./json/medallion\kami.json", "神");
            load_json_shogo_func(@"./json/medallion\bi.json", "尾");
            load_json_shogo_func(@"./json/medallion\kikou.json", "機巧");
            load_json_shogo_func(@"./json/medallion\shinri.json", "真理");
            load_json_shogo_func(@"./json/medallion\hametsu.json", "破滅");
            load_json_shogo_func(@"./json/medallion\unmei.json", "運命");
            load_json_shogo_func(@"./json/medallion\setuna.json", "刹那");
            load_json_shogo_func(@"./json/medallion\sousei.json", "創成");
            load_json_shogo_func(@"./json/medallion\meifu.json", "冥府");
            load_json_shogo_func(@"./json/medallion\kinki.json", "禁忌");
            load_json_shogo_func(@"./json/medallion\rakuen.json", "楽園");
            load_json_shogo_func(@"./json/medallion\character_shogo.json", "キャラクター");

        }
        private void load_json_character()
        {
            List<string> temp_asistskills = new List<string>();
            using (var sr = new StreamReader(@"./json/units/units_ippan.json", System.Text.Encoding.UTF8))
            {
                var jsonReadData = sr.ReadToEnd();

                characters = JsonConvert.DeserializeObject<List<CharacterJson>>(jsonReadData);
                for (int i = 0; i < characters.Count; i++)
                {
                    string temp_name = ReplaceBracketNumberWithColon(characters[i].アシストスキル[0]);
                    if (characters[i].アシストスキル.Count != 0)
                        if (!all_asistskills.Any(item => item.Contains(temp_name)))
                            all_asistskills.Add(temp_name);
                }
            }
            using (var sr = new StreamReader(@"./json/units/units_busho.json", System.Text.Encoding.UTF8))
            {
                var jsonReadData = sr.ReadToEnd();

                var bushoCharacters = JsonConvert.DeserializeObject<List<CharacterJson>>(jsonReadData);
                for (int i = 0; i < bushoCharacters.Count; i++)
                {
                    bushoCharacters[i].キャラクター = true;
                    string temp_name = ReplaceBracketNumberWithColon(bushoCharacters[i].アシストスキル[0]);
                    if (bushoCharacters[i].アシストスキル.Count != 0)
                        if (!all_asistskills.Any(item => item.Contains(temp_name)))
                            all_asistskills.Add(temp_name);
                }
                if (bushoCharacters != null)
                {
                    characters.AddRange(bushoCharacters);
                }
            }
            //var sorted = all_asistskills.OrderBy(x => ).ToList();
            all_asistskills.Clear();
            /*
            foreach (var item in sorted)
            {
                src_asistskills.Add(item);
            }*/
        }
        public class savedata
        {
            public string character_name { get; set; }
            public string character_id { get; set; }
            public string shogo1 { get; set; }
            public string shogo2 { get; set; }
            public string equipment1 { get; set; }
            public string equipment2 { get; set; }
            public string ryoshoku { get; set; }
            public string shogo1_name { get; set; }
            public string shogo2_name { get; set; }
            public string equipment1_name { get; set; }
            public string equipment2_name { get; set; }
            public string ryoshoku_name { get; set; }

            public string assist1 { get; set; }
            public string assist2 { get; set; }
            public string assist3 { get; set; }
            public string assist1_chiryoku { get; set; }
            public string assist2_chiryoku { get; set; }
            public string assist3_chiryoku { get; set; }
            public string unmei { get; set; }
            public bool leader_flag { get; set; }
        }
        public class shidan_savedata
        {
            public string shidan_name { get; set; }
            public string shidan_id { get; set; }
            public string shidan_assist_skill { get; set; }
            public Dictionary<int, character_info> character { get; set; }
        }
        public class legion_savedata 
        {
            public string legion_name { get; set; }
            public string legion_id { get; set; }
            public Dictionary<int, shidan_savedata> shidan { get; set; }
        }

        public class ItemSet
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
        Dictionary<string, (int, int)> shidan_skill = new Dictionary<string, (int, int)>();
        Dictionary<string, (int, int)> leg_shidan1_skill = new Dictionary<string, (int, int)>();
        Dictionary<string, (int, int)> leg_shidan2_skill = new Dictionary<string, (int, int)>();
        Dictionary<string, (int, int)> leg_shidan3_skill = new Dictionary<string, (int, int)>();
        Dictionary<string, (int, int)> legion_skill = new Dictionary<string, (int, int)>();

        ObservableCollection<savedata> all_save_data = new ObservableCollection<savedata>();
        ObservableCollection<shidan_savedata> all_shidan_savedata = new ObservableCollection<shidan_savedata>();
        ObservableCollection<ItemSet> shidan_assist = new ObservableCollection<ItemSet>();
        List<CharacterJson> characters = null; // ここで宣言
        Dictionary<string, List<EquipmentJson>> all_equipments; // ここで宣言
        Dictionary<string, List<ShogoJson>> all_shogo; // ここで宣言
        List<string> all_asistskills;
        Dictionary<int, character_info> all_characters = new Dictionary<int, character_info>();
        Dictionary<int, character_info> all_characters_load = new Dictionary<int, character_info>();

        Dictionary<int, character_info> leg_shidan1_characters = new Dictionary<int, character_info>();
        Dictionary<int, character_info> leg_shidan2_characters = new Dictionary<int, character_info>();
        Dictionary<int, character_info> leg_shidan3_characters = new Dictionary<int, character_info>();

        ObservableCollection<shidan_savedata> leg_all_shidan_savedata = new ObservableCollection<shidan_savedata>();


        public MainWindow()
        {
            InitializeComponent();

            all_equipments = new Dictionary<string, List<EquipmentJson>>();
            all_shogo = new Dictionary<string, List<ShogoJson>>();
            all_asistskills = new List<string>();


            load_json_character();
            load_json_equipment();
            load_json_shogo();
            shidan_saved_list.ItemsSource = all_shidan_savedata;
            saved_list.ItemsSource = all_save_data;

            if (File.Exists("./json/saved.json"))
            {
                string loadedJson = File.ReadAllText("./json/saved.json");
                //all_save_dataをすべて消す
                all_save_data.Clear();
                //all_save_dataに読み込んだjsonを追加する
                all_save_data = JsonConvert.DeserializeObject<ObservableCollection<savedata>>(loadedJson);
                if (all_save_data == null)
                {
                    all_save_data = new ObservableCollection<savedata>();
                }
                saved_list.ItemsSource = all_save_data;
            }
            else
            {
            }
            if (File.Exists("./json/shidan.json"))
            {
                string loadedJson = File.ReadAllText("./json/shidan.json");
                //all_save_dataをすべて消す
                all_shidan_savedata.Clear();
                //all_save_dataに読み込んだjsonを追加する
                all_shidan_savedata = JsonConvert.DeserializeObject<ObservableCollection<shidan_savedata>>(loadedJson);
                if (all_shidan_savedata == null)
                {
                    all_shidan_savedata = new ObservableCollection<shidan_savedata>();
                }
                shidan_saved_list.ItemsSource = all_shidan_savedata;
            }
            else
            {
            }
            assist_skill_box.ItemsSource = shidan_assist;
            assist_skill_box.DisplayMemberPath = "Name";
            assist_skill_box.SelectedValuePath = "Name";

            //レギオン関連

            //shidan.jsonが保存されていないと破綻するので別にしておく。
            leg_hozon_shidan_list.ItemsSource = leg_all_shidan_savedata;
            if (File.Exists("./json/shidan.json"))
            {
                string loadedJson = File.ReadAllText("./json/shidan.json");
                //all_save_dataをすべて消す
                leg_all_shidan_savedata.Clear();
                //all_save_dataに読み込んだjsonを追加する
                leg_all_shidan_savedata = JsonConvert.DeserializeObject<ObservableCollection<shidan_savedata>>(loadedJson);
                if (leg_all_shidan_savedata == null)
                {
                    leg_all_shidan_savedata = new ObservableCollection<shidan_savedata>();
                }
                leg_hozon_shidan_list.ItemsSource = leg_all_shidan_savedata;
            }
            else
            {
            }

        }

        private int ExtractNumberFromBracket(string input)
        {
            int startIndex = input.IndexOf('[');
            int endIndex = input.IndexOf(']');
            if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
            {
                string numberString = input.Substring(startIndex + 1, endIndex - startIndex - 1);
                if (int.TryParse(numberString, out int number))
                {
                    return number;
                }
            }
            return 0; // 解析に失敗した場合は0を返す
        }
        //:で区切られた文字列をstringとintに分割して返す
        private string ReturnColonString(string input)
        {
            int colonIndex = input.IndexOf(':');
            if (colonIndex != -1)
            {
                return input.Substring(0, colonIndex);
            }
            return input; // 解析に失敗した場合は元の文字列を返す
        }


        private void change_character_box(int number)
        {
            var selected = saved_list.SelectedItem as savedata;
            param_status soubi_status;
            param_status shogo_status;
            param_status character_status;
            soubi_status = new param_status();
            shogo_status = new param_status();
            if (selected != null)
            {
                // キャラクター選択
                CharacterJson characterObj = characters.Find(c => c.名称 == selected.character_name);

                if (characterObj != null)
                {
                    var box = (TextBox)this.FindName($"character{number}_name_box");
                    box.Text = characterObj.名称;
                    character_info character_Info = new character_info();
                    //キャラクターのパッシブスキル・装備・称号からスキルを設定する
                    //同一名称のスキルは加算
                    //パッシブスキルはcharacterObjの中から"スキル名:数値"の形になっている
                    //装備と称号は名前しか入っていないので、all_equipmentsとall_shogoから探す

                    //まずはパッシブスキルをパースして、character_infoに格納する
                    character_Info.character_name = characterObj.名称;
                    character_Info.character_id = number.ToString();
                    character_Info.character_kago = characterObj.加護;
                    character_Info.rank = characterObj.ランク;
                    character_Info.stance = characterObj.スタンス;
                    character_Info.assist_skill = characterObj.アシストスキル[0];
                    string temp_skill_name;
                    int temp_skill_value;

                    (temp_skill_name, temp_skill_value) = SkillParser.Div_Skill_Name_Value(characterObj.リーダースキル[0]);
                    character_Info.leader_skill.Add(temp_skill_name, temp_skill_value);
                    (temp_skill_name, temp_skill_value) = SkillParser.Div_Skill_Name_Value(characterObj.リーダースキル[1]);
                    character_Info.leader_skill.Add(temp_skill_name, temp_skill_value);
                    Dictionary<string, int> temp_skills = new Dictionary<string, int>();
                    foreach (var skill in characterObj.パッシブスキル)
                    {
                        //string skill_name = ReplaceBracketNumberWithColon(skill);
                        string skill_name = skill.Split('[')[0];
                        int skill_value = ExtractNumberFromBracket(skill);
                        if (temp_skills.ContainsKey(skill_name))
                        {
                            temp_skills[skill_name] += skill_value;
                        }
                        else
                        {
                            temp_skills[skill_name] = skill_value;
                        }
                    }
                    //次に装備を検索してcharacter_infoに格納する。装備名しか入っていないので、all_equipmentsから同一名称を探してパースして被ったら加算する
                    string[] equipment1_parse = null;
                    if (selected.equipment1 != null && selected.equipment1 != "")
                        equipment1_parse = selected.equipment1_name.Split('(');
                    //all_equipmentsの中のequipment1_parse[0]と同じ名称のものを探す
                    if (equipment1_parse != null && equipment1_parse.Length > 0)
                    {
                        // all_equipments[characterObj.装備[0]] は List<EquipmentJson> 型なので、ContainsKeyは使えない
                        // 代わりにList<EquipmentJson>から名称一致で探す
                        var equipmentList = all_equipments.ContainsKey(characterObj.装備[0]) ? all_equipments[characterObj.装備[0]] : null;
                        character_Info.character_equipment1 = equipment1_parse[0];
                        if (equipmentList != null)
                        {
                            var equipmentObj = equipmentList.Find(e => e.名称 == equipment1_parse[0]);
                            if (equipmentObj != null)
                            {
                                foreach (var skill in equipmentObj.能力付加)
                                {
                                    string[] skill_name = skill.Split(':');
                                    if (temp_skills.ContainsKey(skill_name[0]))
                                    {
                                        int skill_value = 0;
                                        if (skill_name.Length > 1)
                                            skill_value = int.Parse(skill_name[1]);
                                        temp_skills[skill_name[0]] += skill_value;
                                    }
                                    else
                                    {
                                        int skill_value = 0;
                                        if (skill_name.Length > 1)
                                            skill_value = int.Parse(skill_name[1]);
                                        temp_skills[skill_name[0]] = skill_value;
                                    }
                                }
                                soubi_status.kougeki += int.Parse(equipmentObj.性能変化_攻);
                                soubi_status.bougyo += int.Parse(equipmentObj.性能変化_防);
                                soubi_status.sokudo += int.Parse(equipmentObj.性能変化_速);
                                soubi_status.chiryoku += int.Parse(equipmentObj.性能変化_知);
                            }
                        }
                    }
                    if (selected.equipment2 != null && selected.equipment2 != "")
                    {
                        string[] equipment2_parse = selected.equipment2_name.Split('(');
                        if (equipment2_parse != null && equipment2_parse.Length > 0)
                        {
                            character_Info.character_equipment2 = equipment2_parse[0];
                            var equipmentList = all_equipments.ContainsKey(characterObj.装備[1]) ? all_equipments[characterObj.装備[1]] : null;
                            if (equipmentList != null)
                            {
                                var equipmentObj = equipmentList.Find(e => e.名称 == equipment2_parse[0]);
                                if (equipmentObj != null)
                                {
                                    foreach (var skill in equipmentObj.能力付加)
                                    {
                                        string[] skill_name = skill.Split(':');
                                        if (temp_skills.ContainsKey(skill_name[0]))
                                        {
                                            int skill_value = 0;
                                            if (skill_name.Length > 1)
                                                skill_value = int.Parse(skill_name[1]);
                                            temp_skills[skill_name[0]] += skill_value;
                                        }
                                        else
                                        {
                                            int skill_value = 0;
                                            if (skill_name.Length > 1)
                                                skill_value = int.Parse(skill_name[1]);
                                            temp_skills[skill_name[0]] = skill_value;
                                        }
                                    }
                                    soubi_status.kougeki += int.Parse(equipmentObj.性能変化_攻);
                                    soubi_status.bougyo += int.Parse(equipmentObj.性能変化_防);
                                    soubi_status.sokudo += int.Parse(equipmentObj.性能変化_速);
                                    soubi_status.chiryoku += int.Parse(equipmentObj.性能変化_知);
                                }
                            }
                        }
                    }
                    if (selected.ryoshoku != null && selected.ryoshoku != "")
                    {
                        string[] ryoshoku_parse = selected.ryoshoku_name.Split('(');
                        if (ryoshoku_parse != null && ryoshoku_parse.Length > 0)
                        {
                            character_Info.character_ryoshoku = ryoshoku_parse[0];
                            var equipmentList = all_equipments.ContainsKey("糧食") ? all_equipments["糧食"] : null;
                            if (equipmentList != null)
                            {
                                var equipmentObj = equipmentList.Find(e => e.名称 == ryoshoku_parse[0]);
                                if (equipmentObj != null)
                                {
                                    foreach (var skill in equipmentObj.能力付加)
                                    {
                                        string[] skill_name = skill.Split(':');
                                        if (temp_skills.ContainsKey(skill_name[0]))
                                        {
                                            int skill_value = 0;
                                            if (skill_name.Length > 1)
                                                skill_value = int.Parse(skill_name[1]);
                                            temp_skills[skill_name[0]] += skill_value;
                                        }
                                        else
                                        {
                                            int skill_value = 0;
                                            if (skill_name.Length > 1)
                                                skill_value = int.Parse(skill_name[1]);
                                            temp_skills[skill_name[0]] = skill_value;
                                        }
                                    }
                                    soubi_status.kougeki += int.Parse(equipmentObj.性能変化_攻);
                                    soubi_status.bougyo += int.Parse(equipmentObj.性能変化_防);
                                    soubi_status.sokudo += int.Parse(equipmentObj.性能変化_速);
                                    soubi_status.chiryoku += int.Parse(equipmentObj.性能変化_知);
                                }
                            }
                        }
                    }
                    //最後に称号を検索してcharacter_infoに格納する。称号名しか入っていないので、all_shogoから同一名称を探してパースして被ったら加算する
                    if (selected.shogo1 != null && selected.shogo1 != "")
                    {
                        string[] shogo1_parse = selected.shogo1_name.Split('(');
                        if (shogo1_parse != null && shogo1_parse.Length > 0)
                        {

                            character_Info.character_shogo1 = shogo1_parse[0];

                            foreach (var shogoList in all_shogo.Values)
                            {
                                var shogoObj = shogoList.Find(s => s.二つ名 == shogo1_parse[0]);
                                if (shogoObj != null)
                                {
                                    foreach (var skill in shogoObj.能力付与)
                                    {
                                        if (skill.Key == "加護")
                                        {
                                            if (!string.IsNullOrEmpty(skill.Value))
                                            {
                                                if (character_Info.character_kago.Length > 1)
                                                    character_Info.character_kago = skill.Value + character_Info.character_kago[1];
                                                else
                                                    character_Info.character_kago = skill.Value;
                                            }
                                            continue;
                                        }
                                        if (skill.Key == "追加スキル")
                                        {
                                            string[] skill_name = skill.Value.Split(':');
                                            if (temp_skills.ContainsKey(skill_name[0]))
                                            {
                                                int skill_value = 0;
                                                if (skill_name.Length > 1)
                                                    skill_value = int.Parse(skill_name[1]);
                                                temp_skills[skill_name[0]] += skill_value;
                                            }
                                            else
                                            {
                                                int skill_value = 0;
                                                if (skill_name.Length > 1)
                                                    skill_value = int.Parse(skill_name[1]);
                                                temp_skills[skill_name[0]] = skill_value;
                                            }
                                        }
                                    }
                                    foreach (var skill in shogoObj.ステータス変化)
                                    {
                                        if (skill.Key == "攻撃")
                                            shogo_status.kougeki += int.Parse(skill.Value);
                                        else if (skill.Key == "防御")
                                            shogo_status.bougyo += int.Parse(skill.Value);
                                        else if (skill.Key == "速度")
                                            shogo_status.sokudo += int.Parse(skill.Value);
                                        else if (skill.Key == "知力")
                                            shogo_status.chiryoku += int.Parse(skill.Value);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    if (selected.shogo2 != null)
                    {
                        string[] shogo2_parse = selected.shogo2_name.Split("(");
                        if (shogo2_parse != null)
                        {
                            character_Info.character_shogo2 = shogo2_parse[0];

                            foreach (var shogoList in all_shogo.Values)
                            {
                                var shogoObj = shogoList.Find(s => s.二つ名 == shogo2_parse[0]);
                                if (shogoObj != null)
                                {
                                    foreach (var skill in shogoObj.能力付与)
                                    {
                                        if (skill.Key == "加護")
                                        {
                                            if (!string.IsNullOrEmpty(skill.Value))
                                                character_Info.character_kago = character_Info.character_kago[0] + skill.Value;
                                            continue;
                                        }
                                        if (skill.Key == "追加スキル")
                                        {
                                            string[] skill_name = skill.Value.Split(':');
                                            if (temp_skills.ContainsKey(skill_name[0]))
                                            {
                                                int skill_value = 0;
                                                if (skill_name.Length > 1)
                                                    skill_value = int.Parse(skill_name[1]);
                                                temp_skills[skill_name[0]] += skill_value;
                                            }
                                            else
                                            {
                                                int skill_value = 0;
                                                if (skill_name.Length > 1)
                                                    skill_value = int.Parse(skill_name[1]);
                                                temp_skills[skill_name[0]] = skill_value;
                                            }
                                        }
                                    }
                                    foreach (var skill in shogoObj.ステータス変化)
                                    {
                                        if (skill.Key == "攻撃")
                                            shogo_status.kougeki += int.Parse(skill.Value);
                                        else if (skill.Key == "防御")
                                            shogo_status.bougyo += int.Parse(skill.Value);
                                        else if (skill.Key == "速度")
                                            shogo_status.sokudo += int.Parse(skill.Value);
                                        else if (skill.Key == "知力")
                                            shogo_status.chiryoku += int.Parse(skill.Value);
                                    }
                                }
                            }
                        }
                    }
                    character_Info.character_skill = temp_skills;
                    //キャラクターの基礎ステータスを設定する
                    param_status tempstatus = new param_status();
                    status_calc_fix(characterObj, soubi_status, shogo_status, tempstatus,number);
                    Debug.WriteLine("最終ステータス:" + tempstatus.hp + " " + tempstatus.kougeki + " " + tempstatus.bougyo + " " + tempstatus.sokudo + " " + tempstatus.chiryoku);
                    character_Info.character_status.Add("HP", tempstatus.hp);
                    character_Info.character_status.Add("攻撃", tempstatus.kougeki);
                    character_Info.character_status.Add("防御", tempstatus.bougyo);
                    character_Info.character_status.Add("速度", tempstatus.sokudo);
                    character_Info.character_status.Add("知力", tempstatus.chiryoku);
                    character_Info.cost = int.Parse(characterObj.コスト);
                    character_Info.soubi_status = new param_status();
                    character_Info.soubi_status.hp = soubi_status.hp;
                    character_Info.soubi_status.kougeki = soubi_status.kougeki;
                    character_Info.soubi_status.bougyo = soubi_status.bougyo;
                    character_Info.soubi_status.sokudo = soubi_status.sokudo;
                    character_Info.soubi_status.chiryoku = soubi_status.chiryoku;
                    character_Info.shogo_status = new param_status();
                    character_Info.shogo_status.hp = shogo_status.hp;
                    character_Info.shogo_status.kougeki = shogo_status.kougeki;
                    character_Info.shogo_status.bougyo = shogo_status.bougyo;
                    character_Info.shogo_status.sokudo = shogo_status.sokudo;
                    character_Info.shogo_status.chiryoku = shogo_status.chiryoku;
                    character_Info.base_status = new param_status();
                    var textBox = (TextBox)this.FindName($"buko{number}_fig");
                    if (textBox != null && textBox.Text != "" && all_characters.ContainsKey(number))
                    {
                        textBox.Text = all_characters[number].buko.ToString();
                    }
                    else
                    {
                        textBox.Text = "100";
                    }
                    character_Info.buko = int.Parse(textBox.Text);

                    character_Info.base_status.hp = int.Parse(characterObj.基本パラメータ_HP);
                    character_Info.base_status.kougeki = int.Parse(characterObj.基本パラメータ_攻);
                    character_Info.base_status.bougyo = int.Parse(characterObj.基本パラメータ_防);
                    character_Info.base_status.sokudo = int.Parse(characterObj.基本パラメータ_速);
                    character_Info.base_status.chiryoku = int.Parse(characterObj.基本パラメータ_知);
                    character_Info.character_current_status.Add("HP", tempstatus.hp);
                    character_Info.character_current_status.Add("攻撃", tempstatus.kougeki);
                    character_Info.character_current_status.Add("防御", tempstatus.bougyo);
                    character_Info.character_current_status.Add("速度", tempstatus.sokudo);
                    character_Info.character_current_status.Add("知力", tempstatus.sokudo);
                    character_Info.character_shuzoku = characterObj.種族;

                    //キャラクターのパラメータを表示する
                    var character1_status_box = (TextBox)this.FindName($"character{number}_status_box");
                    character1_status_box.Text = "HP:" + character_Info.character_status["HP"] + "\n";
                    character1_status_box.Text += "攻撃:" + character_Info.character_status["攻撃"] + "\n";
                    character1_status_box.Text += "防御:" + character_Info.character_status["防御"] + "\n";
                    character1_status_box.Text += "速度:" + character_Info.character_status["速度"] + "\n";
                    character1_status_box.Text += "知力:" + character_Info.character_status["知力"] + "\n";
                    character1_status_box.Text += "称号1:" + character_Info.character_shogo1 + "\n";
                    character1_status_box.Text += "称号2:" + character_Info.character_shogo2 + "\n";
                    character1_status_box.Text += "装備1:" + character_Info.character_equipment1 + "\n";
                    character1_status_box.Text += "装備2:" + character_Info.character_equipment2 + "\n";
                    character1_status_box.Text += "糧食:" + character_Info.character_ryoshoku + "\n";
                    character1_status_box.Text += "種族:" + character_Info.character_shuzoku + "\n";
                    character1_status_box.Text += "加護:" + character_Info.character_kago + "\n";
                    //shidan_assistの中にnumberがあればアシストスキルを入れ替える
                    if (shidan_assist.Any(item => item.Id == number))
                    {
                        shidan_assist.Remove(shidan_assist.First(item => item.Id == number));
                    }
                    shidan_assist.Add(new ItemSet { Id = number, Name = characterObj.アシストスキル[0] });
                    //character_infoにすべて登録する
                    character_Info.soubi_status = soubi_status;
                    //いったんデバッグ。character1に格納されているスキルをcharacter1_skill_boxへ表示する
                    var character1_skill_box = (TextBox)this.FindName($"character{number}_skill_box");
                    character1_skill_box.Text = "";
                    foreach (var skill in temp_skills)
                    {
                        character1_skill_box.Text += skill.Key + ":" + skill.Value + "\n";
                    }
                    //デバッグここまで
                    if (all_characters.ContainsKey(number))
                        all_characters.Remove(number);
                    all_characters.Add(number, character_Info);
                    var temp_leader_check = (CheckBox)this.FindName($"leader{number}_flag");
                    if (temp_leader_check != null && temp_leader_check.IsChecked == true && character_Info.leader_skill.Count > 0)
                    {
                        add_leader_skill(number);
                    }
                    //師団スキルを設定する
                    set_shidan_skill();
                    //師団スキルを表示する
                    shidan_skill_box.Text = "";
                    foreach (var skill in shidan_skill)
                    {
                        if (skill.Key != "四法結界")
                        {

                            shidan_skill_box.Text += skill.Key;

                            if (skill.Value.Item1 != 0)
                                shidan_skill_box.Text += ":" + skill.Value.Item1;
                            shidan_skill_box.Text += "\n";
                        }
                    }
                }
            }
        }
        private (int, int, int, int) calc_kassei(string kassei_name, character_info character, int skill_value, bool kigen_flag, int temp_kougeki, int temp_bougyo, int temp_sokudo, int temp_chiryoku)
        {
            string temp_kassei = "";

            string skill_head_word = kassei_name.Substring(0, 1);
            int temp_self_kassei = 0;
            string[] keywords = { "火", "水", "風", "土", "光", "闇" };
            //skill_head_wordがkeywordsに含まれていたら、属性活性として扱う
            if (keywords.Contains(skill_head_word))
            {
                if (character.character_kago.Contains(skill_head_word) || kigen_flag)
                {

                    if (character.character_skill.ContainsKey(kassei_name))
                        temp_self_kassei = character.character_skill[kassei_name];
                    temp_kougeki += skill_value - temp_self_kassei;
                    temp_bougyo += skill_value - temp_self_kassei;
                    temp_sokudo += skill_value - temp_self_kassei;
                    temp_chiryoku += (skill_value - temp_self_kassei) / 4;

                }
            }
            else if (character.character_shuzoku.Contains(skill_head_word) || kigen_flag)
            {
                //もし自分が活性を持ってたらその数値分をマイナス
                if (character.character_skill.ContainsKey(kassei_name))
                    temp_self_kassei = character.character_skill[kassei_name];
                temp_kougeki += skill_value - temp_self_kassei;
                temp_bougyo += skill_value - temp_self_kassei;
                temp_sokudo += skill_value - temp_self_kassei;
                temp_chiryoku += (skill_value - temp_self_kassei) / 4;
            }
            return (temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku);
        }
        private (int, int, int, int) shidan_kassei_skill(MainWindow.character_info character, string skillname, int temp_kougeki, int temp_bougyo, int temp_sokudo, int temp_chiryoku)
        {
            int temp_self_kassei = 0;
            if (character.character_skill.ContainsKey(skillname))
                temp_self_kassei = character.character_skill[skillname];
            temp_kougeki += shidan_skill[skillname].Item1 - temp_self_kassei;
            temp_bougyo += shidan_skill[skillname].Item1 - temp_self_kassei;
            temp_sokudo += shidan_skill[skillname].Item1 - temp_self_kassei;
            temp_chiryoku += (shidan_skill[skillname].Item1 - temp_self_kassei) / 4;
            return (temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku);
        }
        private void character_kassei_update()
        {
            int temp_kougeki = 0;
            int temp_bougyo = 0;
            int temp_sokudo = 0;
            int temp_chiryoku = 0;
            //師団スキルにある活性をもとに、キャラクターのステータスを更新する
            int i = 1;
            foreach (var character in all_characters)
            {

                // 各キャラクターごとに初期化
                temp_kougeki = 0;
                temp_bougyo = 0;
                temp_sokudo = 0;
                temp_chiryoku = 0;
                bool kigen_flag = character.Value.character_skill.ContainsKey("血の起源");
                foreach (var skill in shidan_skill)
                {
                    //活性は5番
                    if (skill.Value.Item2 == 6)
                    {
                        if (skill.Key == "師団活性")
                        {
                            (temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku) = shidan_kassei_skill(character.Value, "師団活性", temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku);
                        }
                        else if (skill.Key == "血の起源")
                        {
                            (temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku) = shidan_kassei_skill(character.Value, "血の起源", temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku);
                        }
                        else if (skill.Key == "軍団活性")
                        {
                            (temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku) = shidan_kassei_skill(character.Value, "軍団活性", temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku);
                        }
                        else if (skill.Key == "師団支配")
                        {
                            (temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku) = shidan_kassei_skill(character.Value, "師団支配", temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku);
                        }
                        else if (skill.Key == "軍団支配")
                        {
                            (temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku) = shidan_kassei_skill(character.Value, "軍団支配", temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku);
                        }
                        //布陣があったら
                        else if (skill.Key.Contains("布陣"))
                        {
                            if (skill.Key == "攻撃布陣")
                                temp_kougeki += shidan_skill[skill.Key].Item1;
                            else if (skill.Key == "防御布陣")
                                temp_bougyo += shidan_skill[skill.Key].Item1;
                            else if (skill.Key == "速度布陣")
                                temp_sokudo += shidan_skill[skill.Key].Item1;
                            else if (skill.Key == "知力布陣")
                                temp_chiryoku += (shidan_skill[skill.Key].Item1) / 4;
                        }
                        else if (skill.Key == "武具研磨")
                        {
                            int temp_self_kassei = 0;
                            if (character.Value.character_skill.ContainsKey("武具研磨"))
                                temp_self_kassei = character.Value.character_skill["武具研磨"];

                            temp_kougeki += (int)(((skill.Value.Item1 - temp_self_kassei) / 100.0) * character.Value.soubi_status.kougeki);
                            temp_bougyo += (int)(((skill.Value.Item1 - temp_self_kassei) / 100.0) * character.Value.soubi_status.bougyo);
                            temp_sokudo += (int)(((skill.Value.Item1 - temp_self_kassei) / 100.0) * character.Value.soubi_status.sokudo);
                            temp_sokudo += (int)(((skill.Value.Item1 - temp_self_kassei) / 100.0) * character.Value.soubi_status.chiryoku);
                        }
                        else if (skill.Key == "死の軍勢")
                        {
                            int temp_self_kassei = 0;
                            if (character.Value.character_skill.ContainsKey("死の軍勢"))
                                temp_self_kassei = character.Value.character_skill["死の軍勢"];
                            if (character.Value.character_shuzoku.Contains("死") || kigen_flag)
                            {
                                temp_kougeki += skill.Value.Item1 - temp_self_kassei;
                                temp_bougyo += skill.Value.Item1 - temp_self_kassei;
                                temp_sokudo += skill.Value.Item1 - temp_self_kassei;
                                temp_chiryoku += (skill.Value.Item1 - temp_self_kassei) / 4;
                            }
                        }
                        else
                        {
                            int temp1, temp2, temp3, temp4;
                            (temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku) = calc_kassei(skill.Key, character.Value, skill.Value.Item1, kigen_flag, temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku);
                        }
                    }
                }
                temp_kougeki += character.Value.character_status["攻撃"];
                temp_bougyo += character.Value.character_status["防御"];
                temp_sokudo += character.Value.character_status["速度"];
                temp_chiryoku += character.Value.character_status["知力"];
                var character1_status_box = (TextBox)this.FindName($"character{character.Value.character_id}_status_box");
                character.Value.character_current_status["攻撃"] = temp_kougeki;
                character.Value.character_current_status["防御"] = temp_bougyo;
                character.Value.character_current_status["速度"] = temp_sokudo;
                character.Value.character_current_status["知力"] = temp_chiryoku;

                character1_status_box.Text = "HP:" + character.Value.character_status["HP"] + "\n";
                character1_status_box.Text += "攻撃:" + character.Value.character_current_status["攻撃"] + "(" + character.Value.character_status["攻撃"] + ")" + "\n";
                character1_status_box.Text += "防御:" + character.Value.character_current_status["防御"] + "(" + character.Value.character_status["防御"] + ")" + "\n";
                character1_status_box.Text += "速度:" + character.Value.character_current_status["速度"] + "(" + character.Value.character_status["速度"] + ")" + "\n";
                character1_status_box.Text += "知力:" + character.Value.character_current_status["知力"] + "(" + character.Value.character_status["知力"] + ")" + "\n";
                character1_status_box.Text += "称号1:" + character.Value.character_shogo1 + "\n";
                character1_status_box.Text += "称号2:" + character.Value.character_shogo2 + "\n";
                character1_status_box.Text += "装備1:" + character.Value.character_equipment1 + "\n";
                character1_status_box.Text += "装備2:" + character.Value.character_equipment2 + "\n";
                character1_status_box.Text += "糧食:" + character.Value.character_ryoshoku + "\n";
                character1_status_box.Text += "種族:" + character.Value.character_shuzoku + "\n";
                character1_status_box.Text += "加護:" + character.Value.character_kago + "\n";

                i++;
            }
        }
        //指揮の計算。加算と異なり積算する
        private (double, double, double, double) calc_shiki(string kassei_name, character_info character, int skill_value, bool kigen_flag, double temp_kougeki, double temp_bougyo, double temp_sokudo, double temp_chiryoku)
        {
            string temp_kassei = "";

            string skill_head_word = kassei_name.Substring(0, 1);
            int temp_self_kassei = 0;
            string[] keywords = { "火", "水", "風", "土", "光", "闇" };
            //skill_head_wordがkeywordsに含まれていたら、属性活性として扱う
            if (keywords.Contains(skill_head_word))
            {
                if (character.character_kago.Contains(skill_head_word) || kigen_flag)
                {
                    if (character.character_skill.ContainsKey(kassei_name))
                        temp_self_kassei = character.character_skill[kassei_name];
                    temp_kougeki *= (100.0 + skill_value - temp_self_kassei) / 100.0;
                    temp_bougyo *= (100.0 + skill_value - temp_self_kassei) / 100.0;
                    temp_sokudo *= (100.0 + skill_value - temp_self_kassei) / 100.0;
                    temp_chiryoku *= (400.0 + skill_value - temp_self_kassei) / 400.0;
                }
            }
            else if (character.character_shuzoku.Contains(skill_head_word) || kigen_flag)
            {
                //もし自分が活性を持ってたらその数値分をマイナス
                if (character.character_skill.ContainsKey(kassei_name))
                    temp_self_kassei = character.character_skill[kassei_name];
                temp_kougeki *= (100.0 + skill_value - temp_self_kassei) / 100.0;
                temp_bougyo *= (100.0 + skill_value - temp_self_kassei) / 100.0;
                temp_sokudo *= (100.0 + skill_value - temp_self_kassei) / 100.0;
                temp_chiryoku *= (400.0 + skill_value - temp_self_kassei) / 400.0;
            }
            return (temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku);
        }
        private (double, double, double, double) shidan_shiki_skill(MainWindow.character_info character, string skillname, double temp_kougeki, double temp_bougyo, double temp_sokudo, double temp_chiryoku)
        {
            int temp_self_kassei = 0;
            if (character.character_skill.ContainsKey(skillname))
                temp_self_kassei = character.character_skill[skillname];
            temp_kougeki *= (100.0 + shidan_skill[skillname].Item1 - temp_self_kassei) / 100.0;
            temp_bougyo *= (100.0 + shidan_skill[skillname].Item1 - temp_self_kassei) / 100.0;
            temp_sokudo *= (100.0 + shidan_skill[skillname].Item1 - temp_self_kassei) / 100.0;
            temp_chiryoku *= (400.0 + shidan_skill[skillname].Item1 - temp_self_kassei) / 400.0;
            return (temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku);
        }

        private void character_shiki_update()
        {
            //師団スキルにある活性をもとに、キャラクターのステータスを更新する
            int i = 1;
            foreach (var character in all_characters)
            {

                // 各キャラクターごとに初期化
                double temp_kougeki = 1.0;
                double temp_bougyo = 1.0;
                double temp_sokudo = 1.0;
                double temp_chiryoku = 1.0;
                bool kigen_flag = character.Value.character_skill.ContainsKey("血の起源");
                foreach (var skill in shidan_skill)
                {
                    //指揮は6
                    if (skill.Value.Item2 == 7)
                    {
                        if (skill.Key == "師団指揮")
                        {
                            (temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku) = shidan_shiki_skill(character.Value, "師団指揮", temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku);
                        }
                        else if (skill.Key == "軍団指揮")
                        {
                            (temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku) = shidan_shiki_skill(character.Value, "軍団指揮", temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku);
                        }
                        //布陣があったら
                        else if (skill.Key.Contains("攻撃") || skill.Key.Contains("防御") || skill.Key.Contains("速度") || skill.Key.Contains("知力"))
                        {
                            if (skill.Key == "攻撃指揮")
                                temp_kougeki *= (100.0 + shidan_skill[skill.Key].Item1) / 100.0;
                            else if (skill.Key == "防御指揮")
                                temp_bougyo *= (100.0 + shidan_skill[skill.Key].Item1) / 100.0;
                            else if (skill.Key == "速度指揮")
                                temp_sokudo *= (100.0 + shidan_skill[skill.Key].Item1) / 100.0;
                            else if (skill.Key == "知力指揮")
                                temp_chiryoku *= (100.0 + shidan_skill[skill.Key].Item1) / 100.0;
                        }
                        else
                        {
                            (temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku) = calc_shiki(skill.Key, character.Value, skill.Value.Item1, kigen_flag, temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku);

                        }
                    }
                    if (skill.Key == "英雄覇気")
                    {
                        (temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku) = shidan_shiki_skill(character.Value, "英雄覇気", temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku);
                    }
                }
                temp_kougeki *= character.Value.character_current_status["攻撃"];
                temp_bougyo *= character.Value.character_current_status["防御"];
                temp_sokudo *= character.Value.character_current_status["速度"];
                temp_chiryoku *= character.Value.character_current_status["知力"];
                var character1_status_box = (TextBox)this.FindName($"character{character.Value.character_id}_status_box");
                character.Value.character_current_status["攻撃"] = (int)temp_kougeki;
                character.Value.character_current_status["防御"] = (int)temp_bougyo;
                character.Value.character_current_status["速度"] = (int)temp_sokudo;
                character.Value.character_current_status["知力"] = (int)temp_chiryoku;

                character1_status_box.Text = "HP:" + character.Value.character_status["HP"] + "\n";
                character1_status_box.Text += "攻撃:" + character.Value.character_current_status["攻撃"] + "(" + character.Value.character_status["攻撃"] + ")" + "\n";
                character1_status_box.Text += "防御:" + character.Value.character_current_status["防御"] + "(" + character.Value.character_status["防御"] + ")" + "\n";
                character1_status_box.Text += "速度:" + character.Value.character_current_status["速度"] + "(" + character.Value.character_status["速度"] + ")" + "\n";
                character1_status_box.Text += "知力:" + character.Value.character_current_status["知力"] + "(" + character.Value.character_status["知力"] + ")" + "\n";
                character1_status_box.Text += "称号1:" + character.Value.character_shogo1 + "\n";
                character1_status_box.Text += "称号2:" + character.Value.character_shogo2 + "\n";
                character1_status_box.Text += "装備1:" + character.Value.character_equipment1 + "\n";
                character1_status_box.Text += "装備2:" + character.Value.character_equipment2 + "\n";
                character1_status_box.Text += "糧食:" + character.Value.character_ryoshoku + "\n";
                character1_status_box.Text += "種族:" + character.Value.character_shuzoku + "\n";
                character1_status_box.Text += "加護:" + character.Value.character_kago + "\n";
                i++;
            }
        }

        private void Character1_load_Button_Click(object sender, RoutedEventArgs e)
        {
            change_character_box(1);
            character_kassei_update();
            character_shiki_update();
            kago_calc();
        }
        private void Character2_load_Button_Click(object sender, RoutedEventArgs e)
        {
            change_character_box(2);
            character_kassei_update();
            character_shiki_update();
            kago_calc();
        }
        private void Character3_load_Button_Click(object sender, RoutedEventArgs e)
        {
            change_character_box(3);
            character_kassei_update();
            character_shiki_update();
            kago_calc();
        }

        private void Character4_load_Button_Click(object sender, RoutedEventArgs e)
        {
            change_character_box(4);
            character_kassei_update();
            character_shiki_update();
            kago_calc();
        }
        private void Character5_load_Button_Click(object sender, RoutedEventArgs e)
        {
            change_character_box(5);
            character_kassei_update();
            character_shiki_update();
            kago_calc();
        }
        private void Character6_load_Button_Click(object sender, RoutedEventArgs e)
        {
            change_character_box(6);
            character_kassei_update();
            character_shiki_update();
            kago_calc();
        }

        private void status_calc_fix(CharacterJson characterObj, param_status soubi_status, param_status shogo_status, param_status temp_status,int number)
        {
            double keikenti = 0;
            int hp, soku, kou, bou, chiryoku, cost;
            int level = 0;
            string rank = characterObj.ランク;
            if (rank == "S")
            {
                keikenti += 1110050;
                level = 150;
            }
            if (rank == "A")
            {
                keikenti += 966050;
                level = 140;
            }
            if (rank == "B")
            {
                keikenti += 832050;
                level = 130;
            }
            if (rank == "C")
            {
                keikenti += 708050;
                level = 120;
            }
            if (rank == "D")
            {
                keikenti += 594050;
                level = 110;
            }
            if (rank == "E")
            {
                keikenti += 490050;
                level = 100;
            }
            int temp_buko = 100;
            var textBox = (TextBox)this.FindName($"buko{number}_fig");       
            if (int.TryParse(textBox.Text, out temp_buko))
            {
                if (temp_buko > 100 ) temp_buko = 100;
            }
            else
            {
                temp_buko = 100;
            }
                double buko = (temp_buko - 25.0) / 2.0 / (Math.Sqrt(int.Parse(characterObj.コスト)));
            double level1 = 1.0 + Math.Sqrt(keikenti) / 1000.0;
            double level2 = Math.Sqrt(keikenti) / 50.0;

            double temp = (level - 1.0) / 4.0 + 1.0;
            int hp_status = (int)(int.Parse(characterObj.基本パラメータ_HP) * ((level - 1.0) / 4.0 + 1.0));
            int kou_status = (int)((int.Parse(characterObj.基本パラメータ_攻) + buko + shogo_status.kougeki) * level1 + level2) + soubi_status.kougeki;
            int bou_status = (int)((int.Parse(characterObj.基本パラメータ_防) + buko + shogo_status.bougyo) * level1 + level2) + soubi_status.bougyo;
            int soku_status = (int)((int.Parse(characterObj.基本パラメータ_速) + shogo_status.sokudo) * level1 + level2) + soubi_status.sokudo;
            int chi_status = (int)((int.Parse(characterObj.基本パラメータ_知) + shogo_status.chiryoku) * level1 + level2) + soubi_status.chiryoku;
            Debug.WriteLine(shogo_status.hp + " " + shogo_status.kougeki + " " + shogo_status.bougyo + " " + shogo_status.sokudo + " " + shogo_status.chiryoku);
            Debug.WriteLine(hp_status + " " + kou_status + " " + bou_status + " " + soku_status + " " + chi_status);

            if (chi_status < 1) chi_status = 1;
            if (soku_status < 1) soku_status = 1;
            if (kou_status < 1) kou_status = 1;
            if (bou_status < 1) bou_status = 1;
            temp_status.hp = hp_status;
            temp_status.sokudo = soku_status;
            temp_status.chiryoku = chi_status;
            temp_status.kougeki = kou_status;
            temp_status.bougyo = bou_status;
            //return ;
        }


        //師団スキルの種類を返す。
        //0 : なし
        //1 : 範囲無効系
        //2 : 結界系
        //3 : 嘘関係
        //4 : 運命値
        //5 : 活性系
        //6 : 指揮系
        //7 : 弱体系
        //8 : 回復系
        //9 : 継続攻撃系
        //10 : 砲撃系
        //11 : その他
        Dictionary<string, int> hanni_skills_dict = new Dictionary<string, int>
        {
            ["側面無効"] = 1,
            ["遠隔無効"] = 1,
            ["貫通無効"] = 1,
            ["扇形無効"] = 1,
            ["十字無効"] = 1,
            ["全域無効"] = 1,
            ["範囲無効"] = 1
        };
        Dictionary<string, int> kekkai_skills_dict = new Dictionary<string, int>
        {
            ["自爆結界"] = 1,
            ["四法結界"] = 1,
            ["砲撃結界"] = 1,
            ["対術結界"] = 1,
            ["戦術結界"] = 1,
            ["術式増幅"] = 1,
            ["解毒抗体"] = 1,
            ["酸化抗体"] = 1,
            ["聖戦の導"] = 1,
            ["凍傷気流"] = 1,
            ["竜王守護"] = 1,
        };
        Dictionary<string, int> uso_skills_dict = new Dictionary<string, int>
        {
            ["愚者の嘘"] = 1,
            ["地形無効"] = 1,
            ["正々堂々"] = 1,
            ["決戦領域"] = 1,
            ["罠の領域"] = 1,
        };
        Dictionary<string, int> unmei_skills_dict = new Dictionary<string, int>
        {
            ["運命の輪"] = 1
        };
        Dictionary<string, int> kassei_skills_dict = new Dictionary<string, int>
        {
            ["男性活性"] = 101,
            ["女性活性"] = 102,
            ["人間活性"] = 103,
            ["魔族活性"] = 104,
            ["神族活性"] = 105,
            ["獣族活性"] = 106,
            ["陸生活性"] = 107,
            ["樹霊活性"] = 108,
            ["海洋活性"] = 109,
            ["竜族活性"] = 110,
            ["器兵活性"] = 111,
            ["死者活性"] = 112,
            ["蟲族活性"] = 113,
            ["炎霊活性"] = 114,
            ["雷霊活性"] = 115,
            ["氷霊活性"] = 116,
            ["毒性活性"] = 117,
            ["飛行活性"] = 118,
            ["騎士活性"] = 119,
            ["夜行活性"] = 120,
            ["超越活性"] = 121,
            ["師団活性"] = 122,
            ["軍団活性"] = 123,
            ["火属活性"] = 124,
            ["水属活性"] = 125,
            ["風属活性"] = 126,
            ["土属活性"] = 127,
            ["光属活性"] = 128,
            ["闇属活性"] = 129,
            ["血の起源"] = 129,

            ["攻撃布陣"] = 130,
            ["防御布陣"] = 131,
            ["速度布陣"] = 132,
            ["知力布陣"] = 133,
            ["武具研磨"] = 134,
            ["英雄覇気"] = 38

        };

        Dictionary<string, int> shihai_skills_dict = new Dictionary<string, int>
        {
            ["男性支配"] = 1,
            ["女性支配"] = 2,
            ["人間支配"] = 3,
            ["魔族支配"] = 4,
            ["神族支配"] = 5,
            ["獣族支配"] = 6,
            ["陸生支配"] = 7,
            ["樹霊支配"] = 8,
            ["海洋支配"] = 9,
            ["竜族支配"] = 10,
            ["器兵支配"] = 11,
            ["死者支配"] = 12,
            ["蟲族支配"] = 13,
            ["炎霊支配"] = 14,
            ["雷霊支配"] = 15,
            ["氷霊支配"] = 16,
            ["毒性支配"] = 17,
            ["飛行支配"] = 18,
            ["騎士支配"] = 19,
            ["夜行支配"] = 20,
            ["超越支配"] = 21,
            ["師団支配"] = 22,
            ["軍団支配"] = 23,
            ["火属支配"] = 24,
            ["水属支配"] = 25,
            ["風属支配"] = 26,
            ["土属支配"] = 27,
            ["光属支配"] = 28,
            ["闇属支配"] = 29,
            ["攻撃支配"] = 30,
            ["防御支配"] = 31,
            ["速度支配"] = 32,
            ["知力支配"] = 33,
            ["死の軍勢"] = 34
        };

        Dictionary<string, int> shiki_skills_dict = new Dictionary<string, int>
        {
            ["男性指揮"] = 1,
            ["女性指揮"] = 2,
            ["人間指揮"] = 3,
            ["魔族指揮"] = 4,
            ["神族指揮"] = 5,
            ["獣族指揮"] = 6,
            ["陸生指揮"] = 7,
            ["樹霊指揮"] = 8,
            ["海洋指揮"] = 9,
            ["竜族指揮"] = 10,
            ["器兵指揮"] = 11,
            ["死者指揮"] = 12,
            ["蟲族指揮"] = 13,
            ["炎霊指揮"] = 14,
            ["雷霊指揮"] = 15,
            ["氷霊指揮"] = 16,
            ["毒性指揮"] = 17,
            ["飛行指揮"] = 18,
            ["騎士指揮"] = 19,
            ["夜行指揮"] = 20,
            ["超越指揮"] = 21,
            ["師団指揮"] = 22,
            ["軍団指揮"] = 23,
            ["火属指揮"] = 24,
            ["水属指揮"] = 25,
            ["風属指揮"] = 26,
            ["土属指揮"] = 27,
            ["光属指揮"] = 28,
            ["闇属指揮"] = 29,

            ["攻撃指揮"] = 30,
            ["防御指揮"] = 31,
            ["速度指揮"] = 32,
            ["知力指揮"] = 33,

            ["攻勢転化"] = 34,
            ["守勢転化"] = 35,
            ["速勢転化"] = 36,
            ["知勢転化"] = 37,
            ["武具研磨"] = 134,
            ["英雄覇気"] = 38
        };
        Dictionary<string, int> jakutai_skills_dict = new Dictionary<string, int>
        {
            ["男性弱体"] = 1,
            ["女性弱体"] = 1,
            ["人間弱体"] = 1,
            ["魔族弱体"] = 1,
            ["神族弱体"] = 1,
            ["獣族弱体"] = 1,
            ["陸生弱体"] = 1,
            ["樹霊弱体"] = 1,
            ["海洋弱体"] = 1,
            ["竜族弱体"] = 1,
            ["器兵弱体"] = 1,
            ["死者弱体"] = 1,
            ["蟲族弱体"] = 1,
            ["炎霊弱体"] = 1,
            ["雷霊弱体"] = 1,
            ["氷霊弱体"] = 1,
            ["毒性弱体"] = 1,
            ["飛行弱体"] = 1,
            ["騎士弱体"] = 1,
            ["夜行弱体"] = 1,
            ["超越弱体"] = 1,
            ["師団弱体"] = 1,
            ["軍団弱体"] = 1,
            ["火属弱体"] = 1,
            ["水属弱体"] = 1,
            ["風属弱体"] = 1,
            ["土属弱体"] = 1,
            ["光属弱体"] = 1,
            ["闇属弱体"] = 1,
            ["攻撃弱体"] = 1,
            ["防御弱体"] = 1,
            ["速度弱体"] = 1,
            ["知力弱体"] = 1,
        };
        Dictionary<string, int> treatment_skills_dict = new Dictionary<string, int>
        {
            ["解毒治療"] = 1,
            ["解呪治療"] = 1,
            ["麻痺治療"] = 1,
            ["削減治療"] = 1,
            ["絶対治療"] = 1
        };

        Dictionary<string, int> sonota_skills_dict = new Dictionary<string, int>
        {
            ["城壁崩し"] = 1,
            ["城壁構築"] = 1,
            ["戦術妨害"] = 1,
            ["戦術補助"] = 1,
            ["戦意高揚"] = 1,
            ["戦術強撃"] = 1,
            ["バリアー"] = 1,
            ["奇襲戦法"] = 1,
            ["紀州警戒"] = 1,
            ["行動増加"] = 1,
            ["行動阻害"] = 1,
            ["夜戦赦免"] = 1,
            ["日中赦免"] = 1,
            ["危機管理"] = 1,
            ["資源工面"] = 1,
            ["トレハン"] = 1,
            ["先陣の誉"] = 1,
        };
        Dictionary<string, int> kaifuku_skills_dict = new Dictionary<string, int>{
            { "対象治癒", 1 },
            { "全体治癒", 1 },
            { "平等治癒", 1 },
            { "魔族医療", 1 },
            { "軍団治癒", 1 },
            { "回帰治癒", 1 },
            { "グルメ魂", 1 }
        };

        Dictionary<string, int> hougeki_skills_dict = new Dictionary<string, int>{
            { "火炎砲弾", 1 },
            { "水流砲弾", 1 },
            { "氷撃砲弾", 1 },
            { "雷撃砲弾", 1 },
            { "毒気砲弾", 1 },
            { "神術砲弾", 1 },
            { "魔術砲弾", 1 },
            { "強酸砲弾", 1 },
            { "超火炎砲", 1 },
            { "超水流砲", 1 },
            { "超氷撃砲", 1 },
            { "超雷撃砲", 1 },
            { "超毒気砲", 1 },
            { "超神術砲", 1 },
            { "超魔術砲", 1 },
            { "超強酸砲", 1 },
            { "竜の吐息", 1 }
        };
      Dictionary<string, int> jutushiki_skills_dict = new Dictionary<string, int>{
            { "火炎放射", 1 },
            { "水流放射", 1 },
            { "氷撃放射", 1 },
            { "雷撃放射", 1 },
            { "毒気放射", 1 },
            { "神術放射", 1 },
            { "魔術放射", 1 },
            { "強酸放射", 1 },
            { "大火炎陣", 1 },
            { "大水流陣", 1 },
            { "大氷撃陣", 1 },
            { "大雷撃陣", 1 },
            { "大毒気陣", 1 },
            { "大神術陣", 1 },
            { "大魔術陣", 1 },
            { "大強酸陣", 1 },
            { "超火炎陣", 1 },
            { "超水流陣", 1 },
            { "超氷撃陣", 1 },
            { "超雷撃陣", 1 },
            { "超毒気陣", 1 },
            { "超神術陣", 1 },
            { "超魔術陣", 1 },
            { "超強酸陣", 1 },
            { "竜の吐息", 1 }
        };

    public int check_shidan_skill_type(string skillname)
        {
            //師団スキルをチェックする
            if (hanni_skills_dict.ContainsKey(skillname))
                return 1;
            else if (kekkai_skills_dict.ContainsKey(skillname))
                return 2;
            else if (treatment_skills_dict.ContainsKey(skillname))
                return 3;
            else if (uso_skills_dict.ContainsKey(skillname))
                return 4;
            else if (unmei_skills_dict.ContainsKey(skillname))
                return 5;
            else if (kassei_skills_dict.ContainsKey(skillname) || shihai_skills_dict.ContainsKey(skillname))
                return 6;
            else if (shiki_skills_dict.ContainsKey(skillname))
                return 7;
            else if (jakutai_skills_dict.ContainsKey(skillname))
                return 8;
            else if (kaifuku_skills_dict.ContainsKey(skillname))
                return 9;
            else if (jutushiki_skills_dict.ContainsKey(skillname))
                return 10;
            else if (hougeki_skills_dict.ContainsKey(skillname)) 
                return 11;
            else if (sonota_skills_dict.ContainsKey(skillname))
                return 12;
            else return 0;

        }

        //師団スキルの種類を返す。
        //0 : なし
        //1 : 範囲無効系
        //2 : 結界系
        //3 : 絶対治癒など
        //4 : 嘘関係
        //5 : 運命値
        //6 : 活性系
        //7 : 指揮系
        //8 : 弱体系
        //9 : 回復系
        //10 : 継続攻撃系
        //11 : 砲撃系
        //12 : その他

        public void set_shidan_skill()
        {
            //師団スキルを設定する
            //各キャラクターの最終スキルを全て確認し、師団スキルを計算する
            shidan_skill.Clear();
            foreach (var character in all_characters.Values)
            {
                //character.character_skillを書き換えるとまずいのでディープコピー
                Dictionary<string, int> temp_character_skill = new Dictionary<string, int>();
                foreach (var skill in character.character_skill)
                {
                    if(skill.Key == "四法結界")
                    {
                        if (temp_character_skill.ContainsKey("自爆結界"))
                        {
                            int temp_value = temp_character_skill["自爆結界"];
                            temp_character_skill["自爆結界"] = skill.Value+ temp_value;
                        }
                        else
                        {
                            temp_character_skill.Add("自爆結界", skill.Value);
                        }
                        if (temp_character_skill.ContainsKey("戦術結界"))
                        {
                            int temp_value = temp_character_skill["戦術結界"];
                            temp_character_skill["戦術結界"] = skill.Value + temp_value;
                        }
                        else
                        {
                            temp_character_skill.Add("戦術結界", skill.Value);
                        }
                        if (temp_character_skill.ContainsKey("砲撃結界"))
                        {
                            int temp_value = temp_character_skill["砲撃結界"];
                            temp_character_skill["砲撃結界"] = skill.Value + temp_value;
                        }
                        else
                        {
                            temp_character_skill.Add("砲撃結界", skill.Value);
                        }
                        if (temp_character_skill.ContainsKey("対術結界"))
                        {
                            int temp_value = temp_character_skill["対術結界"];
                            temp_character_skill["対術結界"] = skill.Value + temp_value;
                        }
                        else
                        {
                            temp_character_skill.Add("対術結界", skill.Value);
                        }
                    }
                    temp_character_skill.Add(skill.Key,skill.Value);
                }

                foreach (var skill in temp_character_skill)
                {
                    int value = check_shidan_skill_type(skill.Key);
                    if (value == 1)
                    {
                        shidan_skill[skill.Key] = (skill.Value, value);
                    }
                    else if (value == 2)
                    {
                        if (!shidan_skill.ContainsKey(skill.Key))
                            shidan_skill[skill.Key] = (skill.Value, value);
                        else
                        {
                            if (skill.Value < 100 && shidan_skill[skill.Key].Item1 <100)
                            {
                                //和ではなく積算
                                double temp_value = (100.0 - shidan_skill[skill.Key].Item1) / 100.0;
                                double temp_value2 = (shidan_skill[skill.Key].Item1 + (temp_value * skill.Value));
                                shidan_skill[skill.Key] = ((int)temp_value2, value);
                            }
                            else
                            {
                                if (shidan_skill[skill.Key].Item1 < skill.Value)
                                {
                                    shidan_skill[skill.Key] = (skill.Value, value);
                                }
                            }
                        }
                    }
                    //嘘のみ最大値を設定する
                    else if (value == 4)
                    {
                        if (skill.Key == "愚者の嘘")
                        {
                            if (!shidan_skill.ContainsKey(skill.Key))
                                shidan_skill[skill.Key] = (skill.Value, value);
                            else
                            {
                                if (shidan_skill[skill.Key].Item1 < skill.Value)
                                    shidan_skill[skill.Key] = (skill.Value, value);
                            }
                        }
                        else
                        {
                            shidan_skill[skill.Key] = (skill.Value, value);
                        }
                    }
                    else if (value == 5)
                    {
                        //運命値は加算
                        if (!shidan_skill.ContainsKey(skill.Key))
                            shidan_skill[skill.Key] = (skill.Value, value);
                        else shidan_skill[skill.Key] = (shidan_skill[skill.Key].Item1 + skill.Value, value);
                    }
                    else if (value == 6)
                    {
                        if (!shidan_skill.ContainsKey(skill.Key))
                            shidan_skill[skill.Key] = (skill.Value, value);
                        else shidan_skill[skill.Key] = (shidan_skill[skill.Key].Item1 + skill.Value, value);
                    }
                    //指揮系は加算する
                    else if (value == 7)
                    {
                        if (!shidan_skill.ContainsKey(skill.Key))
                            shidan_skill[skill.Key] = (skill.Value, value);
                        else shidan_skill[skill.Key] = (shidan_skill[skill.Key].Item1 + skill.Value, value);
                    }
                    //弱体系は加算する
                    else if (value == 12)
                    {
                        if (!shidan_skill.ContainsKey(skill.Key))
                            shidan_skill[skill.Key] = (skill.Value, value);
                        else shidan_skill[skill.Key] = (shidan_skill[skill.Key].Item1 + skill.Value, value);
                    }
                    //その他のほうが優先度高い
                    else if (value == 3)
                    {
                        if (!shidan_skill.ContainsKey(skill.Key))
                            shidan_skill[skill.Key] = (skill.Value, value);
                        else shidan_skill[skill.Key] = (shidan_skill[skill.Key].Item1 + skill.Value, value);
                    }
                    //弱体
                    else if (value == 8)
                    {
                        if (!shidan_skill.ContainsKey(skill.Key))
                            shidan_skill[skill.Key] = (skill.Value, value);
                        else shidan_skill[skill.Key] = (shidan_skill[skill.Key].Item1 + skill.Value, value);
                    }
                    //回復
                    else if (value == 9)
                    {
                        if (skill.Key == "回帰治癒")
                        {
                            if (!shidan_skill.ContainsKey(skill.Key))
                                shidan_skill[skill.Key] = (skill.Value, value);
                            else
                            {
                                if (shidan_skill[skill.Key].Item1 < skill.Value)
                                    shidan_skill[skill.Key] = (skill.Value, value);
                            }
                        }
                        if (!shidan_skill.ContainsKey(skill.Key))
                            shidan_skill[skill.Key] = (skill.Value, value);
                        else shidan_skill[skill.Key] = (shidan_skill[skill.Key].Item1 + skill.Value, value);
                    }
                    else if (value == 10)
                    {
                        if (!shidan_skill.ContainsKey(skill.Key))
                            shidan_skill[skill.Key] = (skill.Value, value);
                        else shidan_skill[skill.Key] = (shidan_skill[skill.Key].Item1 + skill.Value, value);
                    }
                    else if (value == 11)
                    {
                        if (!shidan_skill.ContainsKey(skill.Key))
                            shidan_skill[skill.Key] = (skill.Value, value);
                        else shidan_skill[skill.Key] = (shidan_skill[skill.Key].Item1 + skill.Value, value);
                    }
                }
            }
            var sortedAsc = shidan_skill
            .OrderBy(kv => kv.Value.Item2)       // Valueを昇順にソート
            .ToDictionary(kv => kv.Key, kv => kv.Value);
            shidan_skill = sortedAsc;
            foreach (var skill in shidan_skill)
            {
                if (skill.Key != "四法結界")
                {
                    shidan_skill_box.Text += skill.Key;
                    if (skill.Value.Item1 != 0)
                        shidan_skill_box.Text += ":" + skill.Value.Item1;
                    shidan_skill_box.Text += "\n";
                }
            }
        }

        private void Shidan_kakikomi_Button_Click(object sender, RoutedEventArgs e)
        {
            shidan_savedata tempdata;
            tempdata = new shidan_savedata();
            tempdata.shidan_name = shidan_name_box.Text;
            //もしnullなら”師団_年月日時分秒”
            if (tempdata.shidan_name == "")
            {
                tempdata.shidan_name = "師団_" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            }
            // 師団のメンバーを保存する
            tempdata.character = new Dictionary<int, character_info>();
            int temp_leader = 0;
            foreach (var kv in all_characters)
            {
                if (kv.Value.leader_flag == true)
                {
                    del_leader_skill(kv.Key);
                    kv.Value.leader_flag = true;
                    temp_leader = kv.Key;
                }
                Debug.WriteLine(kv.Key + " leaderflag:"  + kv.Value.leader_flag);
            }
            tempdata.character = all_characters.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.DeepCopy());
            all_shidan_savedata.Add(tempdata);
            add_leader_skill(temp_leader);
        }
        private void shidan_yomikomi_Button_Click(object sender, RoutedEventArgs e)
        {
            //all_shidan_savedataから選択されたデータを読み込む
            var selected = shidan_saved_list.SelectedItem as shidan_savedata;
            if (selected != null)
            {
                // selected.character は Dictionary<int, character_info> なので、これをループする
                //セーブされているリーダーフラグがあるか確認。もし変わっていたら戻す
                all_characters.Clear();
                for (int i = 1; i < 7; i++)
                {
                    var nameBox = (TextBox)this.FindName($"character{i}_name_box");
                    if (nameBox != null) nameBox.Text = "";
                    var skillBox = (TextBox)this.FindName($"character{i}_skill_box");
                    if (skillBox != null) skillBox.Text = "";
                    var statusBox = (TextBox)this.FindName($"character{i}_status_box");
                    if (statusBox != null) statusBox.Text = "";
                    var leader_flag = (CheckBox)this.FindName($"leader{i}_flag");
                    leader_flag.IsChecked = false;
                }
                shidan_name_box.Text = selected.shidan_name;

                shidan_assist.Clear();
                all_characters = selected.character.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.DeepCopy());

                foreach (var kv in selected.character)
                {
                    var character1_namebox = (TextBox)this.FindName($"character{kv.Key}_name_box");
                    var character1_skill_box = (TextBox)this.FindName($"character{kv.Key}_skill_box");
                    var leader_flag = (CheckBox)this.FindName($"leader{kv.Key}_flag");
                    var textBox = (TextBox)this.FindName($"buko{kv.Key}_fig");
                    character1_namebox.Text = kv.Value.character_name;
                    shidan_assist.Add(new ItemSet { Id = kv.Key, Name = all_characters[kv.Key].assist_skill });
                    foreach (var skill in kv.Value.character_skill)
                    {
                        character1_skill_box.Text += skill.Key + ":" + skill.Value + "\n";
                    }
                    if (textBox != null)
                        textBox.Text = all_characters[kv.Key].buko.ToString();
                    else
                    {
                        textBox.Text = "100";
                    }
                    if (all_characters[kv.Key].leader_flag == true)
                    {
                        leader_flag.IsChecked = true;
                    }
                    else
                    {
                        leader_flag.IsChecked = false;
                    }
                }
                shidan_skill_box.Text = "";
                set_shidan_skill();
                character_kassei_update();
                character_shiki_update();
                kago_calc();
            }
        }
        private void shidan_sakujo_Button_Click(object sender, RoutedEventArgs e)
        {
            var selected = shidan_saved_list.SelectedItem as shidan_savedata;
            if (selected != null)
            {
                int removedIndex = shidan_saved_list.SelectedIndex;

                // コレクションから削除
                all_shidan_savedata.Remove(selected);

                // 削除後に選択を復元
                if (all_shidan_savedata.Count == 0)
                {
                    shidan_saved_list.SelectedIndex = -1; // 何も選択しない
                }
                else if (removedIndex < all_shidan_savedata.Count)
                {
                    shidan_saved_list.SelectedIndex = removedIndex; // 同じ位置を選ぶ
                }
                else
                {
                    shidan_saved_list.SelectedIndex = all_shidan_savedata.Count - 1; // 1つ前を選ぶ
                }
            }
        }


        private void shidan_file_kakikomi_Button_Click(object sender, RoutedEventArgs e)
        {
            //現在のall_save_dataをjsonにして保存する
            try
            {
                string json = JsonConvert.SerializeObject(all_shidan_savedata, Formatting.Indented);
                // 保存先のフォルダが無ければ作る
                File.WriteAllText("./json/shidan.json", json);
                MessageBox.Show("保存しました。");
                return; // 成功
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("権限が無いため保存できません: " + ex.Message);
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine("ディレクトリが見つかりません: " + ex.Message);
            }
            catch (IOException ex)
            {
                Console.WriteLine("入出力エラー: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("不明なエラー: " + ex.Message);
            }
        }

        private void add_leader_skill(int number)
        {
            if (!all_characters.ContainsKey(number))
            {
                return;
            }
            character_info character = all_characters[number];
            foreach (var skill in character.leader_skill)
            {
                if (character.character_skill.ContainsKey(skill.Key))
                {
                    if (skill.Value != 0)
                    {
                        character.character_skill[skill.Key] += skill.Value;
                    }
                    else
                    {
                        character.character_skill[skill.Key] = 1;
                    }
                }
                else
                {
                    character.character_skill[skill.Key] = skill.Value;
                }
            }

            var character1_skill_box = (TextBox)this.FindName($"character{number}_skill_box");
            character1_skill_box.Text = "";
            foreach (var skill in character.character_skill)
            {
                character1_skill_box.Text += skill.Key + ":" + skill.Value + "\n";
            }
            character.leader_flag = true;
            shidan_skill_box.Text = "";
            stance_text_box.Text = character.stance;
            set_shidan_skill();
            character_kassei_update();
            character_shiki_update();
            kago_calc();
        }
        private void del_leader_skill(int number)
        {
            if (!all_characters.ContainsKey(number)) return;
            character_info character = all_characters[number];
            string skill_name;
            int skill_value;

            foreach (var skill in character.leader_skill)
            {
                if (character.character_skill.ContainsKey(skill.Key))
                {
                    if (skill.Value != 0)
                    {
                        character.character_skill[skill.Key] -= skill.Value;
                        if (character.character_skill[skill.Key] <= 0)
                            character.character_skill.Remove(skill.Key);
                    }
                    else
                    {
                        if (skill.Value == 0)
                        {
                            character.character_skill.Remove(skill.Key);
                        }
                    }
                }
            }
            character.leader_flag = false;

            var character1_skill_box = (TextBox)this.FindName($"character{number}_skill_box");
            character1_skill_box.Text = "";
            foreach (var skill in character.character_skill)
            {
                character1_skill_box.Text += skill.Key + ":" + skill.Value + "\n";
            }
            shidan_skill_box.Text = "";
            stance_text_box.Text = "";
            set_shidan_skill();
            character_kassei_update();
            character_shiki_update();
            kago_calc();
        }

        private void leader1_flag_Checked(object sender, RoutedEventArgs e)
        {
            var current = sender as CheckBox;
            var parent = (current.Parent as Panel);
            //呼び元チェックボックス名から呼ばれた番号を取得して投げる
            int number = current.Name.Contains("1") ? 1 :
                         current.Name.Contains("2") ? 2 :
                         current.Name.Contains("3") ? 3 :
                         current.Name.Contains("4") ? 4 :
                         current.Name.Contains("5") ? 5 :
                         current.Name.Contains("6") ? 6 : 0;
            foreach (var child in parent.Children)
            {
                if (child is CheckBox cb && cb != current && cb.Name.Contains("leader"))
                {
                    cb.IsChecked = false;
                }
            }
            if (all_characters.ContainsKey(number))
            {
                all_characters[number].leader_flag = true;
                var checkd = (CheckBox)this.FindName($"leader{number}_flag");
                if (checkd != null)
                {
                    checkd.IsChecked = true;
                }
            }
            add_leader_skill(number);
        }
        private void leader1_flag_UnChecked(object sender, RoutedEventArgs e)
        {
            //呼ばれた番号を取得
            var current = sender as CheckBox;
            var parent = (current.Parent as Panel);
            //呼び元チェックボックス名から呼ばれた番号を取得して投げる
            int number = current.Name.Contains("1") ? 1 :
                         current.Name.Contains("2") ? 2 :
                         current.Name.Contains("3") ? 3 :
                         current.Name.Contains("4") ? 4 :
                         current.Name.Contains("5") ? 5 :
                         current.Name.Contains("6") ? 6 : 0;
            if (all_characters.ContainsKey(number))
            {
                all_characters[number].leader_flag = false;
                var checkd = (CheckBox)this.FindName($"leader{number}_flag");
                if (checkd != null)
                {
                    checkd.IsChecked = false;
                }
            }
            del_leader_skill(number);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists("./json/saved.json"))
            {
                string loadedJson = File.ReadAllText("./json/saved.json");
                //all_save_dataをすべて消す
                all_save_data.Clear();
                //all_save_dataに読み込んだjsonを追加する
                all_save_data = JsonConvert.DeserializeObject<ObservableCollection<savedata>>(loadedJson);
                if (all_save_data == null)
                {
                    all_save_data = new ObservableCollection<savedata>();
                }
                saved_list.ItemsSource = all_save_data;
            }
            else
            {
            }

        }
        string chuya = "";
        public string kago = "";
        public string leg_kago = "";
        public static int CountChar(string s, char c)
        {
            return s.Length - s.Replace(c.ToString(), "").Length;
        }
        private double kago_bairitu_calc(string chara_kago)
        {
            double kago_bairitu_calc = 1.0;
            int yuri = 0;
            int furi = 0;
            //chara_kagoの中から単語の出現回数をとる
            if (kago == "火")
            {
                yuri = CountChar(chara_kago, '火');
                furi = CountChar(chara_kago, '水');
            }
            else if (kago == "水")
            {
                yuri = CountChar(chara_kago, '水');
                furi = CountChar(chara_kago, '火');

            }
            else if (kago == "風")
            {
                yuri = CountChar(chara_kago, '風');
                furi = CountChar(chara_kago, '土');
            }
            else if (kago == "土")
            {
                yuri = CountChar(chara_kago, '土');
                furi = CountChar(chara_kago, '風');
            }
            else if (kago == "闇")
            {
                yuri = CountChar(chara_kago, '闇');
                furi = CountChar(chara_kago, '光');
            }
            else if (kago == "光")
            {
                yuri = CountChar(chara_kago, '光');
                furi = CountChar(chara_kago, '闇');
            }
            kago_bairitu_calc = 1.0 + 0.25 * yuri - 0.25 * furi;
            return kago_bairitu_calc;
        }
        int otori_number;
        int otori_bougyo;
        //加護だけじゃなくて昼夜とかスタンスの計算もして加算する
        private void kago_calc()
        {
            otori_number = 0;
            otori_bougyo = 999999999;

            int stance_kou = 0, stance_bougyo = 0, stance_soku = 0, stance_chi = 0;
            (stance_kou, stance_bougyo, stance_soku, stance_chi) = stance_calc();
            //加護の計算
            foreach (var character in all_characters)
            {
                var character1_status_box = (TextBox)this.FindName($"character{character.Key}_status_box");
                string chara_kago = character.Value.character_kago;
                double kago_bairitu = 1.0;
                int chikei_koka = 0;
                double chikei_bairitu = 1.0;
                if (!shidan_skill.ContainsKey("地形無効") && !shidan_skill.ContainsKey("決戦領域") && !shidan_skill.ContainsKey("兵士運搬")) {
                    chikei_koka = tikei_calc(character.Value.character_shuzoku,false);
                    chikei_bairitu = (100.0 + chikei_koka) / 100.0;
                }
                else if ( shidan_skill.ContainsKey("兵士運搬"))
                {
                    chikei_koka = tikei_calc(character.Value.character_shuzoku,true);
                    chikei_bairitu = (100.0 + chikei_koka) / 100.0;
                }
                else
                {
                }

                if (kago != "")
                {
                    kago_bairitu = kago_bairitu_calc(chara_kago);
                }

                double chuya_bairitu = 1.0;
                if (chuya == "昼")
                {
                    if (character.Value.character_shuzoku.Contains("夜"))
                    {
                        foreach (var skill in character.Value.character_skill)
                        {
                            if (skill.Key == "日中適応" || shidan_skill.ContainsKey("日中赦免"))
                            {
                                chuya_bairitu = 1.0;
                            }
                            else
                            {
                                chuya_bairitu = 0.5;
                            }
                        }
                    }
                    else
                    {
                        chuya_bairitu = 1.0;
                    }
                }
                else if (chuya == "夜")
                {
                    if (character.Value.character_shuzoku.Contains("夜"))
                    {
                        chuya_bairitu = 1.0;
                    }
                    else
                    {
                        if (character.Value.character_skill.ContainsKey("夜戦適応") || shidan_skill.ContainsKey("夜戦赦免"))
                        {
                            chuya_bairitu = 1.0;
                        }
                        else
                        {
                            chuya_bairitu = 0.5;
                        }
                    }
                }
                int otori_bougyo_temp = (int)(character.Value.character_current_status["HP"]) * (int)(character.Value.character_current_status["防御"] * kago_bairitu * chuya_bairitu * chikei_bairitu + stance_bougyo);
                if (otori_bougyo_temp < otori_bougyo)
                {
                    otori_number = character.Key;
                    otori_bougyo = otori_bougyo_temp;
                }
                character1_status_box.Text = "HP:" + character.Value.character_status["HP"] + "\n";
                character1_status_box.Text += "攻撃:" + (int)(character.Value.character_current_status["攻撃"] * kago_bairitu * chikei_bairitu + stance_kou) + "(" + character.Value.character_status["攻撃"] + ")" + "\n";
                character1_status_box.Text += "防御:" + (int)(character.Value.character_current_status["防御"] * kago_bairitu * chuya_bairitu * chikei_bairitu + stance_bougyo) + "(" + character.Value.character_status["防御"] + ")" + "\n";
                character1_status_box.Text += "速度:" + (int)(character.Value.character_current_status["速度"] * kago_bairitu * chikei_bairitu + stance_soku) + "(" + character.Value.character_status["速度"] + ")" + "\n";
                character1_status_box.Text += "知力:" + (int)(character.Value.character_current_status["知力"] * kago_bairitu * chikei_bairitu + stance_chi) + "(" + character.Value.character_status["知力"] + ")" + "\n";
                character1_status_box.Text += "称号1:" + character.Value.character_shogo1 + "\n";
                character1_status_box.Text += "称号2:" + character.Value.character_shogo2 + "\n";
                character1_status_box.Text += "装備1:" + character.Value.character_equipment1 + "\n";
                character1_status_box.Text += "装備2:" + character.Value.character_equipment2 + "\n";
                character1_status_box.Text += "糧食:" + character.Value.character_ryoshoku + "\n";
                character1_status_box.Text += "種族:" + character.Value.character_shuzoku + "\n";
                character1_status_box.Text += "加護:" + character.Value.character_kago + "\n";
            }
            if(otori_number != 0)
            {
                for (int i = 1;i < 7; i++)
                {
                    var allbox = (TextBox)this.FindName($"character{i}_name_box");
                    allbox.ClearValue(TextBox.BackgroundProperty);
                }
                var status_box = (TextBox)this.FindName($"character{otori_number}_name_box");
                status_box.Background = new SolidColorBrush(Color.FromRgb(255, 200, 200)); // 薄い赤
            }
        }
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var radio = sender as RadioButton;
            chuya = radio.Content.ToString(); //
            kago_calc();
        }
        string leg_chuya;
        private void Leg_RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var radio = sender as RadioButton;
            leg_chuya = radio.Content.ToString(); //
            leg_kago_calc();
        }

        private void kago_Checked(object sender, RoutedEventArgs e)
        {
            var current = sender as CheckBox;
            var parent = (current.Parent as Panel);
            string number = current.Name.Contains("1") ? "火" :
             current.Name.Contains("2") ? "水" :
             current.Name.Contains("3") ? "風" :
             current.Name.Contains("4") ? "土" :
             current.Name.Contains("5") ? "光" :
             current.Name.Contains("6") ? "闇" : "";

            //呼び元チェックボックス名から呼ばれた番号を取得して投げる
            foreach (var child in parent.Children)
            {
                if (child is CheckBox cb && cb != current && cb.Name.Contains("kago"))
                {
                    cb.IsChecked = false;
                }
            }
            kago = number;
            kago_calc();
            current.IsChecked = true;
        }
        private void kago_UnChecked(object sender, RoutedEventArgs e)
        {
            kago = "";
            var current = sender as CheckBox;
            var parent = (current.Parent as Panel);
            kago_calc();
            current.IsChecked = false;

        }
        private void status_calc_characterInfo(character_info characterObj,int i)
        {
            double keikenti = 0;
            int hp, soku, kou, bou, chiryoku, cost;
            int level = 0;
            string rank = characterObj.rank;
            if (rank == "S")
            {
                keikenti += 1110050;
                level = 150;
            }
            if (rank == "A")
            {
                keikenti += 966050;
                level = 140;
            }
            if (rank == "B")
            {
                keikenti += 832050;
                level = 130;
            }
            if (rank == "C")
            {
                keikenti += 708050;
                level = 120;
            }
            if (rank == "D")
            {
                keikenti += 594050;
                level = 110;
            }
            if (rank == "E")
            {
                keikenti += 490050;
                level = 100;
            }
            int temp_buko = 100;
            var textBox = (TextBox)this.FindName($"buko{i}_fig");
            if (int.TryParse(textBox.Text, out temp_buko))
            {
                if(100<temp_buko) temp_buko = 100;
            }
            else
            {
                temp_buko = 100;
            }
            double buko = (temp_buko - 25.0) / 2.0 / (Math.Sqrt(characterObj.cost));
            double level1 = 1.0 + Math.Sqrt(keikenti) / 1000.0;
            double level2 = Math.Sqrt(keikenti) / 50.0;


            double temp = (level - 1.0) / 4.0 + 1.0;
            int hp_status = (int)(characterObj.base_status.hp * ((level - 1.0) / 4.0 + 1.0));
            int kou_status = (int)((characterObj.base_status.kougeki + buko + characterObj.shogo_status.kougeki) * level1 + level2) + characterObj.soubi_status.kougeki;
            int bou_status = (int)((characterObj.base_status.bougyo + buko + characterObj.shogo_status.bougyo) * level1 + level2) + characterObj.soubi_status.bougyo;
            int soku_status = (int)((characterObj.base_status.sokudo + characterObj.shogo_status.sokudo) * level1 + level2) + characterObj.soubi_status.sokudo;
            int chi_status = (int)((characterObj.base_status.chiryoku + characterObj.shogo_status.chiryoku) * level1 + level2) + characterObj.soubi_status.chiryoku;
            Debug.WriteLine(hp_status + " " + kou_status + " " + bou_status + " " + soku_status + " " + chi_status);

            if (chi_status < 1) chi_status = 1;
            if (soku_status < 1) soku_status = 1;
            if (kou_status < 1) kou_status = 1;
            if (bou_status < 1) bou_status = 1;
            characterObj.character_status["HP"] = hp_status;
            characterObj.character_status["攻撃"] = kou_status;
            characterObj.character_status["防御"] = bou_status;
            characterObj.character_status["速度"] = soku_status;
            characterObj.character_status["知力"] = chi_status;
            characterObj.buko = temp_buko;            
        }

        private void buko_fig_TextChanged(object sender, TextChangedEventArgs e)
        {

            var current = sender as TextBox;
            var parent = (current.Parent as Panel);
            //呼び元チェックボックス名から呼ばれた番号を取得して投げる
            int number = current.Name.Contains("1") ? 1 :
                         current.Name.Contains("2") ? 2 :
                         current.Name.Contains("3") ? 3 :
                         current.Name.Contains("4") ? 4 :
                         current.Name.Contains("5") ? 5 :
                         current.Name.Contains("6") ? 6 : 0;
            int temp_buko = 100;
            var textBox = (TextBox)this.FindName($"buko{number}_fig");

            if (int.TryParse(textBox.Text, out temp_buko))
            {
                if (all_characters.ContainsKey(number))
                {
                    status_calc_characterInfo(all_characters[number],number);
                }
            }
            //set_shidan_skill();
            character_kassei_update();
            character_shiki_update();
            kago_calc();
        }




        private (int,int,int,int) stance_calc()
        {
            int kou = 0, bou = 0, soku = 0, chi = 0;
            if (stance_text_box.Text != "")
            {
                double keikenti_bonus=0.0;
                string stance_name = stance_text_box.Text;
                long keikenti = 0; 
                long.TryParse(total_keikenti_box.Text,out keikenti);
                keikenti_bonus = Math.Pow(keikenti, 1.0 / 4.0);

                if (keikenti_bonus > 100)
                {
                    keikenti_bonus = 100;
                }
                if (stance_name == "進撃") {
                    kou = 3 + (int)keikenti_bonus;
                }
                else if (stance_name == "防備")
                {
                    bou = 3 + (int)keikenti_bonus;
                }
                else if (stance_name == "計略")
                {
                    soku = 4 + (int)keikenti_bonus;
                    chi = 4 + (int)keikenti_bonus;
                }
                else if (stance_name == "乱戦")
                {
                    kou = 2 + (int)keikenti_bonus/2;
                    bou = 2 + (int)keikenti_bonus / 2;
                    soku = 2 + (int)keikenti_bonus / 2;
                    chi = 2 + (int)keikenti_bonus / 2;
                }
                else
                {
                }
            }
            return (kou, bou, soku, chi);
        }
        private int tikei_calc(string shuzoku,bool unpan)
        {
            //hito_boxなどからすべてもってくる
            int hito_value=0,ma_value=0,doku_value=0,kami_value=0,ju_value=0,juu_value=0,utuwa_value=0,yoru_value=0,mushi_value=0;
            int umi_value = 0, ryu_value = 0, honoo_value = 0, koori_value = 0, kaminari_value = 0, hi_value = 0, ki_value = 0, cho_value = 0;

            int bairitu = 0;
            //人から超boxまでの値を読み込む

            int.TryParse(hito_box.Text,out hito_value);
            int.TryParse(ma_box.Text,out ma_value);
            int.TryParse(doku_box.Text, out doku_value);
            int.TryParse(kami_box.Text, out kami_value);
            int.TryParse(mushi_box.Text, out mushi_value);
            int.TryParse(ju_box.Text, out ju_value);
            int.TryParse(juu_box.Text, out juu_value);
            int.TryParse(umi_box.Text, out umi_value);
            int.TryParse(utuwa_box.Text, out utuwa_value);
            int.TryParse(ryu_box.Text, out ryu_value);
            int.TryParse(honoo_box.Text, out honoo_value);
            int.TryParse(koori_box.Text, out koori_value);
            int.TryParse(kaminari_box.Text, out kaminari_value);
            int.TryParse(hi_box.Text, out hi_value);
            int.TryParse(ki_box.Text, out ki_value);
            int.TryParse(yoru_box.Text, out yoru_value);

            var keywords = new Dictionary<string, Action>{
                { "人", () =>{
                    if (unpan == false || hito_value > 0) 
                        bairitu += hito_value;
                    }
                },
                { "魔", () =>{
                    if (unpan == false || ma_value > 0)
                        bairitu += ma_value;
                    }
                },
                { "毒", () =>{
                    if (unpan == false || doku_value > 0)
                        bairitu += doku_value;
                    }
                },
                { "雷", () =>{
                    if (unpan == false || kaminari_value > 0)
                        bairitu += kaminari_value;
                    }
                },
                { "樹", () =>{
                    if (unpan == false || ju_value > 0)
                        bairitu += ju_value;
                    }
                },
                { "獣", () =>{
                    if (unpan == false || juu_value > 0)
                        bairitu += juu_value;
                    }
                },
                { "海", () =>{
                    if (unpan == false || umi_value > 0)
                        bairitu += umi_value;
                    }
                },
                { "器", () =>{
                    if (unpan == false || utuwa_value > 0)
                        bairitu += utuwa_value;
                    }
                },
                { "蟲", () =>{
                    if (unpan == false || mushi_value > 0)
                        bairitu += mushi_value;
                    }
                },
                { "竜", () =>{
                    if (unpan == false || ryu_value > 0)
                        bairitu += ryu_value;
                    }
                },
                { "炎", () =>{
                    if (unpan == false || honoo_value > 0)
                        bairitu += honoo_value;
                    }
                },
                { "氷", () =>{
                    if (unpan == false || koori_value > 0)
                        bairitu += koori_value;
                    }
                },
                { "神", () =>{
                    if (unpan == false || kami_value > 0)
                        bairitu += kami_value;
                    }
                },
                { "飛", () =>{
                    if (unpan == false || hi_value > 0)
                        bairitu += hi_value;
                    }
                },
                { "騎", () =>{
                    if (unpan == false || ki_value > 0)
                        bairitu += ki_value;
                    }
                },
                { "夜", () =>{
                    if (unpan == false || yoru_value > 0)
                        bairitu += yoru_value;
                    }
                },
            };
            foreach (var kvp in keywords)
            {
                if (shuzoku.Contains(kvp.Key))
                {
                    kvp.Value.Invoke(); // 対応する処理を実行
                }
            }
            return bairitu;
        }

        private void total_keikenti_box_TextChanged(object sender, TextChangedEventArgs e)
        {
            kago_calc();
        }

        private void tikei_box_TextChanged(object sender, TextChangedEventArgs e)
        {
            kago_calc();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            all_characters.Clear();
            for (int number = 1; number < 7; number++)
            {
                var character1_status_box = (TextBox)this.FindName($"character{number}_status_box");
                var namebox = (TextBox)this.FindName($"character{number}_name_box");
                var skillbox = (TextBox)this.FindName($"character{number}_skill_box");
                character1_status_box.Text = "";
                skillbox.Text = "";
                namebox.Text = "";
            }
        }
        private void SmallBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            TextBox clickedBox = sender as TextBox;
            if (clickedBox == null) return;
            // クリックされたTextBoxの内容を大きいPopupにコピー
            BigTextBox3.Text = clickedBox.Text;
            // トグルで開閉
            BigPopup3.IsOpen = !BigPopup3.IsOpen;
            
            /*
            TextBox clickedBox = sender as TextBox;
            if (clickedBox == null) return;

            // 小さい TextBox の内容を Popup にコピー
            BigTextBox.Text = clickedBox.Text;

            // テキスト量に応じて高さを調整
            BigTextBox.Measure(new Size(BigTextBox.MaxWidth, double.PositiveInfinity));
            BigTextBox.Height = BigTextBox.DesiredSize.Height;

            // Popup をトグルで表示
            BigPopup.IsOpen = !BigPopup.IsOpen;
            */
        }

        public void leg_set_shidan_skill(Dictionary<int,character_info> leg_characters,int leg_number)
        {
            //師団スキルを設定する
            //各キャラクターの最終スキルを全て確認し、師団スキルを計算する
            Dictionary<string, (int, int)> leg_shidan_skill;
            leg_shidan_skill = null;
            if (leg_number == 1) {
                leg_shidan_skill = leg_shidan1_skill;
            }
            if (leg_number == 2)
            {
                leg_shidan_skill = leg_shidan2_skill;
            }
            if(leg_number == 3)
            {
                leg_shidan_skill = leg_shidan3_skill;
            }
            leg_shidan_skill.Clear();
            foreach (var character in leg_characters.Values)
            {
                //character.character_skillを書き換えるとまずいのでディープコピー
                Dictionary<string, int> temp_character_skill = new Dictionary<string, int>();
                foreach (var skill in character.character_skill)
                {
                    if (skill.Key == "四法結界")
                    {
                        if (temp_character_skill.ContainsKey("自爆結界"))
                        {
                            int temp_value = temp_character_skill["自爆結界"];
                            temp_character_skill["自爆結界"] = skill.Value + temp_value;
                        }
                        else
                        {
                            temp_character_skill.Add("自爆結界", skill.Value);
                        }
                        if (temp_character_skill.ContainsKey("戦術結界"))
                        {
                            int temp_value = temp_character_skill["戦術結界"];
                            temp_character_skill["戦術結界"] = skill.Value + temp_value;
                        }
                        else
                        {
                            temp_character_skill.Add("戦術結界", skill.Value);
                        }
                        if (temp_character_skill.ContainsKey("砲撃結界"))
                        {
                            int temp_value = temp_character_skill["砲撃結界"];
                            temp_character_skill["砲撃結界"] = skill.Value + temp_value;
                        }
                        else
                        {
                            temp_character_skill.Add("砲撃結界", skill.Value);
                        }
                        if (temp_character_skill.ContainsKey("対術結界"))
                        {
                            int temp_value = temp_character_skill["対術結界"];
                            temp_character_skill["対術結界"] = skill.Value + temp_value;
                        }
                        else
                        {
                            temp_character_skill.Add("対術結界", skill.Value);
                        }
                    }
                    temp_character_skill.Add(skill.Key, skill.Value);
                }

                foreach (var skill in temp_character_skill)
                {
                    int value = check_shidan_skill_type(skill.Key);
                    if (value == 1)
                    {
                        leg_shidan_skill[skill.Key] = (skill.Value, value);
                    }
                    else if (value == 2)
                    {
                        if (!leg_shidan_skill.ContainsKey(skill.Key))
                            leg_shidan_skill[skill.Key] = (skill.Value, value);
                        else
                        {
                            if (skill.Value < 100 && leg_shidan_skill[skill.Key].Item1 < 100)
                            {
                                //和ではなく積算
                                double temp_value = (100.0 - leg_shidan_skill[skill.Key].Item1) / 100.0;
                                double temp_value2 = (leg_shidan_skill[skill.Key].Item1 + (temp_value * skill.Value));
                                leg_shidan_skill[skill.Key] = ((int)temp_value2, value);
                            }
                            else
                            {
                                if (leg_shidan_skill[skill.Key].Item1 < skill.Value)
                                {
                                    leg_shidan_skill[skill.Key] = (skill.Value, value);
                                }
                            }
                        }
                    }
                    //嘘のみ最大値を設定する
                    else if (value == 4)
                    {
                        if (skill.Key == "愚者の嘘")
                        {
                            if (!leg_shidan_skill.ContainsKey(skill.Key))
                                leg_shidan_skill[skill.Key] = (skill.Value, value);
                            else
                            {
                                if (leg_shidan_skill[skill.Key].Item1 < skill.Value)
                                    leg_shidan_skill[skill.Key] = (skill.Value, value);
                            }
                        }
                        else
                        {
                            leg_shidan_skill[skill.Key] = (skill.Value, value);
                        }
                    }
                    else if (value == 5)
                    {
                        //運命値は加算
                        if (!leg_shidan_skill.ContainsKey(skill.Key))
                            leg_shidan_skill[skill.Key] = (skill.Value, value);
                        else leg_shidan_skill[skill.Key] = (leg_shidan_skill[skill.Key].Item1 + skill.Value, value);
                    }
                    else if (value == 6)
                    {
                        if (!leg_shidan_skill.ContainsKey(skill.Key))
                            leg_shidan_skill[skill.Key] = (skill.Value, value);
                        else leg_shidan_skill[skill.Key] = (leg_shidan_skill[skill.Key].Item1 + skill.Value, value);
                    }
                    //指揮系は加算する
                    else if (value == 7)
                    {
                        if (!leg_shidan_skill.ContainsKey(skill.Key))
                            leg_shidan_skill[skill.Key] = (skill.Value, value);
                        else leg_shidan_skill[skill.Key] = (leg_shidan_skill[skill.Key].Item1 + skill.Value, value);
                    }
                    //弱体系は加算する
                    else if (value == 12)
                    {
                        if (!leg_shidan_skill.ContainsKey(skill.Key))
                            leg_shidan_skill[skill.Key] = (skill.Value, value);
                        else leg_shidan_skill[skill.Key] = (leg_shidan_skill[skill.Key].Item1 + skill.Value, value);
                    }
                    //その他のほうが優先度高い
                    else if (value == 3)
                    {
                        if (!leg_shidan_skill.ContainsKey(skill.Key))
                            leg_shidan_skill[skill.Key] = (skill.Value, value);
                        else leg_shidan_skill[skill.Key] = (leg_shidan_skill[skill.Key].Item1 + skill.Value, value);
                    }
                    //弱体
                    else if (value == 8)
                    {
                        if (!leg_shidan_skill.ContainsKey(skill.Key))
                            leg_shidan_skill[skill.Key] = (skill.Value, value);
                        else leg_shidan_skill[skill.Key] = (leg_shidan_skill[skill.Key].Item1 + skill.Value, value);
                    }
                    //回復
                    else if (value == 9)
                    {
                        if (skill.Key == "回帰治癒")
                        {
                            if (!leg_shidan_skill.ContainsKey(skill.Key))
                                leg_shidan_skill[skill.Key] = (skill.Value, value);
                            else
                            {
                                if (leg_shidan_skill[skill.Key].Item1 < skill.Value)
                                    leg_shidan_skill[skill.Key] = (skill.Value, value);
                            }
                        }
                        if (!leg_shidan_skill.ContainsKey(skill.Key))
                            leg_shidan_skill[skill.Key] = (skill.Value, value);
                        else leg_shidan_skill[skill.Key] = (leg_shidan_skill[skill.Key].Item1 + skill.Value, value);
                    }
                    else if (value == 10)
                    {
                        if (!leg_shidan_skill.ContainsKey(skill.Key))
                            leg_shidan_skill[skill.Key] = (skill.Value, value);
                        else leg_shidan_skill[skill.Key] = (leg_shidan_skill[skill.Key].Item1 + skill.Value, value);
                    }
                    else if (value == 11)
                    {
                        if (!leg_shidan_skill.ContainsKey(skill.Key))
                            leg_shidan_skill[skill.Key] = (skill.Value, value);
                        else leg_shidan_skill[skill.Key] = (leg_shidan_skill[skill.Key].Item1 + skill.Value, value);
                    }
                }
            }
            var sortedAsc = leg_shidan_skill
            .OrderBy(kv => kv.Value.Item2)       // Valueを昇順にソート
            .ToDictionary(kv => kv.Key, kv => kv.Value);
            leg_shidan_skill = sortedAsc;
            var leg_shidan_skillBox = (TextBox) this.FindName($"leg_shidan{leg_number}_skillBox");
            leg_shidan_skillBox.Text = "";
            foreach (var skill in leg_shidan_skill)
            {
                if (skill.Key != "四法結界")
                {
                    leg_shidan_skillBox.Text += skill.Key;
                    if (skill.Value.Item1 != 0)
                        leg_shidan_skillBox.Text += ":" + skill.Value.Item1;
                    leg_shidan_skillBox.Text += "\n";
                }
            }
        }
        private void set_legion_skill()
        {
            legion_skill.Clear();
            foreach (var skill in leg_shidan1_skill)
            {
                if (skill.Key == "軍団活性" || skill.Key == "軍団支配" || skill.Key == "軍団治癒" || skill.Key == "軍団弱体")
                {
                    if (legion_skill.ContainsKey(skill.Key))
                    {
                        legion_skill[skill.Key] = (legion_skill[skill.Key].Item1 + skill.Value.Item1, check_shidan_skill_type(skill.Key));
                    }
                    else
                    {
                        legion_skill[skill.Key] = (skill.Value.Item1, check_shidan_skill_type(skill.Key));
                    }
                }
                if (skill.Key == "地形無効" || skill.Key == "兵士運搬" || skill.Key == "決戦領域")
                {
                    legion_skill[skill.Key] = (skill.Value.Item1, check_shidan_skill_type(skill.Key));
                }
            }
            foreach (var skill in leg_shidan2_skill)
            {
                if (skill.Key == "軍団活性" || skill.Key == "軍団支配" || skill.Key == "軍団治癒" || skill.Key == "軍団弱体")
                {
                    if (legion_skill.ContainsKey(skill.Key))
                    {
                        legion_skill[skill.Key] = (legion_skill[skill.Key].Item1 + skill.Value.Item1, check_shidan_skill_type(skill.Key));
                    }
                    else
                    {
                        legion_skill[skill.Key] = (skill.Value.Item1, check_shidan_skill_type(skill.Key));
                    }
                }
                if (skill.Key == "地形無効" || skill.Key == "兵士運搬" || skill.Key == "決戦領域")
                {
                    legion_skill[skill.Key] = (skill.Value.Item1, check_shidan_skill_type(skill.Key));
                }
            }
            foreach (var skill in leg_shidan3_skill)
            {
                if (skill.Key == "軍団活性" || skill.Key == "軍団支配" || skill.Key == "軍団治癒" || skill.Key == "軍団弱体")
                {
                    if (legion_skill.ContainsKey(skill.Key))
                    {
                        legion_skill[skill.Key] = (legion_skill[skill.Key].Item1 + skill.Value.Item1, check_shidan_skill_type(skill.Key));
                    }
                    else
                    {
                        legion_skill[skill.Key] = (skill.Value.Item1, check_shidan_skill_type(skill.Key));
                    }
                }
                if (skill.Key == "地形無効"||skill.Key == "兵士運搬"||skill.Key=="決戦領域")
                {
                    legion_skill[skill.Key] = (skill.Value.Item1, check_shidan_skill_type(skill.Key));
                }
            }
            var sortedAsc = legion_skill
            .OrderBy(kv => kv.Value.Item2)       // Valueを昇順にソート
            .ToDictionary(kv => kv.Key, kv => kv.Value);
            legion_skill = sortedAsc;
            legion_skill_box.Text = "";
            foreach (var tempskill in legion_skill)
            {
                legion_skill_box.Text += tempskill.Key;

                if (tempskill.Value.Item1 != 0)
                    legion_skill_box.Text += ":" + tempskill.Value.Item1;
                legion_skill_box.Text += "\n";
            }
        }
        private (int, int, int, int) leg_shidan_kassei_skill(Dictionary<string, (int, int)> leg_shidan_skill, MainWindow.character_info character, string skillname, int temp_kougeki, int temp_bougyo, int temp_sokudo, int temp_chiryoku)
        {
            int temp_self_kassei = 0;
            if (character.character_skill.ContainsKey(skillname))
                temp_self_kassei = character.character_skill[skillname];
            temp_kougeki += leg_shidan_skill[skillname].Item1 - temp_self_kassei;
            temp_bougyo += leg_shidan_skill[skillname].Item1 - temp_self_kassei;
            temp_sokudo += leg_shidan_skill[skillname].Item1 - temp_self_kassei;
            temp_chiryoku += (leg_shidan_skill[skillname].Item1 - temp_self_kassei) / 4;
            return (temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku);
        }

        private void leg_character_kassei_update(Dictionary<int, character_info> leg_characters, Dictionary<string, (int, int)> leg_shidan_skill)
        {
            int temp_kougeki = 0;
            int temp_bougyo = 0;
            int temp_sokudo = 0;
            int temp_chiryoku = 0;
            //師団スキルにある活性をもとに、キャラクターのステータスを更新する
            int i = 1;
            foreach (var character in leg_characters)
            {

                // 各キャラクターごとに初期化
                temp_kougeki = 0;
                temp_bougyo = 0;
                temp_sokudo = 0;
                temp_chiryoku = 0;
                bool kigen_flag = character.Value.character_skill.ContainsKey("血の起源");
                foreach (var skill in leg_shidan_skill)
                {
                    //活性は5番
                    if (skill.Value.Item2 == 6)
                    {
                        if (skill.Key == "師団活性")
                        {
                            (temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku) = leg_shidan_kassei_skill(leg_shidan_skill, character.Value, "師団活性", temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku);
                        }
                        else if (skill.Key == "血の起源")
                        {
                            (temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku) = leg_shidan_kassei_skill(leg_shidan_skill,character.Value, "血の起源", temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku);
                        }
                        else if (skill.Key == "軍団活性")
                        {
                            (temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku) = leg_shidan_kassei_skill(leg_shidan_skill, character.Value, "軍団活性", temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku);
                        }
                        else if (skill.Key == "師団支配")
                        {
                            (temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku) = leg_shidan_kassei_skill(leg_shidan_skill, character.Value, "師団支配", temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku);
                        }
                        else if (skill.Key == "軍団支配")
                        {
                            (temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku) = leg_shidan_kassei_skill(leg_shidan_skill, character.Value, "軍団支配", temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku);
                        }
                        //布陣があったら
                        else if (skill.Key.Contains("布陣"))
                        {
                            if (skill.Key == "攻撃布陣")
                                temp_kougeki += leg_shidan_skill[skill.Key].Item1;
                            else if (skill.Key == "防御布陣")
                                temp_bougyo += leg_shidan_skill[skill.Key].Item1;
                            else if (skill.Key == "速度布陣")
                                temp_sokudo += leg_shidan_skill[skill.Key].Item1;
                            else if (skill.Key == "知力布陣")
                                temp_chiryoku += (leg_shidan_skill[skill.Key].Item1) / 4;
                        }
                        else if (skill.Key == "武具研磨")
                        {
                            int temp_self_kassei = 0;
                            if (character.Value.character_skill.ContainsKey("武具研磨"))
                                temp_self_kassei = character.Value.character_skill["武具研磨"];

                            temp_kougeki += (int)(((skill.Value.Item1 - temp_self_kassei) / 100.0) * character.Value.soubi_status.kougeki);
                            temp_bougyo += (int)(((skill.Value.Item1 - temp_self_kassei) / 100.0) * character.Value.soubi_status.bougyo);
                            temp_sokudo += (int)(((skill.Value.Item1 - temp_self_kassei) / 100.0) * character.Value.soubi_status.sokudo);
                            temp_sokudo += (int)(((skill.Value.Item1 - temp_self_kassei) / 100.0) * character.Value.soubi_status.chiryoku);
                        }
                        else if (skill.Key == "死の軍勢")
                        {
                            int temp_self_kassei = 0;
                            if (character.Value.character_skill.ContainsKey("死の軍勢"))
                                temp_self_kassei = character.Value.character_skill["死の軍勢"];
                            if (character.Value.character_shuzoku.Contains("死") || kigen_flag)
                            {
                                temp_kougeki += skill.Value.Item1 - temp_self_kassei;
                                temp_bougyo += skill.Value.Item1 - temp_self_kassei;
                                temp_sokudo += skill.Value.Item1 - temp_self_kassei;
                                temp_chiryoku += (skill.Value.Item1 - temp_self_kassei) / 4;
                            }
                        }
                        else
                        {
                            int temp1, temp2, temp3, temp4;
                            (temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku) = calc_kassei(skill.Key, character.Value, skill.Value.Item1, kigen_flag, temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku);
                        }
                    }
                }
                temp_kougeki += character.Value.character_status["攻撃"];
                temp_bougyo += character.Value.character_status["防御"];
                temp_sokudo += character.Value.character_status["速度"];
                temp_chiryoku += character.Value.character_status["知力"];
                var character1_status_box = (TextBox)this.FindName($"character{character.Value.character_id}_status_box");
                character.Value.character_current_status["攻撃"] = temp_kougeki;
                character.Value.character_current_status["防御"] = temp_bougyo;
                character.Value.character_current_status["速度"] = temp_sokudo;
                character.Value.character_current_status["知力"] = temp_chiryoku;
                i++;
            }
        }
        private int leg_tikei_calc(string shuzoku, bool unpan,int number)
        {
            //hito_boxなどからすべてもってくる
            int hito_value = 0, ma_value = 0, doku_value = 0, kami_value = 0, ju_value = 0, juu_value = 0, utuwa_value = 0, yoru_value = 0, mushi_value = 0;
            int umi_value = 0, ryu_value = 0, honoo_value = 0, koori_value = 0, kaminari_value = 0, hi_value = 0, ki_value = 0, cho_value = 0;
            int bairitu = 0;
            //人から超boxまでの値を読み込む

            int.TryParse(hito_box_leg.Text, out hito_value);
            int.TryParse(ma_box_leg.Text, out ma_value);
            int.TryParse(doku_box_leg.Text, out doku_value);
            int.TryParse(kami_box_leg.Text, out kami_value);
            int.TryParse(mushi_box_leg.Text, out mushi_value);
            int.TryParse(ju_box_leg.Text, out ju_value);
            int.TryParse(juu_box_leg.Text, out juu_value);
            int.TryParse(umi_box_leg.Text, out umi_value);
            int.TryParse(utuwa_box_leg.Text, out utuwa_value);
            int.TryParse(ryu_box_leg.Text, out ryu_value);
            int.TryParse(honoo_box_leg.Text, out honoo_value);
            int.TryParse(koori_box_leg.Text, out koori_value);
            int.TryParse(kaminari_box_leg.Text, out kaminari_value);
            int.TryParse(hi_box_leg.Text, out hi_value);
            int.TryParse(ki_box_leg.Text, out ki_value);
            int.TryParse(yoru_box_leg.Text, out yoru_value);

            var keywords = new Dictionary<string, Action>{
                { "人", () =>{
                    if (unpan == false || hito_value > 0)
                        bairitu += hito_value;
                    }
                },
                { "魔", () =>{
                    if (unpan == false || ma_value > 0)
                        bairitu += ma_value;
                    }
                },
                { "毒", () =>{
                    if (unpan == false || doku_value > 0)
                        bairitu += doku_value;
                    }
                },
                { "雷", () =>{
                    if (unpan == false || kaminari_value > 0)
                        bairitu += kaminari_value;
                    }
                },
                { "樹", () =>{
                    if (unpan == false || ju_value > 0)
                        bairitu += ju_value;
                    }
                },
                { "獣", () =>{
                    if (unpan == false || juu_value > 0)
                        bairitu += juu_value;
                    }
                },
                { "海", () =>{
                    if (unpan == false || umi_value > 0)
                        bairitu += umi_value;
                    }
                },
                { "器", () =>{
                    if (unpan == false || utuwa_value > 0)
                        bairitu += utuwa_value;
                    }
                },
                { "蟲", () =>{
                    if (unpan == false || mushi_value > 0)
                        bairitu += mushi_value;
                    }
                },
                { "竜", () =>{
                    if (unpan == false || ryu_value > 0)
                        bairitu += ryu_value;
                    }
                },
                { "炎", () =>{
                    if (unpan == false || honoo_value > 0)
                        bairitu += honoo_value;
                    }
                },
                { "氷", () =>{
                    if (unpan == false || koori_value > 0)
                        bairitu += koori_value;
                    }
                },
                { "神", () =>{
                    if (unpan == false || kami_value > 0)
                        bairitu += kami_value;
                    }
                },
                { "飛", () =>{
                    if (unpan == false || hi_value > 0)
                        bairitu += hi_value;
                    }
                },
                { "騎", () =>{
                    if (unpan == false || ki_value > 0)
                        bairitu += ki_value;
                    }
                },
                { "夜", () =>{
                    if (unpan == false || yoru_value > 0)
                        bairitu += yoru_value;
                    }
                },
            };
            foreach (var kvp in keywords)
            {
                if (shuzoku.Contains(kvp.Key))
                {
                    kvp.Value.Invoke(); // 対応する処理を実行
                }
            }
            return bairitu;
        }
        private double leg_kago_bairitu_calc(string chara_kago)
        {
            double kago_bairitu_calc = 1.0;
            int yuri = 0;
            int furi = 0;
            //chara_kagoの中から単語の出現回数をとる
            if (kago == "火")
            {
                yuri = CountChar(chara_kago, '火');
                furi = CountChar(chara_kago, '水');
            }
            else if (kago == "水")
            {
                yuri = CountChar(chara_kago, '水');
                furi = CountChar(chara_kago, '火');

            }
            else if (kago == "風")
            {
                yuri = CountChar(chara_kago, '風');
                furi = CountChar(chara_kago, '土');
            }
            else if (kago == "土")
            {
                yuri = CountChar(chara_kago, '土');
                furi = CountChar(chara_kago, '風');
            }
            else if (kago == "闇")
            {
                yuri = CountChar(chara_kago, '闇');
                furi = CountChar(chara_kago, '光');
            }
            else if (kago == "光")
            {
                yuri = CountChar(chara_kago, '光');
                furi = CountChar(chara_kago, '闇');
            }
            kago_bairitu_calc = 1.0 + 0.25 * yuri - 0.25 * furi;
            return kago_bairitu_calc;
        }
        private void leg_kago_calc()
        {
            otori_number = 0;
            otori_bougyo = 999999999;

            int stance_kou = 0, stance_bougyo = 0, stance_soku = 0, stance_chi = 0;
            //加護の計算
            var region_character_list = new List<Dictionary<int, character_info>> { leg_shidan1_characters, leg_shidan2_characters, leg_shidan3_characters };
            int number = 1;
            foreach (var characters in region_character_list)
            {
                (stance_kou, stance_bougyo, stance_soku, stance_chi) = leg_stance_calc(number);
                foreach (var character in characters)
                {
                    var character1_status_box = (TextBox)this.FindName($"shidan{number}_chara{character.Key}_status");
                    string chara_kago = character.Value.character_kago;
                    double kago_bairitu = 1.0;
                    int chikei_koka = 0;
                    double chikei_bairitu = 1.0;
                    if (!legion_skill.ContainsKey("地形無効") && !legion_skill.ContainsKey("決戦領域") && !legion_skill.ContainsKey("兵士運搬"))
                    {
                        chikei_koka = leg_tikei_calc(character.Value.character_shuzoku, false, number);
                        chikei_bairitu = (100.0 + chikei_koka) / 100.0;
                    }
                    else if (shidan_skill.ContainsKey("兵士運搬"))
                    {
                        chikei_koka = leg_tikei_calc(character.Value.character_shuzoku, true,number);
                        chikei_bairitu = (100.0 + chikei_koka) / 100.0;
                    }
                    else
                    {
                    }

                    if (kago != "")
                    {
                        kago_bairitu = kago_bairitu_calc(chara_kago);
                    }
                    double chuya_bairitu = 1.0;
                    if (leg_chuya == "昼")
                    {
                        if (character.Value.character_shuzoku.Contains("夜"))
                        {
                            foreach (var skill in character.Value.character_skill)
                            {
                                if (skill.Key == "日中適応" || shidan_skill.ContainsKey("日中赦免"))
                                {
                                    chuya_bairitu = 1.0;
                                }
                                else
                                {
                                    chuya_bairitu = 0.5;
                                }
                            }
                        }
                        else
                        {
                            chuya_bairitu = 1.0;
                        }
                    }
                    else if (leg_chuya == "夜")
                    {
                        if (character.Value.character_shuzoku.Contains("夜"))
                        {
                            chuya_bairitu = 1.0;
                        }
                        else
                        {
                            if (character.Value.character_skill.ContainsKey("夜戦適応") || shidan_skill.ContainsKey("夜戦赦免"))
                            {
                                chuya_bairitu = 1.0;
                            }
                            else
                            {
                                chuya_bairitu = 0.5;
                            }
                        }
                    }
                    int otori_bougyo_temp = (int)(character.Value.character_current_status["HP"]) * (int)(character.Value.character_current_status["防御"] * kago_bairitu * chuya_bairitu * chikei_bairitu + stance_bougyo);
                    if (otori_bougyo_temp < otori_bougyo)
                    {
                        otori_number = character.Key;
                        otori_bougyo = otori_bougyo_temp;
                    }
                    character1_status_box.Text = "HP:" + character.Value.character_status["HP"] + "\n";
                    character1_status_box.Text += "攻撃:" + (int)(character.Value.character_current_status["攻撃"] * kago_bairitu * chikei_bairitu + stance_kou) + "(" + character.Value.character_status["攻撃"] + ")" + "\n";
                    character1_status_box.Text += "防御:" + (int)(character.Value.character_current_status["防御"] * kago_bairitu * chuya_bairitu * chikei_bairitu + stance_bougyo) + "(" + character.Value.character_status["防御"] + ")" + "\n";
                    character1_status_box.Text += "速度:" + (int)(character.Value.character_current_status["速度"] * kago_bairitu * chikei_bairitu + stance_soku) + "(" + character.Value.character_status["速度"] + ")" + "\n";
                    character1_status_box.Text += "知力:" + (int)(character.Value.character_current_status["知力"] * kago_bairitu * chikei_bairitu + stance_chi) + "(" + character.Value.character_status["知力"] + ")" + "\n";
                    character1_status_box.Text += "称号1:" + character.Value.character_shogo1 + "\n";
                    character1_status_box.Text += "称号2:" + character.Value.character_shogo2 + "\n";
                    character1_status_box.Text += "装備1:" + character.Value.character_equipment1 + "\n";
                    character1_status_box.Text += "装備2:" + character.Value.character_equipment2 + "\n";
                    character1_status_box.Text += "糧食:" + character.Value.character_ryoshoku + "\n";
                    character1_status_box.Text += "種族:" + character.Value.character_shuzoku + "\n";
                    character1_status_box.Text += "加護:" + character.Value.character_kago + "\n";
                }
                if (otori_number != 0)
                {
                    for (int i = 1; i < 7; i++)
                    {
                        var allbox = (TextBox)this.FindName($"shidan{number}_chara{i}_name");
                        allbox.ClearValue(TextBox.BackgroundProperty);
                    }
                    var status_box = (TextBox)this.FindName($"shidan{number}_chara{otori_number}_name");
                    status_box.Background = new SolidColorBrush(Color.FromRgb(255, 200, 200)); // 薄い赤
                }
                number++;
            }
        }
        private (double, double, double, double) leg_shidan_shiki_skill(MainWindow.character_info character, string skillname, double temp_kougeki, double temp_bougyo, double temp_sokudo, double temp_chiryoku)
        {
            int temp_self_kassei = 0;
            if (character.character_skill.ContainsKey(skillname))
                temp_self_kassei = character.character_skill[skillname];
            temp_kougeki *= (100.0 + shidan_skill[skillname].Item1 - temp_self_kassei) / 100.0;
            temp_bougyo *= (100.0 + shidan_skill[skillname].Item1 - temp_self_kassei) / 100.0;
            temp_sokudo *= (100.0 + shidan_skill[skillname].Item1 - temp_self_kassei) / 100.0;
            temp_chiryoku *= (400.0 + shidan_skill[skillname].Item1 - temp_self_kassei) / 400.0;
            return (temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku);
        }

        private void leg_character_shiki_update()
        {
            //師団スキルにある活性をもとに、キャラクターのステータスを更新する
            int i = 1;
            var region_character_list = new List<Dictionary<int, character_info>> { leg_shidan1_characters, leg_shidan2_characters, leg_shidan3_characters };
            int number = 1;
            foreach (var characters in region_character_list)
            {
                foreach (var character in characters)
                {
                    // 各キャラクターごとに初期化
                    double temp_kougeki = 1.0;
                    double temp_bougyo = 1.0;
                    double temp_sokudo = 1.0;
                    double temp_chiryoku = 1.0;
                    bool kigen_flag = character.Value.character_skill.ContainsKey("血の起源");
                    foreach (var skill in shidan_skill)
                    {
                        //指揮は6
                        if (skill.Value.Item2 == 7)
                        {
                            if (skill.Key == "師団指揮")
                            {
                                (temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku) = leg_shidan_shiki_skill(character.Value, "師団指揮", temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku);
                            }
                            else if (skill.Key == "軍団指揮")
                            {
                                (temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku) = leg_shidan_shiki_skill(character.Value, "軍団指揮", temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku);
                            }
                            //布陣があったら
                            else if (skill.Key.Contains("攻撃") || skill.Key.Contains("防御") || skill.Key.Contains("速度") || skill.Key.Contains("知力"))
                            {
                                if (skill.Key == "攻撃指揮")
                                    temp_kougeki *= (100.0 + shidan_skill[skill.Key].Item1) / 100.0;
                                else if (skill.Key == "防御指揮")
                                    temp_bougyo *= (100.0 + shidan_skill[skill.Key].Item1) / 100.0;
                                else if (skill.Key == "速度指揮")
                                    temp_sokudo *= (100.0 + shidan_skill[skill.Key].Item1) / 100.0;
                                else if (skill.Key == "知力指揮")
                                    temp_chiryoku *= (100.0 + shidan_skill[skill.Key].Item1) / 100.0;
                            }
                            else
                            {
                                (temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku) = calc_shiki(skill.Key, character.Value, skill.Value.Item1, kigen_flag, temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku);

                            }
                        }
                        if (skill.Key == "英雄覇気")
                        {
                            (temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku) = leg_shidan_shiki_skill(character.Value, "英雄覇気", temp_kougeki, temp_bougyo, temp_sokudo, temp_chiryoku);
                        }
                    }
                    temp_kougeki *= character.Value.character_current_status["攻撃"];
                    temp_bougyo *= character.Value.character_current_status["防御"];
                    temp_sokudo *= character.Value.character_current_status["速度"];
                    temp_chiryoku *= character.Value.character_current_status["知力"];
                    character.Value.character_current_status["攻撃"] = (int)temp_kougeki;
                    character.Value.character_current_status["防御"] = (int)temp_bougyo;
                    character.Value.character_current_status["速度"] = (int)temp_sokudo;
                    character.Value.character_current_status["知力"] = (int)temp_chiryoku;
                    i++;
                }
            }
        }
        private (int, int, int, int) leg_stance_calc(int number)
        {
            int kou = 0, bou = 0, soku = 0, chi = 0;
            var stance = (TextBox)this.FindName($"leg_shidan{number}_stance");

            if (stance.Text != "")
            {
                double keikenti_bonus = 0.0;
                string stance_name = stance_text_box.Text;
                long keikenti = 0;
                long.TryParse(total_keikenti_box.Text, out keikenti);
                keikenti_bonus = Math.Pow(keikenti, 1.0 / 4.0);

                if (keikenti_bonus > 100)
                {
                    keikenti_bonus = 100;
                }
                if (stance_name == "進撃")
                {
                    kou = 3 + (int)keikenti_bonus;
                }
                else if (stance_name == "防備")
                {
                    bou = 3 + (int)keikenti_bonus;
                }
                else if (stance_name == "計略")
                {
                    soku = 4 + (int)keikenti_bonus;
                    chi = 4 + (int)keikenti_bonus;
                }
                else if (stance_name == "乱戦")
                {
                    kou = 2 + (int)keikenti_bonus / 2;
                    bou = 2 + (int)keikenti_bonus / 2;
                    soku = 2 + (int)keikenti_bonus / 2;
                    chi = 2 + (int)keikenti_bonus / 2;
                }
                else
                {
                }
            }
            return (kou, bou, soku, chi);
        }
        private void leg_kago_Checked(object sender, RoutedEventArgs e)
        {
            var current = sender as CheckBox;
            var parent = (current.Parent as Panel);
            string number = current.Name.Contains("1") ? "火" :
             current.Name.Contains("2") ? "水" :
             current.Name.Contains("3") ? "風" :
             current.Name.Contains("4") ? "土" :
             current.Name.Contains("5") ? "光" :
             current.Name.Contains("6") ? "闇" : "";

            //呼び元チェックボックス名から呼ばれた番号を取得して投げる
            foreach (var child in parent.Children)
            {
                if (child is CheckBox cb && cb != current && cb.Name.Contains("kago"))
                {
                    cb.IsChecked = false;
                }
            }
            leg_kago = number;
            leg_kago_calc();
            current.IsChecked = true;
        }
        private void leg_kago_UnChecked(object sender, RoutedEventArgs e)
        {
            leg_kago = "";
            var current = sender as CheckBox;
            var parent = (current.Parent as Panel);
            leg_kago_calc();
            current.IsChecked = false;
        }

        private void leg_load_shidan1_Click(object sender, RoutedEventArgs e)
        {
            var selected = leg_hozon_shidan_list.SelectedItem as shidan_savedata;
            if (selected != null)
            {
                // selected.character は Dictionary<int, character_info> なので、これをループする
                //セーブされているリーダーフラグがあるか確認。もし変わっていたら戻す
                leg_shidan1_characters.Clear();
                leg_shidan1_name.Text = selected.shidan_name.ToString();
                leg_shidan1_characters = selected.character.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.DeepCopy());
                for (int i = 1; i < 7; i++)
                {
                    var skillBox = (TextBox)this.FindName($"shidan1_chara{i}_name");
                    if (leg_shidan1_characters.ContainsKey(i) && leg_shidan1_characters[i] != null)
                    {
                        if (skillBox != null)
                        {
                            skillBox.Text = "";
                            if (leg_shidan1_characters[i].leader_flag == true)
                            {
                                skillBox.Text = "＊";
                                foreach (var skill in leg_shidan1_characters[i].leader_skill)
                                {
                                    if (leg_shidan1_characters[i].character_skill.ContainsKey(skill.Key))
                                    {
                                        if (skill.Value != 0)
                                        {
                                            leg_shidan1_characters[i].character_skill[skill.Key] += skill.Value;
                                        }
                                        else
                                        {
                                            leg_shidan1_characters[i].character_skill[skill.Key] = 1;
                                        }
                                    }
                                    else
                                    {
                                        leg_shidan1_characters[i].character_skill[skill.Key] = skill.Value;
                                    }
                                    leg_shidan1_stance.Text = leg_shidan1_characters[i].stance;
                                }
                            }
                            skillBox.Text += leg_shidan1_characters[i].character_name + "\n";
                            foreach (var kv in leg_shidan1_characters[i].character_skill)
                            {
                                skillBox.Text += kv.Key + ":" + kv.Value + "\n";
                            }
                        }
                        var statusBox = (TextBox)this.FindName($"shidan1_chara{i}_status");
                        statusBox.Text = "HP:" + leg_shidan1_characters[i].character_current_status["HP"].ToString() + "\n";
                        statusBox.Text += "攻撃:" + leg_shidan1_characters[i].character_current_status["攻撃"].ToString() + "\n";
                        statusBox.Text += "防御:" + leg_shidan1_characters[i].character_current_status["防御"].ToString() + "\n";
                        statusBox.Text += "速度:" + leg_shidan1_characters[i].character_current_status["速度"].ToString() + "\n";
                        statusBox.Text += "知力:" + leg_shidan1_characters[i].character_current_status["知力"].ToString() + "\n";
                    }
                }
                leg_set_shidan_skill(leg_shidan1_characters, 1);
                set_legion_skill();
                leg_character_kassei_update(leg_shidan1_characters, leg_shidan1_skill);
                leg_character_shiki_update();
                leg_kago_calc();
            }
        }
        private void leg_load_shidan2_Click(object sender, RoutedEventArgs e)
        {
            var selected = leg_hozon_shidan_list.SelectedItem as shidan_savedata;
            if (selected != null)
            {
                // selected.character は Dictionary<int, character_info> なので、これをループする
                //セーブされているリーダーフラグがあるか確認。もし変わっていたら戻す
                leg_shidan2_characters.Clear();
                leg_shidan2_name.Text = selected.shidan_name.ToString();
                leg_shidan2_characters = selected.character.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.DeepCopy());
                for (int i = 1; i < 7; i++)
                {
                    var skillBox = (TextBox)this.FindName($"shidan2_chara{i}_name");
                    if (leg_shidan2_characters.ContainsKey(i) && leg_shidan2_characters[i] != null)
                    {
                        if (skillBox != null)
                        {
                            skillBox.Text = "";
                            if (leg_shidan2_characters[i].leader_flag == true)
                            {
                                skillBox.Text = "＊";
                                foreach (var skill in leg_shidan2_characters[i].leader_skill)
                                {
                                    if (leg_shidan2_characters[i].character_skill.ContainsKey(skill.Key))
                                    {
                                        if (skill.Value != 0)
                                        {
                                            leg_shidan2_characters[i].character_skill[skill.Key] += skill.Value;
                                        }
                                        else
                                        {
                                            leg_shidan2_characters[i].character_skill[skill.Key] = 1;
                                        }
                                    }
                                    else
                                    {
                                        leg_shidan2_characters[i].character_skill[skill.Key] = skill.Value;
                                    }
                                    leg_shidan2_stance.Text = leg_shidan2_characters[i].stance;
                                }
                            }
                            skillBox.Text += leg_shidan2_characters[i].character_name + "\n";
                            foreach (var kv in leg_shidan2_characters[i].character_skill)
                            {
                                skillBox.Text += kv.Key + ":" + kv.Value + "\n";
                            }
                        }
                        var statusBox = (TextBox)this.FindName($"shidan2_chara{i}_status");
                        statusBox.Text = "HP:" + leg_shidan2_characters[i].character_current_status["HP"].ToString() + "\n";
                        statusBox.Text += "攻撃:" + leg_shidan2_characters[i].character_current_status["攻撃"].ToString() + "\n";
                        statusBox.Text += "防御:" + leg_shidan2_characters[i].character_current_status["防御"].ToString() + "\n";
                        statusBox.Text += "速度:" + leg_shidan2_characters[i].character_current_status["速度"].ToString() + "\n";
                        statusBox.Text += "知力:" + leg_shidan2_characters[i].character_current_status["知力"].ToString() + "\n";
                    }
                }
                leg_set_shidan_skill(leg_shidan2_characters, 2);
                set_legion_skill();
                leg_character_kassei_update(leg_shidan2_characters, leg_shidan2_skill);
                leg_character_shiki_update();
                leg_kago_calc();
            }
        }

        private void leg_load_shidan3_Click(object sender, RoutedEventArgs e)
        {
            var selected = leg_hozon_shidan_list.SelectedItem as shidan_savedata;
            if (selected != null)
            {
                // selected.character は Dictionary<int, character_info> なので、これをループする
                //セーブされているリーダーフラグがあるか確認。もし変わっていたら戻す
                leg_shidan3_characters.Clear();
                leg_shidan3_name.Text = selected.shidan_name.ToString();
                leg_shidan3_characters = selected.character.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.DeepCopy());
                for (int i = 1; i < 7; i++)
                {
                    var skillBox = (TextBox)this.FindName($"shidan3_chara{i}_name");
                    if (leg_shidan3_characters.ContainsKey(i) && leg_shidan3_characters[i] != null)
                    {
                        if (skillBox != null)
                        {
                            skillBox.Text = "";
                            if (leg_shidan3_characters[i].leader_flag == true)
                            {
                                skillBox.Text = "＊";
                                foreach (var skill in leg_shidan3_characters[i].leader_skill)
                                {
                                    if (leg_shidan3_characters[i].character_skill.ContainsKey(skill.Key))
                                    {
                                        if (skill.Value != 0)
                                        {
                                            leg_shidan3_characters[i].character_skill[skill.Key] += skill.Value;
                                        }
                                        else
                                        {
                                            leg_shidan3_characters[i].character_skill[skill.Key] = 1;
                                        }
                                    }
                                    else
                                    {
                                        leg_shidan3_characters[i].character_skill[skill.Key] = skill.Value;
                                    }
                                    leg_shidan3_stance.Text = leg_shidan3_characters[i].stance;
                                }
                            }
                            skillBox.Text += leg_shidan3_characters[i].character_name + "\n";
                            foreach (var kv in leg_shidan3_characters[i].character_skill)
                            {
                                skillBox.Text += kv.Key + ":" + kv.Value + "\n";
                            }
                        }
                        var statusBox = (TextBox)this.FindName($"shidan3_chara{i}_status");
                        statusBox.Text = "HP:" + leg_shidan3_characters[i].character_current_status["HP"].ToString() + "\n";
                        statusBox.Text += "攻撃:" + leg_shidan3_characters[i].character_current_status["攻撃"].ToString() + "\n";
                        statusBox.Text += "防御:" + leg_shidan3_characters[i].character_current_status["防御"].ToString() + "\n";
                        statusBox.Text += "速度:" + leg_shidan3_characters[i].character_current_status["速度"].ToString() + "\n";
                        statusBox.Text += "知力:" + leg_shidan3_characters[i].character_current_status["知力"].ToString() + "\n";
                    }
                }
                leg_set_shidan_skill(leg_shidan3_characters, 3);
                set_legion_skill();
                leg_character_kassei_update(leg_shidan3_characters, leg_shidan3_skill);
                leg_character_shiki_update();
                leg_kago_calc();
            }
        }

    }
}