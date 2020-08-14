﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Globalization;
using System.Linq;

namespace IRO_UNMO.App
{
    public static class MyExtensions
    {
        public static string DaNe(this bool source)
        {
            return source ? "DA" : "NE";
        }

        public static string Dostupnost(this bool source)
        {
            return source ? "Dostupan" : "Nedostupan";
        }

        public static string ToMyStrHMM(this DateTime source)
        {
            return source.ToString("HH':'mm");
        }

        public static string ToMyStrHMM(this DateTime? source, string valueForNull)
        {
            return source.HasValue ? source.Value.ToString("HH':'mm") : valueForNull;
        }

        public static string ToMyStrDMYY_HMM(this DateTime source)
        {
            return source.ToString("dd'.'MM'.'yy'.  ' HH':'mm");
        }

        public static string ToMyStrDMYYYY_HMM(this DateTime source)
        {
            return source.ToString("dd'.'MM'.'yyyy'. ' HH':'mm");
        }

        public static string ToMyStrDMYY_HMM(this DateTime? source, string valueForNull)
        {
            return source.HasValue ? source.Value.ToString("dd'.'MM'.'yy'. ' HH':'mm") : valueForNull;
        }


        public static string ToMyStrDMYYYY_HMM(this DateTime? source, string valueForNull)
        {
            return source.HasValue ? source.Value.ToString("dd'.'MM'.'yyyy'. ' HH':'mm") : valueForNull;
        }

        public static string ToMyStrDM(this DateTime source)
        {
            return source.ToString("dd'.'MM'.'");
        }

        public static string ToMyStrDM(this DateTime? source, string valueForNull)
        {
            return source.HasValue ? source.Value.ToString("dd'.'MM'.'") : valueForNull;
        }

        public static string ToMyStrDDD(this DateTime source)
        {
            return source.ToString("ddd", new CultureInfo("hr-HR"));
        }

        public static string ToMyStrDDDD(this DateTime? source, string valueForNull)
        {
            return source.HasValue ? source.Value.ToString("dddd", new CultureInfo("hr-HR")) : valueForNull;
        }

        public static string ToMyStrDDDD(this DateTime source)
        {
            return source.ToString("dddd", new CultureInfo("hr-HR"));
        }

        public static string ToMyStrDDD(this DateTime? source, string valueForNull)
        {
            return source.HasValue ? source.Value.ToString("ddd", new CultureInfo("hr-HR")) : valueForNull;
        }

        public static string ToMyStrDMYY(this DateTime source)
        {
            return source.ToString("dd'.'MM'.'yy'.'");
        }

        public static string ToMyStrDMYY(this DateTime? source, string valueForNull)
        {
            return source.HasValue ? source.Value.ToString("dd'.'MM'.'yy'.'") : valueForNull;
        }

        public static string ToMyStrDMYYYY(this DateTime source)
        {
            return source.ToString("dd'.'MM'.'yyyy'.'");
        }

        public static string ToMyStrDMYYYY(this DateTime? source, string valueForNull)
        {
            return source.HasValue ? source.Value.ToString("dd'.'MM'.'yyyy'.'") : valueForNull;
        }

        public static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("");
            return input.First().ToString().ToUpper() + input.ToLower().Substring(1);
        }
    }
}