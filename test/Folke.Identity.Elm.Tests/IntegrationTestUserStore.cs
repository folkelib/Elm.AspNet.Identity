using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Folke.Elm;
using Folke.Elm.Mapping;
using Folke.Elm.MicrosoftSqlServer;
using Folke.Elm.Sqlite;
using Microsoft.Extensions.Options;
using Xunit;

namespace Folke.Identity.Elm.Tests
{
    public class IntegrationTestUserStore
    {
        private readonly FolkeConnection connection;

        public IntegrationTestUserStore()
        {
            //var driver = new MicrosoftSqlServerDriver();
            var driver = new SqliteDriver();
            ElmOptions options = new ElmOptions
            {
                ConnectionString = "Data Source=:memory:"
                //ConnectionString = "Server=SIDOTRON2\\SQLEXPRESS;Database=test;Integrated Security=True"
            };
            this.connection = new FolkeConnection(driver, new Mapper(), new OptionsWrapper<ElmOptions>(options));
        }

        [Fact]
        public async void CreateAsync()
        {
            using (connection.BeginTransaction())
            {
                // Arrange
                var userStore = CreateUserStore();
                var user = new TestUser { UserName = "UserName" };

                // Act
                var result = await userStore.CreateAsync(user);

                // Assert
                Assert.True(result.Succeeded);
            }
        }

        [Fact]
        public async void GetUsersInRoleAsync()
        {
            using (connection.BeginTransaction())
            {
                // Arrange
                connection.UpdateIdentityRoleSchema<int, TestUser, TestRole>();
                var userStore = CreateUserStore();
                var user = new TestUser { UserName =  "UserName" };
                await userStore.CreateAsync(user);
                var roleStore = new RoleStore<TestRole, int>(connection);
                var role = new TestRole { Name = "Role" };
                await roleStore.CreateAsync(role);
                await userStore.AddToRoleAsync(user, role.Name);
                
                // Act
                IList<TestUser> result = await userStore.GetUsersInRoleAsync(role.Name);

                // Assert
                Assert.Equal(user, result[0]);
            }
        }

        private UserStore<TestUser, int> CreateUserStore()
        {
            connection.UpdateIdentityUserSchema<int, TestUser>();
            var userStore = new UserStore<TestUser, int>(connection);
            return userStore;
        }
        
        [Table("bla")]
        public class TestUser : IdentityUser<TestUser, int>
        {
        }

        public class TestRole : IdentityRole<int>
        {
        }
    }
}
