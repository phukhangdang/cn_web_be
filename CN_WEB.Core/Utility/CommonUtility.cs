﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace CN_WEB.Core.Utility
{
    public static class LinqExtension
    {
        public static IEnumerable<TResult> LeftJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector)
        {
            return outer.GroupJoin(inner, outerKeySelector, innerKeySelector, (outerResult, innerResult) => new { outerResult, innerResult })
                .SelectMany(xy => xy.innerResult.DefaultIfEmpty(), (x, y) => resultSelector.Invoke(x.outerResult, y));
        }

        public static IEnumerable<TSource> Distinct<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return source.GroupBy(keySelector).Select(x => x.First());
        }

        public static bool ContainsRelative(this string target, string filter)
        {
            var text = filter.Split("&").Select(x => x.ToLower().RemoveDiacritics());
            if (!string.IsNullOrEmpty(target) && target.ToLower().RemoveDiacritics().ContainsAny(text.ToArray()))
            {
                return true;
            }

            return false;
        }

        public static string RemoveDiacritics(this string text)
        {
            string result = text.ToLower();
            result = Regex.Replace(result, "à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ|/g", "a");
            result = Regex.Replace(result, "è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ|/g", "e");
            result = Regex.Replace(result, "ì|í|ị|ỉ|ĩ|/g", "i");
            result = Regex.Replace(result, "ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ|/g", "o");
            result = Regex.Replace(result, "ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ|/g", "u");
            result = Regex.Replace(result, "ỳ|ý|ỵ|ỷ|ỹ|/g", "y");
            result = Regex.Replace(result, "đ", "d");
            return result;
        }

        public static bool ContainsAny(this string haystack, params string[] needles)
        {
            foreach (string needle in needles)
            {
                if (haystack.Contains(needle))
                    return true;
            }

            return false;
        }

        public static bool EqualsAny(this string haystack, params string[] needles)
        {
            foreach (string needle in needles)
            {
                if (haystack == needle)
                    return true;
            }

            return false;
        }


    }

    public static class MailExtension
    {
        public static bool SendHtmlFormattedEmail(string subject, string body, List<string> locationPaths, MailConfig config)
        {
            try
            {
                var EmailToSend = config.Email;
                var EmailPassword = config.Password;
                var EmailToSendName = config.Email;
                var EmailPort = config.Port;
                var EmailHost = config.Host;
                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(EmailToSendName);
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;

                    if (locationPaths.Any())
                    {
                        foreach (var path in locationPaths)
                        {
                            mailMessage.Attachments.Add(new Attachment(path));
                        }
                    }

                    mailMessage.IsBodyHtml = true;
                    foreach (var mail in config.ReceiveEmail.Split(new char[] { ';' }))
                    {
                        mailMessage.To.Add(new MailAddress(mail));
                    }
                    SmtpClient smtp = new SmtpClient
                    {
                        Host = EmailHost,
                        EnableSsl = true
                    };
                    System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential
                    {
                        UserName = EmailToSend,
                        Password = EmailPassword
                    };
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = int.Parse(EmailPort);
                    smtp.Send(mailMessage);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static string BuildMailTemplate(string bodyTemp, List<MailParamData> data)
        {
            string body = bodyTemp;
            if (data != null && data.Any())
                foreach (var d in data)
                {
                    body = body.Replace("{{" + d.Key + "}}", d.Value);
                }
            return body;
        }
    }
    public class MailParamData
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
    public class MailConfig
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string ReceiveEmail { get; set; }
    }
}
