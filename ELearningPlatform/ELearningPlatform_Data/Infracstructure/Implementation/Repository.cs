using ELearningPlatform_Data.Core;
using ELearningPlatform_Data.Infracstructure.Interfaces;
using ELearningPlatform_Entity.EntityBase;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ELearningPlatform_Data.Infracstructure.Implementation
{
    public class Repository<T> : IRepository<T> where T : class, IEntityBase
    {
        #region Private Fields
        private readonly DbSet<T> _dbSet;
        private readonly DbContext _dbContext;
        private readonly IHttpContextAccessor _context;
        #endregion

        #region Constructor
        public Repository(DbContext dbContext, IHttpContextAccessor context)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
            _context = context;
        }
        #endregion

        #region Methods 

        protected Guid CurrentUserId
        {
            get
            {
                ClaimsIdentity? identity = _context.HttpContext.User.Identity as ClaimsIdentity;
                var userClaim = identity?.Claims.SingleOrDefault(x => x.Type.Equals("Id"));
                if (userClaim != null)
                {
                    return Guid.Parse(userClaim.Value);
                }

                return Guid.Empty;

            }

        }

        /// <summary>
        /// Insert a record into data table.
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Insert(T entity)
        {
            if (entity.Id == Guid.Empty) entity.Id = Guid.NewGuid();
            entity.CreatedBy = this.CurrentUserId;
            entity.CreatedDate = DateTime.Now;
            entity.IsActive = true;

            // Thêm thực thể vào DbSet để chờ lưu vào CSDL
            _dbSet.Add(entity);
        }

        /// <summary>
        /// Remove a list of entities from database.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual void Remove(IEnumerable<T> entities)
        {
            // Xóa danh sách các thực thể khỏi DbSet
            _dbSet.RemoveRange(entities);
        }

        /// <summary>
        /// Remove an entity from database.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual void Remove(T entity)
        {
            // Xóa thực thể khỏi DbSet
            _dbSet.Remove(entity);
        }

        /// <summary>
        /// Insert a list entity
        /// </summary>
        /// <param name="lstEntity"></param>
        public virtual void Insert(IEnumerable<T> lstEntity)
        {
            foreach (var item in lstEntity)
            {
                if (item.Id == Guid.Empty) item.Id = Guid.NewGuid();
                item.CreatedBy = this.CurrentUserId;
                item.CreatedDate = DateTime.Now;
                item.IsActive = true;

                _dbSet.Add(item);
            }
        }

        /// <summary>
        /// Update an entity
        /// </summary>
        /// <param name="entityToUpdate"></param>
        public virtual void Update(IEnumerable<T> entityToUpdate)
        {
            foreach (var item in entityToUpdate)
            {
                _dbSet.Attach(item);
                _dbContext.Entry(item).State = EntityState.Modified;

                item.UpdatedBy = this.CurrentUserId;
                item.UpdatedDate = DateTime.Now;
            }
        }

        public virtual void Update(T entityToUpdate)
        {
            // Đưa thực thể vào trạng thái Attach để theo dõi sự thay đổi
            _dbSet.Attach(entityToUpdate);
            // Đánh dấu thực thể là đã sửa đổi
            /*
                Mặc dù bạn đã đính kèm thực thể vào DbContext, 
                Entity Framework vẫn chưa biết rằng bạn muốn cập nhật thực thể này. 
                Do đó, bạn cần thay đổi trạng thái của thực thể thành Modified 
                để EF hiểu rằng dữ liệu của thực thể đã thay đổi và cần được cập nhật trong cơ sở dữ liệu.
             */
            _dbContext.Entry(entityToUpdate).State = EntityState.Modified;

            entityToUpdate.UpdatedBy = this.CurrentUserId;
            entityToUpdate.UpdatedDate = DateTime.Now;
        }

        /// <summary>
        /// Check exist entity satisfy condition
        /// </summary>
        /// <param name="predicate">Condition to check entity</param>
        /// <param name="checkError">Optional reference object to store error details if an exceptions occurs</param>
        /// <returns></returns>
        public virtual async Task<bool> CheckExist(Expression<Func<T, bool>> predicate, Ref<CheckError> checkError = null)
        {
            try
            {
                // Check if any entity in the DbSet satisfies the predicate condition 
                return await _dbContext.Set<T>().AnyAsync(predicate);
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError()
                    {
                        IsError = true,
                        Exception = ex,
                        Message = ex.Message
                    };
                }

                return false;

            }

        }

        /// <summary>
        /// Get number of entity satisfy condition
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="checkError"></param>
        /// <returns></returns>
        public virtual async Task<int> GetCount(Expression<Func<T, bool>> predicate, Ref<CheckError> checkError = null)
        {
            try
            {
                return await _dbContext.Set<T>().Where(predicate).CountAsync();
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError()
                    {
                        IsError = true,
                        Exception = ex,
                        Message = ex.Message
                    };
                }

                return -1;
            }
        }

        /// <summary>
        /// Get an entity by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="checkError"></param>
        /// <returns></returns>
        public virtual async Task<T> GetById(object id, Ref<CheckError> checkError = null)
        {
            try
            {
                if (id == null) return null;

                return await _dbContext.Set<T>().FindAsync(id);
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError()
                    {
                        IsError = true,
                        Exception = ex,
                        Message = ex.Message
                    };
                }

                throw ex;
            }
        }

        /// <summary>
        /// Get one entity satisfy condition
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="checkError"></param>
        /// <returns></returns>
        public virtual async Task<T> GetOne(Expression<Func<T, bool>> predicate, Ref<CheckError> checkError = null)
        {
            try
            {
                return await _dbContext.Set<T>().Where(predicate).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError()
                    {
                        IsError = true,
                        Exception = ex,
                        Message = ex.Message
                    };
                }

                return null;
            }
        }

        /// <summary>
        /// Get list of entity by store procedure
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="checkError"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<T>> Get(string storedProcedureName, SqlParameter[] parameters = null, Ref<CheckError> checkError = null)
        {
            try
            {
                if (parameters != null)
                {
                    // Tạo chuỗi truy vấn bắt đầu với "Exec <storedProcedureName>"
                    var query = string.Concat("Exec ", storedProcedureName, " ");
                    // Duyệt qua từng tham số trong mảng parameters.
                    foreach (var item in parameters)
                    {
                        // Nếu tham số không phải là OUTPUT, thêm tên tham số vào chuỗi truy vấn.
                        if (item.Direction != ParameterDirection.Output)
                        {
                            query += string.Concat(item.ParameterName, ",");
                        }
                        // Nếu tham số là OUTPUT, thêm tên tham số cùng với từ khóa OUTPUT vào chuỗi truy vấn.
                        else
                        {
                            query += string.Concat(item.ParameterName + " OUTPUT", ",");
                        }
                    }
                    // Xóa dấu phẩy cuối cùng sau khi kết thúc việc xây dựng chuỗi truy vấn.
                    query = parameters.Length > 0 ? query.Substring(0, query.Length - 1) : storedProcedureName;

                    // Thực hiện câu truy vấn SQL bằng FromSqlRaw với chuỗi truy vấn đã tạo và các tham số. 
                    return await _dbSet.FromSqlRaw(query, parameters).ToListAsync();
                }
                else
                {
                    // Nếu không có tham số, chỉ thực hiện stored procedure mà không có tham số nào.
                    return await _dbSet.FromSqlRaw(storedProcedureName).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError()
                    {
                        IsError = true,
                        Exception = ex,
                        Message = ex.Message
                    };
                }

                return null;
            }
        }

        /// <summary>
        /// Get one entity by store procedure 
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="checkError"></param>
        /// <returns></returns>
        public virtual async Task<T> GetOne(string storedProcedureName, SqlParameter[] parameters = null, Ref<CheckError> checkError = null)
        {
            try
            {
                if (parameters != null)
                {
                    var query = string.Concat("Exec ", storedProcedureName, " ");
                    foreach (var item in parameters)
                    {
                        var itemObject = (SqlParameter)item;

                        query += string.Concat(itemObject.ParameterName, ",");
                    }

                    query = parameters.Length > 0 ? query.Substring(0, query.Length - 1) : storedProcedureName;

                    return await _dbSet.FromSqlRaw(query, parameters).FirstOrDefaultAsync();
                }
                else
                {
                    return await _dbSet.FromSqlRaw(storedProcedureName).FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError()
                    {
                        IsError = true,
                        Exception = ex,
                        Message = ex.Message
                    };
                }

                return null;
            }
        }

        /// <summary>
        /// Get output of store procedure
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="checkError"></param>
        /// <returns></returns>
        public virtual IEnumerable<SqlParameter> GetOutPut(string storedProcedureName, SqlParameter[] parameters, Ref<CheckError> checkError = null)
        {
            try
            {
                if (parameters != null)
                {
                    var query = string.Concat("", storedProcedureName, " ");
                    var listParameterOutPut = new List<SqlParameter>();

                    foreach (var item in parameters)
                    {
                        var itemObject = (SqlParameter)item;

                        if (itemObject.Direction == ParameterDirection.Output)
                        {
                            listParameterOutPut.Add(itemObject);

                            query += string.Concat(itemObject.ParameterName, " OUT,");
                        }
                        else
                        {
                            query += string.Concat(itemObject.ParameterName, ",");
                        }
                    }

                    query = parameters.Length > 0 ? query.Substring(0, query.Length - 1) : storedProcedureName;

                    _dbContext.Database.ExecuteSqlRaw(query, parameters);

                    return listParameterOutPut;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError() { IsError = true, Exception = ex, Message = ex.Message };
                }

                return null;
            }
        }

        public IQueryable<T> FindAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> items = _dbContext.Set<T>();

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    items = items.Include(includeProperty);
                }
            }

            return items.Where(predicate);
        }

        public async Task<bool> Delete(T entity, Ref<CheckError> checkError = null)
        {
            try
            {
                _dbContext.Set<T>().Remove(entity);

                return await _dbContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError()
                    {
                        IsError = true,
                        Exception = ex,
                        Message = ex.Message
                    };
                }

                return false;
            }
        }

        /// <summary>
        /// Delete model in DataBase
        /// </summary>
        /// <param name="id">Key in entity</param>
        /// <param name="checkError">Check Error</param>
        /// <returns>Boolean</returns>
        public async Task<bool> Delete(object id, Ref<CheckError> checkError = null)
        {
            try
            {
                T entity = await GetById(id);

                if (entity != null)
                {
                    _dbContext.Set<T>().Remove(entity);

                    return await _dbContext.SaveChangesAsync() > 0;
                }

                return true;
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError()
                    {
                        IsError = true,
                        Exception = ex,
                        Message = ex.Message
                    };
                }

                return false;
            }
        }

        /// <summary>
        /// Delete multiple records
        /// </summary>
        /// <param name="list">list entity</param>
        /// <param name="checkError">Check Error</param>
        /// <returns>Boolean</returns>
        public async Task<bool> DeleteAll(IList<T> list, Ref<CheckError> checkError = null)
        {
            try
            {
                _dbContext.Set<T>().RemoveRange(list);

                return await _dbContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError()
                    {
                        IsError = true,
                        Exception = ex,
                        Message = ex.Message
                    };
                }

                return false;
            }
        }

        public virtual IQueryable<T> GetQueryable()
        {
            return _dbContext.Set<T>();
        }

        public virtual IQueryable<T> GetQueryable(Expression<Func<T, bool>> condition)
        {
            return _dbSet.Where(condition);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        #endregion
    }
}
