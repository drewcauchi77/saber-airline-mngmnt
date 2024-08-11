namespace Saber.AirlineBookingSystem.Domain.General
{
    public class Utilities
    {
        public void Log(string message, string colour = "white", bool isLine = true)
        {
            if (colour == "yellow") Console.ForegroundColor = ConsoleColor.Yellow;
            else if (colour == "green") Console.ForegroundColor = ConsoleColor.Green;
            else if (colour == "red") Console.ForegroundColor = ConsoleColor.Red;

            if (isLine)
                Console.WriteLine(message);
            else
                Console.Write(message);

            if (colour != "white") Console.ResetColor();
        }

        public int GetValidIntInput(string prompt, Func<int, bool> validationFunction, string errorMessage)
        {
            int input;
            bool isValid;
            bool isParseSuccess;

            do
            {
                Log(prompt, "yellow", false);
                isParseSuccess = int.TryParse(Console.ReadLine(), out input);
                if (!isParseSuccess) Log("There has been an error parsing your input - try again!", "red");
                isValid = validationFunction(input);
                if (!isValid) Log(errorMessage, "red");
            } while (!isValid || !isParseSuccess);

            return input;
        }

        public string GetValidStringInput(string prompt, Func<string, bool> validationFunction, string errorMessage)
        {
            string input;
            bool isValid;

            do
            {
                Log(prompt, "yellow", false);
                input = Console.ReadLine() ?? string.Empty;
                isValid = validationFunction(input);
                if (!isValid) Log(errorMessage, "red");
            } while (!isValid);

            return input;
        }

        private bool isValidEmail(string email)
        {
            if (email.EndsWith('.')) return false;
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address.Equals(email);
            }
            catch
            {
                return false;
            }
        }

        public string GetValidEmailInput(string prompt, Func<string, bool> validationFunction, string errorMessage)
        {
            string input;
            bool isValid;

            do
            {
                Log(prompt, "yellow", false);
                input = Console.ReadLine() ?? string.Empty;
                isValid = validationFunction(input) && isValidEmail(input.Trim());
                if (!isValid) Log(errorMessage, "red");
            } while (!isValid);

            return input;
        }

        public T GetValidEnumInput<T>(string prompt, string errorMessage) where T : struct
        {
            T input;
            bool isValid;
            bool isParseSuccess;

            do
            {
                Log(prompt, "yellow", false);
                isParseSuccess = Enum.TryParse(Console.ReadLine(), out input);
                if (!isParseSuccess) Log("There has been an error parsing your input - try again!", "red");
                isValid = Enum.IsDefined(typeof(T), input);
                if (!isValid) Log(errorMessage, "red");
            } while (!isValid || !isParseSuccess);

            return input;
        }

        public DateOnly GetValidDateOnlyInput(string prompt, string errorMessage)
        {
            DateOnly input;
            bool isParseSuccess;

            do
            {
                Log(prompt, "yellow", false);
                isParseSuccess = DateOnly.TryParse(Console.ReadLine(), out input);
                if (!isParseSuccess) Log("There has been an error parsing your input - try again!", "red");
            } while (!isParseSuccess);

            return input;
        }

        public void ShowEnumList<T>() where T : struct, Enum
        {
            var list = Enum.GetValues(typeof(T));

            foreach (T fT in list)
            {
                Log($"Enter '{Convert.ToInt32(fT)}' for ", "white", false);
                Log(fT.ToString(), "yellow");
            }
        }
    }
}
