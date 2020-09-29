using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Xml;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace ConsoleBot
{
    class Program
    {
        static TelegramBotClient client;

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            client = new TelegramBotClient("1357650416:AAHwSAl43BQDVz4fCPuTvjOi8rnyTB8NNAE");

            client.OnMessage += ShowCurrensy;
            client.StartReceiving();

            //TimerCallback tm = new TimerCallback(Count);
            //Timer timer = new Timer(tm, null, 0, 5000);

            Console.Read();
        }
        public static void Count(object obj)
        {
            client.OnMessage += ShowCurrensy;
            client.StartReceiving();
        }
        private static void getMsg(object sender, MessageEventArgs e)
        {
            Console.WriteLine($"{e.Message.Text}");


            client.SendTextMessageAsync(e.Message.Chat.Id, "curent date & time : " + DateTime.Now.ToString());

        }

        public static void ShowCurrensy(object sender, MessageEventArgs e)
        {
            string url = "https://api.privatbank.ua/p24api/pubinfo?exchange&coursid=5";
            XmlTextReader streamReader = new XmlTextReader(url);
            StringBuilder currensy = new StringBuilder("Currensy now : ");



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
                        //Console.WriteLine(streamReader.GetAttribute("ccy") + " : " + streamReader.GetAttribute("base_ccy") + " : " + streamReader.GetAttribute("buy") + " : " + streamReader.GetAttribute("sale"));
                        //client.SendTextMessageAsync(e.Message.Chat.Id, streamReader.GetAttribute("ccy") + " : " + streamReader.GetAttribute("base_ccy") + " : " + streamReader.GetAttribute("buy") + " : " + streamReader.GetAttribute("sale"));

                    }
                    client.SendTextMessageAsync(e.Message.Chat.Id, tmp);
                    break;
                case "btn":
                    var markup = new ReplyKeyboardMarkup(new[] {
                new KeyboardButton("Curent date & Time"),
                new KeyboardButton("Currency"),
                new KeyboardButton("To do")
            });
                    client.SendTextMessageAsync(e.Message.Chat.Id, "Click button", replyMarkup: markup);
                    break;

            }
        }

    }
}
