using Nicehash.Withdrawal.Mvc.Model.App.Msg;
using System.ComponentModel;

namespace Nicehash.Withdrawal.Mvc.Controller.App.Evt
{
    [Description("Core")]
    public class EvtAppStartup : BaseEvent<MsgAppStartup>
    {
        public override void Execute(MsgAppStartup body)
        {
            if (body.StartupResult == false)
            {
                Logger.Fatal("При запуске приложения произошла ошибка");
            }
            else
            {
                Logger.Info("Запуск приложения завершен успешно");
            }
        }
    }
}