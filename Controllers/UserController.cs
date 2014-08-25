using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using MyWebApp.Repositories.Interfaces;

namespace MyWebApp.Controllers
{
    public class UserController : Controller
    {
         private readonly IProblemRepository problemRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IAnswerRepository answerRepository;
        private readonly IUserSolvedRepository userSolvedRepository;
        private readonly IUserAttemptedRepository userAttemptedRepository;
        private readonly IImageRepository imageRepository;
        private readonly ITagRepository tagRepository;
        private readonly ILikeRepository likeRepository;
        private readonly IDislikeRepository dislikeRepository;
        private readonly ICommentRepository commentRepository;


        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        public UserController(IProblemRepository problemRepository, ICategoryRepository categoryRepository, IAnswerRepository answerRepository, IUserSolvedRepository userSolvedRepository, IUserAttemptedRepository userAttemptedRepository, IImageRepository imageRepository, ITagRepository tagRepository, ILikeRepository likeRepository, IDislikeRepository dislikeRepository, ICommentRepository commentRepository)
        {
            this.problemRepository = problemRepository;
            this.categoryRepository = categoryRepository;
            this.answerRepository = answerRepository;
            this.userAttemptedRepository = userAttemptedRepository;
            this.userSolvedRepository = userSolvedRepository;
            this.tagRepository = tagRepository;
            this.imageRepository = imageRepository;
            this.likeRepository = likeRepository;
            this.dislikeRepository = dislikeRepository;
            this.commentRepository = commentRepository;
        }
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Show(string id)
        {
            return View();
        }

    }
}