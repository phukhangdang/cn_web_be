﻿using CN_WEB.Core.API;
using CN_WEB.Core.API.Authentication;
using CN_WEB.Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace CN_WEB.Core.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly ILogger<UnitOfWork> _logger;

        public UnitOfWork(SysDbWriteContext dbContextWrite, SysDbReadContext dbContextRead, IHttpContextAccessor accessor, ILogger<UnitOfWork> logger)
        {
            DataContextWrite = dbContextWrite;
            DataContextRead = dbContextRead;
            
            _accessor = accessor;
            _logger = logger;
        }

        /// <summary>
        /// Define a property of context read class
        /// </summary>
        public SysDbContext DataContextRead { get; }

        /// <summary>
        /// Define a property of context write class
        /// </summary>
        public SysDbContext DataContextWrite { get; }

        /// <summary>
        /// Current user
        /// </summary>
        public string GetCurrentUserId() {
            IEnumerable<Claim> httpClaim;
            httpClaim = _accessor.HttpContext.User.Claims;
            var currentUserId = string.Empty;
            if (httpClaim.Any())
            {
                currentUserId = httpClaim.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value.ToString();
            }
            return currentUserId;
            //object user = _accessor.HttpContext.User;
            //return new CurrentUser(user);
        }

        /// <summary>
        /// Begin a database transaction
        /// </summary>
        /// <returns>Transaction</returns>
        public IDbContextTransaction BeginTransaction()
        {
            return DataContextWrite.Database.BeginTransaction();
        }

        /// <summary>
        /// Find entity by key values
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public TEntity Find<TEntity>(params object[] keyValues) where TEntity : class
        {
            return DataContextRead.Find<TEntity>(keyValues);
        }

        /// <summary>
        /// Find async entity by key values
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public async Task<TEntity> FindAsync<TEntity>(params object[] keyValues) where TEntity : class
        {
            return await DataContextRead.FindAsync<TEntity>(keyValues);
        }

        /// <summary>
        /// Select entity
        /// </summary>
        /// <returns></returns>
        public DbSet<TEntity> Select<TEntity>() where TEntity : class
        {
            return DataContextRead.Set<TEntity>();
        }

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity"></param>
        public void Insert<TEntity>(TEntity entity, bool saveChange = true)
        {
            try
            {
                GenerateId(entity);
                GenerateBaseFieldInsert(entity);
                DataContextWrite.Add(entity);

                if (saveChange)
                {
                    DataContextWrite.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new BadRequestException(ex.Message);
            }
        }

        /// <summary>
        /// Bulk insert entities
        /// </summary>
        /// <param name="entities"></param>
        public void BulkInsert<TEntity>(IEnumerable<TEntity> entities)
        {
            try
            {
                foreach (var entity in entities)
                {
                    Insert(entity);
                }

                DataContextWrite.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new BadRequestException(ex.Message);
            }
        }

        /// <summary>
        /// Insert entity mapping from dto
        /// </summary>
        /// <param name="dto"></param>
        public void Insert<TEntity, TDto>(TDto dto, bool saveChange = true) where TEntity : class
        {
            try
            {
                var entity = (TEntity)Activator.CreateInstance(typeof(TEntity));
                GenerateId(dto);
                Map(entity, dto);
                GenerateBaseFieldInsert(entity);
                DataContextWrite.Set<TEntity>().Add(entity);

                if (saveChange)
                {
                    DataContextWrite.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new BadRequestException(ex.Message);
            }
        }

        /// <summary>
        /// Bulk insert entities from list dto
        /// </summary>
        /// <param name="listDto"></param>
        public void BulkInsert<TEntity, TDto>(IEnumerable<TDto> listDto) where TEntity : class
        {
            try
            {
                foreach (var dto in listDto)
                {
                    Insert<TEntity, TDto>(dto, false);
                }

                DataContextWrite.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new BadRequestException(ex.Message);
            }
        }

        /// <summary>
        /// Insert async entity
        /// </summary>
        /// <param name="entity"></param>
        public async Task InsertAsync<TEntity>(TEntity entity, bool saveChange = true)
        {
            try
            {
                GenerateId(entity);
                GenerateBaseFieldInsert(entity);
                await DataContextWrite.AddAsync(entity);

                if (saveChange)
                {
                    await DataContextWrite.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new BadRequestException(ex.Message);
            }
        }

        /// <summary>
        /// Bulk insert async entities
        /// </summary>
        /// <param name="entities"></param>
        public async Task BulkInsertAsync<TEntity>(IEnumerable<TEntity> entities)
        {
            try
            {
                foreach (var entity in entities)
                {
                    await InsertAsync(entity, false);
                }

                await DataContextWrite.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new BadRequestException(ex.Message);
            }
        }

        /// <summary>
        /// Insert async entity mapping from dto
        /// </summary>
        /// <param name="dto"></param>
        public async Task InsertAsync<TEntity, TDto>(TDto dto, bool saveChange = true) where TEntity : class
        {
            try
            {
                var entity = (TEntity)Activator.CreateInstance(typeof(TEntity));
                Map(entity, dto);
                GenerateId(entity);
                GenerateBaseFieldInsert(entity);
                await DataContextWrite.Set<TEntity>().AddAsync(entity);

                if (saveChange)
                {
                    await DataContextWrite.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new BadRequestException(ex.Message);
            }
        }

        /// <summary>
        /// Bulk insert async entities from list dto
        /// </summary>
        /// <param name="listDto"></param>
        public async Task BulkInsertAsync<TEntity, TDto>(IEnumerable<TDto> listDto) where TEntity : class
        {
            try
            {
                foreach (var dto in listDto)
                {
                    await InsertAsync<TEntity, TDto>(dto, false);
                }

                await DataContextWrite.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new BadRequestException(ex.Message);
            }
        }

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity"></param>
        public void Update<TEntity>(TEntity entity, bool saveChange = true) where TEntity : class
        {
            try
            {
                GenerateBaseFieldUpdate(entity);
                DataContextWrite.DetachLocal(entity);
                DataContextWrite.Entry(entity).State = EntityState.Modified;

                if (saveChange)
                {
                    DataContextWrite.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new BadRequestException(ex.Message);
            }
        }

        /// <summary>
        /// Bulk update entities
        /// </summary>
        /// <param name="entities"></param>
        public void BulkUpdate<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            try
            {
                foreach (var entity in entities)
                {
                    Update(entity, false);
                }

                DataContextWrite.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new BadRequestException(ex.Message);
            }
        }

        /// <summary>
        /// Update entity, specific fields
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="fields">Update fields</param>
        public void Update<TEntity>(TEntity entity, params string[] fields) where TEntity : class
        {
            try
            {
                GenerateBaseFieldUpdate(entity);
                DataContextWrite.DetachLocal(entity);

                foreach (var field in fields)
                {
                    try
                    {
                        if (entity.ContainsProperty(field)
                            && !DataContextWrite.Entry(entity).Property(field).Metadata.IsPrimaryKey())
                        {
                            DataContextWrite.Entry(entity).Property(field).IsModified = true;
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }

                if (entity.ContainsProperty("ModifiedAt"))
                {
                    DataContextWrite.Entry(entity).Property("ModifiedAt").IsModified = true;
                }

                if (entity.ContainsProperty("ModifiedBy"))
                {
                    DataContextWrite.Entry(entity).Property("ModifiedBy").IsModified = true;
                }

                DataContextWrite.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new BadRequestException(ex.Message);
            }
        }

        /// <summary>
        /// Update entity, specific field
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities">Entities</param>
        /// <param name="fields">Update fields</param>
        public void BulkUpdate<TEntity>(IEnumerable<TEntity> entities, params string[] fields) where TEntity : class
        {
            try
            {
                foreach (var entity in entities)
                {
                    GenerateBaseFieldUpdate(entity);
                    DataContextWrite.DetachLocal(entity);

                    foreach (var field in fields)
                    {
                        try
                        {
                            if (entity.ContainsProperty(field)
                                && !DataContextWrite.Entry(entity).Property(field).Metadata.IsPrimaryKey())
                            {
                                DataContextWrite.Entry(entity).Property(field).IsModified = true;
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    if (entity.ContainsProperty("ModifiedAt"))
                    {
                        DataContextWrite.Entry(entity).Property("ModifiedAt").IsModified = true;
                    }

                    if (entity.ContainsProperty("ModifiedBy"))
                    {
                        DataContextWrite.Entry(entity).Property("ModifiedBy").IsModified = true;
                    }
                }

                DataContextWrite.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new BadRequestException(ex.Message);
            }
        }

        /// <summary>
        /// Update entity mapping from dto
        /// </summary>
        /// <param name="entity"></param>
        public void Update<TEntity, TDto>(TDto dto, bool saveChange = true) where TEntity : class
        {
            try
            {
                var entity = (TEntity)Activator.CreateInstance(typeof(TEntity));
                Map(entity, dto);
                GenerateBaseFieldUpdate(entity);
                DataContextWrite.DetachLocal(entity);

                Type t = dto.GetType();
                PropertyInfo[] properties = t.GetProperties();
                foreach (var property in properties)
                {
                    try
                    {
                        if (entity.ContainsProperty(property.Name)
                            && !DataContextWrite.Entry(entity).Property(property.Name).Metadata.IsPrimaryKey())
                        {
                            DataContextWrite.Entry(entity).Property(property.Name).IsModified = true;
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }

                if (entity.ContainsProperty("ModifiedAt"))
                {
                    DataContextWrite.Entry(entity).Property("ModifiedAt").IsModified = true;
                }

                if (entity.ContainsProperty("ModifiedBy"))
                {
                    DataContextWrite.Entry(entity).Property("ModifiedBy").IsModified = true;
                }

                if (saveChange)
                {
                    DataContextWrite.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new BadRequestException(ex.Message);
            }

        }

        /// <summary>
        /// Bulk update entities mapping from dto
        /// </summary>
        /// <param name="listDto"></param>
        public void BulkUpdate<TEntity, TDto>(IEnumerable<TDto> listDto) where TEntity : class
        {
            try
            {
                foreach (var dto in listDto)
                {
                    Update<TEntity, TDto>(dto, false);
                }

                DataContextWrite.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new BadRequestException(ex.Message);
            }
        }

        /// <summary>
        /// Merge entity
        /// </summary>
        /// <param name="entity"></param>
        public TEntity Merge<TEntity>(TEntity entity, bool saveChange = true) where TEntity : class
        {
            try
            {
                if (DataContextRead.Set<TEntity>().AsEnumerable().Any(x => x.EqualsId(entity)))
                {
                    Update(entity, false);
                }
                else
                {
                    Insert(entity, false);
                }

                if (saveChange)
                {
                    DataContextWrite.SaveChanges();
                }

                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new BadRequestException(ex.Message);
            }
        }

        /// <summary>
        /// Merge entity mapping from dto
        /// </summary>
        /// <param name="entity"></param>
        public TDto Merge<TEntity, TDto>(TDto dto, bool saveChange = true) where TEntity : class
        {
            try
            {
                if (DataContextRead.Set<TEntity>().AsEnumerable().Any(x => x.EqualsId(dto)))
                {
                    Update<TEntity, TDto>(dto, false);
                }
                else
                {
                    GenerateId(dto);
                    Insert<TEntity, TDto>(dto, false);
                }

                if (saveChange)
                {
                    DataContextWrite.SaveChanges();
                }

                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new BadRequestException(ex.Message);
            }
        }

        /// <summary>
        /// Bulk merge entities mapping from dto
        /// </summary>
        /// <param name="entities"></param>
        public void BulkMerge<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            try
            {
                foreach (var entity in entities)
                {
                    if (DataContextRead.Set<TEntity>().AsEnumerable().Any(x => x.EqualsId(entity)))
                    {
                        Update(entity, false);
                    }
                    else
                    {
                        Insert(entity, false);
                    }
                }

                DataContextWrite.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new BadRequestException(ex.Message);
            }
        }

        /// <summary>
        /// Bulk merge entities mapping from dto
        /// </summary>
        /// <param name="entities"></param>
        public void BulkMerge<TEntity, TDto>(IEnumerable<TDto> listDto) where TDto : class where TEntity : class
        {
            try
            {
                foreach (var dto in listDto)
                {
                    if (DataContextRead.Set<TEntity>().AsEnumerable().Any(x => x.EqualsId(dto)))
                    {
                        Update<TEntity, TDto>(dto, false);
                    }
                    else
                    {
                        Insert<TEntity, TDto>(dto, false);
                    }
                }

                DataContextWrite.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new BadRequestException(ex.Message);
            }
        }

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="saveChange"></param>
        public void Delete<TEntity>(TEntity entity, bool saveChange = true) where TEntity : class
        {
            try
            {
                DataContextWrite.Entry(entity).State = EntityState.Deleted;

                if (saveChange)
                {
                    DataContextWrite.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new BadRequestException(ex.Message);
            }
        }

        /// <summary>
        /// Delete entity by ids
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="saveChange"></param>
        public void Delete<TEntity>(params string[] ids) where TEntity : class
        {
            try
            {
                foreach (var id in ids)
                {
                    var entity = (TEntity)Activator.CreateInstance(typeof(TEntity));
                    entity.GetType().GetProperty("Id").SetValue(entity, id);
                    DataContextWrite.Set<TEntity>().Attach(entity);
                    DataContextWrite.Set<TEntity>().Remove(entity);
                }

                DataContextWrite.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new BadRequestException(ex.Message);
            }
        }

        /// <summary>
        /// Bulk delete entities
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <param name="saveChange"></param>
        public void BulkDelete<TEntity>(IEnumerable<TEntity> entities, bool saveChange = true) where TEntity : class
        {
            try
            {
                foreach (var entity in entities)
                {
                    DataContextWrite.Entry(entity).State = EntityState.Deleted;
                }

                if (saveChange)
                {
                    DataContextWrite.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new BadRequestException(ex.Message);
            }
        }

        /// <summary>
        /// Database save changes
        /// </summary>
        public void Save()
        {
            DataContextWrite.SaveChanges();
        }

        /// <summary>
        /// Database save changes async
        /// </summary>
        public async Task SaveAsync()
        {
            await DataContextWrite.SaveChangesAsync();
        }

        /// <summary>
        /// Dispose database context
        /// </summary>
        public void Dispose()
        {
            DataContextWrite.Dispose();
        }

        /// <summary>
        /// Dispose async database context
        /// </summary>
        public async Task DisposeAsync()
        {
            await DataContextWrite.DisposeAsync();
        }

        private void Map<TDest, TSource>(TDest destObj, TSource sourceObj)
        {
            // Check null source object
            if (sourceObj == null)
            {
                return;
            }

            List<PropertyInfo> sourceProperties = sourceObj.GetType().GetProperties().ToList();
            List<PropertyInfo> destProperties = destObj.GetType().GetProperties().ToList();
            List<Type> baseType = new List<Type>()
            {
                typeof(Guid), typeof(Guid?), typeof(string), typeof(int), typeof(int?), typeof(long), typeof(long?), typeof(byte), typeof(byte?),
                typeof(short), typeof(short?), typeof(double), typeof(double?), typeof(decimal), typeof(decimal?), typeof(DateTime), typeof(DateTime?),
                typeof(Array), typeof(bool), typeof(bool?), typeof(object), typeof(float), typeof(float?)
            };

            // Set value for properties
            foreach (PropertyInfo destProperty in destProperties)
            {
                if (destProperty.PropertyType.IsPublic & baseType.Contains(destProperty.PropertyType))
                {
                    PropertyInfo source = sourceProperties.Where(d => d.Name.Equals(destProperty.Name)).SingleOrDefault();
                    if (source != null && destProperty.CanWrite)
                    {
                        destProperty.SetValue(destObj, source.GetValue(sourceObj));
                    }
                }
            }
        }

        private void GenerateBaseFieldInsert<TEntity>(params TEntity[] entities)
        {
            if (entities.Length == 0)
            {
                return;
            }


            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return;
            }

            foreach (var entity in entities)
            {
                if (entity.ContainsProperty("CreatedAt"))
                {
                    var createdAtProperty = entity.GetType().GetProperty("CreatedAt");
                    createdAtProperty.SetValue(entity, DateTime.Now);
                }

                if (entity.ContainsProperty("CreatedBy"))
                {
                    var createdByProperty = entity.GetType().GetProperty("CreatedBy");
                    createdByProperty.SetValue(entity, userId);
                }
            }
        }

        private void GenerateBaseFieldUpdate<TEntity>(params TEntity[] entities)
        {
            if (entities.Length == 0)
            {
                return;
            }

            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return;
            }

            foreach (var entity in entities)
            {
                if (entity.ContainsProperty("ModifiedAt"))
                {
                    var modifiedAtProperty = entity.GetType().GetProperty("ModifiedAt");
                    modifiedAtProperty.SetValue(entity, DateTime.Now);
                }

                if (entity.ContainsProperty("ModifiedBy"))
                {
                    var modifiedByProperty = entity.GetType().GetProperty("ModifiedBy");
                    modifiedByProperty.SetValue(entity, userId);
                }
            }
        }

        private void GenerateId<TEntity>(params TEntity[] entities)
        {
            foreach (var entity in entities)
            {
                if (entity.ContainsProperty("Id"))
                {
                    var idProperty = entity.GetType().GetProperty("Id");
                    var id = idProperty.GetValue(entity);
                    if (idProperty.PropertyType == typeof(string) && (id == null || string.IsNullOrEmpty(id.ToString())))
                    {
                        idProperty.SetValue(entity, Guid.NewGuid().ToString("N"));
                    }
                }

            }
        }
    }

    public static class UnitOfWorkExtension
    {
        public static void DetachLocal<TEntity>(this SysDbContext context, TEntity entity) where TEntity : class
        {
            string id = entity.GetType().GetProperty("Id").GetValue(entity).ToString();
            var local = context.Set<TEntity>().Local.FirstOrDefault(entry => entry.GetType().GetProperty("Id").GetValue(entry).ToString().Equals(id));
            if (local != null)
            {
                context.Entry(local).State = EntityState.Detached;
            }
        }

        public static bool ContainsProperty<TEntity>(this TEntity entity, string propertyName)
        {
            var propertyNames = entity.GetType().GetProperties().Select(x => x.Name);
            return propertyNames.Contains(propertyName);
        }

        public static bool EqualsId<TFirst, TSecond>(this TFirst one, TSecond two)
        {
            if (one.ContainsProperty("Id") && two.ContainsProperty("Id"))
            {
                return one.GetType().GetProperty("Id").GetValue(one)?.ToString() == two.GetType().GetProperty("Id").GetValue(two)?.ToString();
            }
            else
            {
                return false;
            }
        }
    }
}
