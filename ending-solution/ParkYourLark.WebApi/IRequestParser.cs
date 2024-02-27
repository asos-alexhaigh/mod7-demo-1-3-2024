using ParkYourLark.WebApi.Data;

namespace ParkYourLark.WebApi;

public interface IRequestParser
{
    LevelSpace Parse(dynamic value);
}