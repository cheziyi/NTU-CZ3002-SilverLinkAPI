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
        public static bool Notify(ApplicationUser user, string title, string body, object data)
        {
            // Build Json message object to send to FCM server
            var message = JsonConvert.SerializeObject(new
            {
                to = user.DeviceId,
                notification = new
                {
                    body = body,
                    title = title
                },
                data = data
            });

            using (var client = new WebClient { UseDefaultCredentials = true })
            {
                // Send message to user's device using FCM
                client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                client.Headers.Add(HttpRequestHeader.Authorization, "key=AIzaSyAH4gxGLNIBNue0aQI4wea7rdrT4ftie3o");
                byte[] response = client.UploadData("https://fcm.googleapis.com/fcm/send", "POST", Encoding.UTF8.GetBytes(message));
            }

            // If notification is successfully sent, return true
            return true;
        }
    }
}