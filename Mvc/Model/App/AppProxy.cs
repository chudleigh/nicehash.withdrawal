using Nicehash.Withdrawal.Mvc.Controller.App.Evt;
using Nicehash.Withdrawal.Mvc.Model.App.Msg;

namespace Nicehash.Withdrawal.Mvc.Model.App
{
    /// <summary>
    /// Прокси для приложения
    /// </summary>
    public class AppProxy : BaseProxy
    {
        public override void OnRegister()
        {
            Facade.RegisterMessage<MsgAppProxyStartup>(new EvtAppProxyStartup());
            Facade.RegisterMessage<MsgAppStartup>(new EvtAppStartup());
        }
    }
}