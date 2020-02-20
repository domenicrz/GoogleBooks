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
            string testFile = "a_example.txt";
            string resultFile = ".txt";

            StreamReader inputStream = new StreamReader(new FileStream(string.Format("{0}{1}", initialPath, testFile), FileMode.Open));
            string[] firstLine = inputStream.ReadLine().Split(' ');
            int booksTypes = int.Parse(firstLine[0]);
            int librariesNumber = int.Parse(firstLine[1]);
            int dayForScanning = int.Parse(firstLine[2]);
            List<int> booksScores = inputStream.ReadLine().Split(' ').Select(i => int.Parse(i)).ToList();
            List<Library> libraries = new List<Library>();
            for (int i = 0; i<librariesNumber; i++)
            {
                string[] libraryData = inputStream.ReadLine().Split(' ');
                string[] booksIndexes = inputStream.ReadLine().Split(' ');
                libraries.Add(new Library(libraryData[0], libraryData[1], libraryData[2],booksIndexes));
            }
            inputStream.Close();

            List<Library> finalLibraries = new List<Library>();
            List<int> remainingBooks = Enumerable.Range(0, booksScores.Count()).ToList();

            while (dayForScanning > 0 && libraries.Count() > 0 && remainingBooks.Count() > 0)
            {
                int bestLibraryIndex = BestLibrary(libraries, booksScores, dayForScanning, remainingBooks);
                finalLibraries.Add(libraries[bestLibraryIndex]);
                dayForScanning -= libraries[bestLibraryIndex].daysForSignupProcess;
                remainingBooks = remainingBooks.Except(libraries[bestLibraryIndex].ordereBooksIDs).ToList();

                libraries.RemoveAt(bestLibraryIndex);
            }


            using (StreamWriter outputFile = new StreamWriter(new FileStream(string.Format("{0}{1}", outputPath, resultFile), FileMode.Create)))
            {

            }
        }

        private static int BestLibrary(List<Library> remaningLibraries, List<int> booksScores, int dayForScanning, List<int> remainingBooks)
        {
            List<int> librariesScores = new List<int>();
            foreach (var library in remaningLibraries)
            {
                librariesScores.Add(library.Score(booksScores, dayForScanning, remainingBooks));
            }
            return librariesScores.IndexOf(librariesScores.Max());

        }

        private class Library
        {
            public int bookCount;
            public int daysForSignupProcess;
            public int shippingForDays;
            public List<int> booksIDs = new List<int>();
            public List<int> ordereBooksIDs = new List<int>();

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

            public int Score(List<int> booksScores, int dayForScanning, List<int> remainingBooks)
            {

                List<int> intersectionBooksIDs = remainingBooks.Intersect(booksIDs).ToList();
                List<int> intersectionBooksScoresOrdered = intersectionBooksIDs.Select(index => booksScores[index]).OrderByDescending(item => item).ToList();

                var capacity = (dayForScanning - daysForSignupProcess) * shippingForDays;
                capacity = capacity > intersectionBooksIDs.Count() ? intersectionBooksIDs.Count : capacity;

                var score = intersectionBooksScoresOrdered.Take(capacity).Sum();
                this.ordereBooksIDs = intersectionBooksIDs.OrderByDescending(i => booksScores[i]).Take(capacity).ToList();

                return score;
            }
        }

    }

}
