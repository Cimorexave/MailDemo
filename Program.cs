using MailKit;
using MailKit.Net.Imap;
using MimeKit;
using System.IO;

namespace MailDemo
{
    internal class Program
    {

        public static string ImapServer { get; set; } = string.Empty;
        public static string Email { get; set; } = string.Empty;
        public static string Password { get; set; } = string.Empty;
        public static string SaveDirectory { get; set; } = string.Empty;
        public static int ImapPort { get; set; } = 993;
        public static ImapClient? ImapClient { get; set; }

        static void GetCredentials()
        {
            string? email = null;
            Console.WriteLine("Email: ");
            email = Console.ReadLine();
            while (email == null)
            {
                email = Console.ReadLine();
                Console.WriteLine("invalid input. Try again.");
            }
            Email = email;
            Console.WriteLine("***");

            string? pass = null;
            Console.WriteLine("Password: ");
            pass = Console.ReadLine();
            while (pass == null)
            {
                pass = Console.ReadLine();
                Console.WriteLine("invalid input. Try again.");
            }
            Password = pass;
            Console.WriteLine("***");

            string? iserver = null;
            Console.WriteLine("IMAP Server: ");
            iserver = Console.ReadLine();
            while (iserver == null)
            {
                iserver = Console.ReadLine();
                Console.WriteLine("invalid input. Try again.");
            }
            ImapServer = iserver;
            Console.WriteLine("***");

            string? saveDir = null;
            Console.WriteLine("Save Directory (C:\\YourPath\\Attachments\\): ");
            saveDir = Console.ReadLine();
            while (saveDir == null || !Directory.Exists(saveDir))
            {
                saveDir = Console.ReadLine();
                Console.WriteLine("invalid input. Try again.");
            }
            SaveDirectory = saveDir;
            Console.WriteLine("***");

        }
        static bool InitializeImapClient()
        {
            // Verbindung zu IMAP-Server herstellen
            using ImapClient client = new();
            try
            {
                PrintCredentials();
                // Use SSL
                client.Connect(ImapServer, ImapPort, true);
                Console.WriteLine("Connected...");


                // Authenticate
                client.Authenticate(Email, Password);
                Console.WriteLine("Authenticated...");

                // Open inbox
                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadWrite); // Read/write access so we can move

                Console.WriteLine($"Number of messages: {inbox.Count}");

                // Iterate through messages (e.g. only the first message for the example)
                for (int i = inbox.Count - 1; i > inbox.Count - 11; i--)
                {
                    var message = inbox.GetMessage(i);
                    Console.WriteLine($"Message from: {message.From}");
                    Console.WriteLine($"Subject: {message.Subject}");
                    Console.WriteLine($"Date: {message.Date}");
                    Console.WriteLine($"Text: {message.TextBody}");
                    Console.WriteLine("----------------------");

                    // Save attachments
                    if (message.Attachments.Any())
                        foreach (var attachment in message.Attachments)
                        {
                            if (attachment is MimePart part)
                            {
                                var fileName = part.FileName;

                                // Make sure the directory exists
                                Directory.CreateDirectory(SaveDirectory);

                                var filePath = Path.Combine(SaveDirectory, fileName);
                                Console.WriteLine($"filepath: {filePath}");

                                // Save file
                                using (var stream = File.Create(filePath))
                                {
                                    part.Content.DecodeTo(stream);
                                }

                                Console.WriteLine($"Attachment saved: {filePath}");
                            }
                        }


                    //// Move messages
                    //IMailFolder root = client.GetFolder(client.PersonalNamespaces[0]);
                    //IMailFolder? destinationFolder = client.GetFolder("INBOX/Messages");

                    //// If the folder does not exist yet, you can create it:
                    //if (destinationFolder == null)
                    //{
                    //    destinationFolder = root.Create("INBOX/Messages", true);
                    //}

                    //destinationFolder.Open(FolderAccess.ReadWrite);

                    //// Move message
                    //inbox.MoveTo(i, destinationFolder);

                    //Console.WriteLine("Message has been moved to the 'Messages' folder.");
                }

                // Disconnect
                client.Disconnect(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }

            return true;
        }

        private static void PrintCredentials()
        {
            Console.WriteLine($"Email: {Email}");
            Console.WriteLine($"Password: {Password}");
            Console.WriteLine($"IMAP Server: {ImapServer}");
            Console.WriteLine($"IMAP Port: {ImapPort}");
            Console.WriteLine($"Save Directory: {SaveDirectory}");
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Beginnt...");
            Console.WriteLine("***");

            GetCredentials();
            if (!InitializeImapClient()) Console.WriteLine("Something Went Wrong. Tray again Later.");

            Console.WriteLine("\nFinished Work...");
            Console.ReadLine();
        }
    }
}