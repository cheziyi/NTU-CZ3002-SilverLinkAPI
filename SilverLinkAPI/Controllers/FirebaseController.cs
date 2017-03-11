using System.Net;
using System.Text;
using Newtonsoft.Json;
using SilverLinkAPI.Models;

namespace SilverLinkAPI.Controllers
{
    public static class FirebaseController
    {
        public static bool Notify(ApplicationUser user, string title, string body, FCMType type, int id)
        {
            if (user.DeviceId == null)
                return false;

            var message = JsonConvert.SerializeObject(new
            {
                to = user.DeviceId,
                notification = new
                {
                    body,
                    title
                },
                data = new
                {
                    type,
                    id
                }
            });

            using (var client = new WebClient {UseDefaultCredentials = true})
            {
                client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                client.Headers.Add(HttpRequestHeader.Authorization,
                    "key=AAAA_Lss80Y:APA91bEKdIrZ4oeeoG4dw8kd9hExMb0pkvDAw1SwxzxMUik6RJGaoLL8FUkf9oq5t4XBWA4xz2WaMbrLn2Bez-ziIpii1IdfMPv7FlFBqoiuld6EGDXLagW_OXPXTv8zABTw_ROeK4S4");
                var response = client.UploadData("https://fcm.googleapis.com/fcm/send", "POST",
                    Encoding.UTF8.GetBytes(message));
            }
            return true;
        }
    }

    public enum FCMType
    {
        LocationRequest = 0,
        FriendAdded = 1,
        FriendMessage = 2,
        GroupMessage = 3,
        PanicEvent = 4
    }
}