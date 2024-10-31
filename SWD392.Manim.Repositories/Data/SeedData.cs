using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities;
using SWD392.Manim.Repositories.Entity;

namespace SWD392.Manim.Repositories
{
    public class SeedData
    {
        private readonly Swd392Context _context;
        public SeedData(Swd392Context context)
        {
            _context = context;
        }

        public void SeedingData()
        {
            try
            {
                if (_context.Database.IsSqlServer())
                {
                    if (_context.Database.IsSqlServer())
                    {
                        bool dbExists = _context.Database.CanConnect();
                        if (dbExists)
                        {
                            _context.Database.Migrate();
                        }
                        Seed();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                _context.Dispose();
            }
        }
        public void Seed()
        {
            int data = 0;
            data = _context.Roles.Count();
            if (data is 0)
            {
                ApplicationRole[] roles = CreateRoleRole();
                _context.AddRange(roles);
            }

            data = _context.Users.Count();
            if (data is 0)
            {
                ApplicationUser[] users = CreateUser();
                _context.AddRange(users);
            }
            _context.SaveChanges();
            AssignAdminRoleToUser("khanhnvn", "AdminSystem");
            AssignAdminRoleToUser("vudq", "User");
            AssignAdminRoleToUser("cuongtd", "User");
            AssignAdminRoleToUser("nghiatm", "User");
            AssignAdminRoleToUser("cuongtq", "User");
            AssignAdminRoleToUser("triethlm", "User");
            data = _context.Subjects.Count();
            if (data is 0)
            {
                Subject[] subjects = CreateSubject();
                _context.AddRange(subjects);
            }
            _context.SaveChanges();

            data = _context.Wallets.Count();
            if (data is 0)
            {
                Wallet[] wallets = CreateWallet();
                _context.AddRange(wallets);
            }
            _context.SaveChanges();

            data = _context.Parameters.Count();
            if (data is 0)
            {
                Parameter[] spring = CreateSpringParameter("c87d6b7f2a014f6b9193a3d28c819cd9");
                Parameter[] pendulum = CreatePendulumParameter("f8e4db932f474de39b3fcbb10f9f5b62");
                _context.AddRange(spring);
                _context.AddRange(pendulum);
            }
            _context.SaveChanges();
            data = _context.Problems.Count();
            if (data is 0)
            {
                Problem[] problems = CreateProblem();
                _context.AddRange(problems);
                _context.SaveChanges();
                AssignProblemParameter("d7c2a9e5f4b0a3f8d6c1e9a7b3f5d2c4", "e7a9c5d4f2b0a3c6f8d1e3b7c9a4f6b1", 0);
                AssignProblemParameter("d7c2a9e5f4b0a3f8d6c1e9a7b3f5d2c4", "4b8f3a2c1d7e5b9a6c0f4d2e3b1a8c7d", 0);
                AssignProblemParameter("1e6a3d5f8c2b4a9f0e3d7a5c6b9f1c8b", "b3f8a2d4e7c1b5a0d9f6c3a4e2b7d1f9", 0);
                AssignProblemParameter("1e6a3d5f8c2b4a9f0e3d7a5c6b9f1c8b", "e7a9c5d4f2b0a3c6f8d1e3b7c9a4f6b1", 0);
                AssignProblemParameter("e2b4a1f5d7c8a3e0f6b9c5a7d4b3f2e9", "1f6a3d8c4b9e7a5d2c0f3b4e9a6d7c5b", 0);
                AssignProblemParameter("e2b4a1f5d7c8a3e0f6b9c5a7d4b3f2e9", "e7a9c5d4f2b0a3c6f8d1e3b7c9a4f6b1", 0);
                AssignProblemParameter("e2b4a1f5d7c8a3e0f6b9c5a7d4b3f2e9", "4b8f3a2c1d7e5b9a6c0f4d2e3b1a8c7d", 0);
                AssignProblemParameter("8f3c5a7d9b1e4c6f2a9b0d3e7f5a8c1d", "e5f3b1a9d6c2a7f4b8d1c0e9a3f5d2b7", 0);
                AssignProblemParameter("8f3c5a7d9b1e4c6f2a9b0d3e7f5a8c1d", "b5e2a3f9c8d0f4a7b1c6e3d9a5b2f0c8", 0);
                AssignProblemParameter("a6d8c1f5e3b2a9f7d4c0e8a5f3b1d9c6", "7f9a6e3d1c5b2a8f0d3b4e1a9c7f2b5d", 0);
                AssignProblemParameter("a6d8c1f5e3b2a9f7d4c0e8a5f3b1d9c6", "e5f3b1a9d6c2a7f4b8d1c0e9a3f5d2b7", 0);
                _context.AddRange(problems);

            }
            _context.SaveChanges();    

            AssignUserToWallet("khanhnvn", "b7d5c4a28e9346fa9eab3c9d2f7804b6");
            AssignUserToWallet("vudq", "8a9b4c32e7f245e0b8a123d9f4c6b5e1");
            AssignUserToWallet("cuongtd", "e63f9b427bd94763a1e5d30bca7b4f82");
            AssignUserToWallet("nghiatm", "1b9a3c6f2f4d48f089ea3b728f416e92");
            AssignUserToWallet("cuongtq", "a12e5cbda3d441ff80fcb1d27c695e07");
            AssignUserToWallet("triethlm", "da5b74c41f92488cbe8d43c0b4a689d6");

            
            _context.SaveChanges();
        }

        private static ApplicationRole[] CreateRoleRole()
        {
            ApplicationRole[] roles =
              [
            new ApplicationRole
            {
                Name = "AdminSystem",
                NormalizedName = "ADMINSYSTEM",
                FullName ="Quản trị hệ thống",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            },
            new ApplicationRole
            {
                Name = "User",
                NormalizedName = "USER",
                FullName = "Người dùng",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            }
        ];
            return roles;
        }

        private static ApplicationUser[] CreateUser()
        {
            ApplicationUser[] users =
            [
                new ApplicationUser
            {
                UserName = "triethlm",
                FullName = "Hoàng Lê Minh Triết",
                PhoneNumber = "0999999999",
                Email = "triethmlse160210@fpt.edu.vn",
                Gender = 1,
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                PasswordHash = HashPasswordService.HashPasswordThrice("triethlm")
            },
            new ApplicationUser
            {
                UserName = "cuongtq",
                FullName = "Trần Quốc Cường",
                PhoneNumber = "0111111111",
                Email = "cuongtqse160059@fpt.edu.vn",
                Gender = 1,
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                PasswordHash = HashPasswordService.HashPasswordThrice("cuongtq")
            },
            new ApplicationUser
            {
                UserName = "nghiatm",
                FullName = "Trần Minh Nghĩa",
                PhoneNumber = "0777777777",
                Email = "nghiatmse160581@fpt.edu.vn",
                Gender = 1,
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                PasswordHash = HashPasswordService.HashPasswordThrice("nghiatm")
            },
            new ApplicationUser
            {
                UserName = "khanhnvn",
                FullName = "Nguyễn Viết Nam Khánh",
                PhoneNumber = "0919385156",
                Email = "khanhnvnse170092@fpt.edu.vn",
                Gender = 1,
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                PasswordHash = HashPasswordService.HashPasswordThrice("khanhnvn")
            },
            new ApplicationUser
            {
                UserName = "cuongtd",
                FullName = "Tạ Đức Cường",
                PhoneNumber = "0888888888",
                Email = "cuongtdse172527@fpt.edu.vn",
                Gender = 1,
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                PasswordHash = HashPasswordService.HashPasswordThrice("cuongtd")
            },
            new ApplicationUser
            {
                UserName = "vudq",
                FullName = "Đào Quang Vũ",
                Email = "vudqse160568@fpt.edu.vn",
                Gender = 1,
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                PasswordHash = HashPasswordService.HashPasswordThrice("vudq")
            }
            ];
            return users;
        }
        private static Subject[] CreateSubject()
        {
            Subject[] subjects =
               [
             new Subject
            {
                Id = "7f6c9bcd1e3d4a7d8d0f7c4e71f6fbea",
                Name = "Toán",
                CreatedAt = DateTime.Now,
                Status = true,
            },
            new Subject
            {
                Id = "b73d9a8b2f3e46979e94482c3b8e20e3",
                Name = "Vật lý 10",
                CreatedAt = DateTime.Now,
                Status = true,
            },
            new Subject
            {
                Id = "e29b7fa35ef24f72b48eb0a47dc9baf3",
                Name = "Vật lý 11",
                CreatedAt = DateTime.Now,
                Status = true,
            },
            new Subject
            {
                Id = "f0a8c2dd6b1a4f19b3a9675ba9168e8a",
                Name = "Vật lý 12",
                CreatedAt = DateTime.Now,
                Status = true,
                Chapters = CreateChapter("f0a8c2dd6b1a4f19b3a9675ba9168e8a")
            },
         ];
            return subjects;
        }
        private static Chapter[] CreateChapter(string subjectId)
        {
            Chapter[] chapters =
               [
             new Chapter
            {
                Id = "f32a1c8b4e8e43c18b2873b65f72b1d2",
                Name = "Chương dao động điều hòa",
                CreatedAt = DateTime.Now,
                Status = true,
                SubjectId = subjectId,
                Topics = CreateTopic("f32a1c8b4e8e43c18b2873b65f72b1d2")
            },
            new Chapter
            {
                Id = "e8a9f5b6c4f14b93b9b782e0d7c80bca",
                Name = "Dao động biến thiên",
                CreatedAt = DateTime.Now,
                Status = true,
                SubjectId = subjectId
            }
         ];
            return chapters;
        }
        private static Topic[] CreateTopic(string chapterId)
        {
            Topic[] topics =
               [
             new Topic
            {
                Id = "f8e4db932f474de39b3fcbb10f9f5b62",
                Name = "Con lắc đơn",
                CreatedAt = DateTime.Now,
                Status = true,
                ChapterId = chapterId,
            },
            new Topic
            {
                Id = "c87d6b7f2a014f6b9193a3d28c819cd9",
                Name = "Con lắc lò xo",
                CreatedAt = DateTime.Now,
                Status = true,
                ChapterId = chapterId,
            }
         ];
            return topics;
        }

        private static Wallet[] CreateWallet()
        {
            Wallet[] wallets =
               [
             new Wallet
            {
                Id = "da5b74c41f92488cbe8d43c0b4a689d6",
                CreatedAt = DateTime.Now,
                Balance = 100000,
            },
            new Wallet
            {
                Id = "a12e5cbda3d441ff80fcb1d27c695e07",
                CreatedAt = DateTime.Now,
                Balance = 100000,
            },
            new Wallet
            {
                Id = "1b9a3c6f2f4d48f089ea3b728f416e92",
                CreatedAt = DateTime.Now,
                Balance = 100000,
            },
            new Wallet
            {
                Id = "e63f9b427bd94763a1e5d30bca7b4f82",
                CreatedAt = DateTime.Now,
                Balance = 100000,
            },
            new Wallet
            {
                Id = "8a9b4c32e7f245e0b8a123d9f4c6b5e1",
                CreatedAt = DateTime.Now,
                Balance = 100000,
            },
            new Wallet
            {
                Id = "b7d5c4a28e9346fa9eab3c9d2f7804b6",
                CreatedAt = DateTime.Now,
                Balance = 100000,
            },
         ];
            return wallets;
        }
        private static Parameter[] CreatePendulumParameter(string topicId)
        {
            Parameter[] parameters =
            [
                new Parameter
                {
                    Id = "4b8f3a2c1d7e5b9a6c0f4d2e3b1a8c7d",
                    CreatedAt = DateTime.Now,
                    Symbol = "l",
                    Name = "Chiều dài",
                    Unit = "m",
                    TopicId = topicId
                },
                new Parameter
                {
                    Id = "e7a9c5d4f2b0a3c6f8d1e3b7c9a4f6b1",
                    CreatedAt = DateTime.Now,
                    Symbol = "g",
                    Name = "Gia tốc trọng trường",
                    Unit = "m/s2",
                    TopicId = topicId
                },
                new Parameter
                {
                    Id = "1f6a3d8c4b9e7a5d2c0f3b4e9a6d7c5b",
                    CreatedAt = DateTime.Now,
                    Symbol = "Ω",
                    Name = "Góc lệch ban đầu",
                    Unit = "°",
                    TopicId = topicId
                    },
                new Parameter
                {
                    Id = "b3f8a2d4e7c1b5a0d9f6c3a4e2b7d1f9",
                    CreatedAt = DateTime.Now,
                    Symbol = "T",
                    Name = "Chu kì giao động",
                    Unit = "s",
                    TopicId = topicId
                },
                new Parameter
                {
                    Id = "c9e1a6b3d7f4c5a8b0d2e9f7a3c4b5d6",
                    CreatedAt = DateTime.Now,
                    Symbol = "Vmax",
                    Name = "Vận tốc cực đại",
                    Unit = "m/s",
                    TopicId = topicId
                },
                //new Parameter
                //{
                //    Id = "6f7b4d3a2c9e8f1a0c5e3d9b7f4a1c8d",
                //    CreatedAt = DateTime.Now,
                //    Symbol = "E",
                //    Name = "Năng lượng cơ học",
                //    Unit = "J",
                //    TopicId = topicId
                //},
                //new Parameter
                //{
                //    Id = "b5e2a3f9c8d0f4a7b1c6e3d9a5b2f0c7",
                //    CreatedAt = DateTime.Now,
                //    Symbol = "m",
                //    Name = "Khối lượng",
                //    Unit = "kg",
                //    TopicId = topicId

                //},
                new Parameter
                {
                    Id = "8d2f4c5a9b3e7a1f6c3d0b8e4a2f1c9d",
                    CreatedAt = DateTime.Now,
                    Symbol = "π",
                    Name = "Pi",
                    TopicId = topicId
                }
            ];
            return parameters;
        }
        public static Problem[] CreateProblem()
        {
            Problem[] problems =
            [
            //con lac lo xo
                new Problem
                {
                    Id = "d7c2a9e5f4b0a3f8d6c1e9a7b3f5d2c4",
                    CreatedAt = DateTime.Now,
                    Description = "Tính chu kỳ dao động (T) của con lắc đơn khi biết chiều dài dây l và gia tốc trọng trường g",
                    Price = 5000,
                    Type = 1,
                    Name = "Tìm T",
                    TopicId = "f8e4db932f474de39b3fcbb10f9f5b62"
                },
                new Problem
                {
                    Id = "1e6a3d5f8c2b4a9f0e3d7a5c6b9f1c8b",
                    CreatedAt = DateTime.Now,
                    Description = "Tính độ dài (l) của con lắc khi biết chu kỳ dao động T và gia tốc trọng trường g",
                    Price = 5000,
                    Type = 2,
                    Name = "Tìm l",
                    TopicId = "f8e4db932f474de39b3fcbb10f9f5b62"

                },
                new Problem
                {
                    Id = "e2b4a1f5d7c8a3e0f6b9c5a7d4b3f2e9",
                    CreatedAt = DateTime.Now,
                    Description = "Tính vận tốc cực đại (Vmax) của con lắc tại vị trí thấp nhất (vị trí cân bằng), khi biết góc lệch ban đầu, chiều dài dây l và gia tốc trọng trường g",
                    Price = 5000,
                    Type = 3,
                    Name = "Tìm Vmax",
                    TopicId = "f8e4db932f474de39b3fcbb10f9f5b62"

                },
                new Problem
                {
                    Id = "8f3c5a7d9b1e4c6f2a9b0d3e7f5a8c1d",
                    CreatedAt = DateTime.Now,
                    Description = "Tìm chu kì dao động (T) của con lắc lò xo hoặc tần số dao động của con lắc lò xo khi biết độ cứng k của lò xo và khối lượng m của vật.",
                    Price = 5000,
                    Type = 4,
                    Name = "Tìm T",                    
                    TopicId = "c87d6b7f2a014f6b9193a3d28c819cd9"

                },
                new Problem
                {
                    Id = "a6d8c1f5e3b2a9f7d4c0e8a5f3b1d9c6",
                    CreatedAt = DateTime.Now,
                    Description = "Tính năng lượng toàn phần (E) của con lắc lò xo khi biết biên độ dao động A và độ cứng k của lò xo.",
                    Price = 5000,
                    Type = 5,
                    Name = "Tìm E",
                    TopicId = "c87d6b7f2a014f6b9193a3d28c819cd9"

                },
            ];
            return problems;
        }
        private static Parameter[] CreateSpringParameter(string topicId)
        {
            Parameter[] parameters =
            [
            //con lac lo xo
                new Parameter
                {
                    Id = "7f9a6e3d1c5b2a8f0d3b4e1a9c7f2b5d",
                    CreatedAt = DateTime.Now,
                    Symbol = "A",
                    Name = "Biên độ dao động",
                    Unit = "m",
                    TopicId = topicId
                },
                new Parameter
                {
                    Id = "b5e2a3f9c8d0f4a7b1c6e3d9a5b2f0c8",
                    CreatedAt = DateTime.Now,
                    Symbol = "m",
                    Name = "Khối lượng",
                    Unit = "kg",
                    TopicId = topicId

                },
                new Parameter
                {
                    Id = "b2c7f1a4e8d5c0a3b9f6e4a7c3d2b8f5",
                    CreatedAt = DateTime.Now,
                    Symbol = "E",
                    Name = "Năng lượng toàn phần",
                    Unit = "J",
                    TopicId = topicId
                },
                new Parameter
                {
                    Id = "e5f3b1a9d6c2a7f4b8d1c0e9a3f5d2b7",
                    CreatedAt = DateTime.Now,
                    Symbol = "k",
                    Name = "Độ cứng",
                    Unit = "N/m",
                    TopicId = topicId
                },
                new Parameter
                {
                    Id = "c4d8a3f1e7b0c9a5f2b6e3d4a1c9f7b8",
                    CreatedAt = DateTime.Now,
                    Symbol = "T",
                    Name = "Chu kì giao động",
                    Unit = "s",
                    TopicId = topicId
                },
                //new Parameter
                //{
                //    Id = "9f5a2b8c3d4e0a7f6c1d3e5a9b4f8c2d",
                //    CreatedAt = DateTime.Now,
                //    Symbol = "x",
                //    Name = "Vật cách vị trí cân bằng",
                //    Unit = "m",
                //    TopicId = topicId
                //},
            ];
            return parameters;
        }
        public void AssignProblemParameter(string problemId, string parameterId, double value)
        {
            
            ProblemParameter pp = new()
            {
                ParameterId = parameterId,
                ProblemId = problemId,
                Value = value,
                CreatedAt = DateTime.Now
            };
            _context.ProblemParameters.Add(pp);
            _context.SaveChanges();
        }
        private void AssignAdminRoleToUser(string username, string roleName)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == username);
            var role = _context.Roles.FirstOrDefault(r => r.Name == roleName);


            if (user != null && role != null)
            {
                if (!_context.UserRoles.Any(ur => ur.UserId == user.Id && ur.RoleId == role.Id))
                {
                    ApplicationUserRoles userRoles = new()
                    {
                        UserId = user.Id,
                        RoleId = role.Id
                    };
                    _context.UserRoles.Add(userRoles);
                    _context.SaveChanges();
                }
            }
        }
        private void AssignUserToWallet(string username, string walletId)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == username);
            var wallet = _context.Wallets.FirstOrDefault(r => r.Id == walletId);


            if (user != null && wallet != null)
            {
                user.Wallet = wallet;
                _context.Users.Update(user);
                _context.SaveChanges();
            }
        }
    }
}
