﻿using System.Text.RegularExpressions;

namespace Service.Helpers
{
    public static class MessageHelper
    {
        public static string ReplacePlaceholder(string text, string placeholder, string value)
        {
            var pattern = $"%{placeholder}%";
            text = Regex.Replace(text, pattern, value);
            return text;
        }

        public static bool ContainsPlaceholder(string text, string value)
        {
            return text.Contains($"%{value}%");
        }

        public static string CreateMessageValue<T>(string template, T valueObj, List<string> placeholders)
        {
            var props = typeof(T).GetProperties();
            var message = template;
            foreach (var placeholder in placeholders)
            {
                if (ContainsPlaceholder(template, placeholder))
                {
                    var val = props.FirstOrDefault(x => x.Name.ToLower() == placeholder).GetValue(valueObj).ToString();
                    message = ReplacePlaceholder(message, placeholder, val);
                }
            }

            return message;
        }

        public static TMessage CreateMessage<TMessage, TObject, TTemplate>(TTemplate template, TObject valueObj, List<string> placeholders)
        {
            var messageType = typeof(TMessage);
            var message = (TMessage)Activator.CreateInstance(messageType);
            var messageProps = messageType.GetProperties();
            var templateProps = typeof(TTemplate).GetProperties();
            
            foreach(var prop in messageProps)
            {
                var templateVal = templateProps.FirstOrDefault(x => x.Name == prop.Name).GetValue(template).ToString();
                prop.SetValue(message, CreateMessageValue(templateVal, valueObj, placeholders));
            }

            return message;
        }
    }
}