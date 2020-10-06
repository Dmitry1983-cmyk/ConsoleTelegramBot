using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using Telegram.Bot;
using System.Xml;

namespace ConsoleTelegramBot
{
    class CurrencySender
    {
        static TelegramBotClient client = new TelegramBotClient("1357650416:AAHwSAl43BQDVz4fCPuTvjOi8rnyTB8NNAE");

       Timer timer;
      public  long telegramId;

        public CurrencySender(long telegraID, int p)
        {
            this.telegramId = telegraID;
            timer = new Timer(p);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }


        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            string url = "https://api.privatbank.ua/p24api/pubinfo?exchange&coursid=5";
            XmlTextReader streamReader = new XmlTextReader(url);
            string tmp_currency = "";
            while (streamReader.Read())
            {
                if (streamReader.HasAttributes)
                {
                    tmp_currency += (streamReader.GetAttribute("ccy") + "  | " + streamReader.GetAttribute("base_ccy") + " Продажа : " + streamReader.GetAttribute("buy") + " Покупка : " + streamReader.GetAttribute("sale") + "\n");
                }
            }
            client.SendTextMessageAsync(telegramId,tmp_currency);
        }

        public void ChangeTime(int inter)
        {
            timer.Interval = inter;
        }

    }
}
