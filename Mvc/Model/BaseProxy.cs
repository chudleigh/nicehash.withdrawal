using NLog;
using PureMVC.Interfaces;
using PureMVC.Patterns.Proxy;
using System.Threading.Tasks;

namespace Nicehash.Withdrawal.Mvc.Model
{
    public abstract class BaseProxy : Proxy, IProxy
    {
        /// <summary>
        /// Фасад приложения
        /// </summary>
        public new ApplicationFacade Facade => base.Facade as ApplicationFacade;

        /// <summary>
        /// Логгер
        /// </summary>
        protected NLog.Logger Logger => LogManager.GetLogger(GetType().Name);

        public BaseProxy(object data = null) : base(string.Empty, data)
        {
            ProxyName = GetType().GUID.ToString();
        }

        /// <summary>
        /// Выполняется при инициализации модуля
        /// </summary>
        public virtual Task<bool> OnInitialization() { return Task.FromResult(true); }

        /// <summary>
        /// Запуск модуля
        /// </summary>
        public virtual Task<bool> StartupAsync() { return Task.FromResult(true); }
    }
}