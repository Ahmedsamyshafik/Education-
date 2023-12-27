using WEB.Models;

namespace WEB.IRepo
{
    public interface IQuizBLL
    {
        int getCount();
        void SaveStudentPdfWithCourses(IFormFile form, ApplicationUser user, int QuizId, int CourseId);
        List<UsersQuiz> WrittenFilterationToCourses(List<UsersQuiz> usersQuizs);
        List<QuizTable> GetAllquizes();
        List<QuizTable> GetAllquizesForAdmin1();
        List<QuizTable> GetAllquizesForAdmin2();

        List<QuizTable> GetAllquizesForOne1();
        List<QuizTable> GetAllquizesForTwo1();
        List<QuizTable> GetAllquizesForThree1();
        List<QuizTable> GetAllquizesForFour1();

        List<QuizTable> GetAllquizesForOne2();
        List<QuizTable> GetAllquizesForTwo2();
        List<QuizTable> GetAllquizesForThree2();
        List<QuizTable> GetAllquizesForFour2();

        QuizTable GetQuizByIdwithQO(int id);
        QuizTable GetQuizByIdwithQ(int id);
        List<Option> GetAllOptions();
        List<Question> GetAllQuestions(int Qid);
        List<UsersQuiz> Showing1();
        List<UsersQuiz> Showing2();
        List<UsersQuiz> ShowCreditCoursesStudents();


        void DeleteFromQuizTable(int id);


        void AddingUE(string Userid, int ExamId, string userName, string ExamName, int CourseID,int ExamDegree, int MaxDegree, Semester semester);
        void AddingUE_ASYNC(string Userid, int ExamId, string userName, string ExamName, int ExamDegree, int MaxDegree, Semester semester);
        void DeleteRangeOdUserQuiz(List<UsersQuiz> quiz);


		void Add(QuizTable quiz);
        void AddPdf(QuizTable quiz);
        void SaveStudentPdf(IFormFile form, ApplicationUser user, int QuizId);
        List<UsersQuiz> WrittenFilteration(List<UsersQuiz> usersQuizs);
        QuizTable GetQuiz(int id);
        UsersQuiz GetQuizUser(int id);
        List<UsersQuiz> GetAllDataToUser(string UserID);
        void Delete(int id);
        //  void Delete2(string Exname);
        void Save();
        void CreatQuizWithCourse(QuizTable quiz, int C_Id);
		void AddQuizPdfWithCourse(QuizTable quiz, int CId);

	}
}
