﻿using System;
using MediaBrowser.Model.ApiClient;

namespace MediaBrowser.WindowsPhone.Interfaces
{
    public interface IServerInfoService
    {
        bool HasServer { get; }
        ServerInfo ServerInfo { get; }
        void SetServerInfo(ServerInfo serverInfo);
        event EventHandler<ServerInfo> ServerInfoChanged;
    }
}
