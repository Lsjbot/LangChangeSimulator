using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace LangChangeSimulator
{
    class util
    {

        public static string ReplaceFirstOccurrence(string Source, string Find, string Replace)
        {
            int Place = Source.IndexOf(Find);
            string result = Source.Remove(Place, Find.Length).Insert(Place, Replace);
            return result;
        }

        public static string ReplaceLastOccurrence(string Source, string Find, string Replace)
        {
            int Place = Source.LastIndexOf(Find);
            string result = Source.Remove(Place, Find.Length).Insert(Place, Replace);
            return result;
        }

        public static T DeepCopy<T>(T obj) // from https://stackoverflow.com/questions/11336935/c-sharp-automatic-deep-copy-of-struct
        {
            BinaryFormatter s = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                s.Serialize(ms, obj);
                ms.Position = 0;
                T t = (T)s.Deserialize(ms);

                return t;
            }
        }

        public static string unusedfilename(string fn0)
        {
            int n = 1;
            string fn = fn0;
            while (File.Exists(fn))
            {
                fn = fn0.Replace(".", n.ToString() + ".");
                n++;
            }
            return fn;
        }


        public static int tryconvert(string word)
        {
            int i = -1;

            if (word.Length == 0)
                return i;

            try
            {
                i = Convert.ToInt32(word);
            }
            catch (OverflowException)
            {
                Console.WriteLine("i Outside the range of the Int32 type: " + word);
            }
            catch (FormatException)
            {
                //if ( !String.IsNullOrEmpty(word))
                //    Console.WriteLine("i Not in a recognizable format: " + word);
            }

            return i;

        }

        public static long tryconvertlong(string word)
        {
            long i = -1;

            if (word.Length == 0)
                return i;

            try
            {
                i = Convert.ToInt64(word);
            }
            catch (OverflowException)
            {
                Console.WriteLine("i Outside the range of the Int64 type: " + word);
            }
            catch (FormatException)
            {
                //Console.WriteLine("i Not in a recognizable long format: " + word);
            }

            return i;

        }

        public static double tryconvertdouble(string word)
        {
            double i = -1;

            if (word.Length == 0)
                return i;

            try
            {
                i = Convert.ToDouble(word.Replace(".", ","));
            }
            catch (OverflowException)
            {
                Console.WriteLine("i Outside the range of the Double type: " + word);
            }
            catch (FormatException)
            {
                try
                {
                    i = Convert.ToDouble(word);
                }
                catch (FormatException)
                {
                    //Console.WriteLine("i Not in a recognizable double format: " + word.Replace(".", ","));
                }
                //Console.WriteLine("i Not in a recognizable double format: " + word);
            }

            return i;

        }
        public static string initialcap(string orig)
        {
            if (String.IsNullOrEmpty(orig))
                return "";

            int initialpos = 0;
            if (orig.IndexOf("[[") == 0)
            {
                if ((orig.IndexOf('|') > 0) && (orig.IndexOf('|') < orig.IndexOf(']')))
                    initialpos = orig.IndexOf('|') + 1;
                else
                    initialpos = 2;
            }
            string s = orig.Substring(initialpos, 1);
            s = s.ToUpper();
            string final = orig;
            final = final.Remove(initialpos, 1).Insert(initialpos, s);
            //s += orig.Remove(0, 1);
            return final;
        }


    }
}
