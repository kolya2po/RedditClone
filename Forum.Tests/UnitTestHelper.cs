using System;
using System.Collections.Generic;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Forum.Tests
{
    public static class UnitTestHelper
    {
        public static DbContextOptions<ForumDbContext> GetDbContextOptions()
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase("TestDb")
                .EnableSensitiveDataLogging()
                .Options;

            using var context = new ForumDbContext(options);
            SeedData(context);

            return options;
        }

        private static void SeedData(ForumDbContext context)
        {
            context.Comments.AddRange(GetTestComments());
            context.Communities.AddRange(GetTestCommunities());
            context.UserCommunities.AddRange(GetTestUserCommunities());
            context.Rules.AddRange(GetTestRules());
            context.Topics.AddRange(GetTestTopics());
            context.Users.AddRange(GetTestUsers());
            context.SaveChanges();
        }


        public static IEnumerable<Comment> GetTestComments() =>
            new[]
            {
                new Comment
                {
                    Id = 1,
                    Rating = 1,
                    Text = "Comment1 Text",
                    UserId = 1,
                    TopicId = 1
                },
                new Comment
                {
                    Id = 2,
                    Rating = 22,
                    Text = "Comment2 Text",
                    UserId = 2,
                    TopicId = 1
                },
                new Comment
                {
                    Id = 3,
                    Rating = -5,
                    Text = "Comment3 Text",
                    UserId = 3,
                    TopicId = 1
                },
                new Comment
                {
                    Id = 4,
                    Rating = 4,
                    Text = "Comment4 Text",
                    UserId = 4,
                    TopicId = 1
                }
            };

        public static IEnumerable<Community> GetTestCommunities() =>
            new[]
            {
                new Community
                {
                    Id = 1,
                    Title = "Community 1",
                    About = "About community 1",
                    CreationDate = new DateTime(2022, 1, 16)
                },
                new Community
                {
                    Id = 2,
                    Title = "Community 2",
                    About = "About community 2",
                    CreationDate = new DateTime(2022, 1, 18)
                },
                new Community
                {
                    Id = 3,
                    Title = "Community 3",
                    About = "About community 3",
                    CreationDate = new DateTime(2022, 3, 16)
                }
            };

        public static IEnumerable<Rule> GetTestRules() =>
            new[]
            {
                new Rule()
                {
                    Id = 1,
                    CommunityId = 1,
                    Title = "Rule 1",
                    RuleText = "Never break rule 1"
                },
                new Rule()
                {
                    Id = 2,
                    CommunityId = 1,
                    Title = "Rule 2",
                    RuleText = "Never break rule 2"
                },
                new Rule()
                {
                    Id = 3,
                    CommunityId = 1,
                    Title = "Rule 3",
                    RuleText = "Never break rule 3"
                },
                new Rule()
                {
                    Id = 4,
                    CommunityId = 1,
                    Title = "Rule 4",
                    RuleText = "Never break rule 4"
                }
            };

        public static IEnumerable<Topic> GetTestTopics() =>
            new[]
            {
                new Topic
                {
                    Id = 1,
                    UserId = 1,
                    CommunityId = 1,
                    Title = "Topic 1",
                    Text = "Text topic 1",
                    PostingDate = new DateTime(2022, 1, 22)
                },
                new Topic
                {
                    Id = 2,
                    UserId = 2,
                    CommunityId = 1,
                    Title = "Topic 2",
                    Text = "Text topic 2",
                    PostingDate = new DateTime(2022, 6, 4)
                },
                new Topic
                {
                    Id = 3,
                    UserId = 3,
                    CommunityId = 2,
                    Title = "Topic 3",
                    Text = "Text topic 3",
                    PostingDate = new DateTime(2022, 2, 16)
                },
                new Topic
                {
                    Id = 4,
                    UserId = 4,
                    CommunityId = 2,
                    Title = "Topic 4",
                    Text = "Text topic 4",
                    PostingDate = new DateTime(2021, 1, 16)
                }
            };

        public static IEnumerable<User> GetTestUsers() =>
            new[]
            {
                new User
                {
                    Id = 1,
                    UserName = "Name 1",
                    ModeratedCommunityId = 1
                },
                new User
                {
                    Id = 2,
                    UserName = "Name 2"
                },
                new User
                {
                    Id = 3,
                    UserName = "Name 3",
                    ModeratedCommunityId = 2
                },
                new User
                {
                    Id = 4,
                    UserName = "Name 4"
                }
            };

        public static IEnumerable<UserCommunity> GetTestUserCommunities() =>
            new[]
            {
                new UserCommunity
                {
                    CommunityId = 1,
                    UserId = 1
                },
                new UserCommunity
                {
                    CommunityId = 1,
                    UserId = 2
                },
                new UserCommunity
                {
                    CommunityId = 2,
                    UserId = 3
                },
                new UserCommunity
                {
                    CommunityId = 2,
                    UserId = 4
                }
            };
    }
}
