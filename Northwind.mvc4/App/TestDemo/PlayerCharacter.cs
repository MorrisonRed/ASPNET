using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppCore.TestDemo
{
    public class PlayerCharacter
    {
        public int Health { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public bool IsNoob { get; set; }
        public List<String> Weapons { get; set; }
        
        #region Constructors and Destructors
        public PlayerCharacter()
        {
            Name = GenerateName();
            IsNoob = true;
            CreateStartingWeapons();
        }
        #endregion

        #region Functions and Subroutines
        public void Sleep()
        {
            var rnd = new Random();
            var healthIncrease = rnd.Next(1, 101);
            Health += healthIncrease;
        }
        public void TakeDamage(int damage)
        {
            Health = Math.Max(1, Health -= damage);
        }
        public string GenerateName()
        {
            var names = new[]
            {
                "Daneith",
                "Derik",
                "Shalnoor",
                "G'Toth'lop", 
                "Boldrakteethtop"
            };

            return names[new Random().Next(0, names.Length)];
        }
        private void CreateStartingWeapons()
        {
            Weapons = new List<String>()
            {
                "Long Bow",
                "Short Bow",
                "Short Sword",
                //"Long Bow", 
                //"Staff Of Wonder"
            };
        }
        #endregion
    }
}