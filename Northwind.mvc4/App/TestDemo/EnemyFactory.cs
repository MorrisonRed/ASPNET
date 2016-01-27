using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppCore.TestDemo
{
    public class EnemyFactory
    {
        public object Create(bool isBoss)
        {
            if (isBoss)
            {
                return new BossEnemy();
            }

            return new NormalEnemy();
        }
    }

    public abstract class Enemy
    {
        public string Name { get; set; }
    }
    public class BossEnemy : Enemy
    {
        public int ExtraPower
        {
            get { return 42; }
        }
    }
    public class NormalEnemy : Enemy
    {

    }
}