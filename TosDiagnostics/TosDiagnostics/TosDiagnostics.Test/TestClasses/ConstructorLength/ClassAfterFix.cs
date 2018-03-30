using System;
using System.Collections.Generic;
using System.Text;

namespace ToSen.Test
{
    class ClassAfterFix
    {
        private string _labas = "";

        public ClassAfterFix(
            string firstName,
            string lastName,
            int age,
            int height,
            int weight)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            Height = height;
            Weight = weight;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }

        private string DoStuff()
        {
            string valueReturned = null;

            if (valueReturned == null)
            {
                valueReturned = 5.ToString();
            }

            return valueReturned;
        }
    }
}