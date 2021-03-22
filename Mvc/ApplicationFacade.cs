using Nicehash.Withdrawal.Mvc.Model;
using Nicehash.Withdrawal.Mvc.Model.App.Msg;
using PureMVC.Interfaces;
using PureMVC.Patterns.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Nicehash.Withdrawal.Mvc
{
    public class ApplicationFacade : Facade
    {
        /// <summary>
        /// Фасад прложения
        /// </summary>
        public static ApplicationFacade Instance => GetInstance(() => new ApplicationFacade()) as ApplicationFacade;

        /// <summary>
        /// Регистрация сообщения
        /// </summary>
        public void RegisterMessage<TMessage>(ICommand commad) where TMessage : BaseMessage
        {
            RegisterCommand(typeof(TMessage).GUID.ToString(), () => commad);
        }

        /// <summary>
        /// Получение прокси
        /// </summary>
        public TProxy RetrieveProxy<TProxy>() where TProxy : BaseProxy
        {
            return RetrieveProxy(typeof(TProxy).GUID.ToString()) as TProxy;
        }

        /// <summary>
        /// Отправка сообщения
        /// </summary>
        public void SendMessage<TMessage>(TMessage obj) where TMessage : BaseMessage
        {
            SendNotification(obj.Key, obj);
        }

        /// <summary>
        /// Запуск приложения. Настройка и запуск прокси
        /// </summary>
        public void Startup()
        {
            // Список всех прокси
            var proxyList = GetAssemblyProxyList();

            // Инициализация прокси
            proxyList.ForEach(x => { x.OnInitialization(); });

            // Первоначальный запуск
            var result = proxyList.Select(async x => await StartupProxyAsync(x)).All(x => x.Result);

            SendMessage(new MsgAppStartup(result));
        }

        /// <summary>
        /// Формирование списка модулей приложения
        /// </summary>
        private List<BaseProxy> GetAssemblyProxyList()
        {
            // Список всех типов наших прокси
            var proxyTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(type => type.IsSubclassOf(typeof(BaseProxy)))
                .Where(type => type.IsAbstract == false);

            // Список всех прокси
            var proxyList = proxyTypes.Select(x => Activator.CreateInstance(x) as BaseProxy).ToList();

            // Регистрируем прокси
            proxyList.ForEach(x => { RegisterProxy(x); });

            return proxyList;
        }

        /// <summary>
        /// Первоначальный запуск модулей
        /// </summary>
        private async Task<bool> StartupProxyAsync(BaseProxy proxy)
        {
            try
            {
                var result = await proxy.StartupAsync();
                SendMessage(new MsgAppProxyStartup(proxy, result));
                return result;
            }
            catch (Exception ex)
            {
                SendMessage(new MsgAppProxyStartup(proxy, false, ex));
                return false;
            }
        }
    }
}