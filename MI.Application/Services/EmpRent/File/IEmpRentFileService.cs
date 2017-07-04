using MI.Application.Dto;

namespace MI.Application.File
{
    public interface IEmpRentFileService
    {
        ExcelDto ExcelExport(EmpRentPagedInputDto input);
    }
}
