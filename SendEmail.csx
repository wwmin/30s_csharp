#r "nuget:FluentEmail.Smtp/2.8.0"
using System.Net;
using System.Net.Mail;
using FluentEmail;
using FluentEmail.Core;
using FluentEmail.Smtp;
string myEmailAddress = "wwei.min@163.com";
string myEmailSmtpPassword = "";//从163中开启Smtp服务并新增授权密码
string toEmailAddress = "294134540@qq.com";
SmtpClient smtp = new SmtpClient
{
    EnableSsl = false,
    Host = "smtp.163.com",
    UseDefaultCredentials = false,
    DeliveryMethod = SmtpDeliveryMethod.Network,
    //这里输入你在发送smtp服务器的用户名和密码
    Credentials = new NetworkCredential(myEmailAddress, myEmailSmtpPassword)
};
Email.DefaultSender = new SmtpSender(smtp);
var email = await Email.From(myEmailAddress, "wwmin").To(toEmailAddress, "wwmin").Subject("test send email").Body("测试发送邮件,时间:" + DateTime.Now).SendAsync();
email.Successful.Dump();