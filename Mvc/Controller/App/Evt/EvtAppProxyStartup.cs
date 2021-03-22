using Nicehash.Withdrawal.Mvc.Model.App.Msg;
using System.ComponentModel;

namespace Nicehash.Withdrawal.Mvc.Controller.App.Evt
{
    [Description("Core")]
    public class EvtAppProxyStartup : BaseEvent<MsgAppProxyStartup>
    {
        public override void Execute(MsgAppProxyStartup body)
        {
            if (body.StartupResult)
            {
                Logger.Info("Запуск модуля завершен: '{0}'", body.Proxy.GetType().Name);
            }
            else
            {
                Logger.Fatal("Ошибка запуска модуля: '{0}'. Текст ошибки: '{1}'", body.Proxy.GetType().Name, body.Exception.Message);
            }
        }
    }
}