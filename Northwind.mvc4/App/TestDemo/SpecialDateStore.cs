using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppCore.TestDemo
{
    public class SpecialDateStore
    {
        public DateTime DateOf(SpecialDates specialDate)
        {
            switch (specialDate)
            {
                case SpecialDates.NewMilennium:
                    return new DateTime(2000, 1, 1, 0, 0, 0, 0);
                default:
                    throw new ArgumentOutOfRangeException("specialDate");
            }
        }
    }

    public enum SpecialDates
    {
        NewMilennium = 1
    }
}