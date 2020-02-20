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
            string outputPath = @"output/";
            string testFile = "a_example.txt";

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
                libraries.Add(new Library(libraryData[0], libraryData[1], libraryData[2],booksIndexes, i));
            }
            inputStream.Close();

            List<Library> finalLibraries = new List<Library>();
            List<int> remainingBooks = Enumerable.Range(0, booksScores.Count()).ToList();

            while (dayForScanning > 0 && libraries.Count() > 0 && remainingBooks.Count() > 0)
            {
                int bestLibraryIndex = BestLibrary(libraries, booksScores, dayForScanning, remainingBooks);
                finalLibraries.Add(libraries[bestLibraryIndex]);
                dayForScanning -= libraries[bestLibraryIndex].daysForSignupProcess;
                remainingBooks = remainingBooks.Except(libraries[bestLibraryIndex].orderedBooksIDs).ToList();

                libraries.RemoveAt(bestLibraryIndex);
            }

            Directory.CreateDirectory(outputPath);
            using (StreamWriter outputFile = new StreamWriter(new FileStream(string.Format("{0}{1}", outputPath, testFile), FileMode.Create)))
            {
                outputFile.WriteLine(finalLibraries.Count);
                foreach(Library library in finalLibraries)
                {
                    outputFile.WriteLine(string.Format("{0} {1}", library.id, library.orderedBooksIDs.Count));
                    outputFile.WriteLine(string.Join(" ", library.orderedBooksIDs));
                }
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

        public class Library
        {
            public int id;
            public int bookCount;
            public int daysForSignupProcess;
            public int shippingForDays;
            public List<int> booksIDs = new List<int>();
            public List<int> orderedBooksIDs = new List<int>();

            public Library(string bC, string suT, string ms,string[] booksIndexes, int index)
            {

                id = index;
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
                this.orderedBooksIDs = intersectionBooksIDs.OrderByDescending(i => booksScores[i]).Take(capacity).ToList();

                return score;
            }
        }

    }

}
