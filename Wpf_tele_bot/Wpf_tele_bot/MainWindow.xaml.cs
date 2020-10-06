using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace Wpf_tele_bot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static TelegramBotClient client;
        public MainWindow()
        {
            InitializeComponent();

            client = new TelegramBotClient("1357650416:AAHwSAl43BQDVz4fCPuTvjOi8rnyTB8NNAE");

            client.OnMessage += ShowCurrency;
            client.StartReceiving();

        }
        public static void Currency(object sender, MessageEventArgs e)
        {
            string url = "https://api.privatbank.ua/p24api/pubinfo?exchange&coursid=5";
            XmlTextReader streamReader = new XmlTextReader(url);
            string tmp = "";
            while (streamReader.Read())
            {
                if (streamReader.HasAttributes)
                {
                    tmp += (streamReader.GetAttribute("ccy") + "  | " + streamReader.GetAttribute("base_ccy") + " Продажа : " + streamReader.GetAttribute("buy") + " Покупка : " + streamReader.GetAttribute("sale") + "\n");
                }
            }
            client.SendTextMessageAsync(e.Message.Chat.Id, tmp);
        }



        private void getMsg(object sender, MessageEventArgs e)
        {
           

            client.SendTextMessageAsync(e.Message.Chat.Id, "curent date & time : " + DateTime.Now.ToString());

        }

        public  void ShowCurrency(object sender, MessageEventArgs e)
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
                case "Data update every 5 min":

                    break;
                case "Data update every 10 min":

                    break;
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
