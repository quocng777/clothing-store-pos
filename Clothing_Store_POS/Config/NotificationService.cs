using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace Clothing_Store_POS.Config
{
    public static class NotificationService
    {
        public static void ShowNotification(string title, string message)
        {
            string toastXml = $@" <toast> <visual> <binding template='ToastGeneric'> <text> {title} </text> <text> {message} </text> </binding> </visual> </toast>";
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(toastXml);

            var toast = new ToastNotification(xmlDoc);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
