using MI.Application.OASession.Dto;

namespace MI.Application.OASession
{
    public interface ISession
    {
        OAUser GetCurrUser();
    }
}