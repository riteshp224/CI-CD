using Microsoft.Extensions.Options;
using Platinum.Model;

namespace Platinum.Service
{
    public abstract class BaseRepository
    {
        public readonly IOptions<DataConfig> _dataConfig;

        public BaseRepository(IOptions<DataConfig> dataConfig)
        {
            _dataConfig = dataConfig;
        }
    }
}
