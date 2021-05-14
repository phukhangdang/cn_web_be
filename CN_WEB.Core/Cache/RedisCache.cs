using CN_WEB.Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using IDatabase = StackExchange.Redis.IDatabase;

namespace CN_WEB.Core.Cache
{
    public interface IRedisCache : IScoped
    {
        void SetCacheAction(ActionExecutedContext actionContext, RedisCacheAttribute cacheAttr);
        void GetCacheAction(ActionExecutingContext actionContext);
    }

    public class RedisCache : IRedisCache
    {
        private readonly IDatabase _database;
        private readonly ILogger<RedisCache> _logger;

        public RedisCache(IDatabase database, ILogger<RedisCache> logger)
        {
            _database = database;
            _logger = logger;
        }

        public void SetCacheAction(ActionExecutedContext actionContext, RedisCacheAttribute cacheAttr)
        {
            // Set key value
            string key = $"{actionContext.HttpContext.Request.Path.ToUriComponent()}{actionContext.HttpContext.Request.QueryString.ToUriComponent()}";

            // Check key exist
            if (!_database.KeyExists(key))
            {
                dynamic result = actionContext.Result;
                var setting = new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    }
                };

                // Set time span
                var timeSpan = TimeSpan.FromSeconds(3600);
                if (cacheAttr.Duration != 0)
                {
                    switch (cacheAttr.Measure)
                    {
                        case TimeMeasure.Second:
                            timeSpan = TimeSpan.FromSeconds(cacheAttr.Duration);
                            break;
                        case TimeMeasure.Minute:
                            timeSpan = TimeSpan.FromMinutes(cacheAttr.Duration);
                            break;
                        case TimeMeasure.Hour:
                            timeSpan = TimeSpan.FromHours(cacheAttr.Duration);
                            break;
                        case TimeMeasure.Day:
                            timeSpan = TimeSpan.FromDays(cacheAttr.Duration);
                            break;
                    }
                }

                // Set cache
                _database.StringSet(key, JsonConvert.SerializeObject(result.Value, setting), timeSpan);
            }
        }

        public void GetCacheAction(ActionExecutingContext actionContext)
        {
            try
            {
                // Set key value
                string key = $"{actionContext.HttpContext.Request.Path.ToUriComponent()}{actionContext.HttpContext.Request.QueryString.ToUriComponent()}";

                // Check key exist
                if (_database.KeyExists(key))
                {
                    var result = JsonConvert.DeserializeObject(_database.StringGet(key));
                    actionContext.Result = new ObjectResult(result);
                }
            } catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }            
        }
    }
}
