using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Text;
using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

namespace RpiMailTrigger
{
    public static class MailTriggerFunction
    {
        public class Sensors
        {
            public double Temperature { get; set; }
            public double Humidity { get; set; }
            public bool Presence { get; set; }
        }

        [FunctionName("MailTriggerFunction")]
        public static void Run([IoTHubTrigger("messages/events", Connection = "ConnectionString")]EventData message, ILogger log)
        {
            SendMail(Encoding.UTF8.GetString(message.Body.Array), message.SystemProperties["iothub-connection-device-id"].ToString(),
                message.SystemProperties["iothub-enqueuedtime"].ToString(), log);
            log.LogInformation("Email sent");
        }

        public static void SendMail(string message, string device, string date, ILogger log)
        {
            Sensors sensors = JsonConvert.DeserializeObject<Sensors>(message);
            if (!sensors.Presence && sensors.Temperature < 30) return;

            log.LogInformation("Sending email...");

            MailMessage mailMessage = new MailMessage("EMAIL_USER_NAME", "EMAIL_USER_NAME");

            if (sensors.Presence)
            {
                mailMessage.Body = string.Format("The device {0} has detected a presence at {1}. " +
                    "If this presence is unexpected go to Azure Portal to take further actions.", device, date);
                mailMessage.Subject = string.Format("Possible intruder detected by {0}", device);
            } else if (sensors.Temperature > 30)
            {
                mailMessage.Body = string.Format("The device '{0}' has detected overheating. Temperature is '{1} ºC' " +
                    "If this presence is unexpected go to Azure Portal to take further actions.", device, sensors.Temperature);
                mailMessage.Subject = string.Format("Overheating detected by '{0}'", device);
            }

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.Credentials = new System.Net.NetworkCredential()
            {
                UserName = "EMAIL_USER_NAME",
                Password = "EMAIL_PASSWORD"
            };

            smtpClient.EnableSsl = true;

            try
            {
                smtpClient.Send(mailMessage);
            }
            catch
            {
                log.LogInformation("Email could not be sent: {}");
            }
        }
    }
}