using APIRetail.Models.Database;
using APIRetail.Repository.IRepository;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace APIRetail.Helper
{
    public class SendDataMessageWhatsApp
    {
        public readonly IConfiguration _configuration;
        public readonly ILogError _logError;
        public readonly retail_systemContext _context;
        public SendDataMessageWhatsApp(IConfiguration Configuration, ILogError logError, retail_systemContext context)
        {
            _configuration = Configuration;
            _logError = logError;
            _context = context;
        }

        public async void SendDataWhatsAppAsync(string? SendMessage, string? NoWhatsApp)
        {
            try
            {
                CancellationToken cancellationToken = new CancellationToken();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration.GetValue<string>("ScheduleJob:UriSendWhatsApp"));
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.PostAsJsonAsync(_configuration.GetValue<string>("ScheduleJob:APISendWhatsApp"), "");
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        var resultJson = JsonConvert.DeserializeObject<object>(jsonString);

                    }
                    else
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        var resultJson = JsonConvert.DeserializeObject<object>(jsonString);

                        SendWhatsappFail sendWhatsAppFailRequest = new SendWhatsappFail();
                        sendWhatsAppFailRequest.NoWhatsApp = NoWhatsApp;
                        sendWhatsAppFailRequest.Message = SendMessage;
                        sendWhatsAppFailRequest.ErrorDescription = jsonString;
                        sendWhatsAppFailRequest.CreateBy = "System";
                        sendWhatsAppFailRequest.CreateDate = DateTime.Now;
                        _context.SendWhatsappFail.Add(sendWhatsAppFailRequest);
                        await _context.SaveChangesAsync();

                    }
                }
            }

            catch (Exception ex)
            {

                SendWhatsappFail sendWhatsAppFailRequest = new SendWhatsappFail();
                sendWhatsAppFailRequest.NoWhatsApp = NoWhatsApp;
                sendWhatsAppFailRequest.Message = SendMessage;
                sendWhatsAppFailRequest.ErrorDescription = "Error SendDataWhatsAppAsync : " + ex.Message;
                sendWhatsAppFailRequest.CreateBy = "System";
                sendWhatsAppFailRequest.CreateDate = DateTime.Now;
                _context.SendWhatsappFail.Add(sendWhatsAppFailRequest);
                await _context.SaveChangesAsync();
            }
        }
    }
}
