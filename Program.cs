using Nicehash.Withdrawal.Logger;
using Nicehash.Withdrawal.Mvc;
using System;
using System.Threading;

namespace Nicehash.Withdrawal
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Console.Title = "NiceHash Withdrawal Bot by chudleigh";

            // Конфигурируем логгер
            ApplicationLogger.Configure();

            // Запустим инициализацию компонентов приложения
            ApplicationFacade.Instance.Startup();

            Thread.Sleep(Timeout.Infinite);
        }
    }
}