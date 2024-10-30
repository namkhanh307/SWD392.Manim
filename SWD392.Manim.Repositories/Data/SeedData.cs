using Microsoft.EntityFrameworkCore;
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
            data = _context.Subjects.Count();
            if (data is 0)
            {
                Subject[] subjects = CreateSubject();
                _context.AddRange(subjects);
            }
            data = _context.Wallets.Count();
            if (data is 0)
            {
                Wallet[] wallets = CreateWallet();
                _context.AddRange(wallets);
            }
            _context.SaveChanges();

            AssignAdminRoleToUser("khanhnvn", "AdminSystem");
            AssignAdminRoleToUser("vudq", "User");
            AssignAdminRoleToUser("cuongtd", "User");
            AssignAdminRoleToUser("nghiatm", "User");
            AssignAdminRoleToUser("cuongtq", "User");
            AssignAdminRoleToUser("triethlm", "User");

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
                Parameters = CreatePendulumParameter("f8e4db932f474de39b3fcbb10f9f5b62")
            },
            new Topic
            {
                Id = "c87d6b7f2a014f6b9193a3d28c819cd9",
                Name = "Con lắc lò xo",
                CreatedAt = DateTime.Now,
                Status = true,
                ChapterId = chapterId,
                Parameters = CreateSpringParameter("c87d6b7f2a014f6b9193a3d28c819cd9")
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
                    Id = "f98c1b2a3d0145c78e5d6f3b9c4e7a1b",
                    CreatedAt = DateTime.Now,
                    Symbol = "l",
                    Name = "Chiều dài",
                    Unit = "m",
                    TopicId = topicId
                },
                new Parameter
                {
                    Id = "b2e7c4f5d8a643f89c7b2d1e4a5f6b3c",
                    CreatedAt = DateTime.Now,
                    Symbol = "g",
                    Name = "Gia tốc trọng trường",
                    Unit = "m/s2",
                    TopicId = topicId
                },
                new Parameter
                {
                    Id = "a9d7c6b5f4e123f8b7d2c5a6e9b3c0d4",
                    CreatedAt = DateTime.Now,
                    Symbol = "Ω",
                    Name = "Góc lệch ban đầu",
                    Unit = "°",
                    TopicId = topicId

                    },
                new Parameter
                {
                    Id = "e4f7b8c9d0a1234b5c6d7a9e2f8b3c1d",
                    CreatedAt = DateTime.Now,
                    Symbol = "T",
                    Name = "Chu kì giao động",
                    Unit = "s",
                    TopicId = topicId

                },
                new Parameter
                {
                    Id = "c3b7d5f9e6a842d7b8a1e5f0c6d3b2a9",
                    CreatedAt = DateTime.Now,
                    Symbol = "vmax",
                    Name = "Vận tốc cực đại",
                    Unit = "m/s",
                    TopicId = topicId
                },
                new Parameter
                {
                    Id = "2a6f5c3b1e9d7b0a4f8c6e3d5b1c2a9d",
                    CreatedAt = DateTime.Now,
                    Symbol = "E",
                    Name = "Năng lượng cơ học",
                    Unit = "J",
                    TopicId = topicId
                },
                new Parameter
                {
                    Id = "3b9f4c1e5a7d6b8f0c3a2d9e6b1f7d4a",
                    CreatedAt = DateTime.Now,
                    Symbol = "m",
                    Name = "Khối lượng",
                    Unit = "kg",
                    TopicId = topicId

                },
                new Parameter
                {
                    Id = "e5a6f3b8c7d4b9a2f0d3e1c5a8b7f6c1",
                    CreatedAt = DateTime.Now,
                    Symbol = "π",
                    Name = "Pi",
                    TopicId = topicId
                }
            ];
            return parameters;

        }
        private static Parameter[] CreateSpringParameter(string topicId)
        {
            Parameter[] parameters =
            [
            //con lac lo xo
                new Parameter
                {
                    Id = "d1b4c9f5a2e7b3c6d8a1e9f0c7b3f4a5",
                    CreatedAt = DateTime.Now,
                    Symbol = "A",
                    Name = "Biên độ dao động",
                    Unit = "m",
                    TopicId = topicId
                },
                new Parameter
                {
                    Id = "a8d5c6f1b4e3a9c7f2d0b5e8a3c4f9b2",
                    CreatedAt = DateTime.Now,
                    Symbol = "A",
                    Name = "Năng lượng toàn phần",
                    Unit = "J",
                    TopicId = topicId
                },
                new Parameter
                {
                    Id = "7f2a4c8b5d9e6a3b1c7d0f4b8e2a1f5c",
                    CreatedAt = DateTime.Now,
                    Symbol = "k",
                    Name = "Độ cứng",
                    Unit = "N/m",
                    TopicId = topicId
                }, 
                new Parameter
                {
                    Id = "e4f7b8c9d0a1234b5c6d7a9e2f8b3c1d",
                    CreatedAt = DateTime.Now,
                    Symbol = "T",
                    Name = "Chu kì giao động",
                    Unit = "s",
                    TopicId = topicId
                },
                new Parameter
                {
                    Id = "c9e3f1b7d4a2b8c5f0d7e6b3a1c8f5d2",
                    CreatedAt = DateTime.Now,
                    Symbol = "x",
                    Name = "Vật cách vị trí cân bằng",
                    Unit = "m",
                    TopicId = topicId
                },
            ];
            return parameters;
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
