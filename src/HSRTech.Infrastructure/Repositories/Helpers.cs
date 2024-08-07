using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Dynamic;
using System.Reflection;
using static Dapper.SqlMapper;
using static HSRTech.Infrastructure.Repositories.Helpers;

namespace HSRTech.Infrastructure.Repositories
{
    public class Helpers
    {
        public static class StrSql
        {
            public static string DynamicKey = "{artifact}Id,{artifact}Codigo,{artifact}Code,{artifact}Identifier";
            private static string GetTableName<T>(object? instance = null)
            {
                var type = instance is null ? typeof(T) : instance.GetType();
                var attrTableDa = type.GetCustomAttributes(true).SingleOrDefault(x => x is TableAttribute) as TableAttribute;
                if (attrTableDa is not null) return attrTableDa.Name;

                return type.Name;
            }

            public static string GetKey<T>(object? instance = null)
            {
                var type = instance is null ? typeof(T) : instance.GetType();
                var tableName = GetTableName<T>(instance);
                var property = type.GetProperties().SingleOrDefault(x => x.GetCustomAttributes(true).Any(x => x is KeyAttribute));
                if (property is not null) return property.Name;

                property = type.GetProperties().SingleOrDefault(x => DynamicKey.Contains(x.Name, StringComparison.OrdinalIgnoreCase) || DynamicKey.Contains(DynamicKey.Replace("{artifact}", tableName), StringComparison.OrdinalIgnoreCase));
                if (property is not null) return property.Name;

                return "";
            }

            public static Type GetKeyType<T>(object? instance = null)
            {
                var type = instance is null ? typeof(T) : instance.GetType();
                var property = type.GetProperties().SingleOrDefault(x => x.GetCustomAttributes(true).Any(x => x is KeyAttribute));
                if (property is not null) return property.PropertyType;
                return default;
            }

            public static object GetKeyObjectFilter<T>(long key, object? instance = null)
            {
                var keyName = GetKey<T>(instance);
                dynamic dicParans = new ExpandoObject();
                ((IDictionary<string, object>)dicParans).Add(keyName, key);
                return dicParans;
            }

            private static string GetColumnName(PropertyInfo prop)
            {
                var attr = prop.GetCustomAttributes(true).FirstOrDefault(z => z is ColumnAttribute);

                if (attr is ColumnAttribute)
                {
                    var attrColumn = attr as ColumnAttribute;
                    if (attrColumn.Name.Equals(DapperCRUD.IncludeKey, StringComparison.OrdinalIgnoreCase)) return prop.Name;
                    return attrColumn.Name;
                }
                return prop.Name;
            }

            private static List<string> GetPropertiesColumn<T>(object? instance = null)
            {
                var key = GetKey<T>(instance);
                var type = instance is null ? typeof(T) : instance.GetType();
                var hasIncludeKey = type.GetProperties().SingleOrDefault(x => x.GetCustomAttributes(true).Any(x => x is KeyAttribute) && x.GetCustomAttributes(true).Any(x => x is ColumnAttribute && (x as ColumnAttribute).Name.Equals(DapperCRUD.IncludeKey, StringComparison.OrdinalIgnoreCase)));
              
                if(hasIncludeKey is not null)
                    return type
                        .GetProperties()
                        .OrderBy(p => p.Name.ToLower())
                        .Where(x => !x.GetCustomAttributes(true).Any(z => z is NotMappedAttribute))
                        .Select(GetColumnName)
                        .ToList();

                return type
                    .GetProperties()
                    .OrderBy(p => p.Name.ToLower())
                    .Where(x => !x.Name.Equals(key, StringComparison.OrdinalIgnoreCase) && !x.GetCustomAttributes(true).Any(z => z is NotMappedAttribute))
                    .Select(GetColumnName)
                    .ToList();
            }

            public static string CreateSqlSelect<T>(string? sqlFilter = null, object? instance = null)
            {
                var tableName = GetTableName<T>(instance);
                var key = GetKey<T>(instance);

                var internalFilter = "";

                if (string.IsNullOrEmpty(sqlFilter)) internalFilter = $" where {key} = @{key} ";

                if (!string.IsNullOrEmpty(sqlFilter) && !sqlFilter.Contains("where", StringComparison.OrdinalIgnoreCase)) internalFilter = " where " + sqlFilter;

                return $@"
                    select *
                    from {tableName}
                    {internalFilter}
                ";
            }

            public static string CreateSqlInsert<T>(object? instance = null)
            {
                var tableName = GetTableName<T>(instance);
                var propertiesEntity = GetPropertiesColumn<T>(instance);
                var key = GetKey<T>(instance);
                var sqlInsert = $@"
                    insert into {tableName} ({string.Join(", ", propertiesEntity)})
                    values ({string.Join(", ", propertiesEntity.Select(x => $"@{x.Replace("\"", "")}"))}); select scope_identity()
                ";
                return sqlInsert;
            }

            public static string CreateSqlUpdate<T>(string? sqlFilter = null, object? instance = null)
            {
                var tableName = GetTableName<T>(instance);
                var propertiesEntity = GetPropertiesColumn<T>(instance);
                var key = GetKey<T>(instance);
                var internalFilter = "";

                if (string.IsNullOrEmpty(sqlFilter)) internalFilter = $" where {key} = @{key} ";

                if (!string.IsNullOrEmpty(sqlFilter) && !sqlFilter.Contains("where", StringComparison.OrdinalIgnoreCase)) internalFilter = " where " + sqlFilter;

                var sqlUpdate = $@"
                    update {tableName}
                    set {string.Join(", ", propertiesEntity.Select(x => $"{x} = @{x.Replace("\"", "")}"))}
                    {internalFilter}
                ";
                return sqlUpdate;
            }

            public static string CreateSqlDelete<T>(string? sqlFilter = null, object? instance = null)
            {
                var tableName = GetTableName<T>(instance);
                var key = GetKey<T>(instance);
                var internalFilter = "";

                if (string.IsNullOrEmpty(sqlFilter)) internalFilter = $" where {key} = @{key} ";

                if (!string.IsNullOrEmpty(sqlFilter) && !sqlFilter.Contains("where", StringComparison.OrdinalIgnoreCase)) internalFilter = " where " + sqlFilter;

                return $@"
                    delete from {tableName} 
                    {internalFilter}
                ";
            }
        }
    }

    public static class DapperCRUD
    {
        /// <summary>
        /// Marcação para incluir a chave nos comando de insert e update, para os casos de chaves compostas.
        /// </summary>
        public const string IncludeKey = "IncludeKey";
        public static async Task Insert<TEntity>(this IDbTransaction dbTransaction, TEntity entity)
        {
            var keyName = StrSql.GetKey<TEntity>(entity);
            var method = entity.GetType().GetProperty(keyName);
            var sqlInsert = StrSql.CreateSqlInsert<TEntity>(entity);
            var typeKey = StrSql.GetKeyType<TEntity>(entity);
            
            if (typeKey is null)
            {
                await dbTransaction.Connection.ExecuteAsync(sqlInsert, entity, transaction: dbTransaction);
                return;
            }

            var keyValueResult = await dbTransaction.Connection.ExecuteScalarAsync(sqlInsert, entity, transaction: dbTransaction);
            if (keyValueResult is null) return;
            var keyValueConverted = Convert.ChangeType(keyValueResult, typeKey);
            method.SetValue(entity, keyValueConverted);
        }

        public static async Task<TEntity> GetByKey<TEntity>(this IDbTransaction dbTransaction, long key) 
        {
            var filterKey = StrSql.GetKeyObjectFilter<TEntity>(key);
            var sqlSelect = StrSql.CreateSqlSelect<TEntity>();
            return await dbTransaction.Connection.QuerySingleOrDefaultAsync<TEntity?>(sqlSelect, filterKey, transaction: dbTransaction);
        }

        public static async Task<bool> Delete<TEntity>(this IDbTransaction dbTransaction, TEntity entity)
        {
            var sqlDelete = StrSql.CreateSqlDelete<TEntity>(instance: entity);
            return (await dbTransaction.Connection.ExecuteAsync(sqlDelete, entity, transaction: dbTransaction)) > 0;
        }

        public static async Task<bool> Update<TEntity>(this IDbTransaction dbTransaction, TEntity entity)
        {
            var sqlUpdate = StrSql.CreateSqlUpdate<TEntity>(instance: entity);
            return (await dbTransaction.Connection.ExecuteAsync(sqlUpdate, entity, transaction: dbTransaction)) > 0;
        }
                
        public static async Task<List<TEntity>> GetAll<TEntity>(this IDbTransaction dbTransaction, object filter)
        {
            var sqlSelect = StrSql.CreateSqlSelect<TEntity>();
            return (await dbTransaction.Connection.QueryAsync<TEntity>(sqlSelect, filter, transaction: dbTransaction)).ToList();
        }        
    }
}
