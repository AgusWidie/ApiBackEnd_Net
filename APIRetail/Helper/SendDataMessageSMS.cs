using APIRetail.Models.Database;
using APIRetail.Repository.IRepository;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace APIRetail.Helper
{
    public class SendDataMessageSMS
    {
        public readonly IConfiguration _configuration;
        public readonly ILogError _logError;
        public readonly retail_systemContext _context;
        public SendDataMessageSMS(IConfiguration Configuration, ILogError logError, retail_systemContext context)
        {
            _configuration = Configuration;
            _logError = logError;
            _context = context;
        }

        public async void SendDataSMSAsync(string? SendMessage, string? NoSMS)
        {
            try
            {
                CancellationToken cancellationToken = new CancellationToken();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration.GetValue<string>("ScheduleJob:UriSendSMS"));
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.PostAsJsonAsync(_configuration.GetValue<string>("ScheduleJob:APISendSMS"), "");
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        var resultJson = JsonConvert.DeserializeObject<object>(jsonString);

                    }
                    else
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        var resultJson = JsonConvert.DeserializeObject<object>(jsonString);

                        SendSmsFail sendSmsFailRequest = new SendSmsFail();
                        sendSmsFailRequest.NoSms = NoSMS;
                        sendSmsFailRequest.Message = SendMessage;
                        sendSmsFailRequest.ErrorDescription = jsonString;
                        sendSmsFailRequest.CreateBy = "System";
                        sendSmsFailRequest.CreateDate = DateTime.Now;
                        _context.SendSmsFail.Add(sendSmsFailRequest);
                        await _context.SaveChangesAsync();
                    }
                }
            }

            catch (Exception ex)
            {
                SendSmsFail sendSmsFailRequest = new SendSmsFail();
                sendSmsFailRequest.NoSms = NoSMS;
                sendSmsFailRequest.Message = SendMessage;
                sendSmsFailRequest.ErrorDescription = "Error SendDataSMSAsync : " + ex.Message;
                sendSmsFailRequest.CreateBy = "System";
                sendSmsFailRequest.CreateDate = DateTime.Now;
                _context.SendSmsFail.Add(sendSmsFailRequest);
                await _context.SaveChangesAsync();
            }

        }
    }
}
