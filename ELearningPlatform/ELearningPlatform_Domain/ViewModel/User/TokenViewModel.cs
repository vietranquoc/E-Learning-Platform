using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearningPlatform_Domain.ViewModel.User
{
    public class TokenViewModel
    {
        #region Properties
        /// <summary>
        /// Code which to accessing into system
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// When should the token expired
        /// </summary>
        public double Expiration { get; set; }
        /// <summary>
        /// LifeTime of token
        /// </summary>
        public int LifeTime { get; set; }
        public AccountViewModel UserProfile { get; set; }
        #endregion

        #region Constructors
        public TokenViewModel()
        {

        }

        public TokenViewModel(string code, double expiration, int lifeTime, AccountViewModel userProfileViewModel)
        {
            Code = code;
            Expiration = expiration;
            LifeTime = lifeTime;
            UserProfile = userProfileViewModel;
        }
        #endregion
    }
}

