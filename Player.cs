using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infiniminers_v0._0
{
    internal class Player
    {
        public int X {  get; set; }
        public int Y { get; set; }
        public int Money { get; set; }
        public int Mana {  get; set; }
        public int Speed { get; set; }
        public int Size {  get; set; }

        public Player(int startX, int startY)
        {
            X = startX;
            Y = startY;
            Money = 0;
            Mana = 100;
            Speed = 5;
            Size = 50;
        }

        public void Move(int dx, int dy)
        {
            X += dx * Speed;
            Y += dy * Speed;
        }

        public void Mine()
        {

        }

        public void UseMana (int amount)
        {
            Mana -= amount;

            if (Mana < 0) { Mana = 0; }
        }
    }
}
