using System.Collections.Generic;
using System.Linq;
using Folke.Orm;
using Microsoft.AspNet.Identity;
using System;
using System.Threading.Tasks;

namespace Elm.AspNet.Identity
{
    public class UserStore : UserStore<IdentityUser<string>>
    {
        public UserStore(FolkeConnection connection, IdentityErrorDescriber describer = null) : base(connection, describer) { }
        {
        }
    }

    public class UserStore<T, TKey> : IUserTwoFactorStore<T, TKey>, IUserLockoutStore<T, TKey>, IUserEmailStore<T, TKey>, IUserPasswordStore<T, TKey>, IUserPhoneNumberStore<T, TKey>, IUserLoginStore<T, TKey>, IUserSecurityStampStore<T, TKey> where T : IdentityUser<TKey>, new()
    {
        private readonly IFolkeConnection connection;
        private bool disposed;

        public UserStore(IFolkeConnection connection)
        {
            this.connection = connection;
        }

        private void ThrowIfDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        public async Task CreateAsync(T user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            
            using (var transaction = connection.BeginTransaction())
            {
                await connection.SaveAsync(user);
                transaction.Commit();
            }
        }

        public async Task DeleteAsync(T user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            await connection.DeleteAsync(user);
        }

        public async Task<T> FindByIdAsync(TKey userId)
        {
            ThrowIfDisposed();
            if (userId.Equals(default(T)))
            {
                throw new ArgumentNullException("userId");
            }
            return await connection.LoadAsync<T>(userId);
        }

        public async Task<T> FindByNameAsync(string userName)
        {
            ThrowIfDisposed();
            if (userName == null)
            {
                throw new ArgumentNullException("userName");
            }            
            return await connection.QueryOver<T>()
                                .Where(x => x.UserName == userName)
                                .SingleOrDefaultAsync();         
        }

        public async Task UpdateAsync(T user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            await connection.UpdateAsync(user);            
        }

        public void Dispose()
        {
            connection.Dispose();
            disposed = true;
        }

        public async Task<string> GetPasswordHashAsync(T user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return await Task.FromResult(user.PasswordHash);
        }

        public async Task<bool> HasPasswordAsync(T user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return await Task.FromResult(user.PasswordHash != null);
        }

        public async Task SetPasswordHashAsync(T user, string passwordHash)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (passwordHash == null)
            {
                throw new ArgumentNullException("passwordHash");
            }
            user.PasswordHash = passwordHash;
            await Task.Delay(0);
        }

        public async Task<T> FindByEmailAsync(string email)
        {
            ThrowIfDisposed();
            if (email == null)
            {
                throw new ArgumentNullException("email");
            }
            
            return await connection.QueryOver<T>()
                                .Where(x => x.Email == email)
                                .SingleOrDefaultAsync();            
        }

        public Task<string> GetEmailAsync(T user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(T user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailAsync(T user, string email)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.Email = email;
            return Task.FromResult(0);
        }

        public Task SetEmailConfirmedAsync(T user, bool confirmed)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task<int> GetAccessFailedCountAsync(T user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(T user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult(user.LockoutEnabled);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(T user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            DateTimeOffset dateTimeOffset;
            if (user.LockoutEndDateUtc.HasValue)
            {
                DateTime? lockoutEndDateUtc = user.LockoutEndDateUtc;
                dateTimeOffset = new DateTimeOffset(DateTime.SpecifyKind(lockoutEndDateUtc.Value, DateTimeKind.Utc));
            }
            else
            {
                dateTimeOffset = new DateTimeOffset();
            }
            return Task.FromResult(dateTimeOffset);
        }

        public Task<int> IncrementAccessFailedCountAsync(T user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            
            user.AccessFailedCount = user.AccessFailedCount + 1;
            return Task.FromResult(0);
        }

        public Task ResetAccessFailedCountAsync(T user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.AccessFailedCount = 0;
            return Task.FromResult(0);
        }

        public Task SetLockoutEnabledAsync(T user, bool enabled)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.LockoutEnabled = enabled;
            return Task.FromResult(0);
        }

        public Task SetLockoutEndDateAsync(T user, DateTimeOffset lockoutEnd)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            DateTime? nullable = lockoutEnd == DateTimeOffset.MinValue ? null : new DateTime?(lockoutEnd.UtcDateTime);

            user.LockoutEndDateUtc = nullable;
            return Task.FromResult(0);
        }
        
        public Task<bool> GetTwoFactorEnabledAsync(T user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task SetTwoFactorEnabledAsync(T user, bool enabled)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException("user");
            
            user.TwoFactorEnabled = enabled;
            return Task.FromResult(0);
        }

        public Task SetPhoneNumberAsync(T user, string phoneNumber)
        {
            user.PhoneNumber = phoneNumber;
            return Task.FromResult(0);
        }

        public Task<string> GetPhoneNumberAsync(T user)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(T user)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(T user, bool confirmed)
        {
            user.PhoneNumberConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public async Task AddLoginAsync(T user, UserLoginInfo login)
        {
            var identityUserLogin = new IdentityUserLogin<T, TKey>
            {
                LoginProvider = login.LoginProvider,
                ProviderKey = login.ProviderKey,
                User = user
            };
            await connection.SaveAsync(identityUserLogin);
        }

        public async Task RemoveLoginAsync(T user, UserLoginInfo login)
        {
            var identityUserLogin =
                await
                    connection.QueryOver<IdentityUserLogin<T, TKey>>()
                        .Where(x => x.User == user && x.LoginProvider == login.LoginProvider)
                        .SingleAsync();
            await connection.DeleteAsync(identityUserLogin);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(T user)
        {
            var results =
                await
                    connection.QueryOver<IdentityUserLogin<T, TKey>>()
                        .Where(x => x.User == user)
                        .ListAsync();
            return results.Select(x => new UserLoginInfo(x.LoginProvider, x.ProviderKey)).ToList();
        }

        public async Task<T> FindAsync(UserLoginInfo login)
        {
            var identityUserLogin =
                await
                    connection.QueryOver<IdentityUserLogin<T, TKey>>(x => x.User)
                        .Where(x => x.LoginProvider == login.LoginProvider && x.ProviderKey == login.ProviderKey)
                        .SingleAsync();
            return identityUserLogin.User;
        }

        public Task SetSecurityStampAsync(T user, string stamp)
        {
            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }

        public Task<string> GetSecurityStampAsync(T user)
        {
            return Task.FromResult(user.SecurityStamp);
        }
    }
}
