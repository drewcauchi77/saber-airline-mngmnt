using System.IO;

namespace Saber.AirlineBookingSystem.Domain.General
{
    public class RepositoryUtilities
    {
        private readonly Utilities _utilities = new();

        public void CheckForExistingFile(string path, string directory)
        {
            if (!File.Exists(path))
            {
                if (!Directory.Exists(path)) Directory.CreateDirectory(directory);
                using FileStream fs = File.Create(path);
            }
        }

        public void LoadFileLines(string directory, string fileName, Action<string[]> logic)
        {
            string path = Path.Combine(directory, fileName);

            try
            {
                CheckForExistingFile(path, directory);
                string[] lines = File.ReadAllLines(path);

                logic(lines);
            }
            catch (IndexOutOfRangeException iex)
            {
                _utilities.Log("Something went wrong with the file parsing, please check the data!", "red");
                _utilities.Log(iex.Message, "yellow");
            }
            catch (FileNotFoundException fnfe)
            {
                _utilities.Log("The file could not be found", "red");
                _utilities.Log(fnfe.Message, "yellow");
            }
            catch (Exception e)
            {
                _utilities.Log("Something went wrong, please try again!", "red");
                _utilities.Log(e.Message, "yellow");
            }
        }

        public void WriteFileLines(string directory, string fileName, Action<string> logic)
        {
            string path = Path.Combine(directory, fileName);

            try 
            {
                CheckForExistingFile(path, directory);

                string toAppend = string.Empty;
                logic(toAppend);
            }
            catch (IndexOutOfRangeException iex)
            {
                _utilities.Log("Something went wrong with the file parsing, please check the data!", "red");
                _utilities.Log(iex.Message, "yellow");
            }
            catch (FileNotFoundException fnfe)
            {
                _utilities.Log("The file could not be found", "red");
                _utilities.Log(fnfe.Message, "yellow");
            }
            catch (Exception e)
            {
                _utilities.Log("Something went wrong, please try again!", "red");
                _utilities.Log(e.Message, "yellow");
            }
        }
    }
}
