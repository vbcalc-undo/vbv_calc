using JsonFileIO.Jsons;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Shapes;
using Tesseract;
using VBV_calc.Helpers;
using VBV_calc.Models;
using static VBV_calc.MainWindow;

namespace VBV_calc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const int FINALSKILL_NUM = 21;
        // ComboBox1 プロパティの型を object から ComboBox に変更
        public ComboBox ComboBox1 { get; private set; }
        private bool _isProgrammaticChange = false;

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

        public class DataSet
        {
            // DisplayMemberとValueMemberにはプロパティで指定する仕組み
            public Dictionary<string, string> ItemValue { get; set; }
            public string ItemCharacter { get; set; }

            // プロパティをコンストラクタでセット
            public DataSet(string character, Dictionary<string, string> s)
            {
                ItemCharacter = character;
                ItemValue = s;
            }
        }

        int soubi_shogo_kou = 0;
        int soubi_shogo_bou = 0;
        int soubi_shogo_soku = 0;
        int soubi_shogo_chiryoku = 0;

        private void ResetAllskills()
        {
            passive1.Text = "";
            passive1_fig.Text = "";
            passive2.Text = "";
            passive2_fig.Text = "";
            passive3.Text = "";
            passive3_fig.Text = "";
            passive4.Text = "";
            passive4_fig.Text = "";
            passive5.Text = "";
            passive5_fig.Text = "";
            passive6.Text = "";
            passive6_fig.Text = "";
            passive7.Text = "";
            passive7_fig.Text = "";
            passive8.Text = "";
            passive8_fig.Text = "";
            shogo1.Text = "";
            shogo1_fig.Text = "";
            shogo2.Text = "";
            shogo2_fig.Text = "";

            leader1.Text = "";
            leader1_fig.Text = "";
            leader2.Text = "";
            leader2_fig.Text = "";
            equipment1_skill1.Text = "";
            equipment1_skill1_fig.Text = "";
            equipment1_skill2.Text = "";
            equipment1_skill2_fig.Text = "";
            equipment2_skill1.Text = "";
            equipment2_skill1_fig.Text = "";
            equipment2_skill2.Text = "";
            equipment2_skill2_fig.Text = "";
            ryoshoku_name.Text = "";
            ryoshoku_fig_box.Text = "";
            final1.Text = "";
            final1_fig.Text = "";
            final2.Text = "";
            final2_fig.Text = "";
            final3.Text = "";
            final3_fig.Text = "";
            final4.Text = "";
            final4_fig.Text = "";
            final5.Text = "";
            final5_fig.Text = "";
            final6.Text = "";
            final6_fig.Text = "";
            final7.Text = "";
            final7_fig.Text = "";
            final8.Text = "";
            final8_fig.Text = "";
            final9.Text = "";
            final9_fig.Text = "";
            final10.Text = "";
            final10_fig.Text = "";
            final11.Text = "";
            final11_fig.Text = "";

            final12.Text = "";
            final12_fig.Text = "";
            final13.Text = "";
            final13_fig.Text = "";
            final14.Text = "";
            final14_fig.Text = "";
            final15.Text = "";
            final15_fig.Text = "";
            final16.Text = "";
            final16_fig.Text = "";
            final17.Text = "";
            final17_fig.Text = "";
            final18.Text = "";
            final18_fig.Text = "";
            final19.Text = "";
            final19_fig.Text = "";
            final20.Text = "";
            final20_fig.Text = "";
            final21.Text = "";
            final21_fig.Text = "";

            soubi_shogo_kou = 0;
            soubi_shogo_bou = 0;
            soubi_shogo_soku = 0;
            soubi_shogo_chiryoku = 0;

        }

        private void status_calc(string hp, string kou, string bou, string soku, string chiryoku, string cost, string rank)
        {
            double keikenti = 0;
            // 空文字または数値でない場合は初期値をセットして終了
            if (string.IsNullOrEmpty(hp) || !double.TryParse(hp, out _) ||
                string.IsNullOrEmpty(soku) || !double.TryParse(soku, out _) ||
                string.IsNullOrEmpty(kou) || !double.TryParse(kou, out _) ||
                string.IsNullOrEmpty(bou) || !double.TryParse(bou, out _) ||
                string.IsNullOrEmpty(chiryoku) || !double.TryParse(chiryoku, out _) ||
                string.IsNullOrEmpty(cost) || !double.TryParse(cost, out _) ||
                string.IsNullOrEmpty(rank))
            {
                _isProgrammaticChange = true;
                levelbox.Text = "1";
                _isProgrammaticChange = false;
                hpbox.Text = "1";
                sokudobox.Text = "1";
                kougekibox.Text = "1";
                bougyobox.Text = "1";
                chiryokubox.Text = "1";
                return;
            }
            int level = 0;
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
            double buko = 75 / 2 / (Math.Sqrt(double.Parse(cost)));
            double level1 = 1 + Math.Sqrt(keikenti) / 1000;
            double level2 = Math.Sqrt(keikenti) / 50;

            //int hp_status = (int)(double.Parse(hp) * ((double)(level)/4.0) + 1.0);
            int hp_status = (int)(double.Parse(hp) * ((double)(level) + 1.0) / 4.0);
            int kou_status = (int)((double.Parse(kou) + buko) * level1 + level2) + soubi1_status.kougeki + soubi2_status.kougeki + ryoshoku_status.kougeki;
            int bou_status = (int)((double.Parse(bou) + buko) * level1 + level2) + soubi1_status.bougyo + soubi2_status.bougyo + ryoshoku_status.bougyo;
            int soku_status = (int)((double.Parse(soku)) * level1 + level2) + soubi1_status.sokudo + soubi2_status.sokudo + ryoshoku_status.sokudo;
            int chi_status = (int)((double.Parse(chiryoku)) * level1 + level2) + soubi1_status.tiryoku + soubi2_status.tiryoku + ryoshoku_status.tiryoku;
            _isProgrammaticChange = true;
            levelbox.Text = level.ToString();
            _isProgrammaticChange = false;
            hpbox.Text = hp_status.ToString();
            sokudobox.Text = soku_status.ToString();
            kougekibox.Text = kou_status.ToString();
            bougyobox.Text = bou_status.ToString();
            chiryokubox.Text = chi_status.ToString();
        }
        private void status_calc_fix()
        {
            double keikenti = 0;
            int hp, soku, kou, bou, chiryoku, cost;
            string rank;
            current_Character_Status.get_status(out hp, out kou, out bou, out soku, out chiryoku, out cost, out rank);
            int level = 0;
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
            double buko = 75.0 / 2.0 / (Math.Sqrt(cost));
            double level1 = 1.0 + Math.Sqrt(keikenti) / 1000.0;
            double level2 = Math.Sqrt(keikenti) / 50.0;

            double temp = (level - 1.0) / 4.0 + 1.0;
            int hp_status = (int)(hp * ((level - 1.0) / 4.0 + 1.0));
            int kou_status = (int)((kou + buko + shogo1_status.kougeki + shogo2_status.kougeki) * level1 + level2) + soubi1_status.kougeki + soubi2_status.kougeki + ryoshoku_status.kougeki;
            int bou_status = (int)((bou + buko + shogo1_status.bougyo + shogo2_status.bougyo) * level1 + level2) + soubi1_status.bougyo + soubi2_status.bougyo + ryoshoku_status.bougyo;
            int soku_status = (int)((soku + shogo1_status.sokudo + shogo2_status.sokudo) * level1 + level2) + soubi1_status.sokudo + soubi2_status.sokudo + ryoshoku_status.sokudo;
            int chi_status = (int)((chiryoku + shogo1_status.tiryoku + shogo2_status.tiryoku) * level1 + level2) + soubi1_status.tiryoku + soubi2_status.tiryoku + ryoshoku_status.tiryoku;

            if (chi_status < 1) chi_status = 1;
            if (soku_status < 1) soku_status = 1;
            if (kou_status < 1) kou_status = 1;
            if (bou_status < 1) bou_status = 1;

            _isProgrammaticChange = true;
            levelbox.Text = level.ToString();
            _isProgrammaticChange = false;
            hpbox.Text = hp_status.ToString();
            sokudobox.Text = soku_status.ToString();
            kougekibox.Text = kou_status.ToString();
            bougyobox.Text = bou_status.ToString();
            chiryokubox.Text = chi_status.ToString();
        }


        private void status_calc_box(int level_value,int bukou_value)
        {
            int hp, soku, kou, bou, chiryoku, cost;
            string rank;
            if (bukou_value > 100) 
                bukou_value = 100;
            if (bukou_value < 1) 
                bukou_value = 1;

            current_Character_Status.get_status(out hp, out kou, out bou, out soku, out chiryoku, out cost, out rank);
            double  keikenti = 50 * (level_value * level_value) - (100 * level_value) + 50;
            double buko = (bukou_value - 25.0) / 2.0 / (Math.Sqrt(cost));
            double level1 = 1.0 + Math.Sqrt(keikenti) / 1000.0;
            double level2 = Math.Sqrt(keikenti) / 50.0;

            double temp = (level_value - 1.0) / 4.0 + 1.0;
            int hp_status = (int)(hp * ((level_value - 1.0) / 4.0 + 1.0));
            int kou_status = (int)((kou + buko + shogo1_status.kougeki + shogo2_status.kougeki) * level1 + level2) + soubi1_status.kougeki + soubi2_status.kougeki + ryoshoku_status.kougeki;
            int bou_status = (int)((bou + buko + shogo1_status.bougyo + shogo2_status.bougyo) * level1 + level2) + soubi1_status.bougyo + soubi2_status.bougyo + ryoshoku_status.bougyo;
            int soku_status = (int)((soku + shogo1_status.sokudo + shogo2_status.sokudo) * level1 + level2) + soubi1_status.sokudo + soubi2_status.sokudo + ryoshoku_status.sokudo;
            int chi_status = (int)((chiryoku + shogo1_status.tiryoku + shogo2_status.tiryoku) * level1 + level2) + soubi1_status.tiryoku + soubi2_status.tiryoku + ryoshoku_status.tiryoku;

            if (chi_status < 1) chi_status = 1;
            if (soku_status < 1) soku_status = 1;
            if (kou_status < 1) kou_status = 1;
            if (bou_status < 1) bou_status = 1;

            if (hpbox != null && sokudobox != null && kougekibox != null && chiryokubox != null && bougyobox != null)
            {
                hpbox.Text = hp_status.ToString();
                sokudobox.Text = soku_status.ToString();
                kougekibox.Text = kou_status.ToString();
                bougyobox.Text = bou_status.ToString();
                chiryokubox.Text = chi_status.ToString();
            }
        }

        /* デフォルトのキャラクターの能力 */
        CurrentCharacterStatus current_Character_Status = new CurrentCharacterStatus();
        private void ComboBox_SelectionChanged_character(object sender, SelectionChangedEventArgs e)
        {
            // labelに現在コンボ選択の内容を表示
            //ItemSet tmp = ((ItemSet)CharacterBox.SelectedItem);//表示名はキャストして取りだす
            int count = characters.Count;
            ResetAllskills();
            var selectedPerson = CharacterBox.SelectedItem as CharacterJson;

            if (selectedPerson != null)
            {
                for (int i = 0; i < count; i++)
                {
                    string skill_name = "";
                    int skill_value = 0;
                    if (characters[i].名称 == selectedPerson.名称 && characters[i].名称!="")
                    {
                        //Passive1 = characters[i].パッシブスキル[0];
                        (skill_name, skill_value) = SkillParser.Div_Skill_Name_Value(characters[i].パッシブスキル[0]);
                        passive1.Text = skill_name;
                        passive1_fig.Text = skill_value.ToString();

                        (skill_name, skill_value) = SkillParser.Div_Skill_Name_Value(characters[i].パッシブスキル[1]);
                        passive2.Text = skill_name;
                        passive2_fig.Text = skill_value.ToString();

                        (skill_name, skill_value) = SkillParser.Div_Skill_Name_Value(characters[i].パッシブスキル[2]);
                        passive3.Text = skill_name;
                        passive3_fig.Text = skill_value.ToString();

                        (skill_name, skill_value) = SkillParser.Div_Skill_Name_Value(characters[i].パッシブスキル[3]);
                        passive4.Text = skill_name;
                        passive4_fig.Text = skill_value.ToString();

                        (skill_name, skill_value) = SkillParser.Div_Skill_Name_Value(characters[i].パッシブスキル[4]);
                        passive5.Text = skill_name;
                        passive5_fig.Text = skill_value.ToString();

                        if (characters[i].パッシブスキル.Count > 5)
                        {
                            (skill_name, skill_value) = SkillParser.Div_Skill_Name_Value(characters[i].パッシブスキル[5]);
                            passive6.Text = skill_name;
                            passive6_fig.Text = skill_value.ToString();
                        }
                        if (characters[i].パッシブスキル.Count > 6)
                        {
                            (skill_name, skill_value) = SkillParser.Div_Skill_Name_Value(characters[i].パッシブスキル[6]);
                            passive7.Text = skill_name;
                            passive7_fig.Text = skill_value.ToString();
                        }
                        if (characters[i].パッシブスキル.Count > 7)
                        {
                            (skill_name, skill_value) = SkillParser.Div_Skill_Name_Value(characters[i].パッシブスキル[7]);
                            passive8.Text = skill_name;
                            passive8_fig.Text = skill_value.ToString();
                        }

                        (skill_name, skill_value) = SkillParser.Div_Skill_Name_Value(characters[i].リーダースキル[0]);
                        leader1.Text = skill_name;
                        leader1_fig.Text = skill_value.ToString();

                        (skill_name, skill_value) = SkillParser.Div_Skill_Name_Value(characters[i].リーダースキル[1]);
                        leader2.Text = skill_name;
                        leader2_fig.Text = skill_value.ToString();

                        int hp = 0, kou = 0, bou = 0, soku = 0, chi = 0, cost = 0;
                        // HP
                        if (!string.IsNullOrEmpty(characters[i].基本パラメータ_HP) && characters[i].基本パラメータ_HP != "-")
                        {
                            if (int.TryParse(characters[i].基本パラメータ_HP, out hp))
                            {
                                // hpを使う
                            }
                            else
                            {
                                hp = 0;
                            }
                        }
                        // 攻
                        if (!string.IsNullOrEmpty(characters[i].基本パラメータ_攻) && characters[i].基本パラメータ_攻 != "-")
                        {
                            if (int.TryParse(characters[i].基本パラメータ_攻, out kou))
                            {
                                // kouを使う
                            }
                            else
                            {
                                kou = 0;
                            }
                        }
                        // 防
                        if (!string.IsNullOrEmpty(characters[i].基本パラメータ_防) && characters[i].基本パラメータ_防 != "-")
                        {
                            if (int.TryParse(characters[i].基本パラメータ_防, out bou))
                            {
                                // bouを使う
                            }
                            else
                            {
                                bou = 0;
                            }
                        }
                        // 速
                        if (!string.IsNullOrEmpty(characters[i].基本パラメータ_速) && characters[i].基本パラメータ_速 != "-")
                        {
                            if (int.TryParse(characters[i].基本パラメータ_速, out soku))
                            {
                                // sokuを使う
                            }
                            else
                            {
                                soku = 0;
                            }
                        }
                        // 知
                        if (!string.IsNullOrEmpty(characters[i].基本パラメータ_知) && characters[i].基本パラメータ_知 != "-")
                        {
                            if (int.TryParse(characters[i].基本パラメータ_知, out chi))
                            {
                                // chiを使う
                            }
                            else
                            {
                                chi = 0;
                            }
                        }
                        // コスト
                        if (!string.IsNullOrEmpty(characters[i].コスト) && characters[i].コスト != "-")
                        {
                            if (int.TryParse(characters[i].コスト, out cost))
                            {
                                // costを使う
                            }
                            else
                            {
                                cost = 0;
                            }
                        }
                        status_calc(characters[i].基本パラメータ_HP, characters[i].基本パラメータ_攻, characters[i].基本パラメータ_防, characters[i].基本パラメータ_速, characters[i].基本パラメータ_知, characters[i].コスト, characters[i].ランク);
                        //current_Character_Status.set_status(int.Parse(characters[i].基本パラメータ_HP),int.Parse(characters[i].基本パラメータ_攻),int.Parse(characters[i].基本パラメータ_防),int.Parse(characters[i].基本パラメータ_速), characters[i].基本パラメータ_知), int.Parse(characters[i].基本パラメータ_コスト),characters[i].ランク);
                        current_Character_Status.set_status(hp, kou, bou, soku, chi, cost, characters[i].ランク, characters[i].種族, characters[i].特攻);
                        shuzoku_box.Text = characters[i].種族;
                        tokko_box.Text = characters[i].特攻;
                        shokugyo_box.Text = characters[i].職業;
                        src_shogo1.Clear();
                        src_shogo2.Clear();
                        //src_shogo1.Add(null);
                        //src_shogo2.Add(null);

                        src_shogo1.Add(new ItemSet(null, null));
                        src_shogo2.Add(new ItemSet(null, null));

                        string rank = characters[i].ランク;
                        int rank_value = 0;
                        if (rank == "S")
                            rank_value = 8;
                        if (rank == "A")
                            rank_value = 6;
                        if (rank == "B")
                            rank_value = 5;
                        if (rank == "C")
                            rank_value = 4;
                        if (rank == "D")
                            rank_value = 3;
                        if (rank == "E")
                            rank_value = 2;
                        // 問題の箇所を修正
                        foreach (var kvp in all_shogo)
                        {
                            var key = kvp.Key;
                            var shogoList = kvp.Value;
                            int count_shogo = all_shogo[key].Count;
                            if (count_shogo > 0)
                            {
                                if ((key.ToString() != "キャラクター" && characters[i].キャラクター != true) || (key.ToString() == "キャラクター" && characters[i].キャラクター == true))
                                {
                                    src_shogo1.Add(new ItemSet("", "※" + key.ToString()));
                                    src_shogo2.Add(new ItemSet("", "※" + key.ToString()));

                                    for (int h = 0; h < count_shogo; h++)
                                    {
                                        if (int.Parse(all_shogo[key][h].レア) <= rank_value)
                                        {
                                            string tempkey = all_shogo[key][h].二つ名;
                                            string tempvalue = "";
                                            if (all_shogo[key][h].能力付与.ContainsKey("追加スキル"))
                                            {
                                                if (all_shogo[key][h].能力付与["追加スキル"] != null)
                                                {
                                                    string addSkill = all_shogo[key][h].能力付与["追加スキル"];
                                                    tempkey += "(" + addSkill + ")";
                                                    tempvalue = addSkill + "," + all_shogo[key][h].ステータス変化["攻撃"] + "," + all_shogo[key][h].ステータス変化["防御"] + "," + all_shogo[key][h].ステータス変化["速度"] + "," + all_shogo[key][h].ステータス変化["知力"] + "," + all_shogo[key][h].ステータス変化["特攻"];
                                                }
                                            }
                                            // 接続リストの内容を判定
                                            if (all_shogo[key][h].接続 != null && all_shogo[key][h].接続.Count > 0)
                                            {
                                                // 接頭

                                                if (all_shogo[key][h].接続["接頭"] == "●")

                                                {
                                                    src_shogo1.Add(new ItemSet(tempvalue, tempkey));
                                                }
                                                // 接尾
                                                if (all_shogo[key][h].接続.Count > 1 && all_shogo[key][h].接続["接尾"] == "●")
                                                {
                                                    src_shogo2.Add(new ItemSet(tempvalue, tempkey));
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        src_equipment1.Clear();
                        src_equipment2.Clear();
                        int count_equip = all_equipments[characters[i].装備[0]].Count;
                        src_equipment1.Add(new ItemSet(null,null));
                        src_equipment2.Add(new ItemSet(null, null));
                        //EquipmentBox1.Items.Clear();:
                        for (int j = 0; j < count_equip; j++)
                        {
                            if (all_equipments[characters[i].装備[0]][j].能力付加.Count > 1)
                            {
                                //Debug.WriteLine("all_equipments[characters[i].装備[0]=" + all_equipments[characters[i].装備[0]][j].名称+"(" + all_equipments[characters[i].装備[0]][j].能力付加[0] + ","+ all_equipments[characters[i].装備[0]][j].能力付加[1] + ")");
                                string tempkey = all_equipments[characters[i].装備[0]][j].名称.ToString() + "(" + all_equipments[characters[i].装備[0]][j].能力付加[0].ToString() + "," + all_equipments[characters[i].装備[0]][j].能力付加[1].ToString() + ")";
                                string tempvalue = all_equipments[characters[i].装備[0]][j].能力付加[0].ToString() + "," + all_equipments[characters[i].装備[0]][j].能力付加[1].ToString() + "," + all_equipments[characters[i].装備[0]][j].性能変化_攻 + "," + all_equipments[characters[i].装備[0]][j].性能変化_防 + "," + all_equipments[characters[i].装備[0]][j].性能変化_速 + "," + all_equipments[characters[i].装備[0]][j].性能変化_知;
                                src_equipment1.Add(new ItemSet(tempvalue, tempkey));
                            }
                            else
                            {
                                string tempkey = all_equipments[characters[i].装備[0]][j].名称.ToString() + "(" + all_equipments[characters[i].装備[0]][j].能力付加[0].ToString() + ")";
                                string tempvalue = all_equipments[characters[i].装備[0]][j].能力付加[0].ToString() + "," + "," + all_equipments[characters[i].装備[0]][j].性能変化_攻 + "," + all_equipments[characters[i].装備[0]][j].性能変化_防 + "," + all_equipments[characters[i].装備[0]][j].性能変化_速 + "," + all_equipments[characters[i].装備[0]][j].性能変化_知;
                                src_equipment1.Add(new ItemSet(tempvalue, tempkey));
                            }
                            if (all_equipments[characters[i].装備[1]][j].能力付加.Count > 1)
                            {
                                //Debug.WriteLine("all_equipments[characters[i].装備[0]=" + all_equipments[characters[i].装備[1]][j].名称 + "(" + all_equipments[characters[i].装備[1]][j].能力付加[0] + "," + all_equipments[characters[i].装備[1]][j].能力付加[1] + ")");
                                string tempkey = all_equipments[characters[i].装備[1]][j].名称.ToString() + "(" + all_equipments[characters[i].装備[1]][j].能力付加[0].ToString() + "," + all_equipments[characters[i].装備[1]][j].能力付加[1].ToString() + ")";
                                string tempvalue = all_equipments[characters[i].装備[1]][j].能力付加[0].ToString() + "," + all_equipments[characters[i].装備[1]][j].能力付加[1].ToString() + "," + all_equipments[characters[i].装備[1]][j].性能変化_攻 + "," + all_equipments[characters[i].装備[1]][j].性能変化_防 + "," + all_equipments[characters[i].装備[1]][j].性能変化_速 + "," + all_equipments[characters[i].装備[1]][j].性能変化_知;
                                src_equipment2.Add(new ItemSet(tempvalue, tempkey));
                            }
                            else
                            {
                                string tempkey = all_equipments[characters[i].装備[1]][j].名称.ToString() + "(" + all_equipments[characters[i].装備[1]][j].能力付加[0].ToString() + ")";
                                string tempvalue = all_equipments[characters[i].装備[1]][j].能力付加[0].ToString() + "," + "," + all_equipments[characters[i].装備[1]][j].性能変化_攻 + "," + all_equipments[characters[i].装備[1]][j].性能変化_防 + "," + all_equipments[characters[i].装備[1]][j].性能変化_速 + "," + all_equipments[characters[i].装備[1]][j].性能変化_知; ;
                                src_equipment2.Add(new ItemSet(tempvalue, tempkey));
                            }

                        }
                        ryoshokuBox.SelectedIndex = -1;
                        //src_equipment1.Add(new ItemSet(all_equipments[characters[i].装備[0]], all_equipments[characters[i].装備[0]]);

                        //Debug.WriteLine("all_equipments[characters[i].装備[0]=" + all_equipments[characters[i].装備[0]]);
                        //Debug.WriteLine("all_equipments[characters[i].装備[1]=" + all_equipments[characters[i].装備[1]]);

                    }
                }
            }
            //labelValue.Text = CharacterBox.SelectedValue.ToString();//値はそのまま取りだせる
            Resync_finalskil();
            calc_final_attack_mag();
            calc_damage();

        }

        // 修正内容: ComboBox の DataSource, DisplayMember, ValueMember は WPF には存在しません。
        // WPF では ItemsSource, DisplayMemberPath, SelectedValuePath を使います。

        CharacterJson character = new CharacterJson();
        List<CharacterJson> characters = null;        

        private void load_json_character2()
        {
            List<String> temp_asistskills = new List<string>();
            using (var sr = new StreamReader(@"./json/units/units_ippan.json", System.Text.Encoding.UTF8))
            {
                var jsonReadData = sr.ReadToEnd();
                characters = JsonConvert.DeserializeObject<List<CharacterJson>>(jsonReadData);
                for (int i = 0; i < characters.Count; i++)
                {
                    String temp_name = ReplaceBracketNumberWithColon(characters[i].アシストスキル[0]);
                    if (characters[i].アシストスキル.Count != 0)
                        if (!src_asistskills.Any(item => item.ItemValue == temp_name))
                            src_asistskills.Add(new ItemSet(temp_name, temp_name));
                }
                characters.Insert(0, new CharacterJson
                {
                    名称 = "",   // 空表示
                });
            }
            using (var sr = new StreamReader(@"./json/units/units_busho.json", System.Text.Encoding.UTF8))
            {
                var jsonReadData = sr.ReadToEnd();
                var bushoCharacters = JsonConvert.DeserializeObject<List<CharacterJson>>(jsonReadData);
                for (int i = 0; i < bushoCharacters.Count; i++)
                {
                    bushoCharacters[i].キャラクター = true;
                    String temp_name = ReplaceBracketNumberWithColon(bushoCharacters[i].アシストスキル[0]);
                    if (bushoCharacters[i].アシストスキル.Count != 0)
                        if (!src_asistskills.Any(item => item.ItemValue == temp_name))
                            src_asistskills.Add(new ItemSet(temp_name, temp_name));
                }
                if (bushoCharacters != null)
                {
                    characters.AddRange(bushoCharacters);
                }
            }
            var sorted = src_asistskills.OrderBy(x => x.ItemDisp).ToList();
            src_asistskills.Clear();
            foreach (var item in sorted)
            {
                src_asistskills.Add(item);
            }
        }

        public static string ReplaceBracketNumberWithColon(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            // グループ1 = ブラケットの前の任意の文字（貪欲になりすぎないよう ? を付ける）
            // グループ2 = ブラケット内の数字（1つ以上の数字）
            var pattern = new Regex(@"([^\[\]]+?)\[(\d+)\]");

            string result = pattern.Replace(input, match =>
            {
                string name = match.Groups[1].Value.Trim();
                string number = match.Groups[2].Value;
                return $"{name}:{number}";
            });

            return result;
        }

        ObservableCollection<ItemSet> src_equipment1;
        ObservableCollection<ItemSet> src_equipment2;
        ObservableCollection<ItemSet> src_shogo1;
        ObservableCollection<ItemSet> src_shogo2;
        ObservableCollection<ItemSet> src_asistskills;
        ObservableCollection<ItemSet> src_asistskill1;
        ObservableCollection<ItemSet> src_asistskill2;
        ObservableCollection<ItemSet> src_asistskill3;
        ObservableCollection<String> enemy_stancelist;
        ObservableCollection<String> stancelist;
        ObservableCollection<String> enemy_shokugholist;

        List<ItemSet> src_ryoshoku;
        Dictionary<string, List<EquipmentJson>> all_equipments; // ここで宣言
        Dictionary<string, List<ShogoJson>> all_shogo; // ここで宣言
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

        private double calc_shokugyo_bairitu()
        {
            string shokugyo = shokugyo_box.Text;
            string enemy_shokugyo = enemy_shokugyo_box.SelectedItem as string;
            //ItemSet tmp = (enemy_shokugyo_box.SelectedItem);//表示名はキャストして取りだす

            if (shokugyo != "" && enemy_shokugyo != "")
            {
                if (shokugyo == "ブレイダー" && enemy_shokugyo == "デストロイヤー")
                {
                    return 1.5;
                }
                if (shokugyo == "デストロイヤー" && enemy_shokugyo == "ガーダー")
                {
                    return 1.5;
                }
                if (shokugyo == "ガーダー" && enemy_shokugyo == "キャスター")
                {
                    return 1.5;
                }
                if (shokugyo == "キャスター" && enemy_shokugyo == "シューター")
                {
                    return 1.5;
                }
                if (shokugyo == "シューター" && enemy_shokugyo == "ランサー")
                {
                    return 1.5;
                }
                if (shokugyo == "ランサー" && enemy_shokugyo == "ブレイダー")
                {
                    return 1.5;
                }
            }
            return 1.0;
        }

        private double calc_stance_bairitu()
        {
            string stance = stance_box.SelectedItem as string;
            string enemy_stance = enemy_stance_box.SelectedItem as string; ;
            if (stance != "" && enemy_stance != "")
            {
                if (stance == "進撃" && enemy_stance == "計略")
                {
                    return 1.25;
                }
                if (stance == "計略" && enemy_stance == "防備")
                {
                    return 1.25;
                }
                if (stance == "防備" && enemy_stance == "進撃")
                {
                    return 1.25;
                }
            }
            return 1.0;
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
            src_ryoshoku.Add(new ItemSet(null, null));

            if (all_equipments["糧食"] != null && all_equipments["糧食"].Count > 0)
            {
                int count = all_equipments["糧食"].Count;
                for (int i = 0; i < count; i++)
                {
                    string tempkey = all_equipments["糧食"][i].名称 + "(" + all_equipments["糧食"][i].能力付加[0] + ")";
                    string tempvalue = all_equipments["糧食"][i].能力付加[0] + "," + all_equipments["糧食"][i].性能変化_攻 + "," + all_equipments["糧食"][i].性能変化_防 + "," + all_equipments["糧食"][i].性能変化_速 + "," + all_equipments["糧食"][i].性能変化_知;
                    src_ryoshoku.Add(new ItemSet(tempvalue, tempkey));
                    // ここで要素を処理
                    // 例: Console.WriteLine($"処理中の要素: {numbers[i]}");
                }
                //Debug.WriteLine(characters[0].パッシブスキル);
                //Debug.WriteLine(characters[0].職業);
            }
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
            load_json_shogo_func(@"./json/medallion\busho_shogo.json", "キャラクター");
        }
        private bool _isInitialized = false;

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
        public bool asist_flag = false;
        ObservableCollection<savedata> all_save_data = new ObservableCollection<savedata>();
        List<string> STANCE_LIST = new List<string> { "", "防備", "計略", "進撃", "乱戦" };
        List<string> SHOKUGYO_LIST = new List<string> { "", "ブレイダー", "ランサー", "シューター", "キャスター", "ガーダー", "デストロイヤー", "ヒーラー" };
        private static InferenceSession session = new InferenceSession(@"feature_extraction/resnet50_features.onnx");
        private static Dictionary<string, float[]> featureDict;
        private static Dictionary<string, string> idNameMap;
        public MainWindow()
        {
            InitializeComponent();
            var version = Assembly.GetExecutingAssembly()
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion?
            .Split('+')[0]; // "+" 以降をカット
            this.Title = $"vbv_calc VBVダメージ計算器 v{version}";

            // Dictionary<string, string> single_line = new Dictionary<int, string>();
            all_equipments = new Dictionary<string, List<EquipmentJson>>();
            all_shogo = new Dictionary<string, List<ShogoJson>>();

            List<DataSet> data = new List<DataSet>();

            List<ItemSet> src_character = new List<ItemSet>();
            src_shogo1 = new ObservableCollection<ItemSet>();
            src_shogo2 = new ObservableCollection<ItemSet>();

            src_equipment1 = new ObservableCollection<ItemSet>();
            src_equipment2 = new ObservableCollection<ItemSet>();
            src_ryoshoku = new List<ItemSet>();

            src_asistskills = new ObservableCollection<ItemSet>();
            src_asistskill1 = new ObservableCollection<ItemSet>();
            src_asistskill2 = new ObservableCollection<ItemSet>();
            src_asistskill3 = new ObservableCollection<ItemSet>();

            enemy_stancelist = new ObservableCollection<String>();
            stancelist = new ObservableCollection<String>();
            enemy_shokugholist = new ObservableCollection<String>();


            //src_character.Add(new ItemSet("Number1", "Number1"));
            //src_character.Add(new ItemSet("Number2", "Number2"));
            //src_character.Add(new ItemSet("Number3", "Number3"));

            //load_json_character(data, src_character);
            load_json_character2();
            load_json_equipment();
            load_json_shogo();


            // ComboBoxに表示と値をセット (WPF流)
            //CharacterBox.ItemsSource = src_character;
            CharacterBox.ItemsSource = characters;
            //CharacterBox.DisplayMemberPath = "ItemDisp";
            //CharacterBox.SelectedValuePath = "ItemValue";
            var view = CollectionViewSource.GetDefaultView(CharacterBox.ItemsSource);
            view.Filter = null;

            shogo1Box.ItemsSource = src_shogo1;
            shogo1Box.DisplayMemberPath = "ItemDisp";
            shogo1Box.SelectedValuePath = "ItemValue";

            shogo2Box.ItemsSource = src_shogo2;
            shogo2Box.DisplayMemberPath = "ItemDisp";
            shogo2Box.SelectedValuePath = "ItemValue";

            EquipmentBox1.ItemsSource = src_equipment1;
            EquipmentBox1.DisplayMemberPath = "ItemDisp";
            EquipmentBox1.SelectedValuePath = "ItemValue";

            EquipmentBox2.ItemsSource = src_equipment2;
            EquipmentBox2.DisplayMemberPath = "ItemDisp";
            EquipmentBox2.SelectedValuePath = "ItemValue";

            ryoshokuBox.ItemsSource = src_ryoshoku;
            ryoshokuBox.DisplayMemberPath = "ItemDisp";
            ryoshokuBox.SelectedValuePath = "ItemValue";
            // 番号付きラッパーに変換
            saved_list.ItemsSource = all_save_data;

            assistskill1_box.ItemsSource = src_asistskills;
            assistskill1_box.DisplayMemberPath = "ItemDisp";
            assistskill1_box.SelectedValuePath = "ItemValue";
            assistskill2_box.ItemsSource = src_asistskills;
            assistskill2_box.DisplayMemberPath = "ItemDisp";
            assistskill2_box.SelectedValuePath = "ItemValue";
            assistskill3_box.ItemsSource = src_asistskills;
            assistskill3_box.DisplayMemberPath = "ItemDisp";
            assistskill3_box.SelectedValuePath = "ItemValue";

            enemy_stance_box.ItemsSource = STANCE_LIST;
            enemy_stance_box.SelectedIndex = -1;
            stance_box.ItemsSource = STANCE_LIST;
            stance_box.SelectedIndex = -1;
            enemy_shokugyo_box.ItemsSource = SHOKUGYO_LIST;
            enemy_shokugyo_box.SelectedIndex = -1;

            //passive1.DataContext = src_character;

            // 初期値セット
            asist_flag = true;
            CharacterBox.SelectedIndex = 0;
            ComboBox_SelectionChanged_character(null, null);
            Resync_finalskil();

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

            //特徴量データの読み込
            string jsonPath = @"feature_extraction/chara_features.json";              // Python特徴量DB
            string csvPath = @"feature_extraction/list.csv";                   // ID→名前
            string json = File.ReadAllText(jsonPath);
            featureDict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, float[]>>(json);
            idNameMap = File.ReadAllLines(csvPath)
            .Skip(1)
            .Select(line => line.Split(','))
            .ToDictionary(parts => parts[1], parts => parts[2]);
            features = featureDict.Select(kv => new ImageFeature { Id = kv.Key, Feature = kv.Value }).ToList();
            
            _isInitialized = true; // 初期化完了
        }
        List<ImageFeature> features;

        private string _Passive1;


        private void OnPropertyChanged(string passive1)
        {
            throw new NotImplementedException();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private int hissatu_from_param(double status_bairitu)
        {
            double sokudo = 1.0;
            double enemy_sokudo = 1.0;
            double hissatu_kakuritu = 1.0;
            if (sokudobox != null && !string.IsNullOrEmpty(sokudobox.Text))
            {
                if (double.TryParse(sokudobox.Text, out sokudo))
                {
                }
                else
                {
                    // 変換できなかった場合はデフォルト値（1.0）のまま
                }
            }
            if (enemy_sokudo_box != null && !string.IsNullOrEmpty(enemy_sokudo_box.Text))
            {
                if (double.TryParse(enemy_sokudo_box.Text, out enemy_sokudo))
                {
                }
                else
                {
                    // 変換できなかった場合はデフォルト値（1.0）のまま
                }
            }
            sokudo = status_bairitu * sokudo;
            hissatu_kakuritu = Math.Sqrt(sokudo) * 3.0 - Math.Sqrt(enemy_sokudo);
            return (int)hissatu_kakuritu;

        }

        private double get_kyojingari_value()
        {
            double kougeki_bairitu = 1.0;
            double skillvalue = 0.0;

            for (int i = 1; i <= FINALSKILL_NUM; i++)
            {
                var textBox = (TextBox)this.FindName($"final{i}");
                if (textBox != null)
                {
                    if (textBox.Text == "巨人狩り")
                    {
                        var valueBox = (TextBox)this.FindName($"final{i}_fig");
                        if (double.TryParse(valueBox.Text, out skillvalue))

                            kougeki_bairitu = (100.0 + skillvalue) / 100.0;
                    }
                }
            }
            return kougeki_bairitu;

        }
        private double get_shinkaku_value()
        {
            double kougeki_bairitu = 1.0;
            double skillvalue = 0.0;
            for (int i = 1; i <= FINALSKILL_NUM; i++)
            {
                var textBox = (TextBox)this.FindName($"final{i}");
                if (textBox != null)
                {
                    if (textBox.Text == "心核穿ち")
                    {
                        var valueBox = (TextBox)this.FindName($"final{i}_fig");
                        if (double.TryParse(valueBox.Text, out skillvalue))

                            kougeki_bairitu = (100.0 + skillvalue) / 100.0;
                    }
                }
            }
            return kougeki_bairitu;
        }
        private double get_renkei_value()
        {
            double kougeki_bairitu = 1.0;
            double skillvalue = 0.0;
            for (int i = 1; i <= FINALSKILL_NUM; i++)
            {
                var textBox = (TextBox)this.FindName($"final{i}");
                if (textBox != null)
                {
                    if (textBox.Text == "連携攻撃")
                    {
                        var valueBox = (TextBox)this.FindName($"final{i}_fig");
                        if (double.TryParse(valueBox.Text, out skillvalue))
                            kougeki_bairitu = (100.0 + skillvalue) / 100.0;
                    }
                }
            }
            return kougeki_bairitu;
        }
        private double get_zenryoku_value()
        {
            double kougeki_bairitu = 1.0;
            double skillvalue = 0.0;
            for (int i = 1; i <= FINALSKILL_NUM; i++)
            {
                var textBox = (TextBox)this.FindName($"final{i}");
                if (textBox != null)
                {
                    if (textBox.Text == "全力攻撃")
                    {
                        var valueBox = (TextBox)this.FindName($"final{i}_fig");
                        if (double.TryParse(valueBox.Text, out skillvalue))
                            kougeki_bairitu = (100.0 + skillvalue) / 100.0;
                    }
                }
            }
            return kougeki_bairitu;
        }

        private double get_kyohon_value()
        {
            double kougeki_bairitu = 1.0;
            double skillvalue = 0.0;
            for (int i = 1; i <= FINALSKILL_NUM; i++)
            {
                var textBox = (TextBox)this.FindName($"final{i}");
                if (textBox != null)
                {
                    if (textBox.Text == "狂奔の牙")
                    {
                        var valueBox = (TextBox)this.FindName($"final{i}_fig");
                        if (double.TryParse(valueBox.Text, out skillvalue))
                            kougeki_bairitu = (100.0 + skillvalue * 6.0) / 100.0;
                    }
                }
            }
            return kougeki_bairitu;
        }
        private double get_kyousenshi_value()
        {
            double kougeki_bairitu = 1.0;
            double skillvalue = 0.0;
            for (int i = 1; i <= FINALSKILL_NUM; i++)
            {
                var textBox = (TextBox)this.FindName($"final{i}");
                if (textBox != null)
                {
                    if (textBox.Text == "狂戦士化")
                    {
                        var valueBox = (TextBox)this.FindName($"final{i}_fig");
                        if (double.TryParse(valueBox.Text, out skillvalue))
                            kougeki_bairitu = (100.0 + skillvalue) / 100.0;
                    }
                }
            }
            return kougeki_bairitu;
        }
        private double get_kakuritu_tuigeki_value(double unmei_value)
        {
            double kougeki_bairitu = 0.0;
            double skillvalue = 0.0;
            for (int i = 1; i <= FINALSKILL_NUM; i++)
            {
                var textBox = (TextBox)this.FindName($"final{i}");
                if (textBox != null)
                {
                    if (textBox.Text == "確率追撃")
                    {
                        var valueBox = (TextBox)this.FindName($"final{i}_fig");
                        if (double.TryParse(valueBox.Text, out skillvalue))
                            kougeki_bairitu = (skillvalue + unmei_value) / 100.0;
                    }
                }
            }
            return kougeki_bairitu;
        }
        private double get_tadan_value()
        {
            double kougeki_bairitu = 1.0;
            double skillvalue = 0.0;
            for (int i = 1; i <= FINALSKILL_NUM; i++)
            {
                var textBox = (TextBox)this.FindName($"final{i}");
                if (textBox != null)
                {
                    if (textBox.Text == "多段攻撃")
                    {
                        var valueBox = (TextBox)this.FindName($"final{i}_fig");
                        if (double.TryParse(valueBox.Text, out skillvalue))
                            if (skillvalue > 6) skillvalue = 6;
                        kougeki_bairitu = (100.0 + (skillvalue * 35)) / 100.0;
                    }
                }
            }
            return kougeki_bairitu;
        }
        private double get_mukougui_value(double unmei_value)
        {
            double kougeki_bairitu = 1.0;
            double skillvalue = 0.0;
            for (int i = 1; i <= FINALSKILL_NUM; i++)
            {
                var textBox = (TextBox)this.FindName($"final{i}");
                if (textBox != null)
                {
                    if (textBox.Text == "無効喰い")
                    {
                        var valueBox = (TextBox)this.FindName($"final{i}_fig");
                        if (double.TryParse(valueBox.Text, out skillvalue))
                        {
                            double temp_value = skillvalue + unmei_value;
                            if (temp_value > 100) temp_value = 100;
                            kougeki_bairitu = (100.0 + temp_value) / 100.0;
                        }

                    }
                }
            }
            return kougeki_bairitu;
        }
        private double get_sippuu_value(double tuigeki_value, double unmei_value)
        {
            double kougeki_bairitu = 1.0;
            double skillvalue = 0.0;
            for (int i = 1; i <= FINALSKILL_NUM; i++)
            {
                var textBox = (TextBox)this.FindName($"final{i}");
                if (textBox != null)
                {
                    if (textBox.Text == "疾風迅雷")
                    {
                        var valueBox = (TextBox)this.FindName($"final{i}_fig");
                        if (double.TryParse(valueBox.Text, out skillvalue))
                            if(tuigeki_value > 1.0)
                                kougeki_bairitu = (100.0 + skillvalue + tuigeki_value - unmei_value) / 100.0;
                            else
                                kougeki_bairitu = (100.0 + skillvalue + tuigeki_value) / 100.0;
                    }
                }
            }
            return kougeki_bairitu;
        }
        private double get_kenkon_value(double tuigeki_value, double unmei_value)
        {
            double kougeki_bairitu = 1.0;
            double skillvalue = 0.0;
            for (int i = 1; i <= FINALSKILL_NUM; i++)
            {
                var textBox = (TextBox)this.FindName($"final{i}");
                if (textBox != null)
                {
                    if (textBox.Text == "乾坤一擲")
                    {
                        var valueBox = (TextBox)this.FindName($"final{i}_fig");
                        if (double.TryParse(valueBox.Text, out skillvalue))
                            if (tuigeki_value > 1.0)
                                kougeki_bairitu = (100.0 + skillvalue + tuigeki_value - unmei_value) / 100.0;
                            else
                                kougeki_bairitu = (100.0 + skillvalue + tuigeki_value) / 100.0;
                    }
                }
            }
            return kougeki_bairitu;
        }
        private double get_shuyaku_value()
        {
            double kougeki_bairitu = 1.0;
            double skillvalue = 0.0;
            for (int i = 1; i <= FINALSKILL_NUM; i++)
            {
                var textBox = (TextBox)this.FindName($"final{i}");
                if (textBox != null)
                {
                    if (textBox.Text == "集約攻撃")
                    {
                        int hani_value = check_hani_fromfinal();
                        kougeki_bairitu = (100.0 + 50.0 + hani_value) / 100.0;
                    }
                }
            }
            return kougeki_bairitu;
        }
        private double get_toushou_value()
        {
            double kougeki_bairitu = 1.0;
            double skillvalue = 0.0;
            for (int i = 1; i <= FINALSKILL_NUM; i++)
            {
                var textBox = (TextBox)this.FindName($"final{i}");
                if (textBox != null)
                {
                    if (textBox.Text == "凍傷気流")
                    {
                        var valueBox = (TextBox)this.FindName($"final{i}_fig");
                        if (double.TryParse(valueBox.Text, out skillvalue))
                            kougeki_bairitu = (100.0 + skillvalue) / 100.0;
                    }
                }
            }
            return kougeki_bairitu;
        }
        private double get_seisen_value()
        {
            double kougeki_bairitu = 1.0;
            double skillvalue = 0.0;
            for (int i = 1; i <= FINALSKILL_NUM; i++)
            {
                var textBox = (TextBox)this.FindName($"final{i}");
                if (textBox != null)
                {
                    if (textBox.Text == "聖戦の導")
                    {
                        var valueBox = (TextBox)this.FindName($"final{i}_fig");
                        if (double.TryParse(valueBox.Text, out skillvalue))
                            kougeki_bairitu = (100.0 + skillvalue) / 100.0;
                    }
                }
            }
            return kougeki_bairitu;
        }
        private double get_kasoku_value()
        {
            double kougeki_bairitu = 1.0;
            double skillvalue = 0.0;
            for (int i = 1; i <= FINALSKILL_NUM; i++)
            {
                var textBox = (TextBox)this.FindName($"final{i}");
                if (textBox != null)
                {
                    if (textBox.Text == "加速進化")
                    {
                        var valueBox = (TextBox)this.FindName($"final{i}_fig");
                        if (double.TryParse(valueBox.Text, out skillvalue))
                            kougeki_bairitu = ((100.0 + skillvalue) / (100.0));
                    }
                }
            }
            return kougeki_bairitu;
        }

        //カブト割の場合は倍率は渡さず値だけ渡す
        private double get_kabutowari_value()
        {
            double skillvalue = 0.0;
            for (int i = 1; i <= FINALSKILL_NUM; i++)
            {
                var textBox = (TextBox)this.FindName($"final{i}");
                if (textBox != null)
                {
                    if (textBox.Text == "カブト割")
                    {
                        var valueBox = (TextBox)this.FindName($"final{i}_fig");
                        if (double.TryParse(valueBox.Text, out skillvalue))
                        {
                        }
                    }
                }
            }
            return skillvalue;
        }
        //次元斬撃の場合は倍率は渡さず値だけ渡す
        private double get_jigenzangeki_value()
        {
            double skillvalue = 0.0;
            for (int i = 1; i <= FINALSKILL_NUM; i++)
            {
                var textBox = (TextBox)this.FindName($"final{i}");
                if (textBox != null)
                {
                    if (textBox.Text == "次元斬撃")
                    {
                        var valueBox = (TextBox)this.FindName($"final{i}_fig");
                        if (double.TryParse(valueBox.Text, out skillvalue))
                        {
                        }
                    }
                }
            }
            return skillvalue;
        }

        //必殺の場合は倍率は渡さず値だけ渡す
        private double get_hissatu_value(double unmei_value)
        {
            double skillvalue = 0.0;
            for (int i = 1; i <= FINALSKILL_NUM; i++)
            {
                var textBox = (TextBox)this.FindName($"final{i}");
                if (textBox != null)
                {
                    if (textBox.Text == "必殺増加")
                    {
                        var valueBox = (TextBox)this.FindName($"final{i}_fig");
                        if (double.TryParse(valueBox.Text, out skillvalue))
                        {
                            skillvalue += unmei_value;
                        }
                    }
                }
            }
            return skillvalue;
        }

        //致命の場合は倍率は渡さず値だけ渡す
        private double get_chimei_value()
        {
            double skillvalue = 0.0;
            for (int i = 1; i <= FINALSKILL_NUM; i++)
            {
                var textBox = (TextBox)this.FindName($"final{i}");
                if (textBox != null)
                {
                    if (textBox.Text == "致命必殺")
                    {
                        var valueBox = (TextBox)this.FindName($"final{i}_fig");
                        if (double.TryParse(valueBox.Text, out skillvalue))
                        {

                        }
                    }
                }
            }
            return skillvalue;
        }

        // final1-FINALSKILL_NUMまで範囲攻撃が入っているかチェック
        private int check_hani_fromfinal()
        {
            int hani_value = 0;
            for (int i = 1; i <= FINALSKILL_NUM; i++)
            {
                var textBox = (TextBox)this.FindName($"final{i}");
                if (textBox != null)
                {
                    if (textBox.Text == "貫通攻撃" || textBox.Text == "扇形攻撃")
                    {
                        if (hani_value != 100)
                            hani_value = 50;
                    }
                    if (textBox.Text == "十字攻撃" || textBox.Text == "全域攻撃" || textBox.Text == "軍団攻撃")
                    {
                        hani_value = 100;
                    }
                }
            }
            return hani_value;
        }
        private int check_tuigeki()
        {
            int tuigeki_value = 0;
            for (int i = 1; i <= FINALSKILL_NUM; i++)
            {
                var textBox = (TextBox)this.FindName($"final{i}");
                if (textBox != null)
                {
                    if (textBox.Text == "確率追撃")
                    {
                        var valueBox = (TextBox)this.FindName($"final{i}_fig");
                        if (int.TryParse(valueBox.Text, out tuigeki_value))
                            return tuigeki_value;
                    }
                }
            }

            return tuigeki_value;
        }
        private int get_unmei_value()
        {
            int unmei_value = 0;
            for (int i = 1; i <= FINALSKILL_NUM; i++)
            {
                var textBox = (TextBox)this.FindName($"final{i}");
                if (textBox != null)
                {
                    if (textBox.Text == "運命の輪" || textBox.Text == "運命改変")
                    {
                        var valueBox = (TextBox)this.FindName($"final{i}_fig");
                        int temp_unmei_value = 0;
                        if (int.TryParse(valueBox.Text, out temp_unmei_value))
                            unmei_value += temp_unmei_value;
                    }
                }
            }
            return unmei_value;
        }
        private int get_sokumen_value()
        {
            int sokumen_value = 0;
            for (int i = 1; i <= FINALSKILL_NUM; i++)
            {
                var textBox = (TextBox)this.FindName($"final{i}");
                if (textBox != null)
                {
                    if (textBox.Text == "側面攻撃")
                    {
                        var valueBox = (TextBox)this.FindName($"final{i}_fig");
                        if (int.TryParse(valueBox.Text, out sokumen_value))
                            return sokumen_value;
                    }
                }
            }
            return sokumen_value;
        }

        private int get_bousou_value(int unmei_value)
        {
            int bousou_value = 0;
            for (int i = 1; i <= FINALSKILL_NUM; i++)
            {
                var textBox = (TextBox)this.FindName($"final{i}");
                if (textBox != null)
                {
                    if (textBox.Text == "暴走攻撃")
                    {
                        var valueBox = (TextBox)this.FindName($"final{i}_fig");
                        if (int.TryParse(valueBox.Text, out bousou_value))
                            return bousou_value + unmei_value;
                    }
                }
            }
            return bousou_value;
        }

        private int ibeido_mushi_calc(int unmei_value)
        {
            int ibeido_mushi_value = 0;
            double tadanti = 0.0;
            double tadan = get_tadan_value();
            int tadan_value = 0;
            if (tadan != 1.0)
            {
                tadanti = (tadan - 1.0) / 0.35;
                tadan_value = (int)(tadanti + 1) * 5;
            }
            int sokumen = get_sokumen_value();
            int bousou = get_bousou_value(unmei_value);
            ibeido_mushi_value = tadan_value + sokumen + bousou;

            return ibeido_mushi_value;
        }
        private bool enkaku_check()
        {
            for (int i = 1; i <= FINALSKILL_NUM; i++)
            {
                var textBox = (TextBox)this.FindName($"final{i}");
                if (textBox != null)
                {
                    if (textBox.Text == "遠隔攻撃" || textBox.Text == "次元斬撃")
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        bool shinma_flag = false;
        bool shuyaku_flag = false;
        bool enkaku_flag = false;
        double hissatu_kougeki_bairitu = 0.0;
        double hissatu_bougyo_bairitu = 0.0;
        double hissatu_kakuritu = 0.0;
        int ibeido_mushi = 0;
        private void calc_damage()
        {
            if (isLoad)
            {
                return;
            }

            // ここに計算ロジックを追加
            double kougeki_bairitu = 1.0, bougyo_bairitu = 1.0;
            double temp_kougeki_bairitu = 1.0, temp_bougyo_bairitu = 1.0;
            double enermy_bougyo = 1.0;
            hissatu_kougeki_bairitu = 1.0;
            hissatu_bougyo_bairitu = 1.0;
            hissatu_kakuritu = 0.0;
            shuyaku_flag = false;
            enkaku_flag = false;
            ibeido_mushi = 0;

            DebugTextBox_damage.Text = "====ダメージ計算====\n";

            //スキル値の取得
            double enemy_tokkou_value = 0.0;
            double enemy_kenshu_value = 0.0;
            double enemy_mukei_value = 0.0;
            double enemy_shinma_value = 0.0;
            double enemy_jigenshoheki_value = 0.0;
            double enemy_hissatu_taisei_value = 0.0;
            double enemy_chimei_taisei_value = 0.0;
            double enemy_tousho_value = 0.0;
            double enemy_seisen_value = 0.0;
            double enemy_ibeido_value = 0.0;
            double enemy_ukenagashi_value = 0.0;
            double enemy_block_value = 0.0;
            int enemy_waisho_value = 0;
            int enemy_sikou_value = 0;
            int enemy_unmei_value = 0;
            if (double.TryParse(tokkobogyo_box.Text, out enemy_tokkou_value))
            {
                // enermy_bougyo に変換された値が入る
            }
            else
            {
                // 変換できなかった場合はデフォルト値（1.0）のまま
            }
            if (double.TryParse(kenshutaiku_box.Text, out enemy_kenshu_value))
            {
                // enermy_bougyo に変換された値が入る
            }
            else
            {
                // 変換できなかった場合はデフォルト値（1.0）のまま
            }
            if (double.TryParse(mukeitaiku_box.Text, out enemy_mukei_value))
            {
                // enermy_bougyo に変換された値が入る
            }
            else
            {
                // 変換できなかった場合はデフォルト値（1.0）のまま
            }
            if (double.TryParse(shinmataiku_box.Text, out enemy_shinma_value))
            {
                // enermy_bougyo に変換された値が入る
            }
            else
            {
                // 変換できなかった場合はデフォルト値（1.0）のまま
            }
            if (double.TryParse(jigenshoheki_box.Text, out enemy_jigenshoheki_value))
            {
                // enermy_bougyo に変換された値が入る
            }
            else
            {
                // 変換できなかった場合はデフォルト値（1.0）のまま
            }
            if (double.TryParse(hissatutaisei_box.Text, out enemy_hissatu_taisei_value))
            {
                // enermy_bougyo に変換された値が入る
            }
            else
            {
                // 変換できなかった場合はデフォルト値（1.0）のまま
            }
            if (double.TryParse(chimeitaisei_box.Text, out enemy_chimei_taisei_value))
            {
                // enermy_bougyo に変換された値が入る
            }
            else
            {
                // 変換できなかった場合はデフォルト値（1.0）のまま
            }
            if (double.TryParse(tosho_box.Text, out enemy_tousho_value))
            {
                // enermy_bougyo に変換された値が入る
            }
            else
            {
                // 変換できなかった場合はデフォルト値（1.0）のまま
            }
            if (double.TryParse(seisen_box.Text, out enemy_seisen_value))
            {
                // enermy_bougyo に変換された値が入る
            }
            else
            {
                // 変換できなかった場合はデフォルト値（1.0）のまま
            }
            if (double.TryParse(ibeido_box.Text, out enemy_ibeido_value))
            {
                // enermy_bougyo に変換された値が入る
            }
            else
            {
                // 変換できなかった場合はデフォルト値（1.0）のまま
            }
            if (double.TryParse(ukenagashi_box.Text, out enemy_ukenagashi_value))
            {
                // enermy_bouga に変換された値が入る
            }
            else
            {
                // 変換できなかった場合はデフォルト値（1.0）のまま
            }
            if (double.TryParse(block_box.Text, out enemy_block_value))
            {
                // enermy_bougyo に変換された値が入る
            }
            else
            {
                // 変換できなかった場合はデフォルト値（1.0）のまま
            }
            if (int.TryParse(enemy_unmei_box.Text, out enemy_unmei_value))
            {
                // enermy_bougyo に変換された値が入る
            }
            else
            {
                // 変換できなかった場合はデフォルト値（1.0）のまま
            }

            //ステータス値の取得
            if (double.TryParse(enemy_bougyo_box.Text, out enermy_bougyo))
            {
            }
            else
            {
                // 変換できなかった場合はデフォルト値（1.0）のまま
            }
            if (int.TryParse(enemy_sikou_box.Text, out enemy_sikou_value))
            {
            }
            else
            {
                // 変換できなかった場合はデフォルト値（1.0）のまま
            }
            if (int.TryParse(enemy_waisho_box.Text, out enemy_waisho_value))
            {
            }


            double player_kougeki = 1.0;
            if (double.TryParse(kougekibox.Text, out player_kougeki))
            {
                // player_kougeki に変換された値が入る
            }
            else
            {
                // 変換できなかった場合はデフォルト値（1.0）のまま
            }

            //運命の輪、運命改変の値を取得
            int unmei_value = 0;
            unmei_value = get_unmei_value();
            //防御倍率の計算
            bool use_kenshu = false;
            shinma_flag = false;
            //堅守の場合上限92
            if (enemy_kenshu_value > 92)
            {
                enemy_kenshu_value = 92;
            }
            //神魔体躯の場合上限98
            if (enemy_shinma_value > 98)
            {
                enemy_shinma_value = 98;
            }

            //神魔を使うか堅守を使うか。同値なら神魔体躯を使う
            if (enemy_kenshu_value > enemy_shinma_value)
            {
                use_kenshu = true;
            }
            else
            {
                use_kenshu = false;
            }

            //特攻防御はしんまと加算
            enemy_tokkou_value += enemy_shinma_value;
            //最大100
            if (enemy_tokkou_value > 100) enemy_tokkou_value = 100;
            double status_bairitu = 1.0;
            //運命値を最初に取得しておく
            double bairitu = 1.0;
            double p_status_bairitu = 1.0;
            double kougeki_kaisu = 1.0;
            double seisen_bairitu = 1.0;
            double enemy_seisen_bairitu = 1.0;
            double toushou_bairitu = 1.0;
            double enemy_toushou_bairitu = 1.0;

            unmei_value = get_unmei_value();

            //まずはステータスが変化する倍率を計算。攻撃のステータス値にかかる。
            p_status_bairitu *= get_kasoku_value();
            p_status_bairitu *= get_kyousenshi_value();
            p_status_bairitu *= get_kyohon_value();
            DebugTextBox_damage.Text += "ステータス倍率:" + p_status_bairitu + "\n";
            player_kougeki = player_kougeki * p_status_bairitu;

            //次に確率追撃の計算
            kougeki_kaisu = get_kakuritu_tuigeki_value(unmei_value);

            //次にほかの倍率を計算
            //巨人狩りは神魔体躯がない場合のみではないらしい
            //if (enemy_shinma_value == 0)
            bairitu *= get_kyojingari_value();
            bairitu *= get_sippuu_value(kougeki_kaisu * 100, unmei_value);
            bairitu *= get_kenkon_value(kougeki_kaisu * 100, unmei_value);
            bairitu *= get_mukougui_value(unmei_value);
            bairitu *= get_renkei_value();
            bairitu *= get_shuyaku_value();
            //bairitu *= get_seisen_value();
            //bairitu *= get_toushou_value();

            seisen_bairitu = get_seisen_value();
            toushou_bairitu = get_toushou_value();

            bairitu *= get_shinkaku_value();
            bairitu *= get_tadan_value();
            bairitu *= get_zenryoku_value();
            DebugTextBox_damage.Text += "ダメージ倍率:" + bairitu + "\n";

            //カブト次元の取得
            double kabuto_value = get_kabutowari_value();
            double jigen_value = get_jigenzangeki_value();

            //減算と上限の計算
            kabuto_value -= enemy_ukenagashi_value;
            jigen_value -= enemy_jigenshoheki_value;
            if (kabuto_value < 0) kabuto_value = 0;
            if (kabuto_value > 75) kabuto_value = 75;
            if (jigen_value < 0) jigen_value = 0;
            if (jigen_value > 75) jigen_value = 75;

            //クリティカルの計算
            double critical_kakuritu = 0.0;
            double hissatu_zouka_value = 0.0;
            double hissatu_param_kakuritu = 0.0;
            double chimei_value = 0.0;
            int hissatu_jougen = 50;
            hissatu_zouka_value = get_hissatu_value(unmei_value);
            chimei_value = get_chimei_value();
            hissatu_param_kakuritu = hissatu_from_param(p_status_bairitu);
            if (hissatu_zouka_value != 0.0)
            {
                hissatu_jougen = 75;
            }
            //確率の計算と上限の計算
            if (enemy_hissatu_taisei_value > 0)
                enemy_hissatu_taisei_value = enemy_hissatu_taisei_value + enemy_unmei_value;
            critical_kakuritu = hissatu_zouka_value + hissatu_param_kakuritu - enemy_hissatu_taisei_value;

            //ここは無形と心核の計算後に無形の上限で切り捨てるところ
            //心核は無形の必殺上限をどうにかする機能はない模様
            //運命値も無視
            int bougyo_mukei_sinkaku = 0;
            if (enemy_mukei_value > 0)
                bougyo_mukei_sinkaku = (int)(enemy_mukei_value) ;
            if (enemy_mukei_value > 60) enemy_mukei_value = 60;

            //必殺の上限値から無形体躯の値を減算
            hissatu_jougen -= (int)enemy_mukei_value;
            if (hissatu_jougen < 5) hissatu_jougen = 5;
            if (critical_kakuritu > hissatu_jougen)
                critical_kakuritu = hissatu_jougen;
            if (hissatu_param_kakuritu > hissatu_jougen)
                hissatu_param_kakuritu = hissatu_jougen;
            if (critical_kakuritu < 5)
                critical_kakuritu = 5;
            //クリティカルと致命の倍率の計算
            double critical_damage_bairitu = hissatu_param_kakuritu + chimei_value - enemy_chimei_taisei_value;
            if (critical_damage_bairitu < 50)
                critical_damage_bairitu = 50;
            critical_damage_bairitu = (100.0 + critical_damage_bairitu) / 100.0;
            double critical_damage_bairitu_with_kakuritu = critical_damage_bairitu * critical_kakuritu / 100.0 + 1.0 * (100.0 - critical_kakuritu) / 100.0;
            DebugTextBox_damage.Text += "クリティカルダメージ倍率:" + critical_damage_bairitu + "\n";
            DebugTextBox_damage.Text += "クリティカルダメージ確率:" + critical_kakuritu + "\n";
            DebugTextBox_damage.Text += "致命値:" + chimei_value + "\n";
            DebugTextBox_damage.Text += "最終クリダメ倍率:" + critical_damage_bairitu_with_kakuritu + "\n";
            //クリティカル計算が終わったら無形の倍率を減らす
            if (get_shinkaku_value() != 1.0)
            {
                enemy_mukei_value -= 25;
            }
            if (enemy_mukei_value < 0) enemy_mukei_value = 0;


            //城壁による防御計算
            double joheki = 0.0;
            if (double.TryParse(enemy_joheki_box.Text, out joheki))
            {

            }
            else
            {
            }
            double joheki_bougyo = Math.Sqrt(joheki);

            //神魔がある場合巨人狩りがあっても無効
            if (use_kenshu == true && enemy_shinma_value == 0 && get_kyojingari_value() != 1.0)
            {
                enemy_kenshu_value -= 10;
                if (enemy_kenshu_value < 0) enemy_kenshu_value = 0;
            }
            //堅守、無形、専守の値で最終ダメージ減算用
            double senshu = 1.0;
            if (enemy_senshu_flag == true)
                senshu = 0.5;
            double bougyo_skill_bairitu = 1.0;
            if (use_kenshu)
                bougyo_skill_bairitu = senshu * ((100.0 - enemy_kenshu_value) / 100.0) * ((100.0 - enemy_mukei_value) / 100.0);
            else
                bougyo_skill_bairitu = senshu * ((100.0 - enemy_shinma_value) / 100.0) * ((100.0 - enemy_mukei_value) / 100.0);

            //特攻倍率の計算
            String shuzoku_str = enemy_shuzoku_box.Text;
            Dictionary<string, int> parse_enemy_shuzoku = new Dictionary<string, int>();
            for (int i = 0; i < shuzoku_str.Length; i++)
            {
                //parse_enemy_shuzokuに存在する場合+1する
                if (parse_enemy_shuzoku.ContainsKey(shuzoku_str[i].ToString()))
                {
                    parse_enemy_shuzoku[shuzoku_str[i].ToString()]++;
                }
                else
                {
                    parse_enemy_shuzoku.Add(shuzoku_str[i].ToString(), 1);
                }
            }
            String player_tokko_str = tokko_box.Text;
            //一文字ずつなめて確認、あれば倍率を+1.0
            double tokko_bairitu = 1.0;
            for (int i = 0; i < player_tokko_str.Length; i++)
            {
                if (parse_enemy_shuzoku.ContainsKey(player_tokko_str[i].ToString()))
                {
                    //倍率を+1.0
                    tokko_bairitu += 1.0;
                }
                else if (player_tokko_str[i].ToString() == "全")
                {
                    tokko_bairitu += parse_enemy_shuzoku.Count;
                }
            }
            tokko_bairitu = 1.0 + (tokko_bairitu - 1.0) * (100 - enemy_tokkou_value) / 100.0;
            DebugTextBox_damage.Text += "特攻倍率:" + tokko_bairitu + "\n";
            int block_value = 0;
            double block_bairitu = 1.0;
            if (block_box.Text != "")
            {
                if (int.TryParse(block_box.Text, out block_value))
                {
                }
            }
            //ブロック値は次元障壁、至高の盾と加算
            block_value += (int)enemy_jigenshoheki_value;
            block_value += (int)enemy_sikou_value;
            if (block_value > 0)
                block_value = block_value + enemy_unmei_value;
            if (get_shuyaku_value() != 1.0)
            {
                //集約攻撃が入っている場合は、ブロックは無視する
            }
            else
            {
                if (block_value > 0)
                {
                    //ブロックが入っている場合は、ブロック値を使う
                    if (block_value > 75) block_value = 75;
                    block_bairitu = 1.0 * (100.0 - block_value) / 100.0 + 0.25 * (block_value) / 100.0;
                    DebugTextBox_damage.Text += "ブロック倍率:" + block_bairitu + "\n";
                }
                else
                {
                    //ブロックが入っていない場合は1.0
                    block_bairitu = 1.0;
                }

            }
            double damage_value = 0.0;

            //イベイド含めた計算
            int ibeido_value = 0;
            if (ibeido_box.Text != "")
            {
                if (int.TryParse(ibeido_box.Text, out ibeido_value))
                {
                    // ibeido_value に変換された値が入る
                }
            }
            else
            {
                // 変換できなかった場合はデフォルト値（0）のまま
            }
            //イベイドと次元障壁と至高の盾加算後に値があるならさらに運命値、攻撃側のイベイド無視と減算
            ibeido_value += (int)enemy_jigenshoheki_value;
            ibeido_value += enemy_sikou_value;
            if (ibeido_value > 0)
                ibeido_value = ibeido_value + enemy_unmei_value;
            ibeido_mushi = ibeido_mushi_calc(unmei_value);
            ibeido_value -= ibeido_mushi;
            if (ibeido_value < 0) ibeido_value = 0;
            if (ibeido_value > 100) ibeido_value = 100;
            enkaku_flag = enkaku_check();
            if (enkaku_flag == true)
                DebugTextBox_damage.Text += "イベイド確率:" + ibeido_value + "\n";
            else
            {
                DebugTextBox_damage.Text += "イベイド確率:0\n";
            }
            //カブト割、次元斬撃、致命必殺の計算
            double kabuto_jigen_chimei_bairitu = 1.0;
            kabuto_jigen_chimei_bairitu = ((100.0 - kabuto_value) / 100.0) * (100.0 - jigen_value) / (100.0);
            double kabuto_jigen_chimei_bairitu_hissatu_force = kabuto_jigen_chimei_bairitu * 0.75;
            double kabuto_jigen_chimei_bairitu_hissatu_kakuritu = kabuto_jigen_chimei_bairitu * (critical_kakuritu);
            double kabuto_jigen_chimei_bairitu_hissatu_nashi = kabuto_jigen_chimei_bairitu;
            DebugTextBox_damage.Text += "カブトと次元の減算値:" + kabuto_jigen_chimei_bairitu + "\n";


            //凍傷と聖戦の計算
            int temp_toushou_bairitu = (int)(toushou_bairitu * 100.0 - 100) - (int)enemy_tousho_value;
            int temp_seisen_bairitu = (int)(seisen_bairitu * 100.0 - 100) - (int)enemy_seisen_value;
            if (temp_toushou_bairitu > 30) temp_toushou_bairitu = 30;
            if (temp_toushou_bairitu < -30) temp_toushou_bairitu = -30;
            if (temp_seisen_bairitu > 30) temp_seisen_bairitu = 30;
            if (temp_seisen_bairitu < -30) temp_seisen_bairitu = -30;

            if (temp_seisen_bairitu > 0)
            {
                seisen_bairitu = (100.0 + temp_seisen_bairitu) / 100.0;
                enemy_seisen_bairitu = 1.0;
            }
            else if (temp_seisen_bairitu < 0)
            {
                seisen_bairitu = 1.0;
                enemy_seisen_bairitu = (100.0 + temp_seisen_bairitu) / 100.0;
            }
            else
            {
                seisen_bairitu = 1.0;
                enemy_seisen_bairitu = 1.0;
            }

            if (temp_toushou_bairitu < 0)
            {
                string p_shuzoku = shuzoku_box.Text;
                toushou_bairitu = 1.0;
                if (Regex.IsMatch(p_shuzoku, "炎|氷|星"))
                {
                    enemy_toushou_bairitu = 1.0;
                }
                else
                {
                    enemy_toushou_bairitu = (100.0 + temp_toushou_bairitu) / 100.0;
                }
            }
            else if (temp_toushou_bairitu > 0)
            {
                if (Regex.IsMatch(shuzoku_str, "炎|氷|星"))
                {
                    toushou_bairitu = 1.0;
                }
                else
                {
                    toushou_bairitu = (100.0 + temp_toushou_bairitu) / 100.0;
                }
                enemy_toushou_bairitu = 1.0;
            }
            else
            {
                toushou_bairitu = 1.0;
                enemy_toushou_bairitu = 1.0;
            }
            DebugTextBox_damage.Text += "聖戦補正値:" + temp_seisen_bairitu + "\n";
            DebugTextBox_damage.Text += "凍傷補正値:" + temp_toushou_bairitu + "\n";
            double enemy_bougyo_value = 1;
            kougeki_bairitu = bairitu * toushou_bairitu * seisen_bairitu;
            bougyo_skill_bairitu = bougyo_skill_bairitu * enemy_toushou_bairitu * enemy_seisen_bairitu;

            //矮小値の計算
            if (enemy_waisho_value > 0)
                enemy_waisho_value = enemy_waisho_value + enemy_unmei_value;

            if (enemy_waisho_value > 80)
                enemy_waisho_value = 80;

            double enemy_waisho_bairitu;
            double enemy_waisho_kakuritu = 0.0;
            enemy_waisho_bairitu = (100.0 + enemy_waisho_value) / 100.0;
            enemy_waisho_kakuritu = (100.0 - enemy_waisho_value) / 100.0; //そのままダメージ期待値にかけてしまう
            DebugTextBox_damage.Text += "矮小値:" + enemy_waisho_value + "\n";
            bougyo_skill_bairitu *= enemy_waisho_bairitu;

            //職業とスタンスの計算
            double shokugyo_bairitu = calc_shokugyo_bairitu();
            double stance_bairitu = calc_stance_bairitu();
            if (shokugyo_bairitu > 1.0)
            {
                DebugTextBox_damage.Text += "職業補正:" + shokugyo_bairitu + "\n";
            }
            if (stance_bairitu > 1.0)
            {
                DebugTextBox_damage.Text += "スタンス補正:" + stance_bairitu + "\n";
            }
            kougeki_bairitu = bairitu * shokugyo_bairitu * stance_bairitu;

            DebugTextBox_damage.Text += "防御スキル倍率:" + bougyo_skill_bairitu + "\n";

            if (double.TryParse(enemy_bougyo_box.Text, out enemy_bougyo_value))
            {
            }
            else
            {
            }
            /*防御値の最小値は1*/
            if (get_chimei_value() != 0.0)
            {
                kabuto_jigen_chimei_bairitu = ((100 - kabuto_value) / 100) * ((100 - jigen_value) / 100);

            }
            //遠隔攻撃がオンのとき、イベイドの確率で0になる。

            //クリティカルなしのダメージ
            temp_bougyo_bairitu = kabuto_jigen_chimei_bairitu;
            double enemy_bougyo_value_temp = enemy_bougyo_value * temp_bougyo_bairitu;
            if (enemy_bougyo_value_temp < 1.0)
                enemy_bougyo_value_temp = 1.0;
            double critical_nasi_damage = ((2.0 * player_kougeki * status_bairitu + 5.0) * Math.Sqrt(double.Parse(hpbox.Text)) / (2.0 + enemy_bougyo_value_temp + joheki_bougyo)) * tokko_bairitu * bougyo_skill_bairitu * block_bairitu * kougeki_bairitu;
            //クリティカルありのダメージ計算
            if (get_chimei_value() != 0.0)
            {
                temp_bougyo_bairitu = kabuto_jigen_chimei_bairitu * 0.75;
            }
            temp_bougyo_bairitu = kabuto_jigen_chimei_bairitu;
            enemy_bougyo_value_temp = enemy_bougyo_value * temp_bougyo_bairitu;
            if (enemy_bougyo_value_temp < 1.0)
                enemy_bougyo_value_temp = 1.0;
            double critical_ari_damage = ((2.0 * player_kougeki * status_bairitu + 5.0) * Math.Sqrt(double.Parse(hpbox.Text)) / (2.0 + enemy_bougyo_value_temp + joheki_bougyo)) * tokko_bairitu * bougyo_skill_bairitu * block_bairitu * critical_damage_bairitu * kougeki_bairitu;
            //統合する
            damage_value = critical_ari_damage * (critical_kakuritu) / 100 + critical_nasi_damage * (100 - critical_kakuritu) / 100;

            //イベイドの確率で最大0になる
            if (enkaku_flag == true && ibeido_value != 0)
            {
                damage_value = (100 - ibeido_value) * damage_value / 100.0;
            }

            damage_value = damage_value * enemy_waisho_kakuritu;

            double tuigeki = 0;
            if (get_kenkon_value(kougeki_kaisu, unmei_value) != 1.0)
            {
                tuigeki = 1.0;
            }
            else
            {
                tuigeki = 1.0 + kougeki_kaisu;
            }


            damage_kaisu_kitaiti_box.Text = tuigeki.ToString();
            damage_kitaiti_box.Text = Math.Truncate(damage_value).ToString();

            //ダメージ最大値の計算。ブロック無視、イベイド無視、必殺必ず発生
            double kougeki_temp = (2.0 * player_kougeki * status_bairitu * hissatu_kougeki_bairitu + 5.0) * Math.Sqrt(double.Parse(hpbox.Text)) * tokko_bairitu * kougeki_bairitu * critical_damage_bairitu;
            temp_bougyo_bairitu = kabuto_jigen_chimei_bairitu;
            if (get_chimei_value() != 0.0)
            {
                temp_bougyo_bairitu = kabuto_jigen_chimei_bairitu * 0.75;
            }
            enemy_bougyo_value_temp = enemy_bougyo_value * temp_bougyo_bairitu;
            if (enemy_bougyo_value_temp < 1.0)
                enemy_bougyo_value_temp = 1.0;
            double bougyo_temp = (2.0 + enemy_bougyo_value_temp + joheki_bougyo);
            double max_damage_value = kougeki_temp / bougyo_temp * bougyo_skill_bairitu;
            damage_kaisu_max_box.Text = tuigeki.ToString();
            damage_max_box.Text = Math.Truncate(max_damage_value).ToString();

            //ダメージ最小値の計算。ブロック必ず発動、必殺なし、イベイドは無視
            kougeki_temp = (2.0 * player_kougeki * status_bairitu + 5.0) * Math.Sqrt(double.Parse(hpbox.Text)) * tokko_bairitu * kougeki_bairitu;
            enemy_bougyo_value_temp = enemy_bougyo_value * temp_bougyo_bairitu;
            if (enemy_bougyo_value_temp < 1.0)
                enemy_bougyo_value_temp = 1.0;
            bougyo_temp = (2.0 + enemy_bougyo_value_temp + joheki_bougyo);
            double min_damage_value;
            if (block_bairitu != 1.0)
            {
                min_damage_value = (kougeki_temp / bougyo_temp) * bougyo_skill_bairitu * 0.25;
            }
            else
            {
                min_damage_value = kougeki_temp / bougyo_temp * bougyo_skill_bairitu;
            }
            damage_kaisu_min_box.Text = tuigeki.ToString();
            damage_min_box.Text = Math.Truncate(min_damage_value).ToString();


        }

        private void calc_final_attack_mag()
        {
            if (isLoad)
            {
                return;
            }

            double bairitu = 1.0;
            double status_bairitu = 1.0;
            double kougeki_kaisu = 1.0;
            int unmei_value = 0;
            DebugTextBox.Text = "====倍率計算====\n";
            //運命値を最初に取得しておく
            unmei_value = get_unmei_value();
            if (unmei_value != 0)
            {
                DebugTextBox.Text += "運命値:" + unmei_value + "\n";
            }
            //まずはステータスが変化する倍率を計算。必殺のダメージにかかわる。
            bairitu *= get_kasoku_value();
            if (get_kasoku_value() != 1.0)
            {
                DebugTextBox.Text += "加速倍率:" + get_kasoku_value() * get_kasoku_value() + "\n";
            }
            bairitu *= get_kyousenshi_value();
            if (get_kyousenshi_value() != 1.0)
            {
                DebugTextBox.Text += "狂戦士倍率:" + get_kyousenshi_value() + "\n";
            }
            bairitu *= get_kyohon_value();
            if (get_kyohon_value() != 1.0)
            {
                DebugTextBox.Text += "狂奔倍率:" + get_kyohon_value() + "\n";
            }
            status_bairitu = bairitu;

            //次に確率追撃の計算
            kougeki_kaisu = get_kakuritu_tuigeki_value(unmei_value);
            if (kougeki_kaisu != 1.0)
            {
                DebugTextBox.Text += "確率追撃回数:" + kougeki_kaisu + "\n";
            }

            //次にほかの倍率を計算
            bairitu *= get_kasoku_value();
            bairitu *= get_kyojingari_value();
            if (get_kyojingari_value() != 1.0)
            {
                DebugTextBox.Text += "巨人狩り倍率:" + get_kyojingari_value() + "\n";
            }
            bairitu *= get_sippuu_value(kougeki_kaisu * 100, unmei_value);
            if (get_sippuu_value(kougeki_kaisu * 100, unmei_value) != 1.0)
            {
                DebugTextBox.Text += "疾風倍率:" + get_sippuu_value(kougeki_kaisu * 100, unmei_value) + "\n";
            }
            bairitu *= get_kenkon_value(kougeki_kaisu * 100, unmei_value);
            if (get_kenkon_value(kougeki_kaisu * 100, unmei_value) != 1.0)
            {
                DebugTextBox.Text += "乾坤倍率:" + get_kenkon_value(kougeki_kaisu * 100, unmei_value) + "\n";
            }
            bairitu *= get_mukougui_value(unmei_value);
            if (get_mukougui_value(unmei_value) != 1.0)
            {
                DebugTextBox.Text += "無効食い倍率:" + get_mukougui_value(unmei_value) + "\n";
            }
            bairitu *= get_renkei_value();
            if (get_renkei_value() != 1.0)
            {
                DebugTextBox.Text += "連携倍率:" + get_renkei_value() + "\n";
            }
            bairitu *= get_shuyaku_value();
            if (get_shuyaku_value() != 1.0)
            {
                DebugTextBox.Text += "集約倍率:" + get_shuyaku_value() + "\n";
            }
            bairitu *= get_seisen_value();
            if (get_seisen_value() != 1.0)
            {
                DebugTextBox.Text += "聖戦倍率:" + get_seisen_value() + "\n";
            }
            bairitu *= get_shinkaku_value();
            if (get_shinkaku_value() != 1.0)
            {
                DebugTextBox.Text += "心核倍率:" + get_shinkaku_value() + "\n";
            }
            bairitu *= get_tadan_value();
            if (get_tadan_value() != 1.0)
            {
                DebugTextBox.Text += "多段倍率:" + get_tadan_value() + "\n";
            }
            bairitu *= get_toushou_value();
            if (get_toushou_value() != 1.0)
            {
                DebugTextBox.Text += "凍傷倍率:" + get_toushou_value() + "\n";
            }
            bairitu *= get_zenryoku_value();
            if (get_zenryoku_value() != 1.0)
            {
                DebugTextBox.Text += "全力倍率:" + get_zenryoku_value() + "\n";
            }
            //カブト次元の計算
            double kabuto_value = get_kabutowari_value();
            if (kabuto_value > 75) kabuto_value = 75;
            if (100.0 / (100 - kabuto_value) != 1.0)
            {
                DebugTextBox.Text += "カブト倍率:" + 100.0 / (100 - kabuto_value) + "\n";
            }
            bairitu *= 100.0 / (100 - kabuto_value);
            double jigen_value = get_jigenzangeki_value();
            if (jigen_value > 75) jigen_value = 75;
            if (jigen_value != 0.0)
            {
                DebugTextBox.Text += "次元倍率:" + 100.0 / (100 - jigen_value) + "\n";
            }
            bairitu *= 100.0 / (100 - jigen_value);

            //クリティカルの計算
            double critical_kakuritu = 0.0;
            double hissatu_zouka_value = 0.0;
            double hissatu_param_kakuritu = 0.0;
            double chimei_value = 0.0;
            int hissatu_jougen = 50;
            hissatu_zouka_value = get_hissatu_value(unmei_value);
            chimei_value = get_chimei_value();
            hissatu_param_kakuritu = hissatu_from_param(status_bairitu);

            if (hissatu_zouka_value != 0.0)
            {
                hissatu_jougen = 75;
            }
            //確率の計算と上限の計算
            critical_kakuritu = hissatu_zouka_value + hissatu_param_kakuritu;
            if (critical_kakuritu > hissatu_jougen)
                critical_kakuritu = hissatu_jougen;
            if (hissatu_param_kakuritu > hissatu_jougen)
                hissatu_param_kakuritu = hissatu_jougen;
            //クリティカルと致命の倍率の計算

            double critical_damage_bairitu = hissatu_param_kakuritu + chimei_value;
            if (critical_damage_bairitu < 50)
                critical_damage_bairitu = 50;
            critical_damage_bairitu = 1.0 + critical_damage_bairitu / 100;
            //critical_damage_bairitu = (100.0 + critical_damage_bairitu) / 100.0 * (100/(100-critical_kakuritu));
            double critical_damage_bairitu_with_kakuritu = 1.0;
            if (get_chimei_value() == 0)
            {
                critical_damage_bairitu_with_kakuritu = critical_damage_bairitu * critical_kakuritu / 100 + 1.0 * (100.0 - critical_kakuritu) / 100.0;
            }
            else
            {
                critical_damage_bairitu_with_kakuritu = 1.25 * critical_damage_bairitu * critical_kakuritu / 100 + 1.0 * (100.0 - critical_kakuritu) / 100.0;
            }
            DebugTextBox.Text += "クリティカルダメージ倍率:" + Math.Round(critical_damage_bairitu, 2) + "\n";
            DebugTextBox.Text += "クリティカルダメージ確率:" + Math.Round(critical_kakuritu, 2) + "(" + hissatu_param_kakuritu + ")" + "\n";
            DebugTextBox.Text += "致命値:" + chimei_value + "\n";
            DebugTextBox.Text += "総合クリティカルダメージ倍率:" + Math.Round(critical_damage_bairitu_with_kakuritu, 2) + "\n";
            bairitu *= critical_damage_bairitu_with_kakuritu;

            //種族特攻の計算
            String shuzoku_str = "";
            if (enemy_shuzoku_box != null && !string.IsNullOrEmpty(enemy_shuzoku_box.Text))
                shuzoku_str = enemy_shuzoku_box.Text;
            Dictionary<string, int> parse_enemy_shuzoku = new Dictionary<string, int>();
            for (int i = 0; i < shuzoku_str.Length; i++)
            {
                //parse_enemy_shuzokuに存在する場合+1する
                if (parse_enemy_shuzoku.ContainsKey(shuzoku_str[i].ToString()))
                {
                    parse_enemy_shuzoku[shuzoku_str[i].ToString()]++;
                }
                else
                {
                    parse_enemy_shuzoku.Add(shuzoku_str[i].ToString(), 1);
                }
            }
            String player_tokko_str = tokko_box.Text;
            //一文字ずつなめて確認、あれば倍率を+1.0
            double tokko_bairitu = 1.0;
            for (int i = 0; i < player_tokko_str.Length; i++)
            {
                if (parse_enemy_shuzoku.ContainsKey(player_tokko_str[i].ToString()))
                {
                    //倍率を+1.0
                    tokko_bairitu += 1.0;
                }
                else if (player_tokko_str[i].ToString() == "全")
                {
                    tokko_bairitu += parse_enemy_shuzoku.Count;
                }
            }
            DebugTextBox.Text += "特攻倍率: " + tokko_bairitu + "\n";
            double tuigeki = 1.0;
            if (get_kenkon_value(kougeki_kaisu, unmei_value) != 1.0)
            {
                tuigeki = 1.0;
            }
            else
            {
                tuigeki = 1.0 + kougeki_kaisu;
            }
            bairitu *= (tokko_bairitu);
            bairitu_kaisu_kitaiti_box.Text = tuigeki.ToString();
            bairitu_kitaiti_box.Text = (Math.Truncate(bairitu * 100) / 100).ToString();

        }
        Boolean leader_flag;
        // 修正内容: Resync_finalskil メソッドの構文エラーと Dictionary の使い方の誤りを修正
        private void Resync_finalskil()
        {
            if (isLoad)
            {
                return;
            }
            // Dictionary の宣言と初期化
            Dictionary<string, int> finalskills = new Dictionary<string, int>();

            // TextBox コントロールは MainWindow のフィールドとして宣言されているため、アクセス可能
            // Dictionary に値を追加する場合は Add メソッドを使う

            for (int j = 1; j <= 8; j++)
            {
                var textBox = (TextBox)this.FindName($"passive{j}");
                if (textBox != null)
                {
                    double value;
                    var valueBox = (TextBox)this.FindName($"passive{j}_fig");
                    if (double.TryParse(valueBox.Text, out value))
                    {
                        if (!finalskills.ContainsKey(textBox.Text))
                        {
                            finalskills.Add(textBox.Text, int.Parse(valueBox.Text));
                        }
                        else
                        {
                            finalskills[textBox.Text] += int.Parse(valueBox.Text);
                        }
                    }
                    else
                    {

                    }
                }
            }
            if (leader_flag == true)
            {
                if (leader1_fig.Text != "")
                {
                    if (finalskills.ContainsKey(leader1.Text))
                    {
                        if (leader1_fig.Text != "")
                            finalskills[leader1.Text] = int.Parse(leader1_fig.Text) + finalskills[leader1.Text];
                    }
                    else
                    {
                        if (leader1_fig.Text != "")
                            finalskills.Add(leader1.Text, int.Parse(leader1_fig.Text));
                    }
                }
                else
                {
                    finalskills.Add(leader1.Text, 0);
                }
                if (leader2_fig.Text != "")
                {
                    if (finalskills.ContainsKey(leader2.Text))
                    {
                        if (leader2_fig.Text != "")
                            finalskills[leader2.Text] = int.Parse(leader2_fig.Text) + finalskills[leader2.Text];
                    }
                    else
                    {
                        if (leader2_fig.Text != "")
                            finalskills.Add(leader2.Text, int.Parse(leader2_fig.Text));
                    }
                }
                else
                {
                    if (!finalskills.ContainsKey(leader2.Text))
                    {

                        finalskills.Add(leader2.Text, 0);
                    }
                }
            }
            string temp_tokko = "";
            if (shogo1.Text != "")
            {
                if (finalskills.ContainsKey(shogo1.Text))
                {
                    if (shogo1_fig.Text != "")
                        finalskills[shogo1.Text] = int.Parse(shogo1_fig.Text) + finalskills[shogo1.Text];
                }
                else
                {
                    if (shogo1_fig.Text != "")
                        finalskills.Add(shogo1.Text, int.Parse(shogo1_fig.Text));
                    else
                        finalskills.Add(shogo1.Text, 0);
                }
                temp_tokko = temp_tokko + shogo1_status.tokko;
            }
            if (shogo2.Text != "")
            {
                if (finalskills.ContainsKey(shogo2.Text))
                {
                    if (shogo2_fig.Text != "")
                        finalskills[shogo2.Text] = int.Parse(shogo2_fig.Text) + finalskills[shogo2.Text];
                }
                else
                {
                    if (shogo2_fig.Text != "")
                        finalskills.Add(shogo2.Text, int.Parse(shogo2_fig.Text));
                    else
                        finalskills.Add(shogo2.Text, 0);
                }
                temp_tokko = temp_tokko + shogo2_status.tokko;
            }
            string tokko, shuzoku = "";
            current_Character_Status.get_shuzoku_tokko(out shuzoku, out tokko);
            tokko_box.Text = tokko + temp_tokko;
            if (equipment1_skill1.Text != "")
            {
                if (finalskills.ContainsKey(equipment1_skill1.Text))
                {
                    equipment_tokko_plus(equipment1_skill1.Text, equipment1_skill1_fig.Text);
                    if (equipment1_skill1_fig.Text != "")
                        finalskills[equipment1_skill1.Text] = int.Parse(equipment1_skill1_fig.Text) + finalskills[equipment1_skill1.Text];
                }
                else
                {
                    equipment_tokko_plus(equipment1_skill1.Text, equipment1_skill1_fig.Text);
                    if (equipment1_skill1_fig.Text != "")
                        finalskills.Add(equipment1_skill1.Text, int.Parse(equipment1_skill1_fig.Text));
                    else
                        finalskills.Add(equipment1_skill1.Text, 0);

                }
            }
            if (equipment1_skill2.Text != "")
            {
                if (finalskills.ContainsKey(equipment1_skill2.Text))
                {
                    equipment_tokko_plus(equipment1_skill2.Text, equipment1_skill2_fig.Text);
                    if (equipment1_skill2_fig.Text != "")
                        finalskills[equipment1_skill2.Text] = int.Parse(equipment1_skill2_fig.Text) + finalskills[equipment1_skill2.Text];
                }
                else
                {
                    equipment_tokko_plus(equipment1_skill2.Text, equipment1_skill2_fig.Text);
                    if (equipment1_skill2_fig.Text != "")
                        finalskills.Add(equipment1_skill2.Text, int.Parse(equipment1_skill2_fig.Text));
                    else
                        finalskills.Add(equipment1_skill2.Text, 0);
                }
            }
            if (equipment2_skill1.Text != "")
            {
                if (finalskills.ContainsKey(equipment2_skill1.Text))
                {
                    equipment_tokko_plus(equipment2_skill1.Text, equipment2_skill1_fig.Text);
                    if (equipment2_skill1_fig.Text != "")
                        finalskills[equipment2_skill1.Text] = int.Parse(equipment2_skill1_fig.Text) + finalskills[equipment2_skill1.Text];
                }
                else
                {
                    equipment_tokko_plus(equipment2_skill1.Text, equipment2_skill1_fig.Text);
                    if (equipment2_skill1_fig.Text != "")
                        finalskills.Add(equipment2_skill1.Text, int.Parse(equipment2_skill1_fig.Text));
                    else finalskills.Add(equipment2_skill1.Text, 0);

                }
            }
            if (equipment2_skill2.Text != "")
            {
                if (finalskills.ContainsKey(equipment2_skill2.Text))
                {
                    equipment_tokko_plus(equipment2_skill2.Text, equipment2_skill2_fig.Text);
                    if (equipment2_skill2_fig.Text != "")
                        finalskills[equipment2_skill2.Text] = int.Parse(equipment2_skill2_fig.Text) + finalskills[equipment2_skill2.Text];
                }
                else
                {
                    equipment_tokko_plus(equipment2_skill2.Text, equipment2_skill2_fig.Text);
                    if (equipment2_skill2_fig.Text != "")
                        finalskills.Add(equipment2_skill2.Text, int.Parse(equipment2_skill2_fig.Text));
                    else
                        finalskills[equipment2_skill2.Text] = 0;
                }
            }
            if (ryoshoku_name.Text != "")
            {
                if (finalskills.ContainsKey(ryoshoku_name.Text))
                {
                    if (ryoshoku_fig_box.Text != "")
                        finalskills[ryoshoku_name.Text] = int.Parse(ryoshoku_fig_box.Text) + finalskills[ryoshoku_name.Text];
                }
                else
                {
                    if (ryoshoku_fig_box.Text != "")
                        finalskills.Add(ryoshoku_name.Text, int.Parse(ryoshoku_fig_box.Text));
                    else
                        finalskills[ryoshoku_name.Text] = 0;
                }
            }
            if (assist1_name.Text != "")
            {
                if (finalskills.ContainsKey(assist1_name.Text))
                {
                    if (assist1_fig_box.Text != "")
                        finalskills[assist1_name.Text] = int.Parse(assist1_fig_box.Text) + finalskills[assist1_name.Text];
                }
                else
                {
                    if (assist1_fig_box.Text != "")
                        finalskills.Add(assist1_name.Text, int.Parse(assist1_fig_box.Text));
                    else
                        finalskills[assist1_name.Text] = 0;
                }
            }
            if (assist2_name.Text != "")
            {
                if (finalskills.ContainsKey(assist2_name.Text))
                {
                    if (assist2_fig_box.Text != "")
                        finalskills[assist2_name.Text] = int.Parse(assist2_fig_box.Text) + finalskills[assist2_name.Text];
                }
                else
                {
                    if (assist2_fig_box.Text != "")
                        finalskills.Add(assist2_name.Text, int.Parse(assist2_fig_box.Text));
                    else
                        finalskills[assist2_name.Text] = 0;
                }
            }
            if (assist3_name.Text != "")
            {
                if (finalskills.ContainsKey(assist3_name.Text))
                {
                    if (assist3_fig_box.Text != "")
                        finalskills[assist3_name.Text] = int.Parse(assist3_fig_box.Text) + finalskills[assist3_name.Text];
                }
                else
                {
                    if (assist3_fig_box.Text != "")
                        finalskills.Add(assist3_name.Text, int.Parse(assist3_fig_box.Text));
                    else
                        finalskills[assist3_name.Text] = 0;
                }
            }
            if (unmei_box.Text != "")
            {
                if (finalskills.ContainsKey("運命改変"))
                {
                    if (unmei_box.Text != "")
                    {
                        int value;
                        if (int.TryParse(unmei_box.Text, out value))
                            finalskills["運命改変"] = value + finalskills["運命改変"];
                    }
                }
                else
                {
                    if (unmei_box.Text != "")
                    {
                        int value;
                        if (int.TryParse(unmei_box.Text, out value))
                            finalskills.Add("運命改変", value);
                    }
                }
            }


            int skill_count = finalskills.Count;
            int i = 0;

            final1.Text = null;
            final1_fig.Text = null;

            final2.Text = null;
            final2_fig.Text = null;

            final3.Text = null;
            final3_fig.Text = null;
            final4.Text = null;
            final4_fig.Text = null;

            final5.Text = null;
            final5_fig.Text = null;

            final6.Text = null;
            final6_fig.Text = null;
            final7.Text = null;
            final7_fig.Text = null;

            final8.Text = null;
            final8_fig.Text = null;

            final9.Text = null;
            final9_fig.Text = null;
            final10.Text = null;
            final10_fig.Text = null;

            final11.Text = null;
            final11_fig.Text = null;
            final12.Text = null;
            final12_fig.Text = null;
            final13.Text = null;
            final13_fig.Text = null;
            final14.Text = null;
            final14_fig.Text = null;
            final15.Text = null;
            final15_fig.Text = null;
            final16.Text = null;
            final16_fig.Text = null;
            final17.Text = null;
            final17_fig.Text = null;
            final18.Text = null;
            final18_fig.Text = null;
            final19.Text = null;
            final19_fig.Text = null;
            final20.Text = null;
            final20_fig.Text = null;
            final21.Text = null;
            final21_fig.Text = null;


            foreach (var key in finalskills)
            {
                if (i == 0)
                {
                    final1.Text = key.Key;
                    final1_fig.Text = key.Value.ToString();
                }
                else if (i == 1)
                {
                    final2.Text = key.Key;
                    final2_fig.Text = key.Value.ToString();
                }
                else if (i == 2)
                {
                    final3.Text = key.Key;
                    final3_fig.Text = key.Value.ToString();

                }
                else if (i == 3)
                {
                    final4.Text = key.Key;
                    final4_fig.Text = key.Value.ToString();
                }
                else if (i == 4)
                {
                    final5.Text = key.Key;
                    final5_fig.Text = key.Value.ToString();
                }
                else if (i == 5)
                {
                    final6.Text = key.Key;
                    final6_fig.Text = key.Value.ToString();
                }
                else if (i == 6)
                {
                    final7.Text = key.Key;
                    final7_fig.Text = key.Value.ToString();
                }
                else if (i == 7)
                {
                    final8.Text = key.Key;
                    final8_fig.Text = key.Value.ToString();
                }
                else if (i == 8)
                {
                    final9.Text = key.Key;
                    final9_fig.Text = key.Value.ToString();
                }
                else if (i == 9)
                {
                    final10.Text = key.Key;
                    final10_fig.Text = key.Value.ToString();
                }
                else if (i == 10)
                {
                    final11.Text = key.Key;
                    final11_fig.Text = key.Value.ToString();
                }
                else if (i == 11)
                {
                    final12.Text = key.Key;
                    final12_fig.Text = key.Value.ToString();
                }
                else if (i == 12)
                {
                    final13.Text = key.Key;
                    final13_fig.Text = key.Value.ToString();
                }
                else if (i == 13)
                {
                    final14.Text = key.Key;
                    final14_fig.Text = key.Value.ToString();
                }
                else if (i == 14)
                {
                    final15.Text = key.Key;
                    final15_fig.Text = key.Value.ToString();
                }
                else if (i == 15)
                {
                    final16.Text = key.Key;
                    final16_fig.Text = key.Value.ToString();
                }
                else if (i == 16)
                {
                    final17.Text = key.Key;
                    final17_fig.Text = key.Value.ToString();
                }
                else if (i == 17)
                {
                    final18.Text = key.Key;
                    final18_fig.Text = key.Value.ToString();
                }
                else if (i == 18)
                {
                    final19.Text = key.Key;
                    final19_fig.Text = key.Value.ToString();
                }
                else if (i == 19)
                {
                    final20.Text = key.Key;
                    final20_fig.Text = key.Value.ToString();
                }
                else if (i == 20)
                {
                    final21.Text = key.Key;
                    final21_fig.Text = key.Value.ToString();
                }

                i++;

            }
            status_calc_fix();
            // 必要に応じて他のスキルも追加可能
            // finalskills.Add(passive2.Text, passive2_fig.Text);
            // finalskills.Add(passive3.Text, passive3_fig.Text);
            // ... など
        }

        private void equipment_tokko_plus(string skillname, string skillvalue)
        {
            //装備のスキル名に特攻があれば、tokko_boxに追加する
            if (skillname.Contains("特攻") && !skillname.Contains("特攻防御"))
            {
                int temp_tokko_value = 0;
                string temp_tokko = skillname[0].ToString();
                //:で分割
                temp_tokko_value = int.Parse(skillvalue);
                for (int i = 0; i < temp_tokko_value; i++)
                {
                    tokko_box.Text += temp_tokko;
                }
            }
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // sender を ToggleButton (CheckBox) としてキャストし、IsChecked プロパティを使う
            var checkBox = sender as CheckBox;
            if (checkBox != null && checkBox.IsChecked == true)
            {
                leader_flag = true;
            }
            else
            {
                leader_flag = false;
            }
            Resync_finalskil();
            calc_final_attack_mag();
            calc_damage();
        }
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            leader_flag = false;
            Resync_finalskil();
            calc_final_attack_mag();
            calc_damage();
        }

        bool enemy_senshu_flag = false;

        private void CheckBox_senshu_Checked(object sender, RoutedEventArgs e)
        {
            // sender を ToggleButton (CheckBox) としてキャストし、IsChecked プロパティを使う
            var checkBox = sender as CheckBox;
            if (checkBox != null && checkBox.IsChecked == true)
            {
                enemy_senshu_flag = true;
            }
            else
            {
                enemy_senshu_flag = false;
            }
            calc_damage();
        }
        private void CheckBox_senshu_Unchecked(object sender, RoutedEventArgs e)
        {
            enemy_senshu_flag = false;
            calc_damage();
        }

        ShogoStatus soubi1_status = new ShogoStatus(0, 0, 0, 0, "");
        ShogoStatus soubi2_status = new ShogoStatus(0, 0, 0, 0, "");
        ShogoStatus shogo1_status = new ShogoStatus(0, 0, 0, 0, "");
        ShogoStatus shogo2_status = new ShogoStatus(0, 0, 0, 0, "");
        ShogoStatus ryoshoku_status = new ShogoStatus(0, 0, 0, 0, "");

        private void exec_equipment1_box()
        {
            // labelに現在コンボ選択の内容を表示
            soubi1_status.change_status(0, 0, 0, 0);
            ItemSet tmp = ((ItemSet)EquipmentBox1.SelectedItem);//表示名はキャストして取りだす
            if (tmp != null)
            {
                if (tmp.ItemValue != null)
                {

                    string[] temp_skillname = tmp.ItemValue.Split(',');
                    string[] temp_skill1 = temp_skillname[0].Split(':');
                    equipment1_skill1.Text = temp_skill1[0];
                    if (temp_skill1.Length > 1)
                    {
                        equipment1_skill1_fig.Text = temp_skill1[1];
                        soubi1_status.change_status(int.Parse(temp_skillname[2]), int.Parse(temp_skillname[3]), int.Parse(temp_skillname[4]), int.Parse(temp_skillname[5]));
                    }
                    else
                    {
                        equipment1_skill1_fig.Text = null;
                    }
                    if (temp_skillname.Length > 1)
                    {
                        string[] temp_skill2 = temp_skillname[1].Split(':');
                        equipment1_skill2.Text = temp_skill2[0];
                        if (temp_skill2.Length > 1)
                        {
                            equipment1_skill2_fig.Text = temp_skill2[1];
                        }
                        else
                        {
                            equipment1_skill2_fig.Text = null;
                        }
                        soubi1_status.change_status(int.Parse(temp_skillname[2]), int.Parse(temp_skillname[3]), int.Parse(temp_skillname[4]), int.Parse(temp_skillname[5]));
                    }
                    else
                    {
                        equipment1_skill2.Text = null;
                        equipment1_skill2_fig.Text = null;
                    }
                }
                else
                {
                    equipment1_skill1.Text = "";
                    equipment1_skill1_fig.Text = "";
                    equipment1_skill2.Text = "";
                    equipment1_skill2_fig.Text = "";
                }
                Resync_finalskil();
                calc_final_attack_mag();
                calc_damage();
            }
        }
        private void ComboBox_SelectionChanged_EquipmentBox1(object sender, SelectionChangedEventArgs e)
        {
            exec_equipment1_box();
        }

        private void exec_equipment2_box()
        {
            // labelに現在コンボ選択の内容を表示
            ItemSet tmp = ((ItemSet)EquipmentBox2.SelectedItem);//表示名はキャストして取りだす

            soubi2_status.change_status(0, 0, 0, 0);

            if (tmp != null)
            {
                if (tmp.ItemValue != null)
                {

                    string[] temp_skillname = tmp.ItemValue.Split(',');
                    string[] temp_skill1 = temp_skillname[0].Split(':');
                    equipment2_skill1.Text = temp_skill1[0];
                    if (temp_skill1.Length > 1)
                    {
                        equipment2_skill1_fig.Text = temp_skill1[1];
                        soubi2_status.change_status(int.Parse(temp_skillname[2]), int.Parse(temp_skillname[3]), int.Parse(temp_skillname[4]), int.Parse(temp_skillname[5]));
                    }
                    else
                    {
                        equipment2_skill1_fig.Text = null;
                    }
                    if (temp_skillname.Length > 1)
                    {
                        string[] temp_skill2 = temp_skillname[1].Split(':');
                        equipment2_skill2.Text = temp_skill2[0];
                        if (temp_skill2.Length > 1)
                        {
                            equipment2_skill2_fig.Text = temp_skill2[1];
                        }
                        else
                        {
                            equipment2_skill2_fig.Text = null;
                        }
                        soubi2_status.change_status(int.Parse(temp_skillname[2]), int.Parse(temp_skillname[3]), int.Parse(temp_skillname[4]), int.Parse(temp_skillname[5]));
                    }
                    else
                    {
                        equipment2_skill2.Text = null;
                        equipment2_skill2_fig.Text = null;
                    }
                }
                else
                {
                    equipment2_skill1.Text = "";
                    equipment2_skill1_fig.Text = "";
                    equipment2_skill2.Text = "";
                    equipment2_skill2_fig.Text = "";
                }
                Resync_finalskil();
                calc_final_attack_mag();
                calc_damage();
            }
        }
        private void ComboBox_SelectionChanged_EquipmentBox2(object sender, SelectionChangedEventArgs e)
        {
            exec_equipment2_box();
        }

        private void TextBox_TextChanged_12(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_13(object sender, TextChangedEventArgs e)
        {

        }
        private void exec_ryoshoku_box()
        {
            // labelに現在コンボ選択の内容を表示
            ItemSet tmp = ((ItemSet)ryoshokuBox.SelectedItem);//表示名はキャストして取りだす
            if (tmp != null)
            {
                if (tmp.ItemValue != null)
                {

                    string[] temp_status = tmp.ItemValue.Split(',');
                    string[] temp_skillname = temp_status[0].Split(':');
                    if (temp_skillname.Length > 1)
                    {
                        ryoshoku_name.Text = temp_skillname[0];
                        ryoshoku_fig_box.Text = temp_skillname[1];
                        ryoshoku_status.change_status(int.Parse(temp_status[1]), int.Parse(temp_status[2]), int.Parse(temp_status[3]), int.Parse(temp_status[4]));
                    }
                    else if(temp_skillname.Length == 1)
                    {
                        ryoshoku_name.Text = temp_skillname[0];
                    }
                }
                else
                {
                    ryoshoku_name.Text = "";
                    ryoshoku_fig_box.Text = "";
                }
                Resync_finalskil();
                calc_final_attack_mag();
                calc_damage();
            }

        }
        private void ComboBox_SelectionChanged_ryoshokuBox(object sender, SelectionChangedEventArgs e)
        {
            exec_ryoshoku_box();
        }

        private void exec_shogo1_box()
        {
            ItemSet tmp = ((ItemSet)shogo1Box.SelectedItem);//表示名はキャストして取りだす
            if (tmp != null)
            {
                if (tmp.ItemValue != null)
                {

                    string[] temp_status = tmp.ItemValue.Split(',');
                    string[] temp_skillname = temp_status[0].Split(':');
                    if (!string.IsNullOrEmpty(tmp.ItemValue))
                    {
                        if (temp_status.Length > 5)
                        {
                            if (temp_skillname.Length > 1)
                            {
                                shogo1.Text = temp_skillname[0];
                                shogo1_fig.Text = temp_skillname[1];
                                shogo1_status.change_status(int.Parse(temp_status[1]), int.Parse(temp_status[2]), int.Parse(temp_status[3]), int.Parse(temp_status[4]));
                                shogo1_status.tokko = temp_status[5];
                            }
                            else if (temp_skillname.Length == 1)
                            {
                                shogo1.Text = temp_skillname[0];
                                shogo1_fig.Text = "";
                                shogo1_status.change_status(int.Parse(temp_status[1]), int.Parse(temp_status[2]), int.Parse(temp_status[3]), int.Parse(temp_status[4]));
                                shogo1_status.tokko = temp_status[5];
                            }
                        }
                    }
                }
                else
                {
                    shogo1.Text = "";
                    shogo1_fig.Text = "";
                }
                Resync_finalskil();
                calc_final_attack_mag();
                calc_damage();
            }
        }
        private void shogo1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            exec_shogo1_box();

        }
        private void enemy_status_changed(object sender, TextChangedEventArgs e)
        {
            //敵のステータスが変更されたときに呼び出される
            //敵の攻撃力、HP、防御力、城壁値を取得して計算する
            calc_final_attack_mag();
            calc_damage();
        }

        private void exec_shogo2_box()
        {
            ItemSet tmp = ((ItemSet)shogo2Box.SelectedItem);//表示名はキャストして取りだす
            if (tmp != null)
            {
                if (tmp.ItemValue != null)
                {
                    string[] temp_status = tmp.ItemValue.Split(',');
                    string[] temp_skillname = temp_status[0].Split(':');
                    if (!string.IsNullOrEmpty(tmp.ItemValue))
                    {
                        if (temp_status.Length > 5)
                        {
                            if (temp_skillname.Length > 1)
                            {
                                shogo2.Text = temp_skillname[0];
                                shogo2_fig.Text = temp_skillname[1];
                                shogo2_status.change_status(int.Parse(temp_status[1]), int.Parse(temp_status[2]), int.Parse(temp_status[3]), int.Parse(temp_status[4]));
                                shogo2_status.tokko = temp_status[5];
                            }
                            if (temp_skillname.Length == 1)
                            {
                                shogo2.Text = temp_skillname[0];
                                shogo2_fig.Text = "0";
                                shogo2_status.change_status(int.Parse(temp_status[1]), int.Parse(temp_status[2]), int.Parse(temp_status[3]), int.Parse(temp_status[4]));
                                shogo2_status.tokko = temp_status[5];
                            }
                        }
                    }
                }
                else
                {
                    shogo2.Text = "";
                    shogo2_fig.Text = "";
                }
                Resync_finalskil();
                calc_final_attack_mag();
                calc_damage();
            }
        }
        private void shogo2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            exec_shogo2_box();
        }

        String filter_shokugyo = "";//職業フィルター

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var clicked = sender as CheckBox;
            if (clicked == null) return;
            foreach (var cb in ((Panel)clicked.Parent).Children.OfType<CheckBox>())
            {
                if (cb != clicked)
                    cb.IsChecked = false;
            }
            var selected = (attack_character_group.Content as Panel)?
                .Children
                .OfType<CheckBox>()
                .FirstOrDefault(cb => cb.IsChecked == true);
            var view = CollectionViewSource.GetDefaultView(CharacterBox.ItemsSource);
            if (selected != null)
            {
                filter_shokugyo = selected.Content.ToString();
                //view.Filter = item => ((CharacterJson)item).職業 == filter_shokugyo;
            }
            else
            {
                filter_shokugyo = "";
                //view.Filter = null;
                //CharacterBox.SelectedIndex = -1; // 解除時に選択リセット
            }
            // 非同期でRefresh
            //Dispatcher.BeginInvoke(() => view.Refresh());
        }
        private void CharacterBox_DropDownOpened(object sender, EventArgs e)
        {
            var view = CollectionViewSource.GetDefaultView(CharacterBox.ItemsSource);

            view.Filter = item =>
            {
                var cj = (CharacterJson)item;

                // 名前フィルタ
                bool nameMatch = string.IsNullOrEmpty(character_search_box.Text)
                    || (cj.名称?.IndexOf(character_search_box.Text, StringComparison.OrdinalIgnoreCase) >= 0);

                // スキルフィルタ（List<string> 部分一致対応）
                bool skillMatch = string.IsNullOrEmpty(character_skill_search_box.Text)
                    || (cj.パッシブスキル?.Any(skill =>
                            skill != null &&
                            skill.IndexOf(character_skill_search_box.Text, StringComparison.OrdinalIgnoreCase) >= 0) ?? false);

                // 職業フィルタ
                bool jobMatch = string.IsNullOrEmpty(filter_shokugyo)
                    || cj.職業 == filter_shokugyo;

                // すべての条件を AND で評価
                return nameMatch && skillMatch && jobMatch;
            };

            Dispatcher.BeginInvoke(() => view.Refresh());
            //ItemsView.Refresh(); // 開いたときにだけフィルタ適用
        }

        private void ShogoBox_DropDownOpened(object sender, EventArgs e)
        {
            var view = CollectionViewSource.GetDefaultView(shogo1Box.ItemsSource);

            if (string.IsNullOrEmpty(shogo_search_box.Text))
            {
                view.Filter = null;
                //shogo1Box.SelectedIndex = -1; // 解除時に選択リセット
            }
            else
            {
                view = CollectionViewSource.GetDefaultView(src_shogo1);

                // フィルタ設定（部分一致・大文字小文字無視）
                view.Filter = item =>
                {
                    if (item is ItemSet i && i.ItemDisp != null)
                        return i.ItemDisp.IndexOf(shogo_search_box.Text, StringComparison.OrdinalIgnoreCase) >= 0;
                    return false;
                };
            }
            Dispatcher.BeginInvoke(() => view.Refresh());
            //ItemsView.Refresh(); // 開いたときにだけフィルタ適用
        }
        private void Shogo2Box_DropDownOpened(object sender, EventArgs e)
        {
            var view = CollectionViewSource.GetDefaultView(shogo2Box.ItemsSource);

            if (string.IsNullOrEmpty(shogo_search_box.Text))
            {
                view.Filter = null;
                //shogo1Box.SelectedIndex = -1; // 解除時に選択リセット
            }
            else
            {
                view = CollectionViewSource.GetDefaultView(src_shogo2);

                // フィルタ設定（部分一致・大文字小文字無視）
                view.Filter = item =>
                {
                    if (item is ItemSet i && i.ItemDisp != null)
                        return i.ItemDisp.IndexOf(shogo_search_box.Text, StringComparison.OrdinalIgnoreCase) >= 0;
                    return false;
                };
            }
            Dispatcher.BeginInvoke(() => view.Refresh());
            //ItemsView.Refresh(); // 開いたときにだけフィルタ適用
        }
        private void EquipmentBox_DropDownOpened(object sender, EventArgs e)
        {
            var view = CollectionViewSource.GetDefaultView(EquipmentBox1.ItemsSource);
            if (string.IsNullOrEmpty(equipment_search_box.Text))
            {
                view.Filter = null;
                //EquipmentBox1.SelectedIndex = -1; // 解除時に選択リセット
            }
            else
            {
                view = CollectionViewSource.GetDefaultView(src_equipment1);
                // フィルタ設定（部分一致・大文字小文字無視）
                view.Filter = item =>
                {
                    if (item is ItemSet i && i.ItemDisp != null)
                        return i.ItemDisp.IndexOf(equipment_search_box.Text, StringComparison.OrdinalIgnoreCase) >= 0;
                    return false;
                };
            }
            Dispatcher.BeginInvoke(() => view.Refresh());
            //ItemsView.Refresh(); // 開いたときにだけフィルタ適用
        }
        private void Equipment2Box_DropDownOpened(object sender, EventArgs e)
        {
            var view = CollectionViewSource.GetDefaultView(EquipmentBox2.ItemsSource);
            if (string.IsNullOrEmpty(equipment_search_box.Text))
            {
                view.Filter = null;
                //EquipmentBox2.SelectedIndex = -1; // 解除時に選択リセット
            }
            else
            {
                view = CollectionViewSource.GetDefaultView(src_equipment2);
                // フィルタ設定（部分一致・大文字小文字無視）
                view.Filter = item =>
                {
                    if (item is ItemSet i && i.ItemDisp != null)
                        return i.ItemDisp.IndexOf(equipment_search_box.Text, StringComparison.OrdinalIgnoreCase) >= 0;
                    return false;
                };
            }
            Dispatcher.BeginInvoke(() => view.Refresh());
            //ItemsView.Refresh(); // 開いたときにだけフィルタ適用
        }
        private void RyoshokuBox_DropDownOpened(object sender, EventArgs e)
        {
            var view = CollectionViewSource.GetDefaultView(ryoshokuBox.ItemsSource);
            if (string.IsNullOrEmpty(equipment_search_box.Text))
            {
                view.Filter = null;
                //EquipmentBox2.SelectedIndex = -1; // 解除時に選択リセット
            }
            else
            {
                view = CollectionViewSource.GetDefaultView(src_ryoshoku);
                // フィルタ設定（部分一致・大文字小文字無視）
                view.Filter = item =>
                {
                    if (item is ItemSet i && i.ItemDisp != null)
                        return i.ItemDisp.IndexOf(equipment_search_box.Text, StringComparison.OrdinalIgnoreCase) >= 0;
                    return false;
                };
            }
            Dispatcher.BeginInvoke(() => view.Refresh());
            //ItemsView.Refresh(); // 開いたときにだけフィルタ適用
        }

        private void status_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isInitialized)
            {
                calc_damage();
                calc_final_attack_mag();
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Saved_Button_Click(object sender, RoutedEventArgs e)
        {
            savedata tempdata;
            if (CharacterBox.ItemsSource != null)
            {
                CharacterJson selectedCharacter = (CharacterJson)CharacterBox.SelectedItem;
                if (selectedCharacter != null)
                {
                    tempdata = new savedata();
                    tempdata.character_name = selectedCharacter.名称;
                    tempdata.character_id = selectedCharacter.名称 + "(" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + ")";
                }
                else
                {
                    return;
                }
                if (EquipmentBox1.ItemsSource != null && EquipmentBox1.SelectedItem != null)
                {
                    ItemSet selectedEquipment1 = (ItemSet)EquipmentBox1.SelectedItem;
                    if (selectedEquipment1 != null)
                    {
                        tempdata.equipment1 = selectedEquipment1.ItemValue;
                        tempdata.equipment1_name = selectedEquipment1.ItemDisp;
                    }
                    else
                    {
                        tempdata.equipment1 = "";
                        tempdata.equipment1_name = "";
                    }
                }
                if (EquipmentBox2.ItemsSource != null && EquipmentBox2.SelectedItem != null)
                {
                    ItemSet selectedEquipment2 = (ItemSet)EquipmentBox2.SelectedItem;
                    if (selectedEquipment2 != null)
                    {
                        tempdata.equipment2 = selectedEquipment2.ItemValue;
                        tempdata.equipment2_name = selectedEquipment2.ItemDisp;
                    }
                    else
                    {
                        tempdata.equipment2 = "";
                        tempdata.equipment2_name = "";
                    }
                }
                if (ryoshokuBox.ItemsSource != null && ryoshokuBox.SelectedItem != null)
                {
                    ItemSet selectedryoshoku = (ItemSet)ryoshokuBox.SelectedItem;
                    if (selectedryoshoku != null)
                    {
                        tempdata.ryoshoku = selectedryoshoku.ItemValue;
                        tempdata.ryoshoku_name = selectedryoshoku.ItemDisp;
                    }
                    else
                    {
                        tempdata.ryoshoku = "";
                        tempdata.ryoshoku_name = "";
                    }
                }
                if (shogo1Box.ItemsSource != null && shogo1Box.SelectedItem != null)
                {
                    ItemSet selectedshogo1 = (ItemSet)shogo1Box.SelectedItem;
                    if (selectedshogo1 != null)
                    {
                        tempdata.shogo1 = selectedshogo1.ItemValue;
                        tempdata.shogo1_name = selectedshogo1.ItemDisp;
                    }
                    else
                    {
                        tempdata.shogo1 = "";
                        tempdata.shogo1_name = "";
                    }
                }
                if (shogo2Box.ItemsSource != null && shogo2Box.SelectedItem != null)
                {
                    ItemSet selectedshogo2 = (ItemSet)shogo2Box.SelectedItem;
                    if (selectedshogo2 != null)
                    {
                        tempdata.shogo2 = selectedshogo2.ItemValue;
                        tempdata.shogo2_name = selectedshogo2.ItemDisp;
                    }
                    else
                    {
                        tempdata.shogo2 = "";
                        tempdata.shogo2_name = "";
                    }
                }
                if (assistskill1_box.ItemsSource != null && assistskill1_box.SelectedItem != null)
                {
                    ItemSet selectedassist1 = (ItemSet)assistskill1_box.SelectedItem;
                    if (selectedassist1 != null)
                    {
                        tempdata.assist1 = selectedassist1.ItemValue;
                    }
                    else
                    {
                        tempdata.assist1 = "";
                    }
                }
                if (assistskill2_box.ItemsSource != null && assistskill2_box.SelectedItem != null)
                {
                    ItemSet selectedassist2 = (ItemSet)assistskill2_box.SelectedItem;
                    if (selectedassist2 != null)
                    {
                        tempdata.assist2 = selectedassist2.ItemValue;
                    }
                    else
                    {
                        tempdata.assist2 = "";
                    }
                }
                if (assistskill3_box.ItemsSource != null && assistskill3_box.SelectedItem != null)
                {
                    ItemSet selectedassist3 = (ItemSet)assistskill3_box.SelectedItem;
                    if (selectedassist3 != null)
                    {
                        tempdata.assist3 = selectedassist3.ItemValue;
                    }
                    else
                    {
                        tempdata.assist3 = "";
                    }
                }
                if (assistskill1_chiryoku_box.Text != null)
                {
                    tempdata.assist1_chiryoku = assistskill1_chiryoku_box.Text;
                }
                else
                {
                    tempdata.assist1_chiryoku = "";
                }
                if (assistskill2_chiryoku_box.Text != null)
                {
                    tempdata.assist2_chiryoku = assistskill2_chiryoku_box.Text;
                }
                else
                {
                    tempdata.assist2_chiryoku = "";
                }
                if (assistskill3_chiryoku_box.Text != null)
                {
                    tempdata.assist3_chiryoku = assistskill3_chiryoku_box.Text;
                }
                else
                {
                    tempdata.assist3_chiryoku = "";
                }
                if (unmei_box.Text != null)
                {
                    tempdata.unmei = unmei_box.Text;
                }
                else
                {
                    tempdata.unmei = "";
                }

                tempdata.leader_flag = leader_flag;
                all_save_data.Add(tempdata);

            }
        }
        public bool isLoad = false;

        private void Load_Button_Click(object sender, RoutedEventArgs e)
        {
            // saved_list.SelectedItem から選択された savedata を取得
            var selected = saved_list.SelectedItem as savedata;
            isLoad = true;

            if (selected != null)
            {

                //this.Visibility = Visibility.Collapsed; // 画面を一時非表示
                var sw = Stopwatch.StartNew();
                shogo1Box.SelectionChanged -= shogo1_SelectionChanged;
                shogo2Box.SelectionChanged -= shogo2_SelectionChanged;
                EquipmentBox1.SelectionChanged -= ComboBox_SelectionChanged_EquipmentBox1;
                EquipmentBox2.SelectionChanged -= ComboBox_SelectionChanged_EquipmentBox2;
                ryoshokuBox.SelectionChanged -= ComboBox_SelectionChanged_ryoshokuBox;
                assistskill1_box.SelectionChanged -= assistskill1_box_SelectionChanged;
                assistskill2_box.SelectionChanged -= assistskill2_box_SelectionChanged;
                assistskill3_box.SelectionChanged -= assistskill3_box_SelectionChanged;
                assistskill1_chiryoku_box.TextChanged -= assistskill1_chiryoku_box_TextChanged;
                assistskill2_chiryoku_box.TextChanged -= assistskill2_chiryoku_box_TextChanged;
                assistskill3_chiryoku_box.TextChanged -= assistskill3_chiryoku_box_TextChanged;
                unmei_box.TextChanged -= unmei_box_TextChanged;

                //まずキャラクター選択のフィルターを解除する
                var view = CollectionViewSource.GetDefaultView(CharacterBox.ItemsSource);

                filter_shokugyo = "";
                view.Filter = null;
                CharacterBox.SelectedIndex = 0; // 解除時に選択リセット
                shokugyo_blader.IsChecked = false;
                shokugyo_caster.IsChecked = false;
                shokugyo_healer.IsChecked = false;
                shokugyo_destroyer.IsChecked = false;
                shokugyo_guarder.IsChecked = false;
                shokugyo_lancer.IsChecked = false;
                shokugyo_shooter.IsChecked = false;
                // 非同期でRefresh

                Dispatcher.BeginInvoke(() => view.Refresh());

                // キャラクター選択
                CharacterJson characterObj = characters.Find(c => c.名称 == selected.character_name);
                if (characterObj != null)
                {
                    CharacterBox.SelectedItem = characterObj;
                }

                Debug.WriteLine($"キャラクター: {sw.ElapsedMilliseconds}ms"); sw.Restart();
                // 装備1選択
                if (!string.IsNullOrEmpty(selected.equipment1))
                {
                    ItemSet equipment1 = src_equipment1.ToList().Find(eq => eq != null && eq.ItemValue == selected.equipment1);
                    if (equipment1 != null)
                    {
                        EquipmentBox1.SelectedItem = equipment1;
                        exec_equipment1_box();
                    }

                }
                else
                {
                    EquipmentBox1.SelectedIndex = -1; // 装備1が空の場合、選択を解除
                }
                // 装備2選択
                if (!string.IsNullOrEmpty(selected.equipment2))
                {
                    ItemSet equipment2 = src_equipment2.ToList().Find(eq => eq != null && eq.ItemValue == selected.equipment2);
                    if (equipment2 != null)
                    {
                        EquipmentBox2.SelectedItem = equipment2;
                        exec_equipment2_box();
                    }
                }
                else
                {
                    EquipmentBox2.SelectedIndex = -1; // 装備2が空の場合、選択を解除
                }
                // 料理選択
                if (!string.IsNullOrEmpty(selected.ryoshoku))
                {
                    ItemSet ryoshoku = src_ryoshoku.Find(r => r != null && r.ItemValue == selected.ryoshoku);
                    if (ryoshoku != null)
                    {
                        ryoshokuBox.SelectedItem = ryoshoku;
                        exec_ryoshoku_box();
                    }
                }
                else
                {
                    ryoshokuBox.SelectedIndex = -1; // 料理が空の場合、選択を解除
                }
                // 称号1選択
                if (!string.IsNullOrEmpty(selected.shogo1))
                {
                    ItemSet shogo1 = src_shogo1.ToList().Find(s => s != null && s.ItemValue == selected.shogo1);
                    if (shogo1 != null)
                    {
                        shogo1Box.SelectedItem = shogo1;
                        exec_shogo1_box();
                    }
                }
                else
                {
                    shogo1Box.SelectedIndex = -1; // 称号1が空の場合、選択を解除
                }
                // 称号2選択
                if (!string.IsNullOrEmpty(selected.shogo2))
                {
                    ItemSet shogo2 = src_shogo2.ToList().Find(s => s != null && s.ItemValue == selected.shogo2);
                    if (shogo2 != null)
                    {
                        shogo2Box.SelectedItem = shogo2;
                        exec_shogo2_box();
                    }
                }
                else
                {
                    shogo2Box.SelectedIndex = -1; // 称号2が空の場合、選択を解除
                }
                assistskill1_box.SelectedIndex = -1;
                assistskill2_box.SelectedIndex = -1;
                assistskill3_box.SelectedIndex = -1;
                if (!string.IsNullOrEmpty(selected.assist1))
                {
                    ItemSet assist1 = src_asistskills.ToList().Find(a => a != null && a.ItemValue == selected.assist1);
                    if (assist1 != null)
                    {
                        assistskill1_box.SelectedItem = assist1;
                    }
                    else
                    {
                        assistskill1_box.SelectedIndex = -1; // アシストスキル1が空の場合、選択を解除
                    }
                }
                else
                {
                    assistskill1_box.SelectedIndex = -1; // アシストスキル1が空の場合、選択を解除
                }
                if (!string.IsNullOrEmpty(selected.assist2))
                {
                    ItemSet assist2 = src_asistskills.ToList().Find(a => a != null && a.ItemValue == selected.assist2);
                    if (assist2 != null)
                    {
                        assistskill2_box.SelectedItem = assist2;
                    }
                    else
                    {
                        assistskill2_box.SelectedIndex = -1; // アシストスキル2が空の場合、選択を解除
                    }
                }
                else
                {
                    assistskill2_box.SelectedIndex = -1; // アシストスキル2が空の場合、選択を解除
                }
                if (!string.IsNullOrEmpty(selected.assist3))
                {
                    ItemSet assist3 = src_asistskills.ToList().Find(a => a != null && a.ItemValue == selected.assist3);
                    if (assist3 != null)
                    {
                        assistskill3_box.SelectedItem = assist3;
                    }
                    else
                    {
                        assistskill3_box.SelectedIndex = -1; // アシストスキル3が空の場合、選択を解除
                    }
                }
                else
                {
                    assistskill3_box.SelectedIndex = -1; // アシストスキル3が空の場合、選択を解除
                }
                if (!string.IsNullOrEmpty(selected.assist1_chiryoku))
                {
                    assistskill1_chiryoku_box.Text = selected.assist1_chiryoku;
                }
                else
                {
                    assistskill1_chiryoku_box.Text = "";
                }
                if (!string.IsNullOrEmpty(selected.assist2_chiryoku))
                {
                    assistskill2_chiryoku_box.Text = selected.assist2_chiryoku;
                }
                else
                {
                    assistskill2_chiryoku_box.Text = "";
                }
                if (!string.IsNullOrEmpty(selected.assist3_chiryoku))
                {
                    assistskill3_chiryoku_box.Text = selected.assist3_chiryoku;
                }
                else
                {
                    assistskill3_chiryoku_box.Text = "";
                }
                if (!string.IsNullOrEmpty(selected.unmei))
                {
                    unmei_box.Text = selected.unmei;
                }
                else
                {
                    unmei_box.Text = "";
                }
                Debug.WriteLine($"その他: {sw.ElapsedMilliseconds}ms"); sw.Restart();

                // リーダーフラグ設定
                leader_flag = selected.leader_flag;
                leaderflag.IsChecked = leader_flag;
                shogo1Box.SelectionChanged += shogo1_SelectionChanged;
                shogo2Box.SelectionChanged += shogo2_SelectionChanged;
                EquipmentBox1.SelectionChanged += ComboBox_SelectionChanged_EquipmentBox1;
                EquipmentBox2.SelectionChanged += ComboBox_SelectionChanged_EquipmentBox2;
                ryoshokuBox.SelectionChanged += ComboBox_SelectionChanged_ryoshokuBox;
                assistskill1_box.SelectionChanged += assistskill1_box_SelectionChanged;
                assistskill2_box.SelectionChanged += assistskill2_box_SelectionChanged;
                assistskill3_box.SelectionChanged += assistskill3_box_SelectionChanged;
                unmei_box.TextChanged += unmei_box_TextChanged;
                assistskill1_chiryoku_box.TextChanged += assistskill1_chiryoku_box_TextChanged;
                assistskill2_chiryoku_box.TextChanged += assistskill2_chiryoku_box_TextChanged;
                assistskill3_chiryoku_box.TextChanged += assistskill3_chiryoku_box_TextChanged;

                //this.Visibility = Visibility.Visible; // 画面を再表示

                isLoad = false;
                Debug.WriteLine($"ステータス: {sw.ElapsedMilliseconds}ms"); sw.Restart();
                // ステータス再計算
                Resync_finalskil();
                calc_final_attack_mag();
                calc_damage();
            }
        }

        private int calc_chiryoku_assist(string fig, int number)
        {
            var chiryokuBox = (TextBox)this.FindName($"assistskill{number}_chiryoku_box");
            int temp_fig = 0;
            int chiryoku = 1;
            if (chiryokuBox != null && !string.IsNullOrEmpty(chiryokuBox.Text) && int.TryParse(chiryokuBox.Text, out int parsedChiryoku))
            {
                chiryoku = parsedChiryoku;
            }
            temp_fig = (int)(Math.Truncate(Math.Sqrt(chiryoku)) + int.Parse(fig));
            return temp_fig;
        }


        private void assistskill1()
        {
            // labelに現在コンボ選択の内容を表示
            ItemSet tmp = ((ItemSet)assistskill1_box.SelectedItem);//表示名はキャストして取りだす
            if (tmp != null)
            {
                if (tmp.ItemValue != null)
                {

                    string[] temp_skillname = tmp.ItemValue.Split(',');
                    string[] temp_skill1 = temp_skillname[0].Split(':');
                    assist1_name.Text = temp_skill1[0];
                    if (temp_skill1.Length > 1)
                    {
                        int temp = calc_chiryoku_assist(temp_skill1[1], 1);
                        //とりあえずすべて25上限
                        if (temp > 25) temp = 25;
                        if (assist1_name.Text == "心核穿ち") temp = 5; //心核穿ちは5固定
                        assist1_fig_box.Text = temp.ToString();
                    }
                    else
                    {
                        assist1_fig_box.Text = null;
                    }

                }
            }
            Resync_finalskil();
            calc_final_attack_mag();
            calc_damage();

        }
        private void assistskill2()
        {
            // labelに現在コンボ選択の内容を表示
            ItemSet tmp = ((ItemSet)assistskill2_box.SelectedItem);//表示名はキャストして取りだす
            if (tmp != null)
            {
                if (tmp.ItemValue != null)
                {

                    string[] temp_skillname = tmp.ItemValue.Split(',');
                    string[] temp_skill2 = temp_skillname[0].Split(':');
                    assist2_name.Text = temp_skill2[0];
                    if (temp_skill2.Length > 1)
                    {
                        int temp = calc_chiryoku_assist(temp_skill2[1], 2);
                        //とりあえずすべて25上限
                        if (temp > 25) temp = 25;
                        if (assist2_name.Text == "心核穿ち") temp = 5; //心核穿ちは5固定
                        assist2_fig_box.Text = temp.ToString();
                    }
                    else
                    {
                        assist2_fig_box.Text = null;
                    }

                }
            }
            Resync_finalskil();
            calc_final_attack_mag();
            calc_damage();
        }
        private void assistskill3()
        {
            // labelに現在コンボ選択の内容を表示
            ItemSet tmp = ((ItemSet)assistskill3_box.SelectedItem);//表示名はキャストして取りだす
            if (tmp != null)
            {
                if (tmp.ItemValue != null)
                {

                    string[] temp_skillname = tmp.ItemValue.Split(',');
                    string[] temp_skill3 = temp_skillname[0].Split(':');
                    assist3_name.Text = temp_skill3[0];
                    if (temp_skill3.Length > 1)
                    {
                        int temp = calc_chiryoku_assist(temp_skill3[1], 3);
                        //とりあえずすべて25上限
                        if (temp > 25) temp = 25;
                        if (assist3_name.Text == "心核穿ち") temp = 5; //心核穿ちは5固定
                        assist3_fig_box.Text = temp.ToString();
                    }
                    else
                    {
                        assist3_fig_box.Text = null;
                    }

                }
            }
            Resync_finalskil();
            calc_final_attack_mag();
            calc_damage();
        }

        private void assistskill1_box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            assistskill1();
        }
        private void assistskill2_box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            assistskill2();
        }

        private void assistskill3_box_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            assistskill3();
        }

        private void assistskill1_chiryoku_box_TextChanged(object sender, TextChangedEventArgs e)
        {
            assistskill1();

        }

        private void assistskill2_chiryoku_box_TextChanged(object sender, TextChangedEventArgs e)
        {
            assistskill2();
        }

        private void assistskill3_chiryoku_box_TextChanged(object sender, TextChangedEventArgs e)
        {
            assistskill3();
        }

        private void enemy_unmei_changed(object sender, TextChangedEventArgs e)
        {
            Resync_finalskil();
            calc_damage();
        }

        private void unmei_box_TextChanged(object sender, TextChangedEventArgs e)
        {
            Resync_finalskil();
            calc_final_attack_mag();
            calc_damage();
        }

        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            // 選択された savedata を取得
            var selected = saved_list.SelectedItem as savedata;
            if (selected != null)
            {
                // 削除前に現在のインデックスを取得
                int index = saved_list.SelectedIndex;

                // リストから削除
                all_save_data.Remove(selected);

                // 新しい選択を決める
                if (all_save_data.Count > 0)
                {
                    // 下のアイテムがあれば選択
                    if (index < all_save_data.Count)
                    {
                        saved_list.SelectedIndex = index; // 下のアイテム
                    }
                    else
                    {
                        saved_list.SelectedIndex = all_save_data.Count - 1; // 上のアイテム
                    }
                }
                else
                {
                    // リストが空なら選択をクリア
                    saved_list.SelectedIndex = -1;
                }
            }
        }

        private void Write_File_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // all_save_data は ObservableCollection<savedata> を想定
                string json = JsonConvert.SerializeObject(all_save_data, Formatting.Indented);
                Directory.CreateDirectory("./json");
                File.WriteAllText("./json/saved.json", json);
                MessageBox.Show("保存しました。");
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

        private void Read_File_Button_Click(object sender, RoutedEventArgs e)
        {
            //jsonファイルから読み込んでall_save_dataに追加する
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
                MessageBox.Show("保存ファイルが見つかりません。");
            }
        }

        private void enemy_waisho_changed(object sender, TextChangedEventArgs e)
        {
            calc_damage();
        }

        private void enemy_stance_boxChanged(object sender, SelectionChangedEventArgs e)
        {
            calc_damage();
        }

        private void Character_CloseButton_Click(object sender, RoutedEventArgs e)
        {
            character_search_box.Text = "";
        }

        private void shogo_CloseButton_Click(object sender, RoutedEventArgs e)
        {
            shogo_search_box.Text = "";
        }
        private void Equipment_CloseButton_Click(object sender, RoutedEventArgs e)
        {
            equipment_search_box.Text = "";
        }
        int level_shitei = 0;
        private void levelbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int level=1, buko=1;
            if (levelbox != null && bukobox != null && _isProgrammaticChange == false)
            {
                if (int.TryParse(levelbox.Text, out level))
                {
                }
                if (int.TryParse(bukobox.Text, out buko))
                {
                }
                status_calc_box(level, buko);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CharacterSkill_CloseButton_Click(object sender, RoutedEventArgs e)
        {

        }

        public static class CaptureWrapper
        {
            [DllImport("CaptureWrapper.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern int CaptureWindowByTitle(string windowTitle, string outputPath);
        }
        private Pix BitmapToPix(Bitmap bmp)
        {
            using MemoryStream ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;
            return Pix.LoadFromMemory(ms.ToArray());
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
        private Bitmap AdaptiveThreshold(Bitmap gray,int i_threshold)
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
                    bin.SetPixel(x, y, Color.FromArgb(val, val, val));
                }
            }
            return bin;
        }
        // 簡易的な文字列類似度（Levenshtein風スコア）
        private double Similarity(string a, string b)
        {
            if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b)) return 0;
            int len = Math.Max(a.Length, b.Length);
            int dist = Levenshtein(a, b);
            return 1.0 - (double)dist / len;
        }

        private int Levenshtein(string a, string b)
        {
            int[,] dp = new int[a.Length + 1, b.Length + 1];
            for (int i = 0; i <= a.Length; i++) dp[i, 0] = i;
            for (int j = 0; j <= b.Length; j++) dp[0, j] = j;

            for (int i = 1; i <= a.Length; i++)
                for (int j = 1; j <= b.Length; j++)
                {
                    int cost = a[i - 1] == b[j - 1] ? 0 : 1;
                    dp[i, j] = Math.Min(Math.Min(
                        dp[i - 1, j] + 1,
                        dp[i, j - 1] + 1),
                        dp[i - 1, j - 1] + cost);
                }

            return dp[a.Length, b.Length];
        }
        private int LevenshteinDistance(string s, string t)
        {
            if (s == t) return 0;
            if (s.Length == 0) return t.Length;
            if (t.Length == 0) return s.Length;

            var d = new int[s.Length + 1, t.Length + 1];

            for (int i = 0; i <= s.Length; i++) d[i, 0] = i;
            for (int j = 0; j <= t.Length; j++) d[0, j] = j;

            for (int i = 1; i <= s.Length; i++)
            {
                for (int j = 1; j <= t.Length; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }

            return d[s.Length, t.Length];
        }
        private void SelectMostSimilar(string input)
        {
            if (string.IsNullOrWhiteSpace(input) || CharacterBox.ItemsSource is not IEnumerable<CharacterJson> items)
                return;

            // 最も近いNameを持つキャラを取得
            var bestMatch = items
                .OrderBy(c => LevenshteinDistance(c.名称, input))
                .FirstOrDefault();

            if (bestMatch != null)
                CharacterBox.SelectedItem = bestMatch;
        }
        public void SelectMostSimilarEquipment(string input,int shurui)
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
                if(shurui == 0)
                    EquipmentBox1.SelectedItem = bestMatch;
                else if (shurui == 1)
                    EquipmentBox2.SelectedItem = bestMatch;
                else if (shurui == 2)
                    ryoshokuBox.SelectedItem = bestMatch;
            }
        }
        bool IsBinarizedImageMostlyBlack(Bitmap binarized, double blackRatioThreshold = 0.98)
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
        private static readonly TesseractEngine _engine = new TesseractEngine(@"./tessdata", "jpn", EngineMode.LstmOnly);

    public string cropedAndselect(System.Drawing.Rectangle cropRect,int kakudai,int threathold)
        {
            string path = @".\Temp\capture.png";
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
            //binarized.Save(debugPath, System.Drawing.Imaging.ImageFormat.Png);
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

        public static class ShogoMatcher
        {

            public static (ItemSet? item1, ItemSet? item2, double score) FindBestPair(
                string ocrText,
                ObservableCollection<ItemSet> src_shogo1,
                ObservableCollection<ItemSet> src_shogo2,
                double minScore = 0.0)
            {
                if (string.IsNullOrWhiteSpace(ocrText) || src_shogo1 == null || src_shogo2 == null)
                    return (null, null, 0.0);

                ocrText = ocrText.Trim();

                // 事前にクリーン済みDispを作る（ここでは要素sがnullの場合を除外してリスト化）
                // ItemSet? sを想定して s != null のフィルタリングを追加推奨
                var list1 = src_shogo1.Where(s => s != null).Select(s => (item: s!, clean: CleanDisp(s!.ItemDisp))).ToList();
                var list2 = src_shogo2.Where(s => s != null).Select(s => (item: s!, clean: CleanDisp(s!.ItemDisp))).ToList();

                // -------------------------------------------------------------------
                // ここからが並列化された処理
                // -------------------------------------------------------------------

                // 1. list1とlist2の要素をSelectManyで結合（クロス積）
                // 2. AsParallel() で並列処理に切り替える
                var bestMatch = list1.AsParallel()
                    .SelectMany(t1 => list2.Select(t2 => new
                    {
                        // 処理に必要なデータだけを一時オブジェクトに格納
                        item1 = t1.item,
                        clean1 = t1.clean,
                        item2 = t2.item,
                        clean2 = t2.clean
                    }))
                    // スコア計算（最も重い処理）を並列実行
                    .Select(p => {
                        string combined = string.Concat(p.clean1, p.clean2);
                        double score = Similarity(ocrText, combined);

                        return new
                        {
                            p.item1,
                            p.item2,
                            score
                        };
                    })
                    // 3. OrderByDescending で最高のスコアを持つペアを絞り込む (これは順次処理に戻る)
                    .OrderByDescending(r => r.score)
                    .FirstOrDefault(); // 最もスコアが高い要素を一つだけ取得

                // -------------------------------------------------------------------

                // 結果の処理
                if (bestMatch == null || bestMatch.score < minScore)
                {
                    return (null, null, (bestMatch?.score ?? 0.0));
                }

                return (bestMatch.item1, bestMatch.item2, bestMatch.score);
            }

            private static string CleanDisp(string? disp)
            {
                if (string.IsNullOrEmpty(disp)) return string.Empty;

                // 正規化（全角/半角などの差を減らす）
                try { disp = disp.Normalize(NormalizationForm.FormKC); } catch { /*ignore*/ }

                // () 内を削除。前後の空白も取り除く。
                // マッチング精度向上のため、( ... ) の前後の余白も削る
                var cleaned = Regex.Replace(disp, @"\s*\(.*?\)\s*", "");
                return cleaned.Trim();
            }

            private static double Similarity(string s1, string s2)
            {
                s1 ??= "";
                s2 ??= "";
                int distance = LevenshteinDistance(s1, s2);
                int maxLen = Math.Max(s1.Length, s2.Length);
                return maxLen == 0 ? 1.0 : 1.0 - (double)distance / maxLen;
            }

            private static int LevenshteinDistance(string s, string t)
            {
                if (s == null) s = "";
                if (t == null) t = "";
                int n = s.Length;
                int m = t.Length;
                if (n == 0) return m;
                if (m == 0) return n;

                var d = new int[n + 1, m + 1];

                for (int i = 0; i <= n; i++) d[i, 0] = i;
                for (int j = 0; j <= m; j++) d[0, j] = j;

                for (int i = 1; i <= n; i++)
                {
                    for (int j = 1; j <= m; j++)
                    {
                        int cost = s[i - 1] == t[j - 1] ? 0 : 1;
                        d[i, j] = Math.Min(
                            Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                            d[i - 1, j - 1] + cost);
                    }
                }

                return d[n, m];
            }
        }


        static DenseTensor<float> BitmapToTensor_KerasCaffe(Bitmap bmp)
        {
            int height = bmp.Height;
            int width = bmp.Width;
            var tensor = new DenseTensor<float>(new int[] { 1, height, width, 3 });

            // Keras preprocess_input(Caffeモード) の平均値(BGR)
            float[] mean = { 103.939f, 116.779f, 123.68f }; // B, G, R

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    System.Drawing.Color c = bmp.GetPixel(x, y);
                    tensor[0, y, x, 0] = c.B - mean[0]; // B
                    tensor[0, y, x, 1] = c.G - mean[1]; // G
                    tensor[0, y, x, 2] = c.R - mean[2]; // R
                }
            }
            return tensor;
        }
        static float[] Normalize_chara(float[] vec)
        {
            double norm = Math.Sqrt(vec.Select(v => v * v).Sum());
            return vec.Select(v => (float)(v / norm)).ToArray();
        }
        class ImageFeature
        {
            public string Id { get; set; }
            public float[] Feature { get; set; }
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
        public void load_from_game(int sw, int sh, int ew, int eh)
        {
            string path = @".\Temp\capture.png";
            using Bitmap tempbmp = new Bitmap(path);

            var cropRect = new System.Drawing.Rectangle(sw, sh, ew, eh);
            using Bitmap bmp = tempbmp.Clone(cropRect, tempbmp.PixelFormat);
            string debugPath = @".\Temp\cropped_debug.png";
            //bmp.Save(debugPath, System.Drawing.Imaging.ImageFormat.Png);
            string onnxPath = @"feature_extraction/resnet50_features.onnx";   // Pythonで変換したONNXモデル
            string jsonPath = @"feature_extraction/chara_features.json";              // Python特徴量DB
            string csvPath = @"feature_extraction/list.csv";                   // ID→名前

            // 1. 画像ロード & 224x224 にリサイズ
            //using Bitmap bmp = new Bitmap(inputPath);
            using Bitmap resized = new Bitmap(bmp, 224, 224);

            // 2. NHWC Tensor に変換 + Keras preprocess_input(Caffeモード)
            var inputTensor = BitmapToTensor_KerasCaffe(resized);

            // 3. ONNX 推論
            //using var session = new InferenceSession(onnxPath);
            string inputName = session.InputMetadata.Keys.First(); // 入力名はONNXで確認
            var result = session.Run(new List<NamedOnnxValue> {
                NamedOnnxValue.CreateFromTensor(inputName, inputTensor)
            });

            float[] queryFeature = result.First().AsEnumerable<float>().ToArray();
            queryFeature = Normalize_chara(queryFeature);


            // 5. 類似検索（コサイン類似度）
            var top = features
                .Select(f => new { f.Id, Name = idNameMap[f.Id], Score = Cosine(f.Feature, queryFeature) })
                .OrderByDescending(x => x.Score)
                .FirstOrDefault();

            if (top != null)
            {
                Debug.WriteLine($"最も近い画像: {top.Name} (ID: {top.Id}, Score: {top.Score:F3})");

                var match = characters.FirstOrDefault(c => c.名称 == top.Name);
                if (match != null)
                {
                    CharacterBox.SelectedItem = match;
                }
            }
        }

        private void CaptureAndOcrButton_Click(object sender, RoutedEventArgs e)
        {
            string path = @".\Temp\capture.png";
            int hr = CaptureWrapper.CaptureWindowByTitle("VenusBloodVALKYRIE", path);
            if (hr != 0)
            {
                MessageBox.Show($"キャプチャ失敗: {hr}");
                return;
            }

            using Bitmap bmp = new Bitmap(path);
            // 必要部分を切り抜き
            //var cropRect = new System.Drawing.Rectangle(593, 100, 250, 33);//名前
            var cropRect_chara = new System.Drawing.Rectangle(315, 85, 190, 190);//名前
            var cropRect_shogo = new System.Drawing.Rectangle(593, 75, 300, 27);//称号
            var cropRect_equip1 = new System.Drawing.Rectangle(558, 245, 231, 22);//装備1
            var cropRect_equip2 = new System.Drawing.Rectangle(558, 271, 231, 22);//装備2
            var cropRect_ryoshoku = new System.Drawing.Rectangle(558, 297, 231, 22);//糧食
            //using Bitmap cropped = bmp.Clone(cropRect, bmp.PixelFormat);
            // グレースケール
            string noSpace="";
            var popup = new ProgressWindow(this);
            popup.Owner = this;
            popup.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            popup.ShowDialog();
        }

        public void best_match_shogo(string noSpace)
        {
            //var (best1, best2, score) = ShogoMatcher.FindBestPair(noSpace, src_shogo1, src_shogo2);
            var (best1, best2, score) = ShogoMatcher.FindBestPair(noSpace, src_shogo1, src_shogo2);
            if (best1 != null && best2 != null)
            {
                shogo1Box.SelectedItem = best1;
                shogo2Box.SelectedItem = best2;
                // 見つかったペアで何かする（selected に設定する／UI更新 等）
                Debug.WriteLine($"Found: {best1.ItemDisp} + {best2.ItemDisp} (score={score:F3})");
            }
            else if (best1 != null)
                shogo1Box.SelectedItem = best1;
            else if (best2 != null)
                shogo2Box.SelectedItem = best2;
            else
                Debug.WriteLine($"No good match (best score={score:F3})");

        }

        private void MoveSelected(int offset)
        {
            var list = saved_list.ItemsSource as ObservableCollection<savedata>;
            if (saved_list.SelectedItem is savedata selected && list != null)
            {
                int idx = list.IndexOf(selected);
                int newIndex = idx + offset;
                if (newIndex < 0 || newIndex >= list.Count) return;

                list.Move(idx, newIndex); // ObservableCollection の Move
                saved_list.SelectedItem = selected;
            }
        }

        private void MoveUpButton_Click(object sender, RoutedEventArgs e) => MoveSelected(-1);
        private void MoveDownButton_Click(object sender, RoutedEventArgs e) => MoveSelected(1);

        private void btnSortId_Click(object sender, RoutedEventArgs e)
        {
            SortList((a, b) => string.Compare(a.character_id, b.character_id, StringComparison.Ordinal));
        }
        private void SortList(Comparison<savedata> comparison)
        {
            var list = all_save_data as ObservableCollection<savedata>;
            if (list == null) return;

            // ObservableCollection を一旦 List に変換
            var sorted = list.ToList();
            sorted.Sort(comparison);

            // 元の ObservableCollection を更新
            list.Clear();
            foreach (var item in sorted)
                list.Add(item);

            // ListBox は ItemsSource が ObservableCollection なので自動更新
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            // 全ウィンドウ閉じる
            foreach (Window w in Application.Current.Windows)
                w.Close();

            // 念のため完全終了
            Application.Current.Shutdown();
        }
    }
}
