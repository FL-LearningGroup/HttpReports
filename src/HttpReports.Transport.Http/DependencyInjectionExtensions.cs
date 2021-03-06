﻿using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using HttpReports;
using HttpReports.Core;
using HttpReports.Transport.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions; 

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IHttpReportsBuilder UseHttpTransport(this IHttpReportsBuilder builder)
        { 
            builder.Services.AddOptions().Configure<HttpTransportOptions>(builder.Configuration.GetSection("Transport"));
            return builder.UseHttpTransportService();
        }

        public static IHttpReportsBuilder UseHttpTransport(this IHttpReportsBuilder builder, Action<HttpTransportOptions> options)
        {
            builder.Services.AddOptions().Configure(options);
            return builder.UseHttpTransportService();
        }

        private static IHttpReportsBuilder UseHttpTransportService(this IHttpReportsBuilder builder)
        {
            builder.Services.AddHttpClient(BasicConfig.HttpReportsHttpClient,client => {

                client.DefaultRequestHeaders.Clear(); 
                client.Timeout = TimeSpan.FromSeconds(10);

            });

            builder.Services.RemoveAll<IReportsTransport>();
            builder.Services.AddSingleton<IReportsTransport, HttpTransport>(); 
            return builder;
        }
    }
}
