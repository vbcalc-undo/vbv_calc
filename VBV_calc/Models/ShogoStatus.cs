namespace VBV_calc.Models
{
    public class ShogoStatus
    {
        public int kougeki;
        public int bougyo;
        public int sokudo;
        public int tiryoku;
        public string tokko;

        public ShogoStatus(int kougeki, int bougyo, int sokudo, int tiryoku, string tokko)
        {
            this.kougeki = kougeki;
            this.bougyo = bougyo;
            this.sokudo = sokudo;
            this.tiryoku = tiryoku;
            this.tokko = tokko;
        }

        public void change_status(int kougeki, int bougyo, int sokudo, int tiryoku)
        {
            this.kougeki = kougeki;
            this.bougyo = bougyo;
            this.sokudo = sokudo;
            this.tiryoku = tiryoku;
        }
    }
}