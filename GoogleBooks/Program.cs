using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleBooks
{
    class Program
    {
        static void Main(string[] args)
        {
            string initialPath = @"D:\Users\domen\source\repos\GoogleBooks\GoogleBooks\input\";
            string outputPath = @"D:\Users\domen\source\repos\GoogleBooks\GoogleBooks\output\";
            string testFile = "f_libraries_of_the_world.txt";
            string resultFile = ".txt";

            StreamReader inputStream = new StreamReader(new FileStream(string.Format("{0}{1}", initialPath, testFile), FileMode.Open));
            string[] firstLine = inputStream.ReadLine().Split(' ');
            int booksTypes = int.Parse(firstLine[0]);
            int librariesNumber = int.Parse(firstLine[1]);
            int deadline = int.Parse(firstLine[2]);
            string[] booksScores = inputStream.ReadLine().Split(' ');
            List<Library> libraries = new List<Library>();
            for (int i = 0; i<librariesNumber; i++)
            {
                string[] libraryData = inputStream.ReadLine().Split(' ');
                string[] booksIndexes = inputStream.ReadLine().Split(' ');
                libraries.Add(new Library(libraryData[0], libraryData[1], libraryData[2],booksIndexes));
            }
            inputStream.Close();
            

            using (StreamWriter outputFile = new StreamWriter(new FileStream(string.Format("{0}{1}", outputPath, resultFile), FileMode.Create)))
            {

            }
        }

        private class Library
        {
            int bookCount;
            int singUpTime;
            int maxSend;
            List<int> booksId = new List<int>();
            double libScore = 0;

            public Library(string bC, string suT, string ms,string[] booksIndexes)
            {
                bookCount = int.Parse(bC);
                singUpTime = int.Parse(suT);
                maxSend = int.Parse(ms);
                foreach (string id in booksIndexes)
                {
                    booksId.Add(int.Parse(id));
                }
            }
        }

    }

}
