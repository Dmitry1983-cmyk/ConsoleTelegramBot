using System;
using System.Collections.Generic;
using System.Timers;
using System.Text;
using System.Threading;
using System.Xml;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using ConsoleTelegramBot;
using System.IO;
using System.Data.SqlClient;

namespace ConsoleBot
{
    class Program
    {
        static TelegramBotClient client;
        static List<CurrencySender> collection = new List<CurrencySender>();


        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;


            CreateCategoryPhoto();

            client = new TelegramBotClient("1357650416:AAHwSAl43BQDVz4fCPuTvjOi8rnyTB8NNAE");

            client.OnMessage += ShowCurrency;
            client.StartReceiving();

            Console.Read();
        }

        public static void CreateCategoryPhoto()
        {
            using (SqlConnection connection = new SqlConnection("Server=tcp:barabash-server.database.windows.net,1433;Initial Catalog=Barabash_DB;Persist Security Info=False;User ID=barabash;Password=F99n5viK;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM [dbo].[Category]", connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (!Directory.Exists($"{Directory.GetCurrentDirectory().ToString()}\\Images\\{reader.GetValue(1).ToString()}"))
                        {
                            try
                            {
                                Directory.CreateDirectory($"{Directory.GetCurrentDirectory().ToString()}\\Images\\{reader.GetValue(1).ToString()}");
                                //client.SendPhotoAsync(e.Message.Chat.Id, photo: "", caption: "");//(int channelId, photo: "http://abc.jpeg", caption: "hii")
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }
            }
        }

        private static CurrencySender SerchId(long p)
        {
            foreach (var item in collection)
            {
                if (item.telegramId == p)
                {
                    return item;
                }
            }
            return null;
        }

        public static void ShowCurrency(object sender, MessageEventArgs e)
        {
            string url = "https://api.privatbank.ua/p24api/pubinfo?exchange&coursid=5";
            XmlTextReader streamReader = new XmlTextReader(url);


            switch (e.Message.Text)
            {
                case "Curent date & Time":
                    client.SendTextMessageAsync(e.Message.Chat.Id, "curent date & time : " + DateTime.Now.ToString());
                    break;
                case "Currency":
                    string tmp = "";
                    while (streamReader.Read())
                    {
                        if (streamReader.HasAttributes)
                        {
                            tmp += (streamReader.GetAttribute("ccy") + "  | " + streamReader.GetAttribute("base_ccy") + " Продажа : " + streamReader.GetAttribute("buy") + " Покупка : " + streamReader.GetAttribute("sale") + "\n");
                        }
                    }
                    client.SendTextMessageAsync(e.Message.Chat.Id, tmp);
                    break;
                case "Data update every 5 min":
                    {
                        CurrencySender user = SerchId(e.Message.Chat.Id);
                        if (user == null)
                        {
                            user = new CurrencySender(e.Message.Chat.Id, 50);
                            collection.Add(user);
                        }
                        user.ChangeTime(60000 * 5);
                        break;
                    }
                case "Data update every 10 min":
                    {
                        CurrencySender user = SerchId(e.Message.Chat.Id);
                        if (user == null)
                        {
                            user = new CurrencySender(e.Message.Chat.Id, 50);
                            collection.Add(user);
                        }
                        user.ChangeTime(600000);
                        break;
                    }
                case "btn":

                    var markup = new ReplyKeyboardMarkup(new[] {
                new KeyboardButton("Curent date & Time"),
                new KeyboardButton("Currency"),
                new KeyboardButton("Data update every 5 min"),
                new KeyboardButton("Data update every 10 min")
            });

                    client.SendTextMessageAsync(e.Message.Chat.Id, "Click button", replyMarkup: markup);
                    break;

            }
        }

    }
}
