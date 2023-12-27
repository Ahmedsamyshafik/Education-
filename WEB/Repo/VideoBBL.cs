using WEB.Data;
using WEB.IRepo;
using WEB.Models;

namespace WEB.Repo
{
    public class VideoBBL :IVideoBBL
    {
        private readonly ApplicationContext db;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly ICoursesNamesBLL cS;
        private readonly ICreditCourses oS;

        public VideoBBL(ApplicationContext db, IWebHostEnvironment hostingEnvironment, ICoursesNamesBLL CS
            , ICreditCourses OS)
        {
            this.db = db;
            this.hostingEnvironment = hostingEnvironment;
            cS = CS;
            oS = OS;
        }

        public List<AccessToVideo> GetAllVideos()
        {
            return db.AccessToVideo.ToList();
        }
        public AccessToVideo GetVideoById(int id) {
        return db.AccessToVideo.FirstOrDefault(x=>x.Id==id);
        }

        public void Delete(int id)
        {
           var vid= GetVideoById(id);
            if (vid !=null)
            {
                db.AccessToVideo.Remove(vid);
                db.SaveChanges();
            }
        }

        public List<AccessToVideo> GetAllVideosForAdmin1()
        {
            return db.AccessToVideo.Where(v => v.Semester == Semester.الأول).ToList();
        }

        public List<AccessToVideo> GetAllVideosForAdmin2()
        {
            return db.AccessToVideo.Where(v => v.Semester == Semester.الثاني).ToList();
        }

        public List<AccessToVideo> GetVideosForOne1()
        {
            var Videos = GetAllVideosForAdmin1();
            List<AccessToVideo> ActulyVideos = new List<AccessToVideo>();
            foreach (var vid in Videos)
            {
                if (vid.Degree == Alldegrees.الأول)
                {
                    ActulyVideos.Add(vid);
                }
            }
			return ActulyVideos.OrderByDescending(x => x.Date).ToList();
		}

		public List<AccessToVideo> GetVideosForTwo1()
        {
            var Videos = GetAllVideos();
            List<AccessToVideo> ActulyVideos = new List<AccessToVideo>();
            foreach (var vid in Videos)
            {
                if (vid.Degree.ToString() == "الثاني" && vid.Semester.ToString() == "الأول")
                {
                    ActulyVideos.Add(vid);
                }
            }
            return ActulyVideos.OrderByDescending(x=>x.Date).ToList();
        }


        public List<AccessToVideo> GetVideosForThree1()
        {
            var Videos = GetAllVideosForAdmin1();
            List<AccessToVideo> ActulyVideos = new List<AccessToVideo>();
            foreach (var vid in Videos)
            {
                if (vid.Degree.ToString() == "الثالث")
                {
                    ActulyVideos.Add(vid);
                }
            }
			return ActulyVideos.OrderByDescending(x => x.Date).ToList();
		}

		public List<AccessToVideo> GetVideosForFour1()
        {
            var Videos = GetAllVideosForAdmin1();
            List<AccessToVideo> ActulyVideos = new List<AccessToVideo>();
            foreach (var vid in Videos)
            {
                if (vid.Degree.ToString() == "آخري")
                {
                    ActulyVideos.Add(vid);
                }
            }
			return ActulyVideos.OrderByDescending(x => x.Date).ToList();
		}

		// Secound Term
		public List<AccessToVideo> GetVideosForOne2()
        {
            var Videos = GetAllVideosForAdmin2();
            List<AccessToVideo> ActulyVideos = new List<AccessToVideo>();
            foreach (var vid in Videos)
            {
                if (vid.Degree.ToString() == "الأول")
                {
                    ActulyVideos.Add(vid);
                }
            }
			return ActulyVideos.OrderByDescending(x => x.Date).ToList();
		}

		public List<AccessToVideo> GetVideosForTwo2()
        {
            var Videos = GetAllVideosForAdmin2();
            List<AccessToVideo> ActulyVideos = new List<AccessToVideo>();
            foreach (var vid in Videos)
            {
                if (vid.Degree.ToString() == "الثاني" )
                {
                    ActulyVideos.Add(vid);
                }
            }
			return ActulyVideos.OrderByDescending(x => x.Date).ToList();
		}


		public List<AccessToVideo> GetVideosForThree2()
        {
            var Videos = GetAllVideosForAdmin2();
            List<AccessToVideo> ActulyVideos = new List<AccessToVideo>();
            foreach (var vid in Videos)
            {
                if (vid.Degree.ToString() == "الثالث" )
                {
                    ActulyVideos.Add(vid);
                }
            }
			return ActulyVideos.OrderByDescending(x => x.Date).ToList();
		}

		public List<AccessToVideo> GetVideosForFour2()
        {
            var Videos = GetAllVideosForAdmin2();
            List<AccessToVideo> ActulyVideos = new List<AccessToVideo>();
            foreach (var vid in Videos)
            {
                if (vid.Degree.ToString() == "آخري" )
                {
                    ActulyVideos.Add(vid);
                }
            }
			return ActulyVideos.OrderByDescending(x => x.Date).ToList();
		}

		public void SaveAsync()
        { db.SaveChangesAsync(); }



        public void Save()
        {
            db.SaveChanges();
        }

        public void SavingVideo(AccessToVideo video)
        {

            var SaveMe = new AccessToVideo()
            {
                Access = video.Access,
                Degree = video.Degree,
                Semester = video.Semester,
                Description = video.Description,
                //	ImgName = uniqueFileName,//{Path.GetExtension(video.File.FileName)}
                Name = video.Name,
                Link = video.Link,
            };
            db.AccessToVideo.Add(SaveMe);
            Save();


            //string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            //         using (var fileStream = new FileStream(filePath, FileMode.Create))
            //         {
            //             await video.File.CopyToAsync(fileStream);
            //         }

        }

        public async void SavingVideoWithCourse(AccessToVideo video, int CourseId)
        {

            var SaveMe = new AccessToVideo()
            {
                Access = video.Access,
                Degree = video.Degree,
                Semester = video.Semester,
                Description = video.Description,
                Name = video.Name,
                Link = video.Link,
            };
            db.AccessToVideo.Add(SaveMe);
            Save();
            // Other Courseee
            var Course = oS.GetCourse(CourseId);
            //  cS.GetById(CourseId);

            CoursesNames coursesNames = new()
            {
                CourseId = CourseId,
                Name = Course.name,
                VidId = SaveMe.Id,
                PdfId = 0,
                QuizId = 0,
            };
            cS.Add(coursesNames);


        }


        public async void SavingVideoAsync(AccessToVideo video)
        {
            var SaveMe = new AccessToVideo()
            {
                Access = video.Access,
                Degree = video.Degree,
                Semester = video.Semester,
                Description = video.Description,
                //	ImgName = uniqueFileName,//{Path.GetExtension(video.File.FileName)}
                Name = video.Name,
                Link = video.Link,
            };
            await db.AccessToVideo.AddAsync(SaveMe);
            // await db.SaveChangesAsync();
        }


        public List<AccessToVideo> videosByIds(List<int> ids)
        {
            List<AccessToVideo> myvideos = new();

            foreach (var id in ids)
            {
                var V = GetVideoById(id);
                if (V != null)
                    myvideos.Add(V);
            }
            return myvideos;
        }


    }
}
