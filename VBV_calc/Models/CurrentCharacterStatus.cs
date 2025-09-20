namespace VBV_calc.Models
{
    public class CurrentCharacterStatus
    {
        int hp;
        int soku;
        int kou;
        int bou;
        int chi;
        int cost;
        string rank;
        string shuzoku;
        string tokko;

        public CurrentCharacterStatus()
        {
            hp = 0;
            soku = 0;
            kou = 0;
            bou = 0;
            chi = 0;
            cost = 0;
            rank = "";
            shuzoku = "";
            tokko = "";
        }
        public void set_status(int hp, int kou, int bou, int soku, int chi, int cost, string rank, string shuzoku, string tokko)
        {
            this.hp = hp;
            this.soku = soku;
            this.kou = kou;
            this.bou = bou;
            this.chi = chi;
            this.cost = cost;
            this.rank = rank;
            this.shuzoku = shuzoku;
            this.tokko = tokko;
        }
        public void get_status(out int hp, out int kou, out int bou, out int soku, out int chi, out int cost, out string rank)
        {
            hp = this.hp;
            soku = this.soku;
            kou = this.kou;
            bou = this.bou;
            chi = this.chi;
            cost = this.cost;
            rank = this.rank;
        }
        public void get_shuzoku_tokko(out string shuzoku, out string tokko)
        {
            shuzoku = this.shuzoku;
            tokko = this.tokko;
        }

    }
}