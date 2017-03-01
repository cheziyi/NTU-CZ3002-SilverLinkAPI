using Newtonsoft.Json;
using SilverLinkAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace SilverLinkAPI.Controllers
{
    public static class FirebaseController
    {
        /// <summary>
        /// Method to send a push notification to user using Firebase Cloud Messaging (FCM)
        /// </summary>
        /// <param name="user">User object to notify</param>
        /// <param name="title">Title of notification</param>
        /// <param name="body">Content of notification</param>
        /// <returns>A bool true or false to indicate status of notification</returns>
        public static bool Notify(ApplicationUser user, string title, string body, FCMType type, int id)
        {
            if (user.DeviceId == null)
                return false;

            // Build Json message object to send to FCM server
            var message = JsonConvert.SerializeObject(new
            {
                to = user.DeviceId,
                notification = new
                {
                    body = body,
                    title = title
                },
                data = new
                {
                    type = type,
                    id = id
                }
            });

            using (var client = new WebClient { UseDefaultCredentials = true })
            {
                // Send message to user's device using FCM
                client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                client.Headers.Add(HttpRequestHeader.Authorization, "key=AAAA_Lss80Y:APA91bEKdIrZ4oeeoG4dw8kd9hExMb0pkvDAw1SwxzxMUik6RJGaoLL8FUkf9oq5t4XBWA4xz2WaMbrLn2Bez-ziIpii1IdfMPv7FlFBqoiuld6EGDXLagW_OXPXTv8zABTw_ROeK4S4");
                byte[] response = client.UploadData("https://fcm.googleapis.com/fcm/send", "POST", Encoding.UTF8.GetBytes(message));
            }

            // If notification is successfully sent, return true
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