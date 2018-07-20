using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StartWebSiteEnglish.Models
{
    public class Quastion<T>
    {
        public T quastion { get; set; }

        public T[] answers { get; set; }

        public int correctAnwer { get; set; } = 0;

        public Quastion(T quest,  T[] words, int correct = 0)
        {
            quastion = quest;
            correctAnwer = correct;
            answers = words;
        }

        public bool ReplyCheck(int check)
        {
            if (correctAnwer == check)
                return true;
            return false;
        }
    }

    public class TestModel<T>
    {
        public List<Quastion<T>> quastions { get; set; }

        public int countCorrentAnswer { get; set; }


        //public string ResultRating()
        //{
        //    if (countCorrentAnswer == quastions.Count)
        //        return "good";
        //    return "bad";
        //}
    }
}