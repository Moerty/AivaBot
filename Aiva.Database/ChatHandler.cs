﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database {
    public class ChatHandler {
        public static async void MessageReceived(object sender, TwitchLib.Events.Client.OnMessageReceivedArgs e) {
            using (var context = new DatabaseEntities()) {
                var Message = new Chat {
                    Name = e.ChatMessage.Username,
                    Message = e.ChatMessage.Message,
                    Timestamp = DateTime.Now.ToString(),
                };

                context.Chat.Add(Message);
                await context.SaveChangesAsync();
            }
        }
    }
}