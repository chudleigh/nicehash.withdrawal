using NLog;
using PureMVC.Interfaces;
using PureMVC.Patterns.Command;

namespace Nicehash.Withdrawal.Mvc.Controller
{
    public abstract class BaseEvent<TMessage> : SimpleCommand, ICommand where TMessage : BaseMessage
    {
        /// <summary>
        /// Фасад приложения
        /// </summary>
        protected new ApplicationFacade Facade => base.Facade as ApplicationFacade;

        /// <summary>
        /// Логгер
        /// </summary>
        protected NLog.Logger Logger => LogManager.GetLogger(GetType().Name);

        public override void Execute(INotification notification)
        {
            Execute(notification.Body as TMessage);
        }

        public abstract void Execute(TMessage body);
    }
}