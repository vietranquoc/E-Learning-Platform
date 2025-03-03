using ELearningPlatform_Data.Infracstructure.Interfaces;
using ELearningPlatform_Entity.EntityModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearningPlatform_Data.Infracstructure.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Fields
        private readonly DbContext _dbContext;
        
        public virtual IRepository<User> Users { get; set; }
        public virtual IRepository<UserDetail> UserDetails { get; set; }
        public virtual IRepository<Role> Roles { get; set; }
        public virtual IRepository<UserRole> UserRoles { get; set; }
        public virtual IRepository<Course> Courses { get; set; }
        public virtual IRepository<Enrollment> Enrollments { get; set; }
        public virtual IRepository<Lesson> Lessons { get; set; }
        public virtual IRepository<Quiz> Quizzes { get; set; }
        public virtual IRepository<Question> Questions { get; set; }
        public virtual IRepository<Answer> Answers { get; set; }
        public virtual IRepository<UserQuizResult> UserQuizResults { get; set; }
        public virtual IRepository<Notification> Notifications { get; set; }
        #endregion

        #region Constructor
        public UnitOfWork(DbContext dbContext,
            IRepository<User> user,
            IRepository<UserDetail> userDetail,
            IRepository<Role> role,
            IRepository<UserRole> userRole,
            IRepository<Course> course,
            IRepository<Enrollment> enrollment,
            IRepository<Lesson> lesson,
            IRepository<Quiz> quizz,
            IRepository<Question> question,
            IRepository<Answer> answer,
            IRepository<UserQuizResult> userQuizResult,
            IRepository<Notification> notification)
        {
            _dbContext = dbContext;

            Users = user;
            UserDetails = userDetail;
            Roles = role;
            UserRoles = userRole;
            Courses = course;
            Enrollments = enrollment;
            Lessons = lesson;
            Quizzes = quizz;
            Questions = question;
            Answers = answer;
            UserQuizResults = userQuizResult;
            Notifications = notification;
        }
        #endregion

        #region Methods
        public IDbContextTransaction BeginTransactionScope()
        {
            return _dbContext.Database.BeginTransaction();
        }

        public int commit()
        {
            return _dbContext.SaveChanges();
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
        #endregion
    }
}
