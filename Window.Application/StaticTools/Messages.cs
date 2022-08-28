﻿using Window.Application.StticTools;

namespace Window.Application.StaticTools;

public static class Messages
{
    //This is for sample
    public static string GetMessageForSetConsultationDate(string date, string time, string phone)
    {
        return
            $"باسلام  {Environment.NewLine}در خواست مشاوره شما برای روز {date + " - ساعت  " + time}.{Environment.NewLine} تنظیم شد. لطفا در تاریخ و زمان مقرر با شماره {phone} تماس بگیرید.{Environment.NewLine} {FilePaths.SiteFarsiName}";
    }

    public static string SendSMSForLogin()
    {
        return
            $"با سلام ،ورود به حساب با موفقیت انجام شده است .{Environment.NewLine} {FilePaths.SiteFarsiName}";
    }

    public static string SendActivationRegisterSms(string activationCode)
    {
        return
            $"باعرض سلام . {Environment.NewLine} کد فعال سازی شما : {activationCode} . {Environment.NewLine} {FilePaths.SiteFarsiName}";
    }
}