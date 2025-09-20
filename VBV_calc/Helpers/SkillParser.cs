namespace VBV_calc.Helpers
{
    public static class SkillParser
    {
        public static (string, int) Div_Skill_Name_Value(string skill)
        {
            string skill_name = "";
            int skill_value = 0;

            string[] temp_skillname = skill.Split('[');
            if (temp_skillname.Length > 1)
            {
                string[] temp_skillvalue = temp_skillname[1].Split(']');
                skill_value = int.Parse(temp_skillvalue[0]);
            }
            skill_name = temp_skillname[0];
            return (skill_name, skill_value);
        }

        public static (string, int) Div_Skill_Name_Value_equip(string skill)
        {
            string skill_name = "";
            int skill_value = 0;

            string[] temp_skillname = skill.Split(':');
            if (temp_skillname.Length > 1)
            {
                skill_value = int.Parse(temp_skillname[1]);
            }
            skill_name = temp_skillname[0];
            return (skill_name, skill_value);
        }
    }
}