using WEB.Models;

namespace WEB.IRepo
{
    public interface IVideoBBL
    {
        List<AccessToVideo> GetAllVideos();
        List<AccessToVideo> GetAllVideosForAdmin1();
        List<AccessToVideo> GetAllVideosForAdmin2();
        List<AccessToVideo> GetVideosForOne1();
        List<AccessToVideo> GetVideosForTwo1();
        List<AccessToVideo> GetVideosForThree1();
        List<AccessToVideo> GetVideosForFour1();
        List<AccessToVideo> GetVideosForOne2();
        List<AccessToVideo> GetVideosForTwo2();
        List<AccessToVideo> GetVideosForThree2();
        List<AccessToVideo> GetVideosForFour2();

        void Delete(int id);



		AccessToVideo GetVideoById(int id);
        void SavingVideo(AccessToVideo video);
        void SavingVideoAsync(AccessToVideo video);
        void SavingVideoWithCourse(AccessToVideo video, int CourseId);
        List<AccessToVideo> videosByIds(List<int> ids);

        void Save();
        void SaveAsync();
    }
}
