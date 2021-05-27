using CN_WEB.Core.Service;
using CN_WEB.Model.PostComment;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PostCommentEntity = CN_WEB.Core.Model.PostComment;

namespace CN_WEB.Service.SignalRHub
{
    public interface ISignalRHubService : IScoped
    {
        Task BroadcastComment(PostCommentDto comment);
    }

    public class SignalRHubService : Hub<ISignalRHubService>
    {
        public async Task BroadcastComment(PostCommentDto comment)
        {
            await Clients.All.BroadcastComment(comment);
        }
    }
}
