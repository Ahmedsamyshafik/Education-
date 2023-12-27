using WEB.Models;

namespace WEB.IRepo
{
    public interface IpdfBLL
    {
        List<PdfMaterial> GetAllPDFs1();
        List<PdfMaterial> GetAllPDFs2();
        PdfMaterial GetPdfById(int id);

        List<PdfMaterial> GetAllPdfForOne1();
        List<PdfMaterial> GetAllPdfForTwo1();
        List<PdfMaterial> GetAllPdfForThree1();
        List<PdfMaterial> GetAllPdfForFour1();

        List<PdfMaterial> GetAllPdfForOne2();
        List<PdfMaterial> GetAllPdfForTwo2();
        List<PdfMaterial> GetAllPdfForThree2();
        List<PdfMaterial> GetAllPdfForFour2();

        void SavePdfWithCourses(PdfMaterial pdf, int Co_Id);
        void DeletePdf(int id);

        void SavingPdf(PdfMaterial pdfMaterial);
        void Save();
        void SaveAsync();
    }
}
