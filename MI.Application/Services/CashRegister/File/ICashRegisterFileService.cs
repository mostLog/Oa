using MI.Application.Dto;

namespace MI.Application.File
{
    public interface ICashRegisterFileService
    {
        ExcelDto ExcelExport(CashRegisterPagedInputDto input);
    }
}
