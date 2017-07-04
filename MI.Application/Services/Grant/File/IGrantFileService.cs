using MI.Application.Dto;

namespace MI.Application.File
{
    public interface IGrantFileService
    {
        ExcelDto ExcelExport(GrantPagedInputDto input);
    }
}
