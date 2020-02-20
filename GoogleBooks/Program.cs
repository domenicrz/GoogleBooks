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
            string initialPath = @"input/";
            string outputPath = @"input/";
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

            foreach (var item in libraries)
            {
                Console.WriteLine(item.Score(booksScores.Select(i => int.Parse(i)).ToList(), deadline, Enumerable.Range(0, booksScores.Count() -1).ToList()));
            }


            using (StreamWriter outputFile = new StreamWriter(new FileStream(string.Format("{0}{1}", outputPath, resultFile), FileMode.Create)))
            {

            }
        }

        private class Library
        {
            int bookCount;
            int daysForSignupProcess;
            int shippingForDays;
            List<int> booksIDs = new List<int>();

            public Library(string bC, string suT, string ms,string[] booksIndexes)
            {
                bookCount = int.Parse(bC);
                daysForSignupProcess = int.Parse(suT);
                shippingForDays = int.Parse(ms);
                foreach (string id in booksIndexes)
                {
                    booksIDs.Add(int.Parse(id));
                }
            }

            public long Score(List<int> booksScores, int dayForScanning, List<int> remainingBooks)
            {

                List<int> intersectionBooksIDs = remainingBooks.Intersect(booksIDs).ToList();
                List<int> intersectionBooksScoresOrdered = intersectionBooksIDs.Select(index => booksScores[index]).OrderByDescending(item => item).ToList();

                var capacity = (dayForScanning - daysForSignupProcess) * shippingForDays;
                capacity = capacity > intersectionBooksIDs.Count() ? intersectionBooksIDs.Count : capacity;

                var score = intersectionBooksScoresOrdered.Take(capacity).Sum();

                return score;
            }
        }

    }

}
