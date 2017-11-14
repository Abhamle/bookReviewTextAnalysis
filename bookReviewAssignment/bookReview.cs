using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookReviewAssignment
{
    public class bookReview : IWordCounter
    {
        private static readonly char[] separators = { ' ' };
        private static string path = string.Empty;
        static void Main(string[] args)
        {
            //Merging Multiple textbooks into a single file for Charlette Text books       
            var inputDirectoryPath = @"D:\workspace\bookReviewProject\bookReviewAssignment\books\";
            var inputFileNamePatternC = "c*.txt";
            var outputFilePathC = @"D:\workspace\bookReviewProject\bookReviewAssignment\books\charloteeAll.txt";

            if (!File.Exists(outputFilePathC))
                CombineMultipleTextBooksIntoSingleFile(inputDirectoryPath, inputFileNamePatternC, outputFilePathC);


            //Merging Multiple textbooks into a single file for Jane Text books   
            var inputFileNamePatternJ = "j*.txt";
            var outputFilePathJ = @"D:\workspace\bookReviewProject\bookReviewAssignment\books\janeAll.txt";

            if (!File.Exists(outputFilePathJ))
                CombineMultipleTextBooksIntoSingleFile(inputDirectoryPath, inputFileNamePatternJ, outputFilePathJ);
           
            Console.WriteLine("Processing for top 10 Words mentioned in Charlet but not in Jane Text Books...");


            //Reading, and Adding to A wordCountC Dictionary for text refered from Charlete
            IDictionary<string, int> wordCountC = new Dictionary<string, int>();
            path = outputFilePathC;
            bookReview charleteBooks = new bookReview();
            wordCountC = charleteBooks.CountWords(path);

            //Reading, and Adding to A wordCountC Dictionary for text refered from Jane
            IDictionary<string, int> wordCountJ = new Dictionary<string, int>();
            path = outputFilePathJ;
            bookReview janeBooks = new bookReview();
            wordCountJ = janeBooks.CountWords(path);

            //This is link query to sort the dictionary in ascending order and exclude jane list and display the top 10. 

            var charleteOnlySorted = wordCountC.Where(c => !wordCountJ.Any(j => c.Key.Contains(j.Key)))
                            .OrderByDescending(x => x.Value)
                            .Take(10);

            //This is to display and format the result. 
            int counter = 1;
            foreach (KeyValuePair<string, int> kvp in charleteOnlySorted)
            {
                Console.WriteLine(" {0} ---- word = {1} ----- count = {2}", counter++, kvp.Key, kvp.Value);
            }
        }
        //This method is for creating word and counting there frequency/occurences
        private static void CombineMultipleTextBooksIntoSingleFile(string inputDirectoryPath, string inputFileNamePattern, string outputFilePath)
        {
            string[] inputFilePaths = Directory.GetFiles(inputDirectoryPath, inputFileNamePattern);
            Console.WriteLine("Number of files: {0}.", inputFilePaths.Length);
            using (var outputStream = File.Create(outputFilePath))
            {
                foreach (var inputFilePath in inputFilePaths)
                {
                    using (var inputStream = File.OpenRead(inputFilePath))
                    {
                        // Here for performance reasons, buffer size can be passed as the second argument.
                        inputStream.CopyTo(outputStream);
                    }
                    Console.WriteLine("The file {0} has been processed.", inputFilePath);
                }
            }
        }
        public IDictionary<string, int> CountWords(string path)
        {
            var wordCount = new Dictionary<string, int>();
            using (var fileStream = File.Open(path, FileMode.Open, FileAccess.Read))
            using (var streamReader = new StreamReader(fileStream))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    var words = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var word in words)
                    {
                        if (wordCount.ContainsKey(word))
                        {
                            wordCount[word]++;
                        }
                        else
                        {
                            wordCount.Add(word, 1);
                        }
                    }
                }
            }
            return wordCount;
        }

    }

}


