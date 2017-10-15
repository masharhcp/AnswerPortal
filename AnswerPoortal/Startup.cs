using AnswerPoortal.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System.Linq;
using System.Web.Services.Description;


//async call to CentiliNotif
//auth CORS
//Questions
[assembly: OwinStartupAttribute(typeof(AnswerPoortal.Startup))]
namespace AnswerPoortal
{
    public partial class Startup
    {

        
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
            InitConfig();
        }



        public void ConfigureServices(ServiceCollection services)
        {
           // services.AddCors(options =>
            //{
              //  options.AddPolicy("AllowSpecificOrigin",
                //    builder => builder.WithOrigins("http://example.com"));
            //});
        }
        private void createRolesandUsers()
        {
            AnswerPortalEntities db = new AnswerPortalEntities();
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup iam creating first Admin Role and creating a default Admin User    
            if (!roleManager.RoleExists("Admin"))
            {

                // first we create Admin rool   
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                  

                var user = new ApplicationUser();
                user.UserName = "admin";
                user.Email = "admin@gmail.com";

                string userPWD = "adminM1.";

                var chkUser = UserManager.Create(user, userPWD);

                

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    //db.SetAdmin(user, userPWD);
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }
            }

            // creating Creating Professor role    
            if (!roleManager.RoleExists("Professor"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Professor";
                roleManager.Create(role);

            }

            // creating Creating Student role    
            if (!roleManager.RoleExists("Student"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Student";
                roleManager.Create(role);

            }

            // creating Creating Employee role    
            if (!roleManager.RoleExists("Unidentified"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Unidentified";
                roleManager.Create(role);

            }
        }

        private void InitConfig()
        {
            AnswerPortalEntities db = new AnswerPortalEntities();
            var L2EQuery = db.Configs;
            var conf = L2EQuery.FirstOrDefault<Models.Config>();
            if (conf == null)
            {
                conf = new Config();
                conf.answerPriceK = 0;
                conf.evaluationPriceE = 0;
                conf.goldPriceG = 0;
                conf.platinumPriceP = 0;
                conf.silverPriceS = 0;
                conf.unlockPriceM = 0;
                db.Configs.Add(conf);
                db.SaveChanges();
            }
        }
    }
    
}
