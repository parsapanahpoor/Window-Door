﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ganss.XSS;

namespace Window.Application.Security
{
    public static class XssSecurity
    {
        public static string SanitizeText(this string text)
        {
            var htmlSanitizer = new HtmlSanitizer
            {
                KeepChildNodes = true,
                AllowDataAttributes = true
            };

            return htmlSanitizer.Sanitize(text);
        }
    }
}
