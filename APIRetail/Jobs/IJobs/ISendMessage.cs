namespace APIRetail.Jobs.IJobs
{
    public interface ISendMessage
    {
        void SendDataWhatsApp();
        void SendDataSMS();
        void SendDataEmail();
    }
}
