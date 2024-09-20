namespace MailDemo
{
    internal class Program
    {

        public static string ImapServer { get; set; } = string.Empty;
        public static string Email { get; set; } = string.Empty;
        public static string Password { get; set; } = string.Empty;
        public static int ImapPort { get; set; } = 993;


        static void Main(string[] args)
        {
            Console.WriteLine("Beginnt...");
            Console.WriteLine("***");

            Console.WriteLine("Email: ");
            string? email = null;
            while (email == null)
            {
                email = Console.ReadLine();
                Console.WriteLine("invalid input. Try again.");
            }
            Email = email;

            Console.WriteLine("***");
            Console.WriteLine("Password: ");
            string? pass = null;
            while (pass == null)
            {
                pass = Console.ReadLine();
                Console.WriteLine("invalid input. Try again.");
            }
            Password = pass;

            Console.WriteLine("***");
            Console.WriteLine("IMAP Server: ");
            string? iserver = null;
            while (iserver == null)
            {
                iserver = Console.ReadLine();
                Console.WriteLine("invalid input. Try again.");
            }
            ImapServer = iserver;

            Console.WriteLine("***");

        }
    }
}