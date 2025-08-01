using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using EFCored.Model;

namespace EFGetStarted.Demo
{
    public class Crud
    {
        private AppDbContext db;

        public Crud()
        {
            db = new AppDbContext();
        }

        // Execute a particular method from this class
        public async void DoTest()
        {
            await this.Query();

            /*
            await this.Insert();
            await this.Update();
            await this.Delete();
            */
        }

        public async Task<Blog> Query()
        {
            Console.WriteLine("Querying for a blog");
            var blog = await db.Blogs
                .OrderBy(b => b.BlogId)
                .FirstAsync();
            return blog;
        }

        public async void Insert()
        {
            Console.WriteLine("Inserting a new blog");
            db.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
            await db.SaveChangesAsync();
        }

        public async void Update()
        {
            var blog = await Query();

            Console.WriteLine("Updating the blog and adding a post");
            blog.Url = "https://devblogs.microsoft.com/dotnet";
            blog.Posts.Add(
                new Post
                {
                    Title = "Hello World",
                    Content = "I wrote an app using EF Core!",
                    Blog = blog // Fix: set required Blog property
                });
            await db.SaveChangesAsync();
        }

        public async void Delete()
        {
            var blog = await Query();

            // Delete
            Console.WriteLine("Delete the blog");
            db.Remove(blog);
            await db.SaveChangesAsync();
        }
    }
}



