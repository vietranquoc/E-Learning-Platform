using ELearningPlatform_Entity.EntityModel;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearningPlatform_Data.Infracstructure.Interfaces
{
    public interface IUnitOfWork
    {
        #region Repositories
        IRepository<User> Users { get; }
        IRepository<UserDetail> UserDetails { get; }
        IRepository<Role> Roles { get; }
        IRepository<UserRole> UserRoles { get; }
        IRepository<Course> Courses { get; }
        IRepository<Enrollment> Enrollments { get; }
        IRepository<Lesson> Lessons { get; }
        IRepository<Quiz> Quizzes { get; }
        IRepository<Question> Questions { get; }
        IRepository<Answer> Answers { get; }
        IRepository<UserQuizResult> UserQuizResults { get; }
        IRepository<Notification> Notifications { get; }
        #endregion

        #region Methods
        int commit();
        Task<int> CommitAsync(CancellationToken cancellationToken = default(CancellationToken));
        IDbContextTransaction BeginTransactionScope();
        #endregion
    }
}
