﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Shiny.Net.Http.Infrastructure;


namespace Shiny.Net.Http
{
    public class HttpTransferManager : AbstractHttpTransferManager
    {
        protected override async Task<IEnumerable<HttpTransfer>> GetDownloads(QueryFilter filter)
        {
            var downloads = await BackgroundDownloader
                .GetCurrentDownloadsAsync()
                .AsTask();

            return null;
        }


        protected override async Task<IEnumerable<HttpTransfer>> GetUploads(QueryFilter filter)
        {
            var downloads = await BackgroundUploader
                .GetCurrentUploadsAsync()
                .AsTask();

            return null;
        }


        protected override Task<HttpTransfer> CreateUpload(HttpTransferRequest request)
        {
            return base.CreateUpload(request);
        }


        protected override async Task<HttpTransfer> CreateDownload(HttpTransferRequest request)
        {
            var task = new BackgroundDownloader
            {
                Method = request.HttpMethod.Method,
                CostPolicy = request.UseMeteredConnection
                    ? BackgroundTransferCostPolicy.Default
                    : BackgroundTransferCostPolicy.UnrestrictedOnly
            };
            foreach (var header in request.Headers)
                task.SetRequestHeader(header.Key, header.Value);

            //var filePath = config.LocalFilePath ?? Path.Combine(ApplicationData.Current.LocalFolder.Path, Path.GetRandomFileName());
            var winFile = await StorageFile.GetFileFromPathAsync(request.LocalFile.FullName).AsTask();
            var op = task.CreateDownload(new Uri(request.Uri), winFile);
            //var operation = task.CreateDownload(new Uri(config.Uri), file);
            //var httpTask = new DownloadHttpTask(config, operation, false);
            //this.Add(httpTask);
            return default(HttpTransfer);
        }

        public override Task Cancel(string id)
        {
            //var tasks = await BackgroundDownloader.GetCurrentDownloadsAsync().AsTask();
            //foreach (var task in tasks)
            //    task.AttachAsync().Cancel();
            throw new NotImplementedException();
        }


        public override IObservable<HttpTransfer> WhenUpdated()
        {
            throw new NotImplementedException();
        }
    }
}